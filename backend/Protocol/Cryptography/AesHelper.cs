using System.Security.Cryptography;

namespace Protocol.Cryptography;
public class AesHelper
{
    private readonly Aes aes = Aes.Create();
    private readonly HashHelper _hashHelper = new HashHelper();

    public KeySize SizeKey { get; set; } = KeySize.SIZE_256;
    public KeySize SizeBlock { get; set; } = KeySize.SIZE_128;

    // hint : ecb mode for decrypt without iv -> security :)
    public CipherMode Mode { get; set; } = CipherMode.ECB;
    public PaddingMode Padding { get; set; } = PaddingMode.PKCS7;

    // settings and default values 😟
    private void Initialize()
    {
        (aes.Mode, aes.Padding) = (Mode, Padding);
        (aes.KeySize, aes.BlockSize) = ((int)SizeKey, (int)SizeBlock);

        if (aes.Mode != CipherMode.ECB)
            LoadIv(_hashHelper.ComputeSha256Hash(GenerateIV()));

        LoadKey(_hashHelper.ComputeSha256Hash(GenerateKey()));
    }
    public AesHelper() => Initialize();

    // load key and iv with constranctor 😟
    public AesHelper(string base64key = default, string base64iv = default)
    {
        Initialize();

        if (string.IsNullOrEmpty(base64iv) == false)
            LoadIv(Convert.FromBase64String(base64iv));

        if (string.IsNullOrEmpty(base64key) == false)
            LoadKey(Convert.FromBase64String(base64key));
    }
    public void LoadKey(byte[] key)
    {
        aes.Key = key;
    }
    public void LoadIv(byte[] iv)
    {
        aes.IV = iv;
    }
    public async Task<string> Decrypt(byte[] cipher)
    {
        ICryptoTransform decryptor = aes.CreateDecryptor();

        using MemoryStream ms = new MemoryStream(cipher);
        using CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);

        using (StreamReader reader = new StreamReader(cs))
            return await reader.ReadToEndAsync();
    }

    public async Task<string> DecryptFromBase64String(string base64cipher)
        => await Decrypt(Convert.FromBase64String(base64cipher));

    public async Task<byte[]> Encrypt(string plain)
    {
        ICryptoTransform encryptor = aes.CreateEncryptor();

        using MemoryStream ms = new MemoryStream();
        using CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);

        using (StreamWriter sw = new StreamWriter(cs))
            await sw.WriteAsync(plain);

        return ms.ToArray();
    }

    public async Task<string> EncryptToBase64String(string plain)
        => Convert.ToBase64String(await Encrypt(plain));
    public async Task<string> EncryptToHexString(string plain)
        => Convert.ToHexString(await Encrypt(plain));

    public string GetBase64Key()
        => Convert.ToBase64String(aes.Key);

    public string GetBase64IV()
        => Convert.ToBase64String(aes.IV);

    public static byte[] GenerateKey()
        => Aes.Create().Key;

    public static byte[] GenerateIV()
        => Aes.Create().IV;
}
