﻿using Shelter.Domain.Abstractions;
using Shelter.Domain.Bookings;
using Shelter.Domain.Reviews.Events;

namespace Shelter.Domain.Reviews;

public sealed class Review : AuditableEntity
{
    private Review(
        Guid id,
        Guid petSitterId,
        Guid bookingId,
        Guid userId,
        Rating rating,
        Comment comment,
        DateTime createdOnUtc)
        : base(id)
    {
        PetSitterId = petSitterId;
        BookingId = bookingId;
        UserId = userId;
        Rating = rating;
        Comment = comment;
        CreatedOnUtc = createdOnUtc;
    }

    private Review() { }

    public Guid PetSitterId { get; private set; }

    public Guid BookingId { get; private set; }

    public Guid UserId { get; private set; }

    public Rating Rating { get; private set; }

    public Comment Comment { get; private set; }

    public DateTime CreatedOnUtc { get; private set; }

    public static Result<Review> Create(
        Booking booking,
        Rating rating,
        Comment comment,
        DateTime createdOnUtc)
    {
        if (booking.Status != BookingStatus.Completed)
        {
            return Result.Failure<Review>(ReviewErrors.NotEligible);
        }

        var review = new Review(
            Guid.NewGuid(),
            booking.PetSitterId,
            booking.Id,
            booking.UserId,
            rating,
            comment,
            createdOnUtc);

        review.RaiseDomainEvent(new ReviewCreatedDomainEvent(review.Id));

        return review;
    }

    public Result<Review> Update(
        Rating rating,
        Comment comment)
    {
        Comment = comment;
        Rating = rating;

        RaiseDomainEvent(new ReviewUpdatedDomainEvent(Id));

        return Result.Success(this);
    }
}
