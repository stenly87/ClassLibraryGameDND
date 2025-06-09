using System.Text.RegularExpressions;

namespace ClassLibraryGameDND.Models.OtherModels
{
    public static class Dice
    {
        public static int Rolling(string dice)
        {
            if (string.IsNullOrEmpty(dice))
                return 0;
            if (InvalidnessCheck(dice))
                throw new ArgumentException("Ошибка в валидности переданого кубика!");

            var diceValues = dice.Split(new char[] { 'd', '+' }, StringSplitOptions.RemoveEmptyEntries);
            var times = int.Parse(diceValues[0]);
            var diceType = int.Parse(diceValues[1]);
            int add = 0;
            if (diceValues.Length > 2)
                add = int.Parse(diceValues[2]);

            var rnd = new Random();
            var result = 0;

            for (int i = 0; i < times; i++)
                result += rnd.Next(1, diceType+1);
            result += add;
            return result;
        }

        private static bool InvalidnessCheck(string dice)
        {
            var diceValues = dice.Split(new char[] { 'd', '+' }, StringSplitOptions.RemoveEmptyEntries);
            if (!int.TryParse(diceValues[0], out _))
                return true;
            if ( diceValues.Length > 1 && !int.TryParse(diceValues[1], out _))
                return true;
            if (diceValues.Length > 2 && !int.TryParse(diceValues[2], out _))
                return true;
            return false;
        }
    }
}
