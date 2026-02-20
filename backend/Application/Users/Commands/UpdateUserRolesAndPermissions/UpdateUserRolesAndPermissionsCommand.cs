using Application.Interfaces;
using MediatR;
using System.Collections.Generic;

namespace Application.Users.Commands.UpdateUserRolesAndPermissions;

public record UpdateUserRolesAndPermissionsCommand(
    Guid UserId,
    List<string> Roles,
    List<string> Permissions) : IRequest;

public class UpdateUserRolesAndPermissionsCommandHandler : IRequestHandler<UpdateUserRolesAndPermissionsCommand>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserRolesAndPermissionsCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Handle(UpdateUserRolesAndPermissionsCommand request, CancellationToken cancellationToken)
    {
        await _userRepository.UpdateRolesAndPermissionsAsync(request.UserId, request.Roles, request.Permissions);
    }
}
