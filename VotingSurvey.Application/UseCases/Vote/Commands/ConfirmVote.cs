using MediatR;
using VotingSurvey.Application.Models;

namespace VotingSurvey.Application.UseCases.Vote.Commands;

public record ConfirmVote : IRequest<ApiResponse<bool>>
{
    public Guid VotingId { get; init; }
    public Guid UserId { get; init; }
}
