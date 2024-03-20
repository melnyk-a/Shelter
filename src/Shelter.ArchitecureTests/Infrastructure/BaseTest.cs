using Shelter.Application.Abstractions.Messaging;
using Shelter.Auth.Keycloak.Authentication;
using Shelter.Domain.Abstractions;
using Shelter.Infrastructure.HealthChecks;
using Shelter.Persistence;

namespace Shelter.ArchitectureTests.Infrastructure;

public class BaseTest
{
    protected static readonly Assembly ApplicationAssembly = typeof(IBaseCommand).Assembly;

    protected static readonly Assembly DomainAssembly = typeof(Entity).Assembly;

    protected static readonly Assembly PersistenceAssembly = typeof(ApplicationDbContext).Assembly;

    protected static readonly Assembly AuthAssembly = typeof(KeycloakOptions).Assembly;

    protected static readonly Assembly InfrastructureAssembly = typeof(CustomSqlHealthCheck).Assembly;

    protected static readonly Assembly BackgroundJobsAssembly = typeof(ApplicationDbContext).Assembly;

    protected static readonly Assembly PresentationAssembly = typeof(Program).Assembly;
}