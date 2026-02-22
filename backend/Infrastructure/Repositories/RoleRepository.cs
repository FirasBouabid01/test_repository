using Application.Common.Pagination;
using Application.Interfaces;
using Application.Roles.Dtos;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _context;

    public RoleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Role?> GetByIdAsync(Guid roleId)
    {
        return await _context.Roles
            .FirstOrDefaultAsync(r => r.Id == roleId);
    }

    public async Task<Role?> GetByNameAsync(string name)
    {
        return await _context.Roles
            .FirstOrDefaultAsync(r => r.Name == name);
    }

    public async Task AddAsync(Role role)
    {
        await _context.Roles.AddAsync(role);
    }

    public async Task<List<Permission>> GetPermissionsByIdsAsync(IEnumerable<Guid> ids)
    {
        return await _context.Permissions
            .Where(p => ids.Contains(p.Id))
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    // ⭐⭐ أهم ميثود ⭐⭐
    public async Task<PaginatedResponse<RoleDto>> GetPaginatedRolesAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm,
        string? sortBy,
        bool sortDescending)
    {
        var query = _context.Roles
            .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
            .Include(r => r.UserRoles)
            .AsQueryable();

        // 🔍 Search
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(r => r.Name.Contains(searchTerm));
        }

        // 🔃 Sorting
        query = sortBy?.ToLower() switch
        {
            "name" => sortDescending
                ? query.OrderByDescending(r => r.Name)
                : query.OrderBy(r => r.Name),

            _ => query.OrderBy(r => r.Name)
        };

        var totalCount = await query.CountAsync();

        var roles = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var items = roles.Select(role => new RoleDto
        {
            Id = role.Id,
            Name = role.Name,
            Permissions = role.RolePermissions
                .Select(rp => rp.Permission.Name)
                .ToList(),
            UserCount = role.UserRoles.Count
        }).ToList();

        return new PaginatedResponse<RoleDto>(
            items,
            totalCount,
            pageNumber,
            pageSize
        );
    }

    public Task<Role?> GetByIdWithPermissionsAsync(Guid roleId)
    {
        throw new NotImplementedException();
    }

    public Task<Role?> GetByIdWithUsersAsync(Guid roleId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Role role)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Role role)
    {
        throw new NotImplementedException();
    }
}