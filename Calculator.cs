using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace finalProject
{
    public class Calculator : MyStack<Node>
    {
        public static int DoUuTien(char c)
        {
            switch (c)
            {
                case '√': return 4;
                case '^': return 3;
                case '*':
                case '/': return 2;
                case '+':
                case '-': return 1;
                default: return 0;
            }
        }

        public static string InfixToPostfix(string infix)
        {
            MyStack<string> stack = new MyStack<string>();
            List<string> postFix = new List<string>();
            string number = "";

            for (int i = 0; i < infix.Length; i++)
            {
                char c = infix[i];

                if (char.IsDigit(c) || c == '.')
                {
                    number += c;
                }
                else if (c == '-' && (i == 0 || infix[i - 1] == '(' || "+-*/^".Contains(infix[i - 1])))
                {
                    // Số âm
                    number += c;
                }
                else if (c == ' ')
                {
                    continue;
                }
                else
                {
                    if (number != "")
                    {
                        postFix.Add(number);
                        number = "";
                    }

                    if (c == '(')
                    {
                        stack.Push(c.ToString());
                    }
                    else if (c == ')')
                    {
                        while (!stack.IsEmpty() && (string)stack.Peek() != "(")
                        {
                            postFix.Add(stack.Pop().data.ToString());
                        }
                        stack.Pop(); // bỏ dấu '('
                    }
                    else // toán tử
                    {
                        string cur = c.ToString();
                        while (!stack.IsEmpty() && DoUuTien(((string)stack.Peek())[0]) >= DoUuTien(cur[0]))
                        {
                            postFix.Add(stack.Pop().data.ToString());
                        }
                        stack.Push(cur);
                    }
                }
            }

            // Thêm số còn lại nếu có
            if (number != "")
            {
                postFix.Add(number);
            }

            // Đẩy các toán tử còn lại ra
            while (!stack.IsEmpty())
            {
                postFix.Add(stack.Pop().data.ToString());
            }

            return string.Join(" ", postFix);
        }

        public static double EvaluatePostfix(string postFix)
        {
            MyStack<double> stack = new MyStack<double>();
            string currentNumber = "";

            for (int i = 0; i < postFix.Length; i++)
            {
                char ch = postFix[i];

                // Xử lý số (bao gồm cả số âm và số thập phân)
                if (char.IsDigit(ch) || ch == '.' || (ch == '-' && (i + 1 < postFix.Length && char.IsDigit(postFix[i + 1]))))
                {
                    currentNumber += ch;
                }
                else if (ch == ' ')
                {
                    if (currentNumber != "")
                    {
                        // Nếu currentNumber bắt đầu bằng '-' và có độ dài > 1, hoặc không bắt đầu bằng '-'
                        if (currentNumber != "-")
                        {
                            stack.Push(double.Parse(currentNumber));
                        }
                        currentNumber = "";
                    }
                }
                else  // Toán tử
                {
                    if (ch == '√')
                    {
                        if (stack.Count() < 1)
                            throw new InvalidOperationException("Không đủ toán hạng");
                        double ele = (double)stack.Pop().data;
                        if (ele < 0)
                            throw new ArgumentException("Lỗi! Là số âm");
                        stack.Push(Math.Sqrt(ele));
                    }
                    else
                    {
                        if (stack.Count() < 2)
                            throw new InvalidOperationException("Không đủ toán hạng");

                        double second_op = (double)stack.Pop().data;
                        double first_op = (double)stack.Pop().data;
                        double result = 0;

                        switch (ch)
                        {
                            case '^': result = Math.Pow(first_op, second_op); break;
                            case '+': result = first_op + second_op; break;
                            case '-': result = first_op - second_op; break;
                            case '*': result = first_op * second_op; break;
                            case '/':
                                if (second_op == 0)
                                    throw new DivideByZeroException();
                                result = first_op / second_op; break;
                            default: throw new ArgumentException($"Invalid operator: {ch}");
                        }
                        stack.Push(result);
                    }
                }
            }

            if (currentNumber != "" && currentNumber != "-")
            {
                stack.Push(double.Parse(currentNumber));
            }

            if (stack.Count() != 1)
                throw new InvalidOperationException("Invalid expression");

            return (double)stack.Pop().data;
        }
    }
}
