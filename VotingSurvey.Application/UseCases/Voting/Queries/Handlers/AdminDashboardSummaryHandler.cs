using MediatR;
using VotingSurvey.Application.Models;
using VotingSurvey.Application.Repositories;

namespace VotingSurvey.Application.UseCases.Voting.Queries.Handlers;

internal class AdminDashboardSummaryHandler : IRequestHandler<AdminDashboardSummary, ApiResponse<(int upcoming, int active, int closed)>>
{
    private readonly IVoting _votingRepo;
    private readonly IUser _userRepo;

    public AdminDashboardSummaryHandler(IVoting votingRepo, IUser userRepo)
    {
        _votingRepo = votingRepo;
        _userRepo = userRepo;
    }

    public async Task<ApiResponse<(int upcoming, int active, int closed)>> Handle(AdminDashboardSummary request, CancellationToken cancellationToken)
    {
        var isAdmin = await _userRepo.HasRoleAsync(request.AdminId, "ADMIN", cancellationToken);
        if (!isAdmin) return ApiResponse<(int upcoming, int active, int closed)>.Failure(new[] { "Only ADMIN can view dashboard" });
        var now = DateTimeOffset.UtcNow;
        var all = await _votingRepo.ListByCreatorAsync(request.AdminId, new QueryParam(), cancellationToken);
        var upcoming = all.Count(v => now < v.Window.StartsAt);
        var active = all.Count(v => now >= v.Window.StartsAt && now <= v.Window.EndsAt);
        var closed = all.Count(v => now > v.Window.EndsAt);
        return ApiResponse<(int upcoming, int active, int closed)>.Success((upcoming, active, closed));
    }
}
