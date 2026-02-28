namespace Application.TrainingLevels.Dtos;

public class UpdateTrainingLevelDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? PrerequisiteLevelId { get; set; }
    public bool IsActive { get; set; }
}
