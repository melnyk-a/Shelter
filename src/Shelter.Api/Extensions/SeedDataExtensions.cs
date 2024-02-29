using Bogus;
using Shelter.Application.Abstractions.Data;
using Shelter.Domain.PetSitters;
using Dapper;

namespace Shelter.Api.Extensions
{
    public static class SeedDataExtensions
    {
        public static void SeedData(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var sqlConnectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
            using var connection = sqlConnectionFactory.CreateConnection();

            var faker = new Faker();

            List<object> petSitters = new();
            for (var i = 0; i < 100; i++)
            {
                petSitters.Add(new
                {
                    Id = Guid.NewGuid(),
                    Name = faker.Company.CompanyName(),
                    Description = "Amazing",
                    Country = faker.Address.Country(),
                    State = faker.Address.State(),
                    City = faker.Address.City(),
                    Street = faker.Address.StreetAddress(),
                    PriceAmount = faker.Random.Decimal(50, 1000),
                    PriceCurrency = "USD",
                    SecurityDepositAmount = faker.Random.Decimal(25, 200),
                    SecurityDepositCurrency = "USD",
                    AllowedPetSizes = new List<int> { (int)PetSize.UpTo5kg, (int)PetSize.From5To10kg },
                    AllowedPetTypes = new List<int> { (int)PetTypes.Cats, (int)PetTypes.Dogs },
                    LastBookedOn = DateTime.MinValue
                });
            }

            const string sql = """
            INSERT INTO public.pet_sitters
            (id, "name", description, address_country, address_state, address_city, address_street, price_amount, price_currency, security_deposit_amount, security_deposit_currency, allowed_pet_sizes, allowed_pet_types, last_booked_on_utc)
            VALUES(@Id, @Name, @Description, @Country, @State, @City, @Street, @PriceAmount, @PriceCurrency, @SecurityDepositAmount, @SecurityDepositCurrency, @AllowedPetSizes,  @AllowedPetTypes, @LastBookedOn);
            """;

            connection.Execute(sql, petSitters);
        }
    }
}
