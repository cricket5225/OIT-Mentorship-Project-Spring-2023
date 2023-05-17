namespace RestaurantReviewProgram.Models
{
    public class RestaurantList : Dictionary<Guid, Restaurant>
    {
        /// <summary>List of all recorded restaurants</summary>
        public RestaurantList() 
        { 
            // This is the singleton. Yay!
        }
    }
}
