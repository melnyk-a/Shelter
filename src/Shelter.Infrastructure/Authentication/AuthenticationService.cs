using Shelter.Application.Abstractions.Authentication;
using Shelter.Domain.Users;
using Shelter.Infrastructure.Authentication.Models;
using System.Net.Http.Json;

namespace Shelter.Infrastructure.Authentication;

internal sealed class AuthenticationService : IAuthenticationService
{
    private const string PassworkCredentialType = "password";

    private readonly HttpClient _httpClient;

    public AuthenticationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> RegisterAsync(
        User user,
        string password,
        CancellationToken cancellationToken = default)
    {
        var userRepresetationModel = UserRepresentationModel.FromUser(user);

        userRepresetationModel.Credentials =
        [
            new()
            {
                Value = password,
                Temporary=false,
                Type = PassworkCredentialType
            }
        ];

        var response = await _httpClient.PostAsJsonAsync(
            "users",
            userRepresetationModel,
            cancellationToken);

        return ExtractIdentityFromLocationHeader(response);
    }

    private static string ExtractIdentityFromLocationHeader(
        HttpResponseMessage httpResponseMessage)
    {
        const string usersSegmentName = "users/";

        var locationHeader = httpResponseMessage.Headers.Location?.PathAndQuery;

        if (locationHeader is null)
        {
            throw new InvalidOperationException("Location header can't be null");
        }

        var userSegmentValueIndex = locationHeader.IndexOf(
            usersSegmentName,
            StringComparison.InvariantCultureIgnoreCase);

        var userIdentityId = locationHeader.Substring(
            userSegmentValueIndex + usersSegmentName.Length);

        return userIdentityId;
    }
}
