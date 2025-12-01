using FluentValidation;
using VotingSurvey.Domain.ValueObjects;

namespace VotingSurvey.Application.UseCases.Vote.Commands.Validations;

internal sealed class SelectVoteValidation : AbstractValidator<SelectVote>
{
    public SelectVoteValidation()
    {
        RuleFor(x => x.VotingId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Option)
            .IsInEnum().WithMessage("Option must be YES or NO")
            .Must(opt => opt == VoteOption.Yes || opt == VoteOption.No);
    }
}
