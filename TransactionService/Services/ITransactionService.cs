using TransactionService.Models;

namespace TransactionService.Services
{
    public interface ITransactionService
    {
        Task<IEnumerable<Transaction>> GetAllAsync();
        Task<Transaction?> GetByIdAsync(int id);
        Task<Transaction> CreateAsync(Transaction transaction);
        Task<Transaction?> UpdateAsync(int id, Transaction transaction);
        Task<bool> DeleteAsync(int id);
    }
}

