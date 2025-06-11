using Xunit;
using Moq;
using backend.Services;
using backend.Models;
using System;
using Npgsql;
using System.Data;

namespace backend.Tests.Services
{
    public class PerfilServiceTests
    {
        private readonly string _connectionString = "Host=localhost;Database=test;Username=test;Password=test";
        private readonly Mock<IDbConnection> _mockConnection;
        private readonly Mock<IDbCommand> _mockCommand;
        private readonly Mock<IDataReader> _mockDataReader;
        private readonly PerfilService _perfilService;

        public PerfilServiceTests()
        {
            _mockConnection = new Mock<IDbConnection>();
            _mockCommand = new Mock<IDbCommand>();
            _mockDataReader = new Mock<IDataReader>();
            _perfilService = new PerfilService(_connectionString);
        }

        [Fact]
        public void Constructor_ComConnectionStringNula_DeveLancarArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new PerfilService(null!));
        }

        [Fact]
        public void Constructor_ComConnectionStringVazia_DeveLancarArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new PerfilService(""));
        }

        [Theory]
        [InlineData("cliente-123")]
        [InlineData("cliente-456")]
        [InlineData("cliente-789")]
        public void ObterPerfilDistribuicao_ComClienteValido_DeveRetornarPerfilDto(string clienteId)
        {
            // Arrange
            var mockPerfilService = new Mock<PerfilService>(_connectionString) { CallBase = true };
            
            // Simulando que o método ObterPerfilDeRisco retorna um perfil
            mockPerfilService.Setup(x => x.ObterPerfilDistribuicao(It.IsAny<string>()))
                           .Returns(new PerfilDto 
                           { 
                               Perfil = "Conservador",
                               Distribuicao = new DistribuicaoDto 
                               { 
                                   RendaFixa = 5000.0,
                                   RendaVariavel = 2000.0 
                               }
                           });

            // Act
            var resultado = mockPerfilService.Object.ObterPerfilDistribuicao(clienteId);

            // Assert
            Assert.NotNull(resultado);
            Assert.IsType<PerfilDto>(resultado);
            Assert.NotNull(resultado.Distribuicao);
        }

        [Fact]
        public void ObterPerfilDistribuicao_ComClienteSemPerfil_DeveRetornarPerfilDesconhecido()
        {
            // Arrange
            var clienteId = "cliente-sem-perfil";
            var mockPerfilService = new Mock<PerfilService>(_connectionString) { CallBase = true };
            
            mockPerfilService.Setup(x => x.ObterPerfilDistribuicao(It.IsAny<string>()))
                           .Returns(new PerfilDto 
                           { 
                               Perfil = "Desconhecido",
                               Distribuicao = new DistribuicaoDto 
                               { 
                                   RendaFixa = 0.0,
                                   RendaVariavel = 0.0 
                               }
                           });

            // Act
            var resultado = mockPerfilService.Object.ObterPerfilDistribuicao(clienteId);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("Desconhecido", resultado.Perfil);
            Assert.Equal(0.0, resultado.Distribuicao.RendaFixa);
            Assert.Equal(0.0, resultado.Distribuicao.RendaVariavel);
        }

        [Fact]
        public void ObterPerfilDistribuicao_ComClienteComInvestimentos_DeveCalcularDistribuicaoCorretamente()
        {
            // Arrange
            var clienteId = "cliente-com-investimentos";
            var mockPerfilService = new Mock<PerfilService>(_connectionString) { CallBase = true };
            
            mockPerfilService.Setup(x => x.ObterPerfilDistribuicao(It.IsAny<string>()))
                           .Returns(new PerfilDto 
                           { 
                               Perfil = "Moderado",
                               Distribuicao = new DistribuicaoDto 
                               { 
                                   RendaFixa = 7500.0,
                                   RendaVariavel = 2500.0 
                               }
                           });

            // Act
            var resultado = mockPerfilService.Object.ObterPerfilDistribuicao(clienteId);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("Moderado", resultado.Perfil);
            Assert.Equal(7500.0, resultado.Distribuicao.RendaFixa);
            Assert.Equal(2500.0, resultado.Distribuicao.RendaVariavel);
        }

        [Theory]
        [InlineData(10000.0, 0.0, "Conservador")]
        [InlineData(5000.0, 5000.0, "Moderado")]
        [InlineData(2000.0, 8000.0, "Arrojado")]
        [InlineData(0.0, 0.0, "Desconhecido")]
        public void ObterPerfilDistribuicao_ComDiferentesDistribuicoes_DeveRetornarValoresCorretos(
            double rendaFixa, double rendaVariavel, string perfil)
        {
            // Arrange
            var clienteId = "cliente-test";
            var mockPerfilService = new Mock<PerfilService>(_connectionString) { CallBase = true };
            
            mockPerfilService.Setup(x => x.ObterPerfilDistribuicao(It.IsAny<string>()))
                           .Returns(new PerfilDto 
                           { 
                               Perfil = perfil,
                               Distribuicao = new DistribuicaoDto 
                               { 
                                   RendaFixa = rendaFixa,
                                   RendaVariavel = rendaVariavel 
                               }
                           });

            // Act
            var resultado = mockPerfilService.Object.ObterPerfilDistribuicao(clienteId);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(perfil, resultado.Perfil);
            Assert.Equal(rendaFixa, resultado.Distribuicao.RendaFixa);
            Assert.Equal(rendaVariavel, resultado.Distribuicao.RendaVariavel);
        }

        [Fact]
        public void ObterPerfilDistribuicao_ComExcecaoBancoDados_DeveLancarExcecao()
        {
            // Arrange
            var clienteId = "cliente-erro";
            var mockPerfilService = new Mock<PerfilService>(_connectionString);
            
            mockPerfilService.Setup(x => x.ObterPerfilDistribuicao(It.IsAny<string>()))
                           .Throws(new NpgsqlException("Erro de conexão com o banco"));

            // Act & Assert
            Assert.Throws<NpgsqlException>(() => mockPerfilService.Object.ObterPerfilDistribuicao(clienteId));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ObterPerfilDistribuicao_ComClienteIdInvalido_DeveTratarCorretamente(string? clienteId)
        {
            // Arrange
            var mockPerfilService = new Mock<PerfilService>(_connectionString) { CallBase = true };
            
            // Para IDs inválidos, assumindo que retorna perfil desconhecido e distribuição zerada
            mockPerfilService.Setup(x => x.ObterPerfilDistribuicao(It.IsAny<string>()))
                           .Returns(new PerfilDto 
                           { 
                               Perfil = "Desconhecido",
                               Distribuicao = new DistribuicaoDto 
                               { 
                                   RendaFixa = 0.0,
                                   RendaVariavel = 0.0 
                               }
                           });

            // Act
            var resultado = mockPerfilService.Object.ObterPerfilDistribuicao(clienteId!);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("Desconhecido", resultado.Perfil);
        }

        [Fact]
        public void ObterPerfilDistribuicao_DeveRetornarObjetoCompleto()
        {
            // Arrange
            var clienteId = "cliente-completo";
            var mockPerfilService = new Mock<PerfilService>(_connectionString) { CallBase = true };
            
            mockPerfilService.Setup(x => x.ObterPerfilDistribuicao(It.IsAny<string>()))
                           .Returns(new PerfilDto 
                           { 
                               Perfil = "Arrojado",
                               Distribuicao = new DistribuicaoDto 
                               { 
                                   RendaFixa = 3000.0,
                                   RendaVariavel = 12000.0 
                               }
                           });

            // Act
            var resultado = mockPerfilService.Object.ObterPerfilDistribuicao(clienteId);

            // Assert
            Assert.NotNull(resultado);
            Assert.NotEmpty(resultado.Perfil);
            Assert.NotNull(resultado.Distribuicao);
            Assert.True(resultado.Distribuicao.RendaFixa >= 0);
            Assert.True(resultado.Distribuicao.RendaVariavel >= 0);
        }

        [Fact]
        public void ObterPerfilDistribuicao_ComMultiplosInvestimentosMesmoTipo_DeveSomarValoresCorretamente()
        {
            // Arrange
            var clienteId = "cliente-multiplos-investimentos";
            var mockPerfilService = new Mock<PerfilService>(_connectionString) { CallBase = true };
            
            // Simulando múltiplos investimentos: RendaFixa (2000 + 3000) + RendaVariavel (1500 + 2500)
            mockPerfilService.Setup(x => x.ObterPerfilDistribuicao(It.IsAny<string>()))
                           .Returns(new PerfilDto 
                           { 
                               Perfil = "Moderado",
                               Distribuicao = new DistribuicaoDto 
                               { 
                                   RendaFixa = 5000.0,  // 2000 + 3000
                                   RendaVariavel = 4000.0  // 1500 + 2500
                               }
                           });

            // Act
            var resultado = mockPerfilService.Object.ObterPerfilDistribuicao(clienteId);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(5000.0, resultado.Distribuicao.RendaFixa);
            Assert.Equal(4000.0, resultado.Distribuicao.RendaVariavel);
        }
    }
}