using MediatR;
using VotingSurvey.Application.Models;
using VotingSurvey.Application.Repositories;
using VotingSurvey.Application.Services;
using VotingSurvey.Domain.ValueObjects;

namespace VotingSurvey.Application.UseCases.Voting.Commands.Handlers;

internal sealed class EditVotingBeforeStartHandler : IRequestHandler<EditVotingBeforeStart, ApiResponse<bool>>
{
    private readonly IVoting _votingRepo;
    private readonly IUnitOfWork _uow;

    public EditVotingBeforeStartHandler(IVoting votingRepo, IUnitOfWork uow)
    {
        _votingRepo = votingRepo;
        _uow = uow;
    }

    public async Task<ApiResponse<bool>> Handle(EditVotingBeforeStart request, CancellationToken cancellationToken)
    {
        try
        {
            await _uow.BeginTransaction(cancellationToken);
            var window = VotingWindow.Create(request.StartDate, request.EndDate);
            await _votingRepo.EditBeforeStartAsync(request.VotingId, request.Title, request.Description, window, cancellationToken);
            await _uow.SaveChanges(cancellationToken);
            await _uow.CommitTransaction(cancellationToken);
            return ApiResponse<bool>.Success(true, "Voting edited");
        }
        catch (Exception ex)
        {
            await _uow.RollbackTransaction(cancellationToken);
            return ApiResponse<bool>.Failure(new[] { ex.Message });
        }
    }
}
