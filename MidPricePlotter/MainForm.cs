using System;
using System.Windows.Forms;
using DevExpress.XtraCharts;
using System.Collections.Generic;
using System.Linq;
// ...

namespace MidPricePlotter {
    public partial class MainForm : Form {

        private Dictionary<string, Series> SignalSeriesMap;
        private SignalDataService _signalDataService = new SignalDataService();
        private DateTime _startTime;
        public MainForm() {
            InitializeComponent();


            SignalSeriesMap = new Dictionary<string, Series>();

            chartControl1.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Center;
            chartControl1.Legend.AlignmentVertical = LegendAlignmentVertical.TopOutside;
            chartControl1.Legend.Direction = LegendDirection.LeftToRight;

            timer1.Interval = (int)numericUpDownTimer.Value;

            _signalDataService.AsyncQueryGo();
            _startTime = _signalDataService.StartTime;
            _startXRange = _startTime;
            _endXRange = _startTime.AddMilliseconds((int)numericUpDownInterval.Value);
                

            chartControl1.Series.RemoveAt(0);
            SwiftPlotDiagram diagram = chartControl1.Diagram as SwiftPlotDiagram;
            if (diagram != null)
            {
                diagram.AxisY.NumericScaleOptions.MeasureUnit = NumericMeasureUnit.Tens;
                diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Second;
            }

            //chartControl1.Series.Add(NewSeries("ASDASD"));
        }

        private double _minRangeValue = 30;
        private double _maxRangeValue = 90;


        const int interval = 1;
        Random random = new Random();
        //int TimeInterval = 10;
        double value1 = 10.0;

        Range AxisXRange {
            get {
                SwiftPlotDiagram diagram = chartControl1.Diagram as SwiftPlotDiagram;
                if (diagram != null)
                    return diagram.AxisX.VisualRange;
                return null;
            }
        }

        Range AxisYRange
        {
            get
            {
                SwiftPlotDiagram diagram = chartControl1.Diagram as SwiftPlotDiagram;
                if (diagram != null)
                    return diagram.AxisY.VisualRange;
                return null;
            }
        }

        double CalculateNextValue(double value) {
            return value + (random.NextDouble() * 10.0 - 5.0);
        }

        void UpdateValues() {
            value1 = CalculateNextValue(value1);
        }


        void InsertOrUpdateSeries(SignalData signalData, DateTime startDate, DateTime endDate)
        {
            var allSignals = signalData.GetAllSignals();
            var allTags = chartControl1.Series.Select(x => ((Series)x).Tag);

            foreach (var signal in allSignals)
            {
                if (!allTags.Contains(signal))
                {
                    InsertSingleSeries(signal, signalData.GetPriceTime(signal));
                }
                else
                {
                   
                    var series = chartControl1.Series.Where(x => ((Series)x).Tag.ToString() == signal).FirstOrDefault();

                    UpdateExistingSingleSeries((Series)series, signalData.GetPriceTime(signal));
                }
            }

            foreach(var tag in allTags)
            {
                if (!allSignals.Contains(tag))
                {
                    var series = chartControl1.Series.Where(x => ((Series)x).Tag.ToString() == tag.ToString()).FirstOrDefault(); 
                    //UpdateFakeValue((Series)series, startDate, endDate);
                }
            }

        }


        void UpdateFakeValue(Series series, DateTime startDate, DateTime endDate)
        {
            if (series == null)
                return;

            var priceTimeMap = new Dictionary<DateTime, double>();
            if (priceTimeMap.Count == 0)
            {

                priceTimeMap.Add(startDate.AddMilliseconds(10), series.Points[series.Points.Count - 1].Values[0]);
            }

            AddSeriesPoints(series, priceTimeMap);
        }


