using DevExpress.XtraCharts;
namespace MidPricePlotter {
    partial class MainForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraCharts.SwiftPlotDiagram swiftPlotDiagram5 = new DevExpress.XtraCharts.SwiftPlotDiagram();
            DevExpress.XtraCharts.Series series5 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.SwiftPlotSeriesView swiftPlotSeriesView5 = new DevExpress.XtraCharts.SwiftPlotSeriesView();
            DevExpress.XtraCharts.SideBySideBarSeriesLabel sideBySideBarSeriesLabel5 = new DevExpress.XtraCharts.SideBySideBarSeriesLabel();
            DevExpress.XtraCharts.ChartTitle chartTitle5 = new DevExpress.XtraCharts.ChartTitle();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.chartControl1 = new DevExpress.XtraCharts.ChartControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBoxPlayPause = new System.Windows.Forms.CheckBox();
            this.comboBoxTime = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listBoxRanges = new System.Windows.Forms.ListBox();
            this.textBoxMin = new System.Windows.Forms.TextBox();
            this.buttonApply = new System.Windows.Forms.Button();
            this.textBoxMax = new System.Windows.Forms.TextBox();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotDiagram5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotSeriesView5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesLabel5)).BeginInit();
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // chartControl1
            // 
            this.chartControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            swiftPlotDiagram5.AxisX.Label.TextPattern = "{A:hh:mm:ss}";
            swiftPlotDiagram5.AxisX.VisibleInPanesSerializable = "-1";
            swiftPlotDiagram5.AxisY.VisibleInPanesSerializable = "-1";
            swiftPlotDiagram5.Margins.Left = 40;
            swiftPlotDiagram5.Margins.Right = 30;
            this.chartControl1.Diagram = swiftPlotDiagram5;
            this.chartControl1.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True;
            this.chartControl1.Location = new System.Drawing.Point(231, 1);
            this.chartControl1.Name = "chartControl1";
            series5.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.DateTime;
            series5.LegendText = "Swift Plot Series";
            series5.Name = "series1";
            swiftPlotSeriesView5.Antialiasing = true;
            series5.View = swiftPlotSeriesView5;
            this.chartControl1.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series5};
            sideBySideBarSeriesLabel5.LineVisibility = DevExpress.Utils.DefaultBoolean.True;
            this.chartControl1.SeriesTemplate.Label = sideBySideBarSeriesLabel5;
            this.chartControl1.Size = new System.Drawing.Size(653, 646);
            this.chartControl1.TabIndex = 0;
            chartTitle5.Text = "Midpoint Plotter";
            this.chartControl1.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle5});
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.Controls.Add(this.statusStrip1);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Location = new System.Drawing.Point(2, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(234, 646);
            this.panel1.TabIndex = 1;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 624);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(234, 22);
            this.statusStrip1.TabIndex = 15;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBoxPlayPause);
            this.groupBox2.Controls.Add(this.comboBoxTime);
            this.groupBox2.Location = new System.Drawing.Point(10, 11);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(221, 64);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Time";
            // 
            // checkBoxPlayPause
            // 
            this.checkBoxPlayPause.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxPlayPause.AutoSize = true;
            this.checkBoxPlayPause.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.checkBoxPlayPause.Location = new System.Drawing.Point(140, 17);
            this.checkBoxPlayPause.MinimumSize = new System.Drawing.Size(75, 10);
            this.checkBoxPlayPause.Name = "checkBoxPlayPause";
            this.checkBoxPlayPause.Size = new System.Drawing.Size(75, 23);
            this.checkBoxPlayPause.TabIndex = 13;
            this.checkBoxPlayPause.Text = "Pause";
            this.checkBoxPlayPause.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxPlayPause.UseVisualStyleBackColor = true;
            this.checkBoxPlayPause.CheckedChanged += new System.EventHandler(this.checkBoxPlayPause_CheckedChanged);
            // 
            // comboBoxTime
            // 
            this.comboBoxTime.FormattingEnabled = true;
            this.comboBoxTime.Location = new System.Drawing.Point(6, 19);
            this.comboBoxTime.Name = "comboBoxTime";
            this.comboBoxTime.Size = new System.Drawing.Size(121, 21);
            this.comboBoxTime.TabIndex = 11;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.listBoxRanges);
            this.groupBox1.Controls.Add(this.textBoxMin);
            this.groupBox1.Controls.Add(this.buttonApply);
            this.groupBox1.Controls.Add(this.textBoxMax);
            this.groupBox1.Location = new System.Drawing.Point(10, 100);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(221, 242);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Range";
            // 
            // listBoxRanges
            // 
            this.listBoxRanges.FormattingEnabled = true;
            this.listBoxRanges.Items.AddRange(new object[] {
            "0 - 50",
            "50 - 100",
            "100 - 150",
            "2000 - 2100"});
            this.listBoxRanges.Location = new System.Drawing.Point(19, 19);
            this.listBoxRanges.Name = "listBoxRanges";
            this.listBoxRanges.Size = new System.Drawing.Size(150, 134);
            this.listBoxRanges.TabIndex = 15;
            this.listBoxRanges.SelectedIndexChanged += new System.EventHandler(this.listBoxRanges_SelectedIndexChanged);
            // 
            // textBoxMin
            // 
            this.textBoxMin.Location = new System.Drawing.Point(19, 176);
            this.textBoxMin.Name = "textBoxMin";
            this.textBoxMin.Size = new System.Drawing.Size(65, 20);
            this.textBoxMin.TabIndex = 2;
            // 
            // buttonApply
            // 
            this.buttonApply.Location = new System.Drawing.Point(66, 213);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(75, 23);
            this.buttonApply.TabIndex = 7;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // textBoxMax
            // 
            this.textBoxMax.Location = new System.Drawing.Point(106, 176);
            this.textBoxMax.Name = "textBoxMax";
            this.textBoxMax.Size = new System.Drawing.Size(63, 20);
            this.textBoxMax.TabIndex = 3;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(78, 17);
            this.toolStripStatusLabel1.Text = "Connecting...";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 160);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "MIN";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(105, 160);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "MAX";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(882, 648);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.chartControl1);
            this.Name = "MainForm";
            this.Text = "Midpoint Plotter";
            ((System.ComponentModel.ISupportInitialize)(swiftPlotDiagram5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotSeriesView5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesLabel5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private DevExpress.XtraCharts.ChartControl chartControl1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBoxMax;
        private System.Windows.Forms.TextBox textBoxMin;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.ComboBox comboBoxTime;
        private System.Windows.Forms.CheckBox checkBoxPlayPause;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox listBoxRanges;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;

    }
}

