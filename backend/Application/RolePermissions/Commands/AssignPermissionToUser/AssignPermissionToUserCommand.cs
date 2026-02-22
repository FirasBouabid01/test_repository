using MediatR;
using Application.Common.Exceptions;
using Application.Interfaces;

namespace Application.RolePermissions.Commands.AssignPermissionToUser;

public record AssignPermissionToUserCommand(Guid UserId, Guid PermissionId) : IRequest<Unit>;

public class AssignPermissionToUserCommandHandler : IRequestHandler<AssignPermissionToUserCommand, Unit>
{
    private readonly IUserRepository _userRepository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly IUserPermissionService _userPermissionService;

    public AssignPermissionToUserCommandHandler(
        IUserRepository userRepository,
        IPermissionRepository permissionRepository,
        IUserPermissionService userPermissionService)
    {
        _userRepository = userRepository;
        _permissionRepository = permissionRepository;
        _userPermissionService = userPermissionService;
    }

    public async Task<Unit> Handle(AssignPermissionToUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
        {
            throw new NotFoundException($"User with ID {request.UserId} not found.");
        }

        var permission = await _permissionRepository.GetByIdAsync(request.PermissionId);
        if (permission == null)
        {
            throw new NotFoundException($"Permission with ID {request.PermissionId} not found.");
        }

        await _userPermissionService.AddUserPermissionAsync(request.UserId, request.PermissionId);

        return Unit.Value;
    }
}
