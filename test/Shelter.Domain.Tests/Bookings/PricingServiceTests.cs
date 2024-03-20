using Shelter.Domain.Bookings;
using Shelter.Domain.Shared;
using Shelter.Domain.UnitTests.PetSitters;

namespace Shelter.Domain.UnitTests.Bookings;

public class PricingServiceTests
{
    [Fact]
    public void CalculatePrice_Should_ReturnCorrectTotalPrice()
    {
        // Arrange
        var price = new Money(10.0m, Currency.Usd);
        var period = DateRange.Create(new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 10));
        var expectedTotalPrice = new Money(price.Amount * period.LengthInDays, price.Currency);
        var petSitter = PetSitterData.Create(price);
        var pricingService = new PricingService();

        // Act
        var pricingDetails = pricingService.CalculatePrice(petSitter, period);

        // Assert
        pricingDetails.TotalPrice.Should().Be(expectedTotalPrice);
    }

    [Fact]
    public void CalculatePrice_Should_ReturnCorrectTotalPrice_WhenSecurityDepositIsIncluded()
    {
        // Arrange
        var price = new Money(10.0m, Currency.Usd);
        var securityDeposit = new Money(99.99m, Currency.Usd);
        var period = DateRange.Create(new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 10));
        var expectedTotalPrice = new Money((price.Amount * period.LengthInDays) + securityDeposit.Amount, price.Currency);
        var petSitter = PetSitterData.Create(price, securityDeposit);
        var pricingService = new PricingService();

        // Act
        var pricingDetails = pricingService.CalculatePrice(petSitter, period);

        // Assert
        pricingDetails.TotalPrice.Should().Be(expectedTotalPrice);
    }
}