        void UpdateExistingSingleSeries(Series series, Dictionary<DateTime, double> priceTimeMap)
        {
            if (series == null)
                return;

            //DateTime argument = DateTime.Now;
            //SeriesPoint[] pointsToUpdate1 = new SeriesPoint[interval];
            //for (int i = 0; i < interval; i++)
            //{
            //    pointsToUpdate1[i] = new SeriesPoint(argument, value1);
            //    argument = argument.AddMilliseconds(1);
            //    UpdateValues();
            //}

            //TO be changed
            /*var lastMaxTime = DateTime.Now;

            var argument = lastMaxTime;
            SeriesPoint[] pointsToUpdate1 = new SeriesPoint[interval];
            for (int i = 0; i < interval; i++)
            {
                argument = argument.AddMilliseconds(1);
                if (priceTimeMap.ContainsKey(argument))
                {
                    var seriesPoint = new SeriesPoint(argument, priceTimeMap[argument]);
                    pointsToUpdate1[i]  = seriesPoint;
                }
            }


            DateTime minDate = lastMaxTime.AddSeconds(-TimeInterval);
            int pointsToRemoveCount = 0;

            foreach (SeriesPoint point in series.Points)
            {
                if (point.DateTimeArgument < minDate)
                    pointsToRemoveCount++;
            }

            if (pointsToRemoveCount < series.Points.Count)
                pointsToRemoveCount--;

            series.Points.AddRange(pointsToUpdate1);

            if (pointsToRemoveCount > 0)
            {
                series.Points.RemoveRange(0, pointsToRemoveCount);
            }
            if (AxisXRange != null)
            {
                AxisXRange.SetMinMaxValues(minDate, argument);
            }*/

           
            AddSeriesPoints(series, priceTimeMap);

        }

        void InsertSingleSeries(string signal, Dictionary<DateTime, double> priceTimeMap)
        {
            //if (signal != "US 500 (Mar) >2012.0 (4:15PM)")
            //    return;
            //if (this.chartControl1.Series.Count < 2)
            {

                var series = NewSeries(signal);
                //series.Tag =  series.Name = signal;

                //var argument = priceTimeMap.Keys.Min();
                //SeriesPoint[] pointsToUpdate1 = new SeriesPoint[interval];
                //for (int i = 0; i < interval; i++)
                //{
                //    argument = argument.AddMilliseconds(1000);
                //    if (priceTimeMap.ContainsKey(argument))
                //    {
                //        var seriesPoint = new SeriesPoint(argument, priceTimeMap[argument]);
                //        pointsToUpdate1[i] = seriesPoint;
                //    }
                //}

                AddSeriesPoints(series, priceTimeMap);
            }


        }

        private void AddSeriesPoints(Series series, Dictionary<DateTime, double> priceTimeMap)
        {
            //if (series.Tag.ToString() != "US 500 (Mar) >2012.0 (4:15PM)")
            //    return;


            foreach (var priceTime in priceTimeMap)
            {
                //if (priceTime.Value > _maxRangeValue || priceTime.Value < _minRangeValue)
                //    continue;
                var seriesPoint = new SeriesPoint(priceTime.Key, priceTime.Value);
                series.Points.Add(seriesPoint);
            }

            //if (AxisXRange != null)
            //{
            //    AxisXRange.SetMinMaxValues(priceTimeMap.Keys.Min(), priceTimeMap.Keys.Max());

            //}

        }

        DateTime _startXRange;
        DateTime _endXRange;

        Series NewSeries(string signal)
        {
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.SwiftPlotSeriesView swiftPlotSeriesView1 = new DevExpress.XtraCharts.SwiftPlotSeriesView();

            series1.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.DateTime;
            //swiftPlotDiagram1.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Millisecond;
            //swiftPlotDiagram1.AxisX.Label.TextPattern = "{A:mm:ss}";


            series1.Tag = series1.LegendText = series1.Name = signal;

            series1.CrosshairLabelPattern = "{S}: {A} - {V}";

            swiftPlotSeriesView1.Antialiasing = true;
            series1.Label.TextPattern = "{S} {A}: {V:f3}";
            series1.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            //c//hartControl1.t
            series1.View = swiftPlotSeriesView1;
            //swiftPlotSeriesView1.Color = System.Drawing.Color.Red;

            

            this.chartControl1.Series.Add(series1);
            return series1;
        }

        DateTime _endTime;

