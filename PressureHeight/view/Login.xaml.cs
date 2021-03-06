﻿using PressureHeight.models;
using PressureHeight.view.im;
using System;
using System.Collections.Generic;
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

namespace PressureHeight.view
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Login : Page
    {
        public Login()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private void Service_B_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ServiceSetting));
        }

        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            //Frame.Navigate(typeof(MainPage));
            ImDbManage.LOGIN_USER = User_A.Text;

            Frame.Navigate(typeof(SessionPage));
        }
    }
}
