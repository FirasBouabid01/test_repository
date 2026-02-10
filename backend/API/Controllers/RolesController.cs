using API.Contracts;
using API.Middleware;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/roles")]
public class RolesController : ControllerBase
{
    private readonly RoleService _roleService;

    public RolesController(RoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpPost("assign")]
    [HasPermission("User.Update")] // غير Admin
    public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequest request)
    {
        await _roleService.AssignRoleToUserAsync(
            request.UserId,
            request.RoleId
        );

        return Ok(new { message = "Role assigned successfully" });
    }
}