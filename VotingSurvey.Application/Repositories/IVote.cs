using VotingSurvey.Domain.ValueObjects;

namespace VotingSurvey.Application.Repositories;

public interface IVote
{
    Task<bool> ExistsAsync(Guid votingId, Guid userId, CancellationToken cancellationToken = default);
    Task CreateAsync(Guid votingId, Guid userId, VoteOption option, DateTimeOffset now, CancellationToken cancellationToken = default);
    Task ConfirmAsync(Guid votingId, Guid userId, DateTimeOffset now, CancellationToken cancellationToken = default);
}
