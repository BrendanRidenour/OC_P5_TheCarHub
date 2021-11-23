using FluentValidation;

namespace TheCarHub
{
    public class CarPocoValidator : AbstractValidator<CarPoco>
    {
        public CarPocoValidator(ISystemClock clock)
        {
            if (clock is null)
                throw new ArgumentNullException(nameof(clock));

            RuleFor(m => m.Year)
                .InclusiveBetween(1990, clock.UtcNow.Year);

            RuleFor(m => m.Make)
                .NotEmpty();

            RuleFor(m => m.Model)
                .NotEmpty();

            RuleFor(m => m.Trim)
                .NotEmpty();

            RuleFor(m => m.PurchasePrice)
                .GreaterThanOrEqualTo(0);

            RuleFor(m => m.RepairCost)
                .GreaterThanOrEqualTo(0);

            RuleFor(m => m.LotDate)
                .Custom((date, context) =>
                {
                    if (!date.HasValue) return;

                    if (date.Value < context.InstanceToValidate.PurchaseDate)
                    {
                        context.AddFailure("The Lot Date cannot come before the Purchase Date.");
                    }
                });

            RuleFor(m => m.SaleDate)
                .Custom((date, context) =>
                {
                    if (!date.HasValue) return;

                    if (date.Value < context.InstanceToValidate.LotDate)
                    {
                        context.AddFailure("The Lot Date cannot come before the Purchase Date.");
                    }
                });
        }
    }
}