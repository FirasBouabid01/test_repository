using Domain.Entities;
using Application.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Services;

public class UserRoleService : IUserRoleService
{
    private readonly AppDbContext _context;

    public UserRoleService(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddUserRoleAsync(Guid userId, Guid roleId)
    {
        var userRole = new UserRole
        {
            UserId = userId,
            RoleId = roleId
        };

        _context.UserRoles.Add(userRole);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveUserRoleAsync(Guid userId, Guid roleId)
    {
        var userRole = await _context.UserRoles.FindAsync(userId, roleId);
        if (userRole != null)
        {
            _context.UserRoles.Remove(userRole);
            await _context.SaveChangesAsync();
        }
    }
}
