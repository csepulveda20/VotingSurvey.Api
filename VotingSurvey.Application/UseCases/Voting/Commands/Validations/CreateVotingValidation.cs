using FluentValidation;

namespace VotingSurvey.Application.UseCases.Voting.Commands.Validations
{
    internal class CreateVotingValidation : AbstractValidator<CreateVoting>
    {
        public CreateVotingValidation()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(200);

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .MaximumLength(1000);

            RuleFor(x => x.Question)
                .Equal("YES_NO").WithMessage("Question must be YES_NO");

            RuleFor(x => x.CreatedById)
                .NotEmpty().WithMessage("CreatedById is required");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("StartDate is required");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("EndDate is required")
                .GreaterThan(x => x.StartDate).WithMessage("EndDate must be after StartDate");

            RuleFor(x => x.Options)
                .NotNull().WithMessage("Options are required")
                .Must(o => o.Count == 2 && o.Contains("YES") && o.Contains("NO"))
                .WithMessage("Options must include YES and NO only");
        }
    }
}
