using Microsoft.AspNetCore.Mvc;

namespace RestaurantReviewProgram.Controllers
{
    [ApiController]
    [Route("[controller]")] 
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
