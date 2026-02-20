using Application.Interfaces;
using MediatR;
using Application.Common.Exceptions;

namespace Application.Permissions.Commands.UpdatePermission;

public record UpdatePermissionCommand(Guid Id, string Name) : IRequest<Unit>;

public class UpdatePermissionCommandHandler : IRequestHandler<UpdatePermissionCommand, Unit>
{
    private readonly IPermissionRepository _permissionRepository;

    public UpdatePermissionCommandHandler(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public async Task<Unit> Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
    {
        var permission = await _permissionRepository.GetByIdAsync(request.Id);
        if (permission == null) throw new NotFoundException("Permission", request.Id);

        permission.Name = request.Name;
        await _permissionRepository.UpdateAsync(permission);

        return Unit.Value;
    }
}
