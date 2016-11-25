using System;
using Windows.Storage;
using Windows.Storage.Streams;

namespace PressureHeight.util
{
    /// <summary>
    /// 文件保存工具类
    /// </summary>
    public  class FileSaveUtil
    {
        /// <summary>
        /// 文件保存
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="folder">需保存的文件夹</param>
        /// <param name="inputBuff">需保存的文件流</param>
        public async void saveFile(string fileName, StorageFolder folder, IBuffer inputBuffer)
        {
            StorageFile destinationFile = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            using (var destinationStream = await destinationFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                using (var desitinationOutputStream = destinationStream.GetOutputStreamAt(0))
                {
                    await desitinationOutputStream.WriteAsync(inputBuffer);
                }
            }
        }
    }
}
