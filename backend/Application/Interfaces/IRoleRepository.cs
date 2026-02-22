using Application.Common.Pagination;
using Application.Roles.Dtos;
using Domain.Entities;

namespace Application.Interfaces;

public interface IRoleRepository
{
    Task<PaginatedResponse<RoleDto>> GetPaginatedRolesAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm,
        string? sortBy,
        bool sortDescending
    );

    Task<Role?> GetByIdAsync(Guid roleId);
    Task<Role?> GetByIdWithPermissionsAsync(Guid roleId);
    Task<Role?> GetByIdWithUsersAsync(Guid roleId);
    Task<Role?> GetByNameAsync(string name);

    Task AddAsync(Role role);
    Task UpdateAsync(Role role);
    Task DeleteAsync(Role role);

    Task<List<Permission>> GetPermissionsByIdsAsync(IEnumerable<Guid> ids);

    Task SaveChangesAsync();
}