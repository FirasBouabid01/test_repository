using Application.Users.Commands.CreateUser;
using Application.Users.Commands.LoginUser;
using Application.Users.Commands.ChangePassword;
using Application.Users.Queries.GetUserProfile;
using Application.Users.Queries.GetUsers;
using Application.Users.Dtos;
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

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _mediator.Send(new GetUsersQuery());
        return Ok(users);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserCommand command)
    {
        var token = await _mediator.Send(command);
        return Ok(new { token });
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserCommand command)
    {
        var userId = await _mediator.Send(command);
        return Ok(userId);
    }
}
