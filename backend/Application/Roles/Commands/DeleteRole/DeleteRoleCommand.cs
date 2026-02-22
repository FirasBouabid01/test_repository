using Application.Interfaces;
using MediatR;
using Application.Common.Exceptions;

namespace Application.Roles.Commands.DeleteRole;

public record DeleteRoleCommand(Guid RoleId) : IRequest<Unit>;

public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, Unit>
{
    private readonly IRoleRepository _roleRepository;

    public DeleteRoleCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<Unit> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdWithUsersAsync(request.RoleId);
        if (role == null)
        {
            throw new NotFoundException($"Role with ID {request.RoleId} not found.");
        }

        if (role.UserRoles.Any())
        {
            throw new ValidationException("Cannot delete role that has assigned users. Please unassign all users first.");
        }

        await _roleRepository.DeleteAsync(role);

        return Unit.Value;
    }
}
