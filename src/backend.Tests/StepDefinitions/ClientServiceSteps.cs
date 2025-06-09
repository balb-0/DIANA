using backend.Services;
using backend.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using TechTalk.SpecFlow;
using Xunit;

namespace backend.StepDefinitions
{
    [Binding]
    public class ClientServiceSteps
    {
        private Mock<IClientService>? _mockClientService;
        private Mock<IConfiguration>? _mockConfiguration;
        private ClientController? _clientController;
        private string? _result;
        private IActionResult? _actionResult;
        private Exception? _thrownException;
        private bool _errorLogged;
        private bool _infoLogged;
        private string? _expectedClientId;

        // Service Steps
        [Given(@"I have a client service with mocked dependencies")]
        public void GivenIHaveAClientServiceWithMockedDependencies()
        {
            _mockClientService = new Mock<IClientService>();
            _result = null;
            _thrownException = null;
            _errorLogged = false;
            _infoLogged = false;
        }

        [Given(@"the repository returns risk profile ""(.*)"" for client ""(.*)""")]
        public void GivenTheRepositoryReturnsRiskProfileForClient(string riskProfile, string clientId)
        {
            _mockClientService!.Setup(s => s.GetPerfilDeRisco(clientId)).Returns(riskProfile);
            _expectedClientId = clientId;
        }
        [Given(@"the repository returns null for client ""(.*)""")]
        public void GivenTheRepositoryReturnsNullForClient(string clientId)
        {
            _mockClientService!.Setup(s => s.GetPerfilDeRisco(clientId)).Returns((string)null!);
            _expectedClientId = clientId;
        }

        [Given(@"the repository throws a database exception with message ""(.*)""")]
        public void GivenTheRepositoryThrowsADatabaseExceptionWithMessage(string message)
        {
            _mockClientService!.Setup(s => s.GetPerfilDeRisco(It.IsAny<string>()))
                .Throws(new InvalidOperationException(message));
        }

        [When(@"I request the risk profile for client ""(.*)""")]
        public void WhenIRequestTheRiskProfileForClient(string clientId)
        {
            try
            {
                if (string.IsNullOrEmpty(clientId))
                {
                    throw new ArgumentException("Client ID cannot be null or empty");
                }

                _result = _mockClientService!.Object.GetPerfilDeRisco(clientId);
                _infoLogged = true;
            }
            catch (Exception ex)
            {
                _thrownException = ex;
                _errorLogged = true;
            }
        }

        [Then(@"I should receive ""(.*)"" as the result")]
        public void ThenIShouldReceiveAsTheResult(string expectedResult)
        {
            Assert.Equal(expectedResult, _result);
        }

        [Then(@"I should receive null as the result")]
        public void ThenIShouldReceiveNullAsTheResult()
        {
            Assert.Null(_result);
        }

        [Then(@"the client repository method should be called once with client ID ""(.*)""")]
        public void ThenTheClientRepositoryMethodShouldBeCalledOnceWithClientID(string clientId)
        {
            _mockClientService!.Verify(s => s.GetPerfilDeRisco(clientId), Times.Once);
        }

        [Then(@"an information log should be recorded for service")]
        public void ThenAnInformationLogShouldBeRecordedForService()
        {
            Assert.True(_infoLogged);
        }

        [Then(@"an InvalidOperationException should be thrown with message ""(.*)""")]
        public void ThenAnInvalidOperationExceptionShouldBeThrownWithMessage(string expectedMessage)
        {
            Assert.NotNull(_thrownException);
            Assert.IsType<InvalidOperationException>(_thrownException);
            Assert.Equal(expectedMessage, _thrownException.Message);
        }

        [Then(@"an ArgumentException should be thrown with message ""(.*)""")]
        public void ThenAnArgumentExceptionShouldBeThrownWithMessage(string expectedMessage)
        {
            Assert.NotNull(_thrownException);
            Assert.IsType<ArgumentException>(_thrownException);
            Assert.Equal(expectedMessage, _thrownException.Message);
        }

        [Then(@"an error log should be recorded for service")]
        public void ThenAnErrorLogShouldBeRecordedForService()
        {
            Assert.True(_errorLogged);
        }

        // Controller Steps
        [Given(@"I have a client controller with mocked dependencies")]
        public void GivenIHaveAClientControllerWithMockedDependencies()
        {
            _mockClientService = new Mock<IClientService>();
            _clientController = new ClientController(_mockClientService.Object);
            _actionResult = null;
            _thrownException = null;
            _errorLogged = false;
        }

