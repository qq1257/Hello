using SQLite.Net.Attributes;

namespace PressureHeight.models.im
{
    /// <summary>
    /// 群历史记录实例
    /// </summary>
    public class GroupHistoryRecordModel
    {
        /// <summary>
        /// 消息ID
        /// </summary>
        [Column("msgId")]
        [PrimaryKey]
        string msgID { set; get; }
        /// <summary>
        /// 圈子ID
        /// </summary>
        [Column("groupId")]
        [PrimaryKey]
        string groupId { set; get; }
        /// <summary>
        /// 消息内容
        /// </summary>
        [Column("content")]
        string content { set; get; }
        /// <summary>
        /// 消息类型
        /// 0发送
        /// 1接收
        /// </summary>
        [Column("contentType")]
        string contentType { set; get; }
        /// <summary>
        /// 发送者用户ID
        /// </summary>
        [Column("userId")]
        string userId { set; get; }
        /// <summary>
        /// 时间
        /// </summary>
        [Column("time")]
        long timeStamp { set; get; }
        /// <summary>
        /// 气泡ID
        /// </summary>
        [Column("bubbleId")]
        int bubbleId { set; get; }
        /// <summary>
        /// 展示模板
        /// 图片模板，文字模板，表情模板，名片模板，账单模板……
        /// </summary>
        [Column("showTemplate")]
        int showTemplate { set; get; }
    }
}
