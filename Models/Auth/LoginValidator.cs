using FastEndpoints;
using FluentValidation;

namespace CarStockApi.Models.Auth
{
    public class LoginValidator : Validator<LoginRequest>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Username)
                    .NotEmpty();

            RuleFor(x => x.Password)
                    .NotEmpty();
        }
    }
}
