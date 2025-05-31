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
        [HttpGet("GetAllExpeditions")]
        public List<Expedition> GetAllExpeditions()
        {
            return DataBaseContext.GetAllExpeditions();
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
        public void DeleteExpedition(Expedition ex)
        {
            DataBaseContext.DeleteExpedition(ex);
        }
    }
}
