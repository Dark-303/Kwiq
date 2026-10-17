namespace Kwiq
{
    internal class Parser
    {
        private readonly List<Token> _tokens;
        private int _current = 0;

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
        }

        internal List<Stmt> Parse()
        {
            List<Stmt> statements = new List<Stmt>();
            while (!IsAtEnd())
            {
                statements.Add(ParseStatement());
            }
            return statements;
        }

        private Stmt ParseStatement()
        {
            if (Check(TokenType.Int) || Check(TokenType.Double) || Check(TokenType.String) || Check(TokenType.Bool) || Check(TokenType.Var))
            {
                return ParseVarDeclaration();
            }

            if (Check(TokenType.Print))
            {
                return ParsePrintStatement();
            }

            throw new Exception($"Unexpected token '{Peek().Value}' at line {Peek().Line}");
        }

        private Stmt ParsePrintStatement()
        {
            Consume(TokenType.Print, "Expect 'print'.");
            Consume(TokenType.OpenParen, "Expect '(' after 'print'.");

            Expr value = ParseExpression();

            Consume(TokenType.CloseParen, "Expect ')' after print expression.");
            Consume(TokenType.Semicolon, "Expect ';' after print statement.");

            return new PrintStmt(value);
        }

        private Stmt ParseVarDeclaration()
        {
            Token typeToken = Advance();
            Token nameToken = Consume(TokenType.Identifier, "Expect variable name after type.");

            Consume(TokenType.Assign, "Expect '=' after variable name.");

            Expr initializer = ParseExpression();

            Consume(TokenType.Semicolon, "Expect ';' after variable declaration.");

            return new VarDeclStmt(typeToken.Value, nameToken.Value, initializer);
        }

        private Expr ParseExpression()
        {
            return ParseAdditive();
        }

        private Expr ParseAdditive()
        {
            Expr expr = ParseMultiplicative();

            while (Match(TokenType.Plus, TokenType.Minus))
            {
                Token op = Previous();
                Expr right = ParseMultiplicative();
                expr = new BinaryExpr(expr, op.Value, right);
            }

            return expr;
        }

        private Expr ParseMultiplicative()
        {
            Expr expr = ParsePrimary();

            while (Match(TokenType.Star, TokenType.Slash))
            {
                Token op = Previous();
                Expr right = ParsePrimary();
                expr = new BinaryExpr(expr, op.Value, right);
            }

            return expr;
        }

        private Expr ParsePrimary()
        {
            if (Match(TokenType.IntLiteral))
            {
                return new LiteralExpr(int.Parse(Previous().Value));
            }
            if (Match(TokenType.DoubleLiteral))
            {
                return new LiteralExpr(double.Parse(Previous().Value));
            }
            if (Match(TokenType.StringLiteral))
            {
                return new LiteralExpr(Previous().Value);
            }
            if (Match(TokenType.BoolLiteral))
            {
                return new LiteralExpr(bool.Parse(Previous().Value));
            }
            if (Match(TokenType.Identifier))
            {
                return new VariableExpr(Previous().Value);
            }
            if (Match(TokenType.OpenParen))
            {
                Expr expr = ParseExpression();
                Consume(TokenType.CloseParen, "Expect ')' after expression.");
                return expr;
            }

            throw new Exception($"Expect expression at line {Peek().Line}, got '{Peek().Value}'");
        }

        private bool IsAtEnd() => Peek().Type == TokenType.EOF;
        private Token Peek() => _tokens[_current];
        private Token Previous() => _tokens[_current - 1];

        private Token Advance()
        {
            if (!IsAtEnd()) _current++;
            return Previous();
        }

        private bool Check(TokenType type) => !IsAtEnd() && Peek().Type == type;

        private bool Match(params TokenType[] types)
        {
            foreach (TokenType type in types)
            {
                if (Check(type))
                {
                    Advance();
                    return true;
                }
            }
            return false;
        }

        private Token Consume(TokenType type, string errorMessage)
        {
            if (Check(type)) return Advance();
            throw new Exception($"Error at line {Peek().Line}: {errorMessage}");
        }
    }
}