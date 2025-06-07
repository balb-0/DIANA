using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using backend.Controllers;
using backend.Services;
using System.Collections.Generic;

namespace backend.Tests.Controllers
{
    public class ClientControllerTest
    {
        private readonly Mock<IClientService> _mockClientService;
        private readonly ClientController _controller;

        public ClientControllerTest()
        {
            _mockClientService = new Mock<IClientService>();
            _controller = new ClientController(_mockClientService.Object);
        }

        [Fact]
        public void GetPerfilDeRisco_ComIdValido_DeveRetornarPerfilEsperado()
        {
            // Given: um ID de cliente válido e um perfil esperado
            var clientId = "123";
            var expectedPerfil = "Conservador";
            _mockClientService.Setup(x => x.GetPerfilDeRisco(clientId)).Returns(expectedPerfil);

            // When: o método GetPerfilDeRisco é chamado no controller
            var result = _controller.GetPerfilDeRisco(clientId);

            // Then: deve retornar um resultado OK com o perfil esperado
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<Dictionary<string, string>>(okResult.Value);
            Assert.Equal(expectedPerfil, value["perfil_de_risco"]);
            Assert.Equal(clientId, value["id"]);
        }

        [Fact]
        public void GetPerfilDeRisco_ComClienteInexistente_DeveRetornarNotFound()
        {
            // Given: um ID de cliente inexistente
            var clientId = "999";
            _mockClientService.Setup(x => x.GetPerfilDeRisco(clientId)).Returns((string?)null);

            // When: o método GetPerfilDeRisco é chamado
            var result = _controller.GetPerfilDeRisco(clientId);

            // Then: deve retornar NotFound
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Cliente não encontrado.", notFoundResult.Value);
        }
    }
}
