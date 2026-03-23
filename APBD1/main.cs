using APBD1.Services;

namespace APBD1;

public class main
{
    public static void Main()
    {
        var userService = new UserService();
        var equipmentService = new EquipmentService();
        var rentalService = new RentalService(userService, equipmentService)
        {
            DefaultRentalDays = 7,
            LateFeePerDay = 12.5f
        };

        Console.WriteLine("=== 11. Dodanie kilku egzemplarzy sprzetu roznych typow ===");
        var laptopA = equipmentService.AddLaptop("Dell Latitude", "Intel i7", 16);
        var laptopB = equipmentService.AddLaptop("Lenovo ThinkPad", "Ryzen 7", 32);
        var projectorA = equipmentService.AddProjector("Epson X1", "1920x1080", true);
        var projectorB = equipmentService.AddProjector("BenQ M5", "1280x720", false);
        var cameraA = equipmentService.AddCamera("Sony A7", 24, true);
        equipmentService.GetAllEquipment();

        Console.WriteLine();
        Console.WriteLine("=== 12. Dodanie kilku uzytkownikow roznych typow ===");
        var student = userService.AddStudent("Anna", "Nowak", "s12345");
        var student2 = userService.AddStudent("Piotr", "Lis", "s54321");
        var employee = userService.AddEmployee("Jan", "Kowalski", "e10001");
        Console.WriteLine($"Dodano: {student.Id} {student.FirstName} {student.LastName} ({student.UserType})");
        Console.WriteLine($"Dodano: {student2.Id} {student2.FirstName} {student2.LastName} ({student2.UserType})");
        Console.WriteLine($"Dodano: {employee.Id} {employee.FirstName} {employee.LastName} ({employee.UserType})");

        Console.WriteLine();
        Console.WriteLine("=== 13. Poprawne wypozyczenie sprzetu ===");
        var rentalOnTime = rentalService.RentEquipment(student.Id, laptopA.Id, DateTime.Now.AddDays(-2), 7);
        var rentalLate = rentalService.RentEquipment(employee.Id, projectorA.Id, DateTime.Now.AddDays(-12), 5);
        Console.WriteLine($"Wypozyczono poprawnie: rental {rentalOnTime.Id} (sprzet {rentalOnTime.EquipmentId})");
        Console.WriteLine($"Wypozyczono poprawnie: rental {rentalLate.Id} (sprzet {rentalLate.EquipmentId})");

        Console.WriteLine();
        Console.WriteLine("=== 14. Proba niepoprawnej operacji ===");
        equipmentService.MarkUnavailable(cameraA.Id);
        try
        {
            rentalService.RentEquipment(student2.Id, cameraA.Id);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Niepoprawna operacja (sprzet niedostepny): {ex.Message}");
        }

        try
        {
            rentalService.RentEquipment(student.Id, laptopB.Id);
            rentalService.RentEquipment(student.Id, projectorB.Id);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Niepoprawna operacja (przekroczony limit): {ex.Message}");
        }

        Console.WriteLine();
        Console.WriteLine("Aktywne wypozyczenia studenta po probie przekroczenia limitu:");
        rentalService.GetActiveRentalsByUser(student.Id);

        Console.WriteLine();
        Console.WriteLine("=== 15. Zwrot sprzetu w terminie ===");
        var returnedOnTime = rentalService.ReturnEquipment(rentalOnTime.Id, DateTime.Now);
        Console.WriteLine($"Zwrot rental {returnedOnTime.Id}, kara: {returnedOnTime.LateFee:0.00}");

        Console.WriteLine();
        Console.WriteLine("=== 16. Zwrot opozniony z kara ===");
        var returnedLate = rentalService.ReturnEquipment(rentalLate.Id, DateTime.Now);
        Console.WriteLine($"Zwrot rental {returnedLate.Id}, kara: {returnedLate.LateFee:0.00}");

        Console.WriteLine();
        Console.WriteLine("Przeterminowane wypozyczenia na teraz:");
        rentalService.GetOverdueRentals();

        Console.WriteLine();
        Console.WriteLine("=== 17. Raport koncowy o stanie systemu ===");
        Console.WriteLine(rentalService.GenerateReport());
        
    }
}