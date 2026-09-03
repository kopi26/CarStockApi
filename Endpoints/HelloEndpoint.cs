using FastEndpoints;

namespace CarStockApi.Endpoints;

public class HelloEndpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/hello");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await Send.StringAsync("Hello from Car Stock API");
    }
}