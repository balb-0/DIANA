using backend.Models.DTOs;
using backend.Models.Enums;
using backend.Repositories;
using backend.Services;
using Moq;
using Xunit;
using Dapper;

namespace backend.Tests.Services
{
    public class CarteiraServiceTests
    {
        private readonly Mock<ICarteiraRepository> _mockRepository;
        private readonly Mock<ILogger<CarteiraService>> _mockLogger;
        private readonly CarteiraService _service;

        public CarteiraServiceTests()
        {
            _mockRepository = new Mock<ICarteiraRepository>();
            _mockLogger = new Mock<ILogger<CarteiraService>>();
            _service = new CarteiraService(_mockRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetCarteirasByRiscoAsync_ValidTipoRisco_ReturnsSuccessResponse()
        {
            // Arrange
            var tipoRisco = "conservador";
            var expectedCarteiras = new List<CarteiraDto>
            {
                new CarteiraDto
                {
                    ClienteId = "1",
                    ClienteNome = "João Silva",
                    RiscoCalculado = RiscoEnum.Conservador
                }
            };

            _mockRepository
                .Setup(r => r.GetCarteirasByRiscoAsync(tipoRisco))
                .ReturnsAsync(expectedCarteiras);

            // Act
            var result = await _service.GetCarteirasByRiscoAsync(tipoRisco);

            // Assert
            Assert.True(result.Success);
            Assert.Single(result.Data);
            Assert.Equal("João Silva", result.Data.First().ClienteNome);
            Assert.Contains("sucesso", result.Message);
        }

        [Theory]
        [InlineData("conservador")]
        [InlineData("moderado")]
        [InlineData("sofisticado")]
        [InlineData("geral")]
        public async Task GetCarteirasByRiscoAsync_ValidTipoRisco_CallsRepository(string tipoRisco)
        {
            // Arrange
            _mockRepository
                .Setup(r => r.GetCarteirasByRiscoAsync(tipoRisco))
                .ReturnsAsync(new List<CarteiraDto>());

            // Act
            await _service.GetCarteirasByRiscoAsync(tipoRisco);

            // Assert
            _mockRepository.Verify(r => r.GetCarteirasByRiscoAsync(tipoRisco), Times.Once);
        }

        [Theory]
        [InlineData("invalido")]
        [InlineData("")]
        [InlineData("123")]
        [InlineData("test")]
        public async Task GetCarteirasByRiscoAsync_InvalidTipoRisco_ReturnsErrorResponse(string tipoRisco)
        {
            // Act
            var result = await _service.GetCarteirasByRiscoAsync(tipoRisco);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("inválido", result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task GetCarteirasByRiscoAsync_RepositoryThrowsException_ReturnsErrorResponse()
        {
            // Arrange
            var tipoRisco = "conservador";
            _mockRepository
                .Setup(r => r.GetCarteirasByRiscoAsync(tipoRisco))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _service.GetCarteirasByRiscoAsync(tipoRisco);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Erro interno do servidor", result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task GetCarteirasByRiscoAsync_CaseInsensitive_AcceptsUpperCase()
        {
            // Arrange
            var tipoRisco = "CONSERVADOR";
            _mockRepository
                .Setup(r => r.GetCarteirasByRiscoAsync(tipoRisco))
                .ReturnsAsync(new List<CarteiraDto>());

            // Act
            var result = await _service.GetCarteirasByRiscoAsync(tipoRisco);

            // Assert
            Assert.True(result.Success);
        }

        [Fact]
        public async Task GetCarteirasByRiscoAsync_LogsInformation_WhenSuccessful()
        {
            // Arrange
            var tipoRisco = "conservador";
            var carteiras = new List<CarteiraDto> { new CarteiraDto(), new CarteiraDto() };
            _mockRepository
                .Setup(r => r.GetCarteirasByRiscoAsync(tipoRisco))
                .ReturnsAsync(carteiras);

            // Act
            await _service.GetCarteirasByRiscoAsync(tipoRisco);

            // Assert
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Encontradas 2 carteiras")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task GetCarteirasByRiscoAsync_LogsError_WhenExceptionOccurs()
        {
            // Arrange
            var tipoRisco = "conservador";
            var exception = new Exception("Test exception");
            _mockRepository
                .Setup(r => r.GetCarteirasByRiscoAsync(tipoRisco))
                .ThrowsAsync(exception);

            // Act
            await _service.GetCarteirasByRiscoAsync(tipoRisco);

            // Assert
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Erro ao buscar carteiras")),
                    exception,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task GetCarteirasByRiscoAsync_ReturnsCarteirasWithCorrectProperties()
        {
            // Arrange
            var tipoRisco = "conservador";
            var expectedCarteiras = new List<CarteiraDto>
            {
                new CarteiraDto
                {
                    ClienteId = "123",
                    ClienteNome = "João Silva",
                    ClienteCpf = "12345678901",
                    RiscoDeclarado = RiscoEnum.Conservador,
                    RiscoCalculado = RiscoEnum.Conservador,
                    RVPercentual = 25.5,
                    RFPercentual = 74.5,
                    ValorTotalCalculado = 150000.00,
                    ValorTotalCarteira = 150000.00,
                    AssessorNome = "Maria Santos",
                    AssessorUnidade = "SP001",
                    Investimentos = new List<InvestimentoDto>()
                }
            };

            _mockRepository
                .Setup(r => r.GetCarteirasByRiscoAsync(tipoRisco))
                .ReturnsAsync(expectedCarteiras);

            // Act
            var result = await _service.GetCarteirasByRiscoAsync(tipoRisco);

            // Assert
            Assert.True(result.Success);
            var carteira = result.Data.First();
            Assert.Equal("123", carteira.ClienteId);
            Assert.Equal("João Silva", carteira.ClienteNome);
            Assert.Equal("12345678901", carteira.ClienteCpf);
            Assert.Equal(RiscoEnum.Conservador, carteira.RiscoDeclarado);
            Assert.Equal(RiscoEnum.Conservador, carteira.RiscoCalculado);
            Assert.Equal(25.5, carteira.RVPercentual);
            Assert.Equal(74.5, carteira.RFPercentual);
            Assert.Equal(150000.00, carteira.ValorTotalCalculado);
            Assert.Equal(150000.00, carteira.ValorTotalCarteira);
            Assert.Equal("Maria Santos", carteira.AssessorNome);
            Assert.Equal("SP001", carteira.AssessorUnidade);
            Assert.NotNull(carteira.Investimentos);
        }
    }
}