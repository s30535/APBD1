namespace APBD1.Utils;

public class IdGenerator
{
    private static int _equipmentId = 1;
    private static int _userId = 1;
    private static int _rentalId = 1;

    public static int GenerateEquipmentId()
    {
        return _equipmentId++;
    }
    public static int GenerateUserId()
    {
        return _userId++;
    }
    public static int GenerateRentalId()
    {
        return _rentalId++;
    }
}