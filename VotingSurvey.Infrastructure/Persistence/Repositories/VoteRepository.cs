using Microsoft.EntityFrameworkCore;
using VotingSurvey.Application.Repositories;
using VotingSurvey.Domain.Entities;
using VotingSurvey.Domain.ValueObjects;
using VotingSurvey.Infrastructure.Persistence.Context;

namespace VotingSurvey.Infrastructure.Persistence.Repositories;

public sealed class VoteRepository(DataBaseContext context) : IVote
{
    private readonly DataBaseContext _context = context;

    public Task<bool> ExistsAsync(Guid votingId, Guid userId, CancellationToken cancellationToken = default)
        => _context.Votes.AnyAsync(v => v.VotingId == votingId && v.UserId == userId, cancellationToken);

    public async Task CreateAsync(Guid votingId, Guid userId, VoteOption option, DateTimeOffset now, CancellationToken cancellationToken = default)
    {
        var vote = VotingSurvey.Domain.Entities.Vote.Select(Guid.NewGuid(), votingId, userId, option, now);
        await _context.Votes.AddAsync(vote, cancellationToken);
    }

    public async Task ConfirmAsync(Guid votingId, Guid userId, DateTimeOffset now, CancellationToken cancellationToken = default)
    {
        var vote = await _context.Votes.FirstOrDefaultAsync(v => v.VotingId == votingId && v.UserId == userId, cancellationToken)
            ?? throw new KeyNotFoundException("Vote not found");
        vote.Confirm(now);
        _context.Votes.Update(vote);
    }
}
