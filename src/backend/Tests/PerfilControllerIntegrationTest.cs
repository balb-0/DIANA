using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tests.Helpers;
using Xunit;

namespace backend.Tests.Integration
{
    public class PerfilControllerIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public PerfilControllerIntegrationTest(WebApplicationFactory<Program> factory)
        {
            var backendPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", ".."));
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.UseContentRoot(backendPath);
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    var dict = new Dictionary<string, string?>
                    {
                        ["ConnectionStrings:DefaultConnection"] = "Host=localhost;Port=5432;Database=btg_db;Username=btg_user;Password=btg_pass"
                    };
                    config.AddInMemoryCollection(dict);
                });
                builder.ConfigureServices(services =>
                {
                    var dbDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                    if (dbDescriptor != null) services.Remove(dbDescriptor);
                    services.AddSingleton<IAuthorizationHandler, AllowAnonymousAuthorizationHandler>();
                });
            });
        }

        private async Task SeedDatabaseAsync()
        {
            var repoRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));
            var sqlPath = Path.Combine(repoRoot, "database", "02_initial_data.sql");
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "psql",
                Arguments = $"-h localhost -U btg_user -d btg_db -f \"{sqlPath}\"",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                Environment = { ["PGPASSWORD"] = "btg_pass" }
            };
            using var process = System.Diagnostics.Process.Start(psi);
            if (process == null)
                throw new Exception("psql process could not be started");
            await process.WaitForExitAsync();
            if (process.ExitCode != 0)
            {
                var error = await process.StandardError.ReadToEndAsync();
                throw new Exception($"psql seed failed: {error}");
            }
        }

        [Fact]
        public async Task GetPerfilDistribuicao_ReturnsPerfilForClient()
        {
            await SeedDatabaseAsync();
            var clientId = "01HXQK6Q1YXKH2QKSR89NR9A1W";
            var client = _factory.CreateClient();
            var response = await client.GetAsync($"/api/perfil/{clientId}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var perfil = await response.Content.ReadFromJsonAsync<PerfilDto>();
            Assert.NotNull(perfil);
            Assert.False(string.IsNullOrEmpty(perfil.Perfil));
        }

        [Fact]
        public async Task GetPerfilDistribuicao_ReturnsNotFoundForNonexistentClient()
        {
            await SeedDatabaseAsync();
            var client = _factory.CreateClient();
            var response = await client.GetAsync($"/api/perfil/naoexiste_{Guid.NewGuid()}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var msg = await response.Content.ReadAsStringAsync();
            Assert.Contains("Cliente não encontrado", msg);
        }
    }
}
