namespace Application.Roles.Dtos;

public class RoleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public IEnumerable<string> Permissions { get; set; } = new List<string>();
    public int UserCount { get; set; }
}
