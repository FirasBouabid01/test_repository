using Application.Permissions.Queries.GetPermissions;
using Application.Permissions.Dtos;
using Application.RolePermissions.Commands.AssignRoleToUser;
using Application.RolePermissions.Commands.RemoveRoleFromUser;
using Application.RolePermissions.Commands.AssignPermissionToUser;
using Application.RolePermissions.Commands.RemovePermissionFromUser;
using Application.RolePermissions.Commands.AssignPermissionToRole;
using Application.RolePermissions.Commands.RemovePermissionFromRole;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PermissionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PermissionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // ==================== Get Permissions (Read-only) ====================

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] string? sortBy = "name",
        [FromQuery] bool sortDescending = false)
    {
        var query = new GetPermissionsQuery(pageNumber, pageSize, searchTerm, sortBy, sortDescending);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    // ==================== Assign/Remove Roles and Permissions ====================

    [HttpPost("assign-role-to-user")]
    public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleToUserDto dto)
    {
        var command = new AssignRoleToUserCommand(dto.UserId, dto.RoleId);
        await _mediator.Send(command);
        return Ok(new { message = "Role assigned successfully." });
    }

    [HttpPost("remove-role-from-user")]
    public async Task<IActionResult> RemoveRoleFromUser([FromBody] RemoveRoleFromUserDto dto)
    {
        var command = new RemoveRoleFromUserCommand(dto.UserId, dto.RoleId);
        await _mediator.Send(command);
        return Ok(new { message = "Role removed successfully." });
    }

    [HttpPost("assign-permission-to-user")]
    public async Task<IActionResult> AssignPermissionToUser([FromBody] AssignPermissionToUserDto dto)
    {
        var command = new AssignPermissionToUserCommand(dto.UserId, dto.PermissionId);
        await _mediator.Send(command);
        return Ok(new { message = "Permission assigned successfully." });
    }

    [HttpPost("remove-permission-from-user")]
    public async Task<IActionResult> RemovePermissionFromUser([FromBody] RemovePermissionFromUserDto dto)
    {
        var command = new RemovePermissionFromUserCommand(dto.UserId, dto.PermissionId);
        await _mediator.Send(command);
        return Ok(new { message = "Permission removed successfully." });
    }

    [HttpPost("assign-permission-to-role")]
    public async Task<IActionResult> AssignPermissionToRole([FromBody] AssignPermissionToRoleDto dto)
    {
        var command = new AssignPermissionToRoleCommand(dto.RoleId, dto.PermissionId);
        await _mediator.Send(command);
        return Ok(new { message = "Permission assigned to role successfully." });
    }

    [HttpPost("remove-permission-from-role")]
    public async Task<IActionResult> RemovePermissionFromRole([FromBody] RemovePermissionFromRoleDto dto)
    {
        var command = new RemovePermissionFromRoleCommand(dto.RoleId, dto.PermissionId);
        await _mediator.Send(command);
        return Ok(new { message = "Permission removed from role successfully." });
    }
}

public record AssignRoleToUserDto(Guid UserId, Guid RoleId);
public record RemoveRoleFromUserDto(Guid UserId, Guid RoleId);
public record AssignPermissionToUserDto(Guid UserId, Guid PermissionId);
public record RemovePermissionFromUserDto(Guid UserId, Guid PermissionId);
public record AssignPermissionToRoleDto(Guid RoleId, Guid PermissionId);
public record RemovePermissionFromRoleDto(Guid RoleId, Guid PermissionId);
