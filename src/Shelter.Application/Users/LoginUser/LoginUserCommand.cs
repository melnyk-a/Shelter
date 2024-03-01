using Shelter.Application.Abstractions.Messaging;

namespace Shelter.Application.Users.LoginUser;

public sealed record LoginUserCommand(
    string Email,
    string Password) : ICommand<AccessTokenResponse>;
