using Shelter.Domain.Abstractions;
using Shelter.Domain.Bookings.Events;
using Shelter.Domain.PetSitters;
using Shelter.Domain.Shared;

namespace Shelter.Domain.Bookings;

public sealed class Booking : Entity
{
    private Booking(
        Guid id,
        Guid petSitterId,
        Guid userId,
        DateRange duration,
        Money priceForPeriod,
        Money securityDeposit,
        Money amenitiesUpCharge,
        Money totalPrice,
        BookingStatus status,
        DateTime createdOnUtc)
        : base(id)
    {
        PetSitterId = petSitterId;
        UserId = userId;
        Duration = duration;
        PriceForPeriod = priceForPeriod;
        SecurityDeposit = securityDeposit;
        AmenitiesUpCharge = amenitiesUpCharge;
        TotalPrice = totalPrice;
        Status = status;
        CreatedOnUtc = createdOnUtc;
    }

    public Guid PetSitterId { get; private set; }
    public Guid UserId { get; private set; }

    public DateRange Duration { get; private set; }
    public Money PriceForPeriod { get; private set; }
    public Money SecurityDeposit { get; private set; }
    public Money AmenitiesUpCharge { get; private set; }
    public Money TotalPrice { get; private set; }
    public BookingStatus Status { get; private set; }
    public DateTime CreatedOnUtc { get; }
    public DateTime? ConfirmedOnUtc { get; private set; }
    public DateTime? RejectedOnUtc { get; private set; }
    public DateTime? CompletedOnUtc { get; private set; }
    public DateTime? CanceledOnUtc { get; private set; }

    public static Booking Reserve(
        PetSitter petSitter,
        Guid userId,
        DateRange duration,
        DateTime utcNow,
        PricingService pricingService)
    {

        var pricingDetails = pricingService.CalculatePrice(petSitter, duration);
        var booking = new Booking(
            Guid.NewGuid(),
            petSitter.Id,
            userId,
            duration,
            pricingDetails.PricingForPeriod,
            pricingDetails.SecurityDeposit,
            pricingDetails.AmenititiesUpCharge,
            pricingDetails.TotalPrice,
            BookingStatus.Reserved,
            utcNow);

        booking.RaiseDomainEvent(new BookingReservedDomainEvent(booking.Id));

        petSitter.LastBookedOnUtc = utcNow;

        return booking;
    }

    public Result Confirm(DateTime utcNow)
    {
        if (Status != BookingStatus.Reserved)
        {
            return Result.Failure(BookingErrors.NotReserved);
        }

        Status = BookingStatus.Confirmed;
        ConfirmedOnUtc = utcNow;

        RaiseDomainEvent(new BookingConfirmedDomainEvent(Id));

        return Result.Success();
    }

    public Result Reject(DateTime utcNow)
    {
        if (Status != BookingStatus.Reserved)
        {
            return Result.Failure(BookingErrors.NotReserved);
        }

        Status = BookingStatus.Rejected;
        RejectedOnUtc = utcNow;

        RaiseDomainEvent(new BookingRejectedDomainEvent(Id));

        return Result.Success();
    }

    public Result Complete(DateTime utcNow)
    {
        if (Status != BookingStatus.Confirmed)
        {
            return Result.Failure(BookingErrors.NotConfirmed);
        }

        Status = BookingStatus.Completed;
        CompletedOnUtc = utcNow;

        RaiseDomainEvent(new BookingCompletedDomainEvent(Id));

        return Result.Success();
    }

    public Result Cancel(DateTime utcNow)
    {
        if (Status != BookingStatus.Confirmed)
        {
            return Result.Failure(BookingErrors.NotConfirmed);
        }

        var currentDate = DateOnly.FromDateTime(utcNow);

        if (currentDate > Duration.Start)
        {
            Result.Failure(BookingErrors.AlreadyStarted);
        }

        Status = BookingStatus.Canceled;
        CanceledOnUtc = utcNow;

        RaiseDomainEvent(new BookingCanceledDomainEvent(Id));

        return Result.Success();
    }
}
