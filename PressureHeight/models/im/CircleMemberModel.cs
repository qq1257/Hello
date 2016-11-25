using SQLite.Net.Attributes;

namespace PressureHeight.models.im
{
    /// <summary>
    /// 圈子成员实例
    /// </summary>
    public class CircleMemberModel
    {
        /// <summary>
        /// 圈子ID
        /// </summary>
        [Column("circleId")]
        [PrimaryKey]
        string circleId { set; get; }
        /// <summary>
        /// 用户ID
        /// </summary>
        [Column("userId")]
        [PrimaryKey]
        string userId { set; get; }
        /// <summary>
        /// 用户所在圈子昵称
        /// </summary>
        [Column("userName")]
        string userName { set; get; }
        /// <summary>
        /// 用户所在圈子头衔
        /// </summary>
        [Column("userTitle")]
        string userTitle { set; get; }

    }
}
