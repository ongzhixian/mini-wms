using System.Security.Claims;

namespace Wms.Extensions;

public static class EnumerableClaimExtensions
{
    public static IEnumerable<Claim> Map(this IEnumerable<Claim> claims, IDictionary<string, string> claimTypeMap)
    {
        foreach (Claim claim in claims)
        {
            if (claimTypeMap.TryGetValue(claim.Type, out string? value))
            {
                yield return new Claim(value, claim.Value, claim.ValueType, claim.Issuer, claim.OriginalIssuer, claim.Subject);
            }
            else
            {
                yield return claim;
            }
        }
    }
}
