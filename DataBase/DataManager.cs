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

        private int createHeight()
        {           
            string sql = "create table " + tableHeght + " (id INTEGER PRIMARY KEY autoincrement, time long NOT NULL, height double NOT NULL,pressure double NOT NULL,walking_steps long NOT NULL,running_steps long NOT NULL)";
            return sqlQuery(sql);
        }

        private int createGPS()
        {  
            string sql = "create table " + tableGPS + " (id INTEGER PRIMARY KEY autoincrement, time long NOT NULL,laitude double NOT NULL, longitude double NOT NULL)";
            return sqlQuery(sql);
        }
        public void setHeightTable()
        {
            tableHeght = "heigth_" + getMaxId();
        }
        public void setGPSTable()
        {
            tableGPS = "gps_" + getMaxId();
        }

        //插入时间高度
        public void insertHeight(long time, double h, double p, long ws,long rs)
        {
            string sql = "insert into " + tableHeght + " (time, height,pressure,walking_steps,running_steps) values (?, ?,?,?,?)";
            sqlQuery(sql, new object[] { time, h, p, ws,rs });
        }
        //插入时间纬度经度
        public void insertGPS(long time, double latitude, double longitude)
        {
            string sql = "insert into " + tableGPS + " (time, laitude,longitude) values (?, ?,?)";
            sqlQuery(sql, new object[] { time, latitude, longitude });
        }
        private int sqlQuery(string sql, params object[] args)
        {
            SQLiteCommand cmd = db.CreateCommand(sql, args);
            return cmd.ExecuteNonQuery();
        }
    }
}
