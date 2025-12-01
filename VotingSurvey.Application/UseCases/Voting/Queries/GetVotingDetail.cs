using MediatR;
using VotingSurvey.Application.Models;
using VotingSurvey.Application.UseCases.Voting.Dtos;

namespace VotingSurvey.Application.UseCases.Voting.Queries;

public record GetVotingDetail : IRequest<ApiResponse<VotingDetailDto>>
{
    public Guid VotingId { get; init; }
    public Guid? UserId { get; init; }
}
