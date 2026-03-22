namespace APBD1.Equipment;

public class Camera : Equipment
{
    public int Resolution { get; set; }
    public bool IsDigital { get; set; }

    public Camera(string name, int resolution, bool isDigital)
        : base(name)
    {
        Resolution = resolution;
        IsDigital = isDigital;
    }
}