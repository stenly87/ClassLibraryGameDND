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
        public string GetCurrentStatus(Character ch)
            => new DNDWalkingPet(MySqlDB.Create()).GetStatus(ch);

        [HttpPost("AddExpedition")]
        public void AddExpedition(string pet)
            => new DNDWalkingPet(MySqlDB.Create()).AddExpedition(PetParser.PetParse(pet).Character, pet);
    }
}
