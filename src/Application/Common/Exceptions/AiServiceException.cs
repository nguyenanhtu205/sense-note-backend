namespace Application.Common.Exceptions;

public class AiServiceException : Exception
{
    public AiServiceException() : base("AI service error occurred.") { }

    public AiServiceException(string message) : base(message) { }
}
