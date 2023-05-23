namespace RestaurantReviewProgram.Models
{
    public class Geometry
    {
        public string type { get; set; }   // Point
        public double[] coordinates { get; set; }
        public Geometry(Restaurant restaurant)
        {
            type = "Point";
            coordinates = new double[] { restaurant.Latitude, restaurant.Longitude };
        }
    }
}
