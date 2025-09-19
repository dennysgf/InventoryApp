using Moq;
using Moq.Protected;
using Xunit;
using TransactionService.Models;
using TransactionService.Repositories;
using System.Net;

using System.Text.Json;


namespace TransactionService.Tests.Services
{
    public static class HttpMessageHandlerExtensions
    {
        public static void SetupRequest(this Mock<HttpMessageHandler> handler, HttpMethod method, string url, HttpResponseMessage response)
        {
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == method &&
                        req.RequestUri!.ToString() == url),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(response);
        }

        public static void SetupRequest(this Mock<HttpMessageHandler> handler, HttpMethod method, string url, string content, string mediaType = "application/json")
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(content, System.Text.Encoding.UTF8, mediaType)
            };
            handler.SetupRequest(method, url, response);
        }

        public static void SetupRequest(this Mock<HttpMessageHandler> handler, HttpMethod method, string url, HttpStatusCode statusCode)
        {
            var response = new HttpResponseMessage(statusCode);
            handler.SetupRequest(method, url, response);
        }
    }

    public class TransactionServiceTests
    {
        private readonly Mock<ITransactionRepository> _mockRepo;
        private readonly TransactionService.Services.TransactionService _service;
        private readonly Mock<HttpMessageHandler> _httpHandler;
        private readonly HttpClient _httpClient;

        public TransactionServiceTests()
        {
            _mockRepo = new Mock<ITransactionRepository>();
            _httpHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpHandler.Object)
            {
                BaseAddress = new Uri("http://localhost/")
            };

            var httpClientFactory = new Mock<IHttpClientFactory>();
            httpClientFactory.Setup(f => f.CreateClient("ProductService"))
                             .Returns(_httpClient);

            _service = new TransactionService.Services.TransactionService(
                _mockRepo.Object,
                httpClientFactory.Object
            );
        }

        [Fact(DisplayName = "Crea transacción de compra correctamente")]
        public async Task Create_Buy()
        {
            var product = new ProductDto { Id = 1, Name = "Laptop", Stock = 10, Price = 1000 };
            var transaction = new Transaction { ProductId = 1, Type = "Compra", Quantity = 2, UnitPrice = 500 };
            var productJson = JsonSerializer.Serialize(product);

            _httpHandler.SetupRequest(HttpMethod.Get, "http://localhost/products/1", productJson);
            _httpHandler.SetupRequest(HttpMethod.Patch, "http://localhost/products/1/stock", HttpStatusCode.OK);

            var result = await _service.CreateAsync(transaction);

            Assert.NotNull(result);
            Assert.Equal(1000, result.TotalPrice);
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Transaction>()), Times.Once);
        }

        [Fact(DisplayName = "Falla al crear venta sin stock suficiente")]
        public async Task Create_Sell_NoStock()
        {
            var product = new ProductDto { Id = 1, Name = "Laptop", Stock = 1 };
            var transaction = new Transaction { ProductId = 1, Type = "Venta", Quantity = 5, UnitPrice = 1000 };
            var productJson = JsonSerializer.Serialize(product);

            _httpHandler.SetupRequest(HttpMethod.Get, "http://localhost/products/1", productJson);

            await Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(transaction));
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Transaction>()), Times.Never);
        }

        [Fact(DisplayName = "Devuelve null al intentar actualizar transacción inexistente")]
        public async Task Update_Null()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Transaction?)null);
            var transaction = new Transaction { ProductId = 1, Type = "Compra", Quantity = 2, UnitPrice = 100 };

            var result = await _service.UpdateAsync(1, transaction);

            Assert.Null(result);
        }

        [Fact(DisplayName = "Devuelve false al eliminar transacción inexistente")]
        public async Task Delete_False()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Transaction?)null);

            var result = await _service.DeleteAsync(1);

            Assert.False(result);
        }
    }
}
