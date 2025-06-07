using Xunit;
using Moq;
using SomaConformeApi.Controllers;
using SomaConformeApi.Services;
using SomaConformeApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SomaConformeApi.Tests.Controllers
{
    public class SomaConformeControllerTests
    {
        [Fact]
        public async Task GetSomaConformePorCategoria_ReturnsOkWithResult()
        {
            // Given (Arrange): Um mock do service configurado para retornar uma lista esperada
            var mockService = new Mock<ISomaConformeService>();
            var expected = new List<SomaConformeCategoriaDto>
            {
                new SomaConformeCategoriaDto { PerfilInvestidor = "Conservador", TotalConformes = 2 },
                new SomaConformeCategoriaDto { PerfilInvestidor = "Moderado", TotalConformes = 1 }
            };
            mockService.Setup(s => s.SomaConformePorCategoriaAsync()).ReturnsAsync(expected);

            var controller = new SomaConformeController(mockService.Object);

            // When (Act): Chamando o endpoint do controller
            var result = await controller.GetSomaConformePorCategoria();

            // Then (Assert): O resultado é OkObjectResult e o valor é igual ao esperado
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(expected, okResult.Value);
        }
    }
}
