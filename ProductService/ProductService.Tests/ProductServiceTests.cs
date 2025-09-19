using Moq;
using ProductService.Models;
using ProductService.Repositories;
using Xunit;

namespace ProductService.ProductService.Tests
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _mockRepo;
        private readonly global::ProductService.Services.ProductService _service;

        public ProductServiceTests()
        {
            _mockRepo = new Mock<IProductRepository>();
            _service = new global::ProductService.Services.ProductService(_mockRepo.Object);
        }

        [Fact(DisplayName = "Devuelve lista de productos cuando existen")]
        public async Task GetAll()
        {
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Stock = 10 },
                new Product { Id = 2, Name = "Mouse", Stock = 50 }
            };
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(products);
            
            var result = await _service.GetAllAsync();

            Assert.NotNull(result);
            Assert.Collection(result,
                p => Assert.Equal("Laptop", p.Name),
                p => Assert.Equal("Mouse", p.Name));
        }

        [Fact(DisplayName = "Devuelve producto por Id cuando existe")]
        public async Task GetById_Exists()
        {
            var product = new Product { Id = 1, Name = "Laptop", Stock = 10 };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

            var result = await _service.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Laptop", result!.Name);
        }

        [Fact(DisplayName = "Devuelve null al buscar producto inexistente")]
        public async Task GetById_Null()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Product?)null);

            var result = await _service.GetByIdAsync(99);

            Assert.Null(result);
        }

        [Fact(DisplayName = "Agrega un producto correctamente")]
        public async Task Create()
        {
            var product = new Product { Id = 1, Name = "Laptop", Stock = 5 };

            await _service.CreateAsync(product);

            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact(DisplayName = "Actualiza producto existente")]
        public async Task Update_Exists()
        {
            var existing = new Product { Id = 1, Name = "Old", Stock = 5 };
            var updated = new Product { Id = 1, Name = "New", Stock = 10 };

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);

            var result = await _service.UpdateAsync(1, updated);

            Assert.NotNull(result);
            Assert.Equal("New", result!.Name);
            Assert.Equal(10, result.Stock);
            _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact(DisplayName = "Devuelve null al intentar actualizar producto inexistente")]
        public async Task Update_Null()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Product?)null);

            var updated = new Product { Id = 1, Name = "New" };

            var result = await _service.UpdateAsync(1, updated);

            Assert.Null(result);
        }

        [Fact(DisplayName = "Elimina producto existente")]
        public async Task Delete_Exists()
        {
            var existing = new Product { Id = 1, Name = "Laptop" };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);

            var result = await _service.DeleteAsync(1);

            Assert.True(result);
            _mockRepo.Verify(r => r.DeleteAsync(existing), Times.Once);
        }

        [Fact(DisplayName = "Devuelve false al intentar eliminar producto inexistente")]
        public async Task Delete_False()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Product?)null);

            var result = await _service.DeleteAsync(1);

            Assert.False(result);
        }

        [Fact(DisplayName = "Incrementa stock cuando cantidad es positiva")]
        public async Task Stock_Increase()
        {
            var product = new Product { Id = 1, Name = "Laptop", Stock = 5 };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

            var result = await _service.UpdateStockAsync(1, 3);

            Assert.True(result);
            Assert.Equal(8, product.Stock);
            _mockRepo.Verify(r => r.UpdateAsync(product), Times.Once);
        }

        [Fact(DisplayName = "Disminuye stock cuando cantidad es negativa")]
        public async Task Stock_Decrease()
        {
            var product = new Product { Id = 1, Name = "Laptop", Stock = 5 };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

            var result = await _service.UpdateStockAsync(1, -2);

            Assert.True(result);
            Assert.Equal(3, product.Stock);
            _mockRepo.Verify(r => r.UpdateAsync(product), Times.Once);
        }

        [Fact(DisplayName = "Devuelve false cuando stock resultante es negativo")]
        public async Task Stock_False()
        {
            var product = new Product { Id = 1, Name = "Laptop", Stock = 1 };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

            var result = await _service.UpdateStockAsync(1, -5);

            Assert.False(result);
            _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Product>()), Times.Never);
        }
    }
}
