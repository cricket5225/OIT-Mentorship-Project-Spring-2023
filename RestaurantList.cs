namespace RestaurantReviewProgram.Models
{
    public class RestaurantList : Dictionary<Guid, Restaurant>
    {
        /// <summmary>List of all recorded restaurants</summary>
        public RestaurantList() 
        { 
            // This is the singleton. Yay!
        }
    }
}
