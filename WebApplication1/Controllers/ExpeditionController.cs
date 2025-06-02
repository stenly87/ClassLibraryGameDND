using ClassLibraryGameDND.Models.DbModels;
using ClassLibraryGameDND.Models.OtherModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpeditionController : ControllerBase
    {
        [HttpGet("GetAllExpeditionsByIdCharacter/{id}")]
        public List<Expedition> GetAllExpeditionsByIdCharacter(int id)
        {
            return DataBaseContext.GetAllExpeditionsByIdCharacter(id);
        }

        [HttpPost("AddExpedition")]
        public void AddExpedition(Expedition ex)
        {
            DataBaseContext.AddExpedition(ex);
        }
        
        [HttpPost("EditExpedition")]
        public void EditExpedition(Expedition ex)
        {
            DataBaseContext.EditExpedition(ex);
        }



        [HttpPost("DeleteExpedition")]
        public void DeleteExpedition(int expId)
        {
            DataBaseContext.DeleteExpedition(expId);
        }
    }
}
