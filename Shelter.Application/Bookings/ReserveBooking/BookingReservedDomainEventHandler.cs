using MediatR;
using Shelter.Application.Abstractions.Email;
using Shelter.Domain.Bookings;
using Shelter.Domain.Bookings.Events;
using Shelter.Domain.Users;

namespace Shelter.Application.Bookings.ReserveBooking;

internal sealed class BookingReservedDomainEventHandler : INotificationHandler<BookingRejectedDomainEvent>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;

    public BookingReservedDomainEventHandler(
        IBookingRepository bookingRepository,
        IUserRepository userRepository,
        IEmailService emailService)
    {
        this._bookingRepository = bookingRepository;
        this._userRepository = userRepository;
        this._emailService = emailService;
    }
    public async Task Handle(BookingRejectedDomainEvent notification, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(notification.BookingId, cancellationToken);

        if (booking is null)
        {
            return;
        }

        var user = await _userRepository.GetByIdAsync(booking.UserId, cancellationToken);

        if (user is null)
        {
            return;
        }

        await _emailService.SendAsync(
            user.Email,
            "Booking reserved",
            "You have 10 minutes to confirm this booking");
    }
}
