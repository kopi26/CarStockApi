using CarStockApi.Data.Repositories;
using CarStockApi.Models;
using FastEndpoints;

namespace CarStockApi.Endpoints.Cars
{
    public class UpdateStockEndpoint : Endpoint<UpdateStockRequest>
    {
        private readonly CarRepository _repository;

        public UpdateStockEndpoint(CarRepository repository)
        {
            _repository = repository;
        }

        public override void Configure()
        {
            Put("/cars/{id}/stock");
        }

        public override async Task HandleAsync(UpdateStockRequest req, CancellationToken ct)
        {
            var carId = Route<int>("id");

            var dealerClaim = User.FindFirst("DealerId");

            if (dealerClaim is null)
            {
                await Send.UnauthorizedAsync(ct);
                return;
            }

            var dealerId = int.Parse(dealerClaim.Value);

            var updated = await _repository.UpdateStockAsync(carId, req.Stock, dealerId);

            if (!updated)
            {
                await Send.NotFoundAsync(ct);
                return;
            }

            await Send.OkAsync(
                new
                {
                    Message = "Stock updated successfully"
                }, ct);
        }
    }
}
