namespace Application.Users.Dtos;

public class UserFilterRequest
{
    public string? SearchTerm { get; set; }
    public bool? IsAdmin { get; set; }
    public string? SortBy { get; set; } = "username";
    public bool SortDescending { get; set; } = false;
}
