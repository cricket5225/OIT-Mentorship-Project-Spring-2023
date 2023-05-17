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
using System.Drawing;

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
            httpClient.BaseAddress = new Uri("https://maps.googleapis.com/maps/api/");
        }

        [HttpGet("map")]
        public async Task createMapTest()
        {
            await generateMap();
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
        // https://maps.googleapis.com/maps/api/geocode/json?place_id=ChIJeRpOeF67j4AR9ydy_PIzPuM&key=YOUR_API_KEY
        private async Task geocode(Restaurant restaurant)
        {
            // Formatting for HTTP
            string address = $"{restaurant.Address},{restaurant.City},{restaurant.State}";
            address = HttpUtility.UrlEncode(address);
            // Access url + return results into a response class + deserialization
            GeocodeResponse? message = await httpClient.GetFromJsonAsync<GeocodeResponse>($"geocode/json?address={address}&key={key}");
            // Response -> Results -> Geometry -> Location
            restaurant.Latitude = message.Results[0].Geometry.Location.Lat; // Need to access deserialized data
            restaurant.Longitude = message.Results[0].Geometry.Location.Lng;
        }

        // Creating map
        private async Task<string> generateMap()
        {
            string markers = "";
            foreach (Restaurant restaurant in restaurantList.Values)
            {
                markers += $"&markers = label: F % 7C{restaurant.Latitude},{restaurant.Longitude}";
                //Add logic for symbols to be + or - depending on rating
            }
            markers = HttpUtility.UrlEncode(markers);
            HttpResponseMessage message = await httpClient.GetAsync($"staticmap?size=400x400{markers}&key={key}");
            return message.ToString();
            // Did research into returning this as a byte[] and ouldn't figure it out, tried to convert to GeoJSON below
        }

        // Return GeoJSON of one restaurant
        // https://learn.microsoft.com/en-us/bingmaps/v8-web-control/modules/geojson-module/
        private string generateGeoJson(Guid id)
        {
            /* This method only turns one restaurant into a GeoJSON. Once this works, a foreach loop 
             * can be used on the restaurantList to do all at once */

            // Should take in one restaurant and generate a GeoJSON object
            GeoJSON geoJSON = new GeoJSON(restaurantList[id]);
            // Should return GeoJSON object as a string
            return JsonSerializer.Serialize(geoJSON);
        }
    }
}