using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PressureHeight.models.im
{
    /// <summary>
    /// 好友数据库操作类
    /// </summary>
   public class FriendsProvider
    {
        private SQLiteConnection db;
        public FriendsProvider(SQLiteConnection db)
        {
            this.db = db;
            creadTable();
        }
        private void creadTable()
        {
            string sql = "create table if not exists friends (id varchar(50) PRIMARY KEY, name varchar(50) NOT NULL,headImgSuffix varchar(10) , chatBackground varchar(255), remarks varchar(50), signature varchar(100))";
            sqlQuery(sql);
        }

        public void add(string id,string name,string headImgSuffix)
        {
            string sql = "insert into friends (id, name,headImgSuffix) values (?, ?,?)";
            sqlQuery(sql, new object[] { id,name,headImgSuffix });
        }

        private int sqlQuery(string sql, params object[] args)
        {
            SQLiteCommand cmd = db.CreateCommand(sql, args);
            return cmd.ExecuteNonQuery();
        }
    }
}
