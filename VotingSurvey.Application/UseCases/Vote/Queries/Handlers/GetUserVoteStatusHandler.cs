using MediatR;
using VotingSurvey.Application.Models;
using VotingSurvey.Application.Repositories;

namespace VotingSurvey.Application.UseCases.Vote.Queries.Handlers;

internal class GetUserVoteStatusHandler : IRequestHandler<GetUserVoteStatus, ApiResponse<string?>>
{
    private readonly IVoting _votingRepo;

    public GetUserVoteStatusHandler(IVoting votingRepo)
    {
        _votingRepo = votingRepo;
    }

    public async Task<ApiResponse<string?>> Handle(GetUserVoteStatus request, CancellationToken cancellationToken)
    {
        var voting = await _votingRepo.GetByIdAsync(request.VotingId, cancellationToken);
        if (voting is null) return ApiResponse<string?>.Failure(new[] { "Voting not found" });
        var vote = voting.Votes.FirstOrDefault(v => v.UserId == request.UserId);
        if (vote is null) return ApiResponse<string?>.Success(null, "User has not voted");
        var status = vote.Confirmed ? (vote.Option == Domain.ValueObjects.VoteOption.Yes ? "YES_CONFIRMED" : "NO_CONFIRMED") : (vote.Option == Domain.ValueObjects.VoteOption.Yes ? "YES_PENDING" : "NO_PENDING");
        return ApiResponse<string?>.Success(status);
    }
}
