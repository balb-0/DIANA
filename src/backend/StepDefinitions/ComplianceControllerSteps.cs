using backend.Controllers;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TechTalk.SpecFlow;
using Xunit;

namespace backend.StepDefinitions
{
    [Binding]
    public class ComplianceControllerSteps
    {
        private Mock<IComplianceService>? _mockService;
        private Mock<ILogger<ComplianceController>>? _mockLogger;
        private ComplianceController? _controller;
        private ActionResult<int>? _result;

        [Given(@"I have a compliance controller with mocked dependencies")]
        public void GivenIHaveAComplianceControllerWithMockedDependencies()
        {
            _mockService = new Mock<IComplianceService>();
            _mockLogger = new Mock<ILogger<ComplianceController>>();
            _controller = new ComplianceController(_mockService.Object, _mockLogger.Object);
        }

        [Given(@"the compliance service returns (.*) (.*) clients")]
        public void GivenTheComplianceServiceReturnsClientsOfType(int count, string clientType)
        {
            bool nonconforming = clientType == "non-conforming";
            _mockService!
                .Setup(s => s.GetClientsCountAsync(nonconforming))
                .ReturnsAsync(count);
        }

        [Given(@"the compliance service throws an exception with message ""(.*)""")]
        public void GivenTheComplianceServiceThrowsAnExceptionWithMessage(string message)
        {
            _mockService!
                .Setup(s => s.GetClientsCountAsync(It.IsAny<bool>()))
                .ThrowsAsync(new Exception(message));
        }

        [When(@"I call the GET clients-count endpoint with nonconforming parameter as (.*)")]
        public async Task WhenICallTheGETClientsCountEndpointWithNonconformingParameterAs(bool nonconforming)
        {
            _result = await _controller!.GetClientsCount(nonconforming);
        }

        [When(@"I call the GET clients-count endpoint without nonconforming parameter")]
        public async Task WhenICallTheGETClientsCountEndpointWithoutNonconformingParameter()
        {
            _result = await _controller!.GetClientsCount();
        }

        [Then(@"I should receive an HTTP 200 OK response")]
        public void ThenIShouldReceiveAnHTTP200OKResponse()
        {
            var okResult = Assert.IsType<OkObjectResult>(_result!.Result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Then(@"I should receive an HTTP 500 Internal Server Error response")]
        public void ThenIShouldReceiveAnHTTP500InternalServerErrorResponse()
        {
            var statusResult = Assert.IsType<ObjectResult>(_result!.Result);
            Assert.Equal(500, statusResult.StatusCode);
        }

        [Then(@"the response body should contain the count (.*)")]
        public void ThenTheResponseBodyShouldContainTheCount(int expectedCount)
        {
            var okResult = Assert.IsType<OkObjectResult>(_result!.Result);
            Assert.Equal(expectedCount, okResult.Value);
        }

        [Then(@"the response body should contain the message ""(.*)""")]
        public void ThenTheResponseBodyShouldContainTheMessage(string expectedMessage)
        {
            var statusResult = Assert.IsType<ObjectResult>(_result!.Result);
            Assert.Equal(expectedMessage, statusResult.Value);
        }

        [Then(@"the compliance service should be called once with nonconforming as (.*)")]
        public void ThenTheComplianceServiceShouldBeCalledOnceWithNonconformingAs(bool nonconforming)
        {
            _mockService!.Verify(s => s.GetClientsCountAsync(nonconforming), Times.Once);
        }

        [Then(@"an error log should be recorded for controller")]
        public void ThenAnErrorLogShouldBeRecordedForController()
        {
            _mockLogger!.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }
    }
}
