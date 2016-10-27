﻿using DataBase;
using System;
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
            double la = -1;
            double lo = -1;
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