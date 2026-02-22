using Domain.Entities;
using Application.Common.Pagination;
using Application.Permissions.Dtos;

namespace Application.Interfaces;

public interface IPermissionRepository : IGenericRepository<Permission>
{
    Task<Permission?> GetByNameAsync(string name);
    Task<IEnumerable<Permission>> GetAllWithDetailsAsync();
    Task<PaginatedResponse<PermissionDto>> GetPaginatedPermissionsAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        string? sortBy = "name",
        bool sortDescending = false
    );
}
