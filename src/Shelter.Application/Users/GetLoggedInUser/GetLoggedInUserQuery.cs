using Shelter.Application.Abstractions.Messaging;

namespace Shelter.Application.Users.GetLoggedInUser;

public sealed record GetLoggedInUserQuery : IQuery<UserResponse>;