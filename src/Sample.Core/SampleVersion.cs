namespace Sample.Core;

public static class SampleVersion
{
    public static string FromBuildNumber(int buildNumber, string versionPrefix = "1.0")
    {
        if (buildNumber <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(buildNumber),
                "Build number must be a positive integer.");
        }

        var normalizedPrefix = NormalizeVersionPrefix(versionPrefix);

        return $"{normalizedPrefix}.{buildNumber}";
    }

    private static string NormalizeVersionPrefix(string versionPrefix)
    {
        if (string.IsNullOrWhiteSpace(versionPrefix))
        {
            throw new ArgumentException("Version prefix is required.", nameof(versionPrefix));
        }

        var parts = versionPrefix.Split(
            '.',
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length != 2 || parts.Any(part => !int.TryParse(part, out _)))
        {
            throw new ArgumentException(
                "Version prefix must use a 'major.minor' numeric format.",
                nameof(versionPrefix));
        }

        return string.Join('.', parts);
    }
}
