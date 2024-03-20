using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Framework.Security;

public class PasswordHelper
{
    public static string GenerateRandomSalt(int keySize)
    {
        return RandomNumberGenerator.GetHexString(keySize);
    }

    public static string GetPasswordHash(string password, string salt)
    {
        var passwordHash = CheckSumGenerator.GetCheckSum(password, salt);
        return passwordHash;
    }
}
