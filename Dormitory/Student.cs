using System.Text;

namespace Dormitory;

public class Student
{
    public Dictionary<string, string> Attributes { get; } = new();
    public List<Residence> Residences { get; } = new();

    public string Get(string name)
    {
        if (Attributes.ContainsKey(name))
            return Attributes[name];
        return "";
    }

    public string FullName
    {
        get { return Get("FullName"); }
    }

    private static readonly Dictionary<string, string> Labels = new()
    {
        ["FullName"] = "ПІБ",
        ["Faculty"] = "Факультет",
        ["Specialty"] = "Спеціальність",
        ["Course"] = "Курс",
        ["Group"] = "Група",
        ["Benefit"] = "Пільга",
        ["Phone"] = "Телефон"
    };

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var (name, value) in Attributes)
        {
            string label = name;
            if (Labels.ContainsKey(name))
                label = Labels[name];
            sb.AppendLine($"{label}: {value}");
        }
        sb.AppendLine("Проживання:");
        foreach (var r in Residences)
            sb.AppendLine("   " + r);
        return sb.ToString();
    }
}