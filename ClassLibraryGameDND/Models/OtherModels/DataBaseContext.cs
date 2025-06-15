using MySqlConnector;
using ClassLibraryGameDND.Models.DbModels;
using System.Security.Cryptography;
using System.Data;

namespace ClassLibraryGameDND.Models.OtherModels
{
    public class DataBaseContext
    {
        private  MySqlConnection _con;
        public DataBaseContext(MySqlConnection connection)
            => _con = connection;

        private  void ExecuteRequest(string request, params MySqlParameter[]? parameters)
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

        private  List<object> ExecuteSelectRequestObject(MySqlCommand cmd, Type type)
        {
            List<object> result = new();
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
                            Reward = dr.GetString("Reward"),
                            Portrait = dr.GetString("Portrait")
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

                    throw new Exception("пизда");

            }
            cmd.Dispose();
            return result;
        }

        private object ExecuteAndReturnValue(MySqlCommand cmd)
        {
            object result = cmd.ExecuteScalar();
            cmd.Dispose();
            return result;
        }

        public void AddEvent(Event ev)
        {
            var request = "insert into `Events` Values (0, @EventName, @Stat);";

            ExecuteRequest(request, new MySqlParameter("EventName", ev.EventName), new MySqlParameter("Stat", ev.Stat));
            ev.Id = (int)(ulong)ExecuteAndReturnValue(new MySqlCommand("select LAST_INSERT_ID()", _con));
        }

        public void AddExpedition(Expedition ex)
        {
            var request = "insert into `Expeditions` Values (@PlayerID, @Pet, @Time, @Status, 0, @PetHP, @Reward, @FinishTime, @Portrait);";

            ExecuteRequest(request,
                new MySqlParameter("PlayerID", ex.PlayerID),
                new MySqlParameter("Pet", ex.Pet),
                new MySqlParameter("Time", ex.Time),
                new MySqlParameter("Status", ex.Status),
                new MySqlParameter("PetHP", ex.PetHP),
                new MySqlParameter("Reward", ex.Reward),
                new MySqlParameter("FinishTime", ex.FinishTime),
                new MySqlParameter("Portrait", ex.Portrait)
            );

            ex.Id = (int)(ulong)ExecuteAndReturnValue(new MySqlCommand("select LAST_INSERT_ID()", _con));
        }

        public void AddLog(Log log)
        {
            var request = "insert into `Logs` Values (0, @Description);";

            ExecuteRequest(request, new MySqlParameter("Description", log.Description));
            log.Id = (int)(ulong)ExecuteAndReturnValue(new MySqlCommand("select LAST_INSERT_ID()", _con));
        }

        public void AddMonster(Monster mon)
        {
            var request = "insert into `Monsters` Values (0, @IsBoss, @Name, @Level, @AC, @AttackBonus, @BAB, @BaseDamage, @CON, @CritHitMult, @DEX, @DamageBonus, @MaxHP, @STR);";

            ExecuteRequest(request,
                new MySqlParameter("IsBoss", mon.IsBoss),
                new MySqlParameter("Name", mon.Name),
                new MySqlParameter("Level", mon.Level),
                new MySqlParameter("AC", mon.AC),
                new MySqlParameter("AttackBonus", mon.AttackBonus),
                new MySqlParameter("BAB", mon.BAB),
                new MySqlParameter("BaseDamage", mon.BaseDamage),
                new MySqlParameter("CON", mon.CON),
                new MySqlParameter("CritHitMult", mon.CritHitMult),
                new MySqlParameter("DEX", mon.DEX),
                new MySqlParameter("DamageBonus", mon.DamageBonus),
                new MySqlParameter("MaxHP", mon.MaxHP),
                new MySqlParameter("STR", mon.STR)
                );
            mon.Id = (int)(ulong)ExecuteAndReturnValue(new MySqlCommand("select LAST_INSERT_ID()", _con));
        }

