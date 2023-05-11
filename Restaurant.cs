using System.Net.Sockets;

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
        private double latitude;
        private double longitude;
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
        /// <summmary>The restaurant's name</summary>
        /// <example>Local Woodfired Grill</example>
        public string Name
        {
            get { return name; }
        }
        /// <summary>A unique GUID to identify a restaurant</summary>
        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }
        /// <summmary>Street address the restaurant is located at</summary>
        /// <example>5315 Windward Parkway</example>
        public string Address
        {
            get { return address; }
        }
        /// <summmary>City the restaurant is located in</summary>
        /// <example>Alpharetta</example>
        public string City
        {
            get { return city; }
        }
        /// <summmary>State the restaurant is located in</summary>
        /// <example>GA</example>
        public string State
        {
            get { return state; }
        }
        /// <summmary>Postal code the restaurant is located in</summary>
        /// <example>30004</example>
        public string Zip
        {
            get { return zip; }
        }
        /// <summmary>Restaurant's latitude</summary>
        public double Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }
        /// <summmary>Restaurant's longitude</summary>
        public double Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }
        /// <summmary>Count of restaurant's positive reviews</summary>
        public int PositiveReviews 
        {
            get { return positiveReviews; }
        }
        /// <summmary>Count of restaurant's negative reviews</summary>
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
