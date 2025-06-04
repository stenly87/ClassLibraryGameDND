namespace ClassLibraryGameDND.Models.DbModels
{
    public class EventExpeditionCross
    {
        public Event Event { get; set; }

        public Expedition Expedition { get; set; }

        public Log Log { get; set; }

        public DateTime Time { get; set; }

        public int CurrentPetHP { get; set; }
    }
}
