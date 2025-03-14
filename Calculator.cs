using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace finalProject
{
    public class Calculator
    {
        public static string InfixToPostFix(string infix)
        {
            string postfix = "";
            Stack<char> stack = new Stack<char>();
            for (int i = 0; i < infix.Length; i++)
            {
                char token = infix[i];
                if (char.IsDigit(token))
                {
                    postfix += token;
                }
                else if (token == '(')
                {
                    stack.Push(token);
                }
                else if (token == ')')
                {
                    while (stack.Count > 0 && stack.Peek() != '(')
                    {
                        postfix += stack.Pop();
                    }
                    stack.Pop();
                }
                else
                {
                    while (stack.Count > 0 && Precedence(stack.Peek()) >= Precedence(token))
                    {
                        postfix += stack.Pop();
                    }
                    stack.Push(token);
                }
            }
            while (stack.Count > 0)
            {
                postfix += stack.Pop();
            }
            return postfix;
        }
        
        public static int Precedence(char token)
        {
            if (token == '+' || token == '-')
            {
                return 1;
            }
            else if (token == '*' || token == '/')
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }

        public static double EvaluatePostfix(string postfix)
        {
            Stack<double> stack = new Stack<double>();
            for (int i = 0; i < postfix.Length; i++)
            {
                char token = postfix[i];
                if (char.IsDigit(token))
                {
                    stack.Push(token - '0');
                }
                else
                {
                    double operand2 = stack.Pop();
                    double operand1 = stack.Pop();
                    double result = ApplyOperation(token, operand1, operand2);
                    stack.Push(result);
                }
            }
            return stack.Pop();
        }

        public static double ApplyOperation(char operation, double operand1, double operand2)
        {
            switch (operation)
            {
                case '+':
                    return operand1 + operand2;
                case '-':
                    return operand1 - operand2;
                case '*':
                    return operand1 * operand2;
                case '/':
                    return operand1 / operand2;
                default:
                    return 0;
            }
        }
    }
}
