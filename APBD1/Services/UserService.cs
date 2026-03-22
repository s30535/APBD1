using APBD1.Users;

namespace APBD1.Services;

public class UserService
{
    private readonly List<User> _users = new();
    
    public Student AddStudent(string firstName, string lastName, string studentId)
    {
        var student = new Student(firstName, lastName, studentId);
        _users.Add(student);
        return student;
    }

    public Employee AddEmployee(string firstName, string lastName, string employeeId)
    {
        var employee = new Employee(firstName, lastName, employeeId);
        _users.Add(employee);
        return employee;
    }

    public User? GetById(int userId)
    {
        return _users.FirstOrDefault(u => u.Id == userId);
    }

    public IReadOnlyList<User> GetAllUsers()
    {
        return _users.AsReadOnly();
    }
}

