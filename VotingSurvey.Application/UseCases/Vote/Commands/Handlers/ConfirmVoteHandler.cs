using MediatR;
using VotingSurvey.Application.Models;
using VotingSurvey.Application.Repositories;
using VotingSurvey.Application.Services;

namespace VotingSurvey.Application.UseCases.Vote.Commands.Handlers;

internal sealed class ConfirmVoteHandler : IRequestHandler<ConfirmVote, ApiResponse<bool>>
{
    private readonly IVote _voteRepo;
    private readonly IUnitOfWork _uow;

    public ConfirmVoteHandler(IVote voteRepo, IUnitOfWork uow)
    {
        _voteRepo = voteRepo;
        _uow = uow;
    }

    public async Task<ApiResponse<bool>> Handle(ConfirmVote request, CancellationToken cancellationToken)
    {
        try
        {
            await _uow.BeginTransaction(cancellationToken);
            await _voteRepo.ConfirmAsync(request.VotingId, request.UserId, DateTimeOffset.UtcNow, cancellationToken);
            await _uow.SaveChanges(cancellationToken);
            await _uow.CommitTransaction(cancellationToken);
            return ApiResponse<bool>.Success(true, "Vote confirmed");
        }
        catch (Exception ex)
        {
            await _uow.RollbackTransaction(cancellationToken);
            return ApiResponse<bool>.Failure(new[] { ex.Message });
        }
    }
}
