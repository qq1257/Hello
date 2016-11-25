using SQLite.Net;
using DataBase.database;
using PressureHeight.util;
using Windows.UI.Xaml;
using PressureHeight.models.im;
using System.Collections.Generic;

namespace PressureHeight.models
{
    /// <summary>
    /// 数据操作类
    /// </summary>
    public  class ImDbManage
    {
        private static ImDbManage dm = null;
        private SQLiteConnection db;
        /// <summary>
        /// 登陆用户名
        /// </summary>
        public static string LOGIN_USER = null;
        private SessionProvider sessionProvider = null;
        public static ImDbManage getInstance()
        {
            if (dm == null)
            {
                dm = new ImDbManage();
            }
            return dm;
        }
        private ImDbManage()
        {
            init();
        }
        private async void init()
        {
            if (LOGIN_USER != null)
            {
                db = new DatabaseConnection_UWP().imDbconnection((await FilePathUtil.getFolder(FilePathUtil.FolderOption.Im, FilePathUtil.SubOption.db)).Path, LOGIN_USER + ".db3");
            }
            else
            {
                Application.Current.Exit();
            }
        }

        private SessionProvider getSessionProvider()
        {
            if (sessionProvider == null)
            {
                sessionProvider = new SessionProvider(db);
            }
            return sessionProvider;
        }
        /// <summary>
        /// 添加会话数据
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="newContent">会话内容</param>
        /// <param name="time">会话时间</param>
        /// <param name="type">用户类型（0私人，1群，2圈子）</param>
        public void addSessionDate(string userId,string newContent,long time,int type=0,string statusTag=null)
        {
            SessionProvider session = getSessionProvider();
            SessionModel model= session.selectSessionModel(userId,type);
            if (model == null)
            {
                if (statusTag == null)
                {
                    session.add(userId, time, newContent, type);
                }
                else
                {
                    session.add(userId, time, newContent, statusTag);
                }                
            }
            else
            {
                session.updateContnet(userId,time,newContent,model.unreadNum+1,type);
            }
        }
        /// <summary>
        /// 设置昵称头像
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="name">用户昵称</param>
        /// <param name="headImgSuffix">头像后缀</param>
        /// <param name="type">用户类型（0私聊，1群，2圈子）</param>
        public void setNameImg(string userId, string name, string headImgSuffix, int type=0)
        {
            SessionProvider session = getSessionProvider();
            session.updateNameImg(userId,name,headImgSuffix,type);
        }
        /// <summary>
        /// 添加好友后，置空会话来源标识
        /// </summary>
        /// <param name="userId"></param>
        public void zeroStatusTag(string userId)
        {
            SessionProvider session = getSessionProvider();
            session.updateStatusTag(userId);
        }
        /// <summary>
        /// 置零会话未读数量
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        public void zeroSessionUnreadNum(string userId, int type = 0)
        {
            SessionProvider session = getSessionProvider();
            session.unreadNumZero(userId,type);
        }
        /// <summary>
        /// 获取会话列表
        /// </summary>
        /// <returns></returns>
        public List<SessionModel> getSessions()
        {
            SessionProvider session = getSessionProvider();
            return session.selectSessionModel();
        }
    }

}
