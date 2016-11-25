using SQLite.Net.Attributes;

namespace PressureHeight.models.im
{
    /// <summary>
    /// 圈子实例
    /// </summary>
    public class CircleModel
    {
        /// <summary>
        /// 圈子ID
        /// </summary>
        [Column("id")]
        [PrimaryKey]
        string id { set; get; }
        /// <summary>
        /// 圈子昵称
        /// </summary>
        [Column("name")]
        string name { set; get; }
        /// <summary>
        /// 圈子头像
        /// </summary>
        [Column("headImg")]
        string headImg { set; get; }
        /// <summary>
        /// 聊天背景
        /// </summary>
        [Column("chatBackground")]
        string chatBackground { set; get; }
        /// <summary>
        /// 圈子公告
        /// </summary>
        [Column("notice")]
        string notice { set; get; }
        /// <summary>
        /// 公告阅读标识 
        ///     1未读，
        ///     0已阅
        /// </summary>
        [Column("noticeRead")]
        int noticeRead { set; get; }
        /// <summary>
        /// 是否显示头衔
        /// 0不显示
        /// 1显示
        /// </summary>
        [Column("isShowTitle")]
        int isShowTitle { set; get; }
    }
}
