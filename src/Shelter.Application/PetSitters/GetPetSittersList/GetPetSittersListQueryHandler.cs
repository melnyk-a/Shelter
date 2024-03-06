using Dapper;
using Shelter.Application.Abstractions.Messaging;
using Shelter.Application.Abstractions.Persistence;
using Shelter.Application.PetSitters.GetPetSitters;
using Shelter.Application.PetSitters.Shared;
using Shelter.Domain.Abstractions;

namespace Shelter.Application.PetSitters.GetPetSittersList;

internal sealed class GetPetSittersListQueryHandler :
    IQueryHandler<GetPetSittersListQuery, IReadOnlyList<PetSitterResponse>>
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public GetPetSittersListQueryHandler(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Result<IReadOnlyList<PetSitterResponse>>> Handle(
        GetPetSittersListQuery request,
        CancellationToken cancellationToken)
    {
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
            FROM pet_sitters AS
            """;

        var petSitters = await connection.QueryAsync<PetSitterResponse, AddressResponse, PetSitterResponse>(
            sql,
            (petSitter, adress) =>
            {
                petSitter.Address = adress;
                return petSitter;
            },
            splitOn: "Country");

        return petSitters.ToList();
    }
}