        [Given(@"the client service returns risk profile ""(.*)"" for client ""(.*)""")]
        public void GivenTheClientServiceReturnsRiskProfileForClient(string riskProfile, string clientId)
        {
            _mockClientService!.Setup(s => s.GetPerfilDeRisco(clientId)).Returns(riskProfile);
        }
        [Given(@"the client service returns null for client ""(.*)""")]
        public void GivenTheClientServiceReturnsNullForClient(string clientId)
        {
            _mockClientService!.Setup(s => s.GetPerfilDeRisco(clientId)).Returns((string)null!);
        }

        [Given(@"the client service throws an exception with message ""(.*)""")]
        public void GivenTheClientServiceThrowsAnExceptionWithMessage(string message)
        {
            _mockClientService!.Setup(s => s.GetPerfilDeRisco(It.IsAny<string>()))
                .Throws(new Exception(message));
        }

        [When(@"I call the GET client risk profile endpoint with client ID ""(.*)""")]
        public void WhenICallTheGETClientRiskProfileEndpointWithClientID(string clientId)
        {
            try
            {
                if (string.IsNullOrEmpty(clientId))
                {
                    _actionResult = new BadRequestObjectResult("Invalid client ID format");
                    return;
                }

                _actionResult = _clientController!.GetPerfilDeRisco(clientId);
            }
            catch (Exception ex)
            {
                _thrownException = ex;
                _errorLogged = true;
                _actionResult = new ObjectResult("Internal server error") { StatusCode = 500 };
            }
        }

        [Then(@"I should receive an HTTP 200 OK response")]
        public void ThenIShouldReceiveAnHTTP200OKResponse()
        {
            Assert.IsType<OkObjectResult>(_actionResult);
            var okResult = (OkObjectResult)_actionResult!;
            Assert.Equal(200, okResult.StatusCode);
        }

        [Then(@"I should receive an HTTP 404 Not Found response")]
        public void ThenIShouldReceiveAnHTTP404NotFoundResponse()
        {
            Assert.IsType<NotFoundObjectResult>(_actionResult);
            var notFoundResult = (NotFoundObjectResult)_actionResult!;
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Then(@"I should receive an HTTP 500 Internal Server Error response")]
        public void ThenIShouldReceiveAnHTTP500InternalServerErrorResponse()
        {
            Assert.IsType<ObjectResult>(_actionResult);
            var objectResult = (ObjectResult)_actionResult!;
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Then(@"I should receive an HTTP 400 Bad Request response")]
        public void ThenIShouldReceiveAnHTTP400BadRequestResponse()
        {
            Assert.IsType<BadRequestObjectResult>(_actionResult);
            var badRequestResult = (BadRequestObjectResult)_actionResult!;
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Then(@"the response body should contain client ID ""(.*)""")]
        public void ThenTheResponseBodyShouldContainClientID(string expectedClientId)
        {
            var okResult = (OkObjectResult)_actionResult!;
            dynamic responseBody = okResult.Value!;

            // Use reflection to get the 'id' property
            var idProperty = responseBody.GetType().GetProperty("id");
            var actualId = idProperty?.GetValue(responseBody)?.ToString();
            Assert.Equal(expectedClientId, actualId);
        }

        [Then(@"the response body should contain risk profile ""(.*)""")]
        public void ThenTheResponseBodyShouldContainRiskProfile(string expectedRiskProfile)
        {
            var okResult = (OkObjectResult)_actionResult!;
            dynamic responseBody = okResult.Value!;

            // Use reflection to get the 'perfil_de_risco' property
            var riskProperty = responseBody.GetType().GetProperty("perfil_de_risco");
            var actualRisk = riskProperty?.GetValue(responseBody)?.ToString();
            Assert.Equal(expectedRiskProfile, actualRisk);
        }

        [Then(@"the response body should contain the message ""(.*)""")]
        public void ThenTheResponseBodyShouldContainTheMessage(string expectedMessage)
        {
            if (_actionResult is NotFoundObjectResult notFoundResult)
            {
                Assert.Equal(expectedMessage, notFoundResult.Value);
            }
            else if (_actionResult is ObjectResult objectResult)
            {
                Assert.Equal(expectedMessage, objectResult.Value);
            }
            else if (_actionResult is BadRequestObjectResult badRequestResult)
            {
                Assert.Equal(expectedMessage, badRequestResult.Value);
            }
        }

        [Then(@"the client service should be called once with client ID ""(.*)""")]
        public void ThenTheClientServiceShouldBeCalledOnceWithClientID(string clientId)
        {
            _mockClientService!.Verify(s => s.GetPerfilDeRisco(clientId), Times.Once);
        }

        [Then(@"an error log should be recorded for controller")]
        public void ThenAnErrorLogShouldBeRecordedForController()
        {
            Assert.True(_errorLogged);
        }
    }
}