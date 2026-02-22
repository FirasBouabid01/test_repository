using Application.Interfaces;
using Application.Users.Dtos;
using MediatR;
using Application.Common.Exceptions;

namespace Application.Users.Queries.GetUserById;

public record GetUserByIdQuery(Guid UserId) : IRequest<UserDto>;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
        {
            throw new NotFoundException($"User with ID {request.UserId} not found.");
        }

        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            DateOfBirth = user.DateOfBirth,
            PhoneNumber = user.PhoneNumber,
            Address = user.Address,
            IsAdmin = user.IsAdmin,
            Roles = user.UserRoles.Select(ur => ur.Role.Name),
            Permissions = user.UserPermissions.Select(up => up.Permission.Name)
        };
    }
}
