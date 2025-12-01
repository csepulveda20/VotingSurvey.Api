using MediatR;
using VotingSurvey.Application.Models;
using VotingSurvey.Application.Repositories;
using VotingSurvey.Application.Services;

namespace VotingSurvey.Application.UseCases.Voting.Commands.Handlers;

internal class CloseVotingEarlyHandler : IRequestHandler<CloseVotingEarly, ApiResponse<bool>>
{
    private readonly IVoting _votingRepo;
    private readonly IUser _userRepo;
    private readonly IUnitOfWork _uow;

    public CloseVotingEarlyHandler(IVoting votingRepo, IUser userRepo, IUnitOfWork uow)
    {
        _votingRepo = votingRepo;
        _userRepo = userRepo;
        _uow = uow;
    }

    public async Task<ApiResponse<bool>> Handle(CloseVotingEarly request, CancellationToken cancellationToken)
    {
        var isAdmin = await _userRepo.HasRoleAsync(request.AdminId, "ADMIN", cancellationToken);
        if (!isAdmin) return ApiResponse<bool>.Failure(new[] { "Only ADMIN can close votings" });
        try
        {
            await _uow.BeginTransaction(cancellationToken);
            await _votingRepo.CloseEarlyAsync(request.VotingId, DateTimeOffset.UtcNow, cancellationToken);
            await _uow.SaveChanges(cancellationToken);
            await _uow.CommitTransaction(cancellationToken);
            return ApiResponse<bool>.Success(true, "Voting closed early");
        }
        catch (Exception ex)
        {
            await _uow.RollbackTransaction(cancellationToken);
            return ApiResponse<bool>.Failure(new[] { ex.Message });
        }
    }
}
