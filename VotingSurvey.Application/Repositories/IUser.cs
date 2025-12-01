namespace VotingSurvey.Application.Repositories;

public interface IUser
{
    Task<bool> ExistsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> HasRoleAsync(Guid userId, string roleCode, CancellationToken cancellationToken = default);
}
