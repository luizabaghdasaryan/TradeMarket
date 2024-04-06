using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class StatisticService : IStatisticService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public StatisticService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ProductModel>> GetCustomersMostPopularProductsAsync(int productCount, int customerId)
        {
            var receipts = await unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
            var customerReceiptsDetails = receipts
                .Where(r => r.CustomerId == customerId)
                .Select(r => r.ReceiptDetails)
                .SelectMany(customerReceiptsDetails => customerReceiptsDetails);
            var groupedResult = customerReceiptsDetails
                .GroupBy(rd => rd.ProductId)
                .Select(group => new
                {
                    group.First().Product,
                    Quantity = group.Sum(item => item.Quantity)
                });

            var popularProducts = groupedResult
                .OrderByDescending(g => g.Quantity)
                .Select(g => g.Product)
                .Take(productCount);
            var popularProductModels = mapper.Map<IEnumerable<ProductModel>>(popularProducts);

            return popularProductModels;
        }

        public async Task<decimal> GetIncomeOfCategoryInPeriod(int categoryId, DateTime startDate, DateTime endDate)
        {
            var receipts = await unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
            var receiptsInPeriod = receipts
                .Where(r => r.OperationDate >= startDate && r.OperationDate <= endDate);
            var receiptDetailsOfCategoryInPeriod = receiptsInPeriod
                .SelectMany(r => r.ReceiptDetails)
                .Where(rd => rd.Product.ProductCategoryId == categoryId);
            decimal income = 0;
            foreach (var detail in receiptDetailsOfCategoryInPeriod)
            {
                income += detail.DiscountUnitPrice * detail.Quantity;
            }

            return income;
        }

        public async Task<IEnumerable<ProductModel>> GetMostPopularProductsAsync(int productCount)
        {
            var receiptDetails = await unitOfWork.ReceiptDetailRepository.GetAllWithDetailsAsync();
            var groupedResult = receiptDetails
                .GroupBy(rd => rd.ProductId)
                .Select(group => new
                {
                    group.First().Product,
                    Quantity = group.Sum(item => item.Quantity)
                });

            var popularProducts = groupedResult
                .OrderByDescending(g => g.Quantity)
                .Select(g => g.Product)
                .Take(productCount);
            var popularProductModels = mapper.Map<IEnumerable<ProductModel>>(popularProducts);

            return popularProductModels;
        }

        public async Task<IEnumerable<CustomerActivityModel>> GetMostValuableCustomersAsync(int customerCount, DateTime startDate, DateTime endDate)
        {
            var receipts = await unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
            var receiptsInPeriod = receipts
                .Where(r => r.OperationDate >= startDate && r.OperationDate <= endDate);
            var groupedResult = receiptsInPeriod
                .GroupBy(r => r.CustomerId)
                .Select(group => new CustomerActivityModel
                    {
                        CustomerId = group.Key,
                        CustomerName = string.Join(' ', group.First().Customer.Person.Name, group.First().Customer.Person.Surname),
                        ReceiptSum = group.Sum(item => item.ReceiptDetails
                        .Select(rd => rd.DiscountUnitPrice * rd.Quantity)
                        .Sum())
                    }
                );

            var mostValuableCustomers = groupedResult
                .OrderByDescending(g => g.ReceiptSum)
                .Take(customerCount);

            return mostValuableCustomers;
        }
    }
}
