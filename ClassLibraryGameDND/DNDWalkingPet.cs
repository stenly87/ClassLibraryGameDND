using ClassLibraryGameDND.Models.DbModels;
using ClassLibraryGameDND.Models.OtherModels;
using MySqlConnector;
using System.Text;

namespace ClassLibraryGameDND
{
    public class DNDWalkingPet
    {
        private DataBaseContext db;

        public void SetUpConnection(MySqlConnection connection)
        {
            this.db = new(connection);
        }

        public Status GetStatus(int characterId)
        {
            Status status = new Status();
            Expedition expedition = db.GetExpeditionByCharacterID(characterId);
            if (expedition != null && !expedition.Status)
            {
                status.Name = expedition.Pet;
                status.Portrait = expedition.Portrait;
                List<CompleteEvent> events = db.GetCompletedEventsFromCrossByExpeditionID(expedition.Id);
                if (events.Count > 0)
                {
                    status.HP = "Здоровье: " + events.Last().CurrentPetHP;
                    status.Reward = expedition.Reward;
                    int minutes = (int)events.Last().Time.Subtract(expedition.Time).TotalMinutes;
                    status.TimePass = "Прошло минут: " + minutes;
                    status.Progress = minutes / 480.0;
                    foreach (CompleteEvent e in events)
                    {
                        if (e.EventName == "battle")
                            e.EventName = "Питомец ввязался в драку и " + ((events.Last().CurrentPetHP > 0) ? "победил" : "проиграл");
                        status.Events.Add("Прошло " + (int)e.Time.Subtract(expedition.Time).TotalMinutes + " минут(ы): " + e.EventName);
                        status.LogId.Add(e.LogId);
                    }
                }
                else
                {
                    status.Info = "Питомец ещё не успел попасть в неприятности";
                }
            }
            else
            {
                status.Info = "Питомцев на прогулке не обнаружено";
            }

            return status;
        }

        public string GetLog(int logId)
        {
            string description = db.GetLogFromCrossByLogId(logId);

            return description;
        }

        public bool CheckExpreditionExist(int charId)
        {
            var find = db.GetExpeditionIdByCharacterID(charId);
            return find != 0;
        }

        public string GetRandomReward()
        {
            var rnd = new Random();
            int result = rnd.Next(1, 10);
            switch (result)
            {
                case 1: return "boots";
                case 2: return "gloves";
                case 3: return "armor";
                case 4: return "helmet";
                case 5: return "ring";
                case 6: return "amulet";
                case 7: return "cloak";
                case 8: return "belt";
                case 9: return "shield";
                default:
                    return "weapon";
            }
        }

        public void AddExpedition(Pet pet)
        {
            DateTime currentDate = db.GetCurrentTime();
            DateTime eventDate = db.GetCurrentTime();

            Random rnd = new();

            var petHp = pet.MaxHP;
            pet.CurrentPetHP = petHp;

            Expedition expedition = new Expedition();
            expedition.PlayerID = pet.Character;
            expedition.Portrait = pet.Portrait;
            expedition.PetHP = petHp;
            expedition.Pet = pet.Name;
            expedition.Time = currentDate;
            expedition.FinishTime = currentDate.AddHours(8);
            //false - в процессе; true - закончена
            expedition.Status = false;
            expedition.Reward = "хз";
            db.AddExpedition(expedition);

            bool end = false;
            while (!end)
            {
                Log log = new Log();
                Event ev = null;

                var periodsOfTime = rnd.Next(30, 61);
                eventDate = eventDate.AddMinutes(periodsOfTime);

                if (pet.CurrentPetHP > 0 && eventDate < currentDate.AddHours(8))
                {
                    // бросаем кубик на проверку бой ли это
                    var isBattle = Dice.Rolling("1d4");
                    if (isBattle == 1)
                    {
                        ev = new Event { Id = 1 };
                        var mon = db.GetRandomMonsterNotBoss() as Monster;
                        mon.CurrentPetHP = mon.MaxHP;
                        log.Description += StartFight(pet, mon, new StringBuilder());

                        if (pet.CurrentPetHP > 0)
                        {
                            int statChange = Dice.Rolling("1d4"); ;
                            pet.STR += statChange;
                            pet.DEX += statChange;
                            log.Description += $"\nПитомец повысил силу и ловкость на {statChange}";
                        }
                        else
                        {
                            expedition.FinishTime = eventDate;
                            end = true;
                            db.EditExpedition(expedition);
                        }
                    }
                    else
                    {
                        ev = db.GetRandomEvent() as Event;
                        var stat = ev.Stat;
                        var rollDice = Dice.Rolling("1d20");
                        bool autofail = rollDice == 1;
                        var changStat = ev.ChangeableStat;
                        var propStatData = pet.GetType().GetProperty(stat);
                        var petStatValue = (int)propStatData.GetValue(pet);
                        log.Description = $"{ev.EventName}: ";
                        int change = 0;
                        if (autofail || (petStatValue < rollDice))
                        {
                            log.Description += ev.NegEffect;
                            change = ev.NegStatChange;
                        }
                        else
                        {
                            log.Description += ev.PosEffect;
                            change = ev.PosStatChange;
                        }
                        var propData = pet.GetType().GetProperty(changStat);
                        var petStat = (int)propData.GetValue(pet);
                        petStat += change;
                        propData.SetValue(pet, petStat);
                    }
                }
                else
                {
                    end = true;
                    expedition.FinishTime = eventDate;
                    if (pet.CurrentPetHP > 0)
                    {
                        ev = new Event { Id = 1 };
                        log.Description = "Сражение с боссом\n\n";
                        var boss = db.GetRandomMonsterBoss() as Monster;
                        boss.CurrentPetHP = boss.MaxHP;
                        log.Description += StartFight(pet, boss, new StringBuilder());
                        if (pet.CurrentPetHP > 0)
                        {
                            //генерится награда
                            expedition.Reward = GetRandomReward();
                        }
                    }
                    db.EditExpedition(expedition);
                }
                if (ev != null)
                {
                    EventExpeditionCross eventExpeditionCross = new EventExpeditionCross();
                    eventExpeditionCross.Event = ev;
                    eventExpeditionCross.Expedition = expedition;
                    eventExpeditionCross.Log = log;
                    eventExpeditionCross.Time = eventDate;
                    eventExpeditionCross.CurrentPetHP = pet.CurrentPetHP;
                    db.AddLog(log);
                    db.AddEventExpeditionCross(eventExpeditionCross);
                }
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

        public string GetReward(int characterId)
        {
            Expedition expedition = db.GetExpeditionByCharacterID(characterId);
            if (expedition == null || expedition.Status)
                return "no";

            string reward = expedition.Reward;
            expedition.Status = true;
            db.EditExpedition(expedition);
            return reward;
        }
    }
}
