namespace APBD1.Equipment;

public class Projector : Equipment
{
    public string Resolution { get; set; }
    public bool IsPortable { get; set; }

    public Projector(string name, string resolution, bool isPortable)
        : base(name)
    {
        Resolution = resolution;
        IsPortable = isPortable;
    }
}