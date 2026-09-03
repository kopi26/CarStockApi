using CarStockApi.Data.Repositories;
using CarStockApi.Models.Auth;
using FastEndpoints;
using FastEndpoints.Security;

namespace CarStockApi.Endpoints.Auth
{
    public class LoginEndpoint : Endpoint<LoginRequest>
    {
        private readonly DealerRepository _repository;

        public LoginEndpoint(DealerRepository repository)
        {
            _repository = repository;
        }
        public override void Configure()
        {
            Post("/auth/login");
            AllowAnonymous();
        }

        public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
        {
            var dealer = await _repository.GetByUsernameAsync(req.Username);

            if (dealer is null ||
                !BCrypt.Net.BCrypt.Verify(req.Password, dealer.PasswordHash))
            {
                await Send.UnauthorizedAsync(ct);
                return;
            }

            var token = JwtBearer.CreateToken(options =>
            {
                options.ExpireAt = DateTime.UtcNow.AddHours(2);

                options.User["DealerId"] = dealer.DealerId.ToString();
                options.User["Username"] = dealer.Username;
            });

            await Send.OkAsync(
                new
                {
                    token = token,
                    message = "Login successful"
                }, ct);
        }
    }
}
