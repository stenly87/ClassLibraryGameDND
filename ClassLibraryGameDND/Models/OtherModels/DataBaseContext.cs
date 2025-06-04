using MySqlConnector;
using ClassLibraryGameDND.Models.DbModels;
using System.Security.Cryptography;
using System.Data;

namespace ClassLibraryGameDND.Models.OtherModels
{
    public class DataBaseContext
    {
        private static MySqlConnection _con;
        public DataBaseContext(MySqlConnection connection)
            => _con = connection;

        private static void ExecuteRequest(string request, MySqlParameter[]? parameters = null)
        {
            try
            {
                if (_con is null || string.IsNullOrEmpty(request))
                    throw new ArgumentException("Пустой запрос или соединение!!");

                using var cmd = new MySqlCommand(request, _con);

                if (parameters is not null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static List<object> ExecuteSelectRequestObject(MySqlCommand cmd, Type type)
        {
            List<object> result = [];
            if (_con is null || cmd is null)
                return result;

            using var dr = cmd.ExecuteReader();
            switch (type.Name)
            {
                case "Expedition":
                    while (dr.Read())
                    {
                        var item = new Expedition
                        {
                            Id = dr.GetInt32("ID"),
                            PlayerID = dr.GetInt32("PlayerID"),
                            Pet = dr.GetString("Pet"),
                            Time = dr.GetDateTime("Time"),
                            Status = dr.GetBoolean("Status"),
                            PetHP = dr.GetInt32("PetHP"),
                            Reward = dr.GetString("Reward")
                        };
                        result.Add(item);
                    }
                    break;

                case "Event":
                    while (dr.Read())
                    {
                        var item = new Event
                        {
                            Id = dr.GetInt32("ID"),
                            EventName = dr.GetString("EventName"),
                            Stat = dr.GetString("Stat"),
                            NegEffect = dr.GetString("NegEffect"),
                            PosEffect = dr.GetString("PosEffect"),
                            NegStatChange = dr.GetInt32("NegStatChange"),
                            PosStatChange = dr.GetInt32("PosStatChange"),
                            ChangeableStat = dr.GetString("ChangeableStat")
                        };
                        result.Add(item);
                    }
                    break;

                case "Log":
                    while (dr.Read())
                    {
                        var item = new Log
                        {
                            Id = dr.GetInt32("ID"),
                            Description = dr.GetString("Description"),
                        };
                        result.Add(item);
                    }
                    break;

                case "Monster":
                    while (dr.Read())
                    {
                        var item = new Monster
                        {
                            Id = dr.GetInt32("ID"),
                            IsBoss = dr.GetBoolean("IsBoss"),
                            Name = dr.GetString("Name"),
                            Level = dr.GetInt32("Level"),
                            AC = dr.GetInt32("AC"),
                            AttackBonus = dr.GetInt32("AttackBonus"),
                            BAB = dr.GetInt32("BAB"),
                            BaseDamage = dr.GetString("BaseDamage"),
                            CON = dr.GetInt32("CON"),
                            CritHitMult = dr.GetInt32("CritHitMult"),
                            DEX = dr.GetInt32("DEX"),
                            DamageBonus = dr.GetString("DamageBonus"),
                            MaxHP = dr.GetInt32("MaxHP"),
                            STR = dr.GetInt32("STR")
                        };
                        result.Add(item);
                    }
                    break;

                default:
                    //while (dr.Read())
                    //{
                    //    Event ev = new();
                    //    Log log = new();
                    //    Expedition ex = new();

                    //    using var cmd2 = new MySqlCommand($"select * from `Events` where `ID`=@eventID;", _con);
                    //    cmd2.Parameters.Add(new MySqlParameter("eventID", dr.GetInt32("EventID")));
                    //    var events = ExecuteSelectRequestObject(cmd2, typeof(Event));
                    //    foreach (Event e in events.Cast<Event>())
                    //        ev = e;

                    //    using var cmd3 = new MySqlCommand($"select * from `Logs` where `ID`=@logID;", _con);
                    //    cmd3.Parameters.Add(new MySqlParameter("logID", dr.GetInt32("LogID")));
                    //    var logs = ExecuteSelectRequestObject(cmd2, typeof(Log));
                    //    foreach (Log lg in logs.Cast<Log>())
                    //        log = lg;

                    //    using var cmd4 = new MySqlCommand($"select * from `Expeditions` where `ID`=@expID;", _con);
                    //    cmd4.Parameters.Add(new MySqlParameter("expID", dr.GetInt32("ExpeditionID")));
                    //    var expeditions = ExecuteSelectRequestObject(cmd2, typeof(Expedition));
                    //    foreach (Expedition exp in expeditions.Cast<Expedition>())
                    //        ex = exp;

                    //    var eventExpeditionCross = new EventExpeditionCross
                    //    {
                    //        Event = ev,
                    //        Expedition = ex,
                    //        Log = log,
                    //        Time = dr.GetDateTime("Time"),
                    //        CurrentPetHP = dr.GetInt32("CurrentPetHP")
                    //    };
                    //    result.Add(eventExpeditionCross);
                    //}
                    throw new Exception("пизда");
                    
            }

            return result;
        }

        private static object ExecuteAndReturnValue(MySqlCommand cmd)
        {
            return cmd.ExecuteScalar();
        }

        public static void AddEvent(Event ev)
        {
            var request = "insert into `Events` Values (0, @EventName, @Stat);";

            List<MySqlParameter> mySqlParameters = [];

            mySqlParameters.Add(new MySqlParameter("EventName", ev.EventName));
            mySqlParameters.Add(new MySqlParameter("Stat", ev.Stat));

            ExecuteRequest(request, [.. mySqlParameters]);
            ev.Id = (int)(ulong)ExecuteAndReturnValue(new MySqlCommand("select LAST_INSERT_ID()", _con));
        }

        

        public static void AddExpedition(Expedition ex)
        {
            var request = "insert into `Expeditions` Values (@PlayerID, @Pet, @Time, @Status, 0, @PetHP, @Reward);";

            List<MySqlParameter> mySqlParameters = [];

            mySqlParameters.Add(new MySqlParameter("PlayerID", ex.PlayerID));
            mySqlParameters.Add(new MySqlParameter("Pet", ex.Pet));
            mySqlParameters.Add(new MySqlParameter("Time", ex.Time));
            mySqlParameters.Add(new MySqlParameter("Status", ex.Status));
            mySqlParameters.Add(new MySqlParameter("PetHP", ex.PetHP));
            mySqlParameters.Add(new MySqlParameter("Reward", ex.Reward));

            ExecuteRequest(request, [.. mySqlParameters]);

            ex.Id = (int)(ulong)ExecuteAndReturnValue(new MySqlCommand("select LAST_INSERT_ID()", _con));
        }

        public static void AddLog(Log log)
        {
            var request = "insert into `Logs` Values (0, @Descripton);";

            List<MySqlParameter> mySqlParameters = [];

            mySqlParameters.Add(new MySqlParameter("Description", log.Description));

            ExecuteRequest(request, [.. mySqlParameters]);
            log.Id = (int)(ulong)ExecuteAndReturnValue(new MySqlCommand("select LAST_INSERT_ID()", _con));
        }

        public static void AddMonster(Monster mon)
        {
            var request = "insert into `Monsters` Values (0, @IsBoss, @Name, @Level, @AC, @AttackBonus, @BAB, @BaseDamage, @CON, @CritHitMult, @DEX, @DamageBonus, @MaxHP, @STR);";

            List<MySqlParameter> mySqlParameters = [];

            mySqlParameters.Add(new MySqlParameter("IsBoss", mon.IsBoss));
            mySqlParameters.Add(new MySqlParameter("Name", mon.Name));
            mySqlParameters.Add(new MySqlParameter("Level", mon.Level));
            mySqlParameters.Add(new MySqlParameter("AC", mon.AC));
            mySqlParameters.Add(new MySqlParameter("AttackBonus", mon.AttackBonus));
            mySqlParameters.Add(new MySqlParameter("BAB", mon.BAB));
            mySqlParameters.Add(new MySqlParameter("BaseDamage", mon.BaseDamage));
            mySqlParameters.Add(new MySqlParameter("CON", mon.CON));
            mySqlParameters.Add(new MySqlParameter("CritHitMult", mon.CritHitMult));
            mySqlParameters.Add(new MySqlParameter("DEX", mon.DEX));
            mySqlParameters.Add(new MySqlParameter("DamageBonus", mon.DamageBonus));
            mySqlParameters.Add(new MySqlParameter("MaxHP", mon.MaxHP));
            mySqlParameters.Add(new MySqlParameter("STR", mon.STR));

            ExecuteRequest(request, [.. mySqlParameters]);
            mon.Id = (int)(ulong)ExecuteAndReturnValue(new MySqlCommand("select LAST_INSERT_ID()", _con));
        }

        public static void DeleteEvent(int eventId)
            => ExecuteRequest($"DELETE from `Events` where `ID` = {eventId};");

        public static void DeleteExpedition(int expId)
            => ExecuteRequest($"DELETE from `Expeditions` where `ID` = {expId};");

        public static void DeleteLog(int logId)
            => ExecuteRequest($"DELETE from `Logs` where `ID` = {logId};");

        public static void DeleteMonster(int monId)
            => ExecuteRequest($"DELETE from `Monsters` where `ID` = {monId};");

        public static void EditEvent(Event ev)
            => ExecuteRequest($"update `Events` set `EventName`={ev.EventName}, `Stat`={ev.Stat}, `NegEffect`={ev.NegEffect}, `PosEffect`={ev.PosEffect},`NegStatChange`={ev.NegStatChange}, `PosStatChange`={ev.PosStatChange}, `ChangeableStat`={ev.ChangeableStat} where `ID` = {ev.Id}");
        public static void EditExpedition(Expedition ex)
            => ExecuteRequest($"update `Expeditions` set `PlayerID`={ex.PlayerID},`Pet`={ex.Pet},`Time`={ex.Time},`Status`={ex.Status},`PetHP`={ex.PetHP},`Reward`={ex.Reward} where `ID` = {ex.Id}");

        public static void EditLog(Log log)
        {
            using var cmd = new MySqlCommand($"update `Logs` set `Description`={log.Description} where `ID` = {log.Id}");
            ExecuteRequest(cmd.CommandText);
        }

        public static void EditMonster(Monster mon)
        {
            using var cmd = new MySqlCommand($"update `Monsters` set `IsBoss`={mon.IsBoss}, `Name`={mon.Name},`Level`={mon.Level},`AC`={mon.AC},`AttackBonus`={mon.AttackBonus},`BAB`={mon.BAB},`BaseDamage`={mon.BaseDamage},`CON`={mon.CON},`CritHitMult`={mon.CritHitMult},`DEX`={mon.DEX},`DamageBonus`={mon.DamageBonus},`MaxHP`={mon.MaxHP},`STR`={mon.STR} where `ID` = {mon.Id}");
            ExecuteRequest(cmd.CommandText);
        }

        public static List<Event> GetAllEvents()
            => ExecuteSelectRequestObject(new MySqlCommand($"select * from `Events` where ID > 1;", _con), typeof(Event)).Cast<Event>().ToList();

       
        public static List<Log> GetAllLogs()
            => ExecuteSelectRequestObject(new MySqlCommand($"select * from `Logs`;", _con), typeof(Log)).Cast<Log>().ToList();

        public static List<Monster> GetAllMonsters()
            => ExecuteSelectRequestObject(new MySqlCommand($"select * from `Monsters`;", _con), typeof(Monster)).Select(s=>(Monster)s).ToList();

        public static List<CompleteEvent> GetCompletedEventsFromCrossByExpeditionID(int id)
            => ExecuteSelectRequestForCompleteEvent(new MySqlCommand($"SELECT  e.EventName, eec.Time, eec.CurrentPetHP, eec.LogID FROM  EventExpeditionCross eec JOIN Events e ON eec.EventID = e.ID  WHERE eec.ExpeditionID = {id} and eec.Time < CURRENT_TIMESTAMP()  ORDER BY eec.Time;", _con));

        private static List<CompleteEvent> ExecuteSelectRequestForCompleteEvent(MySqlCommand mySqlCommand)
        {
            List<CompleteEvent> result = [];
            if (_con is null || mySqlCommand is null)
                return result;

            using var dr = mySqlCommand.ExecuteReader();

            while (dr.Read())
            {
                var item = new CompleteEvent
                {
                     CurrentPetHP = dr.GetInt32("CurrentPetHP"),
                     EventName = dr.GetString("EventName"),
                     LogId = dr.GetInt32("LogID"),
                     Time = dr.GetDateTime("Time")
                };
                result.Add(item);
            }
            return result;
        }

        public static Expedition GetExpeditionByCharacterID(int id)
            => ExecuteSelectRequestObject(new MySqlCommand($"select * from `Expeditions` where `PlayerID` = {id} and `Status` = false ;", _con), typeof(Expedition)).Cast<Expedition>().Last();
        

        public static void AddEventExpeditionCross(EventExpeditionCross ex)
        {
            var request = "insert into `EventExpeditionCross` Values (@EventID, @ExpeditionID, @LogID, @Time, @CurrentPetHP);";

            List<MySqlParameter> mySqlParameters = [];

            mySqlParameters.Add(new MySqlParameter("EventID", ex.Event.Id));
            mySqlParameters.Add(new MySqlParameter("ExpeditionID", ex.Expedition.Id));
            mySqlParameters.Add(new MySqlParameter("LogID", ex.Log.Id));
            mySqlParameters.Add(new MySqlParameter("Time", ex.Time));
            mySqlParameters.Add(new MySqlParameter("CurrentPetHP", ex.CurrentPetHP));
            

            ExecuteRequest(request, [.. mySqlParameters]);
        }

        public static string GetLogFromCrossByLogId(int id)
        {
            return (string?)ExecuteAndReturnValue(new MySqlCommand($"SELECT  l.Description from EventExpeditionCross eec JOIN  Logs l ON eec.LogID = l.ID WHERE eec.LogID = {id}", _con)) ?? "";
        }
             
      
    }
}
