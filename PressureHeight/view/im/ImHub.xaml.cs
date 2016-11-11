using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace PressureHeight.view.im
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ImHub : Page
    {
        public ImHub()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            rootPivot.SelectionChanged += RootPivot_SelectionChanged;
        }

        private void RootPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            setView(rootPivot.SelectedIndex);
        }

        private void setView(int index)
        {
            switch (index)
            {
                case 0:
                    if (Session_Frame.Content == null)
                    {
                        Session_Frame.Content = new SessionPage();
                    }
                    break;
                case 1:
                    if (Friends_Frame.Content == null)
                    {
                        Friends_Frame.Content = new FriendsPage();
                    }
                    break;
                case 2:
                    if (Dymamic_Frame.Content == null)
                    {
                        Dymamic_Frame.Content = new DymamicPage();
                    }
                    break;
            }
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            rootFrame.Navigate(typeof(ChatPage));
        }
    }
}
