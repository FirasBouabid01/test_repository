using Application.Interfaces;
using Application.Common.Pagination;
using Application.Permissions.Dtos;
using MediatR;

namespace Application.Permissions.Queries.GetPermissions;

public record GetPermissionsQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    string? SortBy = "name",
    bool SortDescending = false
) : IRequest<PaginatedResponse<PermissionDto>>;

public class GetPermissionsQueryHandler : IRequestHandler<GetPermissionsQuery, PaginatedResponse<PermissionDto>>
{
    private readonly IPermissionRepository _permissionRepository;

    public GetPermissionsQueryHandler(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public async Task<PaginatedResponse<PermissionDto>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
    {
        return await _permissionRepository.GetPaginatedPermissionsAsync(
            request.PageNumber,
            request.PageSize,
            request.SearchTerm,
            request.SortBy,
            request.SortDescending
        );
    }
}
