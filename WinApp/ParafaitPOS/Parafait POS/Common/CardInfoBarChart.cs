/********************************************************************************************
 * Project Name - Common
 * Description  -  Class for CardInfoBarChart
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.80         20-Aug-2019     Girish Kundar  Modified : Removed unused namespace's and Added logger methods. 
 ********************************************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using ZedGraph;

namespace Parafait_POS
{
    static class CardInfoBarChart
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static DataTable getGraphTable(int CardId, DateTime FromDate, DateTime ToDate)
        {
            log.LogMethodEntry("CardId", FromDate, ToDate);
            string command =
                "select machine_name, total_plays, credit_amount + bonus_amount + time_amount as total_amount " +
                "from " +
                    "(select machine_name, machine_address, " +
                    " count(*) total_plays," +
                    " sum(credits * Convert(float, dbo.get_parafait_defaults('CREDIT_PRICE'))) as credit_amount, " +
                    " sum(courtesy * Convert(float, dbo.get_parafait_defaults('COURTESY_PRICE'))) as courtesy_amount, " +
                    " sum(bonus * Convert(float, dbo.get_parafait_defaults('BONUS_PRICE'))) as bonus_amount, " +
                    " sum(time * Convert(float, dbo.get_parafait_defaults('CREDIT_PRICE'))) as time_amount " +
                    " from gameplay t, machines m " + 
                    "where play_date >= @fromdate and play_date < @todate " +
                    "and credits + courtesy + time + bonus > 0 " +
                    "and m.machine_id = t.machine_id " +
                    "and t.card_id = @card_id " +
                    "group by machine_address, machine_name) a " +
                " order by 3 desc";

            SqlCommand cmd = POSStatic.Utilities.getCommand();
            cmd.CommandText = command;
            cmd.Parameters.AddWithValue("@card_id", CardId);
            cmd.Parameters.AddWithValue("@fromdate", FromDate);
            cmd.Parameters.AddWithValue("@todate", ToDate);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            DataTable BarGraphDT = new DataTable();
            BarGraphDT.Columns.Add("GameName");
            BarGraphDT.Columns.Add("TotalPlays");
            BarGraphDT.Columns.Add("TotalAmount");

            for (int i = 0; i < dt.Rows.Count; i++) // populate a new DT with only machine rows and grand total
            {
                BarGraphDT.Rows.Add(new object[] { dt.Rows[i]["machine_name"], dt.Rows[i]["total_plays"], dt.Rows[i]["total_amount"] });
            }
            log.LogMethodExit();
            return BarGraphDT;
        }

        public static void CreateGraph(ZedGraphControl z1, int CardId, DateTime FromDate, DateTime Todate, bool isCount)
        {
            log.LogMethodEntry("CardId", FromDate, Todate, isCount);
            GraphPane myPane = z1.GraphPane;
            myPane.CurveList.Clear();
            myPane.GraphObjList.Clear();

            myPane.Title.Text = "Game Play Chart";
            myPane.XAxis.Title.Text = "Game Name";
            if (isCount)
                myPane.YAxis.Title.Text = "Game Count";
            else
                myPane.YAxis.Title.Text = "Amount";
            myPane.Title.FontSpec.FontColor = Color.Gray;
            myPane.Title.FontSpec.Size = 12;

            DataTable DT_collection = getGraphTable(CardId, FromDate, Todate) ;
            if (DT_collection.Rows.Count == 0)
                return;

            string[] xAxisArr = new string[DT_collection.Rows.Count];
            double[] yAxisArr = new double[DT_collection.Rows.Count];

            for (int i = 0; i < DT_collection.Rows.Count; i++)
            {
                xAxisArr[i] = DT_collection.Rows[i][0].ToString();
                if (isCount)
                    yAxisArr[i] = Convert.ToDouble(DT_collection.Rows[i][1]);
                else
                    yAxisArr[i] = Convert.ToDouble(DT_collection.Rows[i][2]);
            }

            BarItem myCurve = myPane.AddBar(" ", null, yAxisArr, Color.Blue);

            myCurve.Bar.Fill = new Fill(Color.OrangeRed);
            myCurve.Bar.Fill.Type = FillType.GradientByY;
            myPane.Chart.Fill = new Fill(Color.White, Color.FromArgb(255, 255, 220), 45);
            myPane.Fill = new Fill(Color.White);

            myPane.XAxis.Scale.TextLabels = xAxisArr;
            if (DT_collection.Rows.Count > 20)
                myPane.XAxis.Scale.FontSpec.Angle = 90;
            else
                myPane.XAxis.Scale.FontSpec.Angle = 90;

            myPane.XAxis.Scale.FontSpec.IsBold = false;
            myPane.XAxis.Scale.FontSpec.Size = 12;
            myPane.XAxis.Scale.FontSpec.FontColor = Color.Gray;
            myPane.XAxis.MajorTic.IsBetweenLabels = false;
            myPane.XAxis.MajorTic.Color = Color.Gray;
            myPane.XAxis.Type = AxisType.Text;

            myPane.YAxis.Scale.FontSpec.IsBold = false;
            myPane.YAxis.Scale.FontSpec.Size = 12;
            myPane.YAxis.MajorTic.IsBetweenLabels = false;
            myPane.YAxis.MinorTic.IsAllTics = false;
            myPane.YAxis.MajorTic.Color = Color.Gray;
            myPane.YAxis.Scale.Mag = 0;
            myPane.YAxis.Scale.FontSpec.FontColor = Color.Gray;
            myPane.YAxis.Scale.Format = "N0";

            myPane.Border.Color = Color.LightGray;
            myPane.Chart.Border.Color = Color.Gray;

            // Tell ZedGraph to calculate the axis ranges
            z1.AxisChange();
            CreateBarLabels(myPane, false, "N0");
            z1.Refresh();
            log.LogMethodExit();
        }

        private static void CreateBarLabels(GraphPane pane, bool isBarCenter, string valueFormat)
        {
            log.LogMethodEntry(isBarCenter, valueFormat);
            bool isVertical = pane.BarSettings.Base == BarBase.X;

            // Make the gap between the bars and the labels = 2% of the axis range
            float labelOffset;
            if (isVertical)
                labelOffset = (float)(pane.YAxis.Scale.Max - pane.YAxis.Scale.Min) * 0.02f;
            else
                labelOffset = (float)(pane.XAxis.Scale.Max - pane.XAxis.Scale.Min) * 0.02f;

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
    }
}
