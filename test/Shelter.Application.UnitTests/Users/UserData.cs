﻿using Shelter.Domain.Users;

namespace Shelter.Application.UnitTests.Users;

internal static class UserData
{
    public static User Create() => User.Create(FirstName, LastName, Email);

    public static readonly FirstName FirstName = new("First");
    public static readonly LastName LastName = new("Last");
    public static readonly Email Email = new("test@test.com");
}
