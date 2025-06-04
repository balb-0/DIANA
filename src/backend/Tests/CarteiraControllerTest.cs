using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data;
using Dapper;
using backend.Controllers;
using backend.Services;
using backend.Repositories;
using backend.Models;
using backend.Models.DTOs;
using backend.Models.Enums;

namespace backend.Tests.Controllers
{
    public class CarteirasControllerTests
    {
        private readonly Mock<ICarteiraService> _mockCarteiraService;
        private readonly CarteirasController _controller;

        public CarteirasControllerTests()
        {
            _mockCarteiraService = new Mock<ICarteiraService>();
            _controller = new CarteirasController(_mockCarteiraService.Object);
        }

        [Fact]
        public async Task GetCarteirasByTipo_ValidTipoRisco_ReturnsOkResult()
        {
            // Arrange
            var tipoRisco = "conservador";
            var expectedCarteiras = new List<CarteiraDto>
            {
                new CarteiraDto
                {
                    ClienteId = "1",
                    ClienteNome = "João Silva",
                    ClienteCpf = "12345678901",
                    RiscoDeclarado = RiscoEnum.Conservador,
                    AssessorNome = "Maria Santos",
                    AssessorUnidade = "SP001",
                    RiscoCalculado = RiscoEnum.Conservador,
                    RVPercentual = 20.5,
                    RFPercentual = 79.5,
                    ValorTotalCalculado = 100000.00,
                    ValorTotalCarteira = 100000.00,
                    Investimentos = new List<InvestimentoDto>()
                }
            };
            var expectedResponse = ApiResponse<List<CarteiraDto>>.CreateSuccess(expectedCarteiras, "Success");

            _mockCarteiraService
                .Setup(s => s.GetCarteirasByRiscoAsync(tipoRisco))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetCarteirasByTipo(tipoRisco);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<List<CarteiraDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Single(response.Data);
            Assert.Equal("João Silva", response.Data.First().ClienteNome);
        }

        [Fact]
        public async Task GetCarteirasByTipo_InvalidTipoRisco_ReturnsBadRequest()
        {
            // Arrange
            var tipoRisco = "invalido";
            var errorResponse = ApiResponse<List<CarteiraDto>>.CreateError("Tipo de risco inválido");

            _mockCarteiraService
                .Setup(s => s.GetCarteirasByRiscoAsync(tipoRisco))
                .ReturnsAsync(errorResponse);

            // Act
            var result = await _controller.GetCarteirasByTipo(tipoRisco);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<List<CarteiraDto>>>(badRequestResult.Value);
            Assert.False(response.Success);
            Assert.Contains("inválido", response.Message);
        }

        [Fact]
        public async Task GetCarteirasByTipo_ServiceThrowsException_ReturnsBadRequest()
        {
            // Arrange
            var tipoRisco = "conservador";
            var errorResponse = ApiResponse<List<CarteiraDto>>.CreateError("Erro interno do servidor");

            _mockCarteiraService
                .Setup(s => s.GetCarteirasByRiscoAsync(tipoRisco))
                .ReturnsAsync(errorResponse);

            // Act
            var result = await _controller.GetCarteirasByTipo(tipoRisco);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<List<CarteiraDto>>>(badRequestResult.Value);
            Assert.False(response.Success);
        }

        [Theory]
        [InlineData("conservador")]
        [InlineData("moderado")]
        [InlineData("sofisticado")]
        [InlineData("geral")]
        public async Task GetCarteirasByTipo_AllValidTipoRisco_CallsService(string tipoRisco)
        {
            // Arrange
            var expectedResponse = ApiResponse<List<CarteiraDto>>.CreateSuccess(new List<CarteiraDto>(), "Success");
            _mockCarteiraService
                .Setup(s => s.GetCarteirasByRiscoAsync(tipoRisco))
                .ReturnsAsync(expectedResponse);

            // Act
            await _controller.GetCarteirasByTipo(tipoRisco);

            // Assert
            _mockCarteiraService.Verify(s => s.GetCarteirasByRiscoAsync(tipoRisco), Times.Once);
        }
    }
}