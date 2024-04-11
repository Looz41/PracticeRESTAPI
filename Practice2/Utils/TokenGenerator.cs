using System.Security.Cryptography;

public class TokenGenerator
{
    public  string GenerateToken(int length = 32)
    {
        const string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        var rng = new RNGCryptoServiceProvider();
        var token = new char[length];
        var bytes = new byte[length];

        rng.GetBytes(bytes);

        for (int i = 0; i < length; i++)
        {
            var index = bytes[i] % allowedChars.Length;
            if (index < 0) index += allowedChars.Length;
            token[i] = allowedChars[index];
        }

        return new string(token);
    }
}