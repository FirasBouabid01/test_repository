using Domain.Entities;

namespace Application.Interfaces;

public interface IPermissionRepository : IGenericRepository<Permission>
{
    Task<Permission?> GetByNameAsync(string name);
    Task<bool> HasAssociationsAsync(Guid permissionId);
}
