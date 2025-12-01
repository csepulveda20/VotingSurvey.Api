using MediatR;
using VotingSurvey.Application.Models;
using VotingSurvey.Application.Repositories;
using VotingSurvey.Application.UseCases.Voting.Dtos;

namespace VotingSurvey.Application.UseCases.Voting.Queries.Handlers;

internal sealed class GetVotingDetailHandler : IRequestHandler<GetVotingDetail, ApiResponse<VotingDetailDto>>
{
    private readonly IVoting _votingRepo;

    public GetVotingDetailHandler(IVoting votingRepo)
    {
        _votingRepo = votingRepo;
    }

    public async Task<ApiResponse<VotingDetailDto>> Handle(GetVotingDetail request, CancellationToken cancellationToken)
    {
        var voting = await _votingRepo.GetByIdAsync(request.VotingId, cancellationToken);
        if (voting is null) return ApiResponse<VotingDetailDto>.Failure(new[] { "Voting not found" });

        var (yes, no) = voting.TallyConfirmed();
        var dto = new VotingDetailDto
        {
            Id = voting.Id,
            Title = voting.Title,
            Description = voting.QuestionDescription,
            StartAt = voting.Window.StartsAt,
            EndAt = voting.Window.EndsAt,
            State = voting.Window.IsActive(DateTimeOffset.UtcNow) ? "Active" : (!voting.Window.HasStarted(DateTimeOffset.UtcNow) ? "Upcoming" : "Closed"),
            YesCount = yes,
            NoCount = no,
            YourVote = request.UserId.HasValue ? voting.Votes.FirstOrDefault(v => v.UserId == request.UserId.Value)?.Option switch
            {
                Domain.ValueObjects.VoteOption.Yes => "YES",
                Domain.ValueObjects.VoteOption.No => "NO",
                _ => null
            } : null
        };

        return ApiResponse<VotingDetailDto>.Success(dto);
    }
}
