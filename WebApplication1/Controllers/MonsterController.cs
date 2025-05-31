using ClassLibraryGameDND.Models.DbModels;
using ClassLibraryGameDND.Models.OtherModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonsterController : ControllerBase
    {
        [HttpGet("GetAllMonsters")]
        public List<Monster> GetAllMonsters()
        {
            return DataBaseContext.GetAllMonsters();
        }

        [HttpPost("AddMonster")]
        public void AddMonster(Monster mon)
        {
            DataBaseContext.AddMonster(mon);
        }

        [HttpPost("EditExpedition")]
        public void EditMonster(Monster mon)
        {
            DataBaseContext.EditMonster(mon);
        }

        [HttpPost("DeleteExpedition")]
        public void DeleteMonster(Monster mon)
        {
            DataBaseContext.DeleteMonster(mon);
        }
    }
}
