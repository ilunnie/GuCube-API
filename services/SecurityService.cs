using System.Security.Cryptography;
using System.Text;
using GuCube.Services;

public class SecurityService : ISecurityService
{
    public Task<string> GenerateJwt<T>(T obj)
    {
        throw new NotImplementedException();
    }

    public async Task<string> GenerateSalt()
    {
        var saltBytes = getRandomArray();
        var base64salt = Convert.ToBase64String(saltBytes);
        return base64salt;
    }

    public async Task<string> HashPassword(string password, string salt)
    {
        var saltBytes = Convert.FromBase64String(salt);
        var passwordBytes = Encoding.UTF8.GetBytes(password);

        var hashBytes = getHash(saltBytes, passwordBytes);
        var hash = Convert.ToBase64String(hashBytes);

        return hash;
    }

    public Task<T> ValidadeJwt<T>(string jwt)
    {
        throw new NotImplementedException();
    }

    private byte[] getRandomArray()
    {
        byte[] randomBytes = new byte[24];
        Random.Shared.NextBytes(randomBytes);
        return randomBytes;
    }

    private byte[] getHash(byte[] saltBytes, byte[] passwordBytes)
    {
        const int iterationCount = 1000;
        using var hashAlgorithm = new Rfc2898DeriveBytes(
            passwordBytes, saltBytes, iterationCount
        );
        var hashBytes = hashAlgorithm.GetBytes(32);
        return hashBytes;
    }
}