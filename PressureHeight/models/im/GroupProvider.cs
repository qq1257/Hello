using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PressureHeight.models.im
{
    /// <summary>
    /// 群数据库操作类
    /// </summary>
    public class GroupProvider
    {
        public void creadTable()
        {
            string sql = "create table if not exists group (id varchar(50) PRIMARY KEY, name varchar(50) NOT NULL,headImg varchar(255) , chatBackground varchar(255))";
        }
    }
}
