namespace Dormitory;

public interface IXmlSearchStrategy
{
    string Name { get; }
    List<Student> Search(string xmlPath, SearchCriteria criteria);
}