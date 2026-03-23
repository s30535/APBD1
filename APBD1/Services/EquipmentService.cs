using APBD1.Equipment;
using EquipmentBase = APBD1.Equipment.Equipment;

namespace APBD1.Services;

public class EquipmentService
{
    private readonly List<EquipmentBase> _equipment = new();

    public Laptop AddLaptop(string name, string processor, int ramGb)
    {
        var laptop = new Laptop(name, processor, ramGb);
        _equipment.Add(laptop);
        return laptop;
    }

    public Projector AddProjector(string name, string resolution, bool isPortable)
    {
        var projector = new Projector(name, resolution, isPortable);
        _equipment.Add(projector);
        return projector;
    }

    public Camera AddCamera(string name, int resolution, bool isDigital)
    {
        var camera = new Camera(name, resolution, isDigital);
        _equipment.Add(camera);
        return camera;
    }

    public EquipmentBase? GetById(int equipmentId)
    {
        return _equipment.FirstOrDefault(e => e.Id == equipmentId);
    }

    public void GetAllEquipment()
    {
        if (_equipment.Count == 0)
        {
            Console.WriteLine("Brak sprzetu w systemie.");
            return;
        }

        foreach (var item in _equipment)
        {
            Console.WriteLine($"{item.Id}. {item.Name} - {item.Status}");
        }
    }

    public void GetAvailableEquipment()
    {
        var availableEquipment = _equipment
            .Where(e => e.Status == EquipmentStatus.Available)
            .ToList();

        if (availableEquipment.Count == 0)
        {
            Console.WriteLine("Brak dostepnego sprzetu.");
            return;
        }

        foreach (var item in availableEquipment)
        {
            Console.WriteLine($"{item.Id}. {item.Name} - {item.Status}");
        }
    }

    public void MarkUnavailable(int equipmentId)
    {
        SetStatus(equipmentId, EquipmentStatus.Unavailable);
    }

    public void SetStatus(int equipmentId, EquipmentStatus status)
    {
        var equipment = GetById(equipmentId)
            ?? throw new InvalidOperationException($"Sprzęt o id {equipmentId} nie istnieje.");

        equipment.Status = status;
    }
    
    public List<EquipmentBase> GetAllEquipmentData()
    {
        return _equipment.ToList();
    }
}

