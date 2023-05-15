using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using RestaurantReviewProgram.Models;
using System.Net.Sockets;
using System.Net;
using System.Web;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RestaurantReviewProgram.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RestaurantCreator : ControllerBase
    {
        private readonly ILogger<RestaurantCreator> logger;
        private readonly RestaurantList restaurantList;
        private readonly HttpClient httpClient;
        private readonly string key;

        public RestaurantCreator(RestaurantList restaurantList, ILogger<RestaurantCreator> logger, HttpClient httpClient, IOptions<Models.Secret> secret)
        {
            this.restaurantList = restaurantList;
            this.logger = logger;
            this.httpClient = httpClient;
            this.key = secret.Value.ApiKey;
            httpClient.BaseAddress = new Uri("https://maps.googleapis.com/maps/api/geocode/json");
        }

        // Creation - Post
        [HttpPost()]
        public async Task<ActionResult<Restaurant>> createRestaurant(Restaurant restaurant)
        {
            await geocode(restaurant);
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
        public async Task <ActionResult<Restaurant>> updateRestaurant(Restaurant restaurant)
        {
            // Object will already have an id
            if (restaurantList.Keys.Contains(restaurant.Id))
            {
                await geocode(restaurant);
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

        // Geocoding
        private async Task geocode(Restaurant restaurant)
        {
            // Formatting for HTTP
            string address = $"{restaurant.Address},{restaurant.City},{restaurant.State}";
            address = HttpUtility.UrlEncode(address);
            // Access url + return results into a response class + deserialization
            GeocodeResponse? message = await httpClient.GetFromJsonAsync<GeocodeResponse>($"?address={address}&key={key}");
            // Response -> Results -> Geometry -> Location
            restaurant.Latitude = message.Results[0].Geometry.Location.Lat; // Need to access deserialized data
            restaurant.Longitude = message.Results[0].Geometry.Location.Lng;
        }
        // Creating map
        //      https://developers.google.com/maps/documentation/javascript/datalayer
        //      https://developers.google.com/maps/documentation/javascript/geometry
        /*      ^ this describes a process by which you create a map, then individual marker variables for 
         *      the map. would the best way to do this be creating an arraylist(or other expandable data structure)
         *      of markers associated with each restauraunt, or can/should marker be part of the restaurant class
         *      so the two are intrinsically linked?
         *      addtl: how do we test this? does this only work in javascript, bc it generates an image?
         *      do we want to generate and return a geoJSON with points instead?
         */
        //      https://developers.google.com/maps/documentation/android-sdk/utility
        /*      iconGenerator allows customization of marker- include restaurant name and or ratings in this option?
         */
        public void mapRestaurant(Restaurant restaurant)
        {

        }
    }
}