namespace RestaurantReviewProgram.Models
{
    public class Properties
    {
        public string name { get; set; }
        public string description { get; set; }
        public Properties(Restaurant restaurant)
        {
            name = restaurant.Name;
            description = "Positive reviews: " + restaurant.PositiveReviews + "\tNegative reviews: " + restaurant.NegativeReviews;
        }
    }
}
