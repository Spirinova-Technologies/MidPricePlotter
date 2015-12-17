using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using System.Threading;
namespace MidPricePlotter
{
    class UIDataTable
    {
        public DataTable Data { get; set; }
        private ReaderWriterLock UITableLock = new ReaderWriterLock();

        private DateTime LastReadTime { get; set; }

        public UIDataTable()
        {
            Data = new DataTable();
            Data.Columns.Add(SignalData.Symbol);
            Data.Columns.Add(SignalData.Time, typeof(DateTime));
            Data.Columns.Add(SignalData.Price);
        }

        public List<DataRow> GetChunk(DateTime fromDate, DateTime toDate)
        {
            UITableLock.AcquireReaderLock(Int32.MaxValue);
            var results = (from DataRow myRow in Data.Rows
                          where ( (DateTime)myRow[SignalData.Time] > fromDate &&
                                  (DateTime)myRow[SignalData.Time] < toDate)
                          select myRow).ToList();
            UITableLock.ReleaseReaderLock();
            return results;
        }

        public List<DataRow> GetChunk(DateTime fromDate)
        {
            UITableLock.AcquireReaderLock(Int32.MaxValue);
            var results = (from DataRow myRow in Data.Rows
                            where ((DateTime)myRow[SignalData.Time] > fromDate)
                           select myRow).ToList();
            UITableLock.ReleaseReaderLock();
            return results;
        }

        public void CopyFromDBDataTable(DataTable DBDataTable)
        {
            UITableLock.AcquireWriterLock(Int32.MaxValue);

            var existingMax = DateTime.MinValue;
            if( Data != null && Data.Rows.Count > 0 )
                existingMax = (DateTime)Data.AsEnumerable().Max(x => x[SignalData.Time]);

            var results = from DataRow myRow in DBDataTable.Rows
                            where (Utils.GetExactDateTime(myRow[SignalData.Time].ToString()) > existingMax)
                            select myRow;
                
            foreach(var row in results)
            {
                DataRow newRow = Data.NewRow();
                newRow[SignalData.Symbol] = row[SignalData.Symbol];
                newRow[SignalData.Time] = Utils.GetExactDateTime(row[SignalData.Time].ToString());
                newRow[SignalData.Price] = row[SignalData.Price];
                //newRow.ItemArray = row.ItemArray.Clone() as object[]; ;
                Data.Rows.Add(newRow);
            }
            UITableLock.ReleaseWriterLock();
        }
    }
}
