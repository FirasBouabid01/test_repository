using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

/// <summary>
/// Represents a training level within the system.
/// </summary>
public class TrainingLevel
{
    /// <summary>
    /// Gets or sets the unique identifier for the training level.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the training level.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the training level.
    /// </summary>
    [MaxLength(1000)]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the sequence order of the level.
    /// </summary>
    public int SequenceOrder { get; set; }

    /// <summary>
    /// Gets or sets the ID of the prerequisite training level.
    /// </summary>
    public Guid? PrerequisiteLevelId { get; set; }

    /// <summary>
    /// Gets or sets the prerequisite training level.
    /// </summary>
    public virtual TrainingLevel? PrerequisiteLevel { get; set; }

    /// <summary>
    /// Gets or sets whether the level is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets the date and time when the level was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the level was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the collection of levels for which this level is a prerequisite.
    /// </summary>
    public virtual ICollection<TrainingLevel> DependentLevels { get; set; } = new List<TrainingLevel>();

    /// <summary>
    /// Gets or sets the collection of leader progress records at this level.
    /// </summary>
    public virtual ICollection<LeaderProgress> LeaderProgresses { get; set; } = new List<LeaderProgress>();

    /// <summary>
    /// Gets or sets the collection of history records where this level was the destination level.
    /// </summary>
    public virtual ICollection<LevelHistory> DestinationHistoryRecords { get; set; } = new List<LevelHistory>();

    /// <summary>
    /// Gets or sets the collection of history records where this level was the source level.
    /// </summary>
    public virtual ICollection<LevelHistory> SourceHistoryRecords { get; set; } = new List<LevelHistory>();
}
