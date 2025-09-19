
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TransactionService.Models;
using TransactionService.Repositories;
using JsonSerializerOptions = System.Text.Json.JsonSerializerOptions;

namespace TransactionService.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _repository;
    private readonly HttpClient _httpClient;

    public TransactionService(ITransactionRepository repository, IHttpClientFactory httpClientFactory)
    {
        _repository = repository;
        _httpClient = httpClientFactory.CreateClient("ProductService");
    }

    public async Task<IEnumerable<Transaction>> GetAllAsync() =>
        await _repository.GetAllAsync();

    public async Task<Transaction?> GetByIdAsync(int id) =>
        await _repository.GetByIdAsync(id);

    public async Task<Transaction> CreateAsync(Transaction transaction)
    {

    var product = await _httpClient.GetFromJsonAsync<ProductDto>(
        $"products/{transaction.ProductId}"
    );

    if (product == null)
    {
        Console.WriteLine($"[CreateAsync] Producto {transaction.ProductId} no encontrado en ProductService");
        throw new Exception("Producto no encontrado en ProductService");
    }
    
    if (transaction.Type == "Venta" && product.Stock < transaction.Quantity)
    {
        throw new Exception("Stock insuficiente para la venta");
    }

    var stockAdjustment = transaction.Type == "Venta"
        ? -transaction.Quantity
        : transaction.Quantity;

    Console.WriteLine($"[CreateAsync] Ajuste de stock: {stockAdjustment} para ProductoId={transaction.ProductId}");
    
    var response = await _httpClient.PatchAsJsonAsync(
        $"products/{transaction.ProductId}/stock",
        new { Adjustment = stockAdjustment }
    );
    

    if (!response.IsSuccessStatusCode)
    {
        throw new Exception("Error al actualizar stock en ProductService");
    }
    
    transaction.TotalPrice = transaction.UnitPrice * transaction.Quantity;
    transaction.Date = DateTime.UtcNow;

    await _repository.AddAsync(transaction);
    return transaction;
    }

    public async Task<Transaction?> UpdateAsync(int id, Transaction transaction)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null) return null;
        
        var revertAdjustment = existing.Type == "Venta"
            ? existing.Quantity
            : -existing.Quantity;

        var revertResponse = await _httpClient.PatchAsJsonAsync(
            $"products/{existing.ProductId}/stock",
            new { Adjustment = revertAdjustment }
        );

        if (!revertResponse.IsSuccessStatusCode)
            throw new Exception("Error al revertir stock de la transacción anterior en ProductService");
        
        var newAdjustment = transaction.Type == "Venta"
            ? -transaction.Quantity
            : transaction.Quantity;

        var applyResponse = await _httpClient.PatchAsJsonAsync(
            $"products/{transaction.ProductId}/stock",
            new { Adjustment = newAdjustment }
        );

        if (!applyResponse.IsSuccessStatusCode)
            throw new Exception("Error al aplicar nuevo stock en ProductService");
        
        existing.Type = transaction.Type;
        existing.ProductId = transaction.ProductId;
        existing.Quantity = transaction.Quantity;
        existing.UnitPrice = transaction.UnitPrice;
        existing.TotalPrice = transaction.UnitPrice * transaction.Quantity;
        existing.Detail = transaction.Detail;
        existing.Date = DateTime.UtcNow;

        await _repository.UpdateAsync(existing);
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null) return false;

        var revertAdjustment = existing.Type == "Venta"
            ? existing.Quantity
            : -existing.Quantity;

        var revertResponse = await _httpClient.PatchAsJsonAsync(
            $"products/{existing.ProductId}/stock",
            new { Adjustment = revertAdjustment }
        );

        if (!revertResponse.IsSuccessStatusCode)
            throw new Exception("Error al revertir stock en ProductService");

        await _repository.DeleteAsync(existing);
        return true;
    }
    public async Task<ProductDto?> GetProductByIdAsync(int productId)
    {
        try
        {
            var product = await _httpClient.GetFromJsonAsync<ProductDto>($"products/{productId}");
            return product;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[GetProductByIdAsync] Error obteniendo producto {productId}: {ex.Message}");
            return null;
        }
    }

    
}