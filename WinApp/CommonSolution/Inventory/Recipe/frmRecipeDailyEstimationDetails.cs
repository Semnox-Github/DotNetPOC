/********************************************************************************************
 * Project Name - Inventory
 * Description  - UI to display Daily Estimated Quantity
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.100.0       05-Oct-2020   Deeksha             Created for Recipe Management enhancement.
 *********************************************************************************************/
using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using Semnox.Parafait.Product;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Inventory.Recipe
{
    public partial class frmRecipeDailyEstimationDetails : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executioncontex = ExecutionContext.GetExecutionContext();
        private Utilities utilities;
        private readonly Dictionary<int, Point> calendarDays;
        private DateTime calendarDate;
        private List<RecipeEstimationHeaderDTO> recipeEstimationHeaderDTOList = null;
        private int productId;
        bool renderCalendar = true;
        int minMonth;
        int maxmonth;

        protected override CreateParams CreateParams
        {
            //this method is used to avoid the table layout flickering.
            get
            {
                CreateParams CP = base.CreateParams;
                CP.ExStyle = CP.ExStyle | 0x02000000;
                return CP;
            }
        }

        public frmRecipeDailyEstimationDetails(Utilities utilities, List<RecipeEstimationHeaderDTO> recipeEstimationHeaderDTOList, int productId)
        {
            log.LogMethodEntry(utilities, recipeEstimationHeaderDTOList, productId);
            InitializeComponent();
            this.utilities = utilities;
            this.recipeEstimationHeaderDTOList = recipeEstimationHeaderDTOList;
            this.productId = productId;
            minMonth = recipeEstimationHeaderDTOList.Min(x=>x.FromDate.Month);
            maxmonth = recipeEstimationHeaderDTOList.Max(x=>x.FromDate.Month);
            calendarDays = new Dictionary<int, Point>();
            if (recipeEstimationHeaderDTOList != null && recipeEstimationHeaderDTOList.Any())
            {
                if (ProductContainer.productDTOList != null && ProductContainer.productDTOList.Count > 0)
                {
                    lblEstimate.Text = MessageContainerList.GetMessage(executioncontex, "Daily Estimate for") + " <" + ProductContainer.productDTOList.Find(x => x.ProductId == productId).Description + ">";
                    lblEstimate.Font = new Font(lblEstimate.Font, FontStyle.Bold);
                }
                calendarDate = recipeEstimationHeaderDTOList[0].FromDate;
                SetMonthName(recipeEstimationHeaderDTOList[0].FromDate.Month , recipeEstimationHeaderDTOList[0].FromDate.Year);
            }
            else
            {
                calendarDate = utilities.getServerTime();
                SetMonthName(calendarDate.Month , calendarDate.Year);
            }
            ValidateMonthRange();
            log.LogMethodExit();
        }


        /// <summary>
        /// Sets Month View
        /// </summary>
        /// <param name="currentMonth"></param>
        private void SetMonthName(int currentMonth , int year)
        {
            log.LogMethodEntry(currentMonth);
            string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(currentMonth);
            grpMonth.Text = monthName + " - " + year.ToString();
            lblMonth.Text = monthName + " - " + year.ToString();
            log.LogMethodExit();
        }

        private void Calendar_Paint(object sender, PaintEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                    RenderMonthCalendar(e);
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Refresh Month View
        /// </summary>
        private void RefreshMonth()
        {
            log.LogMethodEntry();
            for (int i = 0; i < Calendar.Controls.Count; i++)
            {
                Control c = Calendar.Controls[i];
                if (c.Name.Contains("recipePlanDisplay"))
                {
                    Calendar.Controls.Remove(c);
                    c.Dispose();
                    i = -1;
                }
            }
            int cellWidth = 110;
            int cellHeight = 60;
            if (recipeEstimationHeaderDTOList != null && recipeEstimationHeaderDTOList.Any())
            {
                for (int i = 1; i <= DateTime.DaysInMonth(calendarDate.Year, calendarDate.Month); i++)
                {
                    DateTime dt = new DateTime(calendarDate.Year, calendarDate.Month, i);
                    RecipeEstimationHeaderDTO headerDTO = recipeEstimationHeaderDTOList.Find(x => x.FromDate.Date == dt.Date);
                    Label recipePlanDisplay = new Label();
                    recipePlanDisplay.Name = "recipePlanDisplay";
                    recipePlanDisplay.Font = new Font("arial", 8);
                    Point point = new Point(0,0);
                    if (calendarDays.Count > 0)
                    {
                        point = calendarDays[i];
                    }
                    string date = dt.ToString("yyyy-MM-dd");
                    if (headerDTO != null && ProductContainer.productDTOList != null && ProductContainer.productDTOList.Count > 0)
                    {
                        decimal qty = headerDTO.RecipeEstimationDetailsDTOList.FindAll(x => x.ProductId == productId).Sum(x => x.TotalEstimatedQty).Value;
                        int uomId = ProductContainer.productDTOList.Find(x => x.ProductId == productId).InventoryUOMId;
                        string uom = UOMContainer.uomDTOList.Find(x => x.UOMId == uomId).UOM;
                        string fromDate = headerDTO.FromDate.ToString("yyyy-MM-dd");
                        if (date == fromDate && qty > 0)
                        {

                            recipePlanDisplay.BorderStyle = BorderStyle.FixedSingle;
                            Color backColor = Color.LightSalmon;

                            recipePlanDisplay.Text = Environment.NewLine + "  " + qty.ToString() + " " + uom;

                            recipePlanDisplay.BackColor = backColor;
                            //recipePlanDisplay.ForeColor = color;

                            recipePlanDisplay.Tag = headerDTO.RecipeEstimationHeaderId;
                            recipePlanDisplay.Size = new Size(cellWidth - 10, cellHeight - 17);
                            recipePlanDisplay.Location = new Point(point.X + 1, point.Y + 1);
                            Calendar.Controls.Add(recipePlanDisplay);
                            recipePlanDisplay.BringToFront();
                        }
                    }
                }
            }
            Calendar.Refresh();
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates Month View
        /// </summary>
        /// <param name="e"></param>
        private void RenderMonthCalendar(PaintEventArgs e)
        {
            log.LogMethodEntry();
            calendarDays.Clear();
            int MarginSize = 2;
            int cellWidth = 110;
            int cellHeight = 60;
            Font dayOfWeekFont = new Font("Arial", 8, FontStyle.Regular);
            Font daysFont = new Font("Arial", 8, FontStyle.Regular);
            Font todayFont = new Font("Arial", 8, FontStyle.Bold);
            Font dateHeaderFont = new Font("Arial", 10, FontStyle.Bold);
            var bmp = new Bitmap(ClientSize.Width, ClientSize.Height);
            Graphics g = Graphics.FromImage(bmp);
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            g.Clear(Color.LightYellow);
            SizeF sunSize = g.MeasureString("Sun", dayOfWeekFont);
            SizeF monSize = g.MeasureString("Mon", dayOfWeekFont);
            SizeF tueSize = g.MeasureString("Tue", dayOfWeekFont);
            SizeF wedSize = g.MeasureString("Wed", dayOfWeekFont);
            SizeF thuSize = g.MeasureString("Thu", dayOfWeekFont);
            SizeF friSize = g.MeasureString("Fri", dayOfWeekFont);
            SizeF satSize = g.MeasureString("Sat", dayOfWeekFont);
            SizeF dateHeaderSize = g.MeasureString(
                calendarDate.ToString("MMMM") + " " + calendarDate.Year.ToString(CultureInfo.InvariantCulture), dateHeaderFont);
            int headerSpacing = 15;
            int controlsSpacing = 5;

            DateTime date = new DateTime(calendarDate.Year, calendarDate.Month, DateTime.DaysInMonth(calendarDate.Year, calendarDate.Month));
            var beginningOfMonth = new DateTime(date.Year, date.Month, 1);

            while (date.Date.AddDays(1).DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek)
                date = date.AddDays(1);

            log.LogMethodExit();
            int numWeeks = (int)Math.Truncate(date.Subtract(beginningOfMonth).TotalDays / 7f) + 1;

            int xStart = MarginSize;
            int yStart = MarginSize;
            DayOfWeek startWeekEnum = new DateTime(calendarDate.Year, calendarDate.Month, 1).DayOfWeek;
            int startWeek = ((int)startWeekEnum) + 1;
            int rogueDays = startWeek - 1;

            yStart += headerSpacing + controlsSpacing;

            int counter = 1;
            int counter2 = 1;

            bool first = false;
            bool first2 = false;
            for (int y = 0; y < numWeeks; y++)
            {
                for (int x = 0; x < 7; x++)
                {
                    if (rogueDays == 0 && counter <= DateTime.DaysInMonth(calendarDate.Year, calendarDate.Month))
                    {
                        if (!calendarDays.ContainsKey(counter))
                            calendarDays.Add(counter, new Point(xStart, (int)(yStart + 2f + g.MeasureString(counter.ToString(CultureInfo.InvariantCulture), daysFont).Height)));
                        if (first == false)
                        {
                            first = true;
                            if (calendarDate.Year == ServerDateTime.Now.Year && calendarDate.Month == ServerDateTime.Now.Month
                         && counter == ServerDateTime.Now.Day)
                            {
                                g.DrawString(
                                    calendarDate.ToString("MMM") + " " + counter.ToString(CultureInfo.InvariantCulture),
                                    todayFont, Brushes.Black, xStart + 1, yStart + 1);
                            }
                            else
                            {
                                g.DrawString(
                                    calendarDate.ToString("MMM") + " " + counter.ToString(CultureInfo.InvariantCulture),
                                    daysFont, Brushes.Black, xStart + 1, yStart + 1);
                            }
                        }
                        else
                        {
                            if (calendarDate.Year == ServerDateTime.Now.Year && calendarDate.Month == ServerDateTime.Now.Month
                         && counter == ServerDateTime.Now.Day)
                            {
                                g.DrawString(counter.ToString(CultureInfo.InvariantCulture), todayFont, Brushes.Black, xStart + 1, yStart + 1);
                            }
                            else
                            {
                                g.DrawString(counter.ToString(CultureInfo.InvariantCulture), daysFont, Brushes.Black, xStart + 1, yStart + 1);
                            }
                        }
                        counter++;
                    }
                    else if (rogueDays > 0)
                    {
                        int dm =
                            DateTime.DaysInMonth(calendarDate.AddMonths(-1).Year, calendarDate.AddMonths(-1).Month) -
                            rogueDays + 1;
                        g.DrawString(dm.ToString(CultureInfo.InvariantCulture), daysFont, new SolidBrush(Color.FromArgb(170, 170, 170)), xStart + 1, yStart + 1);
                        rogueDays--;
                    }

                    g.DrawRectangle(Pens.DarkGray, xStart, yStart, cellWidth, cellHeight);
                    if (rogueDays == 0 && counter > DateTime.DaysInMonth(calendarDate.Year, calendarDate.Month))
                    {
                        if (first2 == false)
                            first2 = true;
                        else
                        {
                            if (counter2 == 1)
                            {
                                g.DrawString(calendarDate.AddMonths(1).ToString("MMM") + " " + counter2.ToString(CultureInfo.InvariantCulture), daysFont,
                                             new SolidBrush(Color.FromArgb(170, 170, 170)), xStart + 1, yStart + 1);
                            }
                            else
                            {
                                g.DrawString(counter2.ToString(CultureInfo.InvariantCulture), daysFont,
                                             new SolidBrush(Color.FromArgb(170, 170, 170)), xStart + 1, yStart + 1);
                            }
                            counter2++;
                        }
                    }
                    xStart += cellWidth;
                }
                xStart = MarginSize;
                yStart += cellHeight;
            }
            xStart = MarginSize + ((cellWidth - (int)sunSize.Width) / 2);
            yStart = MarginSize + controlsSpacing;

            g.DrawString("Sun", dayOfWeekFont, Brushes.Black, xStart, yStart);
            xStart = MarginSize + ((cellWidth - (int)monSize.Width) / 2) + cellWidth;
            g.DrawString("Mon", dayOfWeekFont, Brushes.Black, xStart, yStart);

            xStart = MarginSize + ((cellWidth - (int)tueSize.Width) / 2) + cellWidth * 2;
            g.DrawString("Tue", dayOfWeekFont, Brushes.Black, xStart, yStart);

            xStart = MarginSize + ((cellWidth - (int)wedSize.Width) / 2) + cellWidth * 3;
            g.DrawString("Wed", dayOfWeekFont, Brushes.Black, xStart, yStart);

            xStart = MarginSize + ((cellWidth - (int)thuSize.Width) / 2) + cellWidth * 4;
            g.DrawString("Thu", dayOfWeekFont, Brushes.Black, xStart, yStart);

            xStart = MarginSize + ((cellWidth - (int)friSize.Width) / 2) + cellWidth * 5;
            g.DrawString("Fri", dayOfWeekFont, Brushes.Black, xStart, yStart);

            xStart = MarginSize + ((cellWidth - (int)satSize.Width) / 2) + cellWidth * 6;
            g.DrawString("Sat", dayOfWeekFont, Brushes.Black, xStart, yStart);

            //Show date in header
            g.DrawString(
                calendarDate.ToString("MMMM") + " " + calendarDate.Year.ToString(CultureInfo.InvariantCulture),
                dateHeaderFont, Brushes.Black, ClientSize.Width - MarginSize - dateHeaderSize.Width,
                MarginSize);

            g.Dispose();
            e.Graphics.DrawImage(bmp, 0, 0, ClientSize.Width, ClientSize.Height);
            bmp.Dispose();
            if (renderCalendar)
            {
                RefreshMonth();
                renderCalendar = false;
            }

            log.LogMethodExit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                calendarDate = calendarDate.AddMonths(-1);
                SetMonthName(calendarDate.Month, calendarDate.Year);
                Refresh();
                RefreshMonth();
                ValidateMonthRange();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Disable Prev / Next button if data does not exist.
        /// </summary>
        private void ValidateMonthRange()
        {
            log.LogMethodEntry();
            if (minMonth >= calendarDate.Month)
            {
                btnPrev.Enabled = false;
            }
            else
            {
                btnPrev.Enabled = true;
            }
            if (maxmonth <= calendarDate.Month)
            {
                btnNext.Enabled = false;
            }
            else
            {
                btnNext.Enabled = true;
            }
            log.LogMethodExit();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                calendarDate = calendarDate.AddMonths(1);
                SetMonthName(calendarDate.Month , calendarDate.Year);
                Refresh();
                RefreshMonth();
                ValidateMonthRange();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
    }
}
