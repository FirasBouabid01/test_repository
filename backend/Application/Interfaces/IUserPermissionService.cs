using Domain.Entities;

namespace Application.Interfaces;

public interface IUserPermissionService
{
    Task AddUserPermissionAsync(Guid userId, Guid permissionId);
    Task RemoveUserPermissionAsync(Guid userId, Guid permissionId);
}
