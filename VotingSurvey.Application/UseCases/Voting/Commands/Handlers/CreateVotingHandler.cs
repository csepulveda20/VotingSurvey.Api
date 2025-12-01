using MediatR;
using VotingSurvey.Application.Models;
using VotingSurvey.Application.Repositories;
using VotingSurvey.Application.Services;
using Entity = VotingSurvey.Domain.Entities;
using VotingSurvey.Domain.ValueObjects;

namespace VotingSurvey.Application.UseCases.Voting.Commands.Handlers
{
    internal class CreateVotingHandler : IRequestHandler<CreateVoting, ApiResponse<Guid>>
    {
        private readonly IVoting _votingRepo;
        private readonly IVotingRecipient _recipientRepo;
        private readonly IUser _userRepo;
        private readonly IUnitOfWork _uow;

        public CreateVotingHandler(IVoting votingRepo, IVotingRecipient recipientRepo, IUser userRepo, IUnitOfWork uow)
        {
            _votingRepo = votingRepo;
            _recipientRepo = recipientRepo;
            _userRepo = userRepo;
            _uow = uow;
        }

        public async Task<ApiResponse<Guid>> Handle(CreateVoting request, CancellationToken cancellationToken)
        {
            // Validate creator is ADMIN via repository (placeholder, implement HasRoleAsync)
            var isAdmin = await _userRepo.HasRoleAsync(request.CreatedById, "ADMIN", cancellationToken);
            if (!isAdmin)
            {
                return ApiResponse<Guid>.Failure(["Only ADMIN can create votings"]);
            }

            // Build aggregate
            var voting = BuildVoting(request);

            try
            {
                await _uow.BeginTransaction(cancellationToken);

                await _votingRepo.CreateAsync(voting, cancellationToken);
                // Destinatarios: en este punto request.Options representa YES/NO; no destinatarios. Agregar recipients si se proveen en DTOs futuros.

                await _uow.SaveChanges(cancellationToken);
                await _uow.CommitTransaction(cancellationToken);

                return ApiResponse<Guid>.Success(voting.Id, "Voting created");
            }
            catch (Exception ex)
            {
                await _uow.RollbackTransaction(cancellationToken);
                return ApiResponse<Guid>.Failure([ex.Message]);
            }
        }

        private static Entity.Voting BuildVoting(CreateVoting request)
        {
            var window = VotingWindow.Create(request.StartDate, request.EndDate);
            var voting = Entity.Voting.Create(Guid.Empty, request.CreatedById, request.Title, request.Description, window, DateTimeOffset.UtcNow);
            return voting;
        }
    }
}
