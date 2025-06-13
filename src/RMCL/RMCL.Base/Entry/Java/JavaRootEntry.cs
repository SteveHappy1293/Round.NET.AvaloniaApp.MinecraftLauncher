
namespace RMCL.Base.Entry.Java;

public class JavaRootEntry
{
    public List<JavaDetils> Javas { get; set; } = new();
    public int SelectIndex { get; set; } = -1;
    public bool IsAutomaticSelection { get; set; } = true;
}