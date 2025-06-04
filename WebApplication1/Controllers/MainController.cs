using ClassLibraryGameDND.Models.DbModels;
using ClassLibraryGameDND;
using ClassLibraryGameDND.Models.OtherModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        [HttpPost("GetCurrentStatus")]
        public Status GetCurrentStatus(int characterId)
            => new DNDWalkingPet(MySqlDB.Create()).GetStatus(characterId);

        [HttpPost("AddExpedition")]
        public void AddExpedition([FromBody]Pet pet)
            => new DNDWalkingPet(MySqlDB.Create()).AddExpedition(pet);

        [HttpPost("GetLog")]
        public string GetLog(int logId)
            => new DNDWalkingPet(MySqlDB.Create()).GetLog(logId);

    }
}
