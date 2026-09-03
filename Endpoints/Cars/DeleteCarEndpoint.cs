using CarStockApi.Data.Repositories;
using FastEndpoints;

namespace CarStockApi.Endpoints.Cars
{
    public class DeleteCarEndpoint : EndpointWithoutRequest
    {
        private readonly CarRepository _repository;

        public DeleteCarEndpoint(CarRepository repository)
        {
            _repository = repository;
        }

        public override void Configure()
        {
            Delete("/cars/{id}");
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

            var deleted = await _repository.DeleteAsync(carId, dealerId);

            if(!deleted)
            {
                await Send.NotFoundAsync(ct);
                return;
            }

            await Send.OkAsync(
                new
                {
                    Message = "Car deleted successfully"
                }, ct);
        }
    }
}
