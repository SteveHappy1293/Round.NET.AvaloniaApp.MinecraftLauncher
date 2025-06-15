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
using RMCL.Base.Enum.ButtonStyle;
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
        var selectedIndex = PlayerManager.Player.SelectIndex;

        if (Config.Config.MainConfig.ButtonStyle.QuickChoosePlayerButton == OrdinaryButtonStyle.Default)
        {
            // 设置大图标（选中的皮肤）
            if (lst.Count > 0 && selectedIndex >= 0 && selectedIndex < lst.Count)
            {
                BigSkinIcon.Background = new ImageBrush()
                {
                    Source = SkinHelper.CropAndScaleBitmapOptimized(SkinHelper.Base64ToBitmap(lst[selectedIndex].Skin),
                        new PixelRect(8, 8, 8, 8), 4),
                    Stretch = Stretch.UniformToFill
                };
            }

            // 重置所有小图标
            SmallSkinIcon1.IsVisible = false;
            SmallSkinIcon2.IsVisible = false;
            SmallSkinIcon3.IsVisible = false;
            SmallSkinIconGroup.IsVisible = true;
            UserCount.IsVisible = false;

            // 设置小图标（未选中的皮肤）
            int smallIconIndex = 0;
            for (var i = 0; i < lst.Count; i++)
            {
                if (i == selectedIndex) continue;

                smallIconIndex++;
                if (smallIconIndex > 3) break; // 最多显示3个小图标

                var skinIcon = smallIconIndex == 1 ? SmallSkinIcon1 :
                    smallIconIndex == 2 ? SmallSkinIcon2 : SmallSkinIcon3;

                if (smallIconIndex == 3 && lst.Count > 3)
                {
                    // 如果有超过3个皮肤，第三个图标显示"+N"
                    skinIcon.Child = new TextBlock()
                    {
                        Text = $"+{lst.Count - 3}",
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Foreground = Brushes.White
                    };
                    skinIcon.Background = Brushes.DodgerBlue;
                }
                else
                {
                    // 正常显示皮肤图标
                    skinIcon.Background = new ImageBrush()
                    {
                        Source = SkinHelper.CropAndScaleBitmapOptimized(SkinHelper.Base64ToBitmap(lst[i].Skin),
                            new PixelRect(8, 8, 8, 8), 2),
                        Stretch = Stretch.UniformToFill
                    };
                }

                skinIcon.IsVisible = true;
            }
        }
        else
        {
            // 重置所有小图标
            SmallSkinIcon1.IsVisible = false;
            SmallSkinIcon2.IsVisible = false;
            SmallSkinIcon3.IsVisible = false;
            SmallSkinIconGroup.IsVisible = false;
            
            // 设置大图标
            if (lst.Count > 0 && selectedIndex >= 0 && selectedIndex < lst.Count)
            {
                BigSkinIcon.Background = new ImageBrush()
                {
                    Source = SkinHelper.CropAndScaleBitmapOptimized(SkinHelper.Base64ToBitmap(lst[selectedIndex].Skin),
                        new PixelRect(8, 8, 8, 8), 4),
                    Stretch = Stretch.UniformToFill
                };
                if (lst.Count > 1)
                {
                    UserCount.IsVisible = true;
                    UserCount.Value = lst.Count - 1;
                }
            }
        }

        // 更新玩家列表
        try
        {
            PlayerListBox.Items.Clear();
            lst.ForEach(x => { PlayerListBox.Items.Add(new ListBoxItem() { Content = x.Account.UserName }); });
            PlayerListBox.SelectedIndex = selectedIndex;
        }
        catch { }

        IsEdit = true;
    }

    private void PlayerListBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (IsEdit)
        {
            PlayerManager.Player.SelectIndex = PlayerListBox.SelectedIndex;
            PlayerManager.SaveConfig();

            UpdateUI();
        }
    }
}