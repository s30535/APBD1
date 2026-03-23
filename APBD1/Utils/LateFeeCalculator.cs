namespace APBD1.Utils;

public static class LateFeeCalculator
{
    public static float CalculateLateFee(DateTime dueDate, DateTime returnDate, float lateFeePerDay)
    {
        if (returnDate <= dueDate)
        {
            return 0;
        }

        var lateDays = (int)Math.Ceiling((returnDate - dueDate).TotalDays);
        return lateDays * lateFeePerDay;
    }
}

