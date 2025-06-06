using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using backend.Models;
using backend.Services;

namespace backend.Tests
{
    public class ProfilesServiceTests
    {
        [Fact]
        public async Task SomaPerfisConformesPorCategoria_DeveRetornarSomaCorreta()
        {
            // Arrange
            var perfis = new List<Perfil>
            {
                new Perfil { PerfilInvestidor = "conservador", Conforme = true },
                new Perfil { PerfilInvestidor = "conservador", Conforme = false },
                new Perfil { PerfilInvestidor = "moderado", Conforme = true },
                new Perfil { PerfilInvestidor = "moderado", Conforme = true },
                new Perfil { PerfilInvestidor = "arrojado", Conforme = false }
            };
            var service = new ProfilesService();

            // Act
            var resultado = await service.SomaPerfisConformesPorCategoria(perfis);

            // Assert
            Assert.Collection(resultado,
                item => { Assert.Equal("conservador", item.PerfilInvestidor); Assert.Equal(1, item.TotalConformes); },
                item => { Assert.Equal("moderado", item.PerfilInvestidor); Assert.Equal(2, item.TotalConformes); },
                item => { Assert.Equal("arrojado", item.PerfilInvestidor); Assert.Equal(0, item.TotalConformes); }
            );
        }

        [Fact]
        public async Task SomaPerfisConformesPorCategoria_DeveRetornarVazio_SeNaoHouverPerfis()
        {
            // Arrange
            var perfis = new List<Perfil>();
            var service = new ProfilesService();

            // Act
            var resultado = await service.SomaPerfisConformesPorCategoria(perfis);

            // Assert
            Assert.Empty(resultado);
        }
    }
}
