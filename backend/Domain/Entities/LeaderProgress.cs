using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

/// <summary>
/// Tracks the current training level progress for a user.
/// </summary>
public class LeaderProgress
{
    /// <summary>
    /// Gets or sets the unique identifier for the progress record.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the ID of the user.
    /// </summary>
    [Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the user associated with this progress record.
    /// </summary>
    public virtual User User { get; set; } = null!;

    /// <summary>
    /// Gets or sets the ID of the current training level.
    /// </summary>
    [Required]
    public Guid CurrentLevelId { get; set; }

    /// <summary>
    /// Gets or sets the current training level.
    /// </summary>
    public virtual TrainingLevel CurrentLevel { get; set; } = null!;

    /// <summary>
    /// Gets or sets the date and time when the current level was started.
    /// </summary>
    public DateTime StartedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the record was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}
