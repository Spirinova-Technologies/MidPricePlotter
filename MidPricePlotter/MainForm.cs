using System;
using System.Windows.Forms;
using DevExpress.XtraCharts;
using System.Collections.Generic;
using System.Linq;

// ...

namespace MidPricePlotter {


    public partial class MainForm : Form {

        private double _minYRangeValue = 0;
        private double _maxYRangeValue = 0;

        private const int XScaleMultiplier = 120000;

        private Dictionary<string, Series> SignalSeriesMap;
        private SignalDataService _signalDataService;
        private DateTime _startTime;
        private DateTime _appStartTime;

        private int FullXRangeInMilliSeconds
        {
            get
            {
                //MessageBox.Show(comboBoxTime.SelectedValue.ToString());
                return Convert.ToInt32(comboBoxTime.SelectedValue);
            }
            set
            {
            }
        }

        private void PopulateIntervalComboBox()
        {
            var dict = new Dictionary<string, int>();

            dict.Add("1 SEC", GetInterval(1));
            dict.Add("1 MIN", GetInterval(60));
            dict.Add("2 MINS", GetInterval(120));
            dict.Add("3 MINS", GetInterval(180));
            dict.Add("5 MINS", GetInterval(300));
            dict.Add("15 MINS", GetInterval(900));
            dict.Add("30 MINS", GetInterval(1800));

            comboBoxTime.DataSource = new BindingSource(dict, null);
            comboBoxTime.DisplayMember = "Key";
            comboBoxTime.ValueMember = "Value";

            comboBoxTime.SelectedIndex = 0;
        }


        private int GetInterval(int seconds)
        {
            return XScaleMultiplier * seconds;
        }
        public MainForm() {
            InitializeComponent();

            _signalDataService = new SignalDataService(statusStrip1);
            PopulateIntervalComboBox();
            comboBoxTime.SelectedIndexChanged += new System.EventHandler(this.comboBoxTime_SelectedIndexChanged);

            
            SignalSeriesMap = new Dictionary<string, Series>();

            chartControl1.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Center;
            chartControl1.Legend.AlignmentVertical = LegendAlignmentVertical.TopOutside;
            chartControl1.Legend.Direction = LegendDirection.LeftToRight;

            timer1.Interval = 1000;
           

            _signalDataService.AsyncQueryGo();
            _startTime = _signalDataService.StartTime;
            _appStartTime = _startTime;

            SetXRangeValuesFromStart(_startTime);
                

            chartControl1.Series.RemoveAt(0);
            SwiftPlotDiagram diagram = chartControl1.Diagram as SwiftPlotDiagram;
            if (diagram != null)
            {
                diagram.AxisY.NumericScaleOptions.MeasureUnit = NumericMeasureUnit.Tens;
                diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Second;
            }

            if (AxisYWholeRange != null)
            {
                AxisYWholeRange.SetMinMaxValues(_minYRangeValue, _maxYRangeValue);

            }
            //chartControl1.Series.Add(NewSeries("ASDASD"));
        }

      
        

        Range AxisXVisualRange {
            get {
                SwiftPlotDiagram diagram = chartControl1.Diagram as SwiftPlotDiagram;
                if (diagram != null)
                    return diagram.AxisX.VisualRange;
                return null;
            }
        }

        Range AxisXWholeRange
        {
            get
            {
                SwiftPlotDiagram diagram = chartControl1.Diagram as SwiftPlotDiagram;
                if (diagram != null)
                    return diagram.AxisX.WholeRange;
                return null;
            }
        }

        Range AxisYVisualRange
        {
            get
            {
                SwiftPlotDiagram diagram = chartControl1.Diagram as SwiftPlotDiagram;
                if (diagram != null)
                    return diagram.AxisY.VisualRange;
                return null;
            }
        }

