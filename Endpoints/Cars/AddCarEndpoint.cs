using CarStockApi.Data.Repositories;
using CarStockApi.Models;
using FastEndpoints;

namespace CarStockApi.Endpoints.Cars;

public class AddCarEndpoint : Endpoint<AddCarRequest, object>
{
    private readonly CarRepository _repository;

    public AddCarEndpoint(CarRepository repository)
    {
        _repository = repository;
    }

    public override void Configure()
    {
        Post("/cars");
    }

    public override async Task HandleAsync(AddCarRequest req, CancellationToken ct)
    {
        var dealerClaim = User.FindFirst("DealerId");

        if (dealerClaim is null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var dealerId = int.Parse(dealerClaim.Value);

        var car = new Car
        {
            DealerId = dealerId,
            Make = req.Make,
            Model = req.Model,
            Year = req.Year,
            Stock = req.Stock,

        };

        var carId = await _repository.AddAsync(car);

        await Send.OkAsync(new
        {
            CarId = carId,
            Message = "Car added successfully"
        }, ct);
    }
}