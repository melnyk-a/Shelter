using Shelter.Application.Abstractions.Clock;
using Shelter.Application.Abstractions.Messaging;
using Shelter.Application.Exceptions;
using Shelter.Domain.Abstractions;
using Shelter.Domain.Abstractions.Persistence;
using Shelter.Domain.Bookings;
using Shelter.Domain.PetSitters;
using Shelter.Domain.Reviews;

namespace Shelter.Application.Reviews.CreateReview;

internal sealed class CreateReviewCommandHandler :
    ICommandHandler<CreateReviewCommand, Guid>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IPetSitterRepository _petSitterRepository;
    private readonly IReviewRepository _reviewRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateReviewCommandHandler(
        IBookingRepository bookingRepository,
        IPetSitterRepository petSitterRepository,
        IReviewRepository reviewRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _bookingRepository = bookingRepository;
        _petSitterRepository = petSitterRepository;
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<Guid>> Handle(
        CreateReviewCommand request,
        CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(request.BookingId, cancellationToken);

        if (booking is null)
        {
            return Result.Failure<Guid>(BookingErrors.NotFound);
        }

        var petSitter = await _petSitterRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (petSitter is null)
        {
            return Result.Failure<Guid>(PetSitterErrors.NotFound);
        }

        if (petSitter.Id != booking.UserId)
        {
            return Result.Failure<Guid>(ReviewErrors.NotMatch);
        }

        try
        {
            var review = Review.Create(
            booking,
            Rating.Create(request.Rating).Value,
            new Comment(request.Comment),
            createdOnUtc: _dateTimeProvider.UtcNow).Value;

            _reviewRepository.Add(review);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return review.Id;
        }
        catch (ConcurrencyException)
        {
            return Result.Failure<Guid>(BookingErrors.Overlap);
        }
    }
}
