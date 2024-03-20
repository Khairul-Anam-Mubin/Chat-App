using System.Security.Claims;

namespace Chat.Framework.Identity;

public class UserIdentity
{
    public string? Id { get; private set; }
    public string? Name { get; private set; }
    public string? Email { get; private set; }
    public string? Phone { get; private set; }

    private UserIdentity(string? id, string? name, string? email, string? phone)
    {
        Id = id;
        Name = name;
        Email = email;
        Phone = phone;
    }

    public static UserIdentity? Create(string? id, string? name, string? email, string? phone)
    {
        if (string.IsNullOrEmpty(id) 
            && string.IsNullOrEmpty(name) 
            && string.IsNullOrEmpty(email) 
            && string.IsNullOrEmpty(phone))
        {
            return default;
        }

        return new UserIdentity(id, name, email, phone);
    }

    public static UserIdentity? Create(List<Claim> claims)
    {
        string? id = null, name = null, email = null, phone = null;

        var claimsDictionary = 
            claims.ToDictionary(claim => claim.Type, claim => claim.Value);

        if (claimsDictionary is not null)
        {
            claimsDictionary.TryGetValue("user_id", out id);
            claimsDictionary.TryGetValue("user_name", out name);
            claimsDictionary.TryGetValue("email", out email);
            claimsDictionary.TryGetValue("phone", out phone);
        }

        return Create(id, name, email, phone);
    }
}
