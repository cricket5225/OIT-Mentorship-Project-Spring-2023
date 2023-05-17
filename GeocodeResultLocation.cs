namespace RestaurantReviewProgram.Models
{
    public class GeocodeResultLocation
    {
        /// <summary>Restaurant's latitude, from JSON</summary>
        public double Lat {get; set;}
        /// <summary>Restaurant's longitude, from JSON</summary>
        public double Lng { get; set;}
    }
}
