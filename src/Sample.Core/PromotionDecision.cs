namespace Sample.Core;

public sealed record PromotionDecision(bool CanPromote, string Reason)
{
    public static PromotionDecision Approved(string reason) => new(true, reason);

    public static PromotionDecision Blocked(string reason) => new(false, reason);
}
