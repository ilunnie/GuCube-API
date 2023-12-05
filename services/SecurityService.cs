using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using GuCube.Services;

public class SecurityService : ISecurityService
{
    public async Task<string> GenerateJwt<T>(T obj)
    {
        string password = await getPassword();
        var base64Password = ToBase64(password);
        var jwt = getJwt(obj, base64Password);
        return jwt;
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

    public async Task<T> ValidadeJwt<T>(string jwt)
    {
        var data = jwt.Split('.');

        var header = data[0];
        var payload = data[1];
        var signature = data[2];

        var password = await getPassword();
        var base64Password = ToBase64(password);

        var generatedSignature = getSignature(header, payload, base64Password);
        if (generatedSignature != signature)
        {
            return default(T);
        }

        var payloadBytes = Convert.FromBase64String(payload);
        var payloadJson = Encoding.UTF8.GetString(payloadBytes);

        var obj = JsonSerializer.Deserialize<T>(payloadJson);
        return obj;
    }

    private byte[] getRandomArray()
    {
        byte[] randomBytes = new byte[24];
        Random.Shared.NextBytes(randomBytes);
        return randomBytes;
    }

    private byte[] getHash(byte[] saltBytes, byte[] passwordBytes)
    {
        const int iterationsCount = 1000;
        using var hashAlgorithm = new Rfc2898DeriveBytes(
            passwordBytes, saltBytes, iterationsCount
        );
        var hashBytes = hashAlgorithm.GetBytes(32);
        return hashBytes;
    }

    private async Task<string> getPassword()
    {
        var dotEnvPath = Path.Combine(
            Environment.CurrentDirectory,
            ".env"
        );
        var lines = await File.ReadAllLinesAsync(dotEnvPath);
        foreach (var line in lines)
        {
            var data = line.Split('=');
            if (data[0] != "PASSWORD")
                continue;
            
            return data[1];
        }
        throw new Exception(
            "É necessário um .env com uma PASSWORD para executar está operação."
        );
    }

    private string ToBase64(string text)
    {
        var bytes = Encoding.UTF8.GetBytes(text);
        var base64 = Convert.ToBase64String(bytes);
        var base64withoutPadding = base64.Replace("=", "");
        return base64withoutPadding;
    }

    private string getJwt<T>(T obj, string password)
    {
        var header = getJsonHeader();
        var headerBase64 = ToBase64(header);

        var payload = getJsonPayload(obj);
        var payloadBase64 = ToBase64(payload);

        var signature = getSignature(headerBase64, payloadBase64, password);

        return $"{headerBase64}.{payloadBase64}.{signature}";
    }

    private string getJsonHeader()
    {
        var headerObj = new {
            alg = "HS256",
            typ = "JWT"
        };
        var json = JsonSerializer
            .Serialize(headerObj);
        return json;
    }

    private string getJsonPayload<T>(T obj)
    {
        string json = JsonSerializer.Serialize(obj);
        return json;
    }

    private object getSignature(
        string header,
        string payload,
        string password)
    {
        var passwordBytes = Convert
            .FromBase64String(ToBase64(password));

        var content = $"{header}.{payload}";
        var contentBytes = Encoding
            .UTF8.GetBytes(content);

        using var algorithm = new HMACSHA256(passwordBytes);
        var signatureBytes = algorithm.ComputeHash(contentBytes);
        var signature = Convert.ToBase64String(signatureBytes);
        signature = signature.Replace("=", "");

        return signature;
    }

}