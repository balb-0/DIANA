using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tests.Helpers;
using Xunit;

namespace backend.Tests.Integration
{
    public class InvestmentControllerIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public InvestmentControllerIntegrationTest(WebApplicationFactory<Program> factory)
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
                    // Remove o banco em memória e o service fake, usa o real
                    // Remove qualquer ApplicationDbContext e EfInvestmentService
                    var dbDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                    if (dbDescriptor != null) services.Remove(dbDescriptor);
                    var fakeService = services.SingleOrDefault(d => d.ImplementationType?.Name == "EfInvestmentService");
                    if (fakeService != null) services.Remove(fakeService);
                    // Mantém autenticação desabilitada para facilitar o teste
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
        public async Task GetInvestments_ReturnsInvestmentsForClient()
        {
            await SeedDatabaseAsync();
            var clientId = "01HXQK6Q1YXKH2QKSR89NR9A1W";
            var client = _factory.CreateClient();
            var response = await client.GetAsync($"/api/investment/{clientId}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var investments = await response.Content.ReadFromJsonAsync<List<Investment>>();
            Assert.NotNull(investments);
            Assert.True(investments.Count > 0); // Deve retornar os investimentos do clientId
        }

        [Fact]
        public async Task GetInvestments_ReturnsEmptyForNonexistentClient()
        {
            await SeedDatabaseAsync();
            var client = _factory.CreateClient();
            var response = await client.GetAsync($"/api/investment/naoexiste_{Guid.NewGuid()}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var investments = await response.Content.ReadFromJsonAsync<List<Investment>>();
            Assert.NotNull(investments);
            Assert.Empty(investments);
        }

        [Fact]
        public async Task GetInvestments_ReturnsServerErrorOnDbConnectionError()
        {
            // Simula erro de conexão alterando a connection string
            var factoryWithBadConn = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    var dict = new Dictionary<string, string?>
                    {
                        ["ConnectionStrings:DefaultConnection"] = "Host=localhost;Port=9999;Database=fail;Username=fail;Password=fail"
                    };
                    config.AddInMemoryCollection(dict);
                });
            });
            var client = factoryWithBadConn.CreateClient();
            var response = await client.GetAsync($"/api/investment/client123");
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            var msg = await response.Content.ReadAsStringAsync();
            Assert.Contains("error", msg, StringComparison.OrdinalIgnoreCase);
        }
    }
}
