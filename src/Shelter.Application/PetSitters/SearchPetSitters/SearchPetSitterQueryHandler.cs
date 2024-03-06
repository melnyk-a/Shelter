using Dapper;
using Shelter.Application.Abstractions.Persistence;
using Shelter.Application.Abstractions.Messaging;
using Shelter.Domain.Abstractions;
using Shelter.Domain.Bookings;
using Shelter.Application.PetSitters.Shared;

namespace Shelter.Application.PetSitters.SearchPetSitters;

internal sealed class SearchPetSitterQueryHandler :
    IQueryHandler<SearchPetSitterQuery, IReadOnlyList<PetSitterResponse>>
{
    private static readonly int[] ActiveBookingStatuses =
    [
        (int)BookingStatus.Reserved,
        (int)BookingStatus.Confirmed,
        (int)BookingStatus.Completed
    ];

    private readonly ISqlConnectionFactory _connectionFactory;

    public SearchPetSitterQueryHandler(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Result<IReadOnlyList<PetSitterResponse>>> Handle(SearchPetSitterQuery request, CancellationToken cancellationToken)
    {
        if (request.StartDate > request.EndDate)
        {
            return new List<PetSitterResponse>();
        }

        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT
                a.id AS Id,
                a.name AS Name,
                a.description AS Description,
                a.price_amount AS Price,
                a.price_currency AS Currency,
                a.address_country AS Country,
                a.address_state AS State,
                a.address_city AS City,
                a.address_street AS Street
            FROM pet_sitters AS a
            WHERE NOT EXISTS
            (
                SELECT 1
                FROM bookings AS b
                WHERE
                    b.pet_sitter_id = a.id AND
                    b.duration_start <= @EndDate AND
                    b.duration_end >= @StartDate AND
                    b.status = ANY(@ActiveBookingStatuses)
            )
            """;

        var petSitters = await connection.QueryAsync<PetSitterResponse, AddressResponse, PetSitterResponse>(
            sql,
            (petSitter, adress) =>
            {
                petSitter.Address = adress;
                return petSitter;
            },
            new
            {
                request.StartDate,
                request.EndDate,
                ActiveBookingStatuses
            },
            splitOn: "Country");

        return petSitters.ToList();
    }
}