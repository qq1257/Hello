using SQLite.Net.Attributes;

namespace PressureHeight.models.im
{
    /// <summary>
    /// 群实例
    /// </summary>
    public class GroupModel
    {
        /// <summary>
        /// 群ID
        /// </summary>
        [Column("id")]
        [PrimaryKey]
        string id { set; get; }
        /// <summary>
        /// 群昵称
        /// </summary>
        [Column("name")]
        string name { set; get; }
        /// <summary>
        /// 群头像
        /// </summary>
        [Column("headImg")]
        string headImg { get; set; }
        /// <summary>
        /// 聊天背景
        /// </summary>
        [Column("chatBackground")]
        string chatBackground { set; get; }
    }
}
