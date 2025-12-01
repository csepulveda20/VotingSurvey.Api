using VotingSurvey.Domain.Entities;
using VotingSurvey.Domain.ValueObjects;
using VotingSurvey.Application.Models;

namespace VotingSurvey.Application.Repositories
{
    public interface IVoting
    {
        Task CreateAsync(Voting voting, CancellationToken cancellationToken = default);
        Task<Voting?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        // Admin listings with filtering
        Task<IReadOnlyList<Voting>> ListByCreatorAsync(Guid creatorId, QueryParam query, CancellationToken cancellationToken = default);

        // Resident listings with filtering
        Task<IReadOnlyList<Voting>> ListForRecipientAsync(Guid userId, QueryParam query, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Voting>> ListUpcomingForRecipientAsync(Guid userId, QueryParam query, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Voting>> ListActiveForRecipientAsync(Guid userId, QueryParam query, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Voting>> ListClosedForRecipientAsync(Guid userId, QueryParam query, CancellationToken cancellationToken = default);

        // Recipients
        Task AddRecipientsAsync(Guid votingId, IEnumerable<Guid> userIds, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Guid>> ListRecipientsAsync(Guid votingId, CancellationToken cancellationToken = default);

        // Edit before start (full edit) and extend end
        Task EditBeforeStartAsync(Guid votingId, string newTitle, string newDescription, VotingWindow newWindow, CancellationToken cancellationToken = default);
        Task ExtendEndAsync(Guid votingId, DateTimeOffset newEnd, CancellationToken cancellationToken = default);
        Task CloseEarlyAsync(Guid votingId, DateTimeOffset now, CancellationToken cancellationToken = default);

        // Votes
        Task SelectVoteAsync(Guid votingId, Guid userId, VoteOption option, DateTimeOffset now, CancellationToken cancellationToken = default);
        Task ConfirmVoteAsync(Guid votingId, Guid userId, DateTimeOffset now, CancellationToken cancellationToken = default);

        // Participants lists
        Task<IReadOnlyList<Guid>> ListYesVotersAsync(Guid votingId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Guid>> ListNoVotersAsync(Guid votingId, CancellationToken cancellationToken = default);

        // Tally
        Task<(int yes, int no)> TallyAsync(Guid votingId, CancellationToken cancellationToken = default);
    }
}
