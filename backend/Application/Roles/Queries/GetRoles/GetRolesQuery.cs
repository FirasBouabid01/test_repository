using Application.Interfaces;
using Application.Common.Pagination;
using Application.Roles.Dtos;
using MediatR;

namespace Application.Roles.Queries.GetRoles;

public record GetRolesQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    string? SortBy = "name",
    bool SortDescending = false
) : IRequest<PaginatedResponse<RoleDto>>;

public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, PaginatedResponse<RoleDto>>
{
    private readonly IRoleRepository _roleRepository;

    public GetRolesQueryHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<PaginatedResponse<RoleDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        return await _roleRepository.GetPaginatedRolesAsync(
            request.PageNumber,
            request.PageSize,
            request.SearchTerm,
            request.SortBy,
            request.SortDescending
        );
    }
}