        bool globalPause = false;
        private void timer1_Tick(object sender, EventArgs e)
        {
            //Series series1 = chartControl1.Series[0];

            //if (series1 == null)
            //    return;
            DateTime argument = _startTime;
           // _endTime = _startTime.AddMilliseconds((int)numericUpDownInterval.Value);

          
            bool pause;
            var signalData = _signalDataService.GetSignalData( _startTime, ref _endTime, out pause);
           
            if (!globalPause &&  !pause)
            {
               
                if (signalData != null && signalData.Data != null && signalData.Data.Count != 0)
                {
                    
                    InsertOrUpdateSeries(signalData, _startTime, _endTime);

                   if (AxisXRange != null)
                    {
                        if (_endTime > _endXRange)
                        {
                           // _startXRange = _endXRange;
                           
                            _endXRange = _endXRange.AddMilliseconds((int)numericUpDownInterval.Value);
                            //AxisXRange.SetMinMaxValues(_startXRange, _endXRange);
                            AxisXRange.SetMinMaxValues(_startXRange, _endXRange);
                        }
                    }
                    _startTime = _endTime;
                }
                else
                {
                    //SeriesPoint[] pointsToUpdate1 = new SeriesPoint[interval];
                    //for (int i = 0; i < interval; i++)
                    //{
                    //        pointsToUpdate1[i] = new SeriesPoint(argument, value1);
                    //        argument = argument.AddMilliseconds(1);
                    //        UpdateValues();
                    //}
                }
                    //SeriesPoint[] pointsToUpdate1 = new SeriesPoint[interval];
                    //for (int i = 0; i < interval; i++)
                    //{

                    //    //if (i % 2 == 0)
                    //    {
                    //        pointsToUpdate1[i] = new SeriesPoint(argument, value1);
                    //        argument = argument.AddMilliseconds(1);
                    //        UpdateValues();//
                    //    }
                    //}
                   // DateTime minDate = argument.AddSeconds(-TimeInterval);
                   
                    //foreach (Series series1 in chartControl1.Series)
                    //{
                    //    int pointsToRemoveCount = 0;
                    //    foreach (SeriesPoint point in series1.Points)
                    //        if (point.DateTimeArgument < minDate)
                    //            pointsToRemoveCount++;
                    //    if (pointsToRemoveCount < series1.Points.Count)
                    //        pointsToRemoveCount--;
                      
                    //    if (pointsToRemoveCount > 0)
                    //    {
                    //        //series1.Points.RemoveRange(0, pointsToRemoveCount);
                    //    }
                    //}

                   
                    foreach (Series series1 in chartControl1.Series)
                    {
                        int  pointsToRemoveCount = 0;
                        //var lastPoint = series1.Points[series1.Points.Count - 1];
                        foreach (SeriesPoint point in series1.Points)
                        {
                            if (series1.Points.Count > 1)
                            {
                                if (point.DateTimeArgument < (DateTime)AxisXRange.MinValue)
                                {
                                    pointsToRemoveCount++;
                                   // series1.Points.Remove(point);
                                }

                                if (pointsToRemoveCount < series1.Points.Count)
                                    pointsToRemoveCount--;

                                if (pointsToRemoveCount > 0)
                                {
                                    //series1.Points.RemoveRange(0, pointsToRemoveCount);
                                }
                            }
                        }
                    }
               

                    //if (AxisXRange != null)
                    //{
                    //    AxisXRange.SetMinMaxValues(minDate, argument);
                    //}
                    if (AxisYRange != null)
                    {
                        AxisYRange.SetMinMaxValues(_minRangeValue, _maxRangeValue);
                        
                    }


               
            }
        }

        private void numericUpDownTimer_ValueChanged(object sender, EventArgs e)
        {
            this.timer1.Interval = (int)numericUpDownTimer.Value;
        }

        private void numericUpDownInterval_ValueChanged(object sender, EventArgs e)
        {

        }

     
        private void buttonPlay_Click(object sender, EventArgs e)
        {
            globalPause = false;
        }

        private void buttonPause_Click(object sender, EventArgs e)
        {
            globalPause = true;
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            _minRangeValue = Convert.ToInt32(textBoxMin.Text);
            _maxRangeValue = Convert.ToInt32(textBoxMax.Text);
        }

     

    }
}