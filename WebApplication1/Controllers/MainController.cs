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
        public string AddExpedition([FromBody] Pet pet)
        {
            var dnd = new DNDWalkingPet(MySqlDB.Create());
            if (dnd.CheckExpreditionExist(pet.Character))
                return "Питомец уже гуляет";

            dnd.AddExpedition(pet);            
            return "Питомец отправился гулять";
        }

        [HttpPost("GetLog")]
        public string GetLog(int logId)
            => new DNDWalkingPet(MySqlDB.Create()).GetLog(logId);

    }
}
