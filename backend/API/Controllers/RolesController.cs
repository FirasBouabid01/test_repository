using Application.Roles.Commands.CreateRole;
using Application.Roles.Commands.UpdateRole;
using Application.Roles.Commands.DeleteRole;
using Application.Roles.Queries.GetRoles;
using Application.Roles.Queries.GetRoleById;
using Application.Roles.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RolesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RolesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] string? sortBy = "name",
        [FromQuery] bool sortDescending = false)
    {
        var query = new GetRolesQuery(pageNumber, pageSize, searchTerm, sortBy, sortDescending);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{roleId}")]
    public async Task<IActionResult> GetById(Guid roleId)
    {
        var query = new GetRoleByIdQuery(roleId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoleDto dto)
    {
        var command = new CreateRoleCommand(dto.Name, dto.PermissionIds ?? new List<Guid>());
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { roleId = result.Id }, result);
    }

    [HttpPut("{roleId}")]
    public async Task<IActionResult> Update(Guid roleId, [FromBody] UpdateRoleDto dto)
    {
        var command = new UpdateRoleCommand(roleId, dto.Name, dto.PermissionIds ?? new List<Guid>());
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{roleId}")]
    public async Task<IActionResult> Delete(Guid roleId)
    {
        var command = new DeleteRoleCommand(roleId);
        await _mediator.Send(command);
        return Ok(new { message = "Role deleted successfully." });
    }
}

public record CreateRoleDto(string Name, IEnumerable<Guid>? PermissionIds);
public record UpdateRoleDto(string Name, IEnumerable<Guid>? PermissionIds);