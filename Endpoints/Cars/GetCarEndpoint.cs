using CarStockApi.Data.Repositories;
using CarStockApi.Models;
using FastEndpoints;

namespace CarStockApi.Endpoints.Cars
{
    public class GetCarEndpoint : EndpointWithoutRequest<Car>
    {
        private readonly CarRepository _repository;

        public GetCarEndpoint(CarRepository repository)
        {
            _repository = repository;
        }

        public override void Configure()
        {
            Get("/cars/{id}");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var carId = Route<int>("id");
            var dealerClaim = User.FindFirst("DealerId");

            if (dealerClaim is null)
            {
                await Send.UnauthorizedAsync(ct);
                return;
            }

            var dealerId = int.Parse(dealerClaim.Value);

            var car = await _repository.GetByIdAsync(carId, dealerId);

            if (car is null)
            {
                await Send.NotFoundAsync(ct);
                return;
            }

            await Send.OkAsync(car, ct);

        }
    }
}
