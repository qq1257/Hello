using SQLite.Net;
using SQLite.Net.Interop;
using SQLite.Net.Platform.WinRT;
using System.Diagnostics;
using System.IO;
using Windows.Storage;

namespace DataBase.database
{
    class DatabaseConnection_UWP 
    {
        public SQLiteConnection DbConnection()
        {
            var dbName = "CustomersDb.db3";
            var path = Path.Combine(ApplicationData.Current.LocalFolder.Path, dbName);
            ISQLitePlatform platform = new SQLitePlatformWinRT();
            return new SQLiteConnection(platform, path);
        }
    }
}
