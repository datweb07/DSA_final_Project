using System;
using System.Collections.Generic;


namespace finalProject
{
    public class SpecialCalculator : MyStack<Node>
    {
        // Kiểm tra độ ưu tiên
        public static int DoUuTien(string s)
        {
            switch (s)
            {
                case "sin":
                case "cos":
                case "tan":
                case "cot":
                    return 4;
                case "√":
                    return 4;
                case "^":
                    return 3;
                case "*":
                case "/":
                    return 2;
                case "+":
                case "-":
                    return 1;
                default:
                    return 0;
            }
        }

        // Tokenize expression xử lý các toán tử, toán hạng, hàm lượng giác
        public static List<string> TokenizeExpression(string expression)
        {
            List<string> tokens = new List<string>();
            int i = 0;
            while (i < expression.Length)
            {
                char c = expression[i];

                if (char.IsWhiteSpace(c))
                {
                    i++;
                    continue;
                }

                // Kiểm tra dấu âm
                bool minus = false;
                if (c == '-')
                {
                    // Được coi là dấu âm thực sự
                    if (
                        i == 0
                        || (
                            i > 0
                            && (
                                expression[i - 1] == '+'
                                || expression[i - 1] == '-'
                                || expression[i - 1] == '*'
                                || expression[i - 1] == '/'
                                || expression[i - 1] == '^'
                                || expression[i - 1] == '√'
                                || expression[i - 1] == '('
                            )
                        )
                    )
                    {
                        minus = true;
                        i++; // Tiến tới kí tự tiếp theo để lấy phần số
                        if (i >= expression.Length)   // Nế không còn kí tự nào sau dấu âm (vượt quá giới hạn)
                        {
                            tokens.Add("-");
                            break;
                        }
                        c = expression[i];  // Cập nhật lại kí tự tiếp theo để ghép với dấu âm
                    }
                    // Được coi là toán tử thông thường
                    else
                    {
                        tokens.Add("-");
                        i++;
                        continue;
                    }
                }

                if (char.IsLetter(c))
                {
                    string currentToken = "";
                    if (minus)   // Nếu trước đó là dấu âm thì ghép vào trước
                    {
                        currentToken += "-";
                    }
                    currentToken += c;
                    i++;
                    while (i < expression.Length && char.IsLetter(expression[i]))   // Gắn các kí tự tiếp theo để tạo hàm hoàn chỉnh
                    {
                        currentToken += expression[i];
                        i++;
                    }
                    tokens.Add(currentToken);
                }
                else if (char.IsDigit(c) || c == '.')
                {
                    string currentToken = "";
                    if (minus)
                    {
                        currentToken += "-";
                    }
                    currentToken += c;
                    i++;
                    while (
                        i < expression.Length && (char.IsDigit(expression[i]) || expression[i] == '.')  // Gắn các phần còn lại của số, kể cả phần thập phân
                    )
                    {
                        currentToken += expression[i];
                        i++;
                    }
                    tokens.Add(currentToken);
                }
                else if (
                    c == '+'
                    || c == '-'
                    || c == '*'
                    || c == '/'
                    || c == '^'
                    || c == '√'
                    || c == '('
                    || c == ')'
                )
                {
                    if (minus)
                    {
                        tokens.Add("-");   // Xem như toán tử bình thường
                    }
                    tokens.Add(c.ToString());
                    i++;
                }
                else
                {
                    // Bỏ qua các ký tự không xác định
                    i++;
                }
            }
            return tokens;
        }

        // Chuyển đổi biểu thức infix sang postfix
        public static string InfixToPostfix(string infix)
        {
            MyStack<string> stack = new MyStack<string>();
            List<string> postFix = new List<string>();
            List<string> tokens = TokenizeExpression(infix);

            foreach (string token in tokens)
            {
                if (IsNumber(token))
                {
                    postFix.Add(token);
                }
                else if (token == "(")
                {
                    stack.Push(token);
                }
                else if (token == ")")
                {
                    while (stack.Count() > 0 && (string)stack.Peek() != "(")
                    {
                        postFix.Add(stack.Pop().Data.ToString());
                    }
                    if (stack.Count() > 0 && (string)stack.Peek() == "(")
                    {
                        stack.Pop();
                        // Nếu trước "(" là hàm lượng giác, đưa vào postFix luôn
                        if (stack.Count() > 0 && IsTrigFunction((string)stack.Peek()))
                        {
                            postFix.Add(stack.Pop().Data.ToString());
                        }
                    }
                }
                else if (IsTrigFunction(token) || IsNegativeTrigFunction(token))
                {
                    stack.Push(token);
                }
                else
                {
                    while (
                        stack.Count() > 0
                        && (string)stack.Peek() != "("
                        && DoUuTien((string)stack.Peek()) >= DoUuTien(token)
                    )
                    {
                        postFix.Add(stack.Pop().Data.ToString());
                    }
                    stack.Push(token);
                }
            }
            // Đưa các toán tử còn lại từ stack vào postFix
            while (stack.Count() > 0)
            {
                postFix.Add(stack.Pop().Data.ToString());
            }

            return string.Join(" ", postFix);
        }

