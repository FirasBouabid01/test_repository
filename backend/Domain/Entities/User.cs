namespace Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();

    /// <summary>
    /// Gets or sets the training progress record for this user.
    /// </summary>
    public virtual LeaderProgress? LeaderProgress { get; set; }

    /// <summary>
    /// Gets or sets the history of level changes for this user.
    /// </summary>
    public virtual ICollection<LevelHistory> LevelHistories { get; set; } = new List<LevelHistory>();

    /// <summary>
    /// Gets or sets the history of level changes performed by this user (as admin).
    /// </summary>
    public virtual ICollection<LevelHistory> AdministeredChanges { get; set; } = new List<LevelHistory>();
}
