using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

public class Solution
{
    public static List<SyntaxTree> tree = new List<SyntaxTree>();


    // Token Objects
    public abstract class Token
    {
        public TokenType Type;
        public Token(TokenType type) => this.Type = type;

        public virtual TokenType GetType() => this.Type;

        public abstract string GetName();
        public abstract int GetValue();

    }

    public class CommonToken : Token
    {
        public string Representation;
        public CommonToken(TokenType type, string representation) : base(type)
        {
            base.Type = type;
            this.Representation = representation;
        }

        public override string GetName() => this.Representation;
        public override int GetValue() => throw new NotImplementedException();

        public override string ToString() => $"{base.Type} {this.Representation}";
    }

    public class Data : Token
    {
        public int Value;
        public Data(TokenType type, int value) : base(type)
        {
            base.Type = type;
            this.Value = value;
        }

        public override string GetName() => throw new NotImplementedException();

        public override int GetValue() => this.Value;

        public override string ToString() => $"{base.Type} {this.Value}";
    }

    public enum TokenType
    {
        //Numbers
        Number,
        //Punctuators
        LeftParenthesis, RightParenthesis,
        //Operators
        Plus, Minus,
        //EOL
        EndOfLine
    }

    // Lexer 
    private class Lexer
    {
        private readonly string sourceCode;
        private int currentIndex;
        private char currentChar;

        public Lexer(string sourceCode)
        {
            this.sourceCode = sourceCode;
            currentIndex = 0;
            currentChar = sourceCode[currentIndex];
        }

        private void Eat()
        {
            currentIndex++;
            if (currentIndex >= sourceCode.Length)
                return;
            currentChar = sourceCode[currentIndex];
        }

        public List<Token> Tokenize(string sourceCode)
        {
            List<Token> tokens = new List<Token>();
            while (currentIndex < sourceCode.Length)
            {
                switch (currentChar)
                {
                    case ' ':
                        Eat();
                        break;
                    case '(':
                        tokens.Add(new CommonToken(TokenType.LeftParenthesis, currentChar.ToString()));
                        Eat();
                        break;
                    case ')':
                        tokens.Add(new CommonToken(TokenType.RightParenthesis, currentChar.ToString()));
                        Eat();
                        break;
                    case '+':
                        tokens.Add(new CommonToken(TokenType.Plus, currentChar.ToString()));
                        Eat();
                        break;
                    case '-':
                        tokens.Add(new CommonToken(TokenType.Minus, currentChar.ToString()));
                        Eat();
                        break;
                }
                if (char.IsDigit(currentChar))
                {
                    string number = "";

                    while (char.IsDigit(currentChar) & currentIndex < sourceCode.Length)
                    {
                        number += currentChar;
                        Eat();
                    }
                    tokens.Add(new Data(TokenType.Number, int.Parse(number)));
                }
            }

            tokens.Add(new CommonToken(TokenType.EndOfLine, "EOL"));
            return tokens;
        }
    }

    public class SyntaxTree
    {
        public object LeftNode;
        public object RightNode;
        public string Operator;

        public SyntaxTree(object leftNode, string _operator, object rightNode)
        {
            this.LeftNode = leftNode;
            this.Operator = _operator;
            this.RightNode = rightNode;
        }


        public void PrintTree()
        {
            PrintTreeHelper(this, "");
        }

        private void PrintTreeHelper(SyntaxTree node, string indent)
        {
            Console.WriteLine(indent + $"└─<{node.Operator}>");

            if (node.LeftNode is SyntaxTree)
            {
                Console.WriteLine(indent + "   ├─LeftNode: ");
                PrintTreeHelper((SyntaxTree)node.LeftNode, indent + "   │   ");
            }
            else
            {
                Console.WriteLine(indent + "   ├─LeftNode: " + node.LeftNode);
            }

            if (node.RightNode is SyntaxTree)
            {
                Console.WriteLine(indent + "   └─RightNode: ");
                PrintTreeHelper((SyntaxTree)node.RightNode, indent + "        ");
            }
            else
            {
                Console.WriteLine(indent + "   └─RightNode: " + node.RightNode);
            }
        }
    }
    public class Parser
    {
        private readonly List<Token> tokens;
        private int currentTokenIndex;
        private Token currentToken;

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
            currentTokenIndex = 0;
            currentToken = tokens[currentTokenIndex];
        }

        private void Eat()
        {
            currentTokenIndex++;
            currentToken = tokens[currentTokenIndex];
        }

        int ParseTerm()
        {
            switch (currentToken.Type)
            {
                case TokenType.Number:
                    Console.WriteLine($"actual token: {currentToken}");
                    int value = currentToken.GetValue();
                    Eat();
                    return value;
                case TokenType.LeftParenthesis:
                    Console.WriteLine($"actual token: {currentToken}");
                    Eat();
                    int expressionResult = ParseExpression();
                    Eat();
                    return expressionResult;
                default:
                    Eat();
                    value = -ParseTerm();
                    return value;
            }
        }

        int ParseExpression()
        {
            int leftExpression = ParseTerm();

            while (currentToken.Type == TokenType.Plus | currentToken.Type == TokenType.Minus)
            {
                TokenType _operator = currentToken.Type;
                Console.WriteLine($"we stepped at an arithmetic operation: {_operator}");

                switch (_operator)
                {
                    case TokenType.Plus:
                        Eat();
                        int rightExpression = ParseTerm();
                        HandleTree(leftExpression, _operator, rightExpression);
                        leftExpression += rightExpression;
                        break;

                    case TokenType.Minus:
                        Eat();
                        rightExpression = ParseTerm();
                        HandleTree(leftExpression, _operator, rightExpression);
                        leftExpression -= rightExpression;
                        break;
                }
            }

            return leftExpression;
        }
        public int Parse()
        {
            int result = ParseExpression();
            if (currentToken.Type == TokenType.EndOfLine)
            {
                return result;
            }
            else
            {
                Console.WriteLine("hubo algun error");
                return int.MinValue;
            }

        }


        private void HandleTree(int leftExpression, TokenType currentOperator, int rightExpression)
        {
            switch (currentOperator)
            {
                case TokenType.Plus:
                    if (tree.Count == 0)
                        tree.Add(new SyntaxTree(leftExpression, "Plus", rightExpression));
                    else
                    {
                        var oldtree = tree.Last();
                        tree.Clear();
                        var newtree = new SyntaxTree(oldtree, "Plus", rightExpression);
                        tree.Add(newtree);
                    }
                    break;
                default:
                    if (tree.Count == 0)
                        tree.Add(new SyntaxTree(leftExpression, "Minus", rightExpression));
                    else
                    {
                        var oldtree = tree.Last();
                        tree.Clear();
                        var newtree = new SyntaxTree(oldtree, "Minus", rightExpression);
                        tree.Add(newtree);
                    }
                    break;
            }

        }

    }

    public int Calculate(string s)
    {
        Lexer lexer = new Lexer(s);
        List<Token> tokens = lexer.Tokenize(s);

        // </Uncomment this if youu want to check the tokens list
        // Console.WriteLine(string.Join('\n', tokens));
        Parser parser = new Parser(tokens);

        return parser.Parse();
    }


    static void Main(string[] args)
    {
        Console.Clear();
        Solution solution = new Solution();

        // </Add your test cases here
        string[] strings = new[] { "1 + 1", " 2-1 + 2 ", "(1+(4+5+2)-3)+(6+8)", "1-(    -2)", "-2+1" };

        // </Print your test case result
        for (int i = 0; i < strings.Length; i++)
        {
            int result = solution.Calculate(strings[i]);
            Console.WriteLine($"test case: {i + 1}");

            Console.WriteLine("\n>" + strings[i]);
            tree.Last().PrintTree();
            Console.WriteLine();
            tree.Clear();
            Console.WriteLine($"Result: <{result}>");   
            Console.WriteLine("--------------------------------");
        }
    }
}
