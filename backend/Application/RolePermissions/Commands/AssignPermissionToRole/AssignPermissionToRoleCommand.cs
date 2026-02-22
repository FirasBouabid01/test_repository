using MediatR;
using Application.Common.Exceptions;
using Application.Interfaces;

namespace Application.RolePermissions.Commands.AssignPermissionToRole;

public record AssignPermissionToRoleCommand(Guid RoleId, Guid PermissionId) : IRequest<Unit>;

public class AssignPermissionToRoleCommandHandler : IRequestHandler<AssignPermissionToRoleCommand, Unit>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly IRolePermissionService _rolePermissionService;

    public AssignPermissionToRoleCommandHandler(
        IRoleRepository roleRepository,
        IPermissionRepository permissionRepository,
        IRolePermissionService rolePermissionService)
    {
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
        _rolePermissionService = rolePermissionService;
    }

    public async Task<Unit> Handle(AssignPermissionToRoleCommand request, CancellationToken cancellationToken)
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

        await _rolePermissionService.AddRolePermissionAsync(request.RoleId, request.PermissionId);

        return Unit.Value;
    }
}
