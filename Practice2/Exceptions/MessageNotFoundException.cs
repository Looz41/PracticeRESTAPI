public class MessageNotFoundException : Exception
{
    public MessageNotFoundException() : base("Message not found") { }

    public MessageNotFoundException(string message) : base(message) { }

    public MessageNotFoundException(string message, Exception innerException) : base(message, innerException) { }
}
