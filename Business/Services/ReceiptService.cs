using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation.Exceptions.BadRequestExceptions;
using Business.Validation.Exceptions.NotFoundExceptions;
using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ReceiptService : IReceiptService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ReceiptService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ReceiptModel>> GetAllAsync()
        {
            var receipts = await unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
            var receiptModels = mapper.Map<IEnumerable<ReceiptModel>>(receipts);

            return receiptModels;
        }

        public async Task<ReceiptModel> GetByIdAsync(int id)
        {
            var receipt = await unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(id) ?? throw new ReceiptNotFoundException(id);
            var receiptModel = mapper.Map<ReceiptModel>(receipt);

            return receiptModel;
        }

        public async Task AddAsync(ReceiptModel model)
        {
            var receipt = mapper.Map<Receipt>(model);
            await unitOfWork.ReceiptRepository.AddAsync(receipt);
            await unitOfWork.SaveAsync();
            model.Id = receipt.Id;
        }

        public async Task UpdateAsync(ReceiptModel model)
        {
            var receipt = await unitOfWork.ReceiptRepository.GetByIdAsync(model.Id) ?? throw new ReceiptNotFoundException(model.Id);
            mapper.Map(model, receipt);
            unitOfWork.ReceiptRepository.Update(receipt);
            await unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            var receipt = await unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(modelId) ?? throw new ReceiptNotFoundException(modelId);
            var receiptDetails = receipt.ReceiptDetails;
            foreach (var detail in receiptDetails)
            {
                unitOfWork.ReceiptDetailRepository.Delete(detail);
            }

            await unitOfWork.ReceiptRepository.DeleteByIdAsync(modelId);
            await unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<ReceiptDetailModel>> GetReceiptDetailsAsync(int receiptId)
        {
            var receipt = await unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId) ?? throw new ReceiptNotFoundException(receiptId);
            var receiptDetailModels = mapper.Map<IEnumerable<ReceiptDetailModel>>(receipt.ReceiptDetails);

            return receiptDetailModels;
        }

        public async Task<IEnumerable<ReceiptModel>> GetReceiptsByPeriodAsync(DateTime startDate, DateTime endDate)
        {
            var receipts = await unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
            var receiptsByPeriod = receipts.Where(r => r.OperationDate >= startDate && r.OperationDate <= endDate);
            var receiptModelsByPeriod = mapper.Map<IEnumerable<ReceiptModel>>(receiptsByPeriod);

            return receiptModelsByPeriod;
        }

        public async Task AddProductAsync(int productId, int receiptId, int quantity)
        {
            if (quantity <= 0)
            {
                throw new QuantityParameterBadRequestException();
            }

            var receipt = await unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId) ?? throw new ReceiptNotFoundException(receiptId);
            var receiptDetail = receipt.ReceiptDetails?.SingleOrDefault(rd => rd.ProductId == productId);
            if (receiptDetail is null)
            {
                var product = await unitOfWork.ProductRepository.GetByIdAsync(productId) ?? throw new ProductNotFoundException(productId);
                var newReceiptDetail = new ReceiptDetail(productId, receiptId, quantity)
                {
                    UnitPrice = product.Price,
                    DiscountUnitPrice = product.Price - (product.Price * receipt.Customer.DiscountValue / 100)
                };
                await unitOfWork.ReceiptDetailRepository.AddAsync(newReceiptDetail);
            }
            else
            {
                receiptDetail.Quantity += quantity;
            }

            await unitOfWork.SaveAsync();
        }

        public async Task CheckOutAsync(int receiptId)
        {
            var receipt = await unitOfWork.ReceiptRepository.GetByIdAsync(receiptId) ?? throw new ReceiptNotFoundException(receiptId);
            receipt.IsCheckedOut = true;
            await unitOfWork.SaveAsync();
        }

        public async Task RemoveProductAsync(int productId, int receiptId, int quantity)
        {
            var receipt = await unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId) ?? throw new ReceiptNotFoundException(receiptId);
            var receiptDetail = receipt.ReceiptDetails?.SingleOrDefault(rd => rd.ProductId == productId) ?? throw new ProductNotFoundException(productId);
            if (receiptDetail.Quantity == quantity)
            {
                unitOfWork.ReceiptDetailRepository.Delete(receiptDetail);
            }
            else if (receiptDetail.Quantity < quantity)
            {
                throw new QuantityParameterBadRequestException();
            }
            else
            {
                receiptDetail.Quantity -= quantity;
            }

            await unitOfWork.SaveAsync();   
        }

        public async Task<decimal> ToPayAsync(int receiptId)
        {
            var receipt = await unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId) ?? throw new ReceiptNotFoundException(receiptId);
            var receiptDetails = receipt.ReceiptDetails;
            decimal sum = 0;
            foreach(var detail in receiptDetails)
            {
                sum += detail.DiscountUnitPrice * detail.Quantity;
            }

            return sum;
        }
    }
}