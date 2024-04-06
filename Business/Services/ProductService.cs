using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Business.Validation.Exceptions.BadRequestExceptions;
using Business.Validation.Exceptions.NotFoundExceptions;
using Data.Entities;
using Data.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ProductModel>> GetAllAsync()
        {
            var products = await unitOfWork.ProductRepository.GetAllWithDetailsAsync();
            var productModels = mapper.Map<IEnumerable<ProductModel>>(products);

            return productModels;
        }

        public async Task<IEnumerable<ProductModel>> GetByFilterAsync(FilterSearchModel filterSearch)
        {
            var products = await unitOfWork.ProductRepository.GetAllWithDetailsAsync();
            if (filterSearch.CategoryId is not null)
            {
                products = products.Where(p => p.Category.Id == filterSearch.CategoryId);
            }

            if (filterSearch.MinPrice is not null)
            {
                products = products.Where(p => p.Price >= filterSearch.MinPrice);
            }

            if (filterSearch.MaxPrice is not null)
            {
                products = products.Where(p => p.Price <= filterSearch.MaxPrice);
            }

            var productModels = mapper.Map<IEnumerable<ProductModel>>(products);

            return productModels;
        }

        public async Task<ProductModel> GetByIdAsync(int id)
        {
            var product = await unitOfWork.ProductRepository.GetByIdWithDetailsAsync(id) ?? throw new ProductNotFoundException(id);
            var productModel = mapper.Map<ProductModel>(product);

            return productModel;
        }

        public async Task AddAsync(ProductModel model)
        {
            ValidateProductModel(model);
            var product = mapper.Map<Product>(model);
            await unitOfWork.ProductRepository.AddAsync(product);
            await unitOfWork.SaveAsync();
            model.Id = product.Id;
        }

        public async Task UpdateAsync(ProductModel model)
        {
            ValidateProductModel(model, validateId: true);
            var product = await unitOfWork.ProductRepository.GetByIdAsync(model.Id) ?? throw new ProductNotFoundException(model.Id);
            mapper.Map(model, product);
            unitOfWork.ProductRepository.Update(product);
            await unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            _ = await unitOfWork.ProductRepository.GetByIdAsync(modelId) ?? throw new ProductNotFoundException(modelId);
            await unitOfWork.ProductRepository.DeleteByIdAsync(modelId);
            await unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<ProductCategoryModel>> GetAllProductCategoriesAsync()
        {
            var productCategories = await unitOfWork.ProductCategoryRepository.GetAllAsync();
            var productCategoryModels = mapper.Map<IEnumerable<ProductCategoryModel>>(productCategories);

            return productCategoryModels;
        }

        public async Task AddCategoryAsync(ProductCategoryModel categoryModel)
        {
            ValidateCategoryModel(categoryModel);
            var productCategory = mapper.Map<ProductCategory>(categoryModel);
            await unitOfWork.ProductCategoryRepository.AddAsync(productCategory);
            await unitOfWork.SaveAsync();
            categoryModel.Id = productCategory.Id;
        }

        public async Task UpdateCategoryAsync(ProductCategoryModel categoryModel)
        {
            ValidateCategoryModel(categoryModel, validateId: true);
            var category = await unitOfWork.ProductCategoryRepository.GetByIdAsync(categoryModel.Id) ?? throw new CategoryNotFoundException(categoryModel.Id);
            mapper.Map(categoryModel, category);

            unitOfWork.ProductCategoryRepository.Update(category);
            await unitOfWork.SaveAsync();
        }

        public async Task RemoveCategoryAsync(int categoryId)
        {
            _ = await unitOfWork.ProductCategoryRepository.GetByIdAsync(categoryId) ?? throw new CategoryNotFoundException(categoryId);
            await unitOfWork.ProductCategoryRepository.DeleteByIdAsync(categoryId);
            await unitOfWork.SaveAsync();
        }

        private static void ValidateProductModel(ProductModel model, bool validateId = false)
        {
            ValidationHelper.Validate(model, validateId);

            if (string.IsNullOrEmpty(model.ProductName))
            {
                throw new ProductModelBadRequestException("Product name cannot be null or empty");
            }

            if (model.Price < 0)
            {
                throw new ProductModelBadRequestException("Product price cannot be negative");
            }
        }

        private static void ValidateCategoryModel(ProductCategoryModel model, bool validateId = false)
        {
            ValidationHelper.Validate(model, validateId);

            if (string.IsNullOrEmpty(model.CategoryName))
            {
                throw new CategoryModelBadRequestException("Category name cannot be null or empty");
            }
        }
    }
}
