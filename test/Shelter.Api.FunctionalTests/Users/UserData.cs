using Shelter.Api.Controllers.Users;

namespace Shelter.Api.FunctionalTests.Users;

internal static class UserData
{
    public static RegisterUserRequest RegisterTestUserRequest = new("test@test.com", "test", "test", "12345");
}
