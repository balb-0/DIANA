using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using backend.Controllers;
using backend.Models;
using backend.Services;

namespace backend.Tests
{
    public class ProfilesControllerTests
    {
        [Fact]
        public async Task GetConformingByCategory_DeveRetornarOkComResultado()
        {
            // Arrange
            var mockService = new Mock<IProfilesService>();
            var perfis = new List<Perfil>
            {
                new Perfil { PerfilInvestidor = "conservador", Conforme = true }
            };
            var resultadoEsperado = new List<ProfileCategoryCount>
            {
                new ProfileCategoryCount { PerfilInvestidor = "conservador", TotalConformes = 1 },
                new ProfileCategoryCount { PerfilInvestidor = "moderado", TotalConformes = 0 },
                new ProfileCategoryCount { PerfilInvestidor = "arrojado", TotalConformes = 0 }
            };
            mockService.Setup(s => s.SomaPerfisConformesPorCategoria(perfis))
                .ReturnsAsync(resultadoEsperado);

            var controller = new ProfilesController(mockService.Object);

            // Act
            var result = await controller.GetConformingByCategory(perfis);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(resultadoEsperado, okResult.Value);
        }
    }
}
