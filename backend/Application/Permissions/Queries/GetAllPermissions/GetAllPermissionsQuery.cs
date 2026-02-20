using Domain.Entities;
using MediatR;
using Application.Interfaces;

namespace Application.Permissions.Queries.GetAllPermissions;

public record GetAllPermissionsQuery() : IRequest<IEnumerable<Permission>>;

public class GetAllPermissionsQueryHandler : IRequestHandler<GetAllPermissionsQuery, IEnumerable<Permission>>
{
    private readonly IPermissionRepository _permissionRepository;

    public GetAllPermissionsQueryHandler(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public async Task<IEnumerable<Permission>> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
    {
        return await _permissionRepository.GetAllAsync();
    }
}
