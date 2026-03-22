using APBD1.Utils;

namespace APBD1.Equipment;

public abstract class Equipment
{
    public int Id { get; set; }
    public string Name { get; set; }
    public EquipmentStatus Status { get; set; }

    protected Equipment(string name)
    {
        Id = IdGenerator.GenerateEquipmentId();
        Status = EquipmentStatus.Available;
        Name = name;
    }
}