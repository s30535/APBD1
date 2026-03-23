using APBD1.Rentals;
using APBD1.Utils;

namespace APBD1.Services;

public class RentalService
{
    public int DefaultRentalDays { get; set; } = 7;
    public float LateFeePerDay { get; set; } = 10.0f;

    private readonly List<Rental> _rentals = new();
    private readonly UserService _userService;
    private readonly EquipmentService _equipmentService;

    public RentalService(UserService userService, EquipmentService equipmentService)
    {
        _userService = userService;
        _equipmentService = equipmentService;
    }

    public Rental RentEquipment(int userId, int equipmentId, DateTime? rentDate = null, int? rentalDays = null)
    {
        var user = _userService.GetById(userId)
            ?? throw new InvalidOperationException($"Uzytkownik o id {userId} nie istnieje.");

        var equipment = _equipmentService.GetById(equipmentId)
            ?? throw new InvalidOperationException($"Sprzet o id {equipmentId} nie istnieje.");

        if (equipment.Status == EquipmentStatus.Unavailable)
        {
            throw new InvalidOperationException("Sprzet jest niedostepny i nie mozna go wypozyczyc.");
        }

        if (equipment.Status == EquipmentStatus.Rented)
        {
            throw new InvalidOperationException("Sprzet jest juz wypozyczony.");
        }

        var activeRentals = _rentals.Count(r => r.UserId == userId && r.ReturnDate is null);
        if (activeRentals >= user.MaxActiveRentals)
        {
            throw new InvalidOperationException(
                $"Przekroczono limit. {user.UserType} moze miec maksymalnie {user.MaxActiveRentals} aktywne wypozyczenia.");
        }

        var startDate = rentDate ?? DateTime.Now;
        var days = rentalDays ?? DefaultRentalDays;
        var rental = new Rental(userId, equipmentId, startDate, days);

        equipment.Status = EquipmentStatus.Rented;
        _rentals.Add(rental);

        return rental;
    }

    public Rental ReturnEquipment(int rentalId, DateTime? returnDate = null)
    {
        var rental = _rentals.FirstOrDefault(r => r.Id == rentalId)
            ?? throw new InvalidOperationException($"Wypozyczenie o id {rentalId} nie istnieje.");

        if (rental.ReturnDate is not null)
        {
            throw new InvalidOperationException("To wypozyczenie zostalo juz zwrocone.");
        }

        var actualReturnDate = returnDate ?? DateTime.Now;
        var lateFee = LateFeeCalculator.CalculateLateFee(rental.DueDate, actualReturnDate, LateFeePerDay);
        rental.Return(actualReturnDate, lateFee);

        var equipment = _equipmentService.GetById(rental.EquipmentId);
        if (equipment is not null)
        {
            equipment.Status = EquipmentStatus.Available;
        }

        return rental;
    }

    public void GetActiveRentalsByUser(int userId)
    {
        var activeRentals = _rentals
            .Where(r => r.UserId == userId && r.ReturnDate is null)
            .ToList();

        if (activeRentals.Count == 0)
        {
            Console.WriteLine($"Brak aktywnych wypozyczen dla uzytkownika o id {userId}.");
            return;
        }

        foreach (var rental in activeRentals)
        {
            Console.WriteLine($"{rental.Id}. UserId: {rental.UserId}, EquipmentId: {rental.EquipmentId}, Termin zwrotu: {rental.DueDate:yyyy-MM-dd}");
        }
    }

    public void GetOverdueRentals(DateTime? asOf = null)
    {
        var date = asOf ?? DateTime.Now;
        var overdueRentals = _rentals
            .Where(r => r.ReturnDate is null && r.DueDate < date)
            .ToList();

        if (overdueRentals.Count == 0)
        {
            Console.WriteLine("Brak przeterminowanych wypozyczen.");
            return;
        }

        foreach (var rental in overdueRentals)
        {
            Console.WriteLine($"{rental.Id}. UserId: {rental.UserId}, EquipmentId: {rental.EquipmentId}, Termin zwrotu: {rental.DueDate:yyyy-MM-dd}");
        }
    }

    public string GenerateReport(DateTime? asOf = null)
    {
        var date = asOf ?? DateTime.Now;
        var allEquipment = _equipmentService.GetAllEquipmentData();
        var totalEquipment = allEquipment.Count;
        var availableEquipment = allEquipment.Count(e => e.Status == EquipmentStatus.Available);
        var rentedEquipment = allEquipment.Count(e => e.Status == EquipmentStatus.Rented);
        var unavailableEquipment = allEquipment.Count(e => e.Status == EquipmentStatus.Unavailable);

        var activeRentals = _rentals.Count(r => r.ReturnDate is null);
        var overdueRentals = _rentals.Count(r => r.ReturnDate is null && r.DueDate < date);
        var totalFees = _rentals.Where(r => r.ReturnDate is not null).Sum(r => r.LateFee);
        var usersCount = _userService.GetAllUsers().Count;

        return $"""
               Raport wypozyczalni ({date:yyyy-MM-dd HH:mm})
               Uzytkownicy: {usersCount}
               Sprzet lacznie: {totalEquipment}
               - Dostepny: {availableEquipment}
               - Wypozyczony: {rentedEquipment}
               - Niedostepny: {unavailableEquipment}
               Aktywne wypozyczenia: {activeRentals}
               Przeterminowane wypozyczenia: {overdueRentals}
               Zebrane kary: {totalFees:0.00}
               """;
    }

}

