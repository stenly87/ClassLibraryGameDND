using ClassLibraryGameDND;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly DNDWalkingPet dnd;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, DNDWalkingPet dnd)
        {
            _logger = logger;
            this.dnd = dnd;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public ActionResult<string> Get()
        {
            return dnd.Test();
        }
    }
}
