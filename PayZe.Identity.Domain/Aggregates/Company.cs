using PayZe.Shared.Abstractions;

namespace PayZe.Identity.Domain.Aggregates;

public class Company : AggregateRoot
{
    public string Name { get; private set; }
    public string ApiKey { get; private set; }
    public string HashedSecret { get; private set; }
    public string Salt { get; private set; }
    public string City { get; private set; }
    public string Email { get; private set; }
    public bool IsSystemCompany { get; private set; }

    private Company(string name, string city, string email, string apiKey, string hashedSecret, string salt)
    {
        UId = Guid.NewGuid();
        Name = name;
        ApiKey = apiKey;
        HashedSecret = hashedSecret;
        Email = email;
        City = city;
        Salt = salt;

    }
    public static Company CreateCompany(string name, string city, string email, string apiKey, string hashedSecret, string salt)
    {
        return new Company(name, city, email, apiKey, hashedSecret, salt);
    }
    public static Company CreateSystemCompany(string name, string city, string email, string apiKey, string hashedSecret, string salt)
    {
        return new Company(name, city, email, apiKey, hashedSecret, salt)
        {
            IsSystemCompany = true
        };
    }
}