using Microsoft.EntityFrameworkCore;
using VotingSurvey.Application.Repositories;
using VotingSurvey.Infrastructure.Persistence.Context;

namespace VotingSurvey.Infrastructure.Persistence.Repositories;

public sealed class UnitRepository(DataBaseContext context) : IUnit
{
    private readonly DataBaseContext _context = context;

    public Task<bool> ExistsAsync(Guid unitId, CancellationToken cancellationToken = default)
        => _context.Units.AnyAsync(u => u.Id == unitId, cancellationToken);
}
