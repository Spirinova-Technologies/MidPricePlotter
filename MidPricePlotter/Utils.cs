using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace MidPricePlotter
{
    class Utils
    {
        public static DateTime GetExactDateTime(string time)
        {
            try
            {
                time = time.Substring(0, time.IndexOf(".") + 4);

                var fullDate = DateTime.Now.ToString("dd:MM:yyyy") + " " + time;

                var datetime = DateTime.ParseExact(fullDate,
                                    "dd:MM:yyyy HH:mm:ss.fff",
                                     CultureInfo.InvariantCulture);
                return datetime;
            }
            catch(Exception)
            {
                //Umesh change for testin
                return DateTime.MinValue;
            }
        }

       
    }
}
