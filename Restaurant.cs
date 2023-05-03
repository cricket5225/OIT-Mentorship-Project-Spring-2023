namespace RestaurantReviewProgram.Models
{
    public class Restaurant
    {
        // Attributes
        private string name;
        private Guid id;
        // Location Attributes
        private string address;
        private string city;
        private string state;
        private string zip;
        // Counter attributes
        private int positiveReviews;
        private int negativeReviews;

        // Constructor
        public Restaurant(string name, string address, string city, string state, string zip)
        {
            this.name = name;
            this.address = address;
            this.city = city; 
            this.state = state; 
            this.zip = zip;
        }
        // Getters/Setters
        public string Name
        {
            get { return name; }
        }
        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Address
        {
            get { return address; }
        }
        public string City
        {
            get { return city; }
        }
        public string State
        {
            get { return state; }
        }
        public string Zip
        {
            get { return zip; }
        }
        public int PositiveReviews
        {
            get { return positiveReviews; }
        }
        public int NegativeReviews
        {
            get { return negativeReviews; }
        }
        // Methods
        public void addReview(RestaurantReview restaurantReview)
        {
            if (restaurantReview.RevSentiment == 1) { positiveReviews++; }
            else if (restaurantReview.RevSentiment == 0) { negativeReviews++; }
        }
    }
}
