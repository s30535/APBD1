using APBD1.Utils;

namespace APBD1.Rentals;

public class Rental
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int EquipmentId { get; set; }

    public DateTime RentDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }

    public float LateFee { get; set; }

    public Rental(int userId, int equipmentId, DateTime rentDate, int days)
    {
        Id = IdGenerator.GenerateRentalId();
        UserId = userId;
        EquipmentId = equipmentId;
        RentDate = rentDate;
        DueDate = rentDate.AddDays(days);
    }

    public void Return(DateTime returnDate, float lateFee)
    {
        ReturnDate = returnDate;
        LateFee = lateFee;
    }
}