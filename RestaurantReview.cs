using Microsoft.AspNetCore.Mvc;
using RestaurantReviewProgram.Controllers;
using static RestaurantReviewProgram.SentimentModel;

namespace RestaurantReviewProgram.Models
{
    public class RestaurantReview
    {
        // Attributes
        private string revBody;
        private int revWordCount;
        private int revSentiment;
        // Constructor
        public RestaurantReview(string review)
        {
            revBody = review;
            revWordCount = wordCount(review);
            // ML MODEL

            //Load sample data
            var sampleData = new ModelInput()
            {
                Col0 = review, //@"Crust is not good.",
            };

            //Load model and predict output
            revSentiment = (int)Predict(sampleData).PredictedLabel;
        }
        // Getters and setters-
        public string RevBody
        {
            get { return revBody; }
        }
        public int RevWordCount
        {
            get { return revWordCount; }
        }
        public int RevSentiment
        {
            get { return revSentiment; }
        }
        // Methods
        public int wordCount(string review)
        {
            string[] words = review.Split(' ');
            return words.Count();
        }
    }
}
