namespace Application.Roles.Dtos;

public class RoleFilterRequest
{
    public string? SearchTerm { get; set; }
    public string? SortBy { get; set; } = "name";
    public bool SortDescending { get; set; } = false;
}
