using MySqlConnector;
using System;
using System.Diagnostics.Metrics;
using System.Reflection;
using System.Text;
using ClassLibraryGameDND.Models.DbModels;
using Microsoft.Extensions.Logging;

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
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static List<object> ExecuteSelectRequestObject(MySqlCommand cmd, Type type)
        {
            List<TValue> result;
            if (_con is null || cmd is null)
                return result;
            var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                var request = cmd.CommandText.Split('\'')[1];

                switch (request)
                {
                    case "Expeditions":
                        result = (List<TValue>?)Activator.CreateInstance(typeof(Expedition));
                        result.Add(new Expedition());
                        break;
                    case "Logs":
                        break;
                    case "Monsters":
                        break;
                    case "Events":
                        break;
                    case "EventExpeditionCross":
                        break;
                }

                        var eventExpeditionCross = new EventExpeditionCross
                        {
                            Event = ev,
                            Expedition = ex,
                            Log = log,
                            Time = dr.GetDateTime("Time"),
                            CurrentPetHP = dr.GetInt32("CurrentPetHP")
                        };
                        result.Add(eventExpeditionCross);
                    }
                    break;
            }
            return result;
        }



        public static void AddEvent(Event ev)
        {

            var cmd = new MySqlCommand("insert into `Events` Values (0, @EventName, @Stat);", _con);
            cmd.Parameters.Add(new MySqlParameter("EventName", ev.EventName));

            MySqlParameter lname = new MySqlParameter("Stat", ev.Stat);
            cmd.Parameters.Add(lname);

            ExecuteRequest(cmd);
        }

        public static void AddEventExpeditionCross(EventExpeditionCross ex, int eventId, int expId, int logId)
        {
            throw new NotImplementedException();
        }

        public static void AddExpedition(Expedition ex)
        {
            var cmd = new MySqlCommand("insert into `Expeditions` Values (0, @PlayerID, @Pet, @Time, @Status, @PetHP, @Reward);", _con);
            cmd.Parameters.Add(new MySqlParameter("PlayerID", ex.PlayerID));
            cmd.Parameters.Add(new MySqlParameter("Pet", ex.Pet));
            cmd.Parameters.Add(new MySqlParameter("Time", ex.Time));
            cmd.Parameters.Add(new MySqlParameter("Status", ex.Status));
            cmd.Parameters.Add(new MySqlParameter("PetHP", ex.PetHP));
            cmd.Parameters.Add(new MySqlParameter("Reward", ex.Reward));

            ExecuteRequest(cmd);
        }

        public static void AddLog(Log log)
        {
            var cmd = new MySqlCommand("insert into `Logs` Values (0, @Descripton);", _con);
            cmd.Parameters.Add(new MySqlParameter("Description", log.Description));

            ExecuteRequest(cmd);
        }

        public static void AddMonster(Monster mon)
        {
            var cmd = new MySqlCommand("insert into `Monsters` Values (0, @IsBoss, @Name, @Level, @AC, @AttackBonus, @BAB, @BaseDamage, @CON, @CritHitMult, @DEX, @DamageBonus, @MaxHP, @STR);", _con);
            cmd.Parameters.Add(new MySqlParameter("IsBoss", mon.IsBoss));
            cmd.Parameters.Add(new MySqlParameter("Name", mon.Name));
            cmd.Parameters.Add(new MySqlParameter("Level", mon.Level));
            cmd.Parameters.Add(new MySqlParameter("AC", mon.AC));
            cmd.Parameters.Add(new MySqlParameter("AttackBonus", mon.AttackBonus));
            cmd.Parameters.Add(new MySqlParameter("BAB", mon.BAB));
            cmd.Parameters.Add(new MySqlParameter("BaseDamage", mon.BaseDamage));
            cmd.Parameters.Add(new MySqlParameter("CON", mon.CON));
            cmd.Parameters.Add(new MySqlParameter("CritHitMult", mon.CritHitMult));
            cmd.Parameters.Add(new MySqlParameter("DEX", mon.DEX));
            cmd.Parameters.Add(new MySqlParameter("DamageBonus", mon.DamageBonus));
            cmd.Parameters.Add(new MySqlParameter("MaxHP", mon.MaxHp));
            cmd.Parameters.Add(new MySqlParameter("STR", mon.STR));

            ExecuteRequest(cmd);
        }

        public static void DeleteEvent(int eventId)
        {
            var cmd = new MySqlCommand($"DELETE from `Events` where `ID` = {eventId};", _con);

            ExecuteRequest(cmd);
        }

        public static void DeleteEventExpeditionCross(int expId)
        {
            throw new NotImplementedException();
        }

        public static void DeleteExpedition(int expId)
        {
            var cmd = new MySqlCommand($"DELETE from `Expeditions` where `ID` = {expId};", _con);

            ExecuteRequest(cmd);
        }

        public static void DeleteLog(int logId)
        {
            var cmd = new MySqlCommand($"DELETE from `Logs` where `ID` = {logId};", _con);

            ExecuteRequest(cmd);
        }

        public static void DeleteMonster(int monId)
        {
            var cmd = new MySqlCommand($"DELETE from `Monsters` where `ID` = {monId};", _con);

            ExecuteRequest(cmd);
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
            var cmd = new MySqlCommand($"select * from `Events`;", _con);

            return ExecuteSelectRequestObject(cmd, typeof(Event));
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
