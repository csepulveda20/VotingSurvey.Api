using Microsoft.EntityFrameworkCore;
using VotingSurvey.Application.Repositories;
using VotingSurvey.Infrastructure.Persistence.Context;

namespace VotingSurvey.Infrastructure.Persistence.Repositories;

public sealed class UserRepository(DataBaseContext context) : IUser
{
    private readonly DataBaseContext _context = context;

    public Task<bool> ExistsAsync(Guid userId, CancellationToken cancellationToken = default)
        => _context.Users.AnyAsync(u => u.Id == userId && u.IsActive, cancellationToken);

    public Task<bool> HasRoleAsync(Guid userId, string roleCode, CancellationToken cancellationToken = default)
        => _context.Set<dynamic>().AnyAsync(_ => false, cancellationToken); // TODO: map Role & UserRole tables
}
