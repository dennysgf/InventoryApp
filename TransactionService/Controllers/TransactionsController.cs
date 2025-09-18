using Microsoft.AspNetCore.Mvc;
using TransactionService.Models;
using TransactionService.Models.DTOs;
using TransactionService.Services;

namespace TransactionService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        
        private readonly ITransactionService _service;

        public TransactionsController(ITransactionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetAll()
        {
            var transactions = await _service.GetAllAsync();

            var result = transactions.Select(t => new TransactionDto
            {
                Id = t.Id,
                Date = t.Date,
                Type = t.Type,
                ProductId = t.ProductId,
                Quantity = t.Quantity,
                TotalPrice = t.TotalPrice
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDto>> GetById(int id)
        {
            var transaction = await _service.GetByIdAsync(id);
            if (transaction == null) return NotFound();

            var result = new TransactionDto
            {
                Id = transaction.Id,
                Date = transaction.Date,
                Type = transaction.Type,
                ProductId = transaction.ProductId,
                Quantity = transaction.Quantity,
                TotalPrice = transaction.TotalPrice
            };

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<TransactionDto>> Create(CreateTransactionDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var transaction = new Transaction
            {
                Date = DateTime.UtcNow,
                Type = dto.Type,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                UnitPrice = dto.UnitPrice,
                TotalPrice = dto.UnitPrice * dto.Quantity,
                Detail = dto.Detail
            };

            var created = await _service.CreateAsync(transaction);

            var result = new TransactionDto
            {
                Id = created.Id,
                Date = created.Date,
                Type = created.Type,
                ProductId = created.ProductId,
                Quantity = created.Quantity,
                TotalPrice = created.TotalPrice
            };

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TransactionDto>> Update(int id, UpdateTransactionDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var transaction = new Transaction
            {
                Date = DateTime.UtcNow,
                Type = dto.Type,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                UnitPrice = dto.UnitPrice,
                TotalPrice = dto.UnitPrice * dto.Quantity,
                Detail = dto.Detail
            };

            var updated = await _service.UpdateAsync(id, transaction);
            if (updated == null) return NotFound();

            var result = new TransactionDto
            {
                Id = updated.Id,
                Date = updated.Date,
                Type = updated.Type,
                ProductId = updated.ProductId,
                Quantity = updated.Quantity,
                TotalPrice = updated.TotalPrice
            };

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}

