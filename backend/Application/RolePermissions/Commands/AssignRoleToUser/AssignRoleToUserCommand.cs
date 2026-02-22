using MediatR;
using Application.Common.Exceptions;
using Application.Interfaces;

namespace Application.RolePermissions.Commands.AssignRoleToUser;

public record AssignRoleToUserCommand(Guid UserId, Guid RoleId) : IRequest<Unit>;

public class AssignRoleToUserCommandHandler : IRequestHandler<AssignRoleToUserCommand, Unit>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRoleService _userRoleService;

    public AssignRoleToUserCommandHandler(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IUserRoleService userRoleService)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _userRoleService = userRoleService;
    }

    public async Task<Unit> Handle(AssignRoleToUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
        {
            throw new NotFoundException($"User with ID {request.UserId} not found.");
        }

        var role = await _roleRepository.GetByIdAsync(request.RoleId);
        if (role == null)
        {
            throw new NotFoundException($"Role with ID {request.RoleId} not found.");
        }

        await _userRoleService.AddUserRoleAsync(request.UserId, request.RoleId);

        return Unit.Value;
    }
}
