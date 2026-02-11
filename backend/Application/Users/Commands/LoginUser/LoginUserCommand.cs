using Application.Common.Exceptions;
using Application.Interfaces;
using MediatR;

namespace Application.Users.Commands.LoginUser;

public record LoginUserCommand(string Email, string Password) : IRequest<string>;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, string>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginUserCommandHandler(
        IUserRepository userRepository,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        // ✅ لازم user مع roles
        var user = await _userRepository.GetByEmailWithRolesAsync(request.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new ValidationException("Invalid email or password.");
        }

        // ✅ استخراج role
        var roleName = user.UserRoles
            .Select(ur => ur.Role.Name)
            .FirstOrDefault() ?? "User";

        // ✅ توليد JWT فيه role
       return _jwtTokenGenerator.GenerateToken(user);

    }
}