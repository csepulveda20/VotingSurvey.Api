using MediatR;
using VotingSurvey.Application.Models;

namespace VotingSurvey.Application.UseCases.Voting.Queries;

public record AdminDashboardSummary : IRequest<ApiResponse<(int upcoming, int active, int closed)>>
{
    public Guid AdminId { get; init; }
}
