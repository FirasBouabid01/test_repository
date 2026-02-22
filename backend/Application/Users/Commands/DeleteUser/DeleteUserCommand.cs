using Application.Interfaces;
using MediatR;
using Application.Common.Exceptions;

namespace Application.Users.Commands.DeleteUser;

public record DeleteUserCommand(Guid UserId) : IRequest<Unit>;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
        {
            throw new NotFoundException($"User with ID {request.UserId} not found.");
        }

        await _userRepository.DeleteAsync(user);

        return Unit.Value;
    }
}
