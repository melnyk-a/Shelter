﻿using Shelter.Api.FunctionalTests.Infrastructure;
using Shelter.Application.Users.GetLoggedInUser;

namespace Shelter.Api.FunctionalTests.Users;

public class GetLoggedInUserTests : BaseFunctionalTest
{
    public GetLoggedInUserTests(FunctionalTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Get_ShouldReturnUnauthorized_WhenAccessTokenIsMissing()
    {
        // Act
        var response = await HttpClient.GetAsync("api/v1/users/me");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Get_ShouldReturnUser_WhenAccessTokenIsNotMissing()
    {
        // Arrange
        var accessToken = await GetAccessToken();
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            JwtBearerDefaults.AuthenticationScheme,
            accessToken);

        // Act
        var user = await HttpClient.GetFromJsonAsync<UserResponse>("api/v1/users/me");

        // Assert
        user.Should().NotBeNull();
    }
}
