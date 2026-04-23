namespace Application.Common.Exceptions;

public class ValidationException(List<ValidationFailure> errors)
    : Exception("One or more validation failures have occurred.")
{
    public List<ValidationFailure> Errors { get; } = errors;
}
