using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PermissionRepository : GenericRepository<Permission>, IPermissionRepository
{
    public PermissionRepository(AppDbContext context) : base(context) { }

    public async Task<Permission?> GetByNameAsync(string name)
    {
        return await _dbSet.FirstOrDefaultAsync(p => p.Name == name);
    }

    public async Task<bool> HasAssociationsAsync(Guid permissionId)
    {
        var hasRoles = await _context.RolePermissions.AnyAsync(rp => rp.PermissionId == permissionId);
        var hasUsers = await _context.UserPermissions.AnyAsync(up => up.PermissionId == permissionId);
        return hasRoles || hasUsers;
    }
}
