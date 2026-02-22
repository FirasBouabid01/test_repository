using MediatR;
using Application.Common.Exceptions;
using Application.Interfaces;

namespace Application.RolePermissions.Commands.RemovePermissionFromUser;

public record RemovePermissionFromUserCommand(Guid UserId, Guid PermissionId) : IRequest<Unit>;

public class RemovePermissionFromUserCommandHandler : IRequestHandler<RemovePermissionFromUserCommand, Unit>
{
    private readonly IUserPermissionService _userPermissionService;

    public RemovePermissionFromUserCommandHandler(IUserPermissionService userPermissionService)
    {
        _userPermissionService = userPermissionService;
    }

    public async Task<Unit> Handle(RemovePermissionFromUserCommand request, CancellationToken cancellationToken)
    {
        await _userPermissionService.RemoveUserPermissionAsync(request.UserId, request.PermissionId);
        return Unit.Value;
    }
}
