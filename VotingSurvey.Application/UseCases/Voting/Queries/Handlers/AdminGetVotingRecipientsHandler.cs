using MediatR;
using VotingSurvey.Application.Models;
using VotingSurvey.Application.Repositories;

namespace VotingSurvey.Application.UseCases.Voting.Queries.Handlers;

internal class AdminGetVotingRecipientsHandler : IRequestHandler<AdminGetVotingRecipients, ApiResponse<IReadOnlyList<Guid>>>
{
    private readonly IVoting _votingRepo;
    private readonly IUser _userRepo;

    public AdminGetVotingRecipientsHandler(IVoting votingRepo, IUser userRepo)
    {
        _votingRepo = votingRepo;
        _userRepo = userRepo;
    }

    public async Task<ApiResponse<IReadOnlyList<Guid>>> Handle(AdminGetVotingRecipients request, CancellationToken cancellationToken)
    {
        var isAdmin = await _userRepo.HasRoleAsync(request.AdminId, "ADMIN", cancellationToken);
        if (!isAdmin) return ApiResponse<IReadOnlyList<Guid>>.Failure(new[] { "Only ADMIN can view recipients" });
        var recipients = await _votingRepo.ListRecipientsAsync(request.VotingId, cancellationToken);
        return ApiResponse<IReadOnlyList<Guid>>.Success(recipients);
    }
}
