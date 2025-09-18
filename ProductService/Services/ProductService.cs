using ProductService.Models;
using ProductService.Repositories;

namespace ProductService.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Product>> GetAllAsync() =>
            await _repository.GetAllAsync();

        public async Task<Product?> GetByIdAsync(int id) =>
            await _repository.GetByIdAsync(id);

        public async Task<Product> CreateAsync(Product product)
        {
            await _repository.AddAsync(product);
            return product;
        }

        public async Task<Product?> UpdateAsync(int id, Product product)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return null;

            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.Category = product.Category;
            existing.ImageUrl = product.ImageUrl;
            existing.Price = product.Price;
            existing.Stock = product.Stock;

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
}