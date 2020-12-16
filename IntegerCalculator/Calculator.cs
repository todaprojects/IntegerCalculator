using System;

namespace IntegerCalculator
{
    public static class Calculator
    {
        public static double number1;
        public static string action;
        public static double number2;

        public static double GetResult()
        {
            switch (action)
            {
                case "+":
                    return number1 + number2;
                case "-":
                    return number1 - number2;
                case "/":
                    return number1 / number2;
                case "*":
                    return number1 * number2;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}