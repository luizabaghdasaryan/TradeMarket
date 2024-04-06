using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
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
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<CustomerModel>> GetAllAsync()
        {
            var customers = await unitOfWork.CustomerRepository.GetAllWithDetailsAsync();
            var customerModels = mapper.Map<IEnumerable<CustomerModel>>(customers);

            return customerModels;
        }

        public async Task<CustomerModel> GetByIdAsync(int id)
        {
            var customer = await unitOfWork.CustomerRepository.GetByIdWithDetailsAsync(id) ?? throw new CustomerNotFoundException(id);
            var customerModel = mapper.Map<CustomerModel>(customer);

            return customerModel;
        }

        public async Task AddAsync(CustomerModel model)
        {
            ValidateCustomerModel(model);
            var customer = mapper.Map<Customer>(model);
            await unitOfWork.CustomerRepository.AddAsync(customer);
            await unitOfWork.SaveAsync();
            model.Id = customer.Id;
        }

        public async Task UpdateAsync(CustomerModel model)
        {
            ValidateCustomerModel(model, validateId: true);
            var customer = await unitOfWork.CustomerRepository.GetByIdAsync(model.Id) ?? throw new CustomerNotFoundException(model.Id);

            mapper.Map(model, customer);
            customer.Person = mapper.Map<Person>(model);
            unitOfWork.PersonRepository.Update(customer.Person);
            unitOfWork.CustomerRepository.Update(customer);
            await unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            if (await unitOfWork.CustomerRepository.GetByIdAsync(modelId) is null) 
                throw new CustomerNotFoundException(modelId);
            await unitOfWork.CustomerRepository.DeleteByIdAsync(modelId);
            await unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<CustomerModel>> GetCustomersByProductIdAsync(int productId)
        {
            var customersWithDetails = await unitOfWork.CustomerRepository.GetAllWithDetailsAsync();
            var customersByProductId = customersWithDetails
                .Where(c => c.Receipts
                    .Any(r => r.ReceiptDetails
                        .Any(r => r.ProductId == productId)));

            var customerModelsByProductId = mapper.Map<IEnumerable<CustomerModel>>(customersByProductId);

            return customerModelsByProductId;
        }

        private static void ValidateCustomerModel(CustomerModel model, bool validateId = false)
        {
            ValidationHelper.Validate(model, validateId);

            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Surname))
            {
                throw new CustomerModelBadRequestException("Name or Surname cannot be null or empty");
            }

            if (model.BirthDate.Year >= DateTime.Today.AddYears(-18).Year || model.BirthDate.Year <= DateTime.Today.AddYears(-120).Year)
            {
                throw new CustomerModelBadRequestException("Invalid birthdate");
            }

            if (model.DiscountValue < 0)
            {
                throw new CustomerModelBadRequestException("Discount value cannot be negative");
            }
        }
    }
}