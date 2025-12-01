using MediatR;
using VotingSurvey.Application.Models;
using VotingSurvey.Application.Repositories;

namespace VotingSurvey.Application.UseCases.Voting.Queries.Handlers;

internal class GetVotingParticipantsSeparatedHandler : IRequestHandler<GetVotingParticipantsSeparated, ApiResponse<(IReadOnlyList<Guid> yes, IReadOnlyList<Guid> no)>>
{
    private readonly IVoting _votingRepo;

    public GetVotingParticipantsSeparatedHandler(IVoting votingRepo)
    {
        _votingRepo = votingRepo;
    }

    public async Task<ApiResponse<(IReadOnlyList<Guid> yes, IReadOnlyList<Guid> no)>> Handle(GetVotingParticipantsSeparated request, CancellationToken cancellationToken)
    {
        var yes = await _votingRepo.ListYesVotersAsync(request.VotingId, cancellationToken);
        var no = await _votingRepo.ListNoVotersAsync(request.VotingId, cancellationToken);
        return ApiResponse<(IReadOnlyList<Guid> yes, IReadOnlyList<Guid> no)>.Success((yes, no));
    }
}
