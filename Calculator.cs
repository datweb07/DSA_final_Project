using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace finalProject
{
    public class Calculator
    {
        public class Node
        {
            public Node next;
            public object data;
        }

        public class MyStack<T>
        {
            private Node top;

            public bool IsEmpty()
            {
                return top == null;
            }

            public void Push(object ele)
            {
                Node n = new Node();
                n.data = ele;
                n.next = top;
                top = n;
            }

            public Node Pop()
            {
                if (top == null)
                    return null;
                Node d = top;
                top = top.next;
                return d;
            }

            public object Peek()
            {
                return top.data;
            }

            public int Count()
            {
                int count = 0;
                Node current = top;
                while (current != null)
                {
                    count++;
                    current = current.next;
                }
                return count;
            }
        }

        public static int GetPrecedence(char c)
        {
            switch (c)
            {
                case '√': return 4;
                case '^': return 3;
                case '×':
                case '÷': return 2;
                case '+':
                case '-': return 1;
                default: return 0;
            }
        }

        public static string InfixToPostfix(string infix)
        {
            MyStack<char> stack = new MyStack<char>();
            List<string> postFix = new List<string>();
            bool lastWasDigit = false;

            foreach (char c in infix)
            {
                if (char.IsDigit(c) || c == '.')
                {
                    postFix.Add(c.ToString());
                    lastWasDigit = true;
                }
                else
                {
                    if (lastWasDigit)
                    {
                        postFix.Add(" ");
                        lastWasDigit = false;
                    }

                    if (c == '(')
                    {
                        stack.Push(c);
                    }
                    else if (c == ')')
                    {
                        while (stack.Count() > 0 && (char)stack.Peek() != '(')
                        {
                            postFix.Add(stack.Pop().data.ToString());
                        }
                        stack.Pop();
                    }
                    else if (c != ' ')
                    {
                        while (stack.Count() > 0 && GetPrecedence((char)stack.Peek()) >= GetPrecedence(c))
                        {
                            postFix.Add(stack.Pop().data.ToString());
                        }
                        stack.Push(c);
                    }
                }
            }

            if (lastWasDigit) postFix.Add(" ");

            while (stack.Count() > 0)
            {
                postFix.Add(stack.Pop().data.ToString());
            }

            return string.Join("", postFix);
        }

        public static double EvaluatePostfix(string postFix)
        {
            Stack<double> stack = new Stack<double>();
            string currentNumber = "";

            for (int i = 0; i < postFix.Length; i++)
            {
                char ch = postFix[i];

                if (char.IsDigit(ch) || ch == '.')
                {
                    currentNumber += ch;
                }
                else if (ch == ' ')
                {
                    if (currentNumber != "")
                    {
                        stack.Push(double.Parse(currentNumber));
                        currentNumber = "";
                    }
                }
                else  // Toán tử
                {
                    if (ch == '√')
                    {
                        if (stack.Count() < 1)
                            throw new InvalidOperationException("Not enough operand");
                        double eleSpq = stack.Pop();
                        if (eleSpq < 0)
                            throw new ArgumentException("Negative number");
                        stack.Push(Math.Sqrt(eleSpq));
                    }
                    else
                    {
                        if (stack.Count < 2)
                            throw new InvalidOperationException("Not enough operands for operator");

                        double second_op = stack.Pop();
                        double first_op = stack.Pop();
                        double result = 0;

                        switch (ch)
                        {
                            case '√': result = Math.Sqrt(first_op); break;
                            case '^': result = Math.Pow(first_op, second_op); break;
                            case '+': result = first_op + second_op; break;
                            case '-': result = first_op - second_op; break;
                            case '×': result = first_op * second_op; break;
                            case '÷':
                                if (second_op == 0)
                                    throw new DivideByZeroException();
                                result = first_op / second_op; break;
                            default: throw new ArgumentException($"Invalid operator: {ch}");
                        }
                        stack.Push(result);
                    }
                }
            }

            if (currentNumber != "")
                stack.Push(double.Parse(currentNumber));

            if (stack.Count != 1)
                throw new InvalidOperationException("Invalid expression");

            return stack.Pop();
        }
    }
}
