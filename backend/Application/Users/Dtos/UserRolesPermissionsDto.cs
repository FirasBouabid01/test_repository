namespace Application.Users.Dtos;

public class UserRolesPermissionsDto
{
    public List<string> Roles { get; set; } = new();
    public List<string> Permissions { get; set; } = new();
}
