namespace Wms.Models;

public record class HttpClientSetting
{
    public string BaseAddress { get; init; } = string.Empty;

    public void EnsureIsValid()
    {
        if (string.IsNullOrEmpty(BaseAddress) 
            || (!Uri.IsWellFormedUriString(BaseAddress, UriKind.Absolute)))
        {
            throw new InvalidDataException($"Invalid BaseAddress:{BaseAddress}");
        }
    }

    public override string ToString() =>
        $"BaseAddress:{BaseAddress}";
}
