using LingoMQ.Core.Domain.Common;
using LingoMQ.Core.Domain.Exceptions;
using LingoMQ.Core.Domain.Services.Crypto;
using LingoMQ.Core.Domain.Services.Crypto.Models;

namespace LingoMQ.Core.Domain.Users;

public class UserCredentials : EntityBase<int>
{
    public virtual List<AuthorizationType> AuthorizationTypes { get; private set; } = new();
    public string PasswordHash { get; private set; } = "";
    public string PasswordSalt { get; private set; } = "";

    public UserCredentials(AuthorizationType type, string password)
    {
        AuthorizationTypes.Add(type);
        Crypt(password);
    }

    public UserCredentials(AuthorizationType[] types, string password)
    {
        AuthorizationTypes.AddRange(types);
        Crypt(password);
    }

    protected UserCredentials() { }

    private void Crypt(string word)
    {
        ICrypto<CryptoKeyValuePair> crypto = new Sha256Algorithm();
        var keyValuePair = crypto.Crypt(word);
        PasswordHash = keyValuePair.Key;
        PasswordSalt = keyValuePair.Value;
    }

    public bool CheckValidity(AuthorizationType type, string password)
    {
        ICrypto<CryptoKeyValuePair> crypto = new Sha256Algorithm();
        bool isValidPassword = crypto.Validate(
            password,
            new() { Key = PasswordHash, Value = PasswordSalt }
        );

        return AuthorizationTypes.Any(x => x.Type == type.Type && x.Value == type.Value)
            && isValidPassword;
    }

    public void AddAuthorizationType(AuthorizationType type)
    {
        if (AuthorizationTypes.Any(x => x.Type.Equals(type.Type)))
            throw new AuthorizationTypeIsAlreadySetException();

        AuthorizationTypes.Add(type);
    }

    public void RemoveAuthorizationType(AuthorizationType type) => AuthorizationTypes.Remove(type);

    public void ChangePassword(string password) => Crypt(password);
}
