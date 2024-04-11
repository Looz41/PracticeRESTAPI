﻿public class ChatNotFoundException : Exception
{
    public ChatNotFoundException() : base("Chat not found") { }

    public ChatNotFoundException(string message) : base(message) { }

    public ChatNotFoundException(string message, Exception innerException) : base(message, innerException) { }
}
