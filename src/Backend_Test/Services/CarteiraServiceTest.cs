using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using backend.Services;
using backend.Repositories;
using backend.Models;
using backend.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend_test.Services
{
    public class CarteiraServiceTests
    {
        private readonly Mock<ICarteiraRepository> _mockCarteiraRepository;
        private readonly Mock<ILogger<CarteiraService>> _mockLogger;
        private readonly CarteiraService _service;

        public CarteiraServiceTests()
        {
            _mockCarteiraRepository = new Mock<ICarteiraRepository>();
            _mockLogger = new Mock<ILogger<CarteiraService>>();
            _service = new CarteiraService(_mockCarteiraRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetClientesByRiscoDeclaradoAsync_ComParametrosValidos_DeveRetornarSucesso()
        {
            // Arrange
            var tipoRisco = "conservador";
            var assessorId = "123";
            var clientesEsperados = new List<ClienteSimplificadoDto>
            {
                new ClienteSimplificadoDto { ClienteId = 1.ToString(), ClienteNome = "João Silva" },
                new ClienteSimplificadoDto { ClienteId = 2.ToString(), ClienteNome = "Maria Santos" }
            };

            _mockCarteiraRepository
                .Setup(r => r.GetClientesByRiscoDeclaradoAsync(tipoRisco, assessorId))
                .ReturnsAsync(clientesEsperados);

            // Act
            var result = await _service.GetClientesByRiscoDeclaradoAsync(tipoRisco, assessorId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(2, result.Data.Count);
            Assert.Equal("João Silva", result.Data[0].ClienteNome);
            Assert.Contains("encontrados com sucesso", result.Message);

            _mockCarteiraRepository.Verify(r => r.GetClientesByRiscoDeclaradoAsync(tipoRisco, assessorId), Times.Once);
            VerifyLogInformation($"Iniciando busca por clientes com risco declarado: {tipoRisco} para o assessor: {assessorId}");
            VerifyLogInformation($"Encontrados {clientesEsperados.Count} clientes com risco declarado {tipoRisco} para o assessor {assessorId}");
        }

        [Theory]
        [InlineData("conservador")]
        [InlineData("CONSERVADOR")]
        [InlineData("Conservador")]
        [InlineData("CoNsErVaDoR")]
        public async Task GetClientesByRiscoDeclaradoAsync_ComCaseDiferente_DeveAceitarTipoRisco(string tipoRisco)
        {
            // Arrange
            var assessorId = "123";
            var clientesEsperados = new List<ClienteSimplificadoDto>();

            _mockCarteiraRepository
                .Setup(r => r.GetClientesByRiscoDeclaradoAsync(tipoRisco, assessorId))
                .ReturnsAsync(clientesEsperados);

            // Act
            var result = await _service.GetClientesByRiscoDeclaradoAsync(tipoRisco, assessorId);

            // Assert
            Assert.True(result.Success);
            _mockCarteiraRepository.Verify(r => r.GetClientesByRiscoDeclaradoAsync(tipoRisco, assessorId), Times.Once);
        }

        [Theory]
        [InlineData("geral")]
        [InlineData("moderado")]
        [InlineData("sofisticado")]
        public async Task GetClientesByRiscoDeclaradoAsync_ComTiposRiscoValidos_DeveRetornarSucesso(string tipoRisco)
        {
            // Arrange
            var assessorId = "123";
            var clientesEsperados = new List<ClienteSimplificadoDto>();

            _mockCarteiraRepository
                .Setup(r => r.GetClientesByRiscoDeclaradoAsync(tipoRisco, assessorId))
                .ReturnsAsync(clientesEsperados);

            // Act
            var result = await _service.GetClientesByRiscoDeclaradoAsync(tipoRisco, assessorId);

            // Assert
            Assert.True(result.Success);
            _mockCarteiraRepository.Verify(r => r.GetClientesByRiscoDeclaradoAsync(tipoRisco, assessorId), Times.Once);
        }

        [Theory]
        [InlineData("invalido")]
        [InlineData("alto")]
        [InlineData("baixo")]
        [InlineData("")]
        [InlineData("   ")]
        public async Task GetClientesByRiscoDeclaradoAsync_ComTipoRiscoInvalido_DeveRetornarErro(string tipoRisco)
        {
            // Arrange
            var assessorId = "123";

            // Act
            var result = await _service.GetClientesByRiscoDeclaradoAsync(tipoRisco, assessorId);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Tipo de risco inválido. Valores aceitos: Conservador, Moderado, Sofisticado, Geral", result.Message);
            Assert.Null(result.Data);

            _mockCarteiraRepository.Verify(r => r.GetClientesByRiscoDeclaradoAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            VerifyLogWarning($"Tipo de risco inválido recebido: {tipoRisco}");
        }

        [Fact]
        public async Task GetClientesByRiscoDeclaradoAsync_ComListaVazia_DeveRetornarSucessoComListaVazia()
        {
            // Arrange
            var tipoRisco = "conservador";
            var assessorId = "123";
            var clientesVazios = new List<ClienteSimplificadoDto>();

            _mockCarteiraRepository
                .Setup(r => r.GetClientesByRiscoDeclaradoAsync(tipoRisco, assessorId))
                .ReturnsAsync(clientesVazios);

            // Act
            var result = await _service.GetClientesByRiscoDeclaradoAsync(tipoRisco, assessorId);

            // Assert
            Assert.True(result.Success);
            Assert.Empty(result.Data);
            Assert.Contains("encontrados com sucesso", result.Message);

            VerifyLogInformation($"Encontrados 0 clientes com risco declarado {tipoRisco} para o assessor {assessorId}");
        }

        [Fact]
        public async Task GetClientesByRiscoDeclaradoAsync_ComExcecaoNoRepository_DeveRetornarErro()
        {
            // Arrange
            var tipoRisco = "conservador";
            var assessorId = "123";
            var excecaoEsperada = new Exception("Erro de conexão com banco de dados");

            _mockCarteiraRepository
                .Setup(r => r.GetClientesByRiscoDeclaradoAsync(tipoRisco, assessorId))
                .ThrowsAsync(excecaoEsperada);

            // Act
            var result = await _service.GetClientesByRiscoDeclaradoAsync(tipoRisco, assessorId);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Erro interno do servidor", result.Message);
            Assert.Null(result.Data);

            VerifyLogError(excecaoEsperada, $"Erro ao buscar clientes com risco declarado {tipoRisco} para o assessor {assessorId}. Detalhes: {excecaoEsperada.Message}");
        }

        [Fact]
        public async Task GetClientesByRiscoDeclaradoAsync_ComExcecaoGenerica_DeveRetornarErroGenerico()
        {
            // Arrange
            var tipoRisco = "moderado";
            var assessorId = "456";
            var excecaoEsperada = new InvalidOperationException("Operação inválida");

            _mockCarteiraRepository
                .Setup(r => r.GetClientesByRiscoDeclaradoAsync(tipoRisco, assessorId))
                .ThrowsAsync(excecaoEsperada);

            // Act
            var result = await _service.GetClientesByRiscoDeclaradoAsync(tipoRisco, assessorId);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Erro interno do servidor", result.Message);
            Assert.Null(result.Data);

            _mockCarteiraRepository.Verify(r => r.GetClientesByRiscoDeclaradoAsync(tipoRisco, assessorId), Times.Once);
        }

        [Fact]
        public async Task GetClientesByRiscoDeclaradoAsync_ComMuitosClientes_DeveRetornarTodosOsClientes()
        {
            // Arrange
            var tipoRisco = "sofisticado";
            var assessorId = "789";
            var clientesEsperados = new List<ClienteSimplificadoDto>();

            for (int i = 1; i <= 100; i++)
            {
                clientesEsperados.Add(new ClienteSimplificadoDto
                {
                    ClienteId = i.ToString(),
                    ClienteNome = $"Cliente {i}"
                });
            }

            _mockCarteiraRepository
                .Setup(r => r.GetClientesByRiscoDeclaradoAsync(tipoRisco, assessorId))
                .ReturnsAsync(clientesEsperados);

            // Act
            var result = await _service.GetClientesByRiscoDeclaradoAsync(tipoRisco, assessorId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(100, result.Data.Count);
            Assert.Equal("Cliente 1", result.Data[0].ClienteNome);
            Assert.Equal("Cliente 100", result.Data[99].ClienteNome);

            VerifyLogInformation($"Encontrados 100 clientes com risco declarado {tipoRisco} para o assessor {assessorId}");
        }

        #region Métodos auxiliares para verificação de logs

        private void VerifyLogInformation(string expectedMessage)
        {
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(expectedMessage)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        private void VerifyLogWarning(string expectedMessage)
        {
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(expectedMessage)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        private void VerifyLogError(Exception expectedException, string expectedMessage)
        {
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(expectedMessage)),
                    expectedException,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        #endregion
    }
}