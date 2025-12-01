using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using VotingSurvey.Application.Services;

namespace VotingSurvey.Infrastructure.Persistence.Services
{
    public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        private readonly TContext _context;
        private IDbContextTransaction? _currentTransaction;

        public UnitOfWork(TContext context)
        {
            _context = context;
        }

        public async Task BeginTransaction(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction is not null) return;
            _currentTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransaction(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction is null) return;
            await _context.SaveChangesAsync(cancellationToken);
            await _currentTransaction.CommitAsync(cancellationToken);
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }

        public async Task RollbackTransaction(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction is null) return;
            await _currentTransaction.RollbackAsync(cancellationToken);
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }

        public Task<int> SaveChanges(CancellationToken cancellationToken = default)
            => _context.SaveChangesAsync(cancellationToken);

        public async ValueTask DisposeAsync()
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
    }
}
