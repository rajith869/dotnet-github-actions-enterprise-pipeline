using Sample.Core;

namespace Sample.Tests;

public sealed class ArtifactPromotionPolicyTests
{
    [Theory]
    [InlineData("development")]
    [InlineData("test")]
    [InlineData("staging")]
    public void Evaluate_approves_supported_public_safe_environment(string environment)
    {
        var policy = new ArtifactPromotionPolicy();
        var manifest = new ArtifactManifest(
            ArtifactName: "sample-api-package",
            Version: "1.0.25",
            CommitSha: "sample-sha",
            CreatedAtUtc: DateTimeOffset.UtcNow);

        var decision = policy.Evaluate(manifest, environment);

        Assert.True(decision.CanPromote);
        Assert.Contains(environment, decision.Reason, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Evaluate_blocks_unknown_environment()
    {
        var policy = new ArtifactPromotionPolicy();
        var manifest = new ArtifactManifest(
            ArtifactName: "sample-api-package",
            Version: "1.0.25",
            CommitSha: "sample-sha",
            CreatedAtUtc: DateTimeOffset.UtcNow);

        var decision = policy.Evaluate(manifest, "private-production");

        Assert.False(decision.CanPromote);
    }

    [Fact]
    public void Evaluate_blocks_incomplete_artifact_metadata()
    {
        var policy = new ArtifactPromotionPolicy();
        var manifest = new ArtifactManifest(
            ArtifactName: "",
            Version: "1.0.25",
            CommitSha: "sample-sha",
            CreatedAtUtc: DateTimeOffset.UtcNow);

        var decision = policy.Evaluate(manifest, "development");

        Assert.False(decision.CanPromote);
        Assert.Contains("metadata", decision.Reason, StringComparison.OrdinalIgnoreCase);
    }
}
