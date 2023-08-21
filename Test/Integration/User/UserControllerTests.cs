using System.Net;
using System.Net.Http.Json;
using System.Transactions;
using Bll.Auth.Dto;
using Bll.Planet.Dto;
using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Test.Integration.User;

public class UserControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly WebApplicationFactory<Program> _appFactory;

    public UserControllerTests(CustomWebApplicationFactory appFactory)
    {
        _appFactory = appFactory;
    }

    [Fact]
    public async Task TestRegister() 
    {
        // Arrange
        _appFactory.Server.PreserveExecutionContext = true;
        using var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        var client = _appFactory.CreateClient();
        
        var dto = new Faker<RegisterDto>()
            .CustomInstantiator(f =>
                new RegisterDto 
                (
                    Email: "testemail@gmail.com", 
                    Username: "TestUserName",
                    Password: "test pwd"
                )
            )
            .Generate();
        // Act
        var response = await client.PostAsJsonAsync("/api/register", dto);
        var responseDto = await response.Content.ReadFromJsonAsync<RegisterDto>();
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        
    }
}