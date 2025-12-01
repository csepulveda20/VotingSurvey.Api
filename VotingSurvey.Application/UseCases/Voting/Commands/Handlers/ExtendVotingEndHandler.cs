using MediatR;
using VotingSurvey.Application.Models;
using VotingSurvey.Application.Repositories;
using VotingSurvey.Application.Services;

namespace VotingSurvey.Application.UseCases.Voting.Commands.Handlers;

internal sealed class ExtendVotingEndHandler : IRequestHandler<ExtendVotingEnd, ApiResponse<bool>>
{
    private readonly IVoting _votingRepo;
    private readonly IUnitOfWork _uow;

    public ExtendVotingEndHandler(IVoting votingRepo, IUnitOfWork uow)
    {
        _votingRepo = votingRepo;
        _uow = uow;
    }

    public async Task<ApiResponse<bool>> Handle(ExtendVotingEnd request, CancellationToken cancellationToken)
    {
        try
        {
            await _uow.BeginTransaction(cancellationToken);
            await _votingRepo.ExtendEndAsync(request.VotingId, request.NewEnd, cancellationToken);
            await _uow.SaveChanges(cancellationToken);
            await _uow.CommitTransaction(cancellationToken);
            return ApiResponse<bool>.Success(true, "End extended");
        }
        catch (Exception ex)
        {
            await _uow.RollbackTransaction(cancellationToken);
            return ApiResponse<bool>.Failure(new[] { ex.Message });
        }
    }
}