        public void EditExpedition(Expedition ex)
        {
            ExecuteRequest($"update `Expeditions` set `Time`=@time,`Status`={(ex.Status ? 1 : 0)},`PetHP`={ex.PetHP},`FinishTime`=@finish, `Reward`='{ex.Reward}' where `ID` = {ex.Id}", new MySqlParameter("time", ex.Time), new MySqlParameter("finish", ex.FinishTime));
        }

        public List<CompleteEvent> GetCompletedEventsFromCrossByExpeditionID(int id)
            => ExecuteSelectRequestForCompleteEvent(new MySqlCommand($"SELECT e.ID, e.EventName, eec.Time, eec.CurrentPetHP, eec.LogID FROM  EventExpeditionCross eec JOIN Events e ON eec.EventID = e.ID  WHERE eec.ExpeditionID = {id} and eec.Time < CURRENT_TIMESTAMP()  ORDER BY eec.Time;", _con));

        private List<CompleteEvent> ExecuteSelectRequestForCompleteEvent(MySqlCommand mySqlCommand)
        {
            List<CompleteEvent> result = new();
            if (_con is null || mySqlCommand is null)
                return result;

            using var dr = mySqlCommand.ExecuteReader();

            while (dr.Read())
            {
                var item = new CompleteEvent
                {
                    ID = dr.GetInt32("ID"),
                    CurrentPetHP = dr.GetInt32("CurrentPetHP"),
                    EventName = dr.GetString("EventName"),
                    LogId = dr.GetInt32("LogID"),
                    Time = dr.GetDateTime("Time")
                };
                result.Add(item);
            }
            mySqlCommand.Dispose();
            return result;
        }

        public  Expedition GetExpeditionByCharacterID(int id)
            => ExecuteSelectRequestObject(new MySqlCommand($"select * from `Expeditions` where `PlayerID` = {id} Order By `ID` DESC LIMIT 1;", _con), typeof(Expedition)).Cast<Expedition>().Last();

        public  int GetExpeditionIdByCharacterID(int id)
        {
            var find = ExecuteAndReturnValue(new MySqlCommand($"select `ID` from `Expeditions` where `PlayerID` = {id} and `FinishTime` > CURRENT_TIMESTAMP();", _con));
            if (find != null)
                return -1;
            return 0;
        }

        public  void AddEventExpeditionCross(EventExpeditionCross ex)
        {
            var request = "insert into `EventExpeditionCross` Values (@EventID, @Time, @ExpeditionID, @LogID, @CurrentPetHP);";

            ExecuteRequest(request,
                new MySqlParameter("EventID", ex.Event.Id),
                new MySqlParameter("ExpeditionID", ex.Expedition.Id),
                new MySqlParameter("LogID", ex.Log.Id),
                new MySqlParameter("Time", ex.Time),
                new MySqlParameter("CurrentPetHP", ex.CurrentPetHP)
                );
        }

        public  string GetLogFromCrossByLogId(int id)
        {
            return (string?)ExecuteAndReturnValue(new MySqlCommand($"SELECT  l.Description from EventExpeditionCross eec JOIN  Logs l ON eec.LogID = l.ID WHERE eec.LogID = {id}", _con)) ?? "";
        }

        public  object GetRandomMonsterNotBoss()

            => ExecuteSelectRequestObject(new MySqlCommand($"select * from `Monsters` where `IsBoss` = 0 order by rand() limit 1;", _con), typeof(Monster)).Select(s => (Monster)s).ToList().First();

        public  Monster GetRandomMonsterBoss()
             => ExecuteSelectRequestObject(new MySqlCommand($"select * from `Monsters` where `IsBoss` = 1 order by rand() limit 1;", _con), typeof(Monster)).Select(s => (Monster)s).ToList().First();

        internal  Event GetRandomEvent()
             => ExecuteSelectRequestObject(new MySqlCommand($"select * from `Events` where `ID` > 2 order by rand() limit 1;", _con), typeof(Event)).Select(s => (Event)s).ToList().First();

        internal  DateTime GetCurrentTime()
        {
            return (DateTime)ExecuteAndReturnValue(new MySqlCommand("SELECT CURRENT_TIMESTAMP();", _con));
        }
    }
}
