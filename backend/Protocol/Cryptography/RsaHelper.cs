using System.Security.Cryptography;
using System.Text;

namespace Protocol.Cryptography;
public class RsaHelper
{
    private readonly RSA rsa = RSA.Create();
    private readonly UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
    private readonly RSAEncryptionPadding encryptionPadding = RSAEncryptionPadding.Pkcs1;

    public KeySize SizeKey { get; set; } = KeySize.SIZE_1024;

    public RsaHelper()
    {
        rsa.KeySize = (int)SizeKey;
    }
    public RsaHelper(KeySize size, string publicKey = default, string privateKey = default)
    {
        rsa.KeySize = (int)SizeKey;

        if (string.IsNullOrEmpty(publicKey) == false)
        {
            var bytes = Convert.FromBase64String(publicKey);
            rsa.ImportRSAPublicKey(new ReadOnlySpan<byte>(bytes), out _);
        }
        if (string.IsNullOrEmpty(privateKey) == false)
        {
            var bytes = Convert.FromBase64String(privateKey);
            rsa.ImportRSAPrivateKey(new ReadOnlySpan<byte>(bytes), out _);
        }
    }

    public string Decrypt(byte[] input)
    {
        var decrypted = rsa.Decrypt(input, encryptionPadding);

        return unicodeEncoding.GetString(decrypted);
    }

    public string DecryptFromBase64String(string base64cipher)
    {
        return Decrypt(Convert.FromBase64String(base64cipher));
    }

    public byte[] Encrypt(string plain)
    {
        byte[] cipher = unicodeEncoding.GetBytes(plain);

        return rsa.Encrypt(cipher, encryptionPadding);
    }

    public string EncryptToBase64String(string plain)
    {
        var encrypted = Encrypt(plain);
        return Convert.ToBase64String(encrypted);
    }

    public bool GenerateKeys(out string privateKey, out string publicKey, KeySize size = KeySize.SIZE_512)
    {
        var csp = new RSACryptoServiceProvider((int)size);

        publicKey = Convert.ToBase64String(csp.ExportRSAPublicKey());
        privateKey = Convert.ToBase64String(csp.ExportRSAPrivateKey());

        return true;
    }

    public string GetPrivateKey()
    {
        return Convert.ToBase64String(rsa.ExportRSAPrivateKey());
    }
    public string GetPublicKey()
    {
        return Convert.ToBase64String(rsa.ExportRSAPublicKey());
    }
    public bool TryLoadPublicKey(string publicKey)
    {
        try
        {
            rsa.ImportSubjectPublicKeyInfo(new ReadOnlySpan<byte>(
                Convert.FromBase64String(publicKey)), out _);

            return true;
        }
        catch { return false; }
    }
    public bool TryLoadPrivateKey(string privateKey)
    {
        try
        {
            rsa.ImportRSAPrivateKey(new ReadOnlySpan<byte>(
                Convert.FromBase64String(privateKey)), out _);

            return true;
        }
        catch { return false; }
    }
}