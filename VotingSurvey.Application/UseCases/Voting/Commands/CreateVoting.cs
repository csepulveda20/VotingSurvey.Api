using MediatR;
using VotingSurvey.Application.Models;

namespace VotingSurvey.Application.UseCases.Voting.Commands
{
    public record CreateVoting : IRequest<ApiResponse<Guid>>
    {
        public required string Title { get; init; }
        public required string Description { get; init; }
        public required string Question { get; init; }
        public required Guid CreatedById { get; init; }
        public required List<string> Options { get; init; }
        public DateTimeOffset StartDate { get; init; }
        public DateTimeOffset EndDate { get; init; }
    }
}
