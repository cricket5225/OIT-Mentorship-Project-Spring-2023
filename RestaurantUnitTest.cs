using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using RestaurantReviewProgram.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using RestaurantReviewProgram.Models;
using Moq;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Net.Http;
using Moq.Contrib.HttpClient;
using Microsoft.AspNetCore.Mvc;

namespace UnitTesting
{
    public class RestaurantUnitTest
    {
        // createRestaurant
        [Fact]
        public void restaurantCreation()
        {
            // Arrange
            RestaurantList restaurantList = new RestaurantList();
            ILogger<RestaurantCreator> logger = new NullLogger<RestaurantCreator>();
            // Create a new Secret to replace APIKey
            IOptions<Secret> options = Options.Create(
                new Secret
                {
                    ApiKey = "My API Key"
                }
            );
            // Mock API response
            Mock<HttpMessageHandler> handler = new Mock<HttpMessageHandler>();
            handler.SetupAnyRequest().ReturnsJsonResponse(new GeocodeResponse
            {
                Results = new List<GeocodeResult>
                {
                    new GeocodeResult
                    {
                        Geometry = new GeocodeResultGeometry
                        {
                            Location = new GeocodeResultLocation
                            {
                                Lat = 55,
                                Lng = 50
                            }
                        }
                    }
                }
            });

            IHttpClientFactory factory = handler.CreateClientFactory();
            Mock.Get(factory).Setup(x => x.CreateClient("Google_Maps"))
                .Returns(() =>
                {
                    var client = handler.CreateClient();
                    client.BaseAddress = new Uri("https://maps.googleapis.com/maps/api/");
                    return client;
                });

            RestaurantCreator restaurantCreator = new RestaurantCreator(restaurantList, logger, factory, options);


            // Create restaurant
            Restaurant restaurant = new Restaurant("Local Woodfire Grill","5315 Windward Parkway","Alpharetta","GA","30004");
            // Mock geocode return
            

            // Act
            ActionResult<Restaurant> result = restaurantCreator.createRestaurant(restaurant).Result;

            // Assert
            // Check something was created
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.IsType<CreatedResult>(result.Result);

            // Cast to Created Result
            CreatedResult createdResult = (CreatedResult)result.Result;
            // Something created
            Assert.NotNull(createdResult);
            // Check right type before casting to Restaurant
            Assert.IsType<Restaurant>(createdResult.Value);

            // Cast to Restaurant
            Restaurant restaurantResult = (Restaurant)createdResult.Value;
            // Test all properties
            Assert.Equal("Local Woodfire Grill", restaurantResult.Name);
            Assert.NotEqual(Guid.Empty, restaurantResult.Id);
            Assert.Equal("5315 Windward Parkway", restaurantResult.Address);
            Assert.Equal("Alpharetta", restaurantResult.City);
            Assert.Equal("GA", restaurantResult.State);
            Assert.Equal("30004", restaurantResult.Zip);
            Assert.Equal(55, restaurantResult.Latitude);
            Assert.Equal(50, restaurantResult.Longitude);
        }
        // updateRestaurant
        [Fact]
        public void restaurantUpdating()
        {
            // Arrange
            RestaurantList restaurantList = new RestaurantList();
            ILogger<RestaurantCreator> logger = new NullLogger<RestaurantCreator>();
            // Create a new Secret to replace APIKey
            IOptions<Secret> options = Options.Create(
                new Secret
                {
                    ApiKey = "My API Key"
                }
            );
            // Mock API response
            Mock<HttpMessageHandler> handler = new Mock<HttpMessageHandler>();
            handler.SetupAnyRequest().ReturnsJsonResponse(new GeocodeResponse
            {
                Results = new List<GeocodeResult>
                {
                    new GeocodeResult
                    {
                        Geometry = new GeocodeResultGeometry
                        {
                            Location = new GeocodeResultLocation
                            {
                                Lat = 50,
                                Lng = 55
                            }
                        }
                    }
                }
            });

            IHttpClientFactory factory = handler.CreateClientFactory();
            Mock.Get(factory).Setup(x => x.CreateClient("Google_Maps"))
                .Returns(() =>
                {
                    var client = handler.CreateClient();
                    client.BaseAddress = new Uri("https://maps.googleapis.com/maps/api/");
                    return client;
                });

            RestaurantCreator restaurantCreator = new RestaurantCreator(restaurantList, logger, factory, options);

            // Create restaurant
            Restaurant restaurant = new Restaurant("Local Woodfire Grill", "5315 Windward Parkway", "Alpharetta", "GA", "30004");
            Guid id = Guid.NewGuid();
            restaurant.Latitude = 60;
            restaurant.Longitude = 65;
            restaurantList.Add(id, restaurant);

            // Create "updated" version of same restaurant- Same Guid id as before
            Restaurant restaurantV2 = new Restaurant("Local Woodfire Grill (Sandy Springs)", "1110 Hammond Drive", "Sandy Springs", "WI", "30328");
            restaurantV2.Id = id;

            // Act
            ActionResult<Restaurant> result = restaurantCreator.updateRestaurant(restaurantV2).Result;

            // Assert
            // Check something was created
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<Restaurant>(result.Value);

            // Cast to Restaurant
            Restaurant restaurantResult = (Restaurant)result.Value;
            // Test all properties
            Assert.Equal("Local Woodfire Grill (Sandy Springs)", restaurantResult.Name);
            Assert.Equal(id, restaurantResult.Id);
            Assert.Equal("1110 Hammond Drive", restaurantResult.Address);
            Assert.Equal("Sandy Springs", restaurantResult.City);
            Assert.Equal("WI", restaurantResult.State);
            Assert.Equal("30328", restaurantResult.Zip);
            Assert.Equal(50, restaurantResult.Latitude);
            Assert.Equal(55, restaurantResult.Longitude);
        }
        // getRestaurant
        [Fact]
        public void restaurantGetAll()
        {
            // Arrange
            RestaurantList restaurantList = new RestaurantList();
            ILogger<RestaurantCreator> logger = new NullLogger<RestaurantCreator>();
            // Create a new Secret to replace APIKey
            IOptions<Secret> options = Options.Create(
                new Secret
                {
                    ApiKey = "My API Key"
                }
            );
            // Mock API response
            Mock<HttpMessageHandler> handler = new Mock<HttpMessageHandler>();
            handler.SetupAnyRequest().ReturnsJsonResponse(new GeocodeResponse
            {
                Results = new List<GeocodeResult>
                {
                    new GeocodeResult
                    {
                        Geometry = new GeocodeResultGeometry
                        {
                            Location = new GeocodeResultLocation
                            {
                                Lat = 55,
                                Lng = 50
                            }
                        }
                    }
                }
            });

            IHttpClientFactory factory = handler.CreateClientFactory();
            Mock.Get(factory).Setup(x => x.CreateClient("Google_Maps"))
                .Returns(() =>
                {
                    var client = handler.CreateClient();
                    client.BaseAddress = new Uri("https://maps.googleapis.com/maps/api/");
                    return client;
                });

            RestaurantCreator restaurantCreator = new RestaurantCreator(restaurantList, logger, factory, options);

            // Create restaurant
            Restaurant restaurant1 = new Restaurant("Local Woodfire Grill", "5315 Windward Parkway", "Alpharetta", "GA", "30004");
            restaurant1.Latitude = 50;
            restaurant1.Longitude = 55;
            Guid id1 = Guid.NewGuid();
            restaurant1.Id = id1;
            restaurantList.Add(id1, restaurant1);
            // Create second restaurant
            Restaurant restaurant2 = new Restaurant("Local Woodfire Grill (Sandy Springs)", "1110 Hammond Drive", "Sandy Springs", "WI", "30328");
            Guid id2 = Guid.NewGuid();
            restaurant2.Id = id2;
            restaurantList.Add(id2, restaurant2);
            restaurant2.Latitude = 60;
            restaurant2.Longitude = 65;

            // Act
            List<Restaurant> result = restaurantCreator.getRestaurant();

            // Assert
            // Check something was created
            Assert.NotNull(result);
            // Check right type- Restaurant List
            Assert.IsType<List<Restaurant>>(result);
            // Test all properties of [0]
            Assert.IsType<Restaurant>(result[0]);
            Assert.Equal("Local Woodfire Grill", result[0].Name);
            Assert.Equal(id1, result[0].Id);
            Assert.Equal("5315 Windward Parkway", result[0].Address);
            Assert.Equal("Alpharetta", result[0].City);
            Assert.Equal("GA", result[0].State);
            Assert.Equal("30004", result[0].Zip);
            Assert.Equal(50, result[0].Latitude);
            Assert.Equal(55, result[0].Longitude);
            // Test all properties of [1]
            Assert.IsType<Restaurant>(result[1]);
            Assert.Equal("Local Woodfire Grill (Sandy Springs)", result[1].Name);
            Assert.Equal(id2, result[1].Id);
            Assert.Equal("1110 Hammond Drive", result[1].Address);
            Assert.Equal("Sandy Springs", result[1].City);
            Assert.Equal("WI", result[1].State);
            Assert.Equal("30328", result[1].Zip);
            Assert.Equal(60, result[1].Latitude);
            Assert.Equal(65, result[1].Longitude);
        }
        // getRestaurant(guid)
        [Fact]
        public void restaurantGetOne()
        {
            // Arrange
            RestaurantList restaurantList = new RestaurantList();
            ILogger<RestaurantCreator> logger = new NullLogger<RestaurantCreator>();
            // Create a new Secret to replace APIKey
            IOptions<Secret> options = Options.Create(
                new Secret
                {
                    ApiKey = "My API Key"
                }
            );
            // Mock API response
            Mock<HttpMessageHandler> handler = new Mock<HttpMessageHandler>();
            handler.SetupAnyRequest().ReturnsJsonResponse(new GeocodeResponse
            {
                Results = new List<GeocodeResult>
                {
                    new GeocodeResult
                    {
                        Geometry = new GeocodeResultGeometry
                        {
                            Location = new GeocodeResultLocation
                            {
                                Lat = 55,
                                Lng = 50
                            }
                        }
                    }
                }
            });

            IHttpClientFactory factory = handler.CreateClientFactory();
            Mock.Get(factory).Setup(x => x.CreateClient("Google_Maps"))
                .Returns(() =>
                {
                    var client = handler.CreateClient();
                    client.BaseAddress = new Uri("https://maps.googleapis.com/maps/api/");
                    return client;
                });

            RestaurantCreator restaurantCreator = new RestaurantCreator(restaurantList, logger, factory, options);

            // Create restaurant
            Restaurant restaurant = new Restaurant("Local Woodfire Grill", "5315 Windward Parkway", "Alpharetta", "GA", "30004");
            Guid id = Guid.NewGuid();
            restaurant.Id = id;
            restaurant.Latitude = 50;
            restaurant.Longitude = 55;
            restaurantList.Add(id, restaurant);

            // Act
            ActionResult<Restaurant> result = restaurantCreator.getRestaurant(id);

            // Assert
            // Check something was created
            Assert.NotNull(result);
            Assert.IsType<Restaurant>(result.Value);

            // Cast to Restaurant
            Restaurant restaurantResult = (Restaurant)result.Value;
            // Test all properties
            Assert.Equal("Local Woodfire Grill", restaurantResult.Name);
            Assert.Equal(id, restaurantResult.Id);
            Assert.Equal("5315 Windward Parkway", restaurantResult.Address);
            Assert.Equal("Alpharetta", restaurantResult.City);
            Assert.Equal("GA", restaurantResult.State);
            Assert.Equal("30004", restaurantResult.Zip);
            Assert.Equal(50, restaurantResult.Latitude);
            Assert.Equal(55, restaurantResult.Longitude);
        }
        // createMap
        [Fact]
        public void restaurantCreateMap()
        {
            // Arrange
            RestaurantList restaurantList = new RestaurantList();
            ILogger<RestaurantCreator> logger = new NullLogger<RestaurantCreator>();
            // Create a new Secret to replace APIKey
            IOptions<Secret> options = Options.Create(
                new Secret
                {
                    ApiKey = "My API Key"
                }
            );
            // Mock API response
            Mock<HttpMessageHandler> handler = new Mock<HttpMessageHandler>();
            handler.SetupAnyRequest().ReturnsJsonResponse(new GeocodeResponse
            {
                Results = new List<GeocodeResult>
                {
                    new GeocodeResult
                    {
                        Geometry = new GeocodeResultGeometry
                        {
                            Location = new GeocodeResultLocation
                            {
                                Lat = 55,
                                Lng = 50
                            }
                        }
                    }
                }
            });

            IHttpClientFactory factory = handler.CreateClientFactory();
            Mock.Get(factory).Setup(x => x.CreateClient("Google_Maps"))
                .Returns(() =>
                {
                    var client = handler.CreateClient();
                    client.BaseAddress = new Uri("https://maps.googleapis.com/maps/api/");
                    return client;
                });

            RestaurantCreator restaurantCreator = new RestaurantCreator(restaurantList, logger, factory, options);

            // Create restaurant
            Restaurant restaurant1 = new Restaurant("Local Woodfire Grill", "5315 Windward Parkway", "Alpharetta", "GA", "30004");
            Guid id1 = Guid.NewGuid();
            restaurant1.Id = id1;
            restaurantList.Add(id1, restaurant1);
            // Create second restaurant
            Restaurant restaurant2 = new Restaurant("Local Woodfire Grill (Sandy Springs)", "1110 Hammond Drive", "Sandy Springs", "WI", "30328");
            Guid id2 = Guid.NewGuid();
            restaurant2.Id = id2;
            restaurantList.Add(id2, restaurant2);

            // Act
            FileContentResult result = restaurantCreator.createMap().Result;

            // Assert
            // Check something was created
            Assert.NotNull(result);
            //Assert.NotNull(result.Result);
            Assert.IsType<FileContentResult>(result);
            Assert.Equal("image/png",result.ContentType);
        }
        // createGeoJSON
        [Fact]
        public void restaurantCreateGeoJSON()
        {
            // Arrange
            RestaurantList restaurantList = new RestaurantList();
            ILogger<RestaurantCreator> logger = new NullLogger<RestaurantCreator>();
            // Create a new Secret to replace APIKey
            IOptions<Secret> options = Options.Create(
                new Secret
                {
                    ApiKey = "My API Key"
                }
            );
            // Mock API response
            Mock<HttpMessageHandler> handler = new Mock<HttpMessageHandler>();
            handler.SetupAnyRequest().ReturnsJsonResponse(new GeocodeResponse
            {
                Results = new List<GeocodeResult>
                {
                    new GeocodeResult
                    {
                        Geometry = new GeocodeResultGeometry
                        {
                            Location = new GeocodeResultLocation
                            {
                                Lat = 55,
                                Lng = 50
                            }
                        }
                    }
                }
            });

            IHttpClientFactory factory = handler.CreateClientFactory();
            Mock.Get(factory).Setup(x => x.CreateClient("Google_Maps"))
                .Returns(() =>
                {
                    var client = handler.CreateClient();
                    client.BaseAddress = new Uri("https://maps.googleapis.com/maps/api/");
                    return client;
                });

            RestaurantCreator restaurantCreator = new RestaurantCreator(restaurantList, logger, factory, options);

            // Create restaurant
            Restaurant restaurant = new Restaurant("Local Woodfire Grill", "5315 Windward Parkway", "Alpharetta", "GA", "30004");
            Guid id = Guid.NewGuid();
            restaurant.Id = id;
            restaurantList.Add(id ,restaurant);

            // Act
            ActionResult<GeoJSON> result = restaurantCreator.createGeoJSON(id);

            // Assert
            // Check something was created
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<GeoJSON>(result.Value);

            // Cast to File Content
            GeoJSON geoJSON = (GeoJSON)result.Value;
            // Something created
            Assert.NotNull(geoJSON);
        }
    }
}
