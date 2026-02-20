using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Roles.Commands.CreateRole;

public record CreateRoleCommand(string Name) : IRequest<Guid>;

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Guid>
{
    private readonly IRoleRepository _roleRepository;

    public CreateRoleCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<Guid> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = request.Name
        };

        await _roleRepository.AddAsync(role);
        return role.Id;
    }
}
