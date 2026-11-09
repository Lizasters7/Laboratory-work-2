using System.Xml.Linq;
namespace Dormitory;

public class LinqSearchStrategy : IXmlSearchStrategy
{
    public string Name
    {
        get { return "LINQ to XML (XDocument)"; }
    }

    public List<Student> Search(string xmlPath, SearchCriteria c)
    {
        XDocument doc = XDocument.Load(xmlPath);
        IEnumerable<XElement> query = from s in doc.Descendants("Student")
                                      select s;

        if (c.Faculty != null)
            query = from s in query
                    where (string?)s.Attribute("Faculty") == c.Faculty
                    select s;

        if (c.Specialty != null)
            query = from s in query
                    where (string?)s.Attribute("Specialty") == c.Specialty
                    select s;

        if (c.Course != null)
            query = from s in query
                    where (string?)s.Attribute("Course") == c.Course
                    select s;

        if (c.Building != null)
            query = from s in query
                    where (from r in s.Elements("Residence")
                           where (string?)r.Attribute("Building") == c.Building
                           select r).Any()
                    select s;

        if (c.HasKeyword)
        {
            string kw = c.Keyword!.Trim();
            query = from s in query
                    let inStudent = (from a in s.Attributes()
                                     where a.Value.Contains(kw)
                                     select a).Any()
                    let inResidence = (from r in s.Elements("Residence")
                                       from a in r.Attributes()
                                       where a.Value.Contains(kw)
                                       select a).Any()
                    where inStudent || inResidence
                    select s;
        }
        var students = from s in query
                       select ToStudent(s);
        return students.ToList();
    }

    private static Student ToStudent(XElement e)
    {
        var s = new Student();
        foreach (XAttribute a in e.Attributes())
            s.Attributes[a.Name.LocalName] = a.Value;

        foreach (XElement r in e.Elements("Residence"))
            s.Residences.Add(new Residence
            {
                Building = (string?)r.Attribute("Building") ?? "",
                Room = (string?)r.Attribute("Room") ?? "",
                CheckIn = (string?)r.Attribute("CheckIn") ?? "",
                CheckOut = (string?)r.Attribute("CheckOut") ?? ""
            });
        return s;
    }
}