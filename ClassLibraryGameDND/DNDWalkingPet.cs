using ClassLibraryGameDND.Models.DbModels;
using ClassLibraryGameDND.Models.OtherModels;
using MySqlConnector;
using System.Text;

namespace ClassLibraryGameDND
{
    public class DNDWalkingPet
    {
        private readonly MySqlConnection connection;

        public DNDWalkingPet(MySqlConnection connection)
        {
            this.connection = connection;
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
    }
}
