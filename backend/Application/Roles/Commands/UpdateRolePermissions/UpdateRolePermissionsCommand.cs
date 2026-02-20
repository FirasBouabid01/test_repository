using Application.Interfaces;
using MediatR;
using Application.Common.Exceptions;

namespace Application.Roles.Commands.UpdateRolePermissions;

public record UpdateRolePermissionsCommand(Guid RoleId, List<Guid> PermissionIds) : IRequest<Unit>;

public class UpdateRolePermissionsCommandHandler : IRequestHandler<UpdateRolePermissionsCommand, Unit>
{
    private readonly IRoleRepository _roleRepository;

    public UpdateRolePermissionsCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<Unit> Handle(UpdateRolePermissionsCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdAsync(request.RoleId);
        if (role == null) throw new NotFoundException("Role", request.RoleId);

        await _roleRepository.UpdatePermissionsAsync(request.RoleId, request.PermissionIds);

        return Unit.Value;
    }
}
