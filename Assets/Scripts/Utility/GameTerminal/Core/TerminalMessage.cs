namespace Utility.GameTerminal
{
    public enum MessageType
    {
        Log,
        Warning,
        Error
    }

    public class TerminalMessage
    {
        public MessageType Type { get; }
        public string Text { get; }

        public TerminalMessage(string text, MessageType type)
        {
            Type = type;
            Text = text;
        }
    }
}
