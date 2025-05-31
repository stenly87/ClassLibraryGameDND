using ClassLibraryGameDND.Models.DbModels;
using ClassLibraryGameDND.Models.OtherModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        [HttpGet("GetAllLogs")]
        public List<Log> GetAllLogs()
        {
            return DataBaseContext.GetAllLogs();
        }

        [HttpPost("AddLog")]
        public void AddLog(Log log)
        {
            DataBaseContext.AddLog(log);
        }

        [HttpPost("EditLog")]
        public void EditLog(Log log)
        {
            DataBaseContext.EditLog(log);
        }

        [HttpPost("DeleteLog")]
        public void DeleteLog(Log log)
        {
            DataBaseContext.DeleteLog(log);
        }
    }
}
