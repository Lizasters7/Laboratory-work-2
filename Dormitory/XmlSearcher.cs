namespace Dormitory;

public class XmlSearcher
{
    private IXmlSearchStrategy _strategy;

    public XmlSearcher(IXmlSearchStrategy strategy)
    {
        _strategy = strategy;
    }

    public IXmlSearchStrategy Strategy
    {
        get { return _strategy; }
    }

    public void SetStrategy(IXmlSearchStrategy strategy)
    {
        _strategy = strategy;
    }

    public List<Student> Search(string xmlPath, SearchCriteria criteria)
    {
        return _strategy.Search(xmlPath, criteria);
    }
}