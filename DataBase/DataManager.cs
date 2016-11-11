using DataBase.database;
using SQLite.Net;
using System.Collections.Generic;
using System.Diagnostics;

namespace DataBase
{
    public class DataManager
    {
        private SQLiteConnection db;
        private string tableHeght;
        private string tableGPS;
        private static DataManager dm = null;

        public static DataManager getInstance()
        {
            if (dm == null)
            {
                dm = new DataManager();
            }
            return dm;
        }

        private DataManager()
        {
            db = new DatabaseConnection_UWP().DbConnection();
            db.CreateTable<TravelSummary>();

            //string sql = "select * from Travel_Summary";
            //SQLiteCommand cmd = db.CreateCommand(sql);
            //List<TravelSummary> reader = cmd.ExecuteQuery<TravelSummary>();
            //foreach (TravelSummary c in reader)
            //{
            //    Debug.WriteLine("==========================SUMMARY START======================================");
            //    Debug.WriteLine(c.ID + "--" + c.name + "--" + c.startTime + "--" + c.startLatitude
            //        + "--" + c.startLongitude + "--" + c.endTime + "--" + c.endLatitude
            //        + "--" + c.endLongitude + "--" + c.steps);
            //    Debug.WriteLine("======================HEIGHT START==================================");
            //    sql = "select * from heigth_" + c.ID;
            //    cmd = db.CreateCommand(sql);
            //    List<TravelHeight> reader1 = cmd.ExecuteQuery<TravelHeight>();
            //    foreach (TravelHeight d in reader1)
            //    {
            //        Debug.WriteLine(d.ID + "--" + d.time + "--" + d.height + "--" + d.pressure
            //            + "--" + d.walkingSteps + "--" + d.runningSteps);
            //    }
            //    Debug.WriteLine("======================HEIGHT END==================================");
            //    Debug.WriteLine("======================GPS START==================================");
            //    sql = "select * from gps_" + c.ID;
            //    cmd = db.CreateCommand(sql);
            //    List<TravelGPS> reader2 = cmd.ExecuteQuery<TravelGPS>();
            //    foreach (TravelGPS e in reader2)
            //    {
            //        Debug.WriteLine(e.ID + "--" + e.time + "--" + e.laitude + "--" + e.longitude);
            //    }
            //    Debug.WriteLine("======================GPS END==================================");
            //    Debug.WriteLine("==========================SUMMARY END======================================");
            //}

        }
        public List<TravelSummary> selectTravelSummary()
        {
            return this.select<TravelSummary>("select * from Travel_Summary");
        }

        public List<TravelGPS> selectTravelGPS(int id)
        {
            return this.select<TravelGPS>("select * from gps_" + id);
        }
        public List<TravelHeight> selectTravelheight(int id)
        {
            return this.select<TravelHeight>("select * from heigth_" + id);
        }

        public List<TravelGPS> selectTravelGPS(int id,double latitude,double longitude)
        {
            return this.select<TravelGPS>("select * from gps_" + id+ " where laitude=? and longitude=?", new object[] { latitude,longitude});
        }
        private List<T> select<T>(string sql ,params object[] args)
        {
            SQLiteCommand cmd = db.CreateCommand(sql,args);
            return cmd.ExecuteQuery<T>();
        }
        public void start(double height, double pressure, long ws,long rs, double latitude, double longitude)
        {
            string sql = "insert into Travel_Summary (start_time, start_latitude,start_longitude) values (?, ?,?)";
            long time = TimeUtil.getNowStamp();
            sqlQuery(sql, new object[] { time, latitude, longitude });
            int id = getMaxId();
            tableHeght = "heigth_" + id;
            tableGPS = "gps_" + id;
            createHeight();
            createGPS();
            insertHeight(time, height, pressure, ws,rs);
            insertGPS(time, latitude, longitude);
        }
        public void end( double latitude, double longitude, double height, double pressure, long ws,long rs,long countSteps,string name)
        {
            string sql = "UPDATE Travel_Summary SET end_time=?,end_latitude=?,end_longitude=?,steps=?,name=? where id=?";
            long time = TimeUtil.getNowStamp();
            int id = getMaxId();
            tableHeght = "heigth_" + id;
            tableGPS = "gps_" + id;
            insertHeight(time, height, pressure, ws,rs);
            insertGPS(time, latitude, longitude);
            sqlQuery(sql, new object[] { time, latitude, longitude, countSteps,name, id });
        }
        public void updateSummaryName(string name)
        {
            string sql = "UPDATE Travel_Summary SET name=? where id=?";
            int id = getMaxId();
            sqlQuery(sql, new object[] { name, id });
        }
        public void closeDB()
        {
            db.Dispose();
            db.Close();
            dm = null;
        }

