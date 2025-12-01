using Microsoft.EntityFrameworkCore;
using VotingSurvey.Application.Models;
using VotingSurvey.Application.Repositories;
using VotingSurvey.Domain.Entities;
using VotingSurvey.Domain.ValueObjects;
using VotingSurvey.Infrastructure.Persistence.Context;

namespace VotingSurvey.Infrastructure.Persistence.Repositories
{
    public sealed class VotingRepository : IVoting
    {
        private readonly DataBaseContext _context;
        public VotingRepository(DataBaseContext context) => _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task CreateAsync(Voting voting, CancellationToken cancellationToken = default)
        {
            await _context.Votings.AddAsync(voting, cancellationToken);
        }

        public Task<Voting?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _context.Votings.FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
        }

        public async Task<IReadOnlyList<Voting>> ListByCreatorAsync(Guid creatorId, QueryParam query, CancellationToken cancellationToken = default)
        {
            var q = _context.Votings.Where(v => v.CreatedByUserId == creatorId);
            q = ApplySearchOrder(q, query);
            return await q.ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Voting>> ListForRecipientAsync(Guid userId, QueryParam query, CancellationToken cancellationToken = default)
        {
            var baseQuery = from v in _context.Votings
                            join r in _context.VotingRecipients on v.Id equals r.VotingId
                            where r.UserId == userId
                            select v;
            baseQuery = ApplySearchOrder(baseQuery, query);
            return await baseQuery.Distinct().ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Voting>> ListUpcomingForRecipientAsync(Guid userId, QueryParam query, CancellationToken cancellationToken = default)
        {
            var now = DateTimeOffset.UtcNow;
            var q = from v in _context.Votings
                    join r in _context.VotingRecipients on v.Id equals r.VotingId
                    where r.UserId == userId && now < v.Window.StartsAt
                    select v;
            q = ApplySearchOrder(q, query);
            return await q.Distinct().ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Voting>> ListActiveForRecipientAsync(Guid userId, QueryParam query, CancellationToken cancellationToken = default)
        {
            var now = DateTimeOffset.UtcNow;
            var q = from v in _context.Votings
                    join r in _context.VotingRecipients on v.Id equals r.VotingId
                    where r.UserId == userId && now >= v.Window.StartsAt && now <= v.Window.EndsAt
                    select v;
            q = ApplySearchOrder(q, query);
            return await q.Distinct().ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Voting>> ListClosedForRecipientAsync(Guid userId, QueryParam query, CancellationToken cancellationToken = default)
        {
            var now = DateTimeOffset.UtcNow;
            var q = from v in _context.Votings
                    join r in _context.VotingRecipients on v.Id equals r.VotingId
                    where r.UserId == userId && now > v.Window.EndsAt
                    select v;
            q = ApplySearchOrder(q, query);
            return await q.Distinct().ToListAsync(cancellationToken);
        }

        public async Task AddRecipientsAsync(Guid votingId, IEnumerable<Guid> userIds, CancellationToken cancellationToken = default)
        {
            var now = DateTimeOffset.UtcNow;
            var entities = userIds.Select(uid => new VotingRecipient(votingId, uid, now));
            await _context.VotingRecipients.AddRangeAsync(entities, cancellationToken);
        }

        public async Task<IReadOnlyList<Guid>> ListRecipientsAsync(Guid votingId, CancellationToken cancellationToken = default)
        {
            return await _context.VotingRecipients
                .Where(v => v.VotingId == votingId)
                .Select(v => v.UserId)
                .ToListAsync(cancellationToken);
        }

        public async Task EditBeforeStartAsync(Guid votingId, string newTitle, string newDescription, VotingWindow newWindow, CancellationToken cancellationToken = default)
        {
            var voting = await GetByIdAsync(votingId, cancellationToken) ?? throw new KeyNotFoundException("Voting not found");
            voting.EditBeforeStart(newTitle, newDescription, newWindow, DateTimeOffset.UtcNow);
            _context.Votings.Update(voting);
        }

        public async Task ExtendEndAsync(Guid votingId, DateTimeOffset newEnd, CancellationToken cancellationToken = default)
        {
            var voting = await GetByIdAsync(votingId, cancellationToken) ?? throw new KeyNotFoundException("Voting not found");
            voting.ExtendEnd(newEnd, DateTimeOffset.UtcNow);
            _context.Votings.Update(voting);
        }

        public async Task CloseEarlyAsync(Guid votingId, DateTimeOffset now, CancellationToken cancellationToken = default)
        {
            var voting = await GetByIdAsync(votingId, cancellationToken) ?? throw new KeyNotFoundException("Voting not found");
            voting.CloseEarly(now);
            _context.Votings.Update(voting);
        }

        public async Task SelectVoteAsync(Guid votingId, Guid userId, VoteOption option, DateTimeOffset now, CancellationToken cancellationToken = default)
        {
            var voting = await GetByIdAsync(votingId, cancellationToken) ?? throw new KeyNotFoundException("Voting not found");
            voting.SelectOption(userId, option, now);
            _context.Votings.Update(voting);
        }

        public async Task ConfirmVoteAsync(Guid votingId, Guid userId, DateTimeOffset now, CancellationToken cancellationToken = default)
        {
            var voting = await GetByIdAsync(votingId, cancellationToken) ?? throw new KeyNotFoundException("Voting not found");
            voting.ConfirmVote(userId, now);
            _context.Votings.Update(voting);
        }

        public async Task<IReadOnlyList<Guid>> ListYesVotersAsync(Guid votingId, CancellationToken cancellationToken = default)
        {
            var voting = await GetByIdAsync(votingId, cancellationToken) ?? throw new KeyNotFoundException("Voting not found");
            return voting.Votes.Where(v => v.Confirmed && v.Option == VoteOption.Yes).Select(v => v.UserId).ToList();
        }

        public async Task<IReadOnlyList<Guid>> ListNoVotersAsync(Guid votingId, CancellationToken cancellationToken = default)
        {
            var voting = await GetByIdAsync(votingId, cancellationToken) ?? throw new KeyNotFoundException("Voting not found");
            return voting.Votes.Where(v => v.Confirmed && v.Option == VoteOption.No).Select(v => v.UserId).ToList();
        }

        public async Task<(int yes, int no)> TallyAsync(Guid votingId, CancellationToken cancellationToken = default)
        {
            var voting = await GetByIdAsync(votingId, cancellationToken) ?? throw new KeyNotFoundException("Voting not found");
            return voting.TallyConfirmed();
        }

        private static IQueryable<Voting> ApplySearchOrder(IQueryable<Voting> q, QueryParam query)
        {
            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var term = query.Search.Trim();
                q = q.Where(v => v.Title.Contains(term));
            }

            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                q = query.OrderBy.ToLowerInvariant() switch
                {
                    "title" => query.IsAscending ? q.OrderBy(x => x.Title) : q.OrderByDescending(x => x.Title),
                    "startat" => query.IsAscending ? q.OrderBy(x => x.Window.StartsAt) : q.OrderByDescending(x => x.Window.StartsAt),
                    "endat" => query.IsAscending ? q.OrderBy(x => x.Window.EndsAt) : q.OrderByDescending(x => x.Window.EndsAt),
                    _ => q.OrderByDescending(x => x.CreatedAt)
                };
            }
            else
            {
                q = q.OrderByDescending(x => x.CreatedAt);
            }
            return q;
        }
    }
}
