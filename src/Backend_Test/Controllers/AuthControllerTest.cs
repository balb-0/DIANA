using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using backend.Controllers;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;


namespace backend_test.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly Mock<ILogger<AuthController>> _mockLogger;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockAuthService = new Mock<IAuthService>();
            _mockLogger = new Mock<ILogger<AuthController>>();
            _controller = new AuthController(_mockAuthService.Object, _mockLogger.Object);
        }

        #region Register Tests

        [Fact]
        public async Task Register_WithValidRequest_ReturnsOkResult()
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

            _mockAuthService.Setup(x => x.RegisterUserAsync(It.IsAny<RegisterRequest>()))
                           .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Register(request);

            // Assert
            var okResult = Assert.IsType<ActionResult<AuthResponse>>(result);
            var okObjectResult = Assert.IsType<OkObjectResult>(okResult.Result);
            var response = Assert.IsType<AuthResponse>(okObjectResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Usuário registrado com sucesso", response.Message);
            Assert.NotNull(response.User);
            Assert.Equal("test@example.com", response.User.Email);
        }

        [Fact]
        public async Task Register_WithInvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Email = "invalid-email",
                Password = "123" // Too short
            };

            _controller.ModelState.AddModelError("Email", "Invalid email format");
            _controller.ModelState.AddModelError("Password", "Password too short");

            // Act
            var result = await _controller.Register(request);

            // Assert
            var badRequestResult = Assert.IsType<ActionResult<AuthResponse>>(result);
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(badRequestResult.Result);
            var response = Assert.IsType<AuthResponse>(badRequestObjectResult.Value);

            Assert.False(response.Success);
            Assert.Equal("Dados inválidos", response.Message);
        }

        [Fact]
        public async Task Register_WhenServiceFails_ReturnsBadRequest()
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
                Success = false,
                Message = "Este email já está sendo usado por outra conta"
            };

            _mockAuthService.Setup(x => x.RegisterUserAsync(It.IsAny<RegisterRequest>()))
                           .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Register(request);

            // Assert
            var badRequestResult = Assert.IsType<ActionResult<AuthResponse>>(result);
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(badRequestResult.Result);
            var response = Assert.IsType<AuthResponse>(badRequestObjectResult.Value);

            Assert.False(response.Success);
            Assert.Equal("Este email já está sendo usado por outra conta", response.Message);
        }

        #endregion

        #region Login Tests

        [Fact]
        public async Task Login_WithValidCredentials_ReturnsOkResult()
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
                IdToken = "mock-id-token",
                User = new UserInfo
                {
                    Uid = "test-uid",
                    Email = "test@example.com",
                    DisplayName = "Test User",
                    EmailVerified = true
                }
            };

            _mockAuthService.Setup(x => x.LoginUserAsync(It.IsAny<LoginRequest>()))
                           .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Login(request);

            // Assert
            var okResult = Assert.IsType<ActionResult<AuthResponse>>(result);
            var okObjectResult = Assert.IsType<OkObjectResult>(okResult.Result);
            var response = Assert.IsType<AuthResponse>(okObjectResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Login bem-sucedido", response.Message);
            Assert.Equal("mock-id-token", response.IdToken);
            Assert.NotNull(response.User);
        }

        [Fact]
        public async Task Login_WithInvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "invalid-email",
                Password = ""
            };

            _controller.ModelState.AddModelError("Email", "Invalid email format");
            _controller.ModelState.AddModelError("Password", "Password is required");

            // Act
            var result = await _controller.Login(request);

            // Assert
            var badRequestResult = Assert.IsType<ActionResult<AuthResponse>>(result);
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(badRequestResult.Result);
            var response = Assert.IsType<AuthResponse>(badRequestObjectResult.Value);

            Assert.False(response.Success);
            Assert.Equal("Credenciais inválidas", response.Message);
        }

        [Fact]
        public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
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

            _mockAuthService.Setup(x => x.LoginUserAsync(It.IsAny<LoginRequest>()))
                           .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Login(request);

            // Assert
            var unauthorizedResult = Assert.IsType<ActionResult<AuthResponse>>(result);
            var unauthorizedObjectResult = Assert.IsType<UnauthorizedObjectResult>(unauthorizedResult.Result);
            var response = Assert.IsType<AuthResponse>(unauthorizedObjectResult.Value);

            Assert.False(response.Success);
            Assert.Equal("Senha incorreta", response.Message);
        }

        [Fact]
        public async Task Login_WithNonExistentUser_ReturnsUnauthorized()
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

            _mockAuthService.Setup(x => x.LoginUserAsync(It.IsAny<LoginRequest>()))
                           .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Login(request);

            // Assert
            var unauthorizedResult = Assert.IsType<ActionResult<AuthResponse>>(result);
            var unauthorizedObjectResult = Assert.IsType<UnauthorizedObjectResult>(unauthorizedResult.Result);
            var response = Assert.IsType<AuthResponse>(unauthorizedObjectResult.Value);

            Assert.False(response.Success);
            Assert.Equal("Usuário não encontrado", response.Message);
        }

        #endregion

        #region VerifyToken Tests

        [Fact]
        public async Task VerifyToken_WithValidToken_ReturnsOkResult()
        {
            // Arrange
            var request = new VerifyTokenRequest
            {
                Token = "valid-firebase-token"
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

            _mockAuthService.Setup(x => x.VerifyTokenAsync(It.IsAny<string>()))
                           .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.VerifyToken(request);

            // Assert
            var okResult = Assert.IsType<ActionResult<AuthResponse>>(result);
            var okObjectResult = Assert.IsType<OkObjectResult>(okResult.Result);
            var response = Assert.IsType<AuthResponse>(okObjectResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Token válido", response.Message);
            Assert.NotNull(response.User);
        }

        [Fact]
        public async Task VerifyToken_WithInvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var request = new VerifyTokenRequest
            {
                Token = ""
            };

            _controller.ModelState.AddModelError("Token", "Token is required");

            // Act
            var result = await _controller.VerifyToken(request);

            // Assert
            var badRequestResult = Assert.IsType<ActionResult<AuthResponse>>(result);
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(badRequestResult.Result);
            var response = Assert.IsType<AuthResponse>(badRequestObjectResult.Value);

            Assert.False(response.Success);
            Assert.Equal("Token é obrigatório", response.Message);
        }

        [Fact]
        public async Task VerifyToken_WithInvalidToken_ReturnsUnauthorized()
        {
            // Arrange
            var request = new VerifyTokenRequest
            {
                Token = "invalid-token"
            };

            var expectedResponse = new AuthResponse
            {
                Success = false,
                Message = "Token inválido ou expirado"
            };

            _mockAuthService.Setup(x => x.VerifyTokenAsync(It.IsAny<string>()))
                           .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.VerifyToken(request);

            // Assert
            var unauthorizedResult = Assert.IsType<ActionResult<AuthResponse>>(result);
            var unauthorizedObjectResult = Assert.IsType<UnauthorizedObjectResult>(unauthorizedResult.Result);
            var response = Assert.IsType<AuthResponse>(unauthorizedObjectResult.Value);

            Assert.False(response.Success);
            Assert.Equal("Token inválido ou expirado", response.Message);
        }

        [Fact]
        public async Task VerifyToken_WithExpiredToken_ReturnsUnauthorized()
        {
            // Arrange
            var request = new VerifyTokenRequest
            {
                Token = "expired-token"
            };

            var expectedResponse = new AuthResponse
            {
                Success = false,
                Message = "Token expirado"
            };

            _mockAuthService.Setup(x => x.VerifyTokenAsync(It.IsAny<string>()))
                           .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.VerifyToken(request);

            // Assert
            var unauthorizedResult = Assert.IsType<ActionResult<AuthResponse>>(result);
            var unauthorizedObjectResult = Assert.IsType<UnauthorizedObjectResult>(unauthorizedResult.Result);
            var response = Assert.IsType<AuthResponse>(unauthorizedObjectResult.Value);

            Assert.False(response.Success);
            Assert.Equal("Token expirado", response.Message);
        }

        #endregion

        #region GetProfile Test

        #endregion

        #region Service Interaction Tests

        [Fact]
        public async Task Register_CallsAuthServiceWithCorrectParameters()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Email = "test@example.com",
                Password = "password123",
                DisplayName = "Test User"
            };

            var expectedResponse = new AuthResponse { Success = true };
            _mockAuthService.Setup(x => x.RegisterUserAsync(It.IsAny<RegisterRequest>()))
                           .ReturnsAsync(expectedResponse);

            // Act
            await _controller.Register(request);

            // Assert
            _mockAuthService.Verify(x => x.RegisterUserAsync(It.Is<RegisterRequest>(r =>
                r.Email == "test@example.com" &&
                r.Password == "password123" &&
                r.DisplayName == "Test User")), Times.Once);
        }

        [Fact]
        public async Task Login_CallsAuthServiceWithCorrectParameters()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "test@example.com",
                Password = "password123"
            };

            var expectedResponse = new AuthResponse { Success = true };
            _mockAuthService.Setup(x => x.LoginUserAsync(It.IsAny<LoginRequest>()))
                           .ReturnsAsync(expectedResponse);

            // Act
            await _controller.Login(request);

            // Assert
            _mockAuthService.Verify(x => x.LoginUserAsync(It.Is<LoginRequest>(r =>
                r.Email == "test@example.com" &&
                r.Password == "password123")), Times.Once);
        }

        [Fact]
        public async Task VerifyToken_CallsAuthServiceWithCorrectParameters()
        {
            // Arrange
            var request = new VerifyTokenRequest
            {
                Token = "test-token"
            };

            var expectedResponse = new AuthResponse { Success = true };
            _mockAuthService.Setup(x => x.VerifyTokenAsync(It.IsAny<string>()))
                           .ReturnsAsync(expectedResponse);

            // Act
            await _controller.VerifyToken(request);

            // Assert
            _mockAuthService.Verify(x => x.VerifyTokenAsync("test-token"), Times.Once);
        }

        #endregion
    }
}