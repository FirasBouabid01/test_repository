using Application.Interfaces;
using MediatR;
using Application.Common.Exceptions;

namespace Application.Roles.Commands.DeleteRole;

public record DeleteRoleCommand(Guid Id) : IRequest<Unit>;

public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, Unit>
{
    private readonly IRoleRepository _roleRepository;

    public DeleteRoleCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<Unit> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdAsync(request.Id);
        if (role == null) throw new NotFoundException("Role", request.Id);

        // ✅ Delete Guard: check if role has users
        if (await _roleRepository.HasUsersAsync(request.Id))
        {
            throw new ValidationException("Cannot delete role because it is still assigned to users.");
        }

        _roleRepository.Delete(role);
        await _roleRepository.SaveChangesAsync();

        return Unit.Value;
    }
}
