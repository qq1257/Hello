using DataBase;
using System;
using System.Diagnostics;
using System.Threading;
using Windows.ApplicationModel.Background;
using Windows.Devices.Geolocation;

namespace BackgroundTasks
{
    //定时器，15分钟执行一次，获取gps
    public sealed class LocationBackgroundTask : IBackgroundTask
    {
        CancellationTokenSource cts = null;
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            try
            {
                taskInstance.Canceled += new BackgroundTaskCanceledEventHandler(OnCanceled);
                if (cts == null)
                {
                    cts = new CancellationTokenSource();
                }
                CancellationToken token = cts.Token;
                Geolocator geolocator = new Geolocator();
                Geoposition pos = await geolocator.GetGeopositionAsync().AsTask(token);
                WriteGeolocToAppdata(pos);
            }
            catch (UnauthorizedAccessException)
            {
                //未授权的访问异常
            }
            catch (Exception ex)
            {
                //超时 time out
            }
            finally
            {
                cts = null;
                deferral.Complete();
            }
        }

        private void WriteGeolocToAppdata(Geoposition pos)
        {
            var position= pos.Coordinate.Point.Position;
            DataManager dm =DataManager.getInstance();
            dm.setGPSTable();
            dm.insertGPS(TimeUtil.getNowStamp(), position.Latitude, position.Longitude);
            dm.closeDB();
        }       

        private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            if (cts != null)
            {
                cts.Cancel();
                cts = null;
            }
        }
    }
}
