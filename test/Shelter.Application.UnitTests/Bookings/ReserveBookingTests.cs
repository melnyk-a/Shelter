using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shelter.Application.Abstractions.Clock;
using Shelter.Application.Bookings.ReserveBooking;
using Shelter.Application.Exceptions;
using Shelter.Application.UnitTests.PetSitters;
using Shelter.Application.UnitTests.Users;
using Shelter.Domain.Abstractions.Persistence;
using Shelter.Domain.Bookings;
using Shelter.Domain.PetSitters;
using Shelter.Domain.Users;

namespace Shelter.Application.UnitTests.Bookings;

public sealed class ReserveBookingTests
{
    private static readonly DateTime UtcNow = DateTime.UtcNow;
    private static readonly ReserveBookingCommand Command = new(
        Guid.NewGuid(),
        Guid.NewGuid(),
        new DateOnly(2024, 1, 1),
        new DateOnly(2024, 1, 10));

    private readonly ReserveBookingCommandHandler _handler;
    private readonly IUserRepository _userRepositoryMock;
    private readonly IPetSitterRepository _petSitterRepositoryMock;
    private readonly IBookingRepository _bookingRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly IDateTimeProvider _dateTimeProviderMock;

    public ReserveBookingTests()
    {
        _userRepositoryMock = Substitute.For<IUserRepository>();
        _petSitterRepositoryMock = Substitute.For<IPetSitterRepository>();
        _bookingRepositoryMock = Substitute.For<IBookingRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _dateTimeProviderMock = Substitute.For<IDateTimeProvider>();
        _dateTimeProviderMock.UtcNow.Returns(UtcNow);

        _handler = new ReserveBookingCommandHandler(
           _userRepositoryMock,
           _petSitterRepositoryMock,
           _bookingRepositoryMock,
           _unitOfWorkMock,
           new PricingService(),
           _dateTimeProviderMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenUserIsNull()
    {
        // Arrange
        _userRepositoryMock
            .GetByIdAsync(Command.UserId, Arg.Any<CancellationToken>())
            .Returns((User?)null);

        // Act
        var result = await _handler.Handle(Command, default);

        // Assert
        result.Error.Should().Be(UserErrors.NotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenPetSitterIsNull()
    {
        // Arrange
        var user = UserData.Create();

        _userRepositoryMock
            .GetByIdAsync(Command.UserId, Arg.Any<CancellationToken>())
            .Returns(user);

        _petSitterRepositoryMock
            .GetByIdAsync(Command.PetSitterId, Arg.Any<CancellationToken>())
            .Returns((PetSitter?)null);

        // Act
        var result = await _handler.Handle(Command, default);

        // Assert
        result.Error.Should().Be(PetSitterErrors.NotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenPetSitterIsBooked()
    {
        // Arrange
        var user = UserData.Create();
        var petSitter = PetSitterData.Create();
        var duration = DateRange.Create(Command.StartDate, Command.EndDate);

        _userRepositoryMock
            .GetByIdAsync(Command.UserId, Arg.Any<CancellationToken>())
            .Returns(user);

        _petSitterRepositoryMock
            .GetByIdAsync(Command.PetSitterId, Arg.Any<CancellationToken>())
            .Returns(petSitter);

        _bookingRepositoryMock
            .IsOverlappingAsync(petSitter, duration, Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        var result = await _handler.Handle(Command, default);

        // Assert
        result.Error.Should().Be(BookingErrors.Overlap);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenUnitOfWorkThrows()
    {
        // Arrange
        var user = UserData.Create();
        var petSitter = PetSitterData.Create();
        var duration = DateRange.Create(Command.StartDate, Command.EndDate);

        _userRepositoryMock
            .GetByIdAsync(Command.UserId, Arg.Any<CancellationToken>())
            .Returns(user);

        _petSitterRepositoryMock
            .GetByIdAsync(Command.PetSitterId, Arg.Any<CancellationToken>())
            .Returns(petSitter);

        _bookingRepositoryMock
            .IsOverlappingAsync(petSitter, duration, Arg.Any<CancellationToken>())
            .Returns(false);

        _unitOfWorkMock
            .SaveChangesAsync()
            .ThrowsAsync(new ConcurrencyException("Concurrency", new Exception()));

        // Act
        var result = await _handler.Handle(Command, default);

        // Assert
        result.Error.Should().Be(BookingErrors.Overlap);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenBookingIsReserved()
    {
        // Arrange
        var user = UserData.Create();
        var petSitter = PetSitterData.Create();
        var duration = DateRange.Create(Command.StartDate, Command.EndDate);

        _userRepositoryMock
            .GetByIdAsync(Command.UserId, Arg.Any<CancellationToken>())
            .Returns(user);

        _petSitterRepositoryMock
            .GetByIdAsync(Command.PetSitterId, Arg.Any<CancellationToken>())
            .Returns(petSitter);

        _bookingRepositoryMock
            .IsOverlappingAsync(petSitter, duration, Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        var result = await _handler.Handle(Command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_CallRepository_WhenBookingIsReserved()
    {
        // Arrange
        var user = UserData.Create();
        var petSitter = PetSitterData.Create();
        var duration = DateRange.Create(Command.StartDate, Command.EndDate);

        _userRepositoryMock
            .GetByIdAsync(Command.UserId, Arg.Any<CancellationToken>())
            .Returns(user);

        _petSitterRepositoryMock
            .GetByIdAsync(Command.PetSitterId, Arg.Any<CancellationToken>())
            .Returns(petSitter);
        _bookingRepositoryMock
            .IsOverlappingAsync(petSitter, duration, Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        var result = await _handler.Handle(Command, default);

        // Assert
        _bookingRepositoryMock.Received(1).Add(Arg.Is<Booking>(b => b.Id == result.Value));
    }
}
