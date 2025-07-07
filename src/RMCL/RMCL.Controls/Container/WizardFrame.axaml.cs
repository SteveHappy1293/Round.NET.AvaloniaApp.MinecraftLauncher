using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RMCL.Controls.ControlHelper.Wizard;

namespace RMCL.Controls.Container;

public partial class WizardFrame : UserControl
{
    public List<WizardPageEntry> Wizards = new();
    public WizardFrame()
    {
        InitializeComponent();
    }

    public void RegistedWizard(WizardPageEntry entry)
    {
        Wizards.Add(entry);
        WizardTopBar.Items.Add(new TabItem()
        {
            Header = entry.Title,
            FontSize = 16
        });

        if (Wizards.Count == 1)
        {
            WizardTopBar.SelectedIndex = 0;
            WizardMainFrame.Content = Wizards[0].Page;
        }
    }

    public bool Next()
    {
        WizardTopBar.SelectedIndex = WizardTopBar.SelectedIndex + 1;
        WizardMainFrame.Content = Wizards[WizardTopBar.SelectedIndex].Page;
        return WizardTopBar.SelectedIndex != Wizards.Count;
    }

    public bool Last()
    {
        WizardTopBar.SelectedIndex = WizardTopBar.SelectedIndex - 1;
        WizardMainFrame.Content = Wizards[WizardTopBar.SelectedIndex].Page;
        return WizardTopBar.SelectedIndex != 0;
    }
}