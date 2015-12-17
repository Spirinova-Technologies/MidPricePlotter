using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Globalization;
namespace MidPricePlotter
{
    class SignalData
    {
        public const string Symbol = "Symbol";
        public const string Time = "Time";
        public const string Price = "Price";
        

        public Dictionary<string, Dictionary<DateTime, double>> Data { get; private set; }

        public DateTime LastReadTime
        {
            get;
            private set;
        }
        public SignalData(DataTable table, DateTime lastReadTime)
        {
           LastReadTime = DateTime.MaxValue;
           Data = new Dictionary<string, Dictionary<DateTime, double>>();
           foreach (DataRow row in table.Rows)
           {
               try
               {
                   string symbol = row["Symbol"].ToString();

                   string time = row["Time"].ToString();
                   var datetime = Utils.GetExactDateTime(time);

                   //Ignore value already read
                   if (datetime > lastReadTime)
                   {
                       double price = Convert.ToDouble(row["Price"]);
                       if (!Data.ContainsKey(symbol))
                       {
                           Data[symbol] = new Dictionary<DateTime, double>();
                       }

                       Data[symbol][datetime] = price;

                       if (datetime < LastReadTime)
                           LastReadTime = datetime;
                   }
               }
               catch(Exception )
               {
                   
               }
            }
         
        }

        public SignalData(List<DataRow> rows)
        {
            //LastReadTime = DateTime.MaxValue;
            Data = new Dictionary<string, Dictionary<DateTime, double>>();
            foreach (DataRow row in rows)
            {
                try
                {
                    string symbol = row["Symbol"].ToString();

                    //string time = row["Time"].ToString();
                    var datetime = (DateTime)row["Time"];

                    //Ignore value already read
                    //if (datetime > lastReadTime)
                    {
                        double price = Convert.ToDouble(row["Price"]);
                        if (!Data.ContainsKey(symbol))
                        {
                            Data[symbol] = new Dictionary<DateTime, double>();
                        }

                        Data[symbol][datetime] = price;

                        if (datetime < LastReadTime)
                            LastReadTime = datetime;
                    }
                }
                catch (Exception)
                {

                }
            }
        }
 
        public Dictionary<DateTime, double> GetPriceTime(string signal)
        {
            if (Data.ContainsKey(signal))
            {
                return Data[signal];
            }
            return null;
        }

        public double? GetPrice(string signal, DateTime time)
        {
            var priceTimeMap = GetPriceTime(signal);
            if ( priceTimeMap != null && priceTimeMap.ContainsKey(time))
            {
                return priceTimeMap[time];
            }
            return null;
        }

        public List<string> GetAllSignals()
        {
            return Data.Keys.ToList();
        }
    }
}
