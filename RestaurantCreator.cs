using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using RestaurantReviewProgram.Models;
using System.Net.Mail;

namespace RestaurantReviewProgram.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RestaurantCreator : ControllerBase
    {
        private readonly ILogger<RestaurantCreator> logger;
        private readonly RestaurantList restaurantList;

        public RestaurantCreator(RestaurantList restaurantList, ILogger<RestaurantCreator> logger)
        {
            this.restaurantList = restaurantList;
            this.logger = logger;
        }

        // Creation - Post
        [HttpPost()]
        public ActionResult<Restaurant> createRestaurant(Restaurant restaurant)
        {
            // Add ID- Nullable?
            restaurant.Id = Guid.NewGuid();
            // Store in singleton list
            restaurantList.Add(restaurant.Id, restaurant);
            /* may return restaurant object with the id- 
             * could have different classes for getting and returning but not necessary */
            return Created(restaurant.Id.ToString(), restaurant);
        }
        // Updating - Put
        [HttpPut()]
        public ActionResult<Restaurant> updateRestaurant(Restaurant restaurant)
        {
            // Object will already have an id
            if (restaurantList.Keys.Contains(restaurant.Id))
            {
                // Find Restaurant with ID in singleton 
                // Update all fields in singleton
                restaurantList[restaurant.Id] = restaurant;
                // Return restaurant
                return restaurant;
            }
            else { return new NotFoundResult(); }
        }
        // Gets- One with no arguments, one with arbit. id (int?)
        [HttpGet()]
        public List<Restaurant> getRestaurant()
        {
            // Return list of restaurants- Singleton
            return restaurantList.Values.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Restaurant> getRestaurant(Guid id) 
        {
            // Find Restauraunt with ID in singleton
            if (restaurantList.Keys.Contains(id))
            {
                return restaurantList[id];
            }
            else { return new NotFoundResult(); }
        }
    }
}
