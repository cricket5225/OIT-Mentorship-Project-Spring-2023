using Microsoft.AspNetCore.Mvc;

namespace RestaurantReviewProgram.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RevCreator
    {
        private readonly ILogger<RevCreator> _logger;

        public RevCreator(ILogger<RevCreator> logger)
        {
            _logger = logger;
        }

        [HttpGet()]
        public RestReview reviewCreator(string review) 
        {
            RestReview rev = new RestReview(review);
            return rev;
        }
    }
}
