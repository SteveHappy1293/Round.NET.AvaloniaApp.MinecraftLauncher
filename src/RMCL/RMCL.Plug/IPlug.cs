namespace RMCL.Plug;

public interface IPlug
{
    void Execute();
    string Name { get; }
}