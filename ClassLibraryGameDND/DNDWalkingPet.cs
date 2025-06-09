using ClassLibraryGameDND.Models.DbModels;
using ClassLibraryGameDND.Models.OtherModels;
using MySqlConnector;
using System.Text;

namespace ClassLibraryGameDND
{
    public class DNDWalkingPet(MySqlConnection connection)
    {
        private readonly DataBaseContext connection = new(connection);

        public Status GetStatus(int characterId)
        {
            Status status = new Status();
            StringBuilder sb = new StringBuilder();
            Expedition expedition = DataBaseContext.GetExpeditionByCharacterID(characterId);
            if (expedition != null)
            {
                List<CompleteEvent> events = DataBaseContext.GetCompletedEventsFromCrossByExpeditionID(expedition.Id);
                if (events.Count > 0)
                {
                    sb.Append($"Здоровье питомца: {events.Last().CurrentPetHP}\n");
                    sb.Append("События:\n");
                    foreach (CompleteEvent e in events)
                    {
                        status.Events.Add(e.EventName);
                        status.LogId.Add(e.LogId);
                    }
                }
                else
                {
                    sb.Append("Питомец ещё не успел попасть в неприятности");
                }
            }
            else
            {
                sb.Append("Вы не участвуете в экспедиции :(");
            }

            status.Info = sb.ToString();
            return status;
        }

        public string GetLog(int logId)
        {
            string description = DataBaseContext.GetLogFromCrossByLogId(logId);

            return description;
        }

        public bool AddExpedition(Pet pet)
        {
            var find = DataBaseContext.GetExpeditionIdByCharacterID(pet.Character);
            if (find == 0)
            {

                DateTime currentDate = DataBaseContext.GetCurrentTime();
                DateTime eventDate = DataBaseContext.GetCurrentTime();

                Random rnd = new();

                var petHp = pet.MaxHP;
                pet.CurrentPetHP = petHp;

                Expedition expedition = new Expedition();
                expedition.PlayerID = pet.Character;
                expedition.PetHP = petHp;
                expedition.Pet = pet.ToString();
                expedition.Time = currentDate;
                expedition.FinishTime = currentDate.AddHours(8);
                //false - в процессе; true - закончена
                expedition.Status = false;
                expedition.Reward = "хз";
                DataBaseContext.AddExpedition(expedition);
                List<Event> events = DataBaseContext.GetAllEvents();

                while (!expedition.Status)
                {
                    EventExpeditionCross eventExpeditionCross = new EventExpeditionCross();
                    Log log = new Log();

                    if (pet.CurrentPetHP > 0 && eventDate < currentDate.AddHours(8))
                    {
                        var periodsOfTime = rnd.Next(30, 61);
                        eventDate = eventDate.AddMinutes(periodsOfTime);
                        // бросаем кубик на проверку бой ли это
                        var isBattle = Dice.Rolling("1d8");
                        if (isBattle == 1)
                        {
                            var mon = DataBaseContext.GetRandomMonsterNotBoss() as Monster;
                            mon.CurrentPetHP = mon.MaxHP; log.Description += StartFight(pet, mon, new StringBuilder());
                            eventExpeditionCross.Event = new Event { Id = 1 };
                            eventExpeditionCross.Expedition = expedition;
                            eventExpeditionCross.Log = log;
                            eventExpeditionCross.Time = eventDate;
                            eventExpeditionCross.CurrentPetHP = pet.CurrentPetHP;
                            DataBaseContext.AddLog(log);
                            DataBaseContext.AddEventExpeditionCross(eventExpeditionCross);
                            if (pet.CurrentPetHP <= 0)
                            {
                                expedition.FinishTime = eventDate;
                                expedition.Status = true;
                                DataBaseContext.EditExpedition(expedition);
                            }
                        }
                        else
                        {
                            var ev = DataBaseContext.GetRandomEvent() as Event;
                            var stat = ev.Stat;
                            var rollDice = Dice.Rolling("1d20");
                            bool autofail = rollDice == 1;
                            var changStat = ev.ChangeableStat;
                            var propStatData = pet.GetType().GetProperty(stat);
                            var petStatValue = (int)propStatData.GetValue(pet);
                            if (autofail || (petStatValue < rollDice))
                            {
                                log.Description = ev.NegEffect;
                                var propData = pet.GetType().GetProperty(changStat);
                                var petStat = (int)propData.GetValue(pet);
                                petStat += ev.NegStatChange;
                                propData.SetValue(pet, petStat);
                            }
                            else
                            {
                                log.Description = ev.PosEffect;
                                var propData = pet.GetType().GetProperty(changStat);
                                var petStat = (int)propData.GetValue(pet);
                                petStat += ev.PosStatChange;
                                propData.SetValue(pet, petStat);

                            }
                            eventExpeditionCross.Event = ev;
                            eventExpeditionCross.Expedition = expedition;
                            eventExpeditionCross.Log = log;
                            eventExpeditionCross.Time = eventDate;
                            eventExpeditionCross.CurrentPetHP = pet.CurrentPetHP;
                            DataBaseContext.AddLog(log);
                            DataBaseContext.AddEventExpeditionCross(eventExpeditionCross);
                        }
                    }
                    else
                    {
                        expedition.Status = true;
                        expedition.FinishTime = eventDate;
                        if (pet.CurrentPetHP > 0)
                        {
                            var boss = DataBaseContext.GetRandomMonsterBoss() as Monster;
                            boss.CurrentPetHP = boss.MaxHP;
                            log.Description += StartFight(pet, boss, new StringBuilder());
                            eventExpeditionCross.Event = new Event { Id = 1 };
                            eventExpeditionCross.Expedition = expedition;
                            eventExpeditionCross.Log = log;
                            eventExpeditionCross.Time = eventDate;
                            eventExpeditionCross.CurrentPetHP = pet.CurrentPetHP;
                            DataBaseContext.AddLog(log);
                            DataBaseContext.AddEventExpeditionCross(eventExpeditionCross);

                            if (pet.CurrentPetHP > 0)
                            {
                                //генерится награда
                                expedition.Reward = "";
                            }
                        }
                        DataBaseContext.EditExpedition(expedition);
                    }
                }
                return true;
            }
            else
            {

                Console.WriteLine("Экпедиция уже существует");

                return false;
            }
        }


