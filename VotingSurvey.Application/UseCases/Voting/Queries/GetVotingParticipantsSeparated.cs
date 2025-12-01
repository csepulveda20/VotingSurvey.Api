using MediatR;
using VotingSurvey.Application.Models;

namespace VotingSurvey.Application.UseCases.Voting.Queries;

public record GetVotingParticipantsSeparated : IRequest<ApiResponse<(IReadOnlyList<Guid> yes, IReadOnlyList<Guid> no)>>
{
    public Guid VotingId { get; init; }
}
