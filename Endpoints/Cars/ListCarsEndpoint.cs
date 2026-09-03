using CarStockApi.Data.Repositories;
using CarStockApi.Models;
using FastEndpoints;

namespace CarStockApi.Endpoints.Cars;

public class ListCarsEndpoint : EndpointWithoutRequest<IEnumerable<Car>>
{
    private readonly CarRepository _repository;

    public ListCarsEndpoint(CarRepository repository)
    {
        _repository = repository;
    }

    public override void Configure()
    {
        Get("/cars");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var dealerClaim = User.FindFirst("DealerId");

        if(dealerClaim is null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var dealerId = int.Parse(dealerClaim.Value);

        var cars = await _repository.GetAllAsync(dealerId);

        await Send.OkAsync(cars, ct);
    }
}