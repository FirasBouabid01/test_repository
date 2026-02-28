namespace Application.TrainingLevels.Dtos;

public class CreateTrainingLevelDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? PrerequisiteLevelId { get; set; }
}
