using Domain.Entities;

namespace Application.Interfaces;

public interface IRoleRepository : IGenericRepository<Role>
{
    Task<bool> HasUsersAsync(Guid roleId);
    Task<Role?> GetByNameAsync(string name);
    Task<Role?> GetByIdWithPermissionsAsync(Guid id);
    Task UpdatePermissionsAsync(Guid roleId, IEnumerable<Guid> permissionIds);
}
