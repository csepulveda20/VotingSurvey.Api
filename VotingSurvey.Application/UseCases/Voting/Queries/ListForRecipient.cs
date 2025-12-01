using MediatR;
using VotingSurvey.Application.Models;
using VotingSurvey.Application.UseCases.Voting.Dtos;

namespace VotingSurvey.Application.UseCases.Voting.Queries;

public record ListForRecipient : IRequest<ApiResponse<PaginationResponse<VotingListItemDto>>>
{
    public Guid UserId { get; init; }
    public QueryParam Query { get; init; } = new();
}
