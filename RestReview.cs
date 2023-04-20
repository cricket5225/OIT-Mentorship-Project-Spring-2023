using RestaurantReviewProgram.Controllers;

namespace RestaurantReviewProgram
{
    public class RestReview
    {
        // Attributes
        private string revBody;
        private int revWordCount;
        private int revSentiment;
        // Constructor
        public RestReview(string review)
        {
            revBody = review;
            revWordCount = wordCount(review);
        }
        // Getters and setters-
        /* All of these should be set with constructor, and 
         * likely won't need to be modified, so should they 
         * only have "gets" and no "sets"? */
        public string RevBody
        {
            get { return revBody; }
            // set { revBody = value; }
        }
        public int RevWordCount
        {
            get { return revWordCount; }
            // This set is accessible only for the WordCount controller
            set { revWordCount = value; }
        }
        public int RevSentiment
        {
            get { return revSentiment; }
            /* set 
            { 
                // Set to result of ML model?
                revSentiment = value; 
            } */
        }
        // Methods
        public int wordCount(string review)
        {
            /* Moved this here instead of WordCount controller 
             * so it can be part of the review constructor */
            string[] words = review.Split(' ');
            return words.Count();
        }
    }
}