        Range AxisYWholeRange
        {
            get
            {
                SwiftPlotDiagram diagram = chartControl1.Diagram as SwiftPlotDiagram;
                if (diagram != null)
                    return diagram.AxisY.WholeRange;
                return null;
            }
        }

      
        private void SetXRangeValuesFromStart(DateTime startTime)
        {
            _startXRange = startTime;
            _endXRange = _startXRange.AddMilliseconds(FullXRangeInMilliSeconds);
        }

        private void SetXRangeValuesFromEnd(DateTime endTime)
        {
            _endXRange = endTime;
            _startXRange = endTime.AddMilliseconds(-1 * FullXRangeInMilliSeconds);
            if( _startXRange < _appStartTime )
            {
                SetXRangeValuesFromStart(_appStartTime);
            }
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
            if (_appStartTime == DateTime.MinValue)
            {
                _appStartTime = _startTime = _signalDataService.StartTime;
                return;
            }


            DateTime argument = _startTime;
            bool pause;
            var signalData = _signalDataService.GetSignalData( _startTime, ref _endTime, out pause);
           
            if (!globalPause &&  !pause)
            {
               
                if (signalData != null && signalData.Data != null && signalData.Data.Count != 0)
                {
                    
                    InsertOrUpdateSeries(signalData, _startTime, _endTime);

                   if (AxisXVisualRange != null)
                    {
                        if (_endTime > _endXRange)
                        {
                            SetXRangeValuesFromEnd(_endTime);
                            //AxisXVisualRange.SetMinMaxValues(_startXRange, _endXRange);
                            AxisXWholeRange.SetMinMaxValues(_startXRange, _endXRange);
                        }
                    }
                    _startTime = _endTime;
                }

                //RemoveOldChartPoints();
            }
        }
     
     

        private void RemoveOldChartPoints()
        {
            foreach (Series series1 in chartControl1.Series)
            {
                int pointsToRemoveCount = 0;
                //var lastPoint = series1.Points[series1.Points.Count - 1];
                foreach (SeriesPoint point in series1.Points)
                {
                    if (series1.Points.Count > 1)
                    {
                        if (point.DateTimeArgument < (DateTime)AxisXVisualRange.MinValue)
                        {
                            pointsToRemoveCount++;
                        }

                        if (pointsToRemoveCount < series1.Points.Count)
                            pointsToRemoveCount--;

                        if (pointsToRemoveCount > 0)
                        {
                            series1.Points.RemoveRange(0, pointsToRemoveCount);
                        }
                    }
                }
            }
        }
      
        private void buttonApply_Click(object sender, EventArgs e)
        {
            try
            {
                _minYRangeValue = Convert.ToDouble(textBoxMin.Text);
                _maxYRangeValue = Convert.ToDouble(textBoxMax.Text);

                if (_minYRangeValue >= _maxYRangeValue)
                    throw new Exception("Max should be greater than Min");

                if (AxisYWholeRange != null)
                {
                    AxisYWholeRange.SetMinMaxValues(_minYRangeValue, _maxYRangeValue);

                }
            }
            catch(Exception)
            {
                MessageBox.Show("Please enter a valid range." , "MidPricePlotter", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void checkBoxPlayPause_CheckedChanged(object sender, EventArgs e)
        {
            if( checkBoxPlayPause.Checked == false)
            {
                //MessageBox.Show("False");
                globalPause = false;
                checkBoxPlayPause.Text = "Pause";
            }
            else
            {
                //MessageBox.Show("tru");
                globalPause = true;
                checkBoxPlayPause.Text = "Play";
            }
        }

        private void listBoxRanges_SelectedIndexChanged(object sender, EventArgs e)
        {
            var nums = listBoxRanges.Items[listBoxRanges.SelectedIndex].ToString().Split('-');
            textBoxMin.Text = nums[0].Trim();
            textBoxMax.Text = nums[1].Trim();
        }

        private void comboBoxTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AxisXVisualRange != null)
            {
                SetXRangeValuesFromEnd(_endTime);
                AxisXWholeRange.SetMinMaxValues(_startXRange, _endXRange);
            }
        }

    }
}