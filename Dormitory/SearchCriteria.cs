namespace Dormitory;

public class SearchCriteria
{
    public string? Keyword { get; set; }     
    public string? Faculty { get; set; }
    public string? Specialty { get; set; }
    public string? Course { get; set; }
    public string? Building { get; set; }    

    public bool HasKeyword
    {
        get { return !string.IsNullOrWhiteSpace(Keyword); }
    }

    public bool Matches(Student s)
    {
        if (Faculty != null && s.Get("Faculty") != Faculty) 
            return false;
        if (Specialty != null && s.Get("Specialty") != Specialty) 
            return false;
        if (Course != null && s.Get("Course") != Course) 
            return false;
        if (Building != null && !s.Residences.Any(r => r.Building == Building)) 
            return false;

        if (HasKeyword)
        {
            string kw = Keyword!.Trim();
            bool inStudent = s.Attributes.Values.Any(v => v.Contains(kw));
            bool inResidence = s.Residences.Any(r => r.Values.Any(v => v.Contains(kw)));
            if (!inStudent && !inResidence) 
                return false;
        }
        return true;
    }
}