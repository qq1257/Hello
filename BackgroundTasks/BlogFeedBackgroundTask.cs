using DataBase;
using System;
using System.Diagnostics;
using System.Threading;
using Windows.ApplicationModel.Background;
using Windows.Devices.Geolocation;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.Storage;

namespace BackgroundTasks
{
    public sealed class BlogFeedBackgroundTask : IBackgroundTask, IDisposable
    {
        private BackgroundTaskDeferral _deferral;
        //计步器
        Pedometer pedometer;
        //高度器
        Altimeter high;
        //气压计
        Barometer pressure;
        //数据库连接
        DataManager dm;
        private double nowH=0;
        private int ws=0;
        private int rs=0;
        private Timer timer;       
        // <summary> 
        // Background task entry point.
        // </summary> 
        // <param name="taskInstance"></param>
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            //alti = Altimeter.GetDefault();
            high = Altimeter.GetDefault();
            if (null != high)
            {
                TimerCallback timerc = new TimerCallback(timerTask);
                timer = new Timer(timerc, null, 1000*60*10, 1000 * 60 * 10);

                dm = DataManager.getInstance();
                //dm.setHeightTable();
                //IAsyncOperation<Pedometer> iasyncP = Pedometer.GetDefaultAsync();
                pressure = Barometer.GetDefault();

                // Select a report interval that is both suitable for the purposes of the app and supported by the sensor.
                //uint minReportIntervalMsecs = alti.MinimumReportInterval;
                //alti.ReportInterval = 50000;

                // Subscribe to accelerometer ReadingChanged events.
                 //initAlti = alti.GetCurrentReading().AltitudeChangeInMeters;
                //barometer = Barometer.GetDefault();
                //barometer.ReportInterval = 2000;
                //initBarometer = barometer.GetCurrentReading().StationPressureInHectopascals;
                //barometer.ReadingChanged += new TypedEventHandler<Barometer, BarometerReadingChangedEventArgs>(Barometer_Readingchanged);

                // Take a deferral that is released when the task is completed.
                _deferral = taskInstance.GetDeferral();
                // Get notified when the task is canceled.
                taskInstance.Canceled += new BackgroundTaskCanceledEventHandler(OnCanceled);
                //Task.Delay(500).Wait();
                // Store a setting so that the app knows that the task is running.
                //ApplicationData.Current.LocalSettings.Values["IsBackgroundTaskActive"] = true;
                //pedometer = iasyncP.GetResults();
                pedometer =await Pedometer.GetDefaultAsync();
                start();
                high.ReadingChanged += new TypedEventHandler<Altimeter, AltimeterReadingChangedEventArgs>(ReadingChanged);
                pedometer.ReadingChanged += new TypedEventHandler<Pedometer, PedometerReadingChangedEventArgs>(Pedometer_ReadingChanged);
            }
        }
        /// <summary>
        /// 计时器
        /// </summary>
        /// <param name="a"></param>
        private async void timerTask(object a)
        {
            Debug.WriteLine("11");
            Geolocator geolocator = new Geolocator();
            Geoposition pos = await geolocator.GetGeopositionAsync();
            WriteGeolocToAppdata(pos);
        }

        private void WriteGeolocToAppdata(Geoposition pos)
        {
            var position = pos.Coordinate.Point.Position;
            insertGPS(position.Latitude, position.Longitude);
        }

        private void insertGPS(double latitude, double longitude)
        {
            dm.setGPSTable();
            dm.insertGPS(TimeUtil.getNowStamp(), latitude, longitude);
        }

        /// <summary> 
        /// 步数变化
        /// </summary> 
        private void Pedometer_ReadingChanged(Pedometer sender, PedometerReadingChangedEventArgs args)
        {
            PedometerReading reding =args.Reading;
            if (reding.StepKind == PedometerStepKind.Running)
            {
                if (rs == reding.CumulativeSteps)
                    return;
                rs = reding.CumulativeSteps;
            }
            else
            {
                if (ws == reding.CumulativeSteps)
                    return;
                ws = reding.CumulativeSteps;
            }
            double h = Math.Round(nowH, 1);
            double p = pressure.GetCurrentReading().StationPressureInHectopascals;
            int s = args.Reading.CumulativeSteps;
            long time = TimeUtil.getNowStamp();
            dm.insertHeight(time, h, p, ws,rs);            
        }

        /// <summary> 
        /// Called when the background task is canceled by the app or by the system.
        /// </summary> 
        /// <param name="sender"></param>
        /// <param name="reason"></param>
        private  void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {            
            _deferral.Complete();
            dm.closeDB();
            Dispose();
        }

        /// <summary>
        /// Frees resources held by this background task.
        /// </summary>
        public void Dispose()
        {
            if (null != pedometer)
            {
                pedometer.ReadingChanged -= new TypedEventHandler<Pedometer, PedometerReadingChangedEventArgs>(Pedometer_ReadingChanged);
                pedometer.ReportInterval = 0;
            }
            if (null!=high)
            {
                high.ReadingChanged -= new TypedEventHandler<Altimeter, AltimeterReadingChangedEventArgs>(ReadingChanged);
                high.ReportInterval = 0;
            }
        }

        /// <summary>
        /// 高度变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReadingChanged(object sender, AltimeterReadingChangedEventArgs e)
        {
            nowH=e.Reading.AltitudeChangeInMeters;
            ApplicationData.Current.LocalSettings.Values["nowH"] = nowH;
        }

        //气压变化
        private void Barometer_Readingchanged(Barometer sender, BarometerReadingChangedEventArgs args)
        {
            ApplicationData.Current.LocalSettings.Values["barometer"] = args.Reading.StationPressureInHectopascals;            
        }

        private async void start()
        {
            var aa = pedometer.GetCurrentReadings();
            rs = aa[PedometerStepKind.Running].CumulativeSteps;
            ws = aa[PedometerStepKind.Walking].CumulativeSteps;
            double p = pressure.GetCurrentReading().StationPressureInHectopascals;
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
            dm.start(h, p, ws, rs, la, lo);
            var settings = ApplicationData.Current.LocalSettings;
            settings.Values["startRS"] = rs;
            settings.Values["startWS"] = ws;
        }       
    }
}