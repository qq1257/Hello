using System;

namespace DataBase
{
    public class TimeUtil
    {
        /// <summary>
        /// 获取当前时间戳（毫秒）
        /// </summary>
        /// <returns></returns>
        public static long getNowStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0,DateTimeKind.Utc);
            return Convert.ToInt64(ts.TotalMilliseconds);
        }
        public static DateTime getAppointDate(long t)
        {
            return TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1, 0, 0, 0, 0,DateTimeKind.Utc).AddMilliseconds(t),TimeZoneInfo.Local);
        }

    }
}
