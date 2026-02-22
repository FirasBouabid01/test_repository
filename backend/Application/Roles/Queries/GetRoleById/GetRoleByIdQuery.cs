using Application.Interfaces;
using Application.Roles.Dtos;
using MediatR;
using Application.Common.Exceptions;

namespace Application.Roles.Queries.GetRoleById;

public record GetRoleByIdQuery(Guid RoleId) : IRequest<RoleDto>;

public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, RoleDto>
{
    private readonly IRoleRepository _roleRepository;

    public GetRoleByIdQueryHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<RoleDto> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdWithPermissionsAsync(request.RoleId);
        if (role == null)
        {
            throw new NotFoundException($"Role with ID {request.RoleId} not found.");
        }

        return new RoleDto
        {
            Id = role.Id,
            Name = role.Name,
            Permissions = role.RolePermissions.Select(rp => rp.Permission.Name),
            UserCount = role.UserRoles.Count
        };
    }
}
