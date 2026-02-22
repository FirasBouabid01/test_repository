namespace Application.Permissions.Dtos;

public class PermissionDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int RoleCount { get; set; }
    public int UserCount { get; set; }
}
