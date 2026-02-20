using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Permissions.Commands.CreatePermission;

public record CreatePermissionCommand(string Name) : IRequest<Guid>;

public class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, Guid>
{
    private readonly IPermissionRepository _permissionRepository;

    public CreatePermissionCommandHandler(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public async Task<Guid> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
    {
        var permission = new Permission
        {
            Id = Guid.NewGuid(),
            Name = request.Name
        };

        await _permissionRepository.AddAsync(permission);
        return permission.Id;
    }
}
