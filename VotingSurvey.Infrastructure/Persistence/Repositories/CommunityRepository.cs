using Microsoft.EntityFrameworkCore;
using VotingSurvey.Application.Repositories;
using VotingSurvey.Infrastructure.Persistence.Context;

namespace VotingSurvey.Infrastructure.Persistence.Repositories;

public sealed class CommunityRepository(DataBaseContext context) : ICommunity
{
    private readonly DataBaseContext _context = context;

    public Task<bool> ExistsAsync(Guid communityId, CancellationToken cancellationToken = default)
        => _context.Communities.AnyAsync(c => c.Id == communityId && c.IsActive, cancellationToken);
}
