using SQLite.Net.Attributes;

namespace PressureHeight.models.im
{
    /// <summary>
    /// 群成员实例
    /// </summary>
    public class GroupMemberModel
    {
        /// <summary>
        /// 圈子ID
        /// </summary>
        [Column("groupId")]
        [PrimaryKey]
        string groupId { set; get; }
        /// <summary>
        /// 用户ID
        /// </summary>
        [Column("userId")]
        [PrimaryKey]
        string userId { set; get; }
        /// <summary>
        /// 用户所在群昵称
        /// </summary>
        [Column("userName")]
        string userName { set; get; }
        /// <summary>
        /// 用户所在群头衔
        /// </summary>
        [Column("userTitle")]
        string userTitle { set; get; }
    }
}
