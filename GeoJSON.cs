namespace RestaurantReviewProgram.Models
{
    public class GeoJSON
    {
        public string type {get; set;}
        public List<Feature> features { get; set; }
        public GeoJSON(Restaurant restaurant)
        {
            type = "FeatureCollection";
            features = new List<Feature>() { new Feature(restaurant) };
        }
    }
}