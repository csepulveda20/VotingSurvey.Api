using MediatR;
using VotingSurvey.Application.Models;

namespace VotingSurvey.Application.UseCases.Voting.Commands;

public record CloseVotingEarly : IRequest<ApiResponse<bool>>
{
    public Guid VotingId { get; init; }
    public Guid AdminId { get; init; }
}
