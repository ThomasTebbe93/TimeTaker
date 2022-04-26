using API.BLL.UseCases.DutyHoursManagement.Entities;
using FluentValidation;

namespace API.BLL.UseCases.DutyHoursManagement.Validation
{
    public class DutyHoursRestEntityValidator : AbstractValidator<DutyHoursRestEntity>
    {
        public DutyHoursRestEntityValidator()
        {
            RuleFor(x => x.Start)
                .NotNull().WithMessage("validation.error.notNull")
                .NotEmpty().WithMessage("validation.error.notEmpty");
            RuleFor(x => x.End)
                .NotNull().WithMessage("validation.error.notNull")
                .NotEmpty().WithMessage("validation.error.notEmpty");
            RuleFor(x => x.End)
                .NotNull().WithMessage("validation.error.notNull")
                .NotEmpty().WithMessage("validation.error.notEmpty");
            RuleFor(x => x.End)
                .NotNull().WithMessage("validation.error.notNull")
                .NotEmpty().WithMessage("validation.error.notEmpty");
            RuleFor(x => x)
                .Custom((dutyHour, context) =>
                    {
                        if (dutyHour.Start > dutyHour.End)
                            context.AddFailure("End", "validation.error.endCannotBeBeforeStart");
                    }
                );
        }
    }
}