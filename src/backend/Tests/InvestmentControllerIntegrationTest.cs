using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using backend;
using backend.Services;
using Moq;
using System.Net;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Authorization;
using backend.Models;
using Newtonsoft.Json;
using System.Text;

namespace backend.Tests.Integration
{
    public class InvestmentControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public InvestmentControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove o serviço real e adiciona o mock
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(IInvestmentService));
                    if (descriptor != null)
                        services.Remove(descriptor);

                    // Adiciona mock do serviço
                    var mockService = new Mock<IInvestmentService>();
                    services.AddSingleton(mockService.Object);

                    // Remove autenticação para testes
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            });

            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task GetInvestments_ValidClientId_ReturnsOkResponse()
        {
            // Arrange
            var clientId = "client123";
            var mockService = _factory.Services.GetRequiredService<IInvestmentService>();
            var mockServiceSetup = Mock.Get(mockService);
            
            var expectedInvestments = new List<Investment>
            {
                new Investment
                {
                    Id = "inv1",
                    AtivosId = "asset1",
                    ClientesId = clientId,
                    Quantidade = 100,
                    ValorTotal = 5000.00,
                    Data = DateTime.Now,
                    Ativo = new Asset
                    {
                        Id = "asset1",
                        Nome = "Ação XYZ",
                        Tipo = "Ação",
                        Subgrupo = "Blue Chip"
                    }
                }
            };

            mockServiceSetup.Setup(s => s.GetInvestmentsByClientIdAsync(clientId))
                .ReturnsAsync(expectedInvestments);

            // Act
            var response = await _client.GetAsync($"/api/Investment/{clientId}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var content = await response.Content.ReadAsStringAsync();
            var investments = JsonConvert.DeserializeObject<List<Investment>>(content);
            
            Assert.NotNull(investments);
            Assert.Single(investments);
            Assert.Equal("inv1", investments[0].Id);
        }

        [Fact]
        public async Task GetInvestments_ServiceThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            var clientId = "client456";
            var mockService = _factory.Services.GetRequiredService<IInvestmentService>();
            var mockServiceSetup = Mock.Get(mockService);

            mockServiceSetup.Setup(s => s.GetInvestmentsByClientIdAsync(clientId))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var response = await _client.GetAsync($"/api/Investment/{clientId}");

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("An error occurred while fetching investments", content);
        }

        [Fact]
        public async Task GetInvestments_EmptyClientId_ReturnsOkWithEmptyList()
        {
            // Arrange
            var clientId = "";
            var mockService = _factory.Services.GetRequiredService<IInvestmentService>();
            var mockServiceSetup = Mock.Get(mockService);

            mockServiceSetup.Setup(s => s.GetInvestmentsByClientIdAsync(clientId))
                .ReturnsAsync(new List<Investment>());

            // Act
            var response = await _client.GetAsync($"/api/Investment/{clientId}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var content = await response.Content.ReadAsStringAsync();
            var investments = JsonConvert.DeserializeObject<List<Investment>>(content);
            
            Assert.NotNull(investments);
            Assert.Empty(investments);
        }
    }

    // Classe helper para pular autenticação nos testes
    public class FakePolicyEvaluator : IPolicyEvaluator
    {
        public virtual async Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
        {
            var principal = new ClaimsPrincipal();
            principal.AddIdentity(new ClaimsIdentity(new[]
            {
                new Claim("sub", "test-user-id"),
                new Claim(ClaimTypes.Name, "Test User"),
            }, "FakeScheme"));

            return await Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(principal, 
                new AuthenticationProperties(), "FakeScheme")));
        }

        public virtual async Task<PolicyAuthorizationResult> AuthorizeAsync(AuthorizationPolicy policy, 
            AuthenticateResult authenticationResult, HttpContext context, object resource)
        {
            return await Task.FromResult(PolicyAuthorizationResult.Success());
        }
    }
}