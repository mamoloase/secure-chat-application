using System.Text;
using Protocol.Cryptography;

namespace Protocol.Extensions;
public class AuthenticationExtensions
{
    public static async Task<string> GenerateToken()
    {
        var encrypted = await new AesHelper()
            .Encrypt(Guid.NewGuid().ToString());

        return new HashHelper().ComputeMd5HashHex(
            Encoding.UTF8.GetBytes(DateTime.Now.ToFileTime().ToString())
        ) + new HashHelper().ComputeSha512HashHex(encrypted);
    }

}