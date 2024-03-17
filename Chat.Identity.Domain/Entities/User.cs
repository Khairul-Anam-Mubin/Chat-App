using Chat.Framework.Database.ORM.Interfaces;
using Chat.Framework.Results;
using System.ComponentModel.DataAnnotations;

namespace Chat.Identity.Domain.Entities;

public class User : IRepositoryItem
{
    public string Id { get; private set; }

    [Required]
    public string FirstName { get; private set; }

    [Required]
    public string LastName { get; private set; }

    public string UserName { get; private set; }

    [Required]
    public DateTime BirthDay { get; private set; }

    [EmailAddress]
    public string Email { get; private set; }

    [MaxLength(1000)]
    public string? About { get; private set; }

    public string? ProfilePictureId { get; private set; }

    public int PublicKey { get; private set; }

    public string Password { get; private set; }

    public bool IsEmailVerified { get; private set; }

    private User(string firstName, string lastName, DateTime birthDay, string email, string password)
    {
        Id = Guid.NewGuid().ToString();
        FirstName = firstName;
        LastName = lastName;
        UserName = $"{firstName}-{lastName}";
        BirthDay = birthDay;
        Email = email;
        Password = password;
        IsEmailVerified = false;
    }

    public static IResult<User> Create(string firstName, string lastName, DateTime birthDay, string email, string password, bool isAlreadyExist)
    {
        if (isAlreadyExist)
        {
            return Result.Error<User>("User email or id already exists!!");
        }

        return Result.Success(new User(firstName, lastName, birthDay, email, password));
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

    public IResult<User> Update(User requestUpdateModel)
    {
        var updateInfoCount = 0;

        if (!string.IsNullOrEmpty(requestUpdateModel.FirstName) &&
            FirstName != requestUpdateModel.FirstName)
        {
            FirstName = requestUpdateModel.FirstName;
            updateInfoCount++;
        }

        if (!string.IsNullOrEmpty(requestUpdateModel.LastName) &&
            LastName != requestUpdateModel.LastName)
        {
            LastName = requestUpdateModel.LastName;
            updateInfoCount++;
        }

        if (requestUpdateModel.BirthDay != default &&
            !BirthDay.Equals(requestUpdateModel.BirthDay))
        {
            BirthDay = requestUpdateModel.BirthDay;
            updateInfoCount++;
        }

        if (!string.IsNullOrEmpty(requestUpdateModel.About) &&
            About != requestUpdateModel.About)
        {
            About = requestUpdateModel.About;
            updateInfoCount++;
        }

        if (!string.IsNullOrEmpty(requestUpdateModel.ProfilePictureId) &&
            ProfilePictureId != requestUpdateModel.ProfilePictureId)
        {
            ProfilePictureId = requestUpdateModel.ProfilePictureId;
            updateInfoCount++;
        }

        if (updateInfoCount == 0)
        {
            return Result.Error<User>("No Information updated");
        }

        return Result.Success(this);
    }
}