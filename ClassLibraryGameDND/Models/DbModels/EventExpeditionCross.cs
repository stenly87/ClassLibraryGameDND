using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryGameDND.Models.DbModels
{
    public class EventExpeditionCross
    {
        //Специально не айди, чтобы было легче ориентироваться

        public Event Event { get; set; }

        public Expedition Expedition { get; set; }

        public Log Log { get; set; }

        public DateTime Time { get; set; }

        public int CurrentPetHP { get; set; }
    }
}
