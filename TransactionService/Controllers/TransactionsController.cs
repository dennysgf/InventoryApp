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
            try
            {
                var transactions = await _service.GetAllAsync();
                if (!transactions.Any())
                    return NotFound("No existen transacciones registradas.");

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
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDto>> GetById(int id)
        {
            try
            {
                var transaction = await _service.GetByIdAsync(id);
                if (transaction == null)
                    return NotFound($"Transacción con ID {id} no encontrada.");

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
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<TransactionDto>> Create(CreateTransactionDto dto)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            try
            {
                var transaction = new Transaction
                {
                    Type = dto.Type,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    UnitPrice = dto.UnitPrice,
                    Detail = dto.Detail
                    // Date y TotalPrice ya los calcula el servicio
                };

                var created = await _service.CreateAsync(transaction);
                if (created == null)
                    return BadRequest("Stock insuficiente o producto no encontrado.");

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
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TransactionDto>> Update(int id, UpdateTransactionDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var transaction = new Transaction
                {
                    Type = dto.Type,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    UnitPrice = dto.UnitPrice,
                    Detail = dto.Detail
                };

                var updated = await _service.UpdateAsync(id, transaction);
                if (updated == null)
                    return NotFound($"No se pudo actualizar. Transacción con ID {id} no encontrada o stock insuficiente.");

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
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _service.DeleteAsync(id);
                if (!success)
                    return NotFound($"No se pudo eliminar. Transacción con ID {id} no encontrada.");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
        
    }
    
}

