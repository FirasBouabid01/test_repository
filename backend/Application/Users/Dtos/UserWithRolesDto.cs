namespace Application.Users.Dtos;

public class UserWithRolesDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;

    public List<string> Roles { get; set; } = new();
    public List<string> Permissions { get; set; } = new();
}