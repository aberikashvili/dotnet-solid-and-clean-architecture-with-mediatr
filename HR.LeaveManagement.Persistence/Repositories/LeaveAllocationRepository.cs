using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Domain;
using HR.LeaveManagement.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Persistence.Repositories;

public class LeaveAllocationRepository(HrDatabaseContext context)
    : GenericRepository<LeaveAllocation>(context), ILeaveAllocationRepository
{
    public async Task<LeaveAllocation> GetLeaveAllocationWithDetails(int id)
    {
        var leaveAllocations = await _context.LeaveAllocations
            .Include(x => x.LeaveType)
            .FirstOrDefaultAsync(x => x.Id == id);
        return leaveAllocations;
    }

    public async Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails()
    {
        var leaveAllocations = await _context.LeaveAllocations
            .Include(x => x.LeaveType)
            .ToListAsync();
        return leaveAllocations;
    }

    public async Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails(string userId)
    {
        var leaveAllocations = await _context.LeaveAllocations
            .Where(x => x.EmployeeId == userId)
            .Include(x => x.LeaveType)
            .ToListAsync();
        return leaveAllocations;
    }

    public async Task<bool> AllocationExists(string userId, int leaveTypeId, int period)
    {
        return await _context.LeaveAllocations
            .AnyAsync(x 
                => x.EmployeeId == userId 
                   && x.LeaveTypeId == leaveTypeId 
                   && x.Period == period);
    }

    public async Task AddAllocations(List<LeaveAllocation> allocations)
    {
        await _context.AddRangeAsync(allocations);
        await _context.SaveChangesAsync();
    }

    public async Task<LeaveAllocation> GetUserAllocations(string userId, int leaveTypeId)
    {
        var leaveAllocations = await _context.LeaveAllocations
            // .Include(x => x.LeaveType)
            .FirstOrDefaultAsync(x => x.EmployeeId == userId && x.LeaveTypeId == leaveTypeId);
        return leaveAllocations;
    }
}