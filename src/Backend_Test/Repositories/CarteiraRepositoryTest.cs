using backend.Models.DTOs;
using backend.Repositories;
using Dapper;
using Moq;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace backend_test.Repositories
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
        public async Task GetClientesByRiscoDeclaradoAsync_WithSpecificRisk_FiltersByRisk()
        {
            // Arrange
            var tipoRisco = "conservador";
            var assessorId = "assessor123";
            var expectedClientes = new List<ClienteSimplificadoDto>
            {
                new ClienteSimplificadoDto { ClienteId = "1", ClienteNome = "João Silva" }
            };

            _mockConnection
                .Setup(c => c.QueryAsync<ClienteSimplificadoDto>(
                    It.Is<string>(sql =>
                        sql.Contains("WHERE c.is_active = true") &&
                        sql.Contains("AND c.assessores_id = @AssessorId") &&
                        sql.Contains("AND UPPER(hrd.risco::text) = UPPER(@TipoRisco)")),
                     It.Is<object>(param =>
                        param.GetType().GetProperty("AssessorId") != null &&
                        param.GetType().GetProperty("TipoRisco") != null &&
                        (string)param.GetType().GetProperty("AssessorId").GetValue(param) == assessorId &&
                        (string)param.GetType().GetProperty("TipoRisco").GetValue(param) == tipoRisco
                    ),
                    null, null, null))
                .ReturnsAsync(expectedClientes);

            // Act
            var result = await _repository.GetClientesByRiscoDeclaradoAsync(tipoRisco, assessorId);

            // Assert
            Assert.Single(result);
            Assert.Equal("João Silva", result.First().ClienteNome);
        }

        [Fact]
        public async Task GetClientesByRiscoDeclaradoAsync_WithGeralRisk_DoesNotFilterByRisk()
        {
            // Arrange
            var tipoRisco = "geral";
            var assessorId = "assessor123";
            var expectedClientes = new List<ClienteSimplificadoDto>
            {
                new ClienteSimplificadoDto { ClienteId = "1", ClienteNome = "João Silva" },
                new ClienteSimplificadoDto { ClienteId = "2", ClienteNome = "Maria Santos" }
            };

            _mockConnection
                .Setup(c => c.QueryAsync<ClienteSimplificadoDto>(
                    It.Is<string>(sql =>
                        sql.Contains("WHERE c.is_active = true") &&
                        sql.Contains("AND c.assessores_id = @AssessorId") &&
                        !sql.Contains("UPPER(hrd.risco::text) = UPPER(@TipoRisco)")),
                    It.Is<object>(param =>
                        (string)param.GetType().GetProperty("AssessorId").GetValue(param) == assessorId &&
                        param.GetType().GetProperty("TipoRisco") == null),
                    null, null, null))
                .ReturnsAsync(expectedClientes);

            // Act
            var result = await _repository.GetClientesByRiscoDeclaradoAsync(tipoRisco, assessorId);

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Theory]
        [InlineData("conservador")]
        [InlineData("moderado")]
        [InlineData("sofisticado")]
        public async Task GetClientesByRiscoDeclaradoAsync_SpecificRisk_UsesParameterizedQuery(string tipoRisco)
        {
            // Arrange
            var assessorId = "assessor123";

            _mockConnection
                .Setup(c => c.QueryAsync<ClienteSimplificadoDto>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    null, null, null))
                .ReturnsAsync(new List<ClienteSimplificadoDto>());

            // Act
            await _repository.GetClientesByRiscoDeclaradoAsync(tipoRisco, assessorId);

            // Assert
            _mockConnection.Verify(c => c.QueryAsync<ClienteSimplificadoDto>(
                It.Is<string>(sql => sql.Contains("@TipoRisco")),
                It.Is<object>(param =>
                    param.GetType().GetProperty("AssessorId") != null &&
                    param.GetType().GetProperty("TipoRisco") != null &&
                    (string)param.GetType().GetProperty("AssessorId").GetValue(param) == assessorId &&
                    (string)param.GetType().GetProperty("TipoRisco").GetValue(param) == tipoRisco
                ),
                null, null, null), Times.Once);
        }

        [Fact]
        public async Task GetClientesByRiscoDeclaradoAsync_NoResults_ReturnsEmptyList()
        {
            // Arrange
            var tipoRisco = "conservador";
            var assessorId = "assessor123";

            _mockConnection
                .Setup(c => c.QueryAsync<ClienteSimplificadoDto>(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
                .ReturnsAsync(new List<ClienteSimplificadoDto>());

            // Act
            var result = await _repository.GetClientesByRiscoDeclaradoAsync(tipoRisco, assessorId);

            // Assert
            Assert.Empty(result);
        }
    }
}
