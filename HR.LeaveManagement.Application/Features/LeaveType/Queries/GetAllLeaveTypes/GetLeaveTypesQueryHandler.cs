using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes;

public class GetLeaveTypesQueryHandler(
    IMapper mapper,
    ILeaveTypeRepository leaveTypeRepository,
    IAppLogger<GetLeaveTypesQueryHandler> logger)
    : IRequestHandler<GetLeaveTypesQuery, List<LeaveTypeDto>>
{
    public async Task<List<LeaveTypeDto>> Handle(GetLeaveTypesQuery request, CancellationToken cancellationToken)
    {
        // Query the database
        var leaveTypes = await leaveTypeRepository.GetAsync();

        // Convert database objects to DTO objects
        var data = mapper.Map<List<LeaveTypeDto>>(leaveTypes);
        
        // Log
        logger.LogInformation($"List of leave types");

        // Return ist of DTO objects
        return data;
    }
}
