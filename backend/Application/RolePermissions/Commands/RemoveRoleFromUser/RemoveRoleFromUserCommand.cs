using MediatR;
using Application.Common.Exceptions;
using Application.Interfaces;

namespace Application.RolePermissions.Commands.RemoveRoleFromUser;

public record RemoveRoleFromUserCommand(Guid UserId, Guid RoleId) : IRequest<Unit>;

public class RemoveRoleFromUserCommandHandler : IRequestHandler<RemoveRoleFromUserCommand, Unit>
{
    private readonly IUserRoleService _userRoleService;

    public RemoveRoleFromUserCommandHandler(IUserRoleService userRoleService)
    {
        _userRoleService = userRoleService;
    }

    public async Task<Unit> Handle(RemoveRoleFromUserCommand request, CancellationToken cancellationToken)
    {
        await _userRoleService.RemoveUserRoleAsync(request.UserId, request.RoleId);
        return Unit.Value;
    }
}
