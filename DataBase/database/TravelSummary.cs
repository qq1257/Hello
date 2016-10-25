using SQLite.Net.Attributes;

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
    }
}
