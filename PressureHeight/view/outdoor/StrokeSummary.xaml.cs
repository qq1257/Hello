using DataBase;
using DataBase.database;
using PressureHeight.bdMap;
using PressureHeight.util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Windows.Data.Json;
using Windows.Networking.Connectivity;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.Web.Http;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace PressureHeight.view.outdoor
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class StrokeSummary : Page
    {
        private FrameworkElement felement = null;
        private double buttonListW = 0;
        private Boolean isGetW = false;
        private ObservableCollection<TravelSummary> tsL = null;
        DataManager dm;
        public StrokeSummary()
        {
            this.InitializeComponent();
            NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;
            init();
            //ManipulationCompleted += StrokeSummary_ManipulationCompleted;//订阅手势滑动结束后的事件
            //ManipulationDelta += StrokeSummary_ManipulationDelta;//订阅手势滑动事件
        }

        private async void init()
        {
            dm = DataManager.getInstance();
            var summarys = dm.selectTravelSummary();
            foreach (TravelSummary summary in summarys)
            {
                var gpss = dm.selectTravelGPS(summary.ID, summary.startLatitude, summary.startLongitude);
                GeocoderResult geocoder;
                if (gpss.Count > 0)
                {
                    geocoder = await getGPS(gpss.First(), summary.ID);
                    if (geocoder != null)
                    {
                        summary.sAddress = geocoder.formattedAddress;
                        summary.sDescription = geocoder.sematicDescription;
                    }
                }
                gpss = dm.selectTravelGPS(summary.ID, summary.endLatitude, summary.endLongitude);
                if (gpss.Count > 0)
                {
                    geocoder = await getGPS(gpss.First(), summary.ID);
                    if (geocoder != null)
                    {
                        summary.eAddress = geocoder.formattedAddress;
                        summary.eDescription = geocoder.sematicDescription;
                    }
                }
            }
            tsL = new ObservableCollection<TravelSummary>(summarys);
            //dm.closeDB();
            TravelSummary_L.ItemsSource = tsL;
        }


        private async System.Threading.Tasks.Task<GeocoderResult> getGPS(TravelGPS gps, int tableId)
        {
            GeocoderResult geocoder = null;
            if (gps.address == null && gps.laitude != 200 && gps.longitude != 200)
            {
                geocoder = await getHttp(gps.laitude, gps.longitude);
                if (geocoder != null)
                {
                    dm.updataGPS(tableId, gps.ID, geocoder.formattedAddress, geocoder.business, geocoder.cityCode, geocoder.sematicDescription, geocoder.componentJ.Stringify());
                }
            }
            else if(gps.address!=null)
            {
                geocoder = new GeocoderResult();
                geocoder.formattedAddress = gps.address;
                geocoder.sematicDescription = gps.description;
            }
            return geocoder;

        }

        private void NetworkInformation_NetworkStatusChanged(object sender)
        {
            var b=NetworkInformation.GetInternetConnectionProfile();
            if (b != null)
            {
                //网络连接
            }
        }

        private void Grid_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            //右键弹出菜单
            MenuFlyout myFlyout = new MenuFlyout();
            MenuFlyoutItem fistItem = new MenuFlyoutItem { Text="oneItem"};
            MenuFlyoutItem secondItem = new MenuFlyoutItem { Text = "secondItem" };
            myFlyout.Items.Add(fistItem);
            myFlyout.Items.Add(secondItem);
            FrameworkElement senderElement = sender as FrameworkElement;
            myFlyout.ShowAt(sender as UIElement,e.GetPosition(sender as UIElement));
        }

        private  void TravelSummary_L_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (felement != null)
            {
                getSlideAnimation(felement, sx, 0).Begin();
                sx = 0;
                felement = null;
            }
            else
            {
                var info = e.ClickedItem as TravelSummary;
                List<TravelGPS> gpsL = dm.selectTravelGPS(info.ID);
                //List<TravelHeight> heightL = dm.selectTravelheight(info.ID);
                if (gpsL != null)
                {
                    foreach (TravelGPS gps in gpsL)
                    {
                        getGPS(gps,info.ID);
                    }
                }
            }
        }

        private void Grid_Holding(object sender, HoldingRoutedEventArgs e)
        {
            if (e.HoldingState == HoldingState.Started)
            {
                Grid a = sender as Grid;
                TextBlock b = a.Children.First() as TextBlock;

                MenuFlyout myFlyout = new MenuFlyout();
                MenuFlyoutItem fistItem = new MenuFlyoutItem { Text = "oneItem" };
                MenuFlyoutItem secondItem = new MenuFlyoutItem { Text = "secondItem" };
                myFlyout.Items.Add(fistItem);
                myFlyout.Items.Add(secondItem);
                FrameworkElement senderElement = sender as FrameworkElement;
                myFlyout.ShowAt(sender as UIElement, e.GetPosition(sender as UIElement));
            }
        }

        private void TravelSummary_L_Holding(object sender, HoldingRoutedEventArgs e)
        {
            Debug.WriteLine(e.Handled);
        }
        double sx = 0;
        double ex = 0;
        private void Grid_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (ex < 0)
            {
                felement = sender as Grid;
                if (ex < -50)
                {
                    getSlideAnimation(sender as Grid, sx, -buttonListW).Begin();
                }
                else
                {
                    getSlideAnimation(sender as Grid, sx, 0).Begin();
                }
                //sx = ex;
                ex = 0;
            }
        }

        private void Grid_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            ex += e.Delta.Translation.X;
            if (ex <-20&&ex> -buttonListW)
            {
                getSlideAnimation(sender as Grid, sx, ex).Begin();
                sx = ex;
            }
        }
        /// <summary>
        /// 动画效果
        /// </summary>
        /// <param name="element"></param>
        /// <param name="sx"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        private Storyboard getSlideAnimation( FrameworkElement element,double sx=0,double ex=0)
        {
            TranslateTransform trans = new TranslateTransform { X=sx};

            element.RenderTransform = trans;
            Storyboard sb = new Storyboard();
            //sb.BeginTime = TimeSpan.FromSeconds(0.2);
            var db = new DoubleAnimation();
            db.To = ex;
            db.From = sx;
            db.EasingFunction = new SineEase();
            db.Duration = TimeSpan.FromSeconds(0.2);
            Storyboard.SetTarget(db, trans);
            Storyboard.SetTargetProperty(db, "X");
            sb.Children.Add(db);
            return sb;
        }

        private  void Button_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement el = sender as FrameworkElement;
            switch (el.Tag.ToString())
            {
                case "top":
                    Debug.WriteLine("1111");
                    break;
                case "delete":
                    var bb = (el.Parent as FrameworkElement).Parent as FrameworkElement;
                    TravelSummary info = bb.DataContext as TravelSummary;
                    tsL.Remove(info);
                    DataManager.getInstance().removeSummary(info.ID);
                    break;
            }


        }

        private void Grid_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            if (felement != null)
            {
                getSlideAnimation(felement, sx, 0).Begin();
                sx = 0;
                felement = null;
                e.Handled = true;
            }
            if (!isGetW)
            {
                FrameworkElement el = sender as FrameworkElement;
                DependencyObject p = el.Parent;
                Grid q = p as Grid;
                UIElementCollection ql = q.Children;
                var sp = ql.First() as FrameworkElement;
                buttonListW = sp.ActualWidth;
            }           
        }

        /// <summary>
        /// 获取经纬度地址
        /// </summary>
        /// <param name="latitude">纬度</param>
        /// <param name="longitude">经度</param>
        private async System.Threading.Tasks.Task<GeocoderResult> getHttp(double latitude, double longitude)
        {
            Uri requestUri = new Uri("http://api.map.baidu.com/geocoder/v2/?ak=LUmQKZ3QBGRy8qG6H7Yg5d8G5PaN46Oe&output=json&pois=0&coordtype=wgs84ll&location=" + latitude + "," + longitude);
            HttpUtil http = new HttpUtil();
            try {
                string body = await http.get(requestUri);
                return JsonUtil.gpsToAddress(body);
            }
            catch (Exception e)
            {
                return null;
            }            
        }
    }
}
