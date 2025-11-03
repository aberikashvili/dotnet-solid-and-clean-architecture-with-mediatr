using AutoMapper;
using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveType.Commands.CreateLeaveType;

public class CreateLeaveTypeCommandHandler(IMapper mapper, ILeaveTypeRepository leaveTypeRepository)
    : IRequestHandler<CreateLeaveTypeCommand, int>
{
    public async Task<int> Handle(CreateLeaveTypeCommand request, CancellationToken cancellationToken)
    {
        // Validate incoming data
        var validator = new CreateLeaveTypeCommandValidator(leaveTypeRepository);
        var validationResult = await validator.ValidateAsync(request);

        if (validationResult.Errors.Any())
        {
            throw new BadRequestException($"Invalid LeaveType", validationResult);
        }

        // Convert to domain entity object
        var leaveTypeToCreate = mapper.Map<Domain.LeaveType>(request);

        // Add to database
        await leaveTypeRepository.CreateAsync(leaveTypeToCreate);

        // Return record id
        return leaveTypeToCreate.Id;
    }
}
