using ClassLibraryGameDND.Models.DbModels;
using ClassLibraryGameDND.Models.OtherModels;
using MySqlConnector;
using System.Text;

namespace ClassLibraryGameDND
{
    public class DNDWalkingPet(MySqlConnection connection)
    {
        private readonly DataBaseContext connection = new(connection);

        public string GetStatus(Character character)
        {
            StringBuilder sb = new StringBuilder();
            Expedition expedition = DataBaseContext.GetExpeditionByCharacterID(character.ID);
            int PetCurrentHP = DataBaseContext.GetPetCurrentHPFromCrossByExpeditionID(expedition.Id);
            List<Event> events = DataBaseContext.GetCompletedEventsFromCrossByExpeditionID(expedition.Id);
            sb.Append($"Здоровье питомца: {PetCurrentHP}\n");
            sb.Append("События:\n");
            foreach (Event e in events)
                sb.Append($"{e.EventName}\n");
            return sb.ToString();
        }

        public void AddExpedition(Character character, string Pet)
        {
            DateTime currentDate = DateTime.Now;
            DateTime eventDate = DateTime.Now;

            Random rnd = new();

            Pet pet = PetParser.PetParse(Pet);
            var petHp = pet.MaxHP;

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
                
                if (petHp > 0 && eventDate < currentDate.AddHours(8))
                {
                    var periodsOfTime = rnd.Next(30, 61);
                    eventDate.AddMinutes(periodsOfTime);
                    // бросаем кубик на проверку бой ли это
                    var isBattle = Dice.Rolling("1d2");
                    if (isBattle == 1)
                    {
                        var mon = monsters.FirstOrDefault(s => s.Id == rnd.Next(monsters.Count));
                        bool isBattleOver = false;
                        pet.MaxHP = petHp;
                        while (!isBattleOver)
                        {
                            if (petHp > 0 && mon.MaxHp > 0)
                            {
                                log.Description += StartFight(pet, mon);
                            }
                            else
                            {
                                
                                eventExpeditionCross.Event = events.FirstOrDefault(s => s.EventName == "Бой");
                                eventExpeditionCross.Expedition = expedition;
                                eventExpeditionCross.Log = log;
                                eventExpeditionCross.Time = eventDate;
                                eventExpeditionCross.CurrentPetHP = petHp;
                                DataBaseContext.AddLog(log);
                                DataBaseContext.AddEventExpeditionCross(eventExpeditionCross);
                                isBattleOver = true;
                                if (petHp <= 0)
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
                        eventExpeditionCross.CurrentPetHP = petHp;
                        DataBaseContext.AddLog(log);
                        DataBaseContext.AddEventExpeditionCross(eventExpeditionCross);
                    }
                }
                else
                {
                    expedition.Status = true;
                    if (petHp > 0)
                    {  
                        var bosses = monsters.Where(s => s.IsBoss == true).ToList();
                        var boss = bosses.FirstOrDefault(s => s.Id == rnd.Next(monsters.Count));
                       
                        bool isBossFightOver = false;
                        while (!isBossFightOver)
                        {
                            if (petHp > 0 && boss.MaxHp > 0)
                            {
                                log.Description += StartFight(pet, boss);
                                
                            }
                            else
                            {
                                eventExpeditionCross.Event = events.FirstOrDefault(s => s.EventName == "Бой");
                                eventExpeditionCross.Expedition = expedition;
                                eventExpeditionCross.Log = log;
                                eventExpeditionCross.Time = eventDate;
                                eventExpeditionCross.CurrentPetHP = petHp;
                                DataBaseContext.AddLog(log);
                                DataBaseContext.AddEventExpeditionCross(eventExpeditionCross);
                                isBossFightOver = true;
                                if (petHp > 0)
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

        public string StartFight(Pet pet, Monster monster)
        {
            var statBonus = pet.DEX > pet.STR ? (pet.DEX - 10) / 2 : (pet.STR - 10) / 2;
            var result = Dice.Rolling("1d20");
            bool autofail = result == 1;
            result = pet.BAB + pet.DamageBonus + statBonus + result;

            if (!autofail && (result == 20 || result >= monster.AC))
            {
                int dmg = pet.BaseDamage + pet.DamageBonus;
                monster.MaxHp -= dmg;
                return $"Попадание.\n HP цели:{monster.MaxHp}";
            }
            else
            {
                Expedition expedition = DataBaseContext.GetExpeditionByPetCharacterID(pet.Character.ID);
                DataBaseContext.PetCurrentHPForExpeditionCrossByExpeditionID(expedition.Id);
                return "Промах";
            }
        }
    }
}
