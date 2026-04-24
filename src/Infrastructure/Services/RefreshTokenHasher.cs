using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Services;

public class RefreshTokenHasher : IRefreshTokenHasher
{
    public string Hash(string token)
    {
        return Convert.ToBase64String(
            SHA256.HashData(Encoding.UTF8.GetBytes(token))
        );
    }

    public bool Verify(string token, string hash)
    {
        byte[] computed = SHA256.HashData(Encoding.UTF8.GetBytes(token));
        byte[] stored = Convert.FromBase64String(hash);

        return CryptographicOperations.FixedTimeEquals(computed, stored);
    }
}
