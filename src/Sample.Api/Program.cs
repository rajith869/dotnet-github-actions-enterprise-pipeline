using Sample.Core;
using Sample.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IReleaseReadinessReader, InMemoryReleaseReadinessReader>();
builder.Services.AddSingleton<ArtifactPromotionPolicy>();
var app = builder.Build();

app.MapGet("/health", () =>
{
    return Results.Ok(new
    {
        service = "Sample.Api",
        status = "Healthy",
        purpose = "Public-safe CI/CD portfolio sample"
    });
});

app.MapGet("/release-readiness", (IReleaseReadinessReader reader) =>
{
    return Results.Ok(reader.GetCurrentSnapshot());
});

app.MapGet("/promotion-decision/{environment}", (
    string environment,
    ArtifactPromotionPolicy policy) =>
{
    var manifest = new ArtifactManifest(
        ArtifactName: "sample-api-package",
        Version: "1.0.0",
        CommitSha: "sample-commit-sha",
        CreatedAtUtc: DateTimeOffset.UtcNow);

    var decision = policy.Evaluate(manifest, environment);

    return decision.CanPromote
        ? Results.Ok(decision)
        : Results.BadRequest(decision);
});

app.Run();

public partial class Program;
