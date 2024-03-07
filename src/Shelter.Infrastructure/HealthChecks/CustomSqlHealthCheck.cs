using Dapper;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Shelter.Application.Abstractions.Persistence;


namespace Shelter.Infrastructure.HealthChecks;

public sealed class CustomSqlHealthCheck : IHealthCheck
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public CustomSqlHealthCheck(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("SELECT 1;");

            return HealthCheckResult.Healthy();
        }
       catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(exception: ex);
        }
    }
}
