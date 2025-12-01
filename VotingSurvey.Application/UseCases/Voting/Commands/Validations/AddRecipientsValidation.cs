using FluentValidation;

namespace VotingSurvey.Application.UseCases.Voting.Commands.Validations;

internal sealed class AddRecipientsValidation : AbstractValidator<AddRecipients>
{
    public AddRecipientsValidation()
    {
        RuleFor(x => x.VotingId).NotEmpty();
        RuleFor(x => x.RecipientIds)
            .NotNull().WithMessage("RecipientIds required")
            .Must(list => list.Count > 0).WithMessage("At least one recipient is required");
    }
}
