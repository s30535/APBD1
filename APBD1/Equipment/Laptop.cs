namespace APBD1.Equipment;

public class Laptop : Equipment
{
    public string Processor { get; set; }
    public int RamGb { get; set; }

    public Laptop(string name, string processor, int ramGb)
        : base(name)
    {
        Processor = processor;
        RamGb = ramGb;
    }
}