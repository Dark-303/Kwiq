using Kwiq;

string sourcePath = "main.kq";

if (!File.Exists(sourcePath))
{
    Console.WriteLine($"Error: {sourcePath} not found in project directory!");
    return;
}

string code = File.ReadAllText(sourcePath);

try
{
    var lexer = new Lexer(code);
    var tokens = lexer.Tokenize();

    var parser = new Parser(tokens);
    var statements = parser.Parse();

    var interpreter = new Interpreter();
    interpreter.Interpret(statements);

    Console.WriteLine($"\nSuccessfully parsed {statements.Count} statement(s) into an Abstract Syntax Tree!");
}
catch (Exception ex)
{
    Console.WriteLine($"\nCompiler Error: {ex.Message}");
}