using DataBase;
using PressureHeight.util;
using SQLite.Net.Attributes;
using System.ComponentModel;
using System.Globalization;

namespace PressureHeight.models.im
{
    /// <summary>
    /// 会话实例
    /// </summary>
    public class SessionModel:INotifyPropertyChanged
    {
        private string headPath = null;
        private string _name=null;
        private long _timeStamp = 0;
        private string _headImgSuffix=null;
        private string _newContent = null;
        private int _unreadNum = 0;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 时间戳
        /// </summary>
        [Column("time")]
        public long timeStamp {
            set
            {
                _timeStamp = value;
                onPropertyChanged("showTime");
            }
            get { return _timeStamp; }
        }
        /// <summary>
        /// 时间展示
        /// </summary>
        public string showTime {
            get
            { return TimeUtil.getAppointDate(timeStamp).ToString("HH:mm", DateTimeFormatInfo.InvariantInfo); }
        }
        /// <summary>
        /// 昵称
        /// </summary>
        [Column("name")]
        public string name {
            set
            {
                _name = value;
                onPropertyChanged("name");
            }
            get { return _name; }
        }
        /// <summary>
        /// 头像后缀
        /// </summary>
        [Column("headImgSuffix")]
        public string headImgSuffix
        {
            set
            {
                _headImgSuffix = value;
                onPropertyChanged("headImg");
            }
            get { return _headImgSuffix; }
        }
        /// <summary>
        /// 头像相对路径
        /// </summary>
        public string headImg
        {
            get
            {
                if (headPath == null)
                {
                    headPath = FilePathUtil.LOCAL_PATH + "\\" + FilePathUtil.IM + "\\" + FilePathUtil.HEADIMG + "\\" + userId;
                }
                return headPath + "." + headImgSuffix;
            }
        }
        /// <summary>
        /// 最新的会话内容
        /// </summary>
        [Column("newContent")]
        public string newContent{
            set
            {
                _newContent = value;
                onPropertyChanged("newContent");
            }
            get { return _newContent; }
        }
        /// <summary>
        /// 未读数量
        /// </summary>
        [Column("unreadNum")]
        public int unreadNum
        {
            set
            {
                _unreadNum = value;
                onPropertyChanged("unreadNum");
            }
            get { return _unreadNum; }
        }
        /// <summary>
        /// 会话对象Id
        /// </summary>
        [Column("userId")]
        [PrimaryKey]
        public string userId { get; set; }
        /// <summary>
        /// 置顶标识
        /// 0未置顶，按照置顶先后排序（1、2、3、……）
        /// </summary>
        [Column("topMark")]
        public int topMark { get; set; }
        /// <summary>
        /// 状态标识
        /// NULL代表为好友，不为空，则代表会话来源
        /// </summary>
        [Column("statusTag")]
        public string statusTag { get; set; }
        /// <summary>
        /// 会话类型
        ///     0私聊
        ///     1群聊
        ///     2圈子
        /// </summary>
        [Column("type")]
        [PrimaryKey]
        public int sessionType { get; set; }

        private void onPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
