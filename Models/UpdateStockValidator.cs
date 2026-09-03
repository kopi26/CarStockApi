using FastEndpoints;
using FluentValidation;

namespace CarStockApi.Models
{
    public class UpdateStockValidator : Validator<UpdateStockRequest>
    {
        public UpdateStockValidator()
        {
            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0);
        }
    }
}