        // Nhận dạng hàm lượng giác
        public static bool IsTrigFunction(string token)
        {
            return token == "sin" || token == "cos" || token == "tan" || token == "cot";
        }

        // Nhận dạng hàm lượng giác âm (ví dụ -sin)
        public static bool IsNegativeTrigFunction(string token)
        {
            if (token.StartsWith("-") && token.Length > 1)
            {
                string func = token.Substring(1);
                return IsTrigFunction(func);
            }
            return false;
        }

        // Kiểm tra token có phải số hay không (số âm và số dương)
        public static bool IsNumber(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }
            double number;
            // Token là kiểu string nên không ép về char được, phải dùng char[0]
            if (char.IsDigit(token[0]) || token[0] == '+' || token[0] == '-' || token[0] == '.')
            {
                return double.TryParse(token, out number);   // Thử chuyển đổi token sang kiểu double. đúng thì trả về true
            }
            // Đây không phải là số
            return false;
        }

        // Tính toán giá trị của biểu thức postfix
        public static double EvaluatePostfix(string postFix, bool isRadianMode)
        {
            MyStack<double> stack = new MyStack<double>();
            string[] tokens = new string[] { };
            foreach (char c in postFix)
            {
                tokens = postFix.Split(' ');
            }

            foreach (string token in tokens)
            {
                if (IsNumber(token))
                {
                    stack.Push(double.Parse(token));
                }
                else if (IsTrigFunction(token))
                {
                    if (stack.Count() < 1)
                    {
                        throw new InvalidOperationException("Không đủ toán hạng cho hàm " + token);
                    }
                    double value = (double)stack.Pop().Data;
                    double result = ProcessTrigFunction(token, value, isRadianMode);
                    stack.Push(result);
                }
                else if (IsNegativeTrigFunction(token))
                {
                    if (stack.Count() < 1)
                    {
                        throw new InvalidOperationException("Không đủ toán hạng cho hàm " + token);
                    }
                    double value = (double)stack.Pop().Data;
                    string func = token.Substring(1);
                    double result = ProcessTrigFunction(func, value, isRadianMode);
                    stack.Push(-result);
                }
                else if (token == "√")
                {
                    if (stack.Count() < 1)
                    {
                        throw new InvalidOperationException("Không đủ toán hạng");
                    }
                    double ele = (double)stack.Pop().Data;
                    if (ele < 0)
                    {
                        throw new ArgumentException("Không thể tính căn số âm");
                    }
                    stack.Push(Math.Sqrt(ele));
                }
                else if (token == "+" || token == "-" || token == "*" || token == "/" || token == "^")
                {
                    if (stack.Count() < 2)
                    {
                        throw new InvalidOperationException("Không đủ toán hạng");
                    }
                    double second_op = (double)stack.Pop().Data;
                    double first_op = (double)stack.Pop().Data;
                    double result = ProcessOperator(token[0], first_op, second_op);
                    stack.Push(result);
                }
                else
                {
                    throw new ArgumentException("Toán tử hoặc hàm không hợp lệ: " + token);
                }
            }

            if (stack.Count() != 1)
            {
                throw new InvalidOperationException("Biểu thức không hợp lệ");
            }
            return (double)stack.Pop().Data;
        }

        // Tính toán giá trị của hàm lượng giác
        public static double ProcessTrigFunction(string func, double value, bool isRadianMode)
        {
            double radians;
            if (isRadianMode == false)
            {
                radians = value * Math.PI / 180;
            }
            else
            {
                radians = value;
            }
            switch (func.ToLower())
            {
                case "sin":
                    return Math.Sin(radians);
                case "cos":
                    return Math.Cos(radians);
                case "tan":
                    return Math.Tan(radians);
                case "cot":
                    return 1.0 / Math.Tan(radians);
                default:
                    throw new ArgumentException($"Không nhận diện được hàm: {func}");
            }
        }

        // Tính toán với toán tử 1 và 2 ngôi
        public static double ProcessOperator(char op, double first, double second)
        {
            switch (op)
            {
                case '^':
                    return Math.Pow(first, second);
                case '+':
                    return first + second;
                case '-':
                    return first - second;
                case '*':
                    return first * second;
                case '/':
                    if (second == 0)
                    {
                        throw new DivideByZeroException();
                    }
                    return first / second;
                default:
                    throw new ArgumentException($"Toán tử không hợp lệ: {op}");
            }
        }
    }
}
