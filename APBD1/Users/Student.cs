namespace APBD1.Users;

public class Student : User
{
    public string StudentId { get; set; }

    public Student(string firstName, string lastName, string studentId)
        : base(firstName, lastName)
    {
        StudentId = studentId;
    }

    public override int MaxActiveRentals => 2;
    public override string UserType => "Student";
}