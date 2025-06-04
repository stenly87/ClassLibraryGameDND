namespace ClassLibraryGameDND.Models.DbModels
{
    public class Event
    {
        public int Id { get; set; }

        public string EventName { get; set; }
        
        public string Stat {  get; set; }

        public string NegEffect {  get; set; }

        public string PosEffect {  get; set; }

        public string ChangeableStat {  get; set; }

        public int NegStatChange { get; set; }

        public int PosStatChange { get; set; }
    }
}
