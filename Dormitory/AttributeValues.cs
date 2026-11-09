using System.Xml.Linq;
namespace Dormitory;

public class AttributeValues
{
    public List<string> Faculties { get; private init; } = new();
    public List<string> Specialties { get; private init; } = new();
    public List<string> Courses { get; private init; } = new();
    public List<string> Buildings { get; private init; } = new();

    public static AttributeValues ReadFromFile(string xmlPath)
    {
        XDocument doc = XDocument.Load(xmlPath);
        var students = doc.Descendants("Student").ToList();

        return new AttributeValues
        {
            Faculties = DistinctValues(students, "Faculty"),
            Specialties = DistinctValues(students, "Specialty"),
            Courses = DistinctValues(students, "Course"),
            Buildings = DistinctValues(doc.Descendants("Residence"), "Building")
        };
    }

    private static List<string> DistinctValues(IEnumerable<XElement> elements, string attribute)
    {
        var values = from e in elements
                     let value = (string?)e.Attribute(attribute)   
                     where !string.IsNullOrEmpty(value)              
                     select value!;

        var sorted = from v in values.Distinct()                    
                     orderby v                                       
                     select v;

        return sorted.ToList();
    }
}