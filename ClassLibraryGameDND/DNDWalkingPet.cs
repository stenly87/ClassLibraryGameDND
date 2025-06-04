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
                DateTime currentDate = DateTime.Now;
                List<CompleteEvent> events = DataBaseContext.GetCompletedEventsFromCrossByExpeditionID(expedition.Id);
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

        public void AddExpedition(Character character, string Pet)
        {
            DateTime currentDate = DateTime.Now;
            DateTime eventDate = DateTime.Now;

            Random rnd = new();

            Pet pet = PetParser.PetParse(Pet);
            var petHp = pet.MaxHP;
            pet.CurrentPetHP=petHp;

            Expedition expedition = new Expedition();
            expedition.PlayerID = character.ID;
            expedition.PetHP = petHp;
            expedition.Pet = Pet;
            expedition.Time = currentDate;
            //false - в процессе; true - закончена
            expedition.Status = false;
            expedition.Reward = "хз";
            DataBaseContext.AddExpedition(expedition);
            List<Monster> monsters = DataBaseContext.GetAllMonsters();
            List<Event> events = DataBaseContext.GetAllEvents();

            while (!expedition.Status)
            {
                EventExpeditionCross eventExpeditionCross = new EventExpeditionCross();
                Log log = new Log();

                if (pet.CurrentPetHP > 0 && eventDate < currentDate.AddHours(8))
                {
                    var periodsOfTime = rnd.Next(30, 61);
                    eventDate.AddMinutes(periodsOfTime);
                    // бросаем кубик на проверку бой ли это
                    var isBattle = Dice.Rolling("1d2");
                    if (isBattle == 1)
                    {
                        var mon = monsters.FirstOrDefault(s => s.Id == rnd.Next(monsters.Count));
                        bool isBattleOver = false;
                        while (!isBattleOver)
                        {
                            if (pet.CurrentPetHP > 0 && mon.MaxHp > 0)
                            {
                                log.Description += StartFight(pet, mon);
                            }
                            else
                            {  
                                eventExpeditionCross.Event = events.FirstOrDefault(s => s.EventName == "Бой");
                                eventExpeditionCross.Expedition = expedition;
                                eventExpeditionCross.Log = log;
                                eventExpeditionCross.Time = eventDate;
                                eventExpeditionCross.CurrentPetHP = pet.CurrentPetHP;
                                DataBaseContext.AddLog(log);
                                DataBaseContext.AddEventExpeditionCross(eventExpeditionCross);
                                isBattleOver = true;
                                if (pet.CurrentPetHP <= 0)
                                    expedition.Status = true;
                                DataBaseContext.EditExpedition(expedition);
                            }
                        }

                    }
                    else
                    {
                        Event ev = events.FirstOrDefault(s => s.Id == rnd.Next(events.Count));
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
                    if (pet.CurrentPetHP > 0)
                    {
                        var bosses = monsters.Where(s => s.IsBoss == true).ToList();
                        var boss = bosses.FirstOrDefault(s => s.Id == rnd.Next(monsters.Count));

                        bool isBossFightOver = false;
                        while (!isBossFightOver)
                        {
                            if (pet.CurrentPetHP > 0 && boss.MaxHp > 0)
                            {
                                log.Description += StartFight(pet, boss);

                            }
                            else
                            {
                                eventExpeditionCross.Event = events.FirstOrDefault(s => s.EventName == "Бой");
                                eventExpeditionCross.Expedition = expedition;
                                eventExpeditionCross.Log = log;
                                eventExpeditionCross.Time = eventDate;
                                eventExpeditionCross.CurrentPetHP = pet.CurrentPetHP;
                                DataBaseContext.AddLog(log);
                                DataBaseContext.AddEventExpeditionCross(eventExpeditionCross);
                                isBossFightOver = true;
                                if (pet.CurrentPetHP > 0)
                                {
                                    //генерится награда
                                    expedition.Reward = "";
                                }

                            }
                        }
                        DataBaseContext.EditExpedition(expedition);
                    }
                }
            }
        }

        StringBuilder sbForStartFight = new StringBuilder();
        public string StartFight(Pet pet, Monster monster)
        {
            var statBonus = pet.DEX > pet.STR ? (pet.DEX - 10) / 2 : (pet.STR - 10) / 2;
            var statBonusMonster = monster.DEX > monster.STR ? (monster.DEX - 10) / 2 : (monster.STR - 10) / 2;
            var result = Dice.Rolling("1d20");
            bool autofail = result == 1;
            result = pet.BAB + pet.DamageBonus + statBonus + result;
            if (pet.CurrentPetHP < 1)
            {
                sbForStartFight.Append("\nПитомец проиграл");
                return sbForStartFight.ToString();
            }
            if (monster.MaxHp < 1)
            {
                sbForStartFight.Append("\nПитомец победил");
                return sbForStartFight.ToString();
            }

            if (!autofail && (result == 20 || result >= monster.AC))
            {
                int dmg = pet.BaseDamage + pet.DamageBonus;
                monster.MaxHp -= dmg;
                sbForStartFight.Append($"Питомец нанёс урон.\n HP цели:{monster.MaxHp}\n");
            }
            else
            {
                var resultMonster = Dice.Rolling("1d20");
                bool autofailMonster = resultMonster == 1;
                resultMonster = monster.BAB + Dice.Rolling(monster.DamageBonus) + statBonusMonster + resultMonster;
                if (!autofailMonster && (resultMonster == 20 || resultMonster > pet.AC))
                {
                    int dmgMonster = Dice.Rolling(monster.BaseDamage) + Dice.Rolling(monster.DamageBonus);
                    pet.CurrentPetHP -= dmgMonster;
                    sbForStartFight.Append($"Питомец получил урон.\n HP пета:{pet.CurrentPetHP}\n");
                }
                else
                    StartFight(pet, monster);
            }
            return sbForStartFight.ToString();
        }
    }
}
