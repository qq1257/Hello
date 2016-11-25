using SQLite.Net.Attributes;

namespace PressureHeight.models.im
{
    /// <summary>
    /// 私聊历史记录
    /// </summary>
    public class HistoryRecordModel
    {
        /// <summary>
        /// 消息ID
        /// </summary>
        [Column("msgId")]
        [PrimaryKey]
        string msgID { set; get; }
        /// <summary>
        /// 用户ID
        /// </summary>
        [Column("userId")]
        [PrimaryKey]
        string userId { set; get; }
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
        int contentType { set; get; }
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
