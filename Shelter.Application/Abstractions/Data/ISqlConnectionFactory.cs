using System.Data;

namespace Shelter.Application.Abstractions.Data;

internal interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}
