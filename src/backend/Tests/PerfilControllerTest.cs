using Xunit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using backend.Controllers;
using backend.Services;
using backend.Models;
using System;

namespace backend.Tests.Controllers
{
    public class PerfilControllerTests
    {
        private readonly Mock<IPerfilService> _mockPerfilService;
        private readonly PerfilController _controller;

        public PerfilControllerTests()
        {
            _mockPerfilService = new Mock<IPerfilService>();
            _controller = new PerfilController(_mockPerfilService.Object);
        }

        [Fact]
        public void Constructor_ComServicoNulo_DeveLancarArgumentNullException()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentNullException>(() => new PerfilController(null!));
        }

        [Fact]
        public void GetPerfilDistribuicao_ComClienteValido_DeveRetornarOk()
        {
            // Arrange
            var clienteId = "cliente-123";
            var perfilDto = new PerfilDto
            {
                Perfil = "Conservador",
                Distribuicao = new DistribuicaoDto
                {
                    RendaFixa = 5000.0,
                    RendaVariavel = 2000.0
                }
            };

            _mockPerfilService.Setup(x => x.ObterPerfilDistribuicao(clienteId))
                            .Returns(perfilDto);

            // Act
            var resultado = _controller.GetPerfilDistribuicao(clienteId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var perfilRetornado = Assert.IsType<PerfilDto>(okResult.Value);
            Assert.Equal("Conservador", perfilRetornado.Perfil);
            Assert.Equal(5000.0, perfilRetornado.Distribuicao.RendaFixa);
            Assert.Equal(2000.0, perfilRetornado.Distribuicao.RendaVariavel);
        }

        [Fact]
        public void GetPerfilDistribuicao_ComPerfilNulo_DeveRetornarNotFound()
        {
            // Arrange
            var clienteId = "cliente-inexistente";
            
            _mockPerfilService.Setup(x => x.ObterPerfilDistribuicao(clienteId))
                            .Returns((PerfilDto)null!);

            // Act
            var resultado = _controller.GetPerfilDistribuicao(clienteId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(resultado);
            var response = notFoundResult.Value;
            Assert.NotNull(response);
        }

        [Theory]
        [InlineData("cliente-conservador", "Conservador", 8000.0, 2000.0)]
        [InlineData("cliente-moderado", "Moderado", 5000.0, 5000.0)]
        [InlineData("cliente-arrojado", "Arrojado", 2000.0, 8000.0)]
        [InlineData("cliente-desconhecido", "Desconhecido", 0.0, 0.0)]
        public void GetPerfilDistribuicao_ComDiferentesPerfis_DeveRetornarDadosCorretos(
            string clienteId, string perfil, double rendaFixa, double rendaVariavel)
        {
            // Arrange
            var perfilDto = new PerfilDto
            {
                Perfil = perfil,
                Distribuicao = new DistribuicaoDto
                {
                    RendaFixa = rendaFixa,
                    RendaVariavel = rendaVariavel
                }
            };

            _mockPerfilService.Setup(x => x.ObterPerfilDistribuicao(clienteId))
                            .Returns(perfilDto);

            // Act
            var resultado = _controller.GetPerfilDistribuicao(clienteId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var perfilRetornado = Assert.IsType<PerfilDto>(okResult.Value);
            Assert.Equal(perfil, perfilRetornado.Perfil);
            Assert.Equal(rendaFixa, perfilRetornado.Distribuicao.RendaFixa);
            Assert.Equal(rendaVariavel, perfilRetornado.Distribuicao.RendaVariavel);
        }

        [Fact]
        public void GetPerfilDistribuicao_ComExcecaoNoServico_DeveLancarExcecao()
        {
            // Arrange
            var clienteId = "cliente-erro";
            var mensagemErro = "Erro interno do serviço";

            _mockPerfilService.Setup(x => x.ObterPerfilDistribuicao(clienteId))
                            .Throws(new InvalidOperationException(mensagemErro));

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(
                () => _controller.GetPerfilDistribuicao(clienteId));
            Assert.Equal(mensagemErro, exception.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("cliente-especial-123")]
        [InlineData("CLIENTE-MAIUSCULO")]
        [InlineData("cliente_com_underscore")]
        public void GetPerfilDistribuicao_ComDiferentesFormatosClienteId_DeveProcessarCorretamente(string clienteId)
        {
            // Arrange
            var perfilDto = new PerfilDto
            {
                Perfil = "Conservador",
                Distribuicao = new DistribuicaoDto
                {
                    RendaFixa = 1000.0,
                    RendaVariavel = 500.0
                }
            };

            _mockPerfilService.Setup(x => x.ObterPerfilDistribuicao(clienteId))
                            .Returns(perfilDto);

            // Act
            var resultado = _controller.GetPerfilDistribuicao(clienteId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.NotNull(okResult.Value);
            _mockPerfilService.Verify(x => x.ObterPerfilDistribuicao(clienteId), Times.Once);
        }

        [Fact]
        public void GetPerfilDistribuicao_DeveChamarServicoUmaVez()
        {
            // Arrange
            var clienteId = "cliente-test";
            var perfilDto = new PerfilDto
            {
                Perfil = "Moderado",
                Distribuicao = new DistribuicaoDto
                {
                    RendaFixa = 3000.0,
                    RendaVariavel = 3000.0
                }
            };

            _mockPerfilService.Setup(x => x.ObterPerfilDistribuicao(clienteId))
                            .Returns(perfilDto);

            // Act
            _controller.GetPerfilDistribuicao(clienteId);

            // Assert
            _mockPerfilService.Verify(x => x.ObterPerfilDistribuicao(clienteId), Times.Once);
        }

        [Fact]
        public void GetPerfilDistribuicao_ComValoresZerados_DeveRetornarOk()
        {
            // Arrange
            var clienteId = "cliente-sem-investimentos";
            var perfilDto = new PerfilDto
            {
                Perfil = "Desconhecido",
                Distribuicao = new DistribuicaoDto
                {
                    RendaFixa = 0.0,
                    RendaVariavel = 0.0
                }
            };

            _mockPerfilService.Setup(x => x.ObterPerfilDistribuicao(clienteId))
                            .Returns(perfilDto);

            // Act
            var resultado = _controller.GetPerfilDistribuicao(clienteId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var perfilRetornado = Assert.IsType<PerfilDto>(okResult.Value);
            Assert.Equal(0.0, perfilRetornado.Distribuicao.RendaFixa);
            Assert.Equal(0.0, perfilRetornado.Distribuicao.RendaVariavel);
        }

        [Fact]
        public void GetPerfilDistribuicao_ComValoresAltos_DeveRetornarOk()
        {
            // Arrange
            var clienteId = "cliente-rico";
            var perfilDto = new PerfilDto
            {
                Perfil = "Arrojado",
                Distribuicao = new DistribuicaoDto
                {
                    RendaFixa = 100000.0,
                    RendaVariavel = 500000.0
                }
            };

            _mockPerfilService.Setup(x => x.ObterPerfilDistribuicao(clienteId))
                            .Returns(perfilDto);

            // Act
            var resultado = _controller.GetPerfilDistribuicao(clienteId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var perfilRetornado = Assert.IsType<PerfilDto>(okResult.Value);
            Assert.Equal(100000.0, perfilRetornado.Distribuicao.RendaFixa);
            Assert.Equal(500000.0, perfilRetornado.Distribuicao.RendaVariavel);
        }

        [Fact]
        public void GetPerfilDistribuicao_DeveRetornarStatusCode200()
        {
            // Arrange
            var clienteId = "cliente-status-test";
            var perfilDto = new PerfilDto
            {
                Perfil = "Conservador",
                Distribuicao = new DistribuicaoDto
                {
                    RendaFixa = 1500.0,
                    RendaVariavel = 500.0
                }
            };

            _mockPerfilService.Setup(x => x.ObterPerfilDistribuicao(clienteId))
                            .Returns(perfilDto);

            // Act
            var resultado = _controller.GetPerfilDistribuicao(clienteId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);
        }
    }
}