using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryGameDND.Models.OtherModels
{
    public static class PetParser
    {
        public static Pet PetParse(string pet)
        {
            Pet localPet = new Pet();
            var petParts = pet.Split(',');
            foreach (var part in petParts)
            {
                var stats = part.Split(":");
                switch (stats[0])
                {
                    case "Character":
                        localPet.Character.ID = int.Parse(stats[1]);
                        break;
                    case "AC":
                        localPet.AC = int.Parse(stats[1]);
                        break;
                    case "BAB":
                        localPet.BAB = int.Parse(stats[1]);
                        break;
                    case "BaseDamage":
                        localPet.BaseDamage = Dice.Rolling(stats[1]);
                        break;
                    case "CHA":
                        localPet.CHA = int.Parse(stats[1]);
                        break;
                    case "CON":
                        localPet.CON = int.Parse(stats[1]);
                        break;
                    case "CritHitMult":
                        localPet.CritHitMult = int.Parse(stats[1]);
                        break;
                    case "DEX":
                        localPet.DEX = int.Parse(stats[1]);
                        break;
                    case "DamageBonus":
                        localPet.DamageBonus = Dice.Rolling(stats[1]);
                        break;
                    case "Fort":
                        localPet.Fort = int.Parse(stats[1]);
                        break;
                    case "GoodEvil":
                        localPet.GoodEvil = int.Parse(stats[1]);
                        break;
                    case "INT":
                        localPet.INT = int.Parse(stats[1]);
                        break;
                    case "LawChaos":
                        localPet.LawCHaos = int.Parse(stats[1]);
                        break;
                    case "MaxHP":
                        localPet.MaxHP = int.Parse(stats[1]);
                        break;
                    case "Name":
                        localPet.Name = stats[1];
                        break;
                    case "Refl":
                        localPet.Refl = int.Parse(stats[1]);
                        break;
                    case "STR":
                        localPet.STR = int.Parse(stats[1]);
                        break;
                    case "WIS":
                        localPet.WIS = int.Parse(stats[1]);
                        break;
                    case "Will":
                        localPet.Will = int.Parse(stats[1]);
                        break;
                }
            }
            return localPet;
        }
    }

    
}
