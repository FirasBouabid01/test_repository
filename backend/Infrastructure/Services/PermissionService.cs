using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class PermissionService : IPermissionService
{
    private readonly AppDbContext _context;

    public PermissionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> HasPermissionAsync(Guid userId, string permission)
    {
        var directPermissions =
            from up in _context.UserPermissions
            where up.UserId == userId
            select up.Permission.Name;

        var rolePermissions =
            from ur in _context.UserRoles
            join rp in _context.RolePermissions on ur.RoleId equals rp.RoleId
            join p in _context.Permissions on rp.PermissionId equals p.Id
            where ur.UserId == userId
            select p.Name;

        return await directPermissions
            .Union(rolePermissions)
            .AnyAsync(p => p == permission);
    }
}