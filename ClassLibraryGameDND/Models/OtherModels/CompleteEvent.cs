using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryGameDND.Models.OtherModels
{
    public class CompleteEvent
    {
        public int ID { get; set; }
        public string EventName { get; set; }
        public DateTime Time { get; set; }
        public int CurrentPetHP { get; set; }
        public int LogId { get; set; }
    }
}
