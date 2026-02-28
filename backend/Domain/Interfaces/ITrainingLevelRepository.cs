using Domain.Entities;

namespace Domain.Interfaces;

/// <summary>
/// Repository interface for training level operations.
/// </summary>
public interface ITrainingLevelRepository : IGenericRepository<TrainingLevel>
{
    /// <summary>
    /// Gets a paginated list of training levels ordered by sequence.
    /// </summary>
    Task<IEnumerable<TrainingLevel>> GetAllOrderedAsync(int pageNumber, int pageSize);

    /// <summary>
    /// Gets all training levels with their statistics.
    /// </summary>
    Task<IEnumerable<TrainingLevel>> GetAllWithStatsAsync(bool? isActive = null);

    /// <summary>
    /// Gets a training level by its name.
    /// </summary>
    Task<TrainingLevel?> GetByNameAsync(string name);

    /// <summary>
    /// Checks if a training level has any dependent levels.
    /// </summary>
    Task<bool> HasDependentLevelsAsync(Guid levelId);

    /// <summary>
    /// Checks if any leaders are currently assigned to this training level.
    /// </summary>
    Task<bool> HasAssignedLeadersAsync(Guid levelId);

    /// <summary>
    /// Checks if adding a prerequisite would create a circular dependency.
    /// </summary>
    Task<bool> IsCircularDependencyAsync(Guid levelId, Guid? prerequisiteId);

    /// <summary>
    /// Gets the maximum sequence order currently in use.
    /// </summary>
    Task<int> GetMaxSequenceOrderAsync();

    /// <summary>
    /// Gets a training level along with its statistics (e.g., number of users).
    /// </summary>
    Task<object?> GetLevelWithStatsAsync(Guid id);
}
