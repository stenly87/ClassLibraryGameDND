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

        private static void ExecuteRequest(MySqlCommand cmd)
        {
            if (_con is null || cmd is null)
                return;

            try
            {
                int id = (int)(ulong)cmd.ExecuteNonQuery();
                if (id > 0)
                    Console.WriteLine(id.ToString());
                else
                    Console.WriteLine("Запись не добавлена");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static void AddEvent(Event ev)
        {

            //Между коментариями находится только пример запроса
            //
            var cmd = new MySqlCommand("insert into `Events` Values (0, @EventName, @Stat);", _con);
            cmd.Parameters.Add(new MySqlParameter("EventName", ev.EventName));

            MySqlParameter lname = new MySqlParameter("Stat", ev.Stat);
            cmd.Parameters.Add(lname);
            //

            ExecuteRequest(cmd);
        }

        public static void AddEventExpeditionCross(EventExpeditionCross ex, int eventId, int expId, int logId)
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

        public static void DeleteEvent(int eventId)
        {
            throw new NotImplementedException();
        }

        public static void DeleteEventExpeditionCross(int expId)
        {
            throw new NotImplementedException();
        }

        public static void DeleteExpedition(int expId)
        {
            throw new NotImplementedException();
        }

        public static void DeleteLog(int logId)
        {
            throw new NotImplementedException();
        }

        public static void DeleteMonster(int monId)
        {
            throw new NotImplementedException();
        }

        public static void EditEvent(Event ev)
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

        public static List<Expedition> GetAllExpeditionsByIdCharacter(int id)
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

        internal static List<Event> GetCompletedEventsFromCrossByExpeditionID(int id)
        {
            throw new NotImplementedException();
        }

        public static void PetCurrentHPForExpeditionCrossByExpeditionID(int id)
        {
            throw new NotImplementedException();
        }

        public static void EditExpedition(Expedition ex)
        {
            throw new NotImplementedException();
        }
    }
}
