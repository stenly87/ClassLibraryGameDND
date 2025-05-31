using ClassLibraryGameDND.Models.DbModels;
using ClassLibraryGameDND.Models.OtherModels;
using MySqlConnector;
using System.Text;

namespace ClassLibraryGameDND
{
    public class DNDWalkingPet
    {
        private readonly DataBaseContext connection;

        public DNDWalkingPet(MySqlConnection connection)
            => this.connection = new DataBaseContext(connection);

        public string Test()
        {
            return "";
        }

        public string GetStatus(Character character)
        {
            List<Pet> pets = DataBaseContext.GetCharacterPets();
            Pet pet = pets.FirstOrDefault(s => s.Character.ID == character.ID);
            StringBuilder sb = new StringBuilder();
            Expedition expedition = DataBaseContext.GetExpeditionByPetCharacterID(pet.Character.ID);
            int PetCurrentHP = DataBaseContext.GetPetCurrentHPFromCrossByExpeditionID(expedition.Id);
            //List<Event> events = DataBaseContext.GetCompletedEventsFromCrossByExpeditionID(expedition.Id);
            sb.Append($"PetHP: {PetCurrentHP}\n");
            sb.Append("Completed events:\n");
            foreach (Event e in events)
                sb.Append($"{e.EventName}\n"); 
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
