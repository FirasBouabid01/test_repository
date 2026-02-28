namespace Application.TrainingLevels.Dtos;

public class TrainingLevelDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int SequenceOrder { get; set; }
    public Guid? PrerequisiteLevelId { get; set; }
    public string? PrerequisiteLevelName { get; set; }
    public int LeaderCount { get; set; }
    public int TaskCount { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
