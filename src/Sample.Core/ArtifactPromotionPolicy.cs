namespace Sample.Core;

public sealed class ArtifactPromotionPolicy
{
    private static readonly string[] SupportedEnvironments =
    [
        "development",
        "test",
        "staging"
    ];

    public IReadOnlyCollection<string> GetSupportedEnvironments() => SupportedEnvironments;

    public PromotionDecision Evaluate(ArtifactManifest manifest, string targetEnvironment)
    {
        ArgumentNullException.ThrowIfNull(manifest);

        if (!manifest.HasRequiredMetadata)
        {
            return PromotionDecision.Blocked("Artifact metadata is incomplete.");
        }

        if (string.IsNullOrWhiteSpace(targetEnvironment))
        {
            return PromotionDecision.Blocked("Target environment is required.");
        }

        var isSupportedEnvironment = SupportedEnvironments.Contains(
            targetEnvironment,
            StringComparer.OrdinalIgnoreCase);

        if (!isSupportedEnvironment)
        {
            return PromotionDecision.Blocked(
                $"'{targetEnvironment}' is not part of the public-safe promotion simulation.");
        }

        return PromotionDecision.Approved(
            $"Artifact '{manifest.ArtifactName}' version '{manifest.Version}' can be promoted to '{targetEnvironment}'.");
    }
}
