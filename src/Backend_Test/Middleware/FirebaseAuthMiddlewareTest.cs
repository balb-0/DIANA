using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using backend.Middleware;
using backend.Models;
using backend.Services;
using System.Text;
using System.Threading.Tasks;
using System.IO;




namespace backend_test.Middleware
{
    public class FirebaseAuthMiddlewareTests
    {
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly Mock<ILogger<FirebaseAuthMiddleware>> _mockLogger;
        private readonly Mock<RequestDelegate> _mockNext;
        private readonly FirebaseAuthMiddleware _middleware;

        public FirebaseAuthMiddlewareTests()
        {
            _mockAuthService = new Mock<IAuthService>();
            _mockLogger = new Mock<ILogger<FirebaseAuthMiddleware>>();
            _mockNext = new Mock<RequestDelegate>();
            _middleware = new FirebaseAuthMiddleware(_mockNext.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task InvokeAsync_WithoutAuthorizeAttribute_CallsNextDelegate()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            var endpoint = new Endpoint(null, EndpointMetadataCollection.Empty, "test");
            httpContext.SetEndpoint(endpoint);

            // Act
            await _middleware.InvokeAsync(httpContext, _mockAuthService.Object);

            // Assert
            _mockNext.Verify(x => x(httpContext), Times.Once);
            _mockAuthService.Verify(x => x.VerifyTokenAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task InvokeAsync_WithAuthorizeAttributeAndValidToken_CallsNextDelegate()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            var metadata = new EndpointMetadataCollection(new AuthorizeAttribute());
            var endpoint = new Endpoint(null, metadata, "test");
            httpContext.SetEndpoint(endpoint);
            httpContext.Request.Headers["Authorization"] = "Bearer valid-token";

            var authResponse = new AuthResponse
            {
                Success = true,
                User = new UserInfo
                {
                    Uid = "test-uid",
                    Email = "test@example.com",
                    DisplayName = "Test User",
                    EmailVerified = true
                }
            };

            _mockAuthService.Setup(x => x.VerifyTokenAsync("valid-token"))
                           .ReturnsAsync(authResponse);

            // Act
            await _middleware.InvokeAsync(httpContext, _mockAuthService.Object);

            // Assert
            _mockNext.Verify(x => x(httpContext), Times.Once);
            _mockAuthService.Verify(x => x.VerifyTokenAsync("valid-token"), Times.Once);

            Assert.Equal(authResponse.User, httpContext.Items["User"]);
            Assert.Equal("test-uid", httpContext.Items["UserId"]);
        }

        [Fact]
        public async Task InvokeAsync_WithAuthorizeAttributeAndInvalidToken_Returns401()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Response.Body = new MemoryStream();

            var metadata = new EndpointMetadataCollection(new AuthorizeAttribute());
            var endpoint = new Endpoint(null, metadata, "test");
            httpContext.SetEndpoint(endpoint);
            httpContext.Request.Headers["Authorization"] = "Bearer invalid-token";

            var authResponse = new AuthResponse
            {
                Success = false,
                Message = "Token inválido"
            };

            _mockAuthService.Setup(x => x.VerifyTokenAsync("invalid-token"))
                           .ReturnsAsync(authResponse);

            // Act
            await _middleware.InvokeAsync(httpContext, _mockAuthService.Object);

            // Assert
            Assert.Equal(401, httpContext.Response.StatusCode);
            _mockNext.Verify(x => x(httpContext), Times.Never);

            httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(httpContext.Response.Body);
            var responseText = await reader.ReadToEndAsync();
            Assert.Equal("Token inválido ou expirado", responseText);
        }

        [Fact]
        public async Task InvokeAsync_WithAuthorizeAttributeButNoAuthHeader_Returns401()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Response.Body = new MemoryStream();

            var metadata = new EndpointMetadataCollection(new AuthorizeAttribute());
            var endpoint = new Endpoint(null, metadata, "test");
            httpContext.SetEndpoint(endpoint);

            // Act
            await _middleware.InvokeAsync(httpContext, _mockAuthService.Object);

            // Assert
            Assert.Equal(401, httpContext.Response.StatusCode);
            _mockNext.Verify(x => x(httpContext), Times.Never);
            _mockAuthService.Verify(x => x.VerifyTokenAsync(It.IsAny<string>()), Times.Never);

            httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(httpContext.Response.Body);
            var responseText = await reader.ReadToEndAsync();
            Assert.Equal("Token de autorização necessário", responseText);
        }

        [Fact]
        public async Task InvokeAsync_WithAuthorizeAttributeButInvalidAuthHeaderFormat_Returns401()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Response.Body = new MemoryStream();

