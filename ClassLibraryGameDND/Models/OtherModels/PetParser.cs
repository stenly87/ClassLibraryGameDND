using System.Text.Json.Nodes;

namespace ClassLibraryGameDND.Models.OtherModels
{
    public static class PetParser
    {
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
                Console.WriteLine(ex.Message); ;
            }
            return localPet;
        }
    }
}
