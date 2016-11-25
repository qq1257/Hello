using PressureHeight.util;
using SQLite.Net.Attributes;

namespace PressureHeight.models.im
{
    /// <summary>
    /// 好友实例
    /// </summary>
    public class FriendsModel
    {
        private string headPath=null;
        /// <summary>
        /// 好友ID
        /// </summary>
        [Column("id")]
        [PrimaryKey]
        string id { set; get; }
        /// <summary>
        /// 昵称
        /// </summary>
        [Column("name")]
        string name { set; get; }
        /// <summary>
        /// 头像后缀
        /// </summary>
        [Column("headImgSuffix")]
        string headImgSuffix { set; get; }
        /// <summary>
        /// 头像相对路径
        /// </summary>
        string headImg { get {
                if (headPath == null)
                {
                    headPath = FilePathUtil.LOCAL_PATH+"\\"+FilePathUtil.IM + "\\" + FilePathUtil.HEADIMG+"\\"+id;
                }
                return headPath + "." + headImgSuffix;
            } }
        /// <summary>
        /// 备注
        /// </summary>
        [Column("remarks")]
        string remarks { set; get; }
        /// <summary>
        /// 个性签名
        /// </summary>
        [Column("signature")]
        string signature { set; get; }
        /// <summary>
        /// 会话背景
        /// null为默认背景
        /// </summary>
        [Column("chatBackground")]
        string chatBackground { set; get; }
    }
}
