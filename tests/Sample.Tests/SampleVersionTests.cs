using Sample.Core;

namespace Sample.Tests;

public sealed class SampleVersionTests
{
    [Theory]
    [InlineData(1, "1.0", "1.0.1")]
    [InlineData(42, "2.7", "2.7.42")]
    public void FromBuildNumber_creates_repeatable_sample_version(
        int buildNumber,
        string prefix,
        string expectedVersion)
    {
        var version = SampleVersion.FromBuildNumber(buildNumber, prefix);

        Assert.Equal(expectedVersion, version);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void FromBuildNumber_rejects_non_positive_build_number(int buildNumber)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            SampleVersion.FromBuildNumber(buildNumber));
    }

    [Theory]
    [InlineData("")]
    [InlineData("1")]
    [InlineData("major.minor")]
    public void FromBuildNumber_rejects_invalid_prefix(string prefix)
    {
        Assert.Throws<ArgumentException>(() =>
            SampleVersion.FromBuildNumber(10, prefix));
    }
}
