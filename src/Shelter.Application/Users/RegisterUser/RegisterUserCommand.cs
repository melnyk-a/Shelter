using Shelter.Application.Abstractions.Messaging;

namespace Shelter.Application.Users.RegisterUser;

public sealed record RegisterUserCommand(
    string Email,
    string FirstName,
    string LastName,
    string Password) : ICommand<Guid>;
