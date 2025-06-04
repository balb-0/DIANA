using System;
using System.Data;
using System.Threading.Tasks;
using Moq;
using Moq.Language.Flow;

namespace backend.Tests
{
    public static class MockExtensions
    {
        public static ISetup<IDbConnection, Task<T>> SetupDapperAsync<T>(this Mock<IDbConnection> mock, Func<IDbConnection, Task<T>> expression)
        {
            return mock.Setup(expression);
        }
    }
}