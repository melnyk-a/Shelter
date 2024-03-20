using Shelter.ArchitectureTests.Infrastructure;

namespace Shelter.ArchitectureTests.Layers;

public class LayerTests : BaseTest
{
    [Fact]
    public void DomainLayer_ShouldNotHaveDependencyOn_ApplicationLayer()
    {
        var result = Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn(ApplicationAssembly.GetName().Name)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void DomainLayer_ShouldNotHaveDependencyOn_InfrastructureLayer()
    {
        var infrastructureResult = Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn(InfrastructureAssembly.GetName().Name)
            .GetResult();

        var persistenceResult = Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn(PersistenceAssembly.GetName().Name)
            .GetResult();

        var authResult = Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn(AuthAssembly.GetName().Name)
            .GetResult();

        var backgroundJobsResult = Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn(BackgroundJobsAssembly.GetName().Name)
            .GetResult();

        infrastructureResult.IsSuccessful.Should().BeTrue();
        persistenceResult.IsSuccessful.Should().BeTrue();
        authResult.IsSuccessful.Should().BeTrue();
        backgroundJobsResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void ApplicationLayer_ShouldNotHaveDependencyOn_InfrastructureLayer()
    {
        var infrastructureResult = Types.InAssembly(ApplicationAssembly)
            .Should()
            .NotHaveDependencyOn(InfrastructureAssembly.GetName().Name)
            .GetResult();

        var persistenceResult = Types.InAssembly(ApplicationAssembly)
            .Should()
            .NotHaveDependencyOn(PersistenceAssembly.GetName().Name)
            .GetResult();

        var authResult = Types.InAssembly(ApplicationAssembly)
            .Should()
            .NotHaveDependencyOn(AuthAssembly.GetName().Name)
            .GetResult();

        var backgroundJobsResult = Types.InAssembly(ApplicationAssembly)
            .Should()
            .NotHaveDependencyOn(BackgroundJobsAssembly.GetName().Name)
            .GetResult();

        infrastructureResult.IsSuccessful.Should().BeTrue();
        persistenceResult.IsSuccessful.Should().BeTrue();
        authResult.IsSuccessful.Should().BeTrue();
        backgroundJobsResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void ApplicationLayer_ShouldNotHaveDependencyOn_PresentationLayer()
    {
        var result = Types.InAssembly(ApplicationAssembly)
            .Should()
            .NotHaveDependencyOn(PresentationAssembly.GetName().Name)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void InfrastructureLayer_ShouldNotHaveDependencyOn_PresentationLayer()
    {
        var infrastructureResult = Types.InAssembly(InfrastructureAssembly)
            .Should()
            .NotHaveDependencyOn(PresentationAssembly.GetName().Name)
            .GetResult();

        var persistenceResult = Types.InAssembly(PersistenceAssembly)
            .Should()
            .NotHaveDependencyOn(PresentationAssembly.GetName().Name)
            .GetResult();

        var authResult = Types.InAssembly(AuthAssembly)
            .Should()
            .NotHaveDependencyOn(PresentationAssembly.GetName().Name)
            .GetResult();

        var backgroundJobsResult = Types.InAssembly(BackgroundJobsAssembly)
            .Should()
            .NotHaveDependencyOn(PresentationAssembly.GetName().Name)
            .GetResult();

        infrastructureResult.IsSuccessful.Should().BeTrue();
        persistenceResult.IsSuccessful.Should().BeTrue();
        authResult.IsSuccessful.Should().BeTrue();
        backgroundJobsResult.IsSuccessful.Should().BeTrue();
    }
}