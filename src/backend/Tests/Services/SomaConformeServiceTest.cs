using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using SomaConformeApi.Services;
using SomaConformeApi.Models;
using Microsoft.Extensions.Configuration;

namespace SomaConformeApi.Tests.Services
{
    public class SomaConformeServiceTests
    {
        [Fact]
        public async Task SomaConformePorCategoriaAsync_DeveRetornarDadosCorretos()
        {
            // Given (Arrange): Um mock de IConfiguration configurado para retornar a string de conexão correta
            var mockConfiguration = new Mock<IConfiguration>();
            var mockSection = new Mock<IConfigurationSection>();
            // No teste, use:
            mockSection.Setup(x => x["DefaultConnection"])
                .Returns("Host=localhost;Database=btg_db;Username=btg_user;Password=btg_pass;Port=5432");

            mockConfiguration.Setup(x => x.GetSection("ConnectionStrings"))
                .Returns(mockSection.Object);

            var service = new SomaConformeService(mockConfiguration.Object);

            // When (Act): Chamando o método que retorna a soma conforme por categoria
            var resultado = await service.SomaConformePorCategoriaAsync();

            // Then (Assert): O resultado não é nulo e contém os perfis esperados
            Assert.NotNull(resultado);
            Assert.All(resultado, r => Assert.False(string.IsNullOrEmpty(r.PerfilInvestidor)));
        }
    }
}
