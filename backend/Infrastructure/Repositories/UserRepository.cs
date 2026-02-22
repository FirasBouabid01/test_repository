using Domain.Entities;
using Application.Interfaces;
using Application.Common.Pagination;
using Application.Users.Dtos;
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
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByEmailWithRolesAsync(string email)
    {
        return await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<PaginatedResponse<UserDto>> GetPaginatedUsersAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        bool? isAdmin = null,
        string? sortBy = "username",
        bool sortDescending = false)
    {
        var query = _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                    .ThenInclude(r => r.RolePermissions)
                        .ThenInclude(rp => rp.Permission)
            .Include(u => u.UserPermissions)
                .ThenInclude(up => up.Permission)
            .AsQueryable();

        // 🔎 Search
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.ToLower();

            query = query.Where(u =>
                u.Username.ToLower().Contains(term) ||
                u.Email.ToLower().Contains(term) ||
                u.FirstName.ToLower().Contains(term) ||
                u.LastName.ToLower().Contains(term)
            );
        }

        // 🛡 Filter Admin
        if (isAdmin.HasValue)
        {
            query = query.Where(u => u.IsAdmin == isAdmin.Value);
        }

        // 🔃 Sorting
        query = sortBy?.ToLower() switch
        {
            "email" => sortDescending ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email),
            "firstname" => sortDescending ? query.OrderByDescending(u => u.FirstName) : query.OrderBy(u => u.FirstName),
            "lastname" => sortDescending ? query.OrderByDescending(u => u.LastName) : query.OrderBy(u => u.LastName),
            "isadmin" => sortDescending ? query.OrderByDescending(u => u.IsAdmin) : query.OrderBy(u => u.IsAdmin),
            _ => sortDescending ? query.OrderByDescending(u => u.Username) : query.OrderBy(u => u.Username),
        };

        var totalCount = await query.CountAsync();

        var users = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var userDtos = users.Select(u => new UserDto
        {
            Id = u.Id,
            Username = u.Username,
            Email = u.Email,
            FirstName = u.FirstName,
            LastName = u.LastName,
            DateOfBirth = u.DateOfBirth,
            PhoneNumber = u.PhoneNumber,
            Address = u.Address,
            IsAdmin = u.IsAdmin,

            // 🎭 Roles
            Roles = u.UserRoles
                .Select(ur => ur.Role.Name)
                .Distinct()
                .ToList(),

            // 🔐 Permissions (من User + من Roles)
            Permissions = u.UserPermissions
                .Select(up => up.Permission.Name)
                .Union(
                    u.UserRoles
                        .SelectMany(ur => ur.Role.RolePermissions)
                        .Select(rp => rp.Permission.Name)
                )
                .Distinct()
                .ToList()
        }).ToList();

        return new PaginatedResponse<UserDto>(
            userDtos,
            totalCount,
            pageNumber,
            pageSize
        );
    }

    public Task<List<UserWithRolesDto>> GetUsersWithRolesAndPermissionsAsync()
    {
        throw new NotImplementedException();
    }
}