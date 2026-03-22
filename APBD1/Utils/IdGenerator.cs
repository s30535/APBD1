namespace APBD1.Utils;

public class IdGenerator
{
    private static int _equipmentId = 1;

    public static int GenerateId()
    {
        return _equipmentId++;
    }
}