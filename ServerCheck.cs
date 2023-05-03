using Microsoft.AspNetCore.Mvc;

namespace RestaurantReviewProgram.Controllers
{
    [ApiController]
    [Route("[controller]")] 
    /* Aspect oriented programming(?) Tells what URL should look like to access this-
     * Route should be name of controller -controller */
    public class ServerCheck : ControllerBase
    {
            private readonly ILogger<ServerCheck> logger; // Dependency injection

            public ServerCheck(ILogger<ServerCheck> logger)
            {
                this.logger = logger;
            }

            [HttpGet()]
            public string Get()
            {
                return "The server is currently up."; //Return string 
            }
    }
}
