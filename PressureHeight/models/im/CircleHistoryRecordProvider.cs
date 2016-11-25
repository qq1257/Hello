using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PressureHeight.models.im
{
    /// <summary>
    /// 圈子历史记录数据库操作类
    /// </summary>
   public class CircleHistoryRecordProvider
    {
        public void creadTable()
        {
            string sql = "create table if not exists circleHistoryRecord (msgId varchar(50) NOT NULL , circleId varchar(50) NOT NULL,content text , contentType int NOT NULL DEFAULT 0 ,time long NOT NULL,bubbleId int NOT NULL DEFAULT 0,showTemplate int NOT NULL DEFAULT 0,userId varchar(50),PRIMARY KEY(msgId,circleId))";
        }
    }
}
