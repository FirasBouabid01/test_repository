using Application.Interfaces;
using Application.Roles.Dtos;
using MediatR;
using Application.Common.Exceptions;
using Domain.Entities;

namespace Application.Roles.Commands.CreateRole;

public record CreateRoleCommand(
    string Name,
    IEnumerable<Guid> PermissionIds
) : IRequest<RoleDto>;

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, RoleDto>
{
    private readonly IRoleRepository _roleRepository;

    public CreateRoleCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<RoleDto> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        // 1️⃣ Check if role exists
        var existingRole = await _roleRepository.GetByNameAsync(request.Name);
        if (existingRole != null)
            throw new ValidationException("Role name already exists.");

        // 2️⃣ Get permissions from DB
        var permissions = await _roleRepository
            .GetPermissionsByIdsAsync(request.PermissionIds);

        if (permissions.Count != request.PermissionIds.Count())
            throw new ValidationException("One or more permissions are invalid.");

        // 3️⃣ Create role
        var roleId = Guid.NewGuid();

        var role = new Role
        {
            Id = roleId,
            Name = request.Name,
            RolePermissions = permissions.Select(p => new RolePermission
            {
                RoleId = roleId,
                PermissionId = p.Id
            }).ToList()
        };

        // 4️⃣ Save
        await _roleRepository.AddAsync(role);
        await _roleRepository.SaveChangesAsync();

        // 5️⃣ Return DTO
        return new RoleDto
        {
            Id = role.Id,
            Name = role.Name,
            Permissions = permissions.Select(p => p.Name).ToList(),
            UserCount = 0
        };
    }
}