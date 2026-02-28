using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

/// <summary>
/// Records the history of training level changes for users.
/// </summary>
public class LevelHistory
{
    /// <summary>
    /// Gets or sets the unique identifier for the history record.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the ID of the user whose level changed.
    /// </summary>
    [Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the user whose level changed.
    /// </summary>
    public virtual User User { get; set; } = null!;

    /// <summary>
    /// Gets or sets the ID of the level the user transitioned from.
    /// </summary>
    public Guid? FromLevelId { get; set; }

    /// <summary>
    /// Gets or sets the level the user transitioned from.
    /// </summary>
    public virtual TrainingLevel? FromLevel { get; set; }

    /// <summary>
    /// Gets or sets the ID of the level the user transitioned to.
    /// </summary>
    [Required]
    public Guid ToLevelId { get; set; }

    /// <summary>
    /// Gets or sets the level the user transitioned to.
    /// </summary>
    public virtual TrainingLevel ToLevel { get; set; } = null!;

    /// <summary>
    /// Gets or sets the date and time when the level change occurred.
    /// </summary>
    public DateTime ChangedAt { get; set; }

    /// <summary>
    /// Gets or sets the ID of the user who performed the change.
    /// </summary>
    [Required]
    public Guid ChangedBy { get; set; }

    /// <summary>
    /// Gets or sets the user who performed the change.
    /// </summary>
    public virtual User AdminUser { get; set; } = null!;

    /// <summary>
    /// Gets or sets the reason for the change ("automatic" or "manual_override").
    /// </summary>
    [Required]
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets additional notes about the change.
    /// </summary>
    [MaxLength(500)]
    public string? Notes { get; set; }
}
