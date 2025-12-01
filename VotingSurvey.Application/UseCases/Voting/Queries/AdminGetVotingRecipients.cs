using MediatR;
using VotingSurvey.Application.Models;

namespace VotingSurvey.Application.UseCases.Voting.Queries;

public record AdminGetVotingRecipients : IRequest<ApiResponse<IReadOnlyList<Guid>>>
{
    public Guid VotingId { get; init; }
    public Guid AdminId { get; init; }
}
