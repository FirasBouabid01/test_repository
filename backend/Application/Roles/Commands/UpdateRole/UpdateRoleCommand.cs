using Application.Interfaces;
using Application.Roles.Dtos;
using MediatR;
using Application.Common.Exceptions;

namespace Application.Roles.Commands.UpdateRole;

public record UpdateRoleCommand(
    Guid RoleId,
    string Name,
    IEnumerable<Guid> PermissionIds
) : IRequest<RoleDto>;

public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, RoleDto>
{
    private readonly IRoleRepository _roleRepository;

    public UpdateRoleCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<RoleDto> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdAsync(request.RoleId);
        if (role == null)
        {
            throw new NotFoundException($"Role with ID {request.RoleId} not found.");
        }

        if (role.Name != request.Name)
        {
            var existingRole = await _roleRepository.GetByNameAsync(request.Name);
            if (existingRole != null)
            {
                throw new ValidationException("Role name already exists.");
            }
        }

        role.Name = request.Name;
        await _roleRepository.UpdateAsync(role);

        return new RoleDto
        {
            Id = role.Id,
            Name = role.Name,
            Permissions = role.RolePermissions.Select(rp => rp.Permission.Name),
            UserCount = role.UserRoles.Count
        };
    }
}
