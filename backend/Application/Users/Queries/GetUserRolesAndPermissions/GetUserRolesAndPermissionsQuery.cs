using Application.Interfaces;
using Application.Users.Dtos;
using MediatR;

namespace Application.Users.Queries.GetUserRolesAndPermissions;

public record GetUserRolesAndPermissionsQuery(Guid UserId) : IRequest<UserRolesPermissionsDto>;

public class GetUserRolesAndPermissionsQueryHandler : IRequestHandler<GetUserRolesAndPermissionsQuery, UserRolesPermissionsDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IPermissionService _permissionService;

    public GetUserRolesAndPermissionsQueryHandler(IUserRepository userRepository, IPermissionService permissionService)
    {
        _userRepository = userRepository;
        _permissionService = permissionService;
    }

    public async Task<UserRolesPermissionsDto> Handle(GetUserRolesAndPermissionsQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdWithRolesAsync(request.UserId);
        
        var roles = user?.UserRoles?.Select(ur => ur.Role.Name).ToList() ?? new List<string>();

        var permissions = await _permissionService.GetUserPermissionsAsync(request.UserId);

        return new UserRolesPermissionsDto
        {
            Roles = roles,
            Permissions = permissions
        };
    }
}
