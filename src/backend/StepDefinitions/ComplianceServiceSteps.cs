using backend.Services;
using backend.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using TechTalk.SpecFlow;
using Xunit;

namespace backend.StepDefinitions
{
    [Binding]
    public class ComplianceServiceSteps
    {
        private Mock<IComplianceRepository>? _mockRepository;
        private Mock<ILogger<ComplianceService>>? _mockLogger;
        private ComplianceService? _service;
        private int _result;
        private Exception? _thrownException;

        [Given(@"I have a compliance service with mocked dependencies")]
        public void GivenIHaveAComplianceServiceWithMockedDependencies()
        {
            _mockRepository = new Mock<IComplianceRepository>();
            _mockLogger = new Mock<ILogger<ComplianceService>>();
            _service = new ComplianceService(_mockRepository.Object, _mockLogger.Object);
        }

        [Given(@"the repository returns (.*) (.*) clients")]
        public void GivenTheRepositoryReturnsClientsOfType(int count, string clientType)
        {
            if (clientType == "conforming")
            {
                _mockRepository!
                    .Setup(r => r.GetConformingClientsCountAsync())
                    .ReturnsAsync(count);
            }
            else if (clientType == "non-conforming")
            {
                _mockRepository!
                    .Setup(r => r.GetNonConformingClientsCountAsync())
                    .ReturnsAsync(count);
            }
        }

        [Given(@"the repository throws a database exception with message ""(.*)""")]
        public void GivenTheRepositoryThrowsADatabaseExceptionWithMessage(string message)
        {
            _mockRepository!
                .Setup(r => r.GetConformingClientsCountAsync())
                .ThrowsAsync(new InvalidOperationException(message));
        }

        [When(@"I request the count of (.*) clients")]
        public async Task WhenIRequestTheCountOfClientsOfType(string clientType)
        {
            try
            {
                bool nonconforming = clientType == "non-conforming";
                _result = await _service!.GetClientsCountAsync(nonconforming);
            }
            catch (Exception ex)
            {
                _thrownException = ex;
            }
        }

        [Then(@"I should receive (.*) as the result")]
        public void ThenIShouldReceiveAsTheResult(int expectedCount)
        {
            Assert.Equal(expectedCount, _result);
        }

        [Then(@"the conforming clients repository method should be called once")]
        public void ThenTheConformingClientsRepositoryMethodShouldBeCalledOnce()
        {
            _mockRepository!.Verify(r => r.GetConformingClientsCountAsync(), Times.Once);
        }

        [Then(@"the non-conforming clients repository method should be called once")]
        public void ThenTheNonConformingClientsRepositoryMethodShouldBeCalledOnce()
        {
            _mockRepository!.Verify(r => r.GetNonConformingClientsCountAsync(), Times.Once);
        }

        [Then(@"the conforming clients repository method should not be called")]
        public void ThenTheConformingClientsRepositoryMethodShouldNotBeCalled()
        {
            _mockRepository!.Verify(r => r.GetConformingClientsCountAsync(), Times.Never);
        }

        [Then(@"the non-conforming clients repository method should not be called")]
        public void ThenTheNonConformingClientsRepositoryMethodShouldNotBeCalled()
        {
            _mockRepository!.Verify(r => r.GetNonConformingClientsCountAsync(), Times.Never);
        }

        [Then(@"an InvalidOperationException should be thrown with message ""(.*)""")]
        public void ThenAnInvalidOperationExceptionShouldBeThrownWithMessage(string expectedMessage)
        {
            Assert.IsType<InvalidOperationException>(_thrownException);
            Assert.Equal(expectedMessage, _thrownException!.Message);
        }

        [Then(@"an information log should be recorded")]
        public void ThenAnInformationLogShouldBeRecorded()
        {
            _mockLogger!.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Then(@"an error log should be recorded for service")]
        public void ThenAnErrorLogShouldBeRecordedForService()
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
