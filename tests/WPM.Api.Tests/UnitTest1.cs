using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WPM.Infrastructure.Data;
using System.Net;
using System.Net.Http.Json;

namespace WPM.Api.Tests;

public class ApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly SqliteConnection _connection;

    public ApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        // Use a shared in-memory connection that stays open for the test lifetime
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Remove the real CoreDbContext registration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<CoreDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                // Use the shared connection so schema persists across requests
                services.AddDbContext<CoreDbContext>(options =>
                    options.UseSqlite(_connection));
            });
        });
    }

    [Fact]
    public async Task HealthEndpoint_ReturnsOk()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadFromJsonAsync<HealthResponse>();
        Assert.NotNull(content);
        Assert.Equal("healthy", content.Status);
    }

    [Fact]
    public async Task SitesEndpoint_ReturnsEmptyList()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/sites");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    public void Dispose()
    {
        _connection.Dispose();
    }

    private record HealthResponse(string Status, DateTime Timestamp);
}