        private int getMaxId()
        {
            string sql = "select max(id) id from Travel_Summary";
            int id = 0;
            SQLiteCommand cmd = db.CreateCommand(sql);
            List<TravelSummary> reader = cmd.ExecuteQuery<TravelSummary>();
            foreach (TravelSummary c in reader)
            {
                id = c.ID;
            }
            return id;
        }
        /// <summary>
        /// 创建高度表
        /// </summary>
        /// <returns></returns>
        private int createHeight()
        {           
            string sql = "create table " + tableHeght + " (id INTEGER PRIMARY KEY autoincrement, time long NOT NULL, height double NOT NULL,pressure double NOT NULL,walking_steps long NOT NULL,running_steps long NOT NULL)";
            return sqlQuery(sql);
        }
        /// <summary>
        /// 创建gps表
        /// </summary>
        /// <returns></returns>
        private int createGPS()
        {  
            string sql = "create table " + tableGPS + " (id INTEGER PRIMARY KEY autoincrement, time long NOT NULL,laitude double NOT NULL, longitude double NOT NULL,address varchar(255) NULL,busines varchar(255) NULL,cityCode int NULL,description varchar(255) NULL,component varchar(255) NULL)";
            return sqlQuery(sql);
        }

        /// <summary>
        /// 更新GPS的位置信息
        /// </summary>
        /// <param name="table"></param>
        /// <param name="id"></param>
        /// <param name="address">地址</param>
        /// <param name="busines">商圈</param>
        /// <param name="cityCode">城市ID</param>
        /// <param name="description">语义化结果描述</param>
        /// <param name="component">地址的构成</param>
        public void updataGPS(int tableId,int id,string address,string busines,int cityCode,string description,string component)
        {
            string sql = "UPDATE gps_" + tableId+ " SET address=?,busines=?,cityCode=?,description=?,component=? where id=?";
            sqlQuery(sql, new object[] { address,busines,cityCode,description,component, id });
        }

        public void setHeightTable()
        {
            tableHeght = "heigth_" + getMaxId();
        }
        public void setGPSTable()
        {
            tableGPS = "gps_" + getMaxId();
        }

        /// <summary>
        /// 高度表插入相关数据
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="h">高度</param>
        /// <param name="p">气压</param>
        /// <param name="ws">步行步数</param>
        /// <param name="rs">跑步步数</param>
        public void insertHeight(long time, double h, double p, long ws,long rs)
        {
            string sql = "insert into " + tableHeght + " (time, height,pressure,walking_steps,running_steps) values (?, ?,?,?,?)";
            sqlQuery(sql, new object[] { time, h, p, ws,rs });
        }
        /// <summary>
        /// gps表插入相关数据
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="latitude">纬度</param>
        /// <param name="longitude">经度</param>
        public void insertGPS(long time, double latitude, double longitude)
        {
            string sql = "insert into " + tableGPS + " (time, laitude,longitude) values (?, ?,?)";
            sqlQuery(sql, new object[] { time, latitude, longitude });
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        public void removeSummary(int id)
        {
            string sql = "delete from Travel_Summary where id=?";
            sqlQuery(sql, new object[] { id });
            removeHeightTable(id);
            removeGPSTable(id);
        }
        /// <summary>
        /// 删除高度表
        /// </summary>
        /// <param name="id"></param>
        private void removeHeightTable(int id)
        {
            string sql = "drop table heigth_" + id;
            sqlQuery(sql);
        }
        /// <summary>
        /// 删除gps表
        /// </summary>
        /// <param name="id"></param>
        private void removeGPSTable(int id)
        {
            string sql = "drop table gps_" + id;
            sqlQuery(sql);
        }
        private int sqlQuery(string sql, params object[] args)
        {
            SQLiteCommand cmd = db.CreateCommand(sql, args);
            return cmd.ExecuteNonQuery();
        }
    }
}
