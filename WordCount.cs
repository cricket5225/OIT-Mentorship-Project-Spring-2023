using Microsoft.AspNetCore.Mvc;

namespace RestaurantReviewProgram.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WordCount : ControllerBase
    {
        private readonly ILogger<WordCount> _logger;

        public WordCount(ILogger<WordCount> logger)
        {
            _logger = logger;
        }

        [HttpGet()]
        public RestReview wordCount(string review)
        {
            /* There is a duplicate of this in the RestReview
             * class for its constructor- That returns an int,
             * this returns a RestReview object */
            RestReview newRev = new RestReview(review);
            newRev.RevWordCount = review.Split(' ').Count();
            return newRev;
        }

    }
}
