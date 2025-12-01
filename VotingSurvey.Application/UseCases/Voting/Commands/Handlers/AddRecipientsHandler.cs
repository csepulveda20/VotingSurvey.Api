using MediatR;
using VotingSurvey.Application.Models;
using VotingSurvey.Application.Repositories;
using VotingSurvey.Application.Services;

namespace VotingSurvey.Application.UseCases.Voting.Commands.Handlers;

internal sealed class AddRecipientsHandler : IRequestHandler<AddRecipients, ApiResponse<bool>>
{
    private readonly IVoting _votingRepo;
    private readonly IVotingRecipient _recipientRepo;
    private readonly IUnitOfWork _uow;

    public AddRecipientsHandler(IVoting votingRepo, IVotingRecipient recipientRepo, IUnitOfWork uow)
    {
        _votingRepo = votingRepo;
        _recipientRepo = recipientRepo;
        _uow = uow;
    }

    public async Task<ApiResponse<bool>> Handle(AddRecipients request, CancellationToken cancellationToken)
    {
        try
        {
            await _uow.BeginTransaction(cancellationToken);
            await _recipientRepo.AddAsync(request.VotingId, request.RecipientIds, cancellationToken);
            await _uow.SaveChanges(cancellationToken);
            await _uow.CommitTransaction(cancellationToken);
            return ApiResponse<bool>.Success(true, "Recipients added");
        }
        catch (Exception ex)
        {
            await _uow.RollbackTransaction(cancellationToken);
            return ApiResponse<bool>.Failure(new[] { ex.Message });
        }
    }
}
