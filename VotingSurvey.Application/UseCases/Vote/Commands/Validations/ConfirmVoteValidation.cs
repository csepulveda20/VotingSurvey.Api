using FluentValidation;

namespace VotingSurvey.Application.UseCases.Vote.Commands.Validations;

internal sealed class ConfirmVoteValidation : AbstractValidator<ConfirmVote>
{
    public ConfirmVoteValidation()
    {
        RuleFor(x => x.VotingId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}
