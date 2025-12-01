namespace VotingSurvey.Application.Repositories;

public interface IUnit
{
    Task<bool> ExistsAsync(Guid unitId, CancellationToken cancellationToken = default);
}
