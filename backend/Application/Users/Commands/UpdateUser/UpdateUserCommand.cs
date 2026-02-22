using Application.Interfaces;
using Application.Users.Dtos;
using MediatR;
using Application.Common.Exceptions;

namespace Application.Users.Commands.UpdateUser;

public record UpdateUserCommand(
    Guid UserId,
    string FirstName,
    string LastName,
    string Email,
    DateTime DateOfBirth,
    string PhoneNumber,
    string Address,
    bool IsAdmin
) : IRequest<UserDto>;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
        {
            throw new NotFoundException($"User with ID {request.UserId} not found.");
        }

        // Check if new email is unique
        if (user.Email != request.Email)
        {
            var existingEmail = await _userRepository.GetByEmailAsync(request.Email);
            if (existingEmail != null)
            {
                throw new ValidationException("Email is already registered.");
            }
        }

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Email = request.Email;
        user.DateOfBirth = DateTime.SpecifyKind(request.DateOfBirth, DateTimeKind.Utc);
        user.PhoneNumber = request.PhoneNumber;
        user.Address = request.Address;
        user.IsAdmin = request.IsAdmin;

        await _userRepository.UpdateAsync(user);

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
