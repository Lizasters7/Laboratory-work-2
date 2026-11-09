using System.Text;

namespace Dormitory;
public static class ResultFormatter
{
    public static string Format(IXmlSearchStrategy strategy, SearchCriteria criteria, List<Student> students)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Спосіб аналізу: {strategy.Name}");
        if (strategy is DomSearchStrategy)
            sb.AppendLine($"Згенерований XPath: {DomSearchStrategy.BuildXPath(criteria)}");
        sb.AppendLine($"Знайдено: {students.Count}");
        sb.AppendLine(new string('-', 50));

        foreach (var s in students)
        {
            sb.Append(s);
            sb.AppendLine(new string('-', 50));
        }
        return sb.ToString();
    }
}