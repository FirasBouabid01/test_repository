using Application.Interfaces;
using MediatR;
using Application.Common.Exceptions;

namespace Application.Roles.Commands.UpdateRole;

public record UpdateRoleCommand(Guid id, string Name) : IRequest<Unit>;

public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, Unit>
{
    private readonly IRoleRepository _roleRepository;

    public UpdateRoleCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<Unit> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdAsync(request.id);
        if (role == null) throw new NotFoundException("Role", request.id);

        role.Name = request.Name;
        await _roleRepository.UpdateAsync(role);

        return Unit.Value;
    }
}
