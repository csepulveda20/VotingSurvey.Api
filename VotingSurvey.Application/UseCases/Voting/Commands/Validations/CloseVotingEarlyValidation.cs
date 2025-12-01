using FluentValidation;

namespace VotingSurvey.Application.UseCases.Voting.Commands.Validations;

internal sealed class CloseVotingEarlyValidation : AbstractValidator<CloseVotingEarly>
{
    public CloseVotingEarlyValidation()
    {
        RuleFor(x => x.VotingId).NotEmpty();
        RuleFor(x => x.AdminId).NotEmpty();
    }
}
