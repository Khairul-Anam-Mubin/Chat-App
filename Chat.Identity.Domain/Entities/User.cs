using Chat.Framework.Database.ORM.Interfaces;
using Chat.Framework.DDD;
using Chat.Framework.Results;
using Chat.Framework.Security;
using Chat.Identity.Domain.DomainEvents;
using System.ComponentModel.DataAnnotations;

namespace Chat.Identity.Domain.Entities;

public class User : AggregateRoot, IRepositoryItem
{

#region public properties

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

    public string PasswordHash { get; private set; }

    public string PasswordSalt { get; private set; }

    public bool IsEmailVerified { get; private set; }

    #endregion

    private User(string firstName, string lastName, DateTime birthDay, string email, string password)
        : base(Guid.NewGuid().ToString())
    {
        FirstName = firstName;
        LastName = lastName;
        UserName = $"{firstName}-{lastName}";
        BirthDay = birthDay;
        Email = email;
        IsEmailVerified = false;
        (PasswordHash, PasswordSalt) = GeneratePasswordHash(password);
    }

    public static IResult<User> Create(string firstName, string lastName, DateTime birthDay, string email, string password, bool isAlreadyExist)
    {
        if (isAlreadyExist)
        {
            return Result.Error<User>("UserProfile email or id already exists!!");
        }

        var user = new User(firstName, lastName, birthDay, email, password);

        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));

        var userCreatedResult = Result.Success(user);

        return userCreatedResult;
    }

    public IResult LogIn(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            return Result.Error("PasswordHash Empty");
        }

        if (!IsPasswordMatched(password))
        {
            return Result.Error("Incorrect password");
        }

        if (!IsEmailVerified)
        {
            return Result.Error("Email not verified yet. Please check your email to verify.");
        }

        return Result.Success();
    }

    public IResult Update(string? firstName, string? lastName, DateTime? birthDay, string? about, string? profilePictureId)
    {
        var updateInfoCount = 0;

        if (!string.IsNullOrEmpty(firstName) && FirstName != firstName)
        {
            FirstName = firstName;
            updateInfoCount++;
        }

        if (!string.IsNullOrEmpty(lastName) && LastName != lastName)
        {
            LastName = lastName;
            updateInfoCount++;
        }

        if (birthDay is not null && !BirthDay.Equals(birthDay))
        {
            BirthDay = (DateTime)birthDay;
            updateInfoCount++;
        }

        if (!string.IsNullOrEmpty(about) && !about.Equals(About))
        {
            About = about;
            updateInfoCount++;
        }

        if (!string.IsNullOrEmpty(profilePictureId) && ProfilePictureId != profilePictureId)
        {
            ProfilePictureId = profilePictureId;
            updateInfoCount++;
        }

        if (updateInfoCount == 0)
        {
            return Result.Error("No Information updated");
        }

        return Result.Success();
    }

    public IResult ChangePassword(string previousPassword, string newPassword)
    {
        if (!IsPasswordMatched(previousPassword))
        {
            return Result.Error("Incorrect password.");
        }

        (PasswordHash, PasswordSalt) = GeneratePasswordHash(newPassword);

        return Result.Success();
    }

    #region private methods

    private bool IsPasswordMatched(string password)
    {
        var passwordHash = PasswordHelper.GetPasswordHash(password, PasswordSalt);

        return passwordHash.Equals(PasswordHash);
    }

    private (string PasswordHash, string PasswordSalt) GeneratePasswordHash(string password)
    {
        var salt = PasswordHelper.GenerateRandomSalt(64);
        var passwordHash = PasswordHelper.GetPasswordHash(password, salt);

        return (passwordHash, salt);
    }
#endregion
}