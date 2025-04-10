using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace finalProject
{
    public class SpecialCalculator
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
                return top?.data;
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

        public static int GetPrecedence(string op)
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
                case "*":
                case "÷":
                case "/":
                    return 2;
                case "+":
                case "-": return 1;
                default: return 0;
            }
        }

        public static string InfixToPostfix(string infix)
        {
            MyStack<string> stack = new MyStack<string>();
            List<string> postFix = new List<string>();
            bool lastWasDigit = false;

            infix = infix.Replace("*", "×").Replace("/", "÷");

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
                               GetPrecedence((string)stack.Peek()) >= GetPrecedence(token))
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
        private static bool IsTrigFunction(string token)
        {
            return token == "sin" || token == "cos" || token == "tan" || token == "cot";
        }

        private static List<string> TokenizeExpression(string expression)
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


        public static double EvaluatePostfix(string postFix)
        {
            Stack<double> stack = new Stack<double>();
            List<string> tokens = TokenizePostfix(postFix);

            foreach (string token in tokens)
            {

                if (double.TryParse(token, out double number))
                {
                    stack.Push(number);
                }

                else if (token != " ")
                {
                    switch (token)
                    {
                        case "sin":
                            if (stack.Count < 1)
                                throw new InvalidOperationException("Not enough operands for sin function");
                            double sinValue = stack.Pop();
                            stack.Push(Math.Sin(DegToRad(sinValue))); // Convert degrees to radians
                            break;

                        case "cos":
                            if (stack.Count < 1)
                                throw new InvalidOperationException("Not enough operands for cos function");
                            double cosValue = stack.Pop();
                            stack.Push(Math.Cos(DegToRad(cosValue)));
                            break;

                        case "tan":
                            if (stack.Count < 1)
                                throw new InvalidOperationException("Not enough operands for tan function");
                            double tanValue = stack.Pop();

                            double tanRadians = DegToRad(tanValue);
                            if (Math.Abs(Math.Cos(tanRadians)) < 1e-10)
                                throw new DivideByZeroException("Tangent is undefined for this angle");
                            stack.Push(Math.Tan(tanRadians));
                            break;

                        case "cot":
                            if (stack.Count < 1)
                                throw new InvalidOperationException("Not enough operands for cot function");
                            double cotValue = stack.Pop();

                            double cotRadians = DegToRad(cotValue);
                            if (Math.Abs(Math.Sin(cotRadians)) < 1e-10)
                                throw new DivideByZeroException("Cotangent is undefined for this angle");
                            stack.Push(1.0 / Math.Tan(cotRadians));
                            break;

                        case "√":
                            if (stack.Count < 1)
                                throw new InvalidOperationException("Not enough operands for square root");
                            double sqrtValue = stack.Pop();
                            if (sqrtValue < 0)
                                throw new ArgumentException("Cannot calculate square root of a negative number");
                            stack.Push(Math.Sqrt(sqrtValue));
                            break;

                        case "^":
                            if (stack.Count < 2)
                                throw new InvalidOperationException("Not enough operands for power operator");
                            double exponent = stack.Pop();
                            double baseValue = stack.Pop();
                            stack.Push(Math.Pow(baseValue, exponent));
                            break;

                        case "+":
                            if (stack.Count < 2)
                                throw new InvalidOperationException("Not enough operands for addition");
                            double addend2 = stack.Pop();
                            double addend1 = stack.Pop();
                            stack.Push(addend1 + addend2);
                            break;

                        case "-":
                            if (stack.Count < 2)
                                throw new InvalidOperationException("Not enough operands for subtraction");
                            double subtrahend = stack.Pop();
                            double minuend = stack.Pop();
                            stack.Push(minuend - subtrahend);
                            break;

                        case "×":
                        case "*":
                            if (stack.Count < 2)
                                throw new InvalidOperationException("Not enough operands for multiplication");
                            double factor2 = stack.Pop();
                            double factor1 = stack.Pop();
                            stack.Push(factor1 * factor2);
                            break;

                        case "÷":
                        case "/":
                            if (stack.Count < 2)
                                throw new InvalidOperationException("Not enough operands for division");
                            double divisor = stack.Pop();
                            if (Math.Abs(divisor) < 1e-10)
                                throw new DivideByZeroException("Cannot divide by zero");
                            double dividend = stack.Pop();
                            stack.Push(dividend / divisor);
                            break;

                        default:
                            throw new ArgumentException($"Invalid operator or function: {token}");
                    }
                }
            }


            if (stack.Count == 0)
                throw new InvalidOperationException("Empty expression or evaluation error");


            if (stack.Count > 1)
            {
                Console.WriteLine($"Warning: Expression may be incomplete. {stack.Count} values left on stack.");
            }

            return stack.Pop();
        }


        private static List<string> TokenizePostfix(string postfix)
        {
            List<string> tokens = new List<string>();
            string currentToken = "";

            for (int i = 0; i < postfix.Length; i++)
            {
                char c = postfix[i];


                if (char.IsLetter(c))
                {
                    currentToken += c;
                    while (i + 1 < postfix.Length && char.IsLetter(postfix[i + 1]))
                    {
                        currentToken += postfix[++i];
                    }
                    tokens.Add(currentToken);
                    currentToken = "";
                }

                else if (char.IsDigit(c) || c == '.')
                {
                    currentToken += c;
                    while (i + 1 < postfix.Length && (char.IsDigit(postfix[i + 1]) || postfix[i + 1] == '.'))
                    {
                        currentToken += postfix[++i];
                    }
                    tokens.Add(currentToken);
                    currentToken = "";
                }

                else if (c == '+' || c == '-' || c == '×' || c == '÷' || c == '*' || c == '/' ||
                         c == '^' || c == '√')
                {
                    tokens.Add(c.ToString());
                }

                else if (c == ' ')
                {
                    if (currentToken != "")
                    {
                        tokens.Add(currentToken);
                        currentToken = "";
                    }
                }
            }

            if (currentToken != "")
                tokens.Add(currentToken);

            return tokens;
        }

        // convert degrees to radians
        public static double DegToRad(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }

        // convert radians to degrees
        public static double RadToDeg(double radians)
        {
            return radians * 180.0 / Math.PI;
        }

        //  testing expressions
        public static double Calculate(string expression)
        {
            try
            {
                string postfix = InfixToPostfix(expression);
                Console.WriteLine($"Postfix: {postfix}");
                return EvaluatePostfix(postfix);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating: {ex.Message}");
                return double.NaN;
            }
        }
    }
}
