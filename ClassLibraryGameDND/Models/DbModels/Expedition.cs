namespace ClassLibraryGameDND.Models.DbModels
{
    public class Expedition
    {
        public int Id { get; set; }

        public int PlayerID { get; set; }

        public string Pet { get; set; }

        public DateTime Time { get; set; }

        public bool Status { get; set; }

        public int PetHP { get; set; }

        public string Reward { get; set; }
    }
}
