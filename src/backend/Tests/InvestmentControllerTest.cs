using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using backend.Controllers;
using backend.Services;
using backend.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Tests.Controllers
{
    public class InvestmentControllerTests
    {
        private readonly Mock<IInvestmentService> _mockInvestmentService;
        private readonly Mock<ILogger<InvestmentController>> _mockLogger;
        private readonly InvestmentController _controller;

        public InvestmentControllerTests()
        {
            _mockInvestmentService = new Mock<IInvestmentService>();
            _mockLogger = new Mock<ILogger<InvestmentController>>();
            _controller = new InvestmentController(_mockInvestmentService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetInvestments_ValidClientId_ReturnsOkWithInvestments()
        {
            // Arrange
            var clientId = "client123";
            var expectedInvestments = new List<Investment>
            {
                new Investment
                {
                    Id = "inv1",
                    AtivosId = "asset1",
                    ClientesId = clientId,
                    Quantidade = 100,
                    ValorTotal = 5000.00,
                    Data = DateTime.Now,
                    Ativo = new Asset
                    {
                        Id = "asset1",
                        Nome = "Ação XYZ",
                        Tipo = "Ação",
                        Subgrupo = "Blue Chip"
                    }
                },
                new Investment
                {
                    Id = "inv2",
                    AtivosId = "asset2",
                    ClientesId = clientId,
                    Quantidade = 50,
                    ValorTotal = 2500.00,
                    Data = DateTime.Now.AddDays(-1),
                    Ativo = new Asset
                    {
                        Id = "asset2",
                        Nome = "CDB ABC",
                        Tipo = "Renda Fixa",
                        Subgrupo = "CDB"
                    }
                }
            };

            _mockInvestmentService.Setup(s => s.GetInvestmentsByClientIdAsync(clientId))
                .ReturnsAsync(expectedInvestments);

            // Act
            var result = await _controller.GetInvestments(clientId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var investments = Assert.IsAssignableFrom<IEnumerable<Investment>>(okResult.Value);
            Assert.Equal(2, ((List<Investment>)investments).Count);
            
            _mockInvestmentService.Verify(s => s.GetInvestmentsByClientIdAsync(clientId), Times.Once);
        }

        [Fact]
        public async Task GetInvestments_ClientWithNoInvestments_ReturnsOkWithEmptyList()
        {
            // Arrange
            var clientId = "client456";
            var emptyInvestments = new List<Investment>();

            _mockInvestmentService.Setup(s => s.GetInvestmentsByClientIdAsync(clientId))
                .ReturnsAsync(emptyInvestments);

            // Act
            var result = await _controller.GetInvestments(clientId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var investments = Assert.IsAssignableFrom<IEnumerable<Investment>>(okResult.Value);
            Assert.Empty(investments);
            
            _mockInvestmentService.Verify(s => s.GetInvestmentsByClientIdAsync(clientId), Times.Once);
        }

        [Fact]
        public async Task GetInvestments_ServiceThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            var clientId = "client789";
            var exceptionMessage = "Database connection failed";

            _mockInvestmentService.Setup(s => s.GetInvestmentsByClientIdAsync(clientId))
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _controller.GetInvestments(clientId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("An error occurred while fetching investments", statusCodeResult.Value);
            
            _mockInvestmentService.Verify(s => s.GetInvestmentsByClientIdAsync(clientId), Times.Once);
            
            // Verify that error was logged
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Error fetching investments for client {clientId}")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task GetInvestments_EmptyOrNullClientId_StillCallsService(string clientId)
        {
            // Arrange
            var emptyInvestments = new List<Investment>();
            _mockInvestmentService.Setup(s => s.GetInvestmentsByClientIdAsync(clientId))
                .ReturnsAsync(emptyInvestments);

            // Act
            var result = await _controller.GetInvestments(clientId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var investments = Assert.IsAssignableFrom<IEnumerable<Investment>>(okResult.Value);
            Assert.Empty(investments);
            
            _mockInvestmentService.Verify(s => s.GetInvestmentsByClientIdAsync(clientId), Times.Once);
        }

        [Fact]
        public async Task GetInvestments_SpecificException_ReturnsInternalServerErrorWithGenericMessage()
        {
            // Arrange
            var clientId = "client999";
            var specificException = new InvalidOperationException("Specific database error");

            _mockInvestmentService.Setup(s => s.GetInvestmentsByClientIdAsync(clientId))
                .ThrowsAsync(specificException);

            // Act
            var result = await _controller.GetInvestments(clientId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("An error occurred while fetching investments", statusCodeResult.Value);
            
            // Verify that the specific exception was logged but generic message returned
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Error fetching investments for client {clientId}")),
                    It.Is<Exception>(ex => ex == specificException),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task GetInvestments_ValidClientId_ReturnsCorrectInvestmentStructure()
        {
            // Arrange
            var clientId = "client123";
            var testDate = new DateTime(2024, 1, 15, 10, 30, 0);
            var expectedInvestment = new Investment
            {
                Id = "inv123",
                AtivosId = "asset123",
                ClientesId = clientId,
                Quantidade = 75,
                ValorTotal = 3750.50,
                Data = testDate,
                Ativo = new Asset
                {
                    Id = "asset123",
                    Nome = "Tesouro Direto",
                    Tipo = "Renda Fixa",
                    Subgrupo = "Tesouro"
                }
            };

            _mockInvestmentService.Setup(s => s.GetInvestmentsByClientIdAsync(clientId))
                .ReturnsAsync(new List<Investment> { expectedInvestment });

            // Act
            var result = await _controller.GetInvestments(clientId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var investments = Assert.IsAssignableFrom<IEnumerable<Investment>>(okResult.Value);
            var investmentList = investments.ToList();
            
            Assert.Single(investmentList);
            var investment = investmentList.First();
            
            Assert.Equal("inv123", investment.Id);
            Assert.Equal("asset123", investment.AtivosId);
            Assert.Equal(clientId, investment.ClientesId);
            Assert.Equal(75, investment.Quantidade);
            Assert.Equal(3750.50, investment.ValorTotal);
            Assert.Equal(testDate, investment.Data);
            
            Assert.NotNull(investment.Ativo);
            Assert.Equal("asset123", investment.Ativo.Id);
            Assert.Equal("Tesouro Direto", investment.Ativo.Nome);
            Assert.Equal("Renda Fixa", investment.Ativo.Tipo);
            Assert.Equal("Tesouro", investment.Ativo.Subgrupo);
        }
    }
}