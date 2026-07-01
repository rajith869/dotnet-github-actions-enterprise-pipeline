namespace Sample.Core;

public sealed record ArtifactManifest(
    string ArtifactName,
    string Version,
    string CommitSha,
    DateTimeOffset CreatedAtUtc)
{
    public bool HasRequiredMetadata =>
        !string.IsNullOrWhiteSpace(ArtifactName)
        && !string.IsNullOrWhiteSpace(Version)
        && !string.IsNullOrWhiteSpace(CommitSha);
}
