using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductModel>>> Get([FromQuery] FilterSearchModel filter)
        {
            IEnumerable<ProductModel> products;
            if (filter is null)
            {
                products = await _productService.GetAllAsync();
            }
            else
            {
                products = await _productService.GetByFilterAsync(filter);
            }

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductModel>> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProductModel product)
        {
            await _productService.AddAsync(product);

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] ProductModel product)
        {
            product.Id = id;
            await _productService.UpdateAsync(product);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _productService.DeleteAsync(id);

            return NoContent();
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<ProductCategoryModel>>> GetCategories()
        {
            var categories = await _productService.GetAllProductCategoriesAsync();

            return Ok(categories);
        }

        [HttpPost("categories")]
        public async Task<ActionResult> PostCategory([FromBody] ProductCategoryModel category)
        {
            await _productService.AddCategoryAsync(category);

            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }

        [HttpPut("categories/{id:int}")]
        public async Task<ActionResult> PutCategory(int id, [FromBody] ProductCategoryModel category)
        {
            category.Id = id;
            await _productService.UpdateCategoryAsync(category);

            return NoContent();
        }

        [HttpDelete("categories/{id:int}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            await _productService.RemoveCategoryAsync(id);

            return NoContent();
        }
    }
}
