using Domain.Entities;
using MediatR;
using Application.Interfaces;

namespace Application.Roles.Queries.GetAllRoles;

public record GetAllRolesQuery() : IRequest<IEnumerable<Role>>;

public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, IEnumerable<Role>>
{
    private readonly IRoleRepository _roleRepository;

    public GetAllRolesQueryHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<IEnumerable<Role>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        return await _roleRepository.GetAllAsync();
    }
}
