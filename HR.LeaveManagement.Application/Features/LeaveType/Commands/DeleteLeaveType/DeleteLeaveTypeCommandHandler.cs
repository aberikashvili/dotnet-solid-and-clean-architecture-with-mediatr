using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveType.Commands.DeleteLeaveType;

public class DeleteLeaveTypeCommandHandler(ILeaveTypeRepository leaveTypeRepository)
    : IRequestHandler<DeleteLeaveTypeCommand, Unit>
{
    public async Task<Unit> Handle(DeleteLeaveTypeCommand request, CancellationToken cancellationToken)
    {
        // Retrieve domain entity object
        var leaveTypeToDelete = await leaveTypeRepository.GetByIdAsync(request.Id);
        
        // Verify that record exists
        if (leaveTypeToDelete == null)
        {
            throw new NotFoundException(nameof(Domain.LeaveType), request.Id);
        }
        
        // Remove from database
        await leaveTypeRepository.DeleteAsync(leaveTypeToDelete);

        // Return record id
        return Unit.Value;
    }
}
