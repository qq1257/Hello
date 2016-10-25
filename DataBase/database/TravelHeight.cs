using SQLite.Net.Attributes;

namespace DataBase.database
{
    public class TravelHeight
    {
        [Column("id")]
        [PrimaryKey]
        [AutoIncrement()]
        public int ID { get; set; }

        [Column("time")]
        [NotNull]
        public long time { get; set; }

        [Column("height")]
        [NotNull]
        public double height { get; set; }

        [Column("pressure")]
        [NotNull]
        public double pressure { get; set; }

        [Column("walking_steps")]
        [NotNull]
        public long walkingSteps { get; set; }

        [Column("running_steps")]
        [NotNull]
        public long runningSteps { get; set; }
    }
}
