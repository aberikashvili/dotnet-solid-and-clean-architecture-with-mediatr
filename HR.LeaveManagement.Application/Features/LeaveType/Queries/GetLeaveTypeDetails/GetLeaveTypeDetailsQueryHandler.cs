using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveType.Queries.GetLeaveTypeDetails;

public class GetLeaveTypeDetailsQueryHandler(IMapper mapper, ILeaveTypeRepository leaveTypeRepository) : IRequestHandler<GetLeaveTypeDetailsQuery, LeaveTypeDetailsDto>
{
    public async Task<LeaveTypeDetailsDto> Handle(GetLeaveTypeDetailsQuery request, CancellationToken cancellationToken)
    {
        // Query the database
        var leaveType = await leaveTypeRepository.GetByIdAsync(request.id);

        if (leaveType == null)
        {
            throw new NotFoundException(nameof(LeaveType), request.id);
        }

        // Convert data object to DTO object
        var data = mapper.Map<LeaveTypeDetailsDto>(leaveType);

        // Return DTO Object
        return data;
    }
}
