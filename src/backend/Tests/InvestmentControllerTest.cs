using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using backend.Controllers;
using backend.Models;
using backend.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Tests.Controllers
{
    public class InvestmentControllerTest
    {
        private readonly Mock<IInvestmentService> _mockInvestmentService;
        private readonly Mock<ILogger<InvestmentController>> _mockLogger;
        private readonly InvestmentController _controller;

        public InvestmentControllerTest()
        {
            _mockInvestmentService = new Mock<IInvestmentService>();
            _mockLogger = new Mock<ILogger<InvestmentController>>();
            _controller = new InvestmentController(_mockInvestmentService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetInvestments_WithValidClientId_ReturnsInvestments()
        {
            // Arrange
            var clientId = "client123";
            var investments = new List<Investment>
            {
                new Investment { Id = "1", ClientesId = clientId, AtivosId = "A1", Quantidade = 10, ValorTotal = 1000, Data = System.DateTime.Now },
                new Investment { Id = "2", ClientesId = clientId, AtivosId = "A2", Quantidade = 5, ValorTotal = 500, Data = System.DateTime.Now }
            };
            _mockInvestmentService.Setup(s => s.GetInvestmentsByClientIdAsync(clientId)).ReturnsAsync(investments);

            // Act
            var result = await _controller.GetInvestments(clientId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<Investment>>(okResult.Value);
            Assert.Equal(2, System.Linq.Enumerable.Count(returnValue));
        }

        [Fact]
        public async Task GetInvestments_WhenServiceThrowsException_ReturnsServerError()
        {
            // Arrange
            var clientId = "client123";
            _mockInvestmentService.Setup(s => s.GetInvestmentsByClientIdAsync(clientId)).ThrowsAsync(new System.Exception("DB error"));

            // Act
            var result = await _controller.GetInvestments(clientId);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("An error occurred while fetching investments", objectResult.Value);
        }
    }
}
