using MediatR;
using VotingSurvey.Application.Models;
using VotingSurvey.Application.Repositories;
using VotingSurvey.Application.UseCases.Voting.Dtos;

namespace VotingSurvey.Application.UseCases.Voting.Queries.Handlers;

internal sealed class ListByCreatorHandler : IRequestHandler<ListByCreator, ApiResponse<PaginationResponse<VotingListItemDto>>>
{
    private readonly IVoting _votingRepo;

    public ListByCreatorHandler(IVoting votingRepo)
    {
        _votingRepo = votingRepo;
    }

    public async Task<ApiResponse<PaginationResponse<VotingListItemDto>>> Handle(ListByCreator request, CancellationToken cancellationToken)
    {
        var items = await _votingRepo.ListByCreatorAsync(request.CreatorId, request.Query, cancellationToken);
        var now = DateTimeOffset.UtcNow;
        var q = request.Query;

        var projection = items.Select(v => new VotingListItemDto
        {
            Id = v.Id,
            Title = v.Title,
            StartAt = v.Window.StartsAt,
            EndAt = v.Window.EndsAt,
            State = v.Window.IsActive(now) ? "Active" : (!v.Window.HasStarted(now) ? "Upcoming" : "Closed")
        }).AsQueryable();

        var page = await PaginationResponse<VotingListItemDto>.Create(projection, q.PageNumber, q.PageSize);
        return ApiResponse<PaginationResponse<VotingListItemDto>>.Success(page);
    }
}
