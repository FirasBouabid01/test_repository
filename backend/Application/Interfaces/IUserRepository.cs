using Domain.Entities;
using Application.Common.Pagination;
using Application.Users.Dtos;

namespace Application.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByEmailWithRolesAsync(string email);
    Task<User?> GetByUsernameAsync(string username);
    Task<PaginatedResponse<UserDto>> GetPaginatedUsersAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        bool? isAdmin = null,
        string? sortBy = "username",
        bool sortDescending = false
    );
    Task<List<UserWithRolesDto>> GetUsersWithRolesAndPermissionsAsync();
}