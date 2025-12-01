using MediatR;
using VotingSurvey.Application.Models;

namespace VotingSurvey.Application.UseCases.Voting.Commands;

public record AddRecipients : IRequest<ApiResponse<bool>>
{
    public Guid VotingId { get; init; }
    public IReadOnlyCollection<Guid> RecipientIds { get; init; } = Array.Empty<Guid>();
}
