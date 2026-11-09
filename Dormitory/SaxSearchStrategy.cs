using System.Xml;
namespace Dormitory;

public class SaxSearchStrategy : IXmlSearchStrategy
{
    public string Name
    {
        get { return "SAX API (XmlReader)"; }
    }

    public List<Student> Search(string xmlPath, SearchCriteria criteria)
    {
        var result = new List<Student>();
        Student? current = null;

        using XmlReader reader = XmlReader.Create(xmlPath);
        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element && reader.Name == "Student")
            {
                current = new Student();
                bool isEmpty = reader.IsEmptyElement;  

                while (reader.MoveToNextAttribute())    
                    current.Attributes[reader.Name] = reader.Value;
                reader.MoveToElement();                 

                if (isEmpty)
                {
                    AddIfMatches(current, criteria, result);
                    current = null;
                }
            }
            else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Residence" && current != null)
            {
                current.Residences.Add(new Residence
                {
                    Building = reader.GetAttribute("Building") ?? "",
                    Room = reader.GetAttribute("Room") ?? "",
                    CheckIn = reader.GetAttribute("CheckIn") ?? "",
                    CheckOut = reader.GetAttribute("CheckOut") ?? ""
                });
            }
            else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Student" && current != null)
            {
                AddIfMatches(current, criteria, result);
                current = null;
            }
        }
        return result;
    }

    private static void AddIfMatches(Student s, SearchCriteria criteria, List<Student> result)
    {
        if (criteria.Matches(s))
            result.Add(s);
    }
}