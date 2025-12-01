using MediatR;
using VotingSurvey.Application.Models;
using VotingSurvey.Domain.ValueObjects;

namespace VotingSurvey.Application.UseCases.Vote.Commands;

public record SelectVote : IRequest<ApiResponse<bool>>
{
    public Guid VotingId { get; init; }
    public Guid UserId { get; init; }
    public VoteOption Option { get; init; }
}
