using APBD1.Utils;

namespace APBD1.Users;

public abstract class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    protected User(string firstName, string lastName)
    {
        Id = IdGenerator.GenerateUserId();
        FirstName = firstName;
        LastName = lastName;
    }

    public abstract int MaxActiveRentals { get; }
    public abstract string UserType { get; }
    public override string ToString()
    {
        return $"{Id}. {FirstName} {LastName}";
    }
}