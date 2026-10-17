namespace Kwiq
{
    internal enum TokenType
    {
        // Literals and Identifiers
        Identifier, StringLiteral, IntLiteral, DoubleLiteral, BoolLiteral,

        // Keywords
        Int, Double, String, Bool, Var, // Types
        If, Else, For, Foreach, While, In, // Logic
        Class, Public, Private, Protected, Return, // Access Modifiers
        Print,

        // Operators
        Assign, // =
        Arrow, // =>
        Plus, Minus, Star, Slash, // Math
        Equal, NotEqual, Less, Greater, // Comparisons

        // Delimiters
        Semicolon,      // ;
        Colon,          // :
        Comma,          // ,
        Dot,            // .
        OpenParen,      // (
        CloseParen,     // )
        OpenBrace,      // {
        CloseBrace,     // }
        OpenBracket,    // [
        CloseBracket,   // ]

        // Special
        EOF
    }

    internal record Token(TokenType Type, string Value, int Line);
}