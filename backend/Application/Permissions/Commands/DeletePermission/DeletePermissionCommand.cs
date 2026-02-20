using Application.Interfaces;
using MediatR;
using Application.Common.Exceptions;

namespace Application.Permissions.Commands.DeletePermission;

public record DeletePermissionCommand(Guid Id) : IRequest<Unit>;

public class DeletePermissionCommandHandler : IRequestHandler<DeletePermissionCommand, Unit>
{
    private readonly IPermissionRepository _permissionRepository;

    public DeletePermissionCommandHandler(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public async Task<Unit> Handle(DeletePermissionCommand request, CancellationToken cancellationToken)
    {
        var permission = await _permissionRepository.GetByIdAsync(request.Id);
        if (permission == null) throw new NotFoundException("Permission", request.Id);

        if (await _permissionRepository.HasAssociationsAsync(request.Id))
        {
            throw new ValidationException("Cannot delete permission because it is assigned to roles or users.");
        }

        _permissionRepository.Delete(permission);
        await _permissionRepository.SaveChangesAsync();

        return Unit.Value;
    }
}
