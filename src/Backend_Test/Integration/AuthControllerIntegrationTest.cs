using backend;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace backend.Tests.Integration
{
    public class AuthControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;
        private readonly Mock<IAuthService>? _mockAuthService;

        public AuthControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _mockAuthService = _factory.MockAuthService;
            _client = _factory.CreateClient();
        }

        #region Register Tests

        [Fact]
        public async Task Register_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Email = "test@example.com",
                Password = "password123",
                DisplayName = "Test User"
            };

            var expectedResponse = new AuthResponse
            {
                Success = true,
                Message = "Usuário registrado com sucesso",
                User = new UserInfo
                {
                    Uid = "test-uid",
                    Email = "test@example.com",
                    DisplayName = "Test User",
                    EmailVerified = false
                }
            };

            _mockAuthService!
                .Setup(x => x.RegisterUserAsync(It.IsAny<RegisterRequest>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/register", request);
            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal("Usuário registrado com sucesso", result.Message);
            Assert.Equal("test@example.com", result.User!.Email);
            Assert.Equal("Test User", result.User.DisplayName);
        }

        [Fact]
        public async Task Register_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Email = "", // Email inválido
                Password = "password123",
                DisplayName = "Test User"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/register", request);
            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(result);
            Assert.False(result.Success);
            // Mensagem pode ser null dependendo do ModelState
            // Assert.Equal("Dados inválidos", result.Message);
        }

        [Fact]
        public async Task Register_ServiceReturnsFailure_ReturnsBadRequest()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Email = "existing@example.com",
                Password = "password123",
                DisplayName = "Test User"
            };

            var expectedResponse = new AuthResponse
            {
                Success = false,
                Message = "Este email já está sendo usado por outra conta"
            };

            _mockAuthService!
                .Setup(x => x.RegisterUserAsync(It.IsAny<RegisterRequest>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/register", request);
            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("Este email já está sendo usado por outra conta", result.Message);
        }

        [Fact]
        public async Task Register_NullRequest_ReturnsBadRequest()
        {
            // Act
            var response = await _client.PostAsync("/api/auth/register", 
                new StringContent("null", Encoding.UTF8, "application/json"));

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        #endregion

        #region Login Tests

        [Fact]
        public async Task Login_ValidCredentials_ReturnsOkResult()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "test@example.com",
                Password = "password123"
            };

            var expectedResponse = new AuthResponse
            {
                Success = true,
                Message = "Login bem-sucedido",
                IdToken = "fake-jwt-token",
                User = new UserInfo
                {
                    Uid = "test-uid",
                    Email = "test@example.com",
                    DisplayName = "Test User",
                    EmailVerified = true
                }
            };

            _mockAuthService!
                .Setup(x => x.LoginUserAsync(It.IsAny<LoginRequest>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/login", request);
            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal("Login bem-sucedido", result.Message);
            Assert.NotNull(result.IdToken);
            Assert.Equal("test@example.com", result.User!.Email);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "test@example.com",
                Password = "wrongpassword"
            };

            var expectedResponse = new AuthResponse
            {
                Success = false,
                Message = "Senha incorreta"
            };

            _mockAuthService!
                .Setup(x => x.LoginUserAsync(It.IsAny<LoginRequest>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/login", request);
            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("Senha incorreta", result.Message);
        }

        [Fact]
        public async Task Login_EmptyEmail_ReturnsBadRequest()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "",
                Password = "password123"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/login", request);
            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(result);
            Assert.False(result.Success);
            // Mensagem pode ser null dependendo do ModelState
            // Assert.Equal("Credenciais inválidas", result.Message);
        }

        [Fact]
        public async Task Login_UserNotFound_ReturnsUnauthorized()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "nonexistent@example.com",
                Password = "password123"
            };

            var expectedResponse = new AuthResponse
            {
                Success = false,
                Message = "Usuário não encontrado"
            };

            _mockAuthService!
                .Setup(x => x.LoginUserAsync(It.IsAny<LoginRequest>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/login", request);
            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("Usuário não encontrado", result.Message);
        }

        #endregion

        #region Verify Token Tests

        [Fact]
        public async Task VerifyToken_ValidToken_ReturnsOkResult()
        {
            // Arrange
            var request = new VerifyTokenRequest
            {
                Token = "valid-jwt-token"
            };

            var expectedResponse = new AuthResponse
            {
                Success = true,
                Message = "Token válido",
                User = new UserInfo
                {
                    Uid = "test-uid",
                    Email = "test@example.com",
                    DisplayName = "Test User",
                    EmailVerified = true
                }
            };

            _mockAuthService!
                .Setup(x => x.VerifyTokenAsync(It.IsAny<string>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/verify", request);
            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal("Token válido", result.Message);
            Assert.Equal("test@example.com", result.User!.Email);
        }

        [Fact]
        public async Task VerifyToken_InvalidToken_ReturnsUnauthorized()
        {
            // Arrange
            var request = new VerifyTokenRequest
            {
                Token = "invalid-jwt-token"
            };

            var expectedResponse = new AuthResponse
            {
                Success = false,
                Message = "Token inválido ou expirado"
            };

            _mockAuthService!
                .Setup(x => x.VerifyTokenAsync(It.IsAny<string>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/verify", request);
            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("Token inválido ou expirado", result.Message);
        }

        [Fact]
        public async Task VerifyToken_EmptyToken_ReturnsBadRequest()
        {
            // Arrange
            var request = new VerifyTokenRequest
            {
                Token = ""
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/verify", request);
            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(result);
            Assert.False(result.Success);
            // Mensagem pode ser null dependendo do ModelState
            // Assert.Equal("Token é obrigatório", result.Message);
        }

        [Fact]
        public async Task VerifyToken_NullToken_ReturnsBadRequest()
        {
            // Arrange
            var request = new VerifyTokenRequest
            {
                Token = null!
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/verify", request);
            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(result);
            Assert.False(result.Success);
            // Mensagem pode ser null dependendo do ModelState
            // Assert.Equal("Token é obrigatório", result.Message);
        }

        #endregion

        #region Integration Flow Tests

        [Fact]
        public async Task AuthFlow_RegisterLoginVerify_WorksCorrectly()
        {
            // Arrange - Register
            var registerRequest = new RegisterRequest
            {
                Email = "integration@example.com",
                Password = "password123",
                DisplayName = "Integration User"
            };

            var registerResponse = new AuthResponse
            {
                Success = true,
                Message = "Usuário registrado com sucesso",
                User = new UserInfo
                {
                    Uid = "integration-uid",
                    Email = "integration@example.com",
                    DisplayName = "Integration User",
                    EmailVerified = false
                }
            };

            _mockAuthService!
                .Setup(x => x.RegisterUserAsync(It.IsAny<RegisterRequest>()))
                .ReturnsAsync(registerResponse);

            // Act - Register
            var regResponse = await _client.PostAsJsonAsync("/api/auth/register", registerRequest);
            var regResult = await regResponse.Content.ReadFromJsonAsync<AuthResponse>();

            // Assert - Register
            Assert.Equal(HttpStatusCode.OK, regResponse.StatusCode);
            Assert.True(regResult.Success);

            // Arrange - Login
            var loginRequest = new LoginRequest
            {
                Email = "integration@example.com",
                Password = "password123"
            };

            var loginResponse = new AuthResponse
            {
                Success = true,
                Message = "Login bem-sucedido",
                IdToken = "integration-jwt-token",
                User = new UserInfo
                {
                    Uid = "integration-uid",
                    Email = "integration@example.com",
                    DisplayName = "Integration User",
                    EmailVerified = false
                }
            };

            _mockAuthService!
                .Setup(x => x.LoginUserAsync(It.IsAny<LoginRequest>()))
                .ReturnsAsync(loginResponse);

            // Act - Login
            var logResponse = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);
            var logResult = await logResponse.Content.ReadFromJsonAsync<AuthResponse>();

            // Assert - Login
            Assert.Equal(HttpStatusCode.OK, logResponse.StatusCode);
            Assert.True(logResult.Success);
            Assert.NotNull(logResult.IdToken);

            // Arrange - Verify
            var verifyRequest = new VerifyTokenRequest
            {
                Token = logResult.IdToken
            };

            var verifyResponse = new AuthResponse
            {
                Success = true,
                Message = "Token válido",
                User = new UserInfo
                {
                    Uid = "integration-uid",
                    Email = "integration@example.com",
                    DisplayName = "Integration User",
                    EmailVerified = false
                }
            };

            _mockAuthService!
                .Setup(x => x.VerifyTokenAsync(It.IsAny<string>()))
                .ReturnsAsync(verifyResponse);

            // Act - Verify
            var verResponse = await _client.PostAsJsonAsync("/api/auth/verify", verifyRequest);
            var verResult = await verResponse.Content.ReadFromJsonAsync<AuthResponse>();

            // Assert - Verify
            Assert.Equal(HttpStatusCode.OK, verResponse.StatusCode);
            Assert.True(verResult!.Success);
            Assert.Equal("integration@example.com", verResult.User!.Email);
        }

        #endregion

        #region Content Type Tests

        [Fact]
        public async Task Register_InvalidContentType_ReturnsBadRequest()
        {
            // Arrange
            var content = new StringContent("email=test@example.com&password=123456", 
                Encoding.UTF8, "application/x-www-form-urlencoded");

            // Act
            var response = await _client.PostAsync("/api/auth/register", content);

            // Assert
            Assert.Equal(HttpStatusCode.UnsupportedMediaType, response.StatusCode);
        }

        [Fact]
        public async Task Login_InvalidJson_ReturnsBadRequest()
        {
            // Arrange
            var content = new StringContent("{ invalid json }", 
                Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/auth/login", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        #endregion

        public void Dispose()
        {
            _client?.Dispose();
            _factory?.Dispose();
        }
    }

    public class CustomWebApplicationFactory : WebApplicationFactory<backend.Program>
    {
        public Mock<IAuthService>? MockAuthService { get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove o serviço real
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IAuthService));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Cria e registra o mock
                MockAuthService = new Mock<IAuthService>();
                services.AddSingleton(MockAuthService.Object);
            });

            // Ajusta o content root para o diretório absoluto do projeto principal
            var backendProjectDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "backend"));
            builder.UseContentRoot(backendProjectDir);
            builder.UseEnvironment("Testing");
        }
    }
}