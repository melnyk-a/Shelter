using System.Data;

namespace Shelter.Application.Abstractions.Persistence;

public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}
