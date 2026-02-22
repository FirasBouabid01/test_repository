using MediatR;
using Application.Common.Exceptions;
using Application.Interfaces;

namespace Application.RolePermissions.Commands.RemovePermissionFromRole;

public record RemovePermissionFromRoleCommand(Guid RoleId, Guid PermissionId) : IRequest<Unit>;

public class RemovePermissionFromRoleCommandHandler : IRequestHandler<RemovePermissionFromRoleCommand, Unit>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly IRolePermissionService _rolePermissionService;

    public RemovePermissionFromRoleCommandHandler(
        IRoleRepository roleRepository,
        IPermissionRepository permissionRepository,
        IRolePermissionService rolePermissionService)
    {
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
        _rolePermissionService = rolePermissionService;
    }

    public async Task<Unit> Handle(RemovePermissionFromRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdAsync(request.RoleId);
        if (role == null)
        {
            throw new NotFoundException($"Role with ID {request.RoleId} not found.");
        }

        var permission = await _permissionRepository.GetByIdAsync(request.PermissionId);
        if (permission == null)
        {
            throw new NotFoundException($"Permission with ID {request.PermissionId} not found.");
        }

        await _rolePermissionService.RemoveRolePermissionAsync(request.RoleId, request.PermissionId);

        return Unit.Value;
    }
}
