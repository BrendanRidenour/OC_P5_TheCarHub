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
                .InclusiveBetween(1990, clock.UtcNow.Year + 1);

            RuleFor(m => m.Make)
                .NotEmpty();

            RuleFor(m => m.Model)
                .NotEmpty();

            RuleFor(m => m.Trim)
                .NotEmpty();

            RuleFor(m => m.PurchasePrice)
                .GreaterThanOrEqualTo(0);

            RuleFor(m => m.Repairs)
                .Custom((value, context) =>
                {
                    if (string.IsNullOrWhiteSpace(value) &&
                        context.InstanceToValidate.RepairCost > 0)
                    {
                        context.AddFailure("You added some Repair Cost, so be sure to also add the list of Repairs.");
                    }
                });

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

                    if (!context.InstanceToValidate.LotDate.HasValue)
                    {
                        context.AddFailure("In order to set a Sale Date, you must also set a Lot Date.");
                    }

                    if (date.Value < context.InstanceToValidate.PurchaseDate)
                    {
                        context.AddFailure("The Sale Date cannot come before the Purchase Date.");
                    }

                    if (date.Value < context.InstanceToValidate.LotDate)
                    {
                        context.AddFailure("The Sale Date cannot come before the Lot Date.");
                    }
                });

            RuleFor(m => m.Profit)
                .GreaterThanOrEqualTo(0);
        }
    }
}