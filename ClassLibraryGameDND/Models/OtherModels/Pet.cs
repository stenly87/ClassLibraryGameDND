using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryGameDND.Models.OtherModels
{
    public class Pet
    {
        public Character Character { get; set; }
        public int AC { get; set; }
        public int BAB { get; set; }
        public int BaseDamage { get; set; }
        public int CHA { get; set; }
        public int CON { get; set; }
        public int CritHitMult { get; set; }
        public int DEX { get; set; }
        public int DamageBonus { get; set; }
        public int Fort { get; set; }
        public int GoodEvil { get; set; }
        public int INT { get; set; }
        public int LawCHaos { get; set; }
        public int MaxHP { get; set; }
        public string Name { get; set; }
        public int Refl { get; set; }
        public int STR { get; set; }
        public int WIS { get; set; }
        public int Will { get; set; }

        public override string ToString()
        {
            return "Character:" + Character.ID + "AC:" + AC + "BAB:" + BAB + "BaseDamage:" + BaseDamage + "CHA:" + CHA + "CON:" + CON + "CritHitMult:" + CritHitMult + "DEX:" + DEX + "DamageBonus:" + DamageBonus + "Fort:" + Fort + "GoodEvil:" + GoodEvil + "INT:" + INT + "LawChaos:" + LawCHaos + "MaxHP:" + MaxHP + "Name:" + Name + "Refl:" + Refl + "STR:" + STR + "WIS:" + WIS + "Will:" + Will;
        }
    }

}
