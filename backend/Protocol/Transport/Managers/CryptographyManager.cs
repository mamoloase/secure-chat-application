using Protocol.Cryptography;
using Protocol.Serializer;

namespace Protocol.Transport.Managers;
public class CryptographyManager
{
    public AesHelper _aes { get; set; }
    public RsaHelper _rsa { get; set; }

    public CryptographyManager()
    {
        (_aes, _rsa) = (
            new AesHelper(), new RsaHelper(KeySize.SIZE_512));
    }
    public async Task<string> EncryptMessage(ConnectionMessage message)
       => await _aes.EncryptToBase64String(message.Serialize());

    public async Task<ConnectionMessage> DecryptMessage(string value)
        => (await _aes.DecryptFromBase64String(value))
            .Deserialize<ConnectionMessage>() ?? null;

    public string EncryptSigniture()
        => _rsa.EncryptToBase64String(_aes.GetBase64Key());

    public bool TryLoadPublicKey(string publicKey)
        => _rsa.TryLoadPublicKey(publicKey);
}