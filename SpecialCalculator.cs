﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace finalProject
{
    //public class SpecialCalculator : MyStack<Node>
    //{
    //    // Kiểm tra độ ưu tiên
    //    public static int DoUuTien(string s)
    //    {
    //        switch (s)
    //        {
    //            case "sin":
    //            case "cos":
    //            case "tan":
    //            case "cot": 
    //                return 4;
    //            case "√": 
    //                return 4;
    //            case "^": 
    //                return 3;
    //            case "*":
    //            case "/": 
    //                return 2;
    //            case "+":
    //            case "-": 
    //                return 1;
    //            default: 
    //                return 0;
    //        }
    //    }


    //    public static List<string> TokenizeExpression(string expression)
    //    {
    //        List<string> tokens = new List<string>();
    //        string currentToken = "";

    //        for (int i = 0; i < expression.Length; i++)
    //        {
    //            char c = expression[i];

    //            if (char.IsLetter(c))
    //            {
    //                currentToken += c;
    //                while (i + 1 < expression.Length && char.IsLetter(expression[i + 1]))
    //                {
    //                    currentToken += expression[++i];
    //                }
    //                tokens.Add(currentToken);
    //                currentToken = "";
    //            }
    //            //else if (char.IsDigit(c) || c == '.')
    //            //{
    //            //    currentToken += c;
    //            //    while (i + 1 < expression.Length && (char.IsDigit(expression[i + 1]) || expression[i + 1] == '.'))
    //            //    {
    //            //        currentToken += expression[++i];
    //            //    }
    //            //    tokens.Add(currentToken);
    //            //    currentToken = "";

    //            else if (char.IsDigit(c) || c == '.' || (c == '-' && (i == 0 || "+-*/^√(".Contains(i > 0 ? expression[i - 1] : ' '))))
    //            {
    //                currentToken += c;
    //                while (i + 1 < expression.Length && (char.IsDigit(expression[i + 1]) || expression[i + 1] == '.'))
    //                {
    //                    currentToken += expression[++i];
    //                }
    //                tokens.Add(currentToken);
    //                currentToken = "";
    //            }
    //            else if (c == '+' || c == '-' || c == '*' || c == '/' || c == '^' || c == '√' || c == '(' || c == ')')
    //            {
    //                tokens.Add(c.ToString());
    //            }
    //            else if (c == ' ')
    //            {
    //                continue;
    //            }
    //        }
    //        return tokens;
    //    }

    //    // Chuyển đổi biểu thức infix sang postfix
    //    public static string InfixToPostfix(string infix)
    //    {
    //        MyStack<string> stack = new MyStack<string>();
    //        List<string> postFix = new List<string>();
    //        List<string> tokens = TokenizeExpression(infix);

    //        foreach (string token in tokens)
    //        {
    //            //if (double.TryParse(token, out _) || token == ".")
    //            //{
    //            //    postFix.Add(token);
    //            //}
    //            if (char.IsDigit(token[0]) || token == ".")
    //            {
    //                postFix.Add(token);
    //            }
    //            else
    //            {
    //                if (token == "(")
    //                {
    //                    stack.Push(token);
    //                }
    //                else if (token == ")")
    //                {
    //                    while (stack.Count() > 0 && (string)stack.Peek() != "(")
    //                    {
    //                        postFix.Add(stack.Pop().data.ToString());
    //                    }
    //                    if (stack.Count() > 0 && (string)stack.Peek() == "(")
    //                    {
    //                        stack.Pop(); 
    //                        if (stack.Count() > 0 && IsTrigFunction((string)stack.Peek()))
    //                        {
    //                            postFix.Add(stack.Pop().data.ToString());
    //                        }
    //                    }
    //                }
    //                // Nhận dạng token
    //                else if (IsTrigFunction(token))
    //                {
    //                    stack.Push(token);
    //                }
    //                else
    //                {
    //                    while (stack.Count() > 0 && (string)stack.Peek() != "(" && DoUuTien((string)stack.Peek()) >= DoUuTien(token))
    //                    {
    //                        postFix.Add(stack.Pop().data.ToString());
    //                    }
    //                    stack.Push(token);
    //                }
    //            }
    //        }
    //        while (stack.Count() > 0)
    //        {
    //            postFix.Add(stack.Pop().data.ToString());
    //        }
    //        return string.Join("", postFix);
    //    }

    //    // Nhận dạng hàm lượng giác
    //    public static bool IsTrigFunction(string token)
    //    {
    //        return token == "sin" || token == "cos" || token == "tan" || token == "cot";
    //    }

    //    // Tính toán giá trị của biểu thức postfix
    //    public static double EvaluatePostfix(string postFix, bool isRadianMode)
    //    {
    //        MyStack<double> stack = new MyStack<double>();
    //        string currentToken = "";

    //        for (int i = 0; i < postFix.Length; i++)
    //        {
    //            char ch = postFix[i];

    //            if (char.IsLetter(ch))
    //            {
    //                currentToken += ch;
    //                while (i + 1 < postFix.Length && char.IsLetter(postFix[i + 1]))
    //                {
    //                    currentToken += postFix[++i];
    //                }
    //                if (stack.Count() < 1)
    //                { 
    //                    throw new InvalidOperationException("Không đủ toán hạng cho hàm " + currentToken); 
    //                }

    //                double value = (double)stack.Pop().data;
    //                double result = ProcessTrigFunction(currentToken, value, isRadianMode);
    //                stack.Push(result);
    //                currentToken = "";
    //            }
    //            else if (char.IsDigit(ch) || ch == '.' || (ch == '-' && i == 0))
    //            {
    //                currentToken += ch;
    //                while (i + 1 < postFix.Length && (char.IsDigit(postFix[i + 1]) || postFix[i + 1] == '.'))
    //                {
    //                    currentToken += postFix[++i];
    //                }
    //                if (currentToken != "-")
    //                {
    //                    stack.Push(double.Parse(currentToken));
    //                }
    //                currentToken = "";
    //            }
    //            else
    //            {
    //                if (ch == '√')
    //                {
    //                    if (stack.Count() < 1)
    //                    { 
    //                        throw new InvalidOperationException("Không đủ toán hạng"); 
    //                    }
    //                    double ele = (double)stack.Pop().data;
    //                    if (ele < 0)
    //                    { 
    //                        throw new ArgumentException("Lỗi! Không thể tính căn số âm"); 
    //                    }
    //                    stack.Push(Math.Sqrt(ele));
    //                }
    //                else
    //                {
    //                    if (stack.Count() < 2)
    //                    { 
    //                        throw new InvalidOperationException("Không đủ toán hạng"); 
    //                    }
    //                    double second_op = (double)stack.Pop().data;
    //                    double first_op = (double)stack.Pop().data;
    //                    double result = ProcessOperator(ch, first_op, second_op);
    //                    stack.Push(result);
    //                }
    //            }
    //        }
    //        if (stack.Count() != 1)
    //        { 
    //            throw new InvalidOperationException("Biểu thức không hợp lệ"); 
    //        }
    //        return (double)stack.Pop().data;
    //    }

    //    // Tính toán giá trị của hàm lượng giác
    //    public static double ProcessTrigFunction(string func, double value, bool isRadianMode)
    //    {
    //        double radians;
    //        if (isRadianMode == false)
    //        {
    //            radians = value * Math.PI / 180;
    //        }
    //        else
    //        {
    //            radians = value;
    //        }
    //        switch (func.ToLower())
    //        {
    //            case "sin": 
    //                return Math.Sin(radians);
    //            case "cos": 
    //                return Math.Cos(radians);
    //            case "tan":
    //                if (Math.Abs(Math.Cos(radians)) < 1e-10)
    //                { 
    //                    throw new Exception("Tan undefined for this angle"); 
    //                }
    //                return Math.Tan(radians);
    //            case "cot":
    //                if (Math.Abs(Math.Sin(radians)) < 1e-10)
    //                { 
    //                    throw new Exception("Cot undefined for this angle"); 
    //                }
    //                return 1.0 / Math.Tan(radians);
    //            default: 
    //                throw new ArgumentException($"Unsupported function: {func}");
    //        }
    //    }

    //    // Tính toán với toán tử 1 và 2 ngôi
    //    public static double ProcessOperator(char op, double first, double second)
    //    {
    //        switch (op)
    //        {
    //            case '^': 
    //                return Math.Pow(first, second);
    //            case '+': 
    //                return first + second;
    //            case '-': 
    //                return first - second;
    //            case '*': 
    //                return first * second;
    //            case '/':
    //                if (Math.Abs(second) < 1e-10)
    //                { 
    //                    throw new DivideByZeroException("Lỗi! Không thể chia cho 0"); 
    //                }
    //                return first / second;
    //            default: 
    //                throw new ArgumentException($"Toán tử không hợp lệ: {op}");
    //        }
    //    }
    //}


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
                    if (i == 0 || i > 0 && "+-*/^√(".Contains(expression[i - 1]) || i <= 0 && "+-*/^√(".Contains(' '))
                    {
                        minus = true;
                        i++;
                        if (i >= expression.Length)
                        {
                            // Just a standalone '-', treat as operator token
                            tokens.Add("-");
                            break;
                        }
                        c = expression[i];
                    }
                    else
                    {
                        // binary minus
                        tokens.Add("-");
                        i++;
                        continue;
                    }
                }

                if (char.IsLetter(c))
                {
                    string currentToken = "";
                    if (minus)
                    {
                        currentToken += "-";
                    }
                    currentToken += c;
                    i++;
                    while (i < expression.Length && char.IsLetter(expression[i]))
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
                    while (i < expression.Length && (char.IsDigit(expression[i]) || expression[i] == '.'))
                    {
                        currentToken += expression[i];
                        i++;
                    }
                    tokens.Add(currentToken);
                }
                else if ("+-*/^√()".Contains(c))
                {
                    if (minus)
                    {
                        // unary minus but next char is operator or parenthesis
                        // Add unary minus token explicitly, then add next token if needed
                        tokens.Add("-");
                    }
                    tokens.Add(c.ToString());
                    i++;
                }
                else
                {
                    // Unknown character skip
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
                        postFix.Add(stack.Pop().data.ToString());
                    }
                    if (stack.Count() > 0 && (string)stack.Peek() == "(")
                    {
                        stack.Pop();
                        if (stack.Count() > 0 && IsTrigFunction((string)stack.Peek()))
                        {
                            postFix.Add(stack.Pop().data.ToString());
                        }
                    }
                }
                else if (IsTrigFunction(token) || IsNegativeTrigFunction(token))
                {
                    stack.Push(token);
                }
                else
                {
                    while (stack.Count() > 0 && (string)stack.Peek() != "(" && DoUuTien((string)stack.Peek()) >= DoUuTien(token))
                    {
                        postFix.Add(stack.Pop().data.ToString());
                    }
                    stack.Push(token);
                }
            }

            while (stack.Count() > 0)
            {
                postFix.Add(stack.Pop().data.ToString());
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

        // Kiểm tra chuỗi có phải số hay không (số âm và số d)
        public static bool IsNumber(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }
            double number;
            if (char.IsDigit(token[0]) || token[0] == '+' || token[0] == '-' || token[0] == '.')
            {
                return double.TryParse(token, out number);
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
                    double value = (double)stack.Pop().data;
                    double result = ProcessTrigFunction(token, value, isRadianMode);
                    stack.Push(result);
                }
                else if (IsNegativeTrigFunction(token))
                {
                    if (stack.Count() < 1)
                    {
                        throw new InvalidOperationException("Không đủ toán hạng cho hàm " + token); 
                    }
                    double value = (double)stack.Pop().data;
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
                    double ele = (double)stack.Pop().data;
                    if (ele < 0)
                    {
                        throw new ArgumentException("Lỗi! Không thể tính căn số âm"); 
                    }
                    stack.Push(Math.Sqrt(ele));
                }
                else if ("+-*/^".Contains(token))
                {
                    if (stack.Count() < 2)
                    {
                        throw new InvalidOperationException("Không đủ toán hạng"); 
                    }
                    double second_op = (double)stack.Pop().data;
                    double first_op = (double)stack.Pop().data;
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
            return (double)stack.Pop().data;
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
                    if (Math.Abs(Math.Cos(radians)) < 1e-10)
                    { 
                        throw new Exception("Tan undefined for this angle"); 
                    }
                    return Math.Tan(radians);
                case "cot":
                    if (Math.Abs(Math.Sin(radians)) < 1e-10)
                    {
                        throw new Exception("Cot undefined for this angle"); 
                    }
                    return 1.0 / Math.Tan(radians);
                default:
                    throw new ArgumentException($"Unsupported function: {func}");
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
                    if (Math.Abs(second) < 1e-10)
                    {
                        throw new DivideByZeroException("Lỗi! Không thể chia cho 0"); 
                    }
                    return first / second;
                default:
                    throw new ArgumentException($"Toán tử không hợp lệ: {op}");
            }
        }
    }
}
