using System;
using System.Text;
using System.Globalization;
using System.Collections.Generic;

namespace TPF.Calculation
{
    public class MathParser
    {
        public MathParser()
        {
            try
            {
                _decimalSeparator = char.Parse(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            }
            catch (FormatException ex)
            {
                throw new FormatException("Can't read char decimal separator from system", ex);
            }
        }

        public MathParser(char decimalSeparator)
        {
            _decimalSeparator = decimalSeparator;
        }

        // Marker immer nur ein Zeichen lang um Fehler zu vermeiden
        private const string NumberMarker = "#";
        private const string OperatorMarker = "§";
        private const string FunctionMarker = "?";

        private const string Plus = OperatorMarker + "+";
        private const string UnaryPlus = OperatorMarker + "un+";
        private const string Minus = OperatorMarker + "-";
        private const string UnaryMinus = OperatorMarker + "un-";
        private const string Multiply = OperatorMarker + "*";
        private const string Divide = OperatorMarker + "/";
        private const string Degree = OperatorMarker + "^";
        private const string LeftParenthesis = OperatorMarker + "(";
        private const string RightParenthesis = OperatorMarker + ")";
        private const string Squareroot = FunctionMarker + "sqrt";
        private const string Sin = FunctionMarker + "sin";
        private const string Cos = FunctionMarker + "cos";
        private const string Tan = FunctionMarker + "tan";
        private const string Ctan = FunctionMarker + "ctan";
        private const string Sinh = FunctionMarker + "sinh";
        private const string Cosh = FunctionMarker + "cosh";
        private const string Tanh = FunctionMarker + "tanh";
        private const string Log = FunctionMarker + "log";
        private const string Ln = FunctionMarker + "ln";
        private const string Exp = FunctionMarker + "exp";
        private const string Abs = FunctionMarker + "abs";

        // Unterstützte Operatoren
        private readonly Dictionary<string, string> _supportedOperators = new Dictionary<string, string>
        {
            { "+", Plus },
            { "-", Minus },
            { "*", Multiply },
            { "/", Divide },
            { "^", Degree },
            { "(", LeftParenthesis },
            { ")", RightParenthesis }
        };

        // Unterstützte mathematische Funktionen
        private readonly Dictionary<string, string> _supportedFunctions = new Dictionary<string, string>
        {
            { "sqrt", Squareroot },
            { "√", Squareroot },
            { "sin", Sin },
            { "cos", Cos },
            { "tan", Tan },
            { "ctan", Ctan },
            { "sinh", Sinh },
            { "cosh", Cosh },
            { "tanh", Tanh },
            { "log", Log },
            { "ln", Ln },
            { "exp", Exp },
            { "abs", Abs }
        };

        // Unterstützte Konstanten
        private readonly Dictionary<string, string> _supportedConstants = new Dictionary<string, string>
        {
            {"pi", NumberMarker +  Math.PI.ToString() },
            {"e", NumberMarker + Math.E.ToString() }
        };

        private readonly char _decimalSeparator;
        private bool _isRadians;
        private Dictionary<string, string> _parameters;

        public double Parse(string expression, params MathParserParameter[] parameters)
        {
            return Parse(expression, false, parameters);
        }

        public double Parse(string expression, bool isRadians, params MathParserParameter[] parameters)
        {
            _isRadians = isRadians;

            _parameters = CreateParametersDictionary(parameters);

            try
            {
                var formattedExpression = FormatString(expression);

                var rpnExpression = ConvertToRPN(formattedExpression);

                return Calculate(rpnExpression);
            }
            catch
            {
                throw;
            }
        }

        private static Dictionary<string, string> CreateParametersDictionary(MathParserParameter[] parameters)
        {
            var result = new Dictionary<string, string>();

            // Wenn es keine Parameter gibt brauchen wir auch nichts machen
            if (parameters == null || parameters.Length == 0) return result;

            for (int i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];

                // Doppelte Parameter werden übersprungen
                if (result.ContainsKey(parameter.Token)) continue;

                result.Add(parameter.Token, NumberMarker + parameter.Value.ToString());
            }

            return result;
        }

        private static string FormatString(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression)) throw new ArgumentNullException(nameof(expression));

            var formattedString = new StringBuilder();
            var openParenthesis = 0;

            for (int i = 0; i < expression.Length; i++)
            {
                var currentChar = expression[i];

                if (currentChar == '(') openParenthesis++;
                else if (currentChar == ')') openParenthesis--;

                if (char.IsWhiteSpace(currentChar)) continue;
                else if (char.IsUpper(currentChar)) formattedString.Append(char.ToLower(currentChar));
                else formattedString.Append(currentChar);
            }

            if (openParenthesis != 0)
            {
                if (openParenthesis > 0) throw new FormatException("Too many left parenthesis in the expression");
                else throw new FormatException("Too many right parenthesis in the expression");
            }

            return formattedString.ToString();
        }

        #region Reverse-Polish Notation
        private string ConvertToRPN(string expression)
        {
            var position = 0;
            var outputString = new StringBuilder();
            var stack = new Stack<string>();

            while (position < expression.Length)
            {
                var token = LexicalAnalysisInfixNotation(expression, ref position);

                outputString = SyntaxAnalysisInfixNotation(token, outputString, stack);
            }

            while (stack.Count > 0)
            {
                if (stack.Peek()[0] == OperatorMarker[0])
                {
                    outputString.Append(stack.Pop());
                }
                else
                {
                    throw new FormatException("There is a function without parenthesis");
                }
            }

            return outputString.ToString();
        }

        private string LexicalAnalysisInfixNotation(string expression, ref int position)
        {
            var token = new StringBuilder();
            token.Append(expression[position]);

            // Ist es ein Operator?
            if (_supportedOperators.ContainsKey(token.ToString()))
            {
                var isUnary = position == 0 || expression[position - 1] == '(';
                position++;

                switch (token.ToString())
                {
                    case "+": return isUnary ? UnaryPlus : Plus;
                    case "-": return isUnary ? UnaryMinus : Minus;
                    default: return _supportedOperators[token.ToString()];
                }
            }// Ist es eine Funktion, Konstante oder Parameter?
            else if (char.IsLetter(token[0]) || _supportedFunctions.ContainsKey(token.ToString()) || _supportedConstants.ContainsKey(token.ToString()))
            {
                while (++position < expression.Length && char.IsLetter(expression[position]))
                {
                    token.Append(expression[position]);
                }

                var tokenString = token.ToString();

                if (_supportedFunctions.ContainsKey(tokenString))
                {
                    return _supportedFunctions[tokenString];
                }
                else if (_supportedConstants.ContainsKey(tokenString))
                {
                    return _supportedConstants[tokenString];
                }
                else if (_parameters.ContainsKey(tokenString))
                {
                    return _parameters[tokenString];
                }
                else
                {
                    throw new ArgumentException("Unknown token");
                }
            }
            else if (char.IsDigit(token[0]) || token[0] == _decimalSeparator)
            {
                if (char.IsDigit(token[0]))
                {
                    while (++position < expression.Length && char.IsDigit(expression[position]))
                    {
                        token.Append(expression[position]);
                    }
                }
                else
                {
                    token.Clear();
                }

                // Nachkommastellen der Zahl lesen
                if (position < expression.Length && expression[position] == _decimalSeparator)
                {
                    token.Append(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

                    while (++position < expression.Length && char.IsDigit(expression[position]))
                    {
                        token.Append(expression[position]);
                    }
                }

                // Zahlen in wissenschaftlicher schreibweise parsen (z.B. 1,23e+15)
                if (position + 1 < expression.Length && expression[position] == 'e'
                    && (char.IsDigit(expression[position + 1]) || (position + 2 < expression.Length && (expression[position + 1] == '+' || expression[position + 1] == '-') && char.IsDigit(expression[position + 2]))))
                {
                    token.Append(expression[position++]);

                    if (expression[position] == '+' || expression[position] == '-') token.Append(expression[position++]);

                    while (position < expression.Length && char.IsDigit(expression[position]))
                    {
                        token.Append(expression[position++]);
                    }

                    return NumberMarker + Convert.ToDouble(token.ToString());
                }

                return NumberMarker + token.ToString();
            }
            else
            {
                throw new ArgumentException("Unknown token in expression");
            }
        }

        private static StringBuilder SyntaxAnalysisInfixNotation(string token, StringBuilder outputString, Stack<string> stack)
        {
            // Ist es eine Nummer?          
            if (token[0] == NumberMarker[0])
            {
                outputString.Append(token);
            }// Ist es eine Funktion?
            else if (token[0] == FunctionMarker[0])
            {
                stack.Push(token);
            }// Klammer auf?
            else if (token == LeftParenthesis)
            {
                stack.Push(token);
            }// Klammer zu?
            else if (token == RightParenthesis)
            {
                string element;
                while ((element = stack.Pop()) != LeftParenthesis)
                {
                    outputString.Append(element);
                }

                if (stack.Count > 0 && stack.Peek()[0] == FunctionMarker[0])
                {
                    outputString.Append(stack.Pop());
                }
            }
            else
            {
                while (stack.Count > 0 && HasPriority(token, stack.Peek()))
                {
                    outputString.Append(stack.Pop());
                }

                stack.Push(token);
            }

            return outputString;
        }

        private static bool HasPriority(string token1, string token2)
        {
            return IsRightAssociated(token1) ? GetPriority(token1) < GetPriority(token2) : GetPriority(token1) <= GetPriority(token2);
        }

        private static bool IsRightAssociated(string token)
        {
            return token == Degree;
        }

        private static int GetPriority(string token)
        {
            switch (token)
            {
                case LeftParenthesis:
                    return 0;
                case Plus:
                case Minus:
                    return 2;
                case UnaryPlus:
                case UnaryMinus:
                    return 6;
                case Multiply:
                case Divide:
                    return 4;
                case Degree:
                case Squareroot:
                    return 8;
                case Sin:
                case Cos:
                case Tan:
                case Ctan:
                case Sinh:
                case Cosh:
                case Tanh:
                case Log:
                case Ln:
                case Exp:
                case Abs:
                    return 10;
                default:
                    throw new ArgumentException("Unknown operator");
            }
        }
        #endregion

        #region Calculate Reverse-Polish Notation
        private double Calculate(string expression)
        {
            var position = 0;

            var stack = new Stack<double>();

            while (position < expression.Length)
            {
                var token = LexicalAnalysisRPN(expression, ref position);

                stack = SyntaxAnalysisRPN(stack, token);
            }

            if (stack.Count > 1)
            {
                throw new ArgumentException("Excess operand");
            }

            return stack.Pop();
        }

        private static string LexicalAnalysisRPN(string expression, ref int position)
        {
            var token = new StringBuilder();

            token.Append(expression[position++]);

            while (position < expression.Length && expression[position] != NumberMarker[0] && expression[position] != OperatorMarker[0] && expression[position] != FunctionMarker[0])
            {
                token.Append(expression[position++]);
            }

            return token.ToString();
        }

        private Stack<double> SyntaxAnalysisRPN(Stack<double> stack, string token)
        {
            // Ist es eine Nummer?
            if (token[0] == NumberMarker[0])
            {
                stack.Push(double.Parse(token.Remove(0, 1)));
            } // Ist es eine Operation mit einem Parameter?
            else if (GetNumberOfArguments(token) == 1)
            {
                var value = stack.Pop();
                double result;

                switch (token)
                {
                    case UnaryPlus: result = value; break;
                    case UnaryMinus: result = -value; break;
                    case Squareroot: result = Math.Sqrt(value); break;
                    case Sin: result = ApplyTrigonometricFunction(Math.Sin, value); break;
                    case Cos: result = ApplyTrigonometricFunction(Math.Cos, value); break;
                    case Tan: result = ApplyTrigonometricFunction(Math.Tan, value); break;
                    case Ctan: result = 1 / ApplyTrigonometricFunction(Math.Tan, value); break;
                    case Sinh: result = ApplyTrigonometricFunction(Math.Sinh, value); break;
                    case Cosh: result = ApplyTrigonometricFunction(Math.Cosh, value); break;
                    case Tanh: result = ApplyTrigonometricFunction(Math.Tanh, value); break;
                    case Ln: result = Math.Log(value); break;
                    case Log: result = Math.Log(value, 10.0); break;
                    case Exp: result = Math.Exp(value); break;
                    case Abs: result = Math.Abs(value); break;
                    default: throw new ArgumentException("Unknown operator");
                }

                stack.Push(result);
            }// Ist es eine Operation mit zwei Parameter?
            else
            {
                var value2 = stack.Pop();
                var value1 = stack.Pop();

                double result;

                switch (token)
                {
                    case Plus: result = value1 + value2; break;
                    case Minus: result = value1 - value2; break;
                    case Multiply: result = value1 * value2; break;
                    case Divide:
                    {
                        if (value2 == 0) throw new DivideByZeroException();
                        result = value1 / value2;
                        break;
                    }
                    case Degree: result = Math.Pow(value1, value2); break;
                    default: throw new ArgumentException("Unknown operator");
                }

                stack.Push(result);
            }

            return stack;
        }

        private double ApplyTrigonometricFunction(Func<double, double> function, double value)
        {
            // Die Trigonometrie-Funktionen der Math-Klasse arbeiten mit Radians,
            // also muss der Wert konvertiert werden wenn wir nicht selbst mit Radians rechnen wollen
            if (!_isRadians) value = value * Math.PI / 180;

            return function(value);
        }

        private static int GetNumberOfArguments(string token)
        {
            switch (token)
            {
                case UnaryPlus:
                case UnaryMinus:
                case Squareroot:
                case Tan:
                case Sinh:
                case Cosh:
                case Tanh:
                case Ln:
                case Log:
                case Ctan:
                case Sin:
                case Cos:
                case Exp:
                case Abs:
                    return 1;
                case Plus:
                case Minus:
                case Multiply:
                case Divide:
                case Degree:
                    return 2;
                default:
                    throw new ArgumentException("Unknown operator");
            }
        }
        #endregion
    }
}