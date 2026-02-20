using Application.Roles.Commands.CreateRole;
using Application.Roles.Commands.DeleteRole;
using Application.Roles.Commands.UpdateRole;
using Application.Roles.Queries.GetAllRoles;
using Application.Roles.Queries.GetRoleById;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/roles")]
[Authorize] // Requires login
public class RolesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly RoleService _roleService;

    public RolesController(IMediator mediator, RoleService roleService)
    {
        _mediator = mediator;
        _roleService = roleService;
    }

    private bool IsAdminUser()
    {
        var isAdminClaim = User.FindFirst("is_admin")?.Value;
        return isAdminClaim == "true";
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        if (!IsAdminUser()) return Forbid();
        var roles = await _mediator.Send(new GetAllRolesQuery());
        return Ok(roles);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        if (!IsAdminUser()) return Forbid();
        var role = await _mediator.Send(new GetRoleByIdQuery(id));
        return Ok(role);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoleCommand command)
    {
        if (!IsAdminUser()) return Forbid();
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoleCommand command)
    {
        if (!IsAdminUser()) return Forbid();
        if (id != command.id) return BadRequest("ID mismatch");
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (!IsAdminUser()) return Forbid();
        await _mediator.Send(new DeleteRoleCommand(id));
        return NoContent();
    }

    [HttpPut("{id}/permissions")]
    public async Task<IActionResult> UpdatePermissions(Guid id, [FromBody] List<Guid> permissionIds)
    {
        if (!IsAdminUser()) return Forbid();
        await _mediator.Send(new Application.Roles.Commands.UpdateRolePermissions.UpdateRolePermissionsCommand(id, permissionIds));
        return NoContent();
    }

    [HttpPost("assign")]
    public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequest request)
    {
        if (!IsAdminUser()) return Forbid();
        await _roleService.AssignRoleToUserAsync(
            request.UserId,
            request.RoleId
        );

        return Ok(new { message = "Role assigned successfully" });
    }
}

public record AssignRoleRequest(Guid UserId, Guid RoleId);