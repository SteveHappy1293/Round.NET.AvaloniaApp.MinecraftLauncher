using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using System;
using Avalonia.Threading;
using RMCL.Models.Classes.Manager.UserManager;

namespace RMCL.Views.Pages.Main.HomePages;

public partial class HomeQuickChoosePlayer : UserControl
{
    private IDisposable? _pointerEnterSubscription;
    private IDisposable? _pointerLeaveSubscription;

    public bool IsEdit { get; set; } = false;

    public HomeQuickChoosePlayer()
    {
        InitializeComponent();
        UpdateUI();
        SetupHoverEvents();
    }

    private void SetupHoverEvents()
    {
        // 订阅鼠标进入事件
        _pointerEnterSubscription = HoverArea.AddDisposableHandler(
            InputElement.PointerEnteredEvent,
            (sender, e) => ShowContentBox());
        
        // 订阅鼠标离开事件
        _pointerLeaveSubscription = HoverArea.AddDisposableHandler(
            InputElement.PointerExitedEvent,
            (sender, e) => HideContentBox());
    }

    private void ShowContentBox()
    {
        ContentBox.IsVisible = true;
        ContentBox.Opacity = 1;
        UpdateUI();
    }

    private void HideContentBox()
    {
        ContentBox.Opacity = 0;
        
        // 延迟隐藏以完成动画
        var timer = new System.Threading.Timer(_ =>
        {
            Dispatcher.UIThread.Post(() =>
            {
                if (ContentBox.Opacity == 0) // 确保在动画完成后才隐藏
                {
                    ContentBox.IsVisible = false;
                }
            });
        }, null, 200, System.Threading.Timeout.Infinite);
    }

    public void UpdateUI()
    {
        IsEdit = false;
        var lst = PlayerManager.Player.Accounts;
        BigSkinIcon.Background = new ImageBrush()
        {
            Source = SkinHelper.CropAndScaleBitmapOptimized(SkinHelper.Base64ToBitmap(lst[0].Skin),new PixelRect(8,8,8,8),4),
            Stretch = Stretch.UniformToFill
        };

        if (lst.Count > 2)
        {
            SmallSkinIcon1.Background = new ImageBrush()
            {
                Source = SkinHelper.CropAndScaleBitmapOptimized(SkinHelper.Base64ToBitmap(lst[1].Skin),new PixelRect(8,8,8,8),2),
                Stretch = Stretch.UniformToFill
            };
            SmallSkinIcon1.IsVisible = true;
        }

        if (lst.Count > 3)
        {
            SmallSkinIcon2.Background = new ImageBrush()
            {
                Source = SkinHelper.CropAndScaleBitmapOptimized(SkinHelper.Base64ToBitmap(lst[2].Skin),new PixelRect(8,8,8,8),2),
                Stretch = Stretch.UniformToFill
            };
            SmallSkinIcon2.IsVisible = true;
        }

        if (lst.Count == 3)
        {
            SmallSkinIcon3.Background = new ImageBrush()
            {
                Source = SkinHelper.CropAndScaleBitmapOptimized(SkinHelper.Base64ToBitmap(lst[2].Skin),new PixelRect(8,8,8,8),2),
                Stretch = Stretch.UniformToFill
            };
            SmallSkinIcon3.IsVisible = true;
        }

        if (lst.Count >= 4)
        {
            SmallSkinIcon3.Child = new TextBlock()
            {
                Text = $"+{PlayerManager.Player.Accounts.Count - 3}",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = Brushes.White
            };
            SmallSkinIcon3.Background = Brushes.DodgerBlue;
            SmallSkinIcon3.IsVisible = true;
        }
        
        PlayerListBox.Items.Clear();
        lst.ForEach(x =>
        {
            PlayerListBox.Items.Add(new ComboBoxItem() { Content = x.Account.UserName });
        });
        PlayerListBox.SelectedIndex = PlayerManager.Player.SelectIndex;

        IsEdit = true;
    }

    private void PlayerListBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (IsEdit)
        {
            PlayerManager.Player.SelectIndex = PlayerListBox.SelectedIndex;
            PlayerManager.SaveConfig();
        }
    }
}