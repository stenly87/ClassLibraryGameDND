using MySqlConnector;
using ClassLibraryGameDND.Models.DbModels;

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

                var cmd = new MySqlCommand(request, _con);

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

            var dr = cmd.ExecuteReader();
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
                            MaxHp = dr.GetInt32("MaxHP"),
                            STR = dr.GetInt32("STR")
                        };
                        result.Add(item);
                    }
                    break;

                default:
                    while (dr.Read())
                    {
                        Event ev = new();
                        Log log = new();
                        Expedition ex = new();

                        var cmd2 = new MySqlCommand($"select * from `Events` where `ID`=@eventID;", _con);
                        cmd2.Parameters.Add(new MySqlParameter("eventID", dr.GetInt32("EventID")));
                        var events = ExecuteSelectRequestObject(cmd2, typeof(Event));
                        foreach (Event e in events.Cast<Event>())
                            ev = e;

                        var cmd3 = new MySqlCommand($"select * from `Logs` where `ID`=@logID;", _con);
                        cmd3.Parameters.Add(new MySqlParameter("logID", dr.GetInt32("LogID")));
                        var logs = ExecuteSelectRequestObject(cmd2, typeof(Log));
                        foreach (Log lg in logs.Cast<Log>())
                            log = lg;

                        var cmd4 = new MySqlCommand($"select * from `Expeditions` where `ID`=@expID;", _con);
                        cmd4.Parameters.Add(new MySqlParameter("expID", dr.GetInt32("ExpeditionID")));
                        var expeditions = ExecuteSelectRequestObject(cmd2, typeof(Expedition));
                        foreach (Expedition exp in expeditions.Cast<Expedition>())
                            ex = exp;

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
            var request = "insert into `Events` Values (0, @EventName, @Stat);";

            List<MySqlParameter> mySqlParameters = [];

            mySqlParameters.Add(new MySqlParameter("EventName", ev.EventName));
            mySqlParameters.Add(new MySqlParameter("Stat", ev.Stat));

            ExecuteRequest(request, [.. mySqlParameters]);
        }

        public static void AddEventExpeditionCross(EventExpeditionCross ex)
        {
            throw new NotImplementedException();
        }

        public static void AddExpedition(Expedition ex)
        {
            var request = "insert into `Expeditions` Values (0, @PlayerID, @Pet, @Time, @Status, @PetHP, @Reward);";

            List<MySqlParameter> mySqlParameters = [];

            mySqlParameters.Add(new MySqlParameter("PlayerID", ex.PlayerID));
            mySqlParameters.Add(new MySqlParameter("Pet", ex.Pet));
            mySqlParameters.Add(new MySqlParameter("Time", ex.Time));
            mySqlParameters.Add(new MySqlParameter("Status", ex.Status));
            mySqlParameters.Add(new MySqlParameter("PetHP", ex.PetHP));
            mySqlParameters.Add(new MySqlParameter("Reward", ex.Reward));

            ExecuteRequest(request, [.. mySqlParameters]);
        }

        public static void AddLog(Log log)
        {
            var request = "insert into `Logs` Values (0, @Descripton);";

            List<MySqlParameter> mySqlParameters = [];

            mySqlParameters.Add(new MySqlParameter("Description", log.Description));

            ExecuteRequest(request, [.. mySqlParameters]);
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
            mySqlParameters.Add(new MySqlParameter("MaxHP", mon.MaxHp));
            mySqlParameters.Add(new MySqlParameter("STR", mon.STR));

            ExecuteRequest(request, [.. mySqlParameters]);
        }

        public static void DeleteEvent(int eventId)
            => ExecuteRequest($"DELETE from `Events` where `ID` = {eventId};");

        //!!
        public static void DeleteEventExpeditionCross(int expId)
            => ExecuteRequest($"DELETE from `EventExpeditionCross` where `ID` = {expId};");

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
            var cmd = new MySqlCommand($"update `Logs` set `Description`={log.Description} where `ID` = {log.Id}");
            ExecuteRequest(cmd);
        }

        public static void EditMonster(Monster mon)
        {
            var cmd = new MySqlCommand($"update `Monsters` set `IsBoss`={mon.IsBoss}, `Name`={mon.Name},`Level`={mon.Level},`AC`={mon.AC},`AttackBonus`={mon.AttackBonus},`BAB`={mon.BAB},`BaseDamage`={mon.BaseDamage},`CON`={mon.CON},`CritHitMult`={mon.CritHitMult},`DEX`={mon.DEX},`DamageBonus`={mon.DamageBonus},`MaxHP`={mon.MaxHp},`STR`={mon.STR} where `ID` = {mon.Id}");
            ExecuteRequest(cmd);
        }

        public static List<Event> GetAllEvents()
            => (List<Event>)ExecuteSelectRequestObject(new MySqlCommand($"select * from `Events`;", _con), typeof(Event)).Cast<Event>();

        public static List<Expedition> GetAllExpeditionsByIdCharacter(int id)
        {
            throw new NotImplementedException();
        }

        public static List<Log> GetAllLogs()
            => (List<Log>)ExecuteSelectRequestObject(new MySqlCommand($"select * from `Logs`;", _con), typeof(Log)).Cast<Log>();

        public static List<Monster> GetAllMonsters()
            => (List<Monster>)ExecuteSelectRequestObject(new MySqlCommand($"select * from `Monsters`;", _con), typeof(Monster)).Cast<Monster>();

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
            var cmd = new MySqlCommand($"update `Expeditions` set `PlayerID`={ex.PlayerID},`Pet`={ex.Pet},`Time`={ex.Time},`Status`={ex.Status},`PetHP`={ex.PetHP},`Reward`={ex.Reward} where `ID` = {ex.Id}");
            ExecuteRequest(cmd);
        }
    }
}
