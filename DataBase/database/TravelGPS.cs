using SQLite.Net.Attributes;

namespace DataBase.database
{
    public class TravelGPS
    {
        [Column("id")]
        [PrimaryKey]
        [AutoIncrement()]
        public int ID { get; set; }

        [Column("time")]
        [NotNull]
        public long time { get; set; }

        [Column("laitude")]
        [NotNull]
        public double laitude { get; set; }

        [Column("longitude")]
        [NotNull]
        public double longitude { get; set; }

        [Column("address")]
        public string address { get; set; }

        [Column("busines")]
        public string busines { get; set; }

        [Column("cityCode")]
        public int cityCode { get; set; }

        [Column("description")]
        public string description { get; set; }

        [Column("component")]
        public string component { get; set; }

    }
}
