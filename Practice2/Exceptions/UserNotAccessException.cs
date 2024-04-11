public class UserNotAccessException : Exception
{
    public UserNotAccessException() : base("User does not have access") { }

    public UserNotAccessException(string message) : base(message) { }

    public UserNotAccessException(string message, Exception innerException) : base(message, innerException) { }
}