            var metadata = new EndpointMetadataCollection(new AuthorizeAttribute());
            var endpoint = new Endpoint(null, metadata, "test");
            httpContext.SetEndpoint(endpoint);
            httpContext.Request.Headers["Authorization"] = "InvalidFormat token";

            // Act
            await _middleware.InvokeAsync(httpContext, _mockAuthService.Object);

            // Assert
            Assert.Equal(401, httpContext.Response.StatusCode);
            _mockNext.Verify(x => x(httpContext), Times.Never);
            _mockAuthService.Verify(x => x.VerifyTokenAsync(It.IsAny<string>()), Times.Never);

            httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(httpContext.Response.Body);
            var responseText = await reader.ReadToEndAsync();
            Assert.Equal("Token de autorização necessário", responseText);
        }

        [Fact]
        public async Task InvokeAsync_WithAuthorizeAttributeAndEmptyBearerToken_Returns401()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Response.Body = new MemoryStream();

            var metadata = new EndpointMetadataCollection(new AuthorizeAttribute());
            var endpoint = new Endpoint(null, metadata, "test");
            httpContext.SetEndpoint(endpoint);
            httpContext.Request.Headers["Authorization"] = "Bearer ";

            var authResponse = new AuthResponse
            {
                Success = false,
                Message = "Token inválido"
            };

            _mockAuthService.Setup(x => x.VerifyTokenAsync(""))
                           .ReturnsAsync(authResponse);

            // Act
            await _middleware.InvokeAsync(httpContext, _mockAuthService.Object);

            // Assert
            Assert.Equal(401, httpContext.Response.StatusCode);
            _mockNext.Verify(x => x(httpContext), Times.Never);

            httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(httpContext.Response.Body);
            var responseText = await reader.ReadToEndAsync();
            Assert.Equal("Token inválido ou expirado", responseText);
        }

        [Fact]
        public async Task InvokeAsync_WithAuthorizeAttributeAndValidTokenButNoUser_Returns401()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Response.Body = new MemoryStream();

            var metadata = new EndpointMetadataCollection(new AuthorizeAttribute());
            var endpoint = new Endpoint(null, metadata, "test");
            httpContext.SetEndpoint(endpoint);
            httpContext.Request.Headers["Authorization"] = "Bearer valid-token";

            var authResponse = new AuthResponse
            {
                Success = true,
                User = null // No user in response
            };

            _mockAuthService.Setup(x => x.VerifyTokenAsync("valid-token"))
                           .ReturnsAsync(authResponse);

            // Act
            await _middleware.InvokeAsync(httpContext, _mockAuthService.Object);

            // Assert
            Assert.Equal(401, httpContext.Response.StatusCode);
            _mockNext.Verify(x => x(httpContext), Times.Never);

            httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(httpContext.Response.Body);
            var responseText = await reader.ReadToEndAsync();
            Assert.Equal("Token inválido ou expirado", responseText);
        }

        [Fact]
        public async Task InvokeAsync_WithAuthorizeAttributeAndTokenWithSpaces_TrimsTokenCorrectly()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            var metadata = new EndpointMetadataCollection(new AuthorizeAttribute());
            var endpoint = new Endpoint(null, metadata, "test");
            httpContext.SetEndpoint(endpoint);
            httpContext.Request.Headers["Authorization"] = "Bearer   token-with-spaces   ";

            var authResponse = new AuthResponse
            {
                Success = true,
                User = new UserInfo
                {
                    Uid = "test-uid",
                    Email = "test@example.com",
                    DisplayName = "Test User",
                    EmailVerified = true
                }
            };

            _mockAuthService.Setup(x => x.VerifyTokenAsync("token-with-spaces"))
                           .ReturnsAsync(authResponse);

            // Act
            await _middleware.InvokeAsync(httpContext, _mockAuthService.Object);

            // Assert
            _mockNext.Verify(x => x(httpContext), Times.Once);
            _mockAuthService.Verify(x => x.VerifyTokenAsync("token-with-spaces"), Times.Once);
        }

        [Fact]
        public async Task InvokeAsync_WithNullEndpoint_CallsNextDelegate()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            // No endpoint set

            // Act
            await _middleware.InvokeAsync(httpContext, _mockAuthService.Object);

            // Assert
            _mockNext.Verify(x => x(httpContext), Times.Once);
            _mockAuthService.Verify(x => x.VerifyTokenAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task InvokeAsync_WithNullEndpointMetadata_CallsNextDelegate()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            var endpoint = new Endpoint(null, null, "test");
            httpContext.SetEndpoint(endpoint);

            // Act
            await _middleware.InvokeAsync(httpContext, _mockAuthService.Object);

            // Assert
            _mockNext.Verify(x => x(httpContext), Times.Once);
            _mockAuthService.Verify(x => x.VerifyTokenAsync(It.IsAny<string>()), Times.Never);
        }
    }
}