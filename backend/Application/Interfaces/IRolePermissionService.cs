using Domain.Entities;

namespace Application.Interfaces;

public interface IRolePermissionService
{
    Task AddRolePermissionAsync(Guid roleId, Guid permissionId);
    Task RemoveRolePermissionAsync(Guid roleId, Guid permissionId);
}
