using System.Security.Cryptography;
using LingoMQ.Core.Domain.Services.Crypto.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace LingoMQ.Core.Domain.Services.Crypto;

public class Sha256Algorithm : ICrypto<CryptoKeyValuePair>
{
    public CryptoKeyValuePair Crypt(string word)
    {
        byte[] salt = CreateSalt(128 / 8);
        var hash = KeyDerivation.Pbkdf2(
            password: word,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8
        );

        return new CryptoKeyValuePair()
        {
            Key = Convert.ToBase64String(hash),
            Value = Convert.ToBase64String(salt),
        };
    }

    public bool Validate(string word, CryptoKeyValuePair value)
    {
        var hash = KeyDerivation.Pbkdf2(
            password: word,
            salt: Convert.FromBase64String(value.Value!),
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8
        );

        return string.Equals(
            Convert.ToBase64String(hash),
            value.Key,
            StringComparison.OrdinalIgnoreCase
        );
    }

    private byte[] CreateSalt(int size)
    {
        byte[] buffer = RandomNumberGenerator.GetBytes(size);
        return buffer;
    }
}
