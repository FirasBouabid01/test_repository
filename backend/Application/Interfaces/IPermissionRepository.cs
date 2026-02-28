using Domain.Entities;
using Domain.Interfaces;

namespace Application.Interfaces;

public interface IPermissionRepository : IGenericRepository<Permission>
{
    Task<Permission?> GetByNameAsync(string name);
    Task<bool> HasAssociationsAsync(Guid permissionId);
}
