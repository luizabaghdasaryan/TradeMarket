using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticService _statisticService;

        public StatisticsController(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }

        [HttpGet("popularProducts")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetCustomersMostPopularProducts([FromQuery] int productCount)
        {
            var products = await _statisticService.GetMostPopularProductsAsync(productCount);

            return Ok(products);
        }

        [HttpGet("customer/{id:int}/{productCount:int}")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetCustomersMostPopularProducts([FromRoute] int id, [FromRoute] int productCount)
        {
            var products = await _statisticService.GetCustomersMostPopularProductsAsync(productCount, id);

            return Ok(products);
        }

        [HttpGet("activity/{customerCount:int}")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetMostValuableCustomers([FromRoute] int customerCount, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var customerss = await _statisticService.GetMostValuableCustomersAsync(customerCount, startDate, endDate);

            return Ok(customerss);
        }

        [HttpGet("income/{categoryId:int}")]
        public async Task<ActionResult<IEnumerable<CustomerModel>>> GetIncomeOfCategoryInPeriod([FromRoute] int categoryId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var customers = await _statisticService.GetIncomeOfCategoryInPeriod(categoryId, startDate, endDate);

            return Ok(customers);
        }
    }
}