using PressureHeight.ListHelper;
using System;
using System.Collections.ObjectModel;
using Windows.Devices.Sensors;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Diagnostics;
using Windows.UI.ViewManagement;
using Windows.UI;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.UI.Popups;
using DataBase;
using Windows.Devices.Geolocation;
using PressureHeight.view;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace PressureHeight
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ObservableCollection<Item> items;
        private Altimeter alti;
        private DeviceUseTrigger _deviceUseTrigger;
        // 注册的后台任务对象
        private IBackgroundTaskRegistration _deviceUseBackgroundTaskRegistration =null;
        // Used for refreshing the number of samples received when the app is visible
        // 注册的后台任务对象
        private IBackgroundTaskRegistration gpsTask = null;

        public const string Travel_TaskName = "BlogFeedBackgroundTask";
        public const string Travel_TaskEntryPoint = "BackgroundTasks.BlogFeedBackgroundTask";

        public const string GPS_TaskName = "LocationBackgroundTask";
        public const string GPS_TaskEntryPoint = "BackgroundTasks.LocationBackgroundTask";
        string name = "";
        public MainPage()
        {            
            StatusBar statusBar = StatusBar.GetForCurrentView();
            statusBar.BackgroundColor = Colors.CadetBlue;
            statusBar.ForegroundColor = Colors.White; // 前景色
            statusBar.BackgroundOpacity = 1; // 透明度
            //statusBar.ProgressIndicator.Text = "FontAwesome Offline Reference (v4.2.0)"; // 文本
            statusBar.ProgressIndicator.ProgressValue = 0;
            statusBar.ProgressIndicator.ShowAsync();
            this.InitializeComponent();
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            items = new ObservableCollection<Item>();
            Height_List.ItemsSource = items;
            Connect__Button.Click += Connect__Button_Click;
            Close_Button.Click += Close_Button_Click;
            Open_Button.Click += Open_Button_Click;
            new Geolocator().GetGeopositionAsync();
            
        }

        private  void Open_Button_Click(object sender, RoutedEventArgs e)
        {
            alti = Altimeter.GetDefault();
            if (null != alti)
            {
                _deviceUseTrigger = new DeviceUseTrigger();
            }
            openBackgroup();
        }
        

        private async void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new NameDialog()
            {
                Title = "消息提示",
                PrimaryButtonText = "确定",
                SecondaryButtonText = "取消",
                FullSizeDesired = false,
            };
            
            dialog.PrimaryButtonClick += (sende,args) =>
            {
                name = sende.Content.ToString();               
            };
            ContentDialogResult result =  await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                Window.Current.VisibilityChanged -= new WindowVisibilityChangedEventHandler(VisibilityChanged);
                Open_Button.IsEnabled = true;
                Close_Button.IsEnabled = false;
                closeBackgroundTask();
                end(name);
                Debug.WriteLine("Background task was canceled");
            }
        }



        private async void RegisterBackgroundTask()
        {
            Window.Current.VisibilityChanged += new WindowVisibilityChangedEventHandler(VisibilityChanged);
            if (null != alti)
            {
                // Make sure this app is allowed to run background tasks.
                // RequestAccessAsync must be called on the UI thread.
                BackgroundExecutionManager.RemoveAccess();
                BackgroundAccessStatus accessStatus = await BackgroundExecutionManager.RequestAccessAsync();
                if (accessStatus == BackgroundAccessStatus.Denied)
                {
                    await new MessageDialog("后台任务被禁止!").ShowAsync();
                    return;
                }

                if (await StartSensorBackgroundTaskAsync(alti.DeviceId))
                {
                    Open_Button.IsEnabled = false;
                    Close_Button.IsEnabled = true;
                }

            }
            else
            {
                Debug.WriteLine("BackgroundNo accelerometer found");
            }
        }

        /// <summary>
        /// Starts the sensor background task.
        /// </summary>
        /// <param name="deviceId">Device Id for the sensor to be used by the task.</param>
        /// <param name="e"></param>
        /// <returns>True if the task is started successfully.</returns>
        private async Task<bool> StartSensorBackgroundTaskAsync(String deviceId)
        {
            bool started = false;
            // Make sure only 1 task is running.
            FindAndCancelExistingBackgroundTask(Travel_TaskName);
            // Register the background task.
            var backgroundTaskBuilder = new BackgroundTaskBuilder()
            {
                Name = Travel_TaskName,
                TaskEntryPoint = Travel_TaskEntryPoint
            };

            backgroundTaskBuilder.SetTrigger(_deviceUseTrigger);
            _deviceUseBackgroundTaskRegistration = backgroundTaskBuilder.Register();

            // Make sure we're notified when the task completes or if there is an update.
            _deviceUseBackgroundTaskRegistration.Completed += new BackgroundTaskCompletedEventHandler(OnBackgroundTaskCompleted);
            try
            {
                // Request a DeviceUse task to use the accelerometer.
                DeviceTriggerResult deviceTriggerResult = await _deviceUseTrigger.RequestAsync(deviceId);

                switch (deviceTriggerResult)
                {
                    case DeviceTriggerResult.Allowed:
                        Debug.WriteLine("Background task started");
                        started = true;
                        break;

                    case DeviceTriggerResult.LowBattery:
                        Debug.WriteLine("Insufficient battery to run the background task");
                       break;

                    case DeviceTriggerResult.DeniedBySystem:
                        // The system can deny a task request if the system-wide DeviceUse task limit is reached.
                        Debug.WriteLine("The system has denied the background task request");
                        break;

                    default:
                        Debug.WriteLine("Could not start the background task: " + deviceTriggerResult);
                        break;
                }
            }
            catch (InvalidOperationException)
            {
                // If toggling quickly between 'Disable' and 'Enable', the previous task
                // could still be in the process of cleaning up.
                Debug.WriteLine("A previous background task is still running, please wait for it to exit");
              FindAndCancelExistingBackgroundTask(Travel_TaskName);
            }

            return started;
        }

        private async void StartGPSBackgroundTaskAsync()
        {
            try
            {
                var access = await BackgroundExecutionManager.RequestAccessAsync();
                if (access == BackgroundAccessStatus.Denied)
                {
                    await new MessageDialog("后台任务被禁止!").ShowAsync();
                    return;
                }
                //  注册一个在锁屏上定时15分钟执行的后台任务
                var backgroundTaskBuilder = new BackgroundTaskBuilder()
                {
                    Name = GPS_TaskName,
                    TaskEntryPoint = GPS_TaskEntryPoint
                };                
                var trigger = new TimeTrigger(15, false);
                backgroundTaskBuilder.SetTrigger(trigger);
                gpsTask = backgroundTaskBuilder.Register();
                // 注册后台任务完成事件
                //gpsTask.Completed += new BackgroundTaskCompletedEventHandler(oneTwo);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("error:"+ex.Message);
            }
        }
        

        /// <summary>
        /// This is the background task completion handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnBackgroundTaskCompleted(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs e)
        {           
            // Dispatch to the UI thread to display the output.
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                // An exception may be thrown if an error occurs in the background task.
                try
                {
                    e.CheckResult();
                    if (ApplicationData.Current.LocalSettings.Values.ContainsKey("TaskCancelationReason"))
                    {
                        string cancelationReason = (string)ApplicationData.Current.LocalSettings.Values["TaskCancelationReason"];
                        Debug.WriteLine("Background task was stopped, reason: " + cancelationReason);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception in background task: " + ex.Message);
               }
            });

            // Unregister the background task and let the remaining task finish until completion.
            if (null != _deviceUseBackgroundTaskRegistration)
            {
                _deviceUseBackgroundTaskRegistration.Unregister(false);
                _deviceUseBackgroundTaskRegistration = null;
            }
        }

        private async void end(string name)
        {
            DataManager dm = DataManager.getInstance();
            var settings = ApplicationData.Current.LocalSettings;
            double  nowH= (double)settings.Values["nowH"];
            int startRS=(int)settings.Values["startRS"];
            int startWS= (int)settings.Values["startWS"];
            var aaa = await Pedometer.GetDefaultAsync();
            var aa = aaa.GetCurrentReadings();
            int rs=aa[PedometerStepKind.Running].CumulativeSteps;
            int ws = aa[PedometerStepKind.Walking].CumulativeSteps;
            double p = Barometer.GetDefault().GetCurrentReading().StationPressureInHectopascals;
            double h = Math.Round(nowH, 1);
            double la = 200;
            double lo = 200;
            try
            {
                Geolocator geolocator = new Geolocator();
                // 获取当前的位置
                Geoposition pos = await geolocator.GetGeopositionAsync();
                //纬度
                la = pos.Coordinate.Point.Position.Latitude;
                //经度
                lo = pos.Coordinate.Point.Position.Longitude;
            }
            catch (UnauthorizedAccessException)
            {
                //未授权的访问异常
            }
            catch (Exception ex)
            {
                //超时 time out
            }
            dm.end(la, lo, h, p, ws, rs, (ws - startWS) + (rs - startRS),name);
            dm.closeDB();
        }

        //查找是否有后台任务，有则关闭
        private void FindAndCancelExistingBackgroundTask(string name)
        {
            foreach (var backgroundTask in BackgroundTaskRegistration.AllTasks.Values)
            {
                if (name == backgroundTask.Name)
                {
                    ((BackgroundTaskRegistration)backgroundTask).Unregister(true);
                    break;
                }
            }
        }

        /// <summary>
        /// This is the tick handler for the Refresh timer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshTimer_Tick(object sender, object e)
        {
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("alti"))
            {                
                double sampleCount = (double)ApplicationData.Current.LocalSettings.Values["alti"];
                Height_B.Text = sampleCount.ToString(System.Globalization.CultureInfo.CurrentCulture);
            }
            else
            {
                Height_B.Text = "No data";
            }
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("barometer"))
            {
                double bCount = (double)ApplicationData.Current.LocalSettings.Values["barometer"];
                Pressure_B.Text = bCount.ToString(System.Globalization.CultureInfo.CurrentCulture);
            }
            else
            {
                Pressure_B.Text = "No data";
            }
        }

        //打开后台任务
        private void openBackgroup()
        {
            // Store a setting for the background task to read
            ApplicationData.Current.LocalSettings.Values["IsAppVisible"] = true;
            // 获取后台任务是否已经注册，如果已经注册则获取其注册对象
            foreach (var cur in BackgroundTaskRegistration.AllTasks)
            {
                if (cur.Value.Name == Travel_TaskName)
                {
                    _deviceUseBackgroundTaskRegistration = cur.Value;
                    break;
                }
                else if (cur.Value.Name == GPS_TaskName)
                {
                    gpsTask = cur.Value;
                    break;
                }
            }
            if (_deviceUseBackgroundTaskRegistration != null)
            {
                _deviceUseBackgroundTaskRegistration.Completed += new BackgroundTaskCompletedEventHandler(OnBackgroundTaskCompleted);
            }
            else
            {
                // 如果未注册后台任务，则注册后台任务
                RegisterBackgroundTask();
            }
            if (gpsTask == null)
            {
                // 如果未注册后台任务，则注册后台任务
                StartGPSBackgroundTaskAsync();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Frame rootFrame = Window.Current.Content as Frame;
            base.OnNavigatedTo(e);
            
        }

        /// <summary>
        /// Invoked immediately before the Page is unloaded and is no longer the current source of a parent Frame.
        /// </summary>
        /// <param name="e">
        /// Event data that can be examined by overriding code. The event data is representative
        /// of the navigation that will unload the current Page unless canceled. The
        /// navigation can potentially be canceled by setting Cancel.
        /// </param>
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            Window.Current.VisibilityChanged -= new WindowVisibilityChangedEventHandler(VisibilityChanged);
            ApplicationData.Current.LocalSettings.Values["IsAppVisible"] = false;
            base.OnNavigatingFrom(e);
        }

        private void VisibilityChanged(object sender, VisibilityChangedEventArgs e)
        {
            if (Close_Button.IsEnabled)
            {
                ApplicationData.Current.LocalSettings.Values["IsAppVisible"] = e.Visible;
            }
        }
        private void Connect__Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Login));
        }       

        private DateTime dtBackTimeFirst;
        private DateTime dtBackTimeSecond;

        private  void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            dtBackTimeSecond = DateTime.Now;
            TimeSpan ts = dtBackTimeSecond - dtBackTimeFirst;
            if (ts >= new TimeSpan(0, 0, 2))
            {
                //XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
                //XmlNodeList elements = toastXml.GetElementsByTagName("text");
                //elements[0].AppendChild(toastXml.CreateTextNode("再按一次返回键退出程序 8-)"));

                ////DateTime dt = DateTime.Now.AddSeconds(5);
                ////ScheduledToastNotification toast = new ScheduledToastNotification(toastXml, dt)

                //ToastNotification toast = new ToastNotification(toastXml);
                ////toast.ExpirationTime = dt;
                //ToastNotificationManager.CreateToastNotifier().Show(toast);
                Tip_Storyboard.Begin();
                e.Handled = true;
                dtBackTimeFirst = dtBackTimeSecond;
            }
            else
            {
                Window.Current.VisibilityChanged -= new WindowVisibilityChangedEventHandler(VisibilityChanged);
                closeBackgroundTask();
                Debug.WriteLine("Background task was canceled");
                Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
                Application.Current.Exit();
            }
        }
        //关闭后台任务
        private void closeBackgroundTask()
        {
            if (null != _deviceUseBackgroundTaskRegistration)
            {
                // Cancel and unregister the background task from the current app session.
                _deviceUseBackgroundTaskRegistration.Unregister(true);
                _deviceUseBackgroundTaskRegistration = null;
            }
            else
            {
                // Cancel and unregister the background task from the previous app session.
                FindAndCancelExistingBackgroundTask(Travel_TaskName);
            }
            if (null != gpsTask)
            {
                gpsTask.Unregister(true);
                gpsTask = null;
            }
            else
            {
                FindAndCancelExistingBackgroundTask(GPS_TaskName);
            }
        }


        ////气压变化
        //private async void Barometer_Readingchanged(Barometer sender, BarometerReadingChangedEventArgs args)
        //{
        //    //StringBuilder sb = new StringBuilder();
        //    //Debug.WriteLine("当前气压读数为：{0}", args.Reading.StationPressureInHectopascals);

        //    //sb.AppendFormat("X={0:F2}rn", e.X);
        //    Task.Delay(40000).Wait(); 

        //    await Dispatcher.RunIdleAsync((IdleDispatchedHandlerArgs e) =>
        //   {
        //       Pressure_B.Text = args.Reading.StationPressureInHectopascals.ToString();
        //   });

        //}

    }

}
