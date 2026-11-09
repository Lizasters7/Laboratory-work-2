using System.Xml;
namespace Dormitory;

public class DomSearchStrategy : IXmlSearchStrategy
{
    public string Name
    {
        get { return "DOM API (XmlDocument + XPath)"; }
    }

    public List<Student> Search(string xmlPath, SearchCriteria criteria)
    {
        var doc = new XmlDocument();
        doc.Load(xmlPath);

        string xpath = BuildXPath(criteria);
        XmlNodeList? nodes = doc.SelectNodes(xpath);

        var result = new List<Student>();
        if (nodes == null) return result;

        foreach (XmlNode node in nodes)
            result.Add(ToStudent(node));
        return result;
    }

    public static string BuildXPath(SearchCriteria c)
    {
        var conditions = new List<string>();

        if (c.Faculty != null) conditions.Add($"@Faculty={Literal(c.Faculty)}");
        if (c.Specialty != null) conditions.Add($"@Specialty={Literal(c.Specialty)}");
        if (c.Course != null) conditions.Add($"@Course={Literal(c.Course)}");
        if (c.Building != null) conditions.Add($"Residence[@Building={Literal(c.Building)}]");
        if (c.HasKeyword)
        {
            string kw = Literal(c.Keyword!.Trim());
            conditions.Add($"(@*[contains(., {kw})] or Residence/@*[contains(., {kw})])");
        }

        if (conditions.Count == 0)
            return "//Student";
        return $"//Student[{string.Join(" and ", conditions)}]";
    }

    private static string Literal(string value)
    {
        if (value.Contains('"'))
            return $"'{value}'";
        return $"\"{value}\"";
    }

    private static Student ToStudent(XmlNode node)
    {
        var s = new Student();
        if (node.Attributes != null)
            foreach (XmlAttribute a in node.Attributes)
                s.Attributes[a.Name] = a.Value;

        XmlNodeList? residences = node.SelectNodes("Residence");
        if (residences != null)
            foreach (XmlNode r in residences)
                s.Residences.Add(new Residence
                {
                    Building = r.Attributes?["Building"]?.Value ?? "",
                    Room = r.Attributes?["Room"]?.Value ?? "",
                    CheckIn = r.Attributes?["CheckIn"]?.Value ?? "",
                    CheckOut = r.Attributes?["CheckOut"]?.Value ?? ""
                });
        return s;
    }
}