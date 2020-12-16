using System;
using System.Text.RegularExpressions;

namespace IntegerCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                var inputString = Console.ReadLine();
                if (inputString == "exit") break;

                try
                {
                    Input input = new Input(Regex.Split(inputString, "\\s+|(\\D)"));
                    var inputList = input.GetList();
                    
                    while (inputList.Count >= 3)
                    {
                        var startIndex = input.GetIndex();
                        if (startIndex == -1) continue;

                        var signIndex = startIndex + 1;
                        var endIndex = startIndex + 2;

                        Calculator.number1 = double.Parse(inputList[startIndex]);
                        Calculator.action = inputList[signIndex];
                        Calculator.number2 = double.Parse(inputList[endIndex]);
                        inputList.RemoveRange(startIndex, 3);
                        inputList.Insert(startIndex, Calculator.GetResult().ToString());
                    }

                    Console.WriteLine(Math.Round(double.Parse(inputList[0]), 2));
                }
                catch (Exception e)
                {
                    Console.WriteLine("Wrong input, try again!");
                }
            } while (true);
        }

    }
}