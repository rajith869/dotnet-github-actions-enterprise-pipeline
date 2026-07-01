namespace Sample.Core;

public interface IReleaseReadinessReader
{
    ReleaseReadinessSnapshot GetCurrentSnapshot();
}