        public string StartFight(Monster pet, Monster monster, StringBuilder sbForStartFight)
        {
            if (pet.CurrentPetHP < 1)
            {
                sbForStartFight.Append($"\n{monster.Name} победил");
                return sbForStartFight.ToString();
            }
            if (monster.CurrentPetHP < 1)
            {
                sbForStartFight.Append($"\n{pet.Name} победил");
                return sbForStartFight.ToString();
            }

            var statBonus = pet.DEX > pet.STR ? (pet.DEX - 10) / 2 : (pet.STR - 10) / 2;
            var statBonusMonster = monster.DEX > monster.STR ? (monster.DEX - 10) / 2 : (monster.STR - 10) / 2;
            var d20 = Dice.Rolling("1d20");
            bool autofail = d20 == 1;
            var result = pet.BAB + pet.AttackBonus + statBonus + d20;
            bool attack = !autofail && (d20 == 20 || result >= monster.AC);
            var attackResult = attack ? "Попадание" : "Промах";
            sbForStartFight.Append($"{pet.Name} атакует {monster.Name}: {pet.BAB + pet.AttackBonus + statBonus} + {d20} против {monster.AC} : {attackResult}\n");
            if (attack)
            {
                var statdmgBonus = (pet.STR - 10) / 2;
                int diceBase = Dice.Rolling(pet.BaseDamage);
                int diceBonus = Dice.Rolling(pet.DamageBonus);
                int dmg = statdmgBonus + diceBase + diceBonus;
                monster.CurrentPetHP -= dmg;
                sbForStartFight.Append($"{pet.Name} нанёс урон {dmg}.\n HP {monster.Name}:{monster.CurrentPetHP}\n");
            }

            StartFight(monster, pet, sbForStartFight);


            return sbForStartFight.ToString();
        }
    }
}
