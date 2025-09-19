using TransactionService.Models;
using TransactionService.Repositories;

namespace TransactionService.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _repository;

    public TransactionService(ITransactionRepository repository, IHttpClientFactory httpClientFactory)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Transaction>> GetAllAsync() =>
        await _repository.GetAllAsync();

    public async Task<Transaction?> GetByIdAsync(int id) =>
        await _repository.GetByIdAsync(id);

    public async Task<Transaction> CreateAsync(Transaction transaction)
    {
        transaction.TotalPrice = transaction.UnitPrice * transaction.Quantity;
        await _repository.AddAsync(transaction);
        return transaction;
    }

    public async Task<Transaction?> UpdateAsync(int id, Transaction transaction)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null) return null;

        existing.Date = transaction.Date;
        existing.Type = transaction.Type;
        existing.ProductId = transaction.ProductId;
        existing.Quantity = transaction.Quantity;
        existing.UnitPrice = transaction.UnitPrice;
        existing.TotalPrice = transaction.UnitPrice * transaction.Quantity;
        existing.Detail = transaction.Detail;

        await _repository.UpdateAsync(existing);
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null) return false;

        await _repository.DeleteAsync(existing);
        return true;
    }
}