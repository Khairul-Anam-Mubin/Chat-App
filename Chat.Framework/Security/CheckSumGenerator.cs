using System.Security.Cryptography;
using System.Text;

namespace Chat.Framework.Security;

public class CheckSumGenerator
{
    public static string GetCheckSum(string input)
    {
        using var md5 = MD5.Create();
        
        var hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

        var hashString = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

        return hashString;
    }

    public static string GetCheckSum(params string[] inputs)
    {
        var inputList = inputs.ToList();

        if (!inputList.Any())
        {
            return string.Empty;
        }

        inputList.Sort();

        var concatenatedInput = string.Empty;

        inputList.ForEach(input =>
        {
            concatenatedInput += input;
        });

        return GetCheckSum(concatenatedInput);
    }
}
