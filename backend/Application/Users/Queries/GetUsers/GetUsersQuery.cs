using Application.Interfaces;
using Application.Users.Dtos;
using MediatR;

namespace Application.Users.Queries.GetUsers;

public record GetUsersQuery : IRequest<IEnumerable<UserManagementDto>>;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserManagementDto>>
{
    private readonly IUserRepository _userRepository;

    public GetUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserManagementDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllWithRolesAndPermissionsAsync();

        return users.Select(u => new UserManagementDto
        {
            Id = u.Id,
            Username = u.Username,
            Email = u.Email,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Roles = u.UserRoles.Select(ur => ur.Role.Name).ToList(),
            Permissions = u.UserPermissions.Select(up => up.Permission.Name).ToList()
        });
    }
}
