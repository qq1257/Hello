using DataBase;
using System;
using Windows.ApplicationModel.Background;
using Windows.Devices.Geolocation;

namespace BackgroundTasks
{
    //定时器，15分钟执行一次，获取gps
    public sealed class LocationBackgroundTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            try
            {
                taskInstance.Canceled += new BackgroundTaskCanceledEventHandler(OnCanceled);
                Geolocator geolocator = new Geolocator();
                Geoposition pos = await geolocator.GetGeopositionAsync();
                WriteGeolocToAppdata(pos);
            }
            catch (UnauthorizedAccessException)
            {
                //未授权的访问异常
            }
            catch (Exception ex)
            {
                //超时 time out
                insertGPS(-1,-1);
            }
            finally
            {
                deferral.Complete();
            }
        }

        private void WriteGeolocToAppdata(Geoposition pos)
        {
            var position= pos.Coordinate.Point.Position;
            insertGPS(position.Latitude, position.Longitude);
        }

        private void insertGPS(double latitude,double longitude)
        {
            DataManager dm = DataManager.getInstance();
            dm.setGPSTable();
            dm.insertGPS(TimeUtil.getNowStamp(), latitude,longitude);
            dm.closeDB();
        }
        private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
          
        }
    }
}
