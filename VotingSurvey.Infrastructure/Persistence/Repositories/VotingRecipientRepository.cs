using Microsoft.EntityFrameworkCore;
using VotingSurvey.Application.Repositories;
using VotingSurvey.Infrastructure.Persistence.Context;
using VotingSurvey.Domain.Entities;

namespace VotingSurvey.Infrastructure.Persistence.Repositories;

public sealed class VotingRecipientRepository(DataBaseContext context) : IVotingRecipient
{
    private readonly DataBaseContext _context = context;

    public async Task AddAsync(Guid votingId, IEnumerable<Guid> userIds, CancellationToken cancellationToken = default)
    {
        var now = DateTimeOffset.UtcNow;
        var entities = userIds.Select(uid => new VotingRecipient(votingId, uid, now));
        await _context.VotingRecipients.AddRangeAsync(entities, cancellationToken);
    }

    public Task<bool> IsRecipientAsync(Guid votingId, Guid userId, CancellationToken cancellationToken = default)
        => _context.VotingRecipients.AnyAsync(v => v.VotingId == votingId && v.UserId == userId, cancellationToken);
}
