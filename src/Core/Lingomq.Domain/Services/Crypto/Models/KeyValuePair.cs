namespace LingoMQ.Core.Domain.Services.Crypto.Models;

public class CryptoKeyValuePair : KeyBase
{
    public required string Key { get; set; }
    public required string Value { get; set; }
}
