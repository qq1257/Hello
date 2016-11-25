using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PressureHeight.models.im
{
    /// <summary>
    /// 圈子数据库操作类
    /// </summary>
   public class CircleProvider
    {
        public void creadTable()
        {
            string sql = "create table if not exists circle (id varchar(50) PRIMARY KEY, name varchar(50) NOT NULL,headImg varchar(255) , chatBackground varchar(255),notice varchar(255),noticeRead int NOT NULL DEFAULT 0,isShowTitle int NOT NULL DEFAULT 0)";
        }
    }
}
