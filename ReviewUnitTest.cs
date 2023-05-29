using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using RestaurantReviewProgram.Controllers;
using RestaurantReviewProgram.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTesting
{
    public class ReviewUnitTest
    {
        [Fact]
        public void createReview()
        {
            // Arrange
            ILogger<ReviewCreator> logger = new NullLogger<ReviewCreator>();
            RestaurantList restaurantList = new RestaurantList();
            Guid guid = Guid.NewGuid();
            // create restaurant and add guid to the restaurantlist dictionary
            string reviewString = "I love this place!";
            ReviewCreator reviewCreatorController = new ReviewCreator(restaurantList, logger);

            // Act
            RestaurantReview result = reviewCreatorController.reviewCreator(reviewString, guid);

            // Assert
            Assert.NotNull(result);
        }
    }
}
