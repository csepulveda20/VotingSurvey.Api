using FluentValidation;

namespace VotingSurvey.Application.UseCases.Voting.Commands.Validations;

internal sealed class EditVotingBeforeStartValidation : AbstractValidator<EditVotingBeforeStart>
{
    public EditVotingBeforeStartValidation()
    {
        RuleFor(x => x.VotingId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.StartDate).NotEmpty();
        RuleFor(x => x.EndDate).NotEmpty().GreaterThan(x => x.StartDate);
    }
}
