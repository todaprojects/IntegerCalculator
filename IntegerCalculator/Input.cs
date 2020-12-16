using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace IntegerCalculator
{
    public class Input
    {
        private string[] input;
        private List<string> inputList;
        private static List<string> symb = new List<string>() {"+", "-", "/", "*", "("};

        public Input(string[] input)
        {
            this.input = input;
            inputList = new List<string>();
        }

        public List<string> GetList()
        {
            foreach (var i in input)
            {
                if (!string.IsNullOrEmpty(i))
                {
                    Match m = Regex.Match(i, @"(\(|\)|\d+|\+|\-|\*|\/|\.)");
                    if (!m.Success)
                        throw new ArgumentException();

                    inputList.Add(i);
                }
            }

            ParseNumbers();
            return inputList;
        }

        private void ParseNumbers()
        {
            if (inputList.Count >= 3)
            {
                // if "minus" sign is the first element, - it is concatenated with the further element
                if (inputList[0] == "-")
                {
                    ConcatMinus(0);
                }

                // if "minus" sign comes after an arithmetic operator, - it is concatenated with the further element
                for (int i = 1; i < inputList.Count - 1; i++)
                {
                    if (inputList[i] == "-" && symb.Contains(inputList[i - 1]))
                    {
                        ConcatMinus(i);
                    }

                    if (inputList[i] == ".")
                    {
                        ConcatDot(i);
                    }
                }
            }
        }

        private void ConcatMinus(int index)
        {
            inputList[index + 1] = "-" + inputList[index + 1];
            inputList.RemoveAt(index);
        }
        
        private void ConcatDot(int index)
        {
            inputList[index + 1] = inputList[index - 1] + "." + inputList[index + 1];
            inputList.RemoveRange(index - 1, 2);
        }
        
        public int GetIndex()
        {
            int startIndex = inputList.LastIndexOf("(");
            int endIndex = inputList.IndexOf(")", startIndex + 1);

            if (startIndex != -1 && endIndex != -1)
            {
                string exprBetweenBrackets = "";
                List<string> list = inputList.GetRange(startIndex + 1, endIndex - startIndex - 1);
                foreach (var s in list)
                {
                    exprBetweenBrackets += s;
                }

                if (double.TryParse(exprBetweenBrackets, out double number))
                {
                    // removes one or repeating pair of parentheses around a result of solved arithmetic expression
                    while (inputList[startIndex] == "(" && inputList[endIndex] == ")")
                    {
                        inputList.RemoveAt(endIndex--);
                        inputList.RemoveAt(startIndex--);
                        if (endIndex >= inputList.Count) endIndex = inputList.Count - 1;
                        if (startIndex < 0) startIndex = 0;
                    }

                    startIndex = inputList.LastIndexOf("(");
                    endIndex = inputList.IndexOf(")", startIndex + 1);
                }
            }

            return CheckPriotity(startIndex, endIndex);
        }
        
        private int CheckPriotity(int startIndex, int endIndex)
        {
            var inputListRange = startIndex != endIndex && startIndex != -1 && endIndex != -1
                ? inputList.GetRange(startIndex + 1, endIndex - startIndex - 1)
                : inputList;

            if (inputListRange.Count == 1) return -1;

            foreach (var sign in new List<string>() {"*", "/"})
            {
                var index = inputListRange.IndexOf(sign);
                if (index != -1)
                {
                    return index + startIndex;
                }
            }

            return startIndex + 1;
        }
    }
}