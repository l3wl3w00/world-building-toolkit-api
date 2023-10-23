using Bll.Planet.Dto;
using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Net;
using System.Transactions;
using Dal.Entities;
using FluentAssertions;

namespace Test.Integration.Planet;

public class PlanetControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly WebApplicationFactory<Program> _appFactory;

    public PlanetControllerTests(CustomWebApplicationFactory appFactory)
    {
        _appFactory = appFactory;
    }

    [Fact]
    public async Task DemoTest() 
    {
        // Arrange
        _appFactory.Server.PreserveExecutionContext = true;
        using var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        var client = _appFactory.CreateClient();
        var dto = new Faker<CreatePlanetDto>()
            .CustomInstantiator(f =>
                new
                (
                    Name: "test planet", 
                    Description: "test description",
                    LandColor: new Color(0,0,0,0),
                    AntiLandColor: new Color(0,0,0,0)
                )
            )
            .Generate();
        // Act
        var response = await client.PostAsJsonAsync("/api/planets", dto);
        var responseDto = await response.Content.ReadFromJsonAsync<PlanetDto>();
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
