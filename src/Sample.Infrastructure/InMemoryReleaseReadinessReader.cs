using Sample.Core;

namespace Sample.Infrastructure;

public sealed class InMemoryReleaseReadinessReader : IReleaseReadinessReader
{
    public ReleaseReadinessSnapshot GetCurrentSnapshot()
    {
        ApplicationComponent[] components =
        [
            new(
                Name: "Sample.Api",
                Responsibility: "Public-safe HTTP boundary for release readiness checks.",
                IsRequiredForRelease: true),
            new(
                Name: "Sample.Core",
                Responsibility: "Business-neutral release policy and versioning rules.",
                IsRequiredForRelease: true),
            new(
                Name: "Sample.Infrastructure",
                Responsibility: "Replaceable infrastructure adapter used by the sample API.",
                IsRequiredForRelease: true)
        ];

        string[] principles =
        [
            "Build and package the artifact once.",
            "Promote the same artifact through environment gates.",
            "Keep release decisions separate from application compilation.",
            "Document the workflow without exposing real deployment details."
        ];

        return new ReleaseReadinessSnapshot(
            ApplicationName: "Sample .NET Application Suite",
            VersionStrategy: "1.0.<github-run-number>",
            Components: components,
            ReleasePrinciples: principles);
    }
}
