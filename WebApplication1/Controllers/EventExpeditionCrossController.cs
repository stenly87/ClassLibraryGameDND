using ClassLibraryGameDND.Models.DbModels;
using ClassLibraryGameDND.Models.OtherModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventExpeditionCrossController : ControllerBase
    {

        [HttpPost("AddEventExpeditionCross")]
        public void AddEventExpeditionCross(EventExpeditionCross ex)
        {
            DataBaseContext.AddEventExpeditionCross(ex);
        }


       
    }
}
