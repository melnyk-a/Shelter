using Shelter.Application.IntegrationTests.Infrastructure;
using Shelter.Application.PetSitters.SearchPetSitters;

namespace Shelter.Application.IntegrationTests.PetSitters;

public class SearchPetSitterTests : BaseIntegrationTest
{
    public SearchPetSitterTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task SearchPetSitters_ShouldReturnEmptyList_WhenDateRangeInvalid()
    {
        // Arrange
        var query = new SearchPetSitterQuery(new DateOnly(2024, 1, 10), new DateOnly(2024, 1, 1));

        // Act
        var result = await Sender.Send(query);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }

    [Fact]
    public async Task SearchPetSitters_ShouldReturnPetSitters_WhenDateRangeIsValid()
    {
        // Arrange
        var query = new SearchPetSitterQuery(new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 10));

        // Act
        var result = await Sender.Send(query);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
    }
}

