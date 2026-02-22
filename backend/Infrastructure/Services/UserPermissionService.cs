using Domain.Entities;
using Application.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Services;

public class UserPermissionService : IUserPermissionService
{
    private readonly AppDbContext _context;

    public UserPermissionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddUserPermissionAsync(Guid userId, Guid permissionId)
    {
        var userPermission = new UserPermission
        {
            UserId = userId,
            PermissionId = permissionId
        };

        _context.UserPermissions.Add(userPermission);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveUserPermissionAsync(Guid userId, Guid permissionId)
    {
        var userPermission = await _context.UserPermissions.FindAsync(userId, permissionId);
        if (userPermission != null)
        {
            _context.UserPermissions.Remove(userPermission);
            await _context.SaveChangesAsync();
        }
    }
}
