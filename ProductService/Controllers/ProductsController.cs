using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductService.Models;
using ProductService.Models.DTOs;
using ProductService.Services;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController: ControllerBase 
    {
       private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
        {
            var products = await _service.GetAllAsync();

            var result = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Category = p.Category,
                Price = p.Price,
                Stock = p.Stock
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {
            var product = await _service.GetByIdAsync(id);
            if (product == null) return NotFound();

            var result = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Category = product.Category,
                Price = product.Price,
                Stock = product.Stock
            };

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create(CreateProductDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Category = dto.Category,
                ImageUrl = dto.ImageUrl,
                Price = dto.Price,
                Stock = dto.Stock
            };

            var created = await _service.CreateAsync(product);

            var result = new ProductDto
            {
                Id = created.Id,
                Name = created.Name,
                Category = created.Category,
                Price = created.Price,
                Stock = created.Stock
            };

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDto>> Update(int id, UpdateProductDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Category = dto.Category,
                ImageUrl = dto.ImageUrl,
                Price = dto.Price,
                Stock = dto.Stock
            };

            var updated = await _service.UpdateAsync(id, product);
            if (updated == null) return NotFound();

            var result = new ProductDto
            {
                Id = updated.Id,
                Name = updated.Name,
                Category = updated.Category,
                Price = updated.Price,
                Stock = updated.Stock
            };

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }
        
        [HttpPatch("{id}/stock")]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] StockUpdateDto dto)
        {
            if (dto.Adjustment == 0)
                return BadRequest("El ajuste de stock no puede ser 0.");

            var product = await _service.GetByIdAsync(id);
            if (product == null)
                return NotFound("Producto no encontrado.");

            var newStock = product.Stock + dto.Adjustment;
            if (newStock < 0)
                return BadRequest("El stock no puede ser negativo.");

            product.Stock = newStock;

            var updated = await _service.UpdateAsync(id, product);
            if (updated == null)
                return StatusCode(500, "Error al actualizar el stock del producto.");
            Console.WriteLine($"[UpdateStock] Id={id}, dto={(dto == null ? "NULL" : dto.Adjustment.ToString())}");

            return Ok(new { message = "Stock actualizado correctamente", productId = id, stock = updated.Stock });
        }

        
    }
}

