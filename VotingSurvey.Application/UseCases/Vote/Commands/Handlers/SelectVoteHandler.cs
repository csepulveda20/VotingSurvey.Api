using MediatR;
using VotingSurvey.Application.Models;
using VotingSurvey.Application.Repositories;
using VotingSurvey.Application.Services;

namespace VotingSurvey.Application.UseCases.Vote.Commands.Handlers;

internal sealed class SelectVoteHandler : IRequestHandler<SelectVote, ApiResponse<bool>>
{
    private readonly IVote _voteRepo;
    private readonly IVotingRecipient _recipientRepo;
    private readonly IUnitOfWork _uow;

    public SelectVoteHandler(IVote voteRepo, IVotingRecipient recipientRepo, IUnitOfWork uow)
    {
        _voteRepo = voteRepo;
        _recipientRepo = recipientRepo;
        _uow = uow;
    }

    public async Task<ApiResponse<bool>> Handle(SelectVote request, CancellationToken cancellationToken)
    {
        try
        {
            var isRecipient = await _recipientRepo.IsRecipientAsync(request.VotingId, request.UserId, cancellationToken);
            if (!isRecipient) return ApiResponse<bool>.Failure(new[] { "User is not recipient" });

            var exists = await _voteRepo.ExistsAsync(request.VotingId, request.UserId, cancellationToken);
            if (exists) return ApiResponse<bool>.Failure(new[] { "User already voted" });

            await _uow.BeginTransaction(cancellationToken);
            await _voteRepo.CreateAsync(request.VotingId, request.UserId, request.Option, DateTimeOffset.UtcNow, cancellationToken);
            await _uow.SaveChanges(cancellationToken);
            await _uow.CommitTransaction(cancellationToken);
            return ApiResponse<bool>.Success(true, "Vote selected");
        }
        catch (Exception ex)
        {
            await _uow.RollbackTransaction(cancellationToken);
            return ApiResponse<bool>.Failure(new[] { ex.Message });
        }
    }
}
