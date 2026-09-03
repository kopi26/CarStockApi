using CarStockApi.Data.Repositories;
using CarStockApi.Models;
using FastEndpoints;

namespace CarStockApi.Endpoints.Cars
{
    public class SearchCarsEndpoint : EndpointWithoutRequest<IEnumerable<Car>>
    {
        private readonly CarRepository _repository;

        public SearchCarsEndpoint(CarRepository repository)
        {
            _repository = repository;
        }

        public override void Configure()
        {
            Get("/cars/search");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var make = Query<string>("make");
            var model = Query<string>("model");

            if (string.IsNullOrWhiteSpace(make) || string.IsNullOrWhiteSpace(model))
            {
                await Send.StringAsync(
                    "Make and model are required.",
                    400,
                    cancellation: ct);

                return;
            }

            var dealerClaim = User.FindFirst("DealerId");

            if (dealerClaim is null)
            {
                await Send.UnauthorizedAsync(ct);
                return;
            }

            var dealerId = int.Parse(dealerClaim.Value);

            var cars = await _repository.SearchAsync(make, model, dealerId);
            await Send.OkAsync(cars, ct);
        }
    }
}
