using ClassLibraryGameDND.Models.DbModels;
using System.Text.Json.Serialization;

namespace ClassLibraryGameDND.Models.OtherModels
{
    public class Pet : Monster
    {
        public int Character { get; set; }
        
        public int CHA { get; set; }
        
        public int Fort { get; set; }
        
        public int GoodEvil { get; set; }
        
        public int INT { get; set; }
        
        public int LawCHaos { get; set; }
        
        public int Refl { get; set; }
        
        public int WIS { get; set; }
        
        public int Will { get; set; }
        public string Portrait { get; set; } = "";
    }
}
