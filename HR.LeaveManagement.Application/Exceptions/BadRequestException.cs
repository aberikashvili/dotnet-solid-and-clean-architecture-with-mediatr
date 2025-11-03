using FluentValidation.Results;

namespace HR.LeaveManagement.Application.Exceptions;

public class BadRequestException(string message) : Exception(message)
{
    public BadRequestException(string message, ValidationResult validationResult) : this(message)
    {
        ValidationErrors = new();

        foreach (var error in validationResult.Errors)
        {
            ValidationErrors.Add(error.ErrorMessage);
        }
    }

    private List<string> ValidationErrors { get; set; }
}