namespace VotingSurvey.Application.Repositories;

public interface ICommunity
{
    Task<bool> ExistsAsync(Guid communityId, CancellationToken cancellationToken = default);
}
