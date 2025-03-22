namespace LingoMQ.Core.Domain.Services.Crypto;

/// <summary>
/// Абстрактный класс для последующих алгоритмов шифрования
/// </summary>
public interface ICrypto<T>
{
    /// <summary>
    /// <b>Шифрует</b> полученное слово в пару ключ-значение
    /// </summary>
    /// <param name="word"><b>Шифруемое слово</b></param>
    /// <returns>KeyBase</returns>
    public T Crypt(string word);

    /// <summary>
    /// <b>Валидирует</b> зашифрованное слово с парой ключ-значение
    /// </summary>
    /// <param name="word"><b>Зашифрованное слово</b></param>
    /// <param name="validationKeyPair"><b>Пара ключ-значение</b></param>
    /// <returns>true or false</returns>
    public bool Validate(string word, T value);
}
