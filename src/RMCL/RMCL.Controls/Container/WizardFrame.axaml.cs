using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RMCL.Controls.ControlHelper.Wizard;

namespace RMCL.Controls.Container;

public partial class WizardFrame : UserControl
{
    public List<WizardPageEntry> Wizards = new();
    private List<TextBlock> Items = new();
    public int Index { get; private set; } = 0;
    public WizardFrame()
    {
        InitializeComponent();

        this.Loaded += (sender, args) => SelectWizardItem(0);
    }

    public void RegistedWizard(WizardPageEntry entry)
    {
        Wizards.Add(entry);
        var txtb = new TextBlock()
        {
            Text = entry.Title,
            FontSize = 14,
            Margin = new Thickness(12,4,4,4),
        };
        Items.Add(txtb);
        ItemPanel.Children.Add(txtb);
        
        if (Wizards.Count == 1)
        {
            SelectWizardItem(0);
        }
    }

    public void SelectWizardItem(int index)
    {
        Index = index;
        WizardMainFrame.Content = Wizards[index].Page;

        SelBox.Width = Items[index].Bounds.Width + 10 + 5 +5;
        SelBox.Margin = new Thickness(Items[index].Bounds.X, -4,0,0);
    }
    
    public bool Next()
    {
        SelectWizardItem(Index + 1);
        return Index != Wizards.Count - 1;
    }

    public bool Last()
    {
        SelectWizardItem(Index - 1);
        return Index != 0;
    }
}