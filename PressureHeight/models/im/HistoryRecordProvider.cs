namespace PressureHeight.models.im
{
    /// <summary>
    /// 私聊历史记录数据库操作类
    /// </summary>
   public class HistoryRecordProvider
    {
        public void creadTable()
        {
            string sql = "create table if not exists historyRecord (msgId varchar(50) NOT NULL , userId varchar(50) NOT NULL,content text , contentType int NOT NULL DEFAULT 0 ,time long NOT NULL,bubbleId int NOT NULL DEFAULT 0,showTemplate int NOT NULL DEFAULT 0,PRIMARY KEY(msgId,userId))";
        }
    }
}
