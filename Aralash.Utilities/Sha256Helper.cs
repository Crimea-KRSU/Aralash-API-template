using System.Security.Cryptography;

namespace Aralash.Utilities;

public class Sha256Helper
{
    #region Parameters
    // при изменении этих параметров обратить внимание на это https://stackoverflow.com/a/73126492/17507652
    // а именно на пункт 5 в Pros
    private const int SaltSize = 16; 
    private const int KeySize = 32;
    private const int Iterations = 50000;
    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;
    #endregion
    
    /// <summary>
    /// Сгенерировать хеш с солью для строки
    /// </summary>
    /// <returns>Кортеж - хеш + соль</returns>
    public static (string Hash, string Salt) HashStringWithSalt(string str)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(str, salt, Iterations, Algorithm, KeySize);
        return (Convert.ToBase64String(hash), Convert.ToBase64String(salt));
    }

    /// <summary>
    /// Проверить введенную строку на соответствие 
    /// </summary>
    public static bool Verify(string input, string hashString, string saltStr)
    {
        var hash = Convert.FromBase64String(hashString);
        var salt = Convert.FromBase64String(saltStr);
        var inputHash = Rfc2898DeriveBytes.Pbkdf2(
            input,
            salt,
            Iterations,
            Algorithm,
            hash.Length
        );
        return CryptographicOperations.FixedTimeEquals(inputHash, hash);
    }
}