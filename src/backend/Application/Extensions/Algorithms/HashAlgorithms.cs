using System.Security.Cryptography;
using System.Text;

namespace Application.Extensions.Algorithms;

public static class HashAlgorithms
{
    public static string ConvertToHash(string rawData)
    {
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawData));

        StringBuilder builder = new StringBuilder();

        foreach (var t in bytes)
        {
            builder.Append(t.ToString("x2"));
        }

        return builder.ToString();
    }
}