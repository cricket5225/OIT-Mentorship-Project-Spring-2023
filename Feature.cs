namespace RestaurantReviewProgram.Models
{
    public class Feature
    {
        public string type { get; set; } // Should be set to "Feature"?
        public Geometry geometry { get; set; }
        public Properties properties { get; set; }
        public Feature(Restaurant restaurant)
        {
            type = "Feature";
            geometry = new Geometry(restaurant);
            properties = new Properties(restaurant);
        }
    }
}
