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

        public static void AddExpedition(Expedition ex)
        {
            throw new NotImplementedException();
        }

        public static void AddLog(Log log)
        {
            throw new NotImplementedException();
        }

        public static void AddMonster(Monster mon)
        {
            throw new NotImplementedException();
        }

        public static void DeleteEvent(Event ev)
        {
            throw new NotImplementedException();
        }

        public static void DeleteExpedition(Expedition ex)
        {
            throw new NotImplementedException();
        }

        public static void DeleteLog(Log log)
        {
            throw new NotImplementedException();
        }

        public static void DeleteMonster(Monster mon)
        {
            throw new NotImplementedException();
        }

        public static void EditEvent(Event ev)
        {
            throw new NotImplementedException();
        }

        public static void EditExpedition(Expedition ex)
        {
            throw new NotImplementedException();
        }

        public static void EditLog(Log log)
        {
            throw new NotImplementedException();
        }

        public static void EditMonster(Monster mon)
        {
            throw new NotImplementedException();
        }

        public static List<Event> GetAllEvents()
        {
            throw new NotImplementedException();
        }

        public static List<Expedition> GetAllExpeditions()
        {
            throw new NotImplementedException();
        }

        public static List<Log> GetAllLogs()
        {
            throw new NotImplementedException();
        }

        public static List<Monster> GetAllMonsters()
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
