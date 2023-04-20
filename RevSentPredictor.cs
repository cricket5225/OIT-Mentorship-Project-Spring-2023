using Microsoft.AspNetCore.Mvc;
using static RestaurantReviewProgram.SentimentModel;

namespace RestaurantReviewProgram.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RevSentPredictor : ControllerBase
    {
        private readonly ILogger<RevSentPredictor> _logger;

        public RevSentPredictor(ILogger<RevSentPredictor> logger)
        {
            _logger = logger;
        }

        [HttpGet()]
        public float revPredictor(string review)
        {
            //Load sample data
            var sampleData = new SentimentModel.ModelInput()
            {
                Col0 = @"Crust is not good.",
            };

            //Load model and predict output
            ModelOutput result = SentimentModel.Predict(sampleData);

            return result.PredictedLabel;
        }
    }
}
