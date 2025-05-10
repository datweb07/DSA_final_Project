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
        // Kiểm tra độ ưu tiên
        public static int DoUuTien(char c)
        {
            switch (c)
            {
                case '√': 
                    return 4;
                case '^': 
                    return 3;
                case '*':
                case '/': 
                    return 2;
                case '+':
                case '-': 
                    return 1;
                default: 
                    return 0;
            }
        }

        // Chuyển đổi biểu thức infix sang postfix
        public static string InfixToPostfix(string infix)
        {
            MyStack<string> stack = new MyStack<string>();
            List<string> postFix = new List<string>();
            string createNumber = "";

            for (int i = 0; i < infix.Length; i++)
            {
                char c = infix[i];

                if (char.IsDigit(c) || c == '.')
                {
                    createNumber += c;
                }
                else if (c == '-' && (i == 0 || infix[i - 1] == '(' || "+-*/^".Contains(infix[i - 1])))
                {
                    createNumber += c;
                }
                else if (c == ' ')
                {
                    continue;
                }
                else
                {
                    // Hoàn thành số hiện tại và thêm vào postFix
                    if (createNumber != "")
                    {
                        postFix.Add(createNumber);
                        createNumber = "";
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
                        stack.Pop(); 
                    }
                    // Kiếm tra mức độ ưu tiên của toán tử
                    else 
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

            // Đảm bảo rằng số cuối cùng trong biểu thức cũng được thêm vào postFix
            if (createNumber != "")
            {
                postFix.Add(createNumber);
            }

            // Đẩy các toán tử còn lại từ stakk vào postFix
            while (!stack.IsEmpty())
            {
                postFix.Add(stack.Pop().data.ToString());
            }

            return string.Join(" ", postFix);
        }

        // Tính toán giá trị của biểu thức postfix
        public static double EvaluatePostfix(string postFix)
        {
            MyStack<double> stack = new MyStack<double>();
            string createNumber = "";

            for (int i = 0; i < postFix.Length; i++)
            {
                char ch = postFix[i];

                // Xử lý số (bao gồm cả số âm và số thập phân)
                if (char.IsDigit(ch) || ch == '.' || (ch == '-' && (i + 1 < postFix.Length && char.IsDigit(postFix[i + 1]))))
                {
                    createNumber += ch;
                }
                else if (ch == ' ') // Nếu gặp khoảng trắng, việc xây dựng số đã hoàn tất
                {
                    if (createNumber != "") // Nếu createNumber không rỗng, các ký tự trước khoảng trắng đã hình thành số hợp lệ và cần xử lý tiếp
                    {
                        if (createNumber != "-") // Đảm bảo không tồn tại dấu - riêng lẻ trong stack mà phải là số âm hoàn chỉnh
                        {
                            stack.Push(double.Parse(createNumber));
                        }
                        createNumber = "";
                    }
                }
                // Tính toán với các toán hạng và toán tử
                else
                {
                    // Toán tử 1 ngôi
                    if (ch == '√')
                    {
                        if (stack.Count() < 1)
                        {
                            throw new InvalidOperationException("Không đủ toán hạng");
                        }    
                        double ele = (double)stack.Pop().data;
                        if (ele < 0)
                        { 
                            throw new ArgumentException("Lỗi! Là số âm"); 
                        }
                        stack.Push(Math.Sqrt(ele));
                    }
                    // Toán tử 2 ngôi
                    else
                    {
                        if (stack.Count() < 2)
                        { 
                            throw new InvalidOperationException("Không đủ toán hạng"); 
                        }

                        double second_op = (double)stack.Pop().data;
                        double first_op = (double)stack.Pop().data;
                        double result = 0;

                        switch (ch)
                        {
                            case '^': 
                                result = Math.Pow(first_op, second_op); 
                                break;
                            case '+': 
                                result = first_op + second_op; 
                                break;
                            case '-': 
                                result = first_op - second_op; 
                                break;
                            case '*': 
                                result = first_op * second_op; 
                                break;
                            case '/':
                                if (second_op == 0)
                                { 
                                    throw new DivideByZeroException(); 
                                }
                                result = first_op / second_op; 
                                break;
                            default: throw new ArgumentException($"Toán tử không hợp lệ: {ch}");
                        }
                        stack.Push(result);
                    }
                }
            }

            // Đảm bảo số cuối trùng được thêm vào stack và không phải là dấu âm riêng lẻ
            if (createNumber != "" && createNumber != "-")
            {
                stack.Push(double.Parse(createNumber));
            }

            // Kiểm tra tính hợp lệ của biểu thức trong stack
            if (stack.Count() != 1)
            { 
                throw new InvalidOperationException("Biểu thức không hợp lệ"); 
            }

            return (double)stack.Pop().data;
        }
    }
}
