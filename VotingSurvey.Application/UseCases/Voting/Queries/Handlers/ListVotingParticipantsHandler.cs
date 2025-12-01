using MediatR;
using VotingSurvey.Application.Models;
using VotingSurvey.Application.Repositories;

namespace VotingSurvey.Application.UseCases.Voting.Queries.Handlers;

internal class ListVotingParticipantsHandler : IRequestHandler<ListVotingParticipants, ApiResponse<IReadOnlyList<Guid>>>
{
    private readonly IVoting _votingRepo;

    public ListVotingParticipantsHandler(IVoting votingRepo)
    {
        _votingRepo = votingRepo;
    }

    public async Task<ApiResponse<IReadOnlyList<Guid>>> Handle(ListVotingParticipants request, CancellationToken cancellationToken)
    {
        var yes = await _votingRepo.ListYesVotersAsync(request.VotingId, cancellationToken);
        var no = await _votingRepo.ListNoVotersAsync(request.VotingId, cancellationToken);
        var merged = yes.Concat(no).Distinct().ToList();
        return ApiResponse<IReadOnlyList<Guid>>.Success(merged);
    }
}
