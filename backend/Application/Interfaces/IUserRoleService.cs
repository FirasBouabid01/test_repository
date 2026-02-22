using Domain.Entities;

namespace Application.Interfaces;

public interface IUserRoleService
{
    Task AddUserRoleAsync(Guid userId, Guid roleId);
    Task RemoveUserRoleAsync(Guid userId, Guid roleId);
}
