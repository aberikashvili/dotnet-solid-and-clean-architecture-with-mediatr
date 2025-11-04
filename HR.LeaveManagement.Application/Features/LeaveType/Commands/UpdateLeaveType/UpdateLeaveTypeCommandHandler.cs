using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using MediatR;
using Microsoft.IdentityModel.Tokens.Experimental;

namespace HR.LeaveManagement.Application.Features.LeaveType.Commands.UpdateLeaveType;

public class UpdateLeaveTypeCommandHandler(
    IMapper mapper,
    ILeaveTypeRepository leaveTypeRepository,
    IAppLogger<UpdateLeaveTypeCommandHandler> logger)
    : IRequestHandler<UpdateLeaveTypeCommand, Unit>
{
    public async Task<Unit> Handle(UpdateLeaveTypeCommand request, CancellationToken cancellationToken)
    {
        // Validate incoming data
        var validator = new UpdateLeaveTypeCommandValidator(leaveTypeRepository);
        var validationResult = await validator.ValidateAsync(request);

        if (validationResult.Errors.Any())
        {
            logger.LogWarning("Validation errors in update request for {0} - {1}", nameof(Domain.LeaveType),
                request.Id);
        }

        // Convert to domain entity object
        var leaveTypeToUpdate = mapper.Map<Domain.LeaveType>(request);

        // Add to database
        await leaveTypeRepository.UpdateAsync(leaveTypeToUpdate);

        // Return record id
        return Unit.Value;
    }
}
