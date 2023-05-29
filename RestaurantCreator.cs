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
using System.Runtime.InteropServices;

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

        [HttpGet("generateMap")]
        public async Task<FileContentResult> createMap()
        {
            byte[] map = await generateMap();
            return File(map, "image/png");
        }

        [HttpGet("generateGeoJSON/{id}")]
        public GeoJSON createGeoJSON(Guid id)
        {
            return new GeoJSON(restaurantList[id]);
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
        private async Task<byte[]> generateMap()
        {
            string markers = "";
            List<double> latArray = new List<double>();
            List<double> longArray = new List<double>();
            foreach (Restaurant restaurant in restaurantList.Values)
            {
                // Evaluating color
                if (restaurant.PositiveReviews > restaurant.NegativeReviews) { restaurant.Color = "green"; }
                else if (restaurant.NegativeReviews > restaurant.PositiveReviews) { restaurant.Color = "red"; }
                else if (restaurant.NegativeReviews == restaurant.PositiveReviews) { restaurant.Color = "yellow"; }
                // Add coordinates to 2D array
                latArray.Add(restaurant.Latitude);
                longArray.Add(restaurant.Longitude);
                // Marker string
                markers += ($"&markers=color:{restaurant.Color}%7Clabel:{restaurant.PositiveReviews+restaurant.NegativeReviews}%7C{restaurant.Latitude},{restaurant.Longitude}");
            }
            // Finding median latitude- Unnecessary
            double latMedian;
            latArray.Sort();
            if (latArray.Count % 2 == 1 ) // Odd number of elements in list, take middle
            {
                latMedian = latArray[(latArray.Count() - 1) / 2]; 
            }
            else // Even, take middle
            {
                latMedian = latArray[(latArray.Count()) / 2];
            }
            double longMedian;
            longArray.Sort();
            if (longArray.Count % 2 == 1) // Odd number of elements in list, take middle
            {
                longMedian = longArray[(longArray.Count() - 1) / 2];
            }
            else // Even, take middle
            {
                longMedian = longArray[(longArray.Count()) / 2];
            }

            // Make request
            HttpResponseMessage message = await httpClient.GetAsync($"staticmap?center={latMedian},{longMedian}&size=400x400{markers}&key={key}");

            return await message.Content.ReadAsByteArrayAsync();
        }

        // Return GeoJSON of one restaurant
        // https://learn.microsoft.com/en-us/bingmaps/v8-web-control/modules/geojson-module/
        /* private string generateGeoJson(Guid id)
        {
            /* This method only turns one restaurant into a GeoJSON. Once this works, a foreach loop 
             * can be used on the restaurantList to do all at once */

            // Should take in one restaurant and generate a GeoJSON object
            /*GeoJSON geoJSON = new GeoJSON(restaurantList[id]);
            // Should return GeoJSON object as a string
            return geoJSON; //JsonSerializer.Serialize(geoJSON);
        } */
    }
}