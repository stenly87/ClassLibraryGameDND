using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibraryGameDND.Models.DbModels;
using MySqlConnector;

namespace ClassLibraryGameDND.Models.OtherModels
{
    public class DataBaseContext
    {
        private static MySqlConnection _con;
        public DataBaseContext(MySqlConnection connection)
            => _con = connection;

        public static void AddEvent(Event ev)
        {
            throw new NotImplementedException();
        }

        public static void DeleteEvent(Event ev)
        {
            throw new NotImplementedException();
        }

        public static void EditEvent(Event ev)
        {
            throw new NotImplementedException();
        }

        public static List<Event> GetAllEvents()
        {
            throw new NotImplementedException();
        }

        public static List<Pet> GetCharacterPets()
        {
            throw new NotImplementedException();
        }

        public static Expedition GetExpeditionByPetCharacterID(int iD)
        {
            throw new NotImplementedException();
        }

        public static int GetPetCurrentHPFromCrossByExpeditionID(int id)
        {
            throw new NotImplementedException();
        }
    }
}
