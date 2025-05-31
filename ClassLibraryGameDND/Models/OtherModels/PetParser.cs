using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace ClassLibraryGameDND.Models.OtherModels
{
    public static class PetParser
    {
        //{"Character":666, "AC":27,"BAB":10,"BaseDamage":"1d8","CHA":10,"CON":18,"CritHitMult":2,"DEX":20,"DamageBonus":"1d6","Fort":14,"GoodEvil":5,"INT":9,"LawChaos":1,"MaxHP":160,"Name":"\u0417\u0438\u043c\u043d\u0438\u0439 \u0432\u043e\u043b\u043a","Refl":12,"STR":24,"WIS":14,"Will":12}
        public static Pet PetParse(string pet)
        {
            var jObj = JsonObject.Parse(pet);
            Pet localPet = new Pet();
            try
            {
                localPet.Character = new Character { ID = (int)jObj["Character"] };
                localPet.AC = (int)jObj["AC"];
                localPet.BAB = (int)jObj["BAB"];

                localPet.BaseDamage = Dice.Rolling((string)jObj["BaseDamage"]);
                localPet.CHA = (int)jObj["CHA"];
                localPet.CON = (int)jObj["CON"];
                localPet.CritHitMult = (int)jObj["CritHitMult"];
                localPet.DEX = (int)jObj["DEX"];
                localPet.DamageBonus = Dice.Rolling((string)jObj["DamageBonus"]);
                localPet.Fort = (int)jObj["Fort"];
                localPet.GoodEvil = (int)jObj["GoodEvil"];
                localPet.INT = (int)jObj["INT"];
                localPet.LawCHaos = (int)jObj["LawChaos"];
                localPet.MaxHP = (int)jObj["MaxHP"];
                localPet.Name = (string)jObj["Name"];
                localPet.Refl = (int)jObj["Refl"];
                localPet.STR = (int)jObj["STR"];
                localPet.WIS = (int)jObj["WIS"];
                localPet.Will = (int)jObj["Will"];
            }
            catch (Exception ex)
            {
                ;
            }

            return localPet;

            /*var petParts = pet.Split(',');
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
            }*/
        }
    }


}
