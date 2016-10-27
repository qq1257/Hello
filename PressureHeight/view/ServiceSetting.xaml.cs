﻿using PressureHeight.config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class ServiceSetting : Page
    {
        public ServiceSetting()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed; ;
            var settings = ApplicationData.Current.LocalSettings;
            if (settings.Values.ContainsKey(TotalStaticVariable.SERVICE_HOST))
            {
                Host_T.Text = settings.Values[TotalStaticVariable.SERVICE_HOST].ToString();
            }
            if (settings.Values.ContainsKey(TotalStaticVariable.SERVICE_PORT))
            {
                Port_T.Text = settings.Values[TotalStaticVariable.SERVICE_PORT].ToString();
            }
            if (settings.Values.ContainsKey(TotalStaticVariable.SERVICE_DOMIN))
            {
                Domin_T.Text = settings.Values[TotalStaticVariable.SERVICE_DOMIN].ToString();
            }
            base.OnNavigatedTo(e);
        }

        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            goBack();
            e.Handled = true;
        }

        private async void Submit_B_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Host_T.Text) || string.IsNullOrEmpty(Port_T.Text) || string.IsNullOrEmpty(Domin_T.Text))
            {
                ContentDialog dialog = new ContentDialog
                {
                    Title = "提示：",
                    Content = "配置信息不能为空",
                    SecondaryButtonText = "确定"
                };
                await dialog.ShowAsync();
            }
            else
            {
                var settings = ApplicationData.Current.LocalSettings;
                settings.Values[TotalStaticVariable.SERVICE_HOST] = Host_T.Text;
                settings.Values[TotalStaticVariable.SERVICE_PORT] =Port_T.Text;
                settings.Values[TotalStaticVariable.SERVICE_DOMIN] =Domin_T.Text;
                goBack();
            }
        }

        private void Cancel_B_Click(object sender, RoutedEventArgs e)
        {
            goBack();
        }

        private void goBack()
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
    }
}
