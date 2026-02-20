using Domain.Entities;
using MediatR;
using Application.Interfaces;
using Application.Common.Exceptions;

namespace Application.Roles.Queries.GetRoleById;

public record GetRoleByIdQuery(Guid Id) : IRequest<Role>;

public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, Role>
{
    private readonly IRoleRepository _roleRepository;

    public GetRoleByIdQueryHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<Role> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdWithPermissionsAsync(request.Id);

        if (role == null) throw new NotFoundException("Role", request.Id);
        return role;
    }
}
