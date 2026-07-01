namespace Sample.Core;

public sealed record ApplicationComponent(
    string Name,
    string Responsibility,
    bool IsRequiredForRelease);
