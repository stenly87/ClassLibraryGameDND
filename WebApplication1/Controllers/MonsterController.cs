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

        [HttpPost("EditMonster")]
        public void EditMonster(Monster mon)
        {
            DataBaseContext.EditMonster(mon);
        }

        [HttpPost("DeleteMonster/{monId}")]
        public void DeleteMonster(int monId)
        {
            DataBaseContext.DeleteMonster(monId);
        }
    }
}
