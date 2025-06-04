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

        public override string ToString()
            => "Character:" + Character + "AC:" + AC + "BAB:" + BAB + "BaseDamage:" + BaseDamage + "CHA:" + CHA + "CON:" + CON + "CritHitMult:" + CritHitMult + "DEX:" + DEX + "DamageBonus:" + DamageBonus + "Fort:" + Fort + "GoodEvil:" + GoodEvil + "INT:" + INT + "LawChaos:" + LawCHaos + "MaxHP:" + MaxHP + "Name:" + Name + "Refl:" + Refl + "STR:" + STR + "WIS:" + WIS + "Will:" + Will;
    }
}
