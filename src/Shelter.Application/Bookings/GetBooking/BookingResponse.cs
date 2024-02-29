namespace Shelter.Application.Bookings.GetBooking;

public sealed class BookingResponse
{
    public Guid Id { get; init; }
    public Guid PetSitterId { get; init; }
    public Guid UserId { get; init; }
    public decimal PriceAmount { get; init; }
    public string PriceCurrency { get; init; }
    public decimal SecurityDepositAmount { get; init; }
    public string SecurityDepositCurrency { get; init; }
    public decimal AmenitiesUpChargeAmount { get; init; }
    public string AmenitiesUpChargeCurrency { get; init; }
    public decimal TotalPriceAmount { get; init; }
    public string TotalPriceCurrency { get; init; }
    public int Status { get; init; }
    public DateOnly DurationStart { get; init; }
    public DateOnly DurationEnd { get; init; }
    public DateTime CreatedOnUtc { get; init; }
    public DateTime? ConfirmedOnUtc { get; init; }
    public DateTime? RejectedOnUtc { get; init; }
    public DateTime? CompletedOnUtc { get; init; }
    public DateTime? CanceledOnUtc { get; init; }
}
