using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using System.IO;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MidPricePlotter
{
    class SignalDataService
    {
        private string DatabaseConnectionString = ConfigurationManager.ConnectionStrings["MidPricePlotter"].ConnectionString;

        private int _dbPollTime = 1000;

        private DateTime _lastReadTime = DateTime.MinValue;
        private StatusStrip _uiStatusStrip;
        //private int _interval = 5000;
        //private System.Windows.Forms.Timer timer1;

        public SignalDataService(StatusStrip strip)
        {
            _uiStatusStrip = strip;
        }
        private DataSet RunStoredProcedureAndGetDataSet(string storedProcName, List<SqlParameter> parameters)
        {
            using (SqlConnection connection = new SqlConnection(DatabaseConnectionString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandTimeout = 60;
                cmd.CommandText = storedProcName;
                cmd.CommandType = CommandType.StoredProcedure;
                if (parameters != null)
                {
                    foreach (var par in parameters)
                    {
                        cmd.Parameters.Add(par);
                    }
                }
                connection.Open();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                return ds;
            }
        }
          
        private DateTime _startTime = DateTime.MinValue;
        public DateTime StartTime
        {
            get
            {
                if (_startTime == DateTime.MinValue)
                {
                    /*var data = RunStoredProcedureAndGetDataSet("getMidPointGraph", null);
                    if (data == null || data.Tables[0].Rows.Count == 0)
                    {
                        throw new Exception("No data");
                    }
                    _startTime = Utils.GetExactDateTime(data.Tables[0].AsEnumerable().Min(x => x[SignalData.Time]).ToString());                     
                     */

                    lock (DBTableLock)
                    {
                        if( DBTable != null && DBTable.Rows.Count > 0)
                            _startTime = Utils.GetExactDateTime(DBTable.Rows[DBTable.Rows.Count - 1][SignalData.Time].ToString());
                    }
                }
                return _startTime;
            }
        }
       
        public SignalData GetSignalData()
        {
            var data = RunStoredProcedureAndGetDataSet("getMidPointGraph", null);
            if (data.Tables.Count == 0)
                return null;
            else
            {
                var signalData = new SignalData(data.Tables[0], _lastReadTime);
                _lastReadTime = signalData.LastReadTime;
                return signalData;
            }
        }

        private DataTable DBTable = new DataTable();
        private UIDataTable UITable = new UIDataTable();
        private object DBTableLock = new object();
        private DateTime _LastAsyncReadTime;
        private object LastAsyncTimeLock = new object();
        public void StoreSignalDataTask()
        {
            bool connected = false;

            while (true)
            {
                try
                {
                    StoreSignalData();
                    if (connected == false)
                    {
                        _uiStatusStrip.BeginInvoke((Action)delegate()
                        {
                            _uiStatusStrip.Items[0].Text = "Connected";
                        });
                        connected = true;
                    }
                    Thread.Sleep(_dbPollTime);
                }
                catch (Exception)
                {
                    _uiStatusStrip.BeginInvoke((Action)delegate()
                    {
                        _uiStatusStrip.Items[0].Text = "Connection failed. Reconnecting...";
                    });
                    connected = false;
                }
            }
        }

       public void StoreSignalData()
        {
            var data = RunStoredProcedureAndGetDataSet("getMidPointGraph", null);
            if (data != null && data.Tables.Count != 0)
            {
                lock (DBTableLock)
                {
                    //DBTable.Clear();
                    //foreach (DataRow dr in data.Tables[0].Rows) 
                    //{
                    //    DBTable.Rows.Add(dr.ItemArray);
                    //}

                    DBTable = data.Tables[0];

                    lock (LastAsyncTimeLock)
                    {
                        if (data.Tables[0] != null && data.Tables[0].Rows.Count > 0)
                            _LastAsyncReadTime = Utils.GetExactDateTime(data.Tables[0].Rows[0][SignalData.Time].ToString());
                    }

                    UITable.CopyFromDBDataTable(DBTable);
                }
            }
        }
        public void AsyncQueryGo()
        {
            Task task = new Task(() => StoreSignalDataTask());
            task.Start();
            //timer1 = new System.Windows.Forms.Timer();
            //this.timer1.Interval = _interval;
            //this.timer1.Enabled = true;
            //this.timer1.Tick += new System.EventHandler(this.timer1_Tick);

           // var bgw = new BackgroundWorker();

        }

        public SignalData GetSignalData(DateTime fromTime, ref DateTime toTime, out bool pause)
        {
            DateTime lastAsyncTime = DateTime.MaxValue;
            lock (LastAsyncTimeLock)
            {
                lastAsyncTime = _LastAsyncReadTime;
            }
            if (fromTime > lastAsyncTime)
            {
                //Synchronus call
                //StoreSignalData();
                pause = true;
                return null;
            }
            var chunk = UITable.GetChunk(fromTime);
            
            var signalData = new SignalData(chunk);
           

            if( signalData.Data.Count == 0)
            {
                //var val = new Dictionary<DateTime,double>();
                //val.Add(toTime.AddMilliseconds(10), 2200);
                //signalData.Data.Add("TEST", val);
                pause = true;
            }
            else
            {
                //fromTime = Utils.GetExactDateTime(chunk[0]["Time"].ToString());
                toTime = (DateTime)(chunk[chunk.Count - 1]["Time"]);
                pause = false;
            }
            return signalData;
        }


        public SignalData GetNextData(DateTime lastReadTime)
        {
            return data;
        }
        SignalData data;
        private void timer1_Tick(object sender, EventArgs e)
        {
            //var signalData = _signalDataService.GetTestSignalData();
            data = GetSignalData();

            //RemoveDeletedSeries(signalData);
            //InsertOrUpdateSeries(signalData);

            //textBox1.Text = signalData.Data.Count.ToString();
            //if( SignalSeriesMap.ContainsKey)
            //this.timer1.Tick -= new System.EventHandler(this.timer1_Tick);
        }




        //public SignalData GetSignalDataAsnc()
        //{

        //}

        #region Test
        int seed = 0;

       DataTable sortedDT = null;
       
       private void ReadSampleData()
        {
            if (sortedDT == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("Symbol"));
                dt.Columns.Add(new DataColumn("Time", typeof(DateTime)));
                dt.Columns.Add(new DataColumn("Price", typeof(double)));

                var lines = File.ReadAllLines("Sample.csv");
                foreach (var line in lines)
                {
                    //dt.Rows.Add("US 500(DEC) > 2098.5 (4:15PM)", 
                    var data = line.Split(',');
                    dt.Rows.Add(data[0].Trim(), Convert.ToDateTime(data[1].Trim().Substring(0, data[1].LastIndexOf("."))), Convert.ToDouble(data[2].Trim()));
                }

                DataView dv = dt.DefaultView;
                dv.Sort = "Time desc";
                sortedDT = dv.ToTable();
            }
        }

        public SignalData GetTestSignalData()
        {
            ReadSampleData();
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Symbol"));
            dt.Columns.Add(new DataColumn("Time", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("Price", typeof(double)));
            int i = seed;
            for (; i < sortedDT.Rows.Count; i++)
            {
                var row = dt.NewRow();
                row.ItemArray = sortedDT.Rows[i].ItemArray;
                Random random = new Random();
                row["Time"] = DateTime.Now.AddSeconds(dt.Rows.Count);

                if (row["Symbol"].ToString() == "US 500 (Dec) >2083.0 (4:15PM)")
                {
                    row["Price"] = Convert.ToDouble(sortedDT.Rows[i]["Price"]) + 2 * random.NextDouble();
                    dt.Rows.Add(row);
                }
                else if (row["Symbol"].ToString() == "US 500 (Dec) >2086.0 (4:15PM)")
                {
                    row["Price"] = Convert.ToDouble(sortedDT.Rows[i]["Price"]) + 15  + 3 * random.NextDouble();
                    dt.Rows.Add(row);
                }
                else if (row["Symbol"].ToString() == "US 500 (Dec) >2089.0 (4:15PM)")
                {
                    row["Price"] = Convert.ToDouble(sortedDT.Rows[i]["Price"]) + 30 + 3 * random.NextDouble();
                    dt.Rows.Add(row);
                }

                else if (row["Symbol"].ToString() == "US 500 (Dec) >2092.0 (4:15PM)")
                {
                    row["Price"] = Convert.ToDouble(sortedDT.Rows[i]["Price"]) + 40 + 3 * random.NextDouble();
                    dt.Rows.Add(row);
                }
             

                if (dt.Rows.Count == 20)
                    break;
            }
           
            //seed = i;
            return new SignalData(dt, DateTime.MinValue);

        }

        #endregion Test
    }
}
