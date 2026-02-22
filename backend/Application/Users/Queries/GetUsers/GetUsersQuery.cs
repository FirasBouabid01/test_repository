using Application.Interfaces;
using Application.Common.Pagination;
using Application.Users.Dtos;
using MediatR;

namespace Application.Users.Queries.GetUsers;

public record GetUsersQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsAdmin = null,
    string? SortBy = "username",
    bool SortDescending = false
) : IRequest<PaginatedResponse<UserDto>>;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, PaginatedResponse<UserDto>>
{
    private readonly IUserRepository _userRepository;

    public GetUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<PaginatedResponse<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return await _userRepository.GetPaginatedUsersAsync(
            request.PageNumber,
            request.PageSize,
            request.SearchTerm,
            request.IsAdmin,
            request.SortBy,
            request.SortDescending
        );
    }
}
