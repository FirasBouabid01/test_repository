using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Users.Queries.GetUsers;

public record GetUsersQuery : IRequest<IEnumerable<User>>;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<User>>
{
    private readonly IUserRepository _userRepository;

    public GetUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<User>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return await _userRepository.GetAllAsync();
    }
}
