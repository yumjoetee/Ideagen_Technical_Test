using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace WebApplication2
{
    public partial class Default : System.Web.UI.Page
    {
        private static string[] availableOperators = { "-", "+", "/", "*" };


        protected void Page_Load(object sender, EventArgs e)
        {
            double answer = 0;
            answer = Calculate("1 + 1");
            answer = Calculate("2 * 2");
            answer = Calculate("1 + 2 + 3");
            answer = Calculate("6 / 2");
            answer = Calculate("11 + 23");
            answer = Calculate("11.1 + 23");
            answer = Calculate("2 * 3 + 4");
            answer = Calculate("2 + 3 * 4");
            answer = Calculate("77 + 6 / 3");
            answer = Calculate("( 11.5 + 15.4 ) + 10.1");
            answer = Calculate("23 - ( 29.3 - 12.5 )");
            answer = Calculate("( 1 / 2 ) - 1 + 1");
            answer = Calculate("10 - ( 2 + 3 * ( 7 - 5 ) )");
        }

        public static double Calculate(string sum)
        {
            string[] mathExpressionVariableList = sum.Split(' ');
            bool isReversed = false;
            Stack<double> numberStack = new Stack<double>();
            Stack<String> operatorStack = new Stack<String>();

            int index = 0;
            while (index < mathExpressionVariableList.Count())
            {
                string mathExpressionValue = mathExpressionVariableList[index];

                if (mathExpressionValue == "(")
                {
                    int closeBracketIndexPosition = Array.FindLastIndex(mathExpressionVariableList,x=> x == ")");
                    string[] nestedMathExpressionArray = mathExpressionVariableList.Skip(index + 1).Take(closeBracketIndexPosition - index - 1).ToArray();
                    string nestedExpressionString = String.Join(" ", nestedMathExpressionArray);
                    numberStack.Push(Calculate(nestedExpressionString));
                    /* To Skip To the Index after Nexted Expression*/
                    index = closeBracketIndexPosition + 1;
                    continue;

                }
                /* If is Operator */
                else if (Array.Find(availableOperators, x => x == mathExpressionValue) != null)
                {
                    if (operatorStack.Count > 0 && Array.IndexOf(availableOperators, mathExpressionValue) < Array.IndexOf(availableOperators, operatorStack.Peek()))
                    {
                        double value = solve(numberStack.Pop(), numberStack.Pop(), operatorStack.Pop());
                        numberStack.Push(value);
                    }
                    operatorStack.Push(mathExpressionValue);
                }
                /* If Is Number */
                else
                {
                    numberStack.Push(double.Parse(mathExpressionValue));
                }

                index++;
            }


            /* Required Reverse If Whole Expression is oni + and - */
            if (operatorStack.Count > 1 && (operatorStack.ToArray().ToList().IndexOf("*") < 0 && operatorStack.ToArray().ToList().IndexOf("/") < 0))
            {
                Stack<String> revOperatorStack = new Stack<String>();
                Stack<double> revNumberStack = new Stack<double>();

                while (operatorStack.Count != 0)
                {
                    revOperatorStack.Push(operatorStack.Pop());
                }

                while (numberStack.Count != 0)
                {
                    revNumberStack.Push(numberStack.Pop());
                }

                numberStack = revNumberStack;
                operatorStack = revOperatorStack;

                isReversed = true;

            }

            while (operatorStack.Count > 0)
            {
                string mathOperator = operatorStack.Pop();
                double num1 = numberStack.Pop();
                double num2 = numberStack.Pop();
                double value = 0;
                if (isReversed)
                {
                    value = solve(num2, num1, mathOperator);
                }
                else
                {
                    value = solve(num1,num2, mathOperator);
                }
                numberStack.Push(value);
            }

            return numberStack.Pop();
        }

        public static double solve(double num2, double num1, string mathOperator)
        {
            switch (mathOperator)
            {
                case "+":
                    return num1 + num2;
                case "-":
                    return num1 - num2;
                case "*":
                    return num1 * num2;
                case "/":
                    return num1 / num2;
            }
            return 0;
        }


      

    }
}