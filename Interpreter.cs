namespace Kwiq
{
    internal class Interpreter
    {
        private readonly Dictionary<string, object> _variables = new();

        public void Interpret(List<Stmt> statements)
        {
            foreach (var stmt in statements)
            {
                Execute(stmt);
            }
        }

        private void Execute(Stmt stmt)
        {
            switch (stmt)
            {
                case VarDeclStmt varDecl:
                    object value = Evaluate(varDecl.Initializer);
                    _variables[varDecl.Name] = value;
                    break;

                case PrintStmt printStmt:
                    object valToPrint = Evaluate(printStmt.Expression);
                    Console.WriteLine(valToPrint);
                    break;

                default:
                    throw new Exception($"Unknown statement type: {stmt.GetType().Name}");
            }
        }

        private object Evaluate(Expr expr)
        {
            return expr switch
            {
                LiteralExpr literal => literal.Value,

                VariableExpr variable => _variables.TryGetValue(variable.Name, out var val)
                    ? val
                    : throw new Exception($"Undefined variable '{variable.Name}'"),

                BinaryExpr binary => EvaluateBinary(binary),

                _ => throw new Exception($"Unknown expression type: {expr.GetType().Name}")
            };
        }

        private object EvaluateBinary(BinaryExpr binary)
        {
            object left = Evaluate(binary.Left);
            object right = Evaluate(binary.Right);

            return binary.Operator switch
            {
                "+" => Add(left, right),
                "-" => Convert.ToDouble(left) - Convert.ToDouble(right),
                "*" => Convert.ToDouble(left) * Convert.ToDouble(right),
                "/" => Convert.ToDouble(left) / Convert.ToDouble(right),
                _ => throw new Exception($"Unknown operator '{binary.Operator}'")
            };
        }

        private object Add(object left, object right)
        {
            if (left is int lInt && right is int rInt) return lInt + rInt;
            if (left is string || right is string) return left.ToString() + right.ToString();
            return Convert.ToDouble(left) + Convert.ToDouble(right);
        }
    }
}