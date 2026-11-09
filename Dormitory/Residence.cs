namespace Dormitory;

public class Residence
{
    public string Building { get; set; } = "";
    public string Room { get; set; } = "";
    public string CheckIn { get; set; } = "";
    public string CheckOut { get; set; } = "";

    public IEnumerable<string> Values
    {
        get { return new[] { Building, Room, CheckIn, CheckOut }; }
    }

    public override string ToString()
    {
        string till = string.IsNullOrEmpty(CheckOut) ? "досі" : CheckOut;
        return $"корпус {Building}, кімната {Room}: {CheckIn} – {till}";
    }
}