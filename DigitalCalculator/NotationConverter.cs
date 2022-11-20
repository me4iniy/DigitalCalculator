using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalCalculator
{
    public static class NotationConverter
    {
        private static char[] Alphabet = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
                            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
                            'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
                            'U', 'V', 'W', 'X', 'Y', 'Z',
                            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
                            'k', 'l', 'm', 'n'};

        public static int ConvertSymbolToDigit(char symbol)
        {
            for (int i = 0; i < Alphabet.Length; i++) 
                if (symbol == Alphabet[i])
                    return i;

            return -1;
        }

        public static char ConvertDigitToSymbol(int digit)
        {
            if (digit < 0 || digit > Alphabet.Length - 1)
                return char.MinValue;

            return Alphabet[digit];
        }
        public static char[] GetAlphabetForNotation(int notation)
        {
            List<char> localAlphabet = new List<char>();

            for (int i = 0; i < notation; i++) 
                localAlphabet.Add(Alphabet[i]);

            return localAlphabet.ToArray();
        }
        public static int GetPointPosition(string strNumber)
        {
            for(int i = 0; i < strNumber.Length; i++)
                if (strNumber[i] == '.')
                    return i-1;

            return strNumber.Length-1;//Потому что если в числе нет точки, она всегда на конце, просто без дробной части
        }
        public static string GetReversOfStrNumber(string srtNumber)
        {
            StringBuilder stringBuilder = new();

            for (int i = srtNumber.Length-1; i >= 0; i--)
                stringBuilder.Append(srtNumber[i]);

            return stringBuilder.ToString();
        }
    }
}
