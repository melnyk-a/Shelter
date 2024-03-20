using Shelter.Application.Abstractions.Clock;
using Shelter.Application.Abstractions.Messaging;
using Shelter.Application.Exceptions;
using Shelter.Domain.Abstractions;
using Shelter.Domain.Abstractions.Persistence;
using Shelter.Domain.Bookings;
using Shelter.Domain.PetSitters;
using Shelter.Domain.Users;

namespace Shelter.Application.Bookings.ReserveBooking;

internal sealed class ReserveBookingCommandHandler : ICommandHandler<ReserveBookingCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IPetSitterRepository _petSitterRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly PricingService _pricingService;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ReserveBookingCommandHandler(
        IUserRepository userRepository,
        IPetSitterRepository petSitterRepository,
        IBookingRepository bookingRepository,
        IUnitOfWork unitOfWork,
        PricingService pricingService,
        IDateTimeProvider dateTimeProvider)
    {
        _userRepository = userRepository;
        _petSitterRepository = petSitterRepository;
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
        _pricingService = pricingService;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<Guid>> Handle(ReserveBookingCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure<Guid>(UserErrors.NotFound);
        }

        var petSitter = await _petSitterRepository.GetByIdAsync(request.PetSitterId, cancellationToken);

        if (petSitter is null)
        {
            return Result.Failure<Guid>(PetSitterErrors.NotFound);
        }

        var duration = DateRange.Create(request.StartDate, request.EndDate);

        if (await _bookingRepository.IsOverlappingAsync(petSitter, duration, cancellationToken))
        {
            return Result.Failure<Guid>(BookingErrors.Overlap);
        }

        try
        {
            var booking = Booking.Reserve(
            petSitter,
            user.Id,
            duration,
            utcNow: _dateTimeProvider.UtcNow,
            _pricingService);

            _bookingRepository.Add(booking);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return booking.Id;
        }
        catch (ConcurrencyException)
        {
            return Result.Failure<Guid>(BookingErrors.Overlap);
        }
    }
}
