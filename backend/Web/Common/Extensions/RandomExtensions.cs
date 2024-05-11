namespace Web.Common.Extensions;
public class RandomExtensions
{
    public static string GenerateRandomCode(int length)
    {
        Random random = new Random();
        string randomNumber = string.Empty;

        for (int i = 0; i < length; i++)
            randomNumber += random.Next(0, 10).ToString();
        return randomNumber;
    }
}
