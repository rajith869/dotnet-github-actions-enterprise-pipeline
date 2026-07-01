namespace Sample.Core;

public sealed record ReleaseReadinessSnapshot(
    string ApplicationName,
    string VersionStrategy,
    IReadOnlyCollection<ApplicationComponent> Components,
    IReadOnlyCollection<string> ReleasePrinciples);
