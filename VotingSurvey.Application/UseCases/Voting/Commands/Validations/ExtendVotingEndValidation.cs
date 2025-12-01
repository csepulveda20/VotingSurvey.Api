using FluentValidation;

namespace VotingSurvey.Application.UseCases.Voting.Commands.Validations;

internal sealed class ExtendVotingEndValidation : AbstractValidator<ExtendVotingEnd>
{
    public ExtendVotingEndValidation()
    {
        RuleFor(x => x.VotingId).NotEmpty();
        RuleFor(x => x.NewEnd).NotEmpty();
    }
}
