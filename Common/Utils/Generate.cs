using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Common.Utils;

public sealed class Genarate
{
    public static string PasswordHash(string password, string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return string.Empty;
        }

        using var hmac = new HMACSHA512();
        hmac.Key = Encoding.UTF8.GetBytes(key);

        return Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
    }
    public static bool SslCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain,
        SslPolicyErrors sslPolicyErrors) => true;
}

