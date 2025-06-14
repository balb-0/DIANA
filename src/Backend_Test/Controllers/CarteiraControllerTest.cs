using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using backend.Controllers;
using backend.Services;
using backend.Models;
using backend.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend_test.Controllers
{
    public class CarteirasControllerTests
    {
        private readonly Mock<ICarteiraService> _mockCarteiraService;
        private readonly CarteirasController _controller;

        public CarteirasControllerTests()
        {
            _mockCarteiraService = new Mock<ICarteiraService>();
            _controller = new CarteirasController(_mockCarteiraService.Object);

            // Setup do HttpContext para simular o contexto da requisição
            var httpContext = new DefaultHttpContext();
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
        }

        [Fact]
        public async Task GetClientesByRiscoDeclarado_ComAssessorIdValido_DeveRetornarOk()
        {
            // Arrange
            var tipoRisco = "conservador";
            var assessorId = "123";
            var clientesEsperados = new List<ClienteSimplificadoDto>
            {
                new ClienteSimplificadoDto { ClienteId = "1", ClienteNome = "João Silva" },
                new ClienteSimplificadoDto { ClienteId = "2", ClienteNome = "Maria Santos" }
            };

            var responseEsperada = ApiResponse<List<ClienteSimplificadoDto>>.CreateSuccess(
                clientesEsperados,
                "Clientes encontrados com sucesso"
            );

            _controller.HttpContext.Items["UserId"] = assessorId;
            _mockCarteiraService
                .Setup(s => s.GetClientesByRiscoDeclaradoAsync(tipoRisco, assessorId))
                .ReturnsAsync(responseEsperada);

            // Act
            var result = await _controller.GetClientesByRiscoDeclarado(tipoRisco);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<List<ClienteSimplificadoDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(2, response.Data.Count);
            Assert.Equal("João Silva", response.Data[0].ClienteNome);

            _mockCarteiraService.Verify(s => s.GetClientesByRiscoDeclaradoAsync(tipoRisco, assessorId), Times.Once);
        }

        [Fact]
        public async Task GetClientesByRiscoDeclarado_SemAssessorId_DeveRetornarUnauthorized()
        {
            // Arrange
            var tipoRisco = "conservador";
            // Não definir UserId no HttpContext.Items

            // Act
            var result = await _controller.GetClientesByRiscoDeclarado(tipoRisco);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            var response = unauthorizedResult.Value;
            Assert.NotNull(response);

            _mockCarteiraService.Verify(s => s.GetClientesByRiscoDeclaradoAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task GetClientesByRiscoDeclarado_ComAssessorIdVazio_DeveRetornarUnauthorized()
        {
            // Arrange
            var tipoRisco = "conservador";
            _controller.HttpContext.Items["UserId"] = "";

            // Act
            var result = await _controller.GetClientesByRiscoDeclarado(tipoRisco);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.NotNull(unauthorizedResult.Value);

            _mockCarteiraService.Verify(s => s.GetClientesByRiscoDeclaradoAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task GetClientesByRiscoDeclarado_ComServiceRetornandoErro_DeveRetornarBadRequest()
        {
            // Arrange
            var tipoRisco = "invalido";
            var assessorId = "123";
            var responseComErro = ApiResponse<List<ClienteSimplificadoDto>>.CreateError("Tipo de risco inválido");

            _controller.HttpContext.Items["UserId"] = assessorId;
            _mockCarteiraService
                .Setup(s => s.GetClientesByRiscoDeclaradoAsync(tipoRisco, assessorId))
                .ReturnsAsync(responseComErro);

            // Act
            var result = await _controller.GetClientesByRiscoDeclarado(tipoRisco);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<List<ClienteSimplificadoDto>>>(badRequestResult.Value);
            Assert.False(response.Success);
            Assert.Equal("Tipo de risco inválido", response.Message);

            _mockCarteiraService.Verify(s => s.GetClientesByRiscoDeclaradoAsync(tipoRisco, assessorId), Times.Once);
        }

        [Theory]
        [InlineData("conservador")]
        [InlineData("moderado")]
        [InlineData("sofisticado")]
        [InlineData("geral")]
        public async Task GetClientesByRiscoDeclarado_ComTiposRiscoValidos_DeveChamarService(string tipoRisco)
        {
            // Arrange
            var assessorId = "123";
            var responseEsperada = ApiResponse<List<ClienteSimplificadoDto>>.CreateSuccess(
                new List<ClienteSimplificadoDto>(),
                "Sucesso"
            );

            _controller.HttpContext.Items["UserId"] = assessorId;
            _mockCarteiraService
                .Setup(s => s.GetClientesByRiscoDeclaradoAsync(tipoRisco, assessorId))
                .ReturnsAsync(responseEsperada);

            // Act
            var result = await _controller.GetClientesByRiscoDeclarado(tipoRisco);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            _mockCarteiraService.Verify(s => s.GetClientesByRiscoDeclaradoAsync(tipoRisco, assessorId), Times.Once);
        }

        [Fact]
        public async Task GetClientesByRiscoDeclarado_ComListaVazia_DeveRetornarOkComListaVazia()
        {
            // Arrange
            var tipoRisco = "conservador";
            var assessorId = "123";
            var responseEsperada = ApiResponse<List<ClienteSimplificadoDto>>.CreateSuccess(
                new List<ClienteSimplificadoDto>(),
                "Nenhum cliente encontrado"
            );

            _controller.HttpContext.Items["UserId"] = assessorId;
            _mockCarteiraService
                .Setup(s => s.GetClientesByRiscoDeclaradoAsync(tipoRisco, assessorId))
                .ReturnsAsync(responseEsperada);

            // Act
            var result = await _controller.GetClientesByRiscoDeclarado(tipoRisco);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<List<ClienteSimplificadoDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Empty(response.Data);
        }
    }
}