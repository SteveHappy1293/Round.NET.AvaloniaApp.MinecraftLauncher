public class ExceptionCategory
{
    public string Name { get; }
    public string Description { get; }
    public string Severity { get; }

    public ExceptionCategory(string name, string description, string severity)
    {
        Name = name;
        Description = description;
        Severity = severity;
    }
}