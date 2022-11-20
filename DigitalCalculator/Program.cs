using DigitalCalculator;
using System;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;

namespace DigitalCalculator

{
    public class Program
    {
        public static void Main()
        {
            /*UserInterface.ShowTextFromArray(new string[] { "Это консольное приложение создано для перевода систем счисления от 1 до 50, " +
                                                            "а также служит калькулятором для перевода чисел в римскую СС.",
                                                            "Выполнил Бабиков К. А., Группа При-101" });

            Thread.Sleep(4800);*/

            bool exit = false;
            while (!exit)
            {
                Console.Clear();

                int userChoice = UserInterface.GetSelectOfOne(new string[] { "Перевод числа из любой СС в любую другую СС.",
                                                            "Перевод числа в римскую СС.",
                                                            "Перевод из римской СС",
                                                            "Суммирование в любой СС." },
                                                            "Выбор осуществляется стрелочками или кнопками W/S");

                switch (userChoice)
                {
                    case -1:
                        exit = true;
                        break;
                    case 0:
                        Console.Clear();
                        FirstFunction();
                        break;
                    case 1:
                        Console.Clear();
                        SecondFunction();
                        break;
                    case 2:
                        Console.Clear();
                        ThirdFunction();
                        break;
                }
            }
        }

        private static double ConvertFromAnyToDec(string strNumber, int fromNotation)
        {
            UserInterface.ShowTextFromArray(new string[] {  "(Для упрощения СС - Система счисления)",
                                                            "В первую очередь необходимо привести ()-ю СС к 10 СС.", 
                                                            "Для этого в первую очередь необходимо проиндексировать числа относительно последнего числа, " +
                                                            "либо точки разделяющей число с его дробной частью.",
                                                            "Следующим этапом будет приведение числа к 10 СС, по формуле -",
                                                            "(Элемент числа)*((СС этого числа) в степени индекса этого числа) + все остальные элементы числа",
                                                            "Выглядеть это должно вот так:"});

            int pointIndex = NotationConverter.GetPointPosition(strNumber);
            
            StringBuilder stringBuilder = new StringBuilder();

            strNumber = strNumber.Replace(".", "");

            for (int i = 0; i < strNumber.Length; i++)
            {
                stringBuilder.Append($"{strNumber[i]}({pointIndex - i}), ");
            }

            UserInterface.ShowTextFromArray(new string[] { "Индексация чисел:", stringBuilder.ToString() });

            stringBuilder.Clear();

            for (int i = 0; i < strNumber.Length; i++)
            {
                stringBuilder.Append($"({NotationConverter.ConvertSymbolToDigit(strNumber[i])}*{fromNotation}^({pointIndex - i})) + ");
            }

            UserInterface.ShowTextFromArray(new string[] { "Теперь перевод в десятичную систему счисления:", stringBuilder.ToString() });

            double sum = 0;

            for (int i = 0; i < strNumber.Length; i++)//Непосредственно сам расчёт
            {
                sum += NotationConverter.ConvertSymbolToDigit(strNumber[i]) * Math.Pow(fromNotation,pointIndex - i);
            }

            UserInterface.ShowText($"Из числа {strNumber}, в {fromNotation} СС, мы получили {sum}, в 10 СС");

            return sum;
        }

        private static string ConvertFromDecToAny(double number, int newNotation)
        {
            UserInterface.ShowTextFromArray(new string[] { $"Теперь необходимо получить из числа {number}, в 10 СС, это же число в {newNotation} СС",
                                                            "Для этого нам нужно разделить наше число на новую СС, а остаток от деления будет числом в новой СС",
                                                            "И нужно делить, пока наше число не станет = 0",
                                                            "После расчета всех числел, нужно поставить их задом-наперёд.",
                                                            "При расчете дробной части, мы умножаем на СС и берем от числа, целую часть",
                                                            "Дробную часть, переворачить не нужно.",
                                                            "Для упрощения расчётов, буду считать до 5 знаков после запятой."});

            List<string> operationsOfInteger = new();

            operationsOfInteger.Add("Высчитываем целую часть:");

            int integerNumber = (int)number;

            StringBuilder intNumberBuilder = new StringBuilder();

            while (integerNumber != 0)
            {
                operationsOfInteger.Add($"{integerNumber} / {newNotation} = {NotationConverter.ConvertDigitToSymbol(integerNumber % newNotation)}");
                intNumberBuilder.Append(NotationConverter.ConvertDigitToSymbol(integerNumber % newNotation));
                integerNumber /= newNotation;
            }

            UserInterface.ShowTextFromArray(operationsOfInteger.ToArray());

            operationsOfInteger.Clear();
            operationsOfInteger.Add("Высчитываем дробную часть:");

            StringBuilder doubleNumberBuilder = new StringBuilder();

            List<string> operationsOfDouble = new();

            double residueOfNumber = number - (int)number;
            int countOperation = 0;

            if (residueOfNumber > 0) 
            {
                while (residueOfNumber!=0 && countOperation != 5)
                {
                    countOperation++;

                    operationsOfDouble.Add($"{residueOfNumber} * {newNotation} = {NotationConverter.ConvertDigitToSymbol((int)(residueOfNumber * newNotation))}");
                    doubleNumberBuilder.Append(NotationConverter.ConvertDigitToSymbol((int)(residueOfNumber * newNotation)));
                    residueOfNumber = (residueOfNumber * newNotation) - ((int)(residueOfNumber * newNotation));
                }

                UserInterface.ShowTextFromArray(operationsOfDouble.ToArray());
            }

            return $"{NotationConverter.GetReversOfStrNumber(intNumberBuilder.ToString())}.{doubleNumberBuilder}";
        }

        private static void FirstFunction()
        {
            bool exit = false;
            int originNotation = int.MinValue;
            int endingNotation = int.MinValue;
            string originNumber = string.Empty;


            while (!exit) // get original notation
            {
                string userInput = UserInterface.GetUserInput("Введите систему счисления из которой мы будем конвертировать число");

                if(!int.TryParse(userInput, out originNotation))
                {
                    UserInterface.ShowText("Некорректное число");
                    continue;
                }

                if(originNotation < 1 || originNotation > 50)
                {
                    UserInterface.ShowText("Слишком " + (originNotation < 1 ? "маленькое" : "большое") + " число");
                    continue;
                }

                exit = true;
            }

            exit = false;

            while (!exit) // get number
            {
                originNumber = UserInterface.GetUserInput($"Введите число, которое мы будем конвертировать. Число должно состоять из символов: {string.Join(", ", NotationConverter.GetAlphabetForNotation(originNotation))}");

                if(!IsNumberCorrectInNotation(originNumber, originNotation, out _))
                {
                    UserInterface.ShowText("Некорректное число");
                    continue;
                }

                exit = true;
            }

            exit = false;

            while (!exit) // get notation of number to convert
            {
                string userInput = UserInterface.GetUserInput("Введите систему счисления в которую мы будем конвертировать число");

                if (!int.TryParse(userInput, out endingNotation))
                {
                    UserInterface.ShowText("Некорректное число");
                    continue;
                }

                if (endingNotation < 1 || endingNotation > 50)
                {
                    UserInterface.ShowText("Слишком " + (endingNotation < 1 ? "маленькое" : "большое") + " число");
                    continue;
                }

                exit = true;
            }

            UserInterface.ShowText($"Ваше число: {originNumber}, в системе счисления: {originNotation}, нужно перевести под {endingNotation} систему счисления");

            Thread.Sleep(4800);
            Console.Clear();

            UserInterface.ShowText($"результат вычислений - {ConvertFromDecToAny(ConvertFromAnyToDec(originNumber, originNotation), endingNotation)}");

            UserInterface.GetUserActivity();
        }

        private static void SecondFunction()
        {
            int[] arabicNumeralsEqualsRom = { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };
            string[] romanNumerals = { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };

            int numberForConvert = 0;

            bool exit = false;

            while (!exit) // Получаем число для конвертирования
            {
                string userInput = UserInterface.GetUserInput("Введите число в диапазоне от 1 до 5000");

                if (!int.TryParse(userInput, out numberForConvert))
                {
                    UserInterface.ShowText("Некорректное число");
                    continue;
                }

                if (numberForConvert < 1 || numberForConvert > 5000)
                {
                    UserInterface.ShowText("Слишком " + (numberForConvert < 1 ? "маленькое" : "большое") + " число");
                    continue;
                }

                exit = true;
            }

            List<string> userInformation = new List<string>();

            StringBuilder romNumberBuilder = new StringBuilder();

            for(int i = 0; i < arabicNumeralsEqualsRom.Length; i++)
            {
                if (arabicNumeralsEqualsRom[i] <= numberForConvert)
                {
                    userInformation.Add($"{numberForConvert} - {arabicNumeralsEqualsRom[i]} = {numberForConvert - arabicNumeralsEqualsRom[i]}, ({romanNumerals[i]})");

                    numberForConvert -= arabicNumeralsEqualsRom[i];

                    romNumberBuilder.Append(romanNumerals[i]);

                    i--;
                }
            }

            userInformation.Add($"Результат вычислений - {romNumberBuilder}");

            UserInterface.ShowTextFromArray(userInformation.ToArray());

            UserInterface.GetUserActivity();
        }

        private static void ThirdFunction()
        {
            Dictionary<char, int> dictionartRomToArab = new() { { 'I', 1 }, { 'V', 5 }, { 'X', 10 }, { 'L', 50 }, { 'C', 100 }, { 'D', 500 }, { 'M', 1000 } };

            bool exit = false;

            string userNumber = "";

            while (!exit) // Получаем число для конвертирования
            {
                exit = true;

                userNumber = UserInterface.GetUserInput("Введите число в римской системе счисления");

                if(userNumber.Length < 1)
                {
                    UserInterface.ShowText("Некорректное число");
                    exit = false;
                }

                for (int i = 0; i < userNumber.Length; i++)
                {
                    if (!dictionartRomToArab.TryGetValue(userNumber[i], out _)) 
                    {
                        UserInterface.ShowText("Некорректное число");
                        exit = false;
                    }
                }
            }

            UserInterface.ShowText($"Разбиваем число {userNumber} на символы: {string.Join(" ", userNumber.Split(""))}");

            List<string> userInformation = new List<string>();

            int arabNumberFromRom = 0;

            for (short i = 0; i < userNumber.Length - 1; i++)
            {
                if (dictionartRomToArab[userNumber[i]] < dictionartRomToArab[userNumber[i + 1]])
                {
                    userInformation.Add($"Число слева {dictionartRomToArab[userNumber[i]]} меньше числа справа {dictionartRomToArab[userNumber[i + 1]]}" +
                                        $", поэтому вычитаем из результирующега числа левое {dictionartRomToArab[userNumber[i]]}");

                    arabNumberFromRom -= dictionartRomToArab[userNumber[i]];
                }
                else
                {
                    userInformation.Add($"Число слева {dictionartRomToArab[userNumber[i]]} больше, чем число справа {dictionartRomToArab[userNumber[i + 1]]}," +
                                        $" то прибавляем к результирующему числу левое {dictionartRomToArab[userNumber[i]]}");

                    arabNumberFromRom += dictionartRomToArab[userNumber[i]];
                }
            }

            arabNumberFromRom += dictionartRomToArab[userNumber[^1]];

            userInformation.Add($"Конечное число в арабской системе счисления - {arabNumberFromRom}");

            UserInterface.ShowTextFromArray(userInformation.ToArray());

            UserInterface.GetUserActivity();
        }
    
        private static bool IsNumberCorrectInNotation(string numberInStr, int notation, out bool isDouble)
        {
            isDouble = false;

            for(int i = 0; i < numberInStr.Length; i++)
            {
                if (numberInStr[i] == '.' && !isDouble)
                {
                    isDouble = true;
                    continue;
                }

                int digit = NotationConverter.ConvertSymbolToDigit(numberInStr[i]);

                if (digit == -1 || digit > notation) 
                    return false;
            }

            return true;
        }
        private static char GetSumOfTwoInNotation(char fistNumber, char secindNumber, char additionalValue, int notation, out char additionalValueOut)
        {
            additionalValueOut = '0';

            int resultOfSum = fistNumber + secindNumber + additionalValue;

            additionalValueOut = NotationConverter.ConvertDigitToSymbol((resultOfSum - notation) > 0 ? resultOfSum - notation : 0);

            return NotationConverter.ConvertDigitToSymbol(resultOfSum % notation);
        }
    }
}