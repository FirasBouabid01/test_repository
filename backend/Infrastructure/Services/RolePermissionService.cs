using Domain.Entities;
using Application.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Services;

public class RolePermissionService : IRolePermissionService
{
    private readonly AppDbContext _context;

    public RolePermissionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddRolePermissionAsync(Guid roleId, Guid permissionId)
    {
        var rolePermission = new RolePermission
        {
            RoleId = roleId,
            PermissionId = permissionId
        };

        _context.RolePermissions.Add(rolePermission);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveRolePermissionAsync(Guid roleId, Guid permissionId)
    {
        var rolePermission = await _context.RolePermissions.FindAsync(roleId, permissionId);
        if (rolePermission != null)
        {
            _context.RolePermissions.Remove(rolePermission);
            await _context.SaveChangesAsync();
        }
    }
}
