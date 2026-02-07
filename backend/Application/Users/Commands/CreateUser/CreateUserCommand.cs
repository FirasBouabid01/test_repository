using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Application.Common.Exceptions;

namespace Application.Users.Commands.CreateUser;

public record CreateUserCommand(
    string Username,
    string FirstName,
    string LastName,
    string Email,
    string Password,
    DateTime DateOfBirth,
    string PhoneNumber,
    string Address) : IRequest<Guid>;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Check for unique username
        var existingUser = await _userRepository.GetByUsernameAsync(request.Username);
        if (existingUser != null)
        {
            throw new ValidationException("Username is already taken.");
        }

        // Check for unique email
        var existingEmail = await _userRepository.GetByEmailAsync(request.Email);
        if (existingEmail != null)
        {
            throw new ValidationException("Email is already registered.");
        }

        // Hash the password using BCrypt
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        // Convert DateOfBirth to UTC for PostgreSQL compatibility
        var dateOfBirthUtc = DateTime.SpecifyKind(request.DateOfBirth, DateTimeKind.Utc);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = passwordHash,
            DateOfBirth = dateOfBirthUtc,
            PhoneNumber = request.PhoneNumber,
            Address = request.Address
        };

        await _userRepository.AddAsync(user);

        return user.Id;
    }
}
