using Application.Permissions.Commands.CreatePermission;
using Application.Permissions.Commands.DeletePermission;
using Application.Permissions.Commands.UpdatePermission;
using Application.Permissions.Queries.GetAllPermissions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/permissions")]
[Authorize]
public class PermissionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PermissionsController(IMediator mediator)
    {
        _mediator = mediator;
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
        var permissions = await _mediator.Send(new GetAllPermissionsQuery());
        return Ok(permissions);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePermissionCommand command)
    {
        if (!IsAdminUser()) return Forbid();
        var id = await _mediator.Send(command);
        return Ok(new { id });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePermissionCommand command)
    {
        if (!IsAdminUser()) return Forbid();
        if (id != command.Id) return BadRequest("ID mismatch");
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (!IsAdminUser()) return Forbid();
        await _mediator.Send(new DeletePermissionCommand(id));
        return NoContent();
    }
}
