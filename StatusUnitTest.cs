using Microsoft.Extensions.Logging.Abstractions;
using Xunit;
using RestaurantReviewProgram.Controllers;
using Microsoft.Extensions.Logging;

namespace UnitTesting
{
    public class StatusUnitTest
    {
        [Fact]
        public void ServerStatusCheck()
        {
            // Arrange
            ILogger<ServerCheck> logger = new NullLogger<ServerCheck>();
            ServerCheck serverCheck = new ServerCheck(logger);

            // Act
            string result = serverCheck.Get();

            // Assert
            Assert.NotNull(result);
        }
    }
}