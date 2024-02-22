using Chat.Domain.Shared.Entities;
using Chat.Framework.Database.ORM.Interfaces;
using Chat.Framework.Results;

namespace Chat.Identity.Domain.Models;

public class UserModel : UserProfile, IEntity
{
    public string Password { get; set; } = string.Empty;
    public bool IsEmailVerified { get; set; }

    public UserModel()
    {
        IsEmailVerified = false;
    }

    public IResult LogIn(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            return Result.Error("Password Empty");
        }
        
        if (!Password.Equals(password))
        {
            return Result.Error("Incorrect password");
        }

        if (!IsEmailVerified)
        {
            return Result.Error("Email not verified yet");
        }
        
        return Result.Success();
    }
}