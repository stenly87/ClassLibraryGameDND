namespace ClassLibraryGameDND.Models.DbModels
{
    public class Monster
    {
        public int Id { get; set; }

        public bool IsBoss { get; set; }

        public string Name { get; set; }

        public int Level { get; set; }
        
        public int AC { get; set; }

        public int AttackBonus { get; set; }

        public int BAB { get; set; }

        public string BaseDamage { get; set; }

        public int CON { get; set; }

        public int CritHitMult { get; set; }

        public int DEX { get; set; }

        public string DamageBonus { get; set; }

        public int MaxHp { get; set; }

        public int STR { get; set; }
    }
}
