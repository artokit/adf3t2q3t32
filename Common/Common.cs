using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Common;

public static class Common
{
    public static string ToJson<T>(this T obj)
    {
        return JsonSerializer.Serialize(obj);
    }

    public static T FromJson<T>(this string json)
    {
        return JsonSerializer.Deserialize<T>(json);
    }
    
    public static string PasswordHash(string password)
    {
        var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}
