namespace APBD1.Users;

public class Employee : User
{
    public string EmployeeId { get; set; }

    public Employee(string firstName, string lastName, string employeeId)
        : base(firstName, lastName)
    {
        EmployeeId = employeeId;
    }

    public override int MaxActiveRentals => 5;
    public override string UserType => "Employee";
}