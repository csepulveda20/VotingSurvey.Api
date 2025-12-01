namespace VotingSurvey.Application.Repositories;

public interface IVotingRecipient
{
    Task AddAsync(Guid votingId, IEnumerable<Guid> userIds, CancellationToken cancellationToken = default);
    Task<bool> IsRecipientAsync(Guid votingId, Guid userId, CancellationToken cancellationToken = default);
}
