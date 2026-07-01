using Sample.Infrastructure;

namespace Sample.Tests;

public sealed class InMemoryReleaseReadinessReaderTests
{
    [Fact]
    public void GetCurrentSnapshot_returns_public_safe_component_summary()
    {
        var reader = new InMemoryReleaseReadinessReader();

        var snapshot = reader.GetCurrentSnapshot();

        Assert.Equal("Sample .NET Application Suite", snapshot.ApplicationName);
        Assert.All(snapshot.Components, component =>
        {
            Assert.StartsWith("Sample.", component.Name, StringComparison.Ordinal);
            Assert.True(component.IsRequiredForRelease);
        });
        Assert.Contains(
            snapshot.ReleasePrinciples,
            principle => principle.Contains("Build and package", StringComparison.OrdinalIgnoreCase));
    }
}
