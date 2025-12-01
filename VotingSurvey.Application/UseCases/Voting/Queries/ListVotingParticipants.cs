using MediatR;
using VotingSurvey.Application.Models;

namespace VotingSurvey.Application.UseCases.Voting.Queries;

public record ListVotingParticipants : IRequest<ApiResponse<IReadOnlyList<Guid>>>
{
    public Guid VotingId { get; init; }
}
