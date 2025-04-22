using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace finalProject
{
    public class SpecialCalculator : MyStack<object>
    {
        public static int DoUuTien(string op)
        {
            switch (op)
            {
                case "sin":
                case "cos":
                case "tan":
                case "cot": return 4;
                case "√": return 4;
                case "^": return 3;
                case "×":
                case "÷": return 2;
                case "+":
                case "-": return 1;
                default: return 0;
            }
        }

        public static List<string> TokenizeExpression(string expression)
        {
            List<string> tokens = new List<string>();
            string currentToken = "";

            for (int i = 0; i < expression.Length; i++)
            {
                char c = expression[i];

                if (char.IsLetter(c))
                {
                    currentToken += c;

                    while (i + 1 < expression.Length && char.IsLetter(expression[i + 1]))
                    {
                        currentToken += expression[++i];
                    }

                    tokens.Add(currentToken);
                    currentToken = "";
                }

                else if (char.IsDigit(c) || c == '.')
                {
                    currentToken += c;


                    while (i + 1 < expression.Length && (char.IsDigit(expression[i + 1]) || expression[i + 1] == '.'))
                    {
                        currentToken += expression[++i];
                    }

                    tokens.Add(currentToken);
                    currentToken = "";
                }

                else if (c == '+' || c == '-' || c == '×' || c == '÷' || c == '*' || c == '/' ||
                        c == '^' || c == '√' || c == '(' || c == ')')
                {
                    tokens.Add(c.ToString());
                }

                else if (c == ' ')
                {
                    continue;
                }
            }

            return tokens;
        }

        public static string InfixToPostfix(string infix)
        {
            MyStack<string> stack = new MyStack<string>();
            List<string> postFix = new List<string>();
            bool lastWasDigit = false;

            List<string> tokens = TokenizeExpression(infix);

            foreach (string token in tokens)
            {
                if (double.TryParse(token, out _) || token == ".")
                {
                    postFix.Add(token);
                    lastWasDigit = true;
                }
                else
                {
                    if (lastWasDigit)
                    {
                        postFix.Add(" ");
                        lastWasDigit = false;
                    }

                    if (token == "(")
                    {
                        stack.Push(token);
                    }
                    else if (token == ")")
                    {
                        while (stack.Count() > 0 && (string)stack.Peek() != "(")
                        {
                            postFix.Add(stack.Pop().data.ToString());
                        }

                        if (stack.Count() > 0 && (string)stack.Peek() == "(")
                        {
                            stack.Pop(); // xóa "("
                            if (stack.Count() > 0 && IsTrigFunction((string)stack.Peek()))
                            {
                                postFix.Add(stack.Pop().data.ToString());
                            }
                        }
                    }
                    else if (IsTrigFunction(token))
                    {
                        stack.Push(token);
                    }
                    else if (token != " ")
                    {
                        while (stack.Count() > 0 &&
                               (string)stack.Peek() != "(" &&
                               DoUuTien((string)stack.Peek()) >= DoUuTien(token))
                        {
                            postFix.Add(stack.Pop().data.ToString());
                        }
                        stack.Push(token);
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

        // check if token is a trigonometric function
        public static bool IsTrigFunction(string token)
        {
            return token == "sin" || token == "cos" || token == "tan" || token == "cot";
        }

        public static double EvaluatePostfix(string postFix, bool isRadianMode)
        {
            MyStack<double> stack = new MyStack<double>();
            string currentToken = "";

            for (int i = 0; i < postFix.Length; i++)
            {
                char ch = postFix[i];

                // Xử lý token (số, hàm, toán tử)
                if (char.IsLetter(ch))
                {
                    // Xử lý hàm lượng giác (sin, cos, tan, cot)
                    currentToken += ch;
                    while (i + 1 < postFix.Length && char.IsLetter(postFix[i + 1]))
                    {
                        currentToken += postFix[++i];
                    }

                    if (stack.Count() < 1)
                        throw new InvalidOperationException("Không đủ toán hạng cho hàm " + currentToken);

                    double value = (double)stack.Pop().data;
                    //double result = ProcessTrigFunction(currentToken, value, true);
                    double result = ProcessTrigFunction(currentToken, value, isRadianMode);
                    stack.Push(result);
                    currentToken = "";
                }
                else if (char.IsDigit(ch) || ch == '.' || (ch == '-' && (i == 0 || postFix[i - 1] == ' ')))
                {
                    // Xử lý số (bao gồm số âm và số thập phân)
                    currentToken += ch;
                    while (i + 1 < postFix.Length && (char.IsDigit(postFix[i + 1]) || postFix[i + 1] == '.'))
                    {
                        currentToken += postFix[++i];
                    }

                    if (currentToken != "-") // Tránh trường hợp chỉ có dấu -
                    {
                        stack.Push(double.Parse(currentToken));
                    }
                    currentToken = "";
                }
                else if (ch == ' ')
                {
                    // Bỏ qua khoảng trắng
                    currentToken = "";
                }
                else // Xử lý toán tử
                {
                    if (ch == '√')
                    {
                        // Xử lý căn bậc 2
                        if (stack.Count() < 1)
                            throw new InvalidOperationException("Không đủ toán hạng");
                        double ele = (double)stack.Pop().data;
                        if (ele < 0)
                            throw new ArgumentException("Lỗi! Không thể tính căn số âm");
                        stack.Push(Math.Sqrt(ele));
                    }
                    else
                    {
                        // Xử lý toán tử 2 ngôi
                        if (stack.Count() < 2)
                            throw new InvalidOperationException("Không đủ toán hạng");

                        double second_op = (double)stack.Pop().data;
                        double first_op = (double)stack.Pop().data;
                        double result = ProcessOperator(ch, first_op, second_op);
                        stack.Push(result);
                    }
                }
            }

            if (stack.Count() != 1)
                throw new InvalidOperationException("Biểu thức không hợp lệ");

            return (double)stack.Pop().data;
        }

        public static double ProcessTrigFunction(string func, double value, bool isRadianMode)
        {
            double radians = isRadianMode ? value : (value * Math.PI / 180.0);

            switch (func.ToLower())
            {
                case "sin":
                    return Math.Sin(radians);
                case "cos":
                    return Math.Cos(radians);
                case "tan":
                    if (Math.Abs(Math.Cos(radians)) < 1e-10)
                        throw new Exception("Tan undefined for this angle");
                    return Math.Tan(radians);
                case "cot":
                    if (Math.Abs(Math.Sin(radians)) < 1e-10)
                        throw new Exception("Cot undefined for this angle");
                    return 1.0 / Math.Tan(radians);
                default:
                    throw new ArgumentException($"Unsupported function: {func}");
            }
        }

        public static double ProcessOperator(char op, double first, double second)
        {
            switch (op)
            {
                case '^': return Math.Pow(first, second);
                case '+': return first + second;
                case '-': return first - second;
                case '×': return first * second;
                case '÷':
                    if (Math.Abs(second) < 1e-10)
                        throw new DivideByZeroException("Lỗi! Không thể chia cho 0");
                    return first / second;
                default:
                    throw new ArgumentException($"Toán tử không hợp lệ: {op}");
            }
        }
    }
}
