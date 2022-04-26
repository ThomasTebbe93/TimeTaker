using API.BLL.UseCases.DutyHoursManagement.Entities;
using FluentValidation;

namespace API.BLL.UseCases.DutyHoursManagement.Validation
{
    public class DutyHoursBookingValidator : AbstractValidator<DutyHoursBooking>
    {
        public DutyHoursBookingValidator(DutyHoursBooking lastBooking)
        {
            RuleFor(x => x.UserIdent)
                .NotNull().WithMessage("validation.error.notNull")
                .NotEmpty().WithMessage("validation.error.notEmpty");
            RuleFor(x => x.CreatorIdent)
                .NotNull().WithMessage("validation.error.notNull")
                .NotEmpty().WithMessage("validation.error.notEmpty");
            RuleFor(x => x.BookingTime)
                .NotNull().WithMessage("validation.error.notNull")
                .NotEmpty().WithMessage("validation.error.notEmpty");
            RuleFor(x => x)
                .Custom((booking, context) =>
                    {
                        if (lastBooking?.IsSignedIn == booking.IsSignedIn)
                            context.AddFailure($"{booking.UserIdent}", "validation.error.AlreadySinged");
                    }
                );
        }
    }
}