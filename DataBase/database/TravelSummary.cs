using SQLite.Net.Attributes;
using System.Globalization;

namespace DataBase.database
{
    [Table("Travel_Summary")]
    public class TravelSummary 
    {
        [Column("id")]
        [PrimaryKey]
        [AutoIncrement()]
        public int ID { get; set; }

        [Column("name")]
        [MaxLength(20)]
        public string name { get; set; }

        [Column("start_time")]
        [NotNull]
        public long startTime { get; set; }

        [Column("start_latitude")]
        [NotNull]
        public double startLatitude { get; set; }

        [Column("start_longitude")]
        [NotNull]
        public double startLongitude { get; set; }

        [Column("end_time")]
        public long endTime { get; set; }

        [Column("end_latitude")]
        public double endLatitude { get; set; }

        [Column("end_longitude")]
        public double endLongitude { get; set; }

        [Column("steps")]
        public long steps { get; set; }

        public string time { get
            { return formatLongToTimeStr(endTime - startTime); }
        }
        public string sTime
        {
            get
            {return TimeUtil.getAppointDate(startTime).ToString("DyyMMddHHmm", DateTimeFormatInfo.InvariantInfo); }
        }
        public string eTime
        {
            get
            { return TimeUtil.getAppointDate(endTime).ToString("DyyMMddHHmm", DateTimeFormatInfo.InvariantInfo); }
        }
        public string sAddress { set; get; }
        public string sDescription { set; get; }
        public string eAddress { set; get; }
        public string eDescription { set; get; }

        private string formatLongToTimeStr(long i)
        {
            
            int hour = 0;
            int minude = 0;
            int second = 0;
            if (i > 0)
            {
                second = (int)(i / 1000);
                if (second >= 60)
                {
                    minude = second / 60;
                    second = second % 60;
                }
                if (minude >= 60)
                {
                    hour = minude / 60;
                    minude = minude % 60;
                }
            }              
            return hour + ":" + minude + ":" + second;
        }
    }
}
