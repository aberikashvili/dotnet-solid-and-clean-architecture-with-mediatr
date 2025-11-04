using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Persistence;

namespace HR.LeaveManagement.Application.Features.LeaveType.Commands.UpdateLeaveType;

public class UpdateLeaveTypeCommandValidator : AbstractValidator<UpdateLeaveTypeCommand>
{
    private readonly ILeaveTypeRepository _leaveTypeRepository;
    
    public UpdateLeaveTypeCommandValidator(ILeaveTypeRepository leaveTypeRepository)
    {
        RuleFor(p => p.Id)
            .NotNull()
            .MustAsync(LeaveTypeMustExist);

        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull()
            .MaximumLength(70).WithMessage("{PropertyName} must not exceed 70 characters.");

        RuleFor(p => p.DefaultDays)
            .GreaterThan(100).WithMessage("{PropertyName} cannot exceed 100.")
            .LessThan(1).WithMessage("{PropertyName} cannot be less than 1.");

        RuleFor(q => q)
            .MustAsync(LeaveTypeNameUnique)
            .WithMessage("{PropertyName} already exists.");
        
        _leaveTypeRepository = leaveTypeRepository;
    }
    
    private Task<bool> LeaveTypeNameUnique(UpdateLeaveTypeCommand command, CancellationToken calCancellationToken)
    {
        return _leaveTypeRepository.IsLeaveTypeUnique(command.Name);
    }
    
    private async Task<bool> LeaveTypeMustExist(int id, CancellationToken calCancellationToken)
    {
        var leaveType =  await _leaveTypeRepository.GetByIdAsync(id);

        return leaveType.Id == id;
    }
}
