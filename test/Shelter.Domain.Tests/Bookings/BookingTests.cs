using Shelter.Domain.Bookings.Events;
using Shelter.Domain.Bookings;
using Shelter.Domain.Shared;
using Shelter.Domain.Users;
using Shelter.Domain.UnitTests.Infrastructure;
using Shelter.Domain.UnitTests.Users;
using Shelter.Domain.UnitTests.PetSitters;

namespace Shelter.Domain.UnitTests.Bookings;

public class BookingTests : BaseTest
{
    [Fact]
    public void Reserve_Should_RaiseBookingReservedDomainEvent()
    {
        // Arrange
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);
        var price = new Money(10.0m, Currency.Usd);
        var duration = DateRange.Create(new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 10));
        var petSitter = PetSitterData.Create(price);
        var pricingService = new PricingService();

        // Act
        var booking = Booking.Reserve(petSitter, user.Id, duration, DateTime.UtcNow, pricingService);

        // Assert
        var bookingReservedDomainEvent = AssertDomainEventWasPublished<BookingReservedDomainEvent>(booking);

        bookingReservedDomainEvent.BookingId.Should().Be(booking.Id);
    }
}
