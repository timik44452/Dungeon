namespace Utility.GameTerminal
{
    public class ParseResult
    {
        public bool IsMethod { get; set; }
        public string Operand {get;}
        public string Value {get;}

        public ParseResult(string operand)
        {
            Operand = operand;
            Value = string.Empty;
        }

        public ParseResult(string operand, string value)
        {
            Operand = operand;
            Value = value;
        }
    }
}
