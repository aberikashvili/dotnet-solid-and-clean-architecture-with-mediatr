using System.Text.Json;
using AutoMapper;
using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveType.Commands.CreateLeaveType;

public class CreateLeaveTypeCommandHandler(
    IMapper mapper,
    ILeaveTypeRepository leaveTypeRepository,
    IAppLogger<CreateLeaveTypeCommandHandler> logger)
    : IRequestHandler<CreateLeaveTypeCommand, int>
{
    public async Task<int> Handle(CreateLeaveTypeCommand command, CancellationToken cancellationToken)
    {
        // Validate incoming data
        var validator = new CreateLeaveTypeCommandValidator(leaveTypeRepository);
        var validationResult = await validator.ValidateAsync(command);

        if (validationResult.Errors.Any())
        {
            logger.LogWarning($"Invalid request: {JsonSerializer.Serialize(command)}");
            throw new BadRequestException($"Invalid LeaveType", validationResult);
        }

        // Convert to domain entity object
        var leaveTypeToCreate = mapper.Map<Domain.LeaveType>(command);

        // Add to database
        await leaveTypeRepository.CreateAsync(leaveTypeToCreate);

        // Return record id
        return leaveTypeToCreate.Id;
    }
}
