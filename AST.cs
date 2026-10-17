namespace Kwiq
{
    internal abstract class Stmt { }
    internal abstract class Expr { }

    internal class VarDeclStmt : Stmt
    {
        public string TypeName { get; }
        public string Name { get; }
        public Expr Initializer { get; }

        public VarDeclStmt(string typeName, string name, Expr initializer)
        {
            TypeName = typeName;
            Name = name;
            Initializer = initializer;
        }
    }

    internal class PrintStmt : Stmt
    {
        public Expr Expression { get; }

        public PrintStmt(Expr expression)
        {
            Expression = expression;
        }
    }

    internal class LiteralExpr : Expr
    {
        public object Value { get; }
        public LiteralExpr(object value) => Value = value;
    }

    internal class VariableExpr : Expr
    {
        public string Name { get; }
        public VariableExpr(string name) => Name = name;
    }

    internal class BinaryExpr : Expr
    {
        public Expr Left { get; }
        public string Operator { get; }
        public Expr Right { get; }

        public BinaryExpr(Expr left, string op, Expr right)
        {
            Left = left;
            Operator = op;
            Right = right;
        }
    }
}
