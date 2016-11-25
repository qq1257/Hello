using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PressureHeight.models.im
{
    /// <summary>
    /// 圈子成员数据库操作类
    /// </summary>
    public class CircleMemberProvider
    {
        public void creadTable()
        {
            string sql = "create table if not exists circleMember (circleId varchar(50) NOT NULL , userId varchar(50) NOT NULL,userName varchar(50) NOT NULL , userTitle varchar(50) NOT NULL,PRIMARY KEY(circleId,userId))";
        }
    }
}
