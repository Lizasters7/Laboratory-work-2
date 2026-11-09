using System.Xml.Linq;
using System.Xml.Xsl;

namespace Dormitory;

public class HtmlTransformer
{
    public string Transform(string xmlPath, string xslPath, string htmlPath)
    {
        var xslt = new XslCompiledTransform();
        xslt.Load(xslPath);                 
        xslt.Transform(xmlPath, htmlPath); 
        return File.ReadAllText(htmlPath);  
    }

    public static void SaveStudentsAsXml(IEnumerable<Student> students, string path)
    {
        var root = new XElement("Dormitory",
            new XAttribute("Name", "Результати пошуку"),
            students.Select(s => new XElement("Student",
                s.Attributes.Select(a => new XAttribute(a.Key, a.Value)),
                s.Residences.Select(r => new XElement("Residence",
                    new XAttribute("Building", r.Building),
                    new XAttribute("Room", r.Room),
                    new XAttribute("CheckIn", r.CheckIn),
                    string.IsNullOrEmpty(r.CheckOut) ? null : new XAttribute("CheckOut", r.CheckOut))))));

        new XDocument(new XDeclaration("1.0", "utf-8", null), root).Save(path);
    }
}