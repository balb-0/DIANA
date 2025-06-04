using backend.Models.DTOs;
using backend.Models.Enums;
using backend.Repositories;
using Moq;
using System.Data;
using Xunit;
using Dapper;

namespace backend.Tests.Repositories
{
    public class CarteiraRepositoryTests
    {
        private readonly Mock<IDbConnection> _mockConnection;
        private readonly CarteiraRepository _repository;

        public CarteiraRepositoryTests()
        {
            _mockConnection = new Mock<IDbConnection>();
            _repository = new CarteiraRepository(_mockConnection.Object);
        }

        [Fact]
        public async Task GetCarteirasByRiscoAsync_ValidTipoRisco_ExecutesCorrectQuery()
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
            var expectedInvestimentos = new List<InvestimentoDto>();

            // Setup para a query principal
            _mockConnection
                .Setup(c => c.QueryAsync<CarteiraDto>(
                    It.Is<string>(sql => sql.Contains("SELECT DISTINCT") && sql.Contains("clientes c")),
                    It.Is<object>(param => param.GetType().GetProperty("TipoRisco").GetValue(param).ToString() == tipoRisco),
                    null, null, null))
                .ReturnsAsync(expectedCarteiras);

            // Setup para a query de investimentos
            _mockConnection
                .Setup(c => c.QueryAsync<InvestimentoDto>(
                    It.Is<string>(sql => sql.Contains("SELECT") && sql.Contains("investimento i")),
                    It.IsAny<object>(),
                    null, null, null))
                .ReturnsAsync(expectedInvestimentos);

            // Act
            var result = await _repository.GetCarteirasByRiscoAsync(tipoRisco);

            // Assert
            Assert.Single(result);
            Assert.Equal("João Silva", result.First().ClienteNome);

            // Verify que as queries foram executadas
            _mockConnection.Verify(c => c.QueryAsync<CarteiraDto>(
                It.Is<string>(sql => sql.Contains("WHERE c.is_active = true") && sql.Contains("AND LOWER(hrc.risco) = LOWER(@TipoRisco)")),
                It.IsAny<object>(),
                null, null, null), Times.Once);
        }

        [Fact]
        public async Task GetCarteirasByRiscoAsync_TipoRiscoGeral_DoesNotFilterByRisk()
        {
            // Arrange
            var tipoRisco = "geral";
            var expectedCarteiras = new List<CarteiraDto>
            {
                new CarteiraDto { ClienteId = "1", ClienteNome = "João Silva" },
                new CarteiraDto { ClienteId = "2", ClienteNome = "Maria Santos" }
            };

            _mockConnection
                .Setup(c => c.QueryAsync<CarteiraDto>(
                    It.Is<string>(sql => sql.Contains("SELECT DISTINCT") && !sql.Contains("AND LOWER(hrc.risco) = LOWER(@TipoRisco)")),
                    null,
                    null, null, null))
                .ReturnsAsync(expectedCarteiras);

            _mockConnection
                .Setup(c => c.QueryAsync<InvestimentoDto>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    null, null, null))
                .ReturnsAsync(new List<InvestimentoDto>());

            // Act
            var result = await _repository.GetCarteirasByRiscoAsync(tipoRisco);

            // Assert
            Assert.Equal(2, result.Count);

            // Verify que a query não contém filtro de risco
            _mockConnection.Verify(c => c.QueryAsync<CarteiraDto>(
                It.Is<string>(sql => sql.Contains("WHERE c.is_active = true") && !sql.Contains("AND LOWER(hrc.risco) = LOWER(@TipoRisco)")),
                null,
                null, null, null), Times.Once);
        }

        [Fact]
        public async Task GetCarteirasByRiscoAsync_LoadsInvestimentosForEachCarteira()
        {
            // Arrange
            var tipoRisco = "conservador";
            var expectedCarteiras = new List<CarteiraDto>
            {
                new CarteiraDto { ClienteId = "1", ClienteNome = "João" },
                new CarteiraDto { ClienteId = "2", ClienteNome = "Maria" }
            };
            var expectedInvestimentos = new List<InvestimentoDto>
            {
                new InvestimentoDto { InvestimentoId = "1", AtivoNome = "Tesouro Direto" }
            };

            _mockConnection
                .Setup(c => c.QueryAsync<CarteiraDto>(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
                .ReturnsAsync(expectedCarteiras);

            _mockConnection
                .Setup(c => c.QueryAsync<InvestimentoDto>(
                    It.Is<string>(sql => sql.Contains("investimento i") && sql.Contains("WHERE i.clientes_id = @ClienteId")),
                    It.IsAny<object>(),
                    null, null, null))
                .ReturnsAsync(expectedInvestimentos);

            // Act
            var result = await _repository.GetCarteirasByRiscoAsync(tipoRisco);

            // Assert
            Assert.Equal(2, result.Count);

            // Verify que a query de investimentos foi chamada para cada carteira
            _mockConnection.Verify(c => c.QueryAsync<InvestimentoDto>(
                It.Is<string>(sql => sql.Contains("investimento i")),
                It.IsAny<object>(),
                null, null, null), Times.Exactly(2));
        }

        [Fact]
        public async Task GetCarteirasByRiscoAsync_EmptyResult_ReturnsEmptyList()
        {
            // Arrange
            var tipoRisco = "conservador";
            var emptyCarteiras = new List<CarteiraDto>();

            _mockConnection
                .Setup(c => c.QueryAsync<CarteiraDto>(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
                .ReturnsAsync(emptyCarteiras);

            // Act
            var result = await _repository.GetCarteirasByRiscoAsync(tipoRisco);

            // Assert
            Assert.Empty(result);
        }

        [Theory]
        [InlineData("conservador")]
        [InlineData("moderado")]
        [InlineData("sofisticado")]
        public async Task GetCarteirasByRiscoAsync_SpecificRiskTypes_UsesParameterizedQuery(string tipoRisco)
        {
            // Arrange
            _mockConnection
                .Setup(c => c.QueryAsync<CarteiraDto>(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
                .ReturnsAsync(new List<CarteiraDto>());

            // Act
            await _repository.GetCarteirasByRiscoAsync(tipoRisco);

            // Assert
            _mockConnection.Verify(c => c.QueryAsync<CarteiraDto>(
                It.Is<string>(sql => sql.Contains("@TipoRisco")),
                It.Is<object>(param => param.GetType().GetProperty("TipoRisco").GetValue(param).ToString() == tipoRisco),
                null, null, null), Times.Once);
        }
    }
}