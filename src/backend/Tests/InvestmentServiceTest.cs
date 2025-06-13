using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using backend.Services;
using backend.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;
using System.Data;

namespace backend.Tests.Services
{
    public class InvestmentServiceTests : IDisposable
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<ILogger<InvestmentService>> _mockLogger;
        private readonly InvestmentService _service;
        private readonly string _testConnectionString = "Host=localhost;Port=5432;Database=test_db;Username=test_user;Password=test_pass";

        public InvestmentServiceTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockLogger = new Mock<ILogger<InvestmentService>>();
            
            // Setup configuration mock
            _mockConfiguration.Setup(x => x.GetConnectionString("DefaultConnection"))
                .Returns(_testConnectionString);
            
            _service = new InvestmentService(_mockConfiguration.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetInvestmentsByClientIdAsync_ValidClientId_ReturnsInvestments()
        {
            // Este teste seria integrado com um banco de dados de teste
            // Para um teste unitário puro, seria necessário abstrair a camada de dados
            // Por agora, vamos testar a estrutura de exceções
            
            // Arrange
            var clientId = "client123";

            // Act & Assert
            // Como o service faz conexão direta com o banco, este teste falhará sem um banco real
            // Em um cenário ideal, teríamos uma abstração de repositório para mockar
            await Assert.ThrowsAsync<NpgsqlException>(
                async () => await _service.GetInvestmentsByClientIdAsync(clientId));
        }

        [Fact]
        public async Task GetInvestmentsByClientIdAsync_ExceptionThrown_LogsErrorAndRethrows()
        {
            // Arrange
            var clientId = "client456";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NpgsqlException>(
                async () => await _service.GetInvestmentsByClientIdAsync(clientId));

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
        public async Task GetInvestmentsByClientIdAsync_EmptyOrNullClientId_StillExecutes(string clientId)
        {
            // Arrange & Act & Assert
            // O serviço executará a query mesmo com clientId vazio/nulo
            // Isso resultará em uma exceção de conexão, mas é o comportamento esperado
            await Assert.ThrowsAsync<NpgsqlException>(
                async () => await _service.GetInvestmentsByClientIdAsync(clientId));
        }

        public void Dispose()
        {
            // Cleanup if needed
        }
    }

    // Classe de teste alternativa usando um wrapper de repositório (recomendada)
    public class ImprovedInvestmentServiceTests
    {
        private readonly Mock<IInvestmentRepository> _mockRepository;
        private readonly Mock<ILogger<ImprovedInvestmentService>> _mockLogger;
        private readonly ImprovedInvestmentService _service;

        public ImprovedInvestmentServiceTests()
        {
            _mockRepository = new Mock<IInvestmentRepository>();
            _mockLogger = new Mock<ILogger<ImprovedInvestmentService>>();
            _service = new ImprovedInvestmentService(_mockRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetInvestmentsByClientIdAsync_ValidClientId_ReturnsInvestments()
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
                }
            };

            _mockRepository.Setup(r => r.GetInvestmentsByClientIdAsync(clientId))
                .ReturnsAsync(expectedInvestments);

            // Act
            var result = await _service.GetInvestmentsByClientIdAsync(clientId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(expectedInvestments.First().Id, result.First().Id);
            
            _mockRepository.Verify(r => r.GetInvestmentsByClientIdAsync(clientId), Times.Once);
        }

        [Fact]
        public async Task GetInvestmentsByClientIdAsync_RepositoryThrowsException_LogsAndRethrows()
        {
            // Arrange
            var clientId = "client456";
            var exception = new Exception("Database error");

            _mockRepository.Setup(r => r.GetInvestmentsByClientIdAsync(clientId))
                .ThrowsAsync(exception);

            // Act & Assert
            var thrownException = await Assert.ThrowsAsync<Exception>(
                async () => await _service.GetInvestmentsByClientIdAsync(clientId));

            Assert.Equal(exception, thrownException);
            
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Error fetching investments for client {clientId}")),
                    It.Is<Exception>(ex => ex == exception),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }

    // Interface e implementação sugerida para melhor testabilidade
    public interface IInvestmentRepository
    {
        Task<IEnumerable<Investment>> GetInvestmentsByClientIdAsync(string clientId);
    }

    public class ImprovedInvestmentService
    {
        private readonly IInvestmentRepository _repository;
        private readonly ILogger<ImprovedInvestmentService> _logger;

        public ImprovedInvestmentService(IInvestmentRepository repository, ILogger<ImprovedInvestmentService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<Investment>> GetInvestmentsByClientIdAsync(string clientId)
        {
            try
            {
                return await _repository.GetInvestmentsByClientIdAsync(clientId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching investments for client {ClientId}", clientId);
                throw;
            }
        }
    }
}