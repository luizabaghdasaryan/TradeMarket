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
    public class ReceiptsController : ControllerBase
    {
        private readonly IReceiptService _receiptService;

        public ReceiptsController(IReceiptService receiptService)
        {
            _receiptService = receiptService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReceiptModel>>> Get()
        {
            var receipts = await _receiptService.GetAllAsync();

            return Ok(receipts);
        }

        [HttpGet("period")]
        public async Task<ActionResult<IEnumerable<ReceiptModel>>> GetReceiptsByPeriod([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var receipts = await _receiptService.GetReceiptsByPeriodAsync(startDate, endDate);

            return Ok(receipts);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ReceiptModel>> GetById(int id)
        {
            var receipt = await _receiptService.GetByIdAsync(id);
            
            return Ok(receipt);
        }

        [HttpGet("{id:int}/details")]
        public async Task<ActionResult<ReceiptDetailModel>> GetReceiptDetails(int id)
        {
            var receipt = await _receiptService.GetReceiptDetailsAsync(id);

            return Ok(receipt);
        }

        [HttpGet("{id:int}/sum")]
        public async Task<ActionResult<CustomerModel>> GetSum(int id)
        {
            var sum = await _receiptService.ToPayAsync(id);

            return Ok(sum);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ReceiptModel receipt)
        {
            await _receiptService.AddAsync(receipt);

            return CreatedAtAction(nameof(GetById), new { id = receipt.Id }, receipt);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] ReceiptModel receipt)
        {
            receipt.Id = id;
            await _receiptService.UpdateAsync(receipt);

            return NoContent();
        }

        [HttpPut("{id:int}/products/add/{productId:int}/{quantity:int}")]
        public async Task<ActionResult> PutProduct(int id, int productId, int quantity)
        {
            await _receiptService.AddProductAsync(productId, id, quantity);

            return NoContent();
        }

        [HttpPut("{id:int}/products/remove/{productId:int}/{quantity:int}")]
        public async Task<ActionResult> RemoveProduct(int id, int productId, int quantity)
        {
            await _receiptService.RemoveProductAsync(productId, id, quantity);

            return NoContent();
        }

        [HttpPut("{id:int}/checkout")]
        public async Task<ActionResult> CheckOut(int id)
        {
            await _receiptService.CheckOutAsync(id);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _receiptService.DeleteAsync(id);

            return NoContent();
        }
    }
}