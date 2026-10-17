namespace Kwiq
{
    internal class Lexer
    {
        private readonly string _source;
        private int _position = 0;
        private int _line = 1;

        private static readonly Dictionary<string, TokenType> Keywords = new()
        {
            { "int", TokenType.Int },
            { "double", TokenType.Double },
            { "string", TokenType.String },
            { "bool", TokenType.Bool },
            { "var", TokenType.Var },
            { "if", TokenType.If },
            { "else", TokenType.Else },
            { "for", TokenType.For },
            { "foreach", TokenType.Foreach },
            { "while", TokenType.While },
            { "in", TokenType.In },
            { "class", TokenType.Class },
            { "public", TokenType.Public },
            { "private", TokenType.Private },
            { "protected", TokenType.Protected },
            { "print", TokenType.Print },
            { "return", TokenType.Return },
            { "true", TokenType.BoolLiteral },
            { "false", TokenType.BoolLiteral }
        };

        public Lexer(string source)
        {
            _source = source.TrimEnd();
        }

        internal List<Token> Tokenize()
        {
            List<Token> tokens = new List<Token>();
            while (_position < _source.Length)
            {
                char current = _source[_position];
                if (char.IsWhiteSpace(current))
                {
                    if (current == '\n') _line++;
                    _position++;
                    continue;
                }

                if (current == '/' && Peek() == '/')
                {
                    while (_position < _source.Length && _source[_position] != '\n')
                        _position++;
                    continue;
                }

                if (current == '=')
                {
                    if (Peek() == '>')
                    {
                        _position += 2;
                        tokens.Add(new Token(TokenType.Arrow, "=>", _line));
                        continue;
                    }
                    if (Peek() == '=')
                    {
                        _position += 2;
                        tokens.Add(new Token(TokenType.Equal, "==", _line));
                        continue;
                    }
                    tokens.Add(ReadSingle(TokenType.Assign, "="));
                    continue;
                }

                if (current == '!' && Peek() == '=')
                {
                    _position += 2;
                    tokens.Add(new Token(TokenType.NotEqual, "!=", _line));
                    continue;
                }

                switch (current)
                {
                    case '+': tokens.Add(ReadSingle(TokenType.Plus, "+")); continue;
                    case '-': tokens.Add(ReadSingle(TokenType.Minus, "-")); continue;
                    case '*': tokens.Add(ReadSingle(TokenType.Star, "*")); continue;
                    case '/': tokens.Add(ReadSingle(TokenType.Slash, "/")); continue;
                    case '<': tokens.Add(ReadSingle(TokenType.Less, "<")); continue;
                    case '>': tokens.Add(ReadSingle(TokenType.Greater, ">")); continue;
                    case ';': tokens.Add(ReadSingle(TokenType.Semicolon, ";")); continue;
                    case ':': tokens.Add(ReadSingle(TokenType.Colon, ":")); continue;
                    case ',': tokens.Add(ReadSingle(TokenType.Comma, ",")); continue;
                    case '.': tokens.Add(ReadSingle(TokenType.Dot, ".")); continue;
                    case '(': tokens.Add(ReadSingle(TokenType.OpenParen, "(")); continue;
                    case ')': tokens.Add(ReadSingle(TokenType.CloseParen, ")")); continue;
                    case '{': tokens.Add(ReadSingle(TokenType.OpenBrace, "{")); continue;
                    case '}': tokens.Add(ReadSingle(TokenType.CloseBrace, "}")); continue;
                    case '[': tokens.Add(ReadSingle(TokenType.OpenBracket, "[")); continue;
                    case ']': tokens.Add(ReadSingle(TokenType.CloseBracket, "]")); continue;
                }

                if (current == '"')
                {
                    tokens.Add(ReadString());
                    continue;
                }

                if (char.IsDigit(current))
                {
                    tokens.Add(ReadNumber());
                    continue;
                }

                if (char.IsLetter(current) || current == '_')
                {
                    tokens.Add(ReadIdentifierOrKeyword());
                    continue;
                }

                throw new Exception($"Unexpected character '{current}' at line {_line}");
            }

            tokens.Add(new Token(TokenType.EOF, "", _line));
            return tokens;
        }

        private Token ReadSingle(TokenType type, string val)
        {
            _position++;
            return new Token(type, val, _line);
        }

        private char Peek() => _position + 1 < _source.Length ? _source[_position + 1] : '\0';

        private Token ReadString()
        {
            _position++;
            int start = _position;
            while (_position < _source.Length && _source[_position] != '"')
            {
                if (_source[_position] == '\n') _line++;
                _position++;
            }
            string text = _source[start.._position];
            _position++;
            return new Token(TokenType.StringLiteral, text, _line);
        }

        private Token ReadNumber()
        {
            int start = _position;
            bool isDouble = false;
            while (_position < _source.Length && (char.IsDigit(_source[_position]) || _source[_position] == '.'))
            {
                if (_source[_position] == '.') isDouble = true;
                _position++;
            }
            string val = _source[start.._position];
            return new Token(isDouble ? TokenType.DoubleLiteral : TokenType.IntLiteral, val, _line);
        }

        private Token ReadIdentifierOrKeyword()
        {
            int start = _position;
            while (_position < _source.Length && (char.IsLetterOrDigit(_source[_position]) || _source[_position] == '_'))
            {
                _position++;
            }
            string word = _source[start.._position];
            TokenType type = Keywords.TryGetValue(word, out TokenType kwType) ? kwType : TokenType.Identifier;
            return new Token(type, word, _line);
        }
    }
}
