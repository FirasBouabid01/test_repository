using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RoleRepository : GenericRepository<Role>, IRoleRepository
{
    public RoleRepository(AppDbContext context) : base(context) { }

    public async Task<bool> HasUsersAsync(Guid roleId)
    {
        return await _context.UserRoles.AnyAsync(ur => ur.RoleId == roleId);
    }

    public async Task<Role?> GetByNameAsync(string name)
    {
        return await _dbSet.FirstOrDefaultAsync(r => r.Name == name);
    }

    public async Task<Role?> GetByIdWithPermissionsAsync(Guid id)
    {
        return await _dbSet
            .Include(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task UpdatePermissionsAsync(Guid roleId, IEnumerable<Guid> permissionIds)
    {
        var currentPermissions = await _context.RolePermissions
            .Where(rp => rp.RoleId == roleId)
            .ToListAsync();

        _context.RolePermissions.RemoveRange(currentPermissions);

        foreach (var pId in permissionIds)
        {
            _context.RolePermissions.Add(new RolePermission
            {
                RoleId = roleId,
                PermissionId = pId
            });
        }

        await _context.SaveChangesAsync();
    }
}
