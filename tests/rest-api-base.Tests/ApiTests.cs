using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using rest_api_base.Models;

namespace rest_api_base.Tests;

public sealed class ApiTests
{
    [Fact]
    public async Task HealthEndpoint_ReturnsOk()
    {
        using var factory = CreateFactory();
        using var client = CreateClient(factory);
        var cancellationToken = TestContext.Current.CancellationToken;

        var response = await client.GetAsync("/health", cancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task SamplePizzaEndpoint_ReturnsSeededPizza()
    {
        using var factory = CreateFactory();
        using var client = CreateClient(factory);
        var cancellationToken = TestContext.Current.CancellationToken;

        var pizza = await client.GetFromJsonAsync<Pizza>("/Pizza/1", cancellationToken);

        Assert.NotNull(pizza);
        Assert.Equal(1, pizza.Id);
        Assert.Equal("Classic Italian", pizza.Name);
    }

    [Fact]
    public async Task PizzaEndpoints_SupportCrudFlow()
    {
        using var factory = CreateFactory();
        using var client = CreateClient(factory);
        var cancellationToken = TestContext.Current.CancellationToken;

        var createResponse = await client.PostAsJsonAsync(
            "/Pizza",
            new PizzaCreateRequest("Hawaii", false),
            cancellationToken
        );
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var createdPizza = await createResponse.Content.ReadFromJsonAsync<Pizza>(cancellationToken);
        Assert.NotNull(createdPizza);
        Assert.True(createdPizza.Id > 0);

        var updateResponse = await client.PutAsJsonAsync(
            $"/Pizza/{createdPizza.Id}",
            new PizzaUpdateRequest("Hawaiian", false),
            cancellationToken
        );
        Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);

        var updatedPizza = await client.GetFromJsonAsync<Pizza>(
            $"/Pizza/{createdPizza.Id}",
            cancellationToken
        );
        Assert.NotNull(updatedPizza);
        Assert.Equal("Hawaiian", updatedPizza.Name);

        var deleteResponse = await client.DeleteAsync(
            $"/Pizza/{createdPizza.Id}",
            cancellationToken
        );
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var missingResponse = await client.GetAsync(
            $"/Pizza/{createdPizza.Id}",
            cancellationToken
        );
        Assert.Equal(HttpStatusCode.NotFound, missingResponse.StatusCode);
    }

    [Fact]
    public async Task SwaggerEndpoint_IsAvailableInDevelopment()
    {
        using var factory = CreateFactory();
        using var client = CreateClient(factory);
        var cancellationToken = TestContext.Current.CancellationToken;

        var response = await client.GetAsync("/swagger/index.html", cancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private static WebApplicationFactory<Program> CreateFactory() =>
        new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            builder.UseEnvironment("Development")
        );

    private static HttpClient CreateClient(WebApplicationFactory<Program> factory) =>
        factory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("https://localhost"),
            }
        );
}
