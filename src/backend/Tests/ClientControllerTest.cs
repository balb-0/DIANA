using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using backend.Controllers;
using backend.Services;

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
        public void GetPerfilDeRisco_WithValidId_ReturnsExpectedPerfil()
        {
            var clientId = "123";
            var expectedPerfil = "Conservador";

            _mockClientService.Setup(x => x.GetPerfilDeRisco(clientId))
                              .Returns(expectedPerfil);

            var result = _controller.GetPerfilDeRisco(clientId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            dynamic response = okResult.Value!;

            Assert.Equal(clientId, (string)response.id);
            Assert.Equal(expectedPerfil, (string)response.perfil_de_risco);
        }

        [Fact]
        public void GetPerfilDeRisco_WithUnknownId_ReturnsNotFound()
        {
            var clientId = "999";

            _mockClientService.Setup(x => x.GetPerfilDeRisco(clientId))
                              .Returns((string)null!);

            var result = _controller.GetPerfilDeRisco(clientId);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Cliente não encontrado.", notFoundResult.Value);
        }
    }
}
