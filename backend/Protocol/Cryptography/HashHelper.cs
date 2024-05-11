using System.Security.Cryptography;

namespace Protocol.Cryptography;
public class HashHelper
{
    public byte[] ComputeMd5Hash(ReadOnlySpan<byte> source)
    {
        return MD5.HashData(source);
    }

    public byte[] ComputeSha1Hash(ReadOnlySpan<byte> source)
    {
        return SHA1.HashData(source);
    }

    public byte[] ComputeSha256Hash(ReadOnlySpan<byte> source)
    {
        return SHA256.HashData(source);
    }

    public byte[] ComputeSha512Hash(ReadOnlySpan<byte> source)
    {
        return SHA512.HashData(source);
    }

    public string ComputeSha1HashHex(byte[] value)
    {
        return Convert.ToHexString(
            ComputeSha1Hash(new ReadOnlySpan<byte>(value)));
    }

    public string ComputeSha256HashHex(byte[] value)
    {
        return Convert.ToHexString(
            ComputeSha256Hash(new ReadOnlySpan<byte>(value)));
    }

    public string ComputeSha256HashBase64(byte[] value)
    {
        return Convert.ToBase64String(
            ComputeSha256Hash(new ReadOnlySpan<byte>(value)));
    }
    public string ComputeSha512HashBase64(byte[] value)
    {
        return Convert.ToBase64String(
            ComputeSha512Hash(new ReadOnlySpan<byte>(value)));
    }
    public string ComputeSha512HashHex(byte[] value)
    {
        return Convert.ToHexString(
            ComputeSha512Hash(new ReadOnlySpan<byte>(value)));
    }
    public string ComputeMd5HashHex(byte[] value)
    {
        return Convert.ToHexString(
            ComputeMd5Hash(new ReadOnlySpan<byte>(value)));
    }
}
