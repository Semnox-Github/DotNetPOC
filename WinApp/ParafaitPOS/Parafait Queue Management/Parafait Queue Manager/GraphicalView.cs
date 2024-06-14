/********************************************************************************************
* Project Name - Parafait Queue Management
* Description  - Graphical View 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
* 2.80        13-Sep-2019      Jinto Thomas         Added logger for methods
********************************************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using ZedGraph;

namespace ParafaitQueueManagement
{
    public partial class GraphicalView : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // Utilities parafaitUtility = new Utilities();
        double totaldays;
        public GraphicalView()
        {
            log.LogMethodEntry();
            InitializeComponent();
            log.LogMethodExit();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            BuildGraph(zedGraph);
            zedGraph.Refresh();
            log.LogMethodExit();
        }
        public void BuildGraph(ZedGraphControl z)
        {
            log.LogMethodEntry(z);
            GraphPane gamePane = z.GraphPane;
            gamePane.CurveList.Clear();
            gamePane.GraphObjList.Clear();
            gamePane.XAxis.Title.Text = "Lane Name";
            gamePane.YAxis.Title.Text = "Play Count";
            gamePane.Title.FontSpec.Size = 12;

            DataTable dtplayStatistics = getDetails();
            gamePane.Title.Text = " Cricket Lane Statistics - For " + totaldays + " Days";
            if (dtplayStatistics.Rows.Count == 0)
            {
                log.LogMethodExit();
                return;
            }
            string[] xaxisLaneName = new string[dtplayStatistics.Rows.Count];
            double[] yaxisPlayCount = new double[dtplayStatistics.Rows.Count];

            for (int i = 0; i < dtplayStatistics.Rows.Count; i++)
            {
                xaxisLaneName[i] = dtplayStatistics.Rows[i][0].ToString();
                yaxisPlayCount[i] = Convert.ToDouble(dtplayStatistics.Rows[i][1]);
            }

            BarItem myCurve = gamePane.AddBar(" ", null, yaxisPlayCount, Color.Blue);

            myCurve.Bar.Fill = new Fill(Color.OrangeRed);
            myCurve.Bar.Fill.Type = FillType.GradientByY;
            gamePane.Chart.Fill = new Fill(Color.White, Color.FromArgb(255, 255, 220), 45);
            gamePane.Fill = new Fill(Color.White);

            gamePane.XAxis.Scale.TextLabels = xaxisLaneName;
            if (dtplayStatistics.Rows.Count > 20)
                gamePane.XAxis.Scale.FontSpec.Angle = 90;
            else
                gamePane.XAxis.Scale.FontSpec.Angle = 90;

            gamePane.XAxis.Scale.FontSpec.IsBold = false;
            gamePane.XAxis.Scale.FontSpec.Size = 10;
            gamePane.XAxis.Scale.FontSpec.FontColor = Color.Gray;
            gamePane.XAxis.MajorTic.IsBetweenLabels = false;
            gamePane.XAxis.MajorTic.Color = Color.Gray;
            gamePane.XAxis.Type = AxisType.Text;

            gamePane.YAxis.Scale.FontSpec.IsBold = false;
            gamePane.YAxis.Scale.FontSpec.Size = 10;
            gamePane.YAxis.MajorTic.IsBetweenLabels = false;
            gamePane.YAxis.MinorTic.IsAllTics = false;
            gamePane.YAxis.MajorTic.Color = Color.Gray;
            gamePane.YAxis.Scale.Mag = 0;
            gamePane.YAxis.Scale.FontSpec.FontColor = Color.Gray;
            gamePane.YAxis.Scale.Format = "N0";

            gamePane.Border.Color = Color.LightGray;
            gamePane.Chart.Border.Color = Color.Gray;

            
            z.AxisChange();
            BarLablesCreate(gamePane, false, "N0");
            log.LogMethodExit();
        }

        private void BarLablesCreate(GraphPane pane, bool isBarCenter, string valueFormat)
        {
            log.LogMethodEntry(pane, isBarCenter, valueFormat);
            bool isVertical = pane.BarSettings.Base == BarBase.X;

            // Make the gap between the bars and the labels = 2% of the axis range
            float labelOffset;
            if (isVertical)
                labelOffset = (float)(pane.YAxis.Scale.Max - pane.YAxis.Scale.Min) * 0.05f;
            else
                labelOffset = (float)(pane.XAxis.Scale.Max - pane.XAxis.Scale.Min) * 0.05f;

            // keep a count of the number of BarItems
            int curveIndex = 0;

            // Get a valuehandler to do some calculations for us
            ValueHandler valueHandler = new ValueHandler(pane, true);

            // Loop through each curve in the list
            foreach (CurveItem curve in pane.CurveList)
            {
                // work with BarItems only
                BarItem bar = curve as BarItem;
                if (bar != null)
                {
                    IPointList points = curve.Points;

                    // Loop through each point in the BarItem
                    for (int i = 0; i < points.Count; i++)
                    {
                        // Get the high, low and base values for the current bar
                        // note that this method will automatically calculate the "effective"
                        // values if the bar is stacked
                        double baseVal, lowVal, hiVal;
                        valueHandler.GetValues(curve, i, out baseVal, out lowVal, out hiVal);

                        // Get the value that corresponds to the center of the bar base
                        // This method figures out how the bars are positioned within a cluster
                        float centerVal = (float)valueHandler.BarCenterValue(bar,
                            bar.GetBarWidth(pane), i, baseVal, curveIndex);

                        // Create a text label -- note that we have to go back to the original point
                        // data for this, since hiVal and lowVal could be "effective" values from a bar stack
                        string barLabelText = (isVertical ? points[i].Y : points[i].X).ToString(valueFormat);
                        //string barLabelText = (isVertical ? points[i].Y.ToString() : points[i].X.ToString());
                        // Calculate the position of the label -- this is either the X or the Y coordinate
                        // depending on whether they are horizontal or vertical bars, respectively
                        float position;
                        if (isBarCenter)
                            position = (float)(hiVal + lowVal) / 2.0f;
                        else
                            position = (float)hiVal + labelOffset;

                        // Create the new TextObj
                        TextObj label;
                        if (isVertical)
                            label = new TextObj(barLabelText, centerVal, position);
                        else
                            label = new TextObj(barLabelText, position, centerVal);

                        // Configure the TextObj
                        label.Location.CoordinateFrame = CoordType.AxisXYScale;
                        label.FontSpec.Size = 12;
                        label.FontSpec.FontColor = Color.Black;
                        label.FontSpec.Angle = 0;// isVertical ? 90 : 0;
                        label.Location.AlignH = isBarCenter ? AlignH.Center : AlignH.Left;
                        label.Location.AlignV = AlignV.Center;
                        label.FontSpec.Border.IsVisible = false;
                        label.FontSpec.Fill.IsVisible = false;

                        // Add the TextObj to the GraphPane
                        pane.GraphObjList.Add(label);
                    }
                }
                curveIndex++;
            }
            log.LogMethodExit();
        }
        public DataTable getDetails()
        {
            log.LogMethodEntry();
            DataTable dt = new DataTable();
            string techCard = string.Empty;
            string playdate = string.Empty;
            TimeSpan diff = dtend.Value - dtfrom.Value;
            totaldays = diff.Days + 1;
            lblNoOfDays.Text = totaldays.ToString() + " Days";
            if (chkIncludeTechCard.Checked)
                techCard = "";
            else
                techCard = " and isnull(cardTypeID,0) not in( select CardTypeId from CardType where CardType like '%HE Tech Card%')";

            playdate = " play_date >= @fromDate and play_date < @toDate";
            try
            {
                SqlCommand cmd = Common.Utilities.getCommand();
                cmd.CommandText = "select machine_name LaneName,count(cardgame) PlayCount from gamemetricview where" +
                               playdate +
                             " and dbo.GetGameProfileValue(game_id,'QUEUE_SETUP_REQUIRED')= '1' " + techCard +
                              "group by machine_name";

                cmd.Parameters.AddWithValue("@fromDate", dtfrom.Value.Date.AddHours(6));
                cmd.Parameters.AddWithValue("@toDate", dtend.Value.Date.AddDays(1).AddHours(6));

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit(dt);
            return dt;
        }

        private void GraphicalView_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            btnSubmit.PerformClick();
            chkIncludeTechCard.Checked = true;
            log.LogMethodExit();
        }

    }
}
