using ClassLibraryGameDND.Models.DbModels;
using ClassLibraryGameDND.Models.OtherModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        [HttpGet("GetAllEvents")]
        public List<Event> GetAllEvents()
        {
            return DataBaseContext.GetAllEvents();
        }

        [HttpPost("AddEvent")]
        public void AddEvent(Event ev)
        {
            DataBaseContext.AddEvent(ev);
        }

        [HttpPost("EditEvent")]
        public void EditEvent(Event ev)
        {
            DataBaseContext.EditEvent(ev);
        }

        [HttpPost("DeleteEvent")]
        public void DeleteEvent(int eventId)
        {
            DataBaseContext.DeleteEvent(eventId);
        }
    }
}
