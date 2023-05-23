using Microsoft.AspNetCore.Mvc;
using RestaurantReviewProgram.Models;

namespace RestaurantReviewProgram.Controllers
{
    [ApiController]
    [Route("Restaurant/")]
    public class ReviewCreator : ControllerBase
    {
        private readonly ILogger<ReviewCreator> logger;
        private readonly RestaurantList restaurantList;

        public ReviewCreator(RestaurantList restaurantList, ILogger<ReviewCreator> logger)
        {
            this.restaurantList = restaurantList;
            this.logger = logger;
        }

        [HttpPost("{restaurantId}/Review")] 
        public RestaurantReview reviewCreator(string reviewString, Guid restaurantId) 
        {
            // Turn reviewString into a review object to get sentiment
            RestaurantReview review = new RestaurantReview(reviewString);
            // Call addReview to add review to restaurant w/ id (method handles positive vs negative)
            restaurantList[restaurantId].addReview(review);
            return review;
        }
    }
}
