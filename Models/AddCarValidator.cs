using FastEndpoints;
using FluentValidation;

namespace CarStockApi.Models
{
    public class AddCarValidator : Validator<AddCarRequest>
    {
        public AddCarValidator()
        {
            RuleFor(x => x.Make)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Model)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Year)
                .ExclusiveBetween(1900 , DateTime.UtcNow.Year + 1);

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0);
        }
    }
}
