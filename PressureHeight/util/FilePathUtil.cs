using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace PressureHeight.util
{
    /// <summary>
    /// 文件路径管理工具类
    /// </summary>
    public class FilePathUtil
    {
        public static string MOTION = "motion";
        public static string IM = "im";
        public static string HEADIMG = "headImg";
        public static string EMOJI = "emoji";
        public static string BUBBLE = "bubble";
        public static string DB = "db";
        /// <summary>
        /// app所在路径
        /// </summary>
        public static readonly string LOCAL_PATH = ApplicationData.Current.LocalFolder.Path;
        /// <summary>
        /// app缓存所在路径
        /// </summary>
        public static readonly string LOCAL_CACHE_PATH = ApplicationData.Current.LocalCacheFolder.Path;
        /// <summary>
        /// 文件夹类型
        /// </summary>
        public enum FolderOption
        {
            /// <summary>
            /// 运动
            /// </summary>
            Motion=0,
            /// <summary>
            /// 即时通讯
            /// </summary>
            Im =1
        }

        /// <summary>
        /// 子文件类型
        /// </summary>
        public enum SubOption
        {
            /// <summary>
            /// 头像
            /// </summary>
            HeadImg=0,
            /// <summary>
            /// 表情
            /// </summary>
            Emoji=1,
            /// <summary>
            /// 气泡
            /// </summary>
            Bubble=2,
            /// <summary>
            /// 本地缓存
            /// </summary>
            db=3
        }
        /// <summary>
        /// 获取对应模块的文件夹
        /// </summary>
        /// <param name="folderOption">根文件夹类型</param>
        /// <param name="subOption">子文件夹类型</param>
        /// <returns>返回对应模块的文件夹</returns>
        public static async Task<StorageFolder> getFolder(FolderOption folderOption, SubOption subOption)
        {
            StorageFolder folder = null;
            switch (folderOption)
            {
                case FolderOption.Motion:
                    folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(MOTION, CreationCollisionOption.OpenIfExists);
                    break;
                case FolderOption.Im:
                    folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(IM, CreationCollisionOption.OpenIfExists);
                    break;
                default:
                    return null;
            }
            StorageFolder sub = null;
            switch (subOption)
            {
                case SubOption.HeadImg:
                    sub=await folder.CreateFolderAsync(HEADIMG, CreationCollisionOption.OpenIfExists);
                    break;
                case SubOption.Emoji:
                    sub = await folder.CreateFolderAsync(EMOJI, CreationCollisionOption.OpenIfExists);
                    break;
                case SubOption.Bubble:
                    sub = await folder.CreateFolderAsync(BUBBLE, CreationCollisionOption.OpenIfExists);
                    break;
                case SubOption.db:
                    sub = await folder.CreateFolderAsync(DB, CreationCollisionOption.OpenIfExists);
                    break;
                default:
                    return null;
            }
            return sub;
        }
    }
}
