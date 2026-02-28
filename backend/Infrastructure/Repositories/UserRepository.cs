using Domain.Entities;
using Application.Interfaces;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
    }

    // ✅ الصحيح
    public async Task<User?> GetByEmailWithRolesAsync(string email)
    {
        return await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByIdWithRolesAsync(Guid id)
    {
        return await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<List<User>> GetAllWithRolesAndPermissionsAsync()
    {
        return await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Include(u => u.UserPermissions)
                .ThenInclude(up => up.Permission)
            .ToListAsync();
    }

    public async Task UpdateRolesAndPermissionsAsync(Guid userId, List<string> roleNames, List<string> permissionNames)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles)
            .Include(u => u.UserPermissions)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null) throw new Exception("User not found");

        // Roles
        var roles = await _context.Roles.Where(r => roleNames.Contains(r.Name)).ToListAsync();
        user.UserRoles.Clear();
        foreach (var role in roles)
        {
            user.UserRoles.Add(new UserRole { UserId = userId, RoleId = role.Id });
        }

        // Permissions
        var permissions = await _context.Permissions.Where(p => permissionNames.Contains(p.Name)).ToListAsync();
        user.UserPermissions.Clear();
        foreach (var p in permissions)
        {
            user.UserPermissions.Add(new UserPermission { UserId = userId, PermissionId = p.Id });
        }

        await _context.SaveChangesAsync();
    }
}
