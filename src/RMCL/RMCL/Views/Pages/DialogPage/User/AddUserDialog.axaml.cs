using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RMCL.Base.Enum.User;

namespace RMCL.Views.Pages.DialogPage.User;

public partial class AddUserDialog : UserControl
{
    private string UserType = "msa";
    public AddUserDialog()
    {
        InitializeComponent();
    }

    public UserType GetUserType()
    {
        return UserType switch
        {
            "msa" => Base.Enum.User.UserType.Microsoft,
            "off" => Base.Enum.User.UserType.Offline
        };
    }

    private void ComboBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var List = new List<string>() { "off", "msa" };
        try
        {
            UserType = List[this.ComboBox.SelectedIndex];
        }catch{ }
    }
}