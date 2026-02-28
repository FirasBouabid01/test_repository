namespace Application.TrainingLevels.Dtos;

public class TrainingLevelHierarchyDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int SequenceOrder { get; set; }
    public List<TrainingLevelHierarchyDto> Children { get; set; } = new();
}
