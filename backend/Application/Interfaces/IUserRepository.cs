using Domain.Entities;
using Domain.Interfaces;

namespace Application.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByEmailWithRolesAsync(string email);
    Task<User?> GetByIdWithRolesAsync(Guid id);
    Task<User?> GetByUsernameAsync(string username);
    Task<List<User>> GetAllWithRolesAndPermissionsAsync();
    Task UpdateRolesAndPermissionsAsync(Guid userId, List<string> roleNames, List<string> permissionNames);
}