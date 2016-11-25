using SQLite.Net;
using System.Collections.Generic;

namespace PressureHeight.models.im
{
    /// <summary>
    /// 会话列表数据库操作类
    /// </summary>
    public class SessionProvider
    {
        private SQLiteConnection db;
        public SessionProvider(SQLiteConnection db)
        {
            this.db = db;
            creadTable();
        }
        private void creadTable()
        {
            string sql = "create table if not exists session (userId varchar(50), time long NOT NULL, name varchar(50) ,headImgSuffix varchar(10) ,newContent varchar(50) NOT NULL,unreadNum int NOT NULL DEFAULT 1,topMark int NOT NULL DEFAULT 0,statusTag varchar(100),type int NOT NULL DEFAULT 0,PRIMARY KEY(userId,type))";
            sqlQuery(sql);
        }
        /// <summary>
        /// 添加会话
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="time">时间戳</param>
        /// <param name="newContent">会话内容</param>
        public void add(string userId, long time, string newContent,int type)
        {
            string sql = "insert into session (userId,time,newContent,type) values (?, ?,?,?)";
            sqlQuery(sql, new object[] { userId, time, newContent , type });
        }
        /// <summary>
        /// 非好友私聊，添加会话来源
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="time">时间戳</param>
        /// <param name="newContent">会话内容</param>
        /// <param name="statusTag">会话来源</param>
        public void add(string userId, long time, string newContent, string statusTag)
        {
            string sql = "insert into session (userId,time,newContent,statusTag) values (?, ?,?,?)";
            sqlQuery(sql, new object[] { userId, time, newContent, statusTag });
        }
        /// <summary>
        /// 更新会话内容
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="time">时间戳</param>
        /// <param name="newContent">会话内容</param>
        /// <param name="unreadNum">未读数量</param>
        public void updateContnet(string userId, long time, string newContent,int unreadNum,int type)
        {
            string sql = "UPDATE session SET time=?,newContent=?,unreadNum=? where userId=? and type=?";
            sqlQuery(sql, new object[] { time, newContent, unreadNum, userId ,type});
        }
        /// <summary>
        /// 置空会话来源标识
        /// </summary>
        /// <param name="userId"></param>
        public void updateStatusTag(string userId)
        {
            string sql = "UPDATE session SET statusTag=NULL where userId=? and type=0";
            sqlQuery(sql, new object[] { userId });
        }
        /// <summary>
        /// 未读消息置零
        /// </summary>
        /// <param name="userId">用户id</param>
        public void unreadNumZero(string userId, int type)
        {
            string sql = "UPDATE session SET unreadNum=0 where userId=? and type=?";
            sqlQuery(sql, new object[] { userId,type });
        }
       /// <summary>
       /// 更新用户昵称，头像
       /// </summary>
       /// <param name="userId">用户ID</param>
       /// <param name="name">用户昵称</param>
       /// <param name="headImgSuffix">用户头像后缀</param>
        public void updateNameImg(string userId,string name,string headImgSuffix, int type)
        {
            string sql = "UPDATE session SET name=? ,headImgSuffix=? where userId=? and type=?";
            sqlQuery(sql, new object[] { name, headImgSuffix,userId ,type});
        }
        /// <summary>
        /// 更新置顶标识
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="topMark">置顶标识</param>
        public void updateTopMark(string userId, int topMark, int type)
        {
            string sql = "UPDATE session SET topMark=? where userId=? and type=?";
            sqlQuery(sql, new object[] { topMark, userId ,type});
        }
        /// <summary>
        /// 获取整个会话列表
        /// </summary>
        /// <returns></returns>
        public List<SessionModel> selectSessionModel()
        {
            return this.select<SessionModel>("select * from session ORDER BY time DESC");
        }
        /// <summary>
        /// 获取指定用户的未读数量
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public SessionModel selectSessionModel(string userId, int type)
        {
            List < SessionModel > list= this.select<SessionModel>("select unreadNum from session where userId=? and type=?", new object[] { userId,type});
            if (list.Count==0)
            {
                return null;
            }
            return list[0];
        }

        private List<T> select<T>(string sql, params object[] args)
        {
            SQLiteCommand cmd = db.CreateCommand(sql, args);
            return cmd.ExecuteQuery<T>();
        }
        private int sqlQuery(string sql, params object[] args)
        {
            SQLiteCommand cmd = db.CreateCommand(sql, args);
            return cmd.ExecuteNonQuery();
        }
    }
}
