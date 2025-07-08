using System.Security.AccessControl;

namespace RMCL.Base.Entry.Game.GameDrawer;

public class GameDrawerGroupEntry
{
    public string Name { get; set; } = "New Group";
    public string Uuid { get; set; } = Guid.NewGuid().ToString();
    public string ColorHtmlCode { get; set; } = "#fff";
    public List<GameDrawerItem> Children { get; set; } = new();
}