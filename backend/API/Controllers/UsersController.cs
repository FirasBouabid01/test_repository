using Application.Users.Commands.CreateUser;
using Application.Users.Commands.LoginUser;
using Application.Users.Commands.ChangePassword;
using Application.Users.Commands.UpdateUser;
using Application.Users.Commands.DeleteUser;
using Application.Users.Queries.GetUserProfile;
using Application.Users.Queries.GetUsers;
using Application.Users.Queries.GetUserById;
using Application.Users.Dtos;
using Application.Common.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // ==================== Authentication ====================

    [HttpGet("profile")]
    [Authorize]
    public async Task<IActionResult> GetProfile()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized();
        }

        var userId = Guid.Parse(userIdClaim.Value);
        var query = new GetUserProfileQuery(userId);
        var userProfile = await _mediator.Send(query);

        return Ok(userProfile);
    }

    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized();
        }

        var userId = Guid.Parse(userIdClaim.Value);
        var command = new ChangePasswordCommand(userId, request.CurrentPassword, request.NewPassword, request.ConfirmNewPassword);
        
        await _mediator.Send(command);

        return Ok(new { message = "Password changed successfully." });
    }

    public record ChangePasswordRequest(string CurrentPassword, string NewPassword, string ConfirmNewPassword);

    [HttpPost("login")]
public async Task<IActionResult> Login(LoginUserCommand command)
{
    var token = await _mediator.Send(command);
    return Ok(new { token });
}

    // ==================== CRUD Operations ====================

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] bool? isAdmin = null,
        [FromQuery] string? sortBy = "username",
        [FromQuery] bool sortDescending = false)
    {
        var query = new GetUsersQuery(pageNumber, pageSize, searchTerm, isAdmin, sortBy, sortDescending);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{userId}")]
    [Authorize]
    public async Task<IActionResult> GetById(Guid userId)
    {
        var query = new GetUserByIdQuery(userId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserCommand command)
    {
        var userId = await _mediator.Send(command);
        return Ok(new { userId });
    }

    [HttpPut("{userId}")]
    [Authorize]
    public async Task<IActionResult> Update(Guid userId, UpdateUserDto dto)
    {
        var command = new UpdateUserCommand(
            userId,
            dto.FirstName,
            dto.LastName,
            dto.Email,
            dto.DateOfBirth,
            dto.PhoneNumber,
            dto.Address,
            dto.IsAdmin
        );
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{userId}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid userId)
    {
        var command = new DeleteUserCommand(userId);
        await _mediator.Send(command);
        return Ok(new { message = "User deleted successfully." });
    }
}
