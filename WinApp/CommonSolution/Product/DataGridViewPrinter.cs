using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Data;
using System.Windows.Forms;

namespace Semnox.Parafait.Product
{
    public class DataGridViewPrinter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataGridView TheDataGridView; // The DataGridView Control which will be printed
        private PrintDocument ThePrintDocument; // The PrintDocument to be used for printing
        private bool IsCenterOnPage; // Determine if the report will be printed in the Top-Center of the page
        private bool IsWithTitle; // Determine if the page contain title text
        private string TheTitleText; // The title text to be printed in each page (if IsWithTitle is set to true)
        private Font TheTitleFont; // The font to be used with the title text (if IsWithTitle is set to true)
        private Color TheTitleColor; // The color to be used with the title text (if IsWithTitle is set to true)
        private bool IsWithPaging; // Determine if paging is used
        private bool IsWithLogo; // Determine if logo is used

        static int CurrentRow; // A static parameter that keep track on which Row (in the DataGridView control) that should be printed

        static int PageNumber;

        private int PageWidth;
        private int PageHeight;
        private int LeftMargin;
        private int TopMargin;
        private int RightMargin;
        private int BottomMargin;

        private float CurrentY; // A parameter that keep track on the y coordinate of the page, so the next object to be printed will start from this y coordinate

        private float RowHeaderHeight;
        private List<float> RowsHeight;
        private List<float> ColumnsWidth;
        private float TheDataGridViewWidth;

        // Maintain a generic list to hold start/stop points for the column printing
        // This will be used for wrapping in situations where the DataGridView will not fit on a single page
        private List<int[]> mColumnPoints;
        private List<float> mColumnPointsWidth;
        private int mColumnPoint;

        private Image companyLogo;

        // The class constructor with logo entry point -- iqbal
        public DataGridViewPrinter(DataGridView aDataGridView, PrintDocument aPrintDocument, bool CenterOnPage, bool WithTitle, string aTitleText, Font aTitleFont, Color aTitleColor, bool WithPaging, Image acompanyLogo)
        {
            log.LogMethodEntry(aDataGridView, aPrintDocument, CenterOnPage, WithTitle, aTitleText, aTitleFont, aTitleColor, WithPaging, acompanyLogo);
            IsWithLogo = true;
            companyLogo = acompanyLogo;
            Constructor(aDataGridView, aPrintDocument, CenterOnPage, WithTitle, aTitleText, aTitleFont, aTitleColor, WithPaging);
            log.LogMethodExit(null);
        }

        // The class constructor entry point
        public DataGridViewPrinter(DataGridView aDataGridView, PrintDocument aPrintDocument, bool CenterOnPage, bool WithTitle, string aTitleText, Font aTitleFont, Color aTitleColor, bool WithPaging)
        {
            log.LogMethodEntry(aDataGridView, aPrintDocument, CenterOnPage, WithTitle, aTitleText, aTitleFont, aTitleColor, WithPaging);
            Constructor(aDataGridView, aPrintDocument, CenterOnPage, WithTitle, aTitleText, aTitleFont, aTitleColor, WithPaging);
            log.LogMethodExit(null);
        }

        // The class constructor -- iqbal
        private void Constructor(DataGridView aDataGridView, PrintDocument aPrintDocument, bool CenterOnPage, bool WithTitle, string aTitleText, Font aTitleFont, Color aTitleColor, bool WithPaging)
        {
            log.LogMethodEntry(aDataGridView, aPrintDocument, CenterOnPage, WithTitle, aTitleText, aTitleFont, aTitleColor, WithPaging);
            TheDataGridView = aDataGridView;
            ThePrintDocument = aPrintDocument;
            IsCenterOnPage = CenterOnPage;
            IsWithTitle = WithTitle;
            TheTitleText = aTitleText;
            TheTitleFont = aTitleFont;
            TheTitleColor = aTitleColor;
            IsWithPaging = WithPaging;

            PageNumber = 0;

            RowsHeight = new List<float>();
            ColumnsWidth = new List<float>();

            mColumnPoints = new List<int[]>();
            mColumnPointsWidth = new List<float>();

            // Claculating the PageWidth and the PageHeight
            if (!ThePrintDocument.DefaultPageSettings.Landscape)
            {
                PageWidth = ThePrintDocument.DefaultPageSettings.PaperSize.Width;
                PageHeight = ThePrintDocument.DefaultPageSettings.PaperSize.Height;
            }
            else
            {
                PageHeight = ThePrintDocument.DefaultPageSettings.PaperSize.Width;
                PageWidth = ThePrintDocument.DefaultPageSettings.PaperSize.Height;
            }

            // Claculating the page margins
            LeftMargin = ThePrintDocument.DefaultPageSettings.Margins.Left;
            TopMargin = ThePrintDocument.DefaultPageSettings.Margins.Top;
            RightMargin = ThePrintDocument.DefaultPageSettings.Margins.Right;
            BottomMargin = ThePrintDocument.DefaultPageSettings.Margins.Bottom;

            // First, the current row to be printed is the first row in the DataGridView control
            CurrentRow = 0;
            log.LogMethodExit(null);
        }

        // The function that calculate the height of each row (including the header row), the width of each column (according to the longest text in all its cells including the header cell), and the whole DataGridView width
        private void Calculate(Graphics g)
        {
            log.LogMethodEntry(g);
            if (PageNumber == 0) // Just calculate once
            {
                SizeF tmpSize = new SizeF();
                Font tmpFont;
                float tmpWidth;

                TheDataGridViewWidth = 0;
                for (int i = 0; i < TheDataGridView.Columns.Count; i++)
                {
                    tmpFont = TheDataGridView.ColumnHeadersDefaultCellStyle.Font;
                    if (tmpFont == null) // If there is no special HeaderFont style, then use the default DataGridView font style
                        tmpFont = TheDataGridView.DefaultCellStyle.Font;

                    // iqbal
                    //tmpSize = g.MeasureString(TheDataGridView.Columns[i].HeaderText, tmpFont);
                    tmpSize = TheDataGridView.Columns[i].HeaderCell.Size;
                    tmpWidth = tmpSize.Width;
                    RowHeaderHeight = tmpSize.Height;

                    for (int j = 0; j < TheDataGridView.Rows.Count; j++)
                    {
                        tmpFont = TheDataGridView.Rows[j].DefaultCellStyle.Font;
                        if (tmpFont == null) // If the there is no special font style of the CurrentRow, then use the default one associated with the DataGridView control
                            tmpFont = TheDataGridView.DefaultCellStyle.Font;

                        tmpSize = g.MeasureString("Anything", tmpFont);
                        RowsHeight.Add(tmpSize.Height);

                        if (TheDataGridView.Rows[j].Cells[i].EditedFormattedValue != null)
                        {
                            tmpSize = g.MeasureString(TheDataGridView.Rows[j].Cells[i].EditedFormattedValue.ToString(), tmpFont);
                        }
                        
                        if (tmpSize.Width > tmpWidth)
                            tmpWidth = tmpSize.Width;
                    }
                    if (TheDataGridView.Columns[i].Visible)
                        TheDataGridViewWidth += tmpWidth;
                    ColumnsWidth.Add(tmpWidth);
                }

                // Define the start/stop column points based on the page width and the DataGridView Width
                // We will use this to determine the columns which are drawn on each page and how wrapping will be handled
                // By default, the wrapping will occurr such that the maximum number of columns for a page will be determine
                int k;

                int mStartPoint = 0;
                for (k = 0; k < TheDataGridView.Columns.Count; k++)
                    if (TheDataGridView.Columns[k].Visible)
                    {
                        mStartPoint = k;
                        break;
                    }

                int mEndPoint = TheDataGridView.Columns.Count;
                for (k = TheDataGridView.Columns.Count - 1; k >= 0; k--)
                    if (TheDataGridView.Columns[k].Visible)
                    {
                        mEndPoint = k + 1;
                        break;
                    }

                float mTempWidth = TheDataGridViewWidth;
                float mTempPrintArea = (float)PageWidth - (float)LeftMargin - (float)RightMargin;

                // We only care about handling where the total datagridview width is bigger then the print area
                if (TheDataGridViewWidth > mTempPrintArea)
                {
                    mTempWidth = 0.0F;
                    for (k = 0; k < TheDataGridView.Columns.Count; k++)
                    {
                        if (TheDataGridView.Columns[k].Visible)
                        {
                            mTempWidth += ColumnsWidth[k];
                            // If the width is bigger than the page area, then define a new column print range
                            if (mTempWidth > mTempPrintArea)
                            {
                                mTempWidth -= ColumnsWidth[k];
                                mColumnPoints.Add(new int[] { mStartPoint, mEndPoint });
                                mColumnPointsWidth.Add(mTempWidth);
                                mStartPoint = k;
                                mTempWidth = ColumnsWidth[k];
                            }
                        }
                        // Our end point is actually one index above the current index
                        mEndPoint = k + 1;
                    }
                }
                // Add the last set of columns
                mColumnPoints.Add(new int[] { mStartPoint, mEndPoint });
                mColumnPointsWidth.Add(mTempWidth);
                mColumnPoint = 0;
            }
            log.LogMethodExit(null);
        }

        // The funtion that print the title, page number, and the header row
        private void DrawHeader(Graphics g)
        {
            log.LogMethodEntry(g);
            CurrentY = (float)TopMargin;

            // Printing the logo (if IsWithLogo is set to true)
            if (IsWithLogo)
            {
                RectangleF LogoRectangle = new RectangleF((float)LeftMargin, CurrentY, companyLogo.Width, companyLogo.Height);

                g.DrawImage(companyLogo, LogoRectangle);

                CurrentY += companyLogo.Height;
            }

            // Printing the page number (if isWithPaging is set to true)
            if (IsWithPaging)
            {
                PageNumber++;
                string PageString = "Page " + PageNumber.ToString();

                StringFormat PageStringFormat = new StringFormat();
                PageStringFormat.Trimming = StringTrimming.Word;
                PageStringFormat.FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
                PageStringFormat.Alignment = StringAlignment.Far;

                Font PageStringFont = new Font("Tahoma", 8, FontStyle.Regular, GraphicsUnit.Point);

                RectangleF PageStringRectangle = new RectangleF((float)LeftMargin, CurrentY, (float)PageWidth - (float)RightMargin - (float)LeftMargin, g.MeasureString(PageString, PageStringFont).Height);

                g.DrawString(PageString, PageStringFont, new SolidBrush(Color.Black), PageStringRectangle, PageStringFormat);

                CurrentY += g.MeasureString(PageString, PageStringFont).Height;
            }

            // Printing the title (if IsWithTitle is set to true)
            if (IsWithTitle)
            {
                StringFormat TitleFormat = new StringFormat();
                TitleFormat.Trimming = StringTrimming.Word;
                TitleFormat.FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
                if (IsCenterOnPage)
                    TitleFormat.Alignment = StringAlignment.Center;
                else
                    TitleFormat.Alignment = StringAlignment.Near;

                RectangleF TitleRectangle = new RectangleF((float)LeftMargin, CurrentY, (float)PageWidth - (float)RightMargin - (float)LeftMargin, g.MeasureString(TheTitleText, TheTitleFont).Height);

                g.DrawString(TheTitleText, TheTitleFont, new SolidBrush(TheTitleColor), TitleRectangle, TitleFormat);

                CurrentY += g.MeasureString(TheTitleText, TheTitleFont).Height;
            }

            // Calculating the starting x coordinate that the printing process will start from
            float CurrentX = (float)LeftMargin;
            if (IsCenterOnPage)
                CurrentX += (((float)PageWidth - (float)RightMargin - (float)LeftMargin) - mColumnPointsWidth[mColumnPoint]) / 2.0F;

            // Setting the HeaderFore style
            Color HeaderForeColor = TheDataGridView.ColumnHeadersDefaultCellStyle.ForeColor;
            if (HeaderForeColor.IsEmpty) // If there is no special HeaderFore style, then use the default DataGridView style
                HeaderForeColor = TheDataGridView.DefaultCellStyle.ForeColor;
            SolidBrush HeaderForeBrush = new SolidBrush(HeaderForeColor);

            // Setting the HeaderBack style
            Color HeaderBackColor = TheDataGridView.ColumnHeadersDefaultCellStyle.BackColor;
            if (HeaderBackColor.IsEmpty) // If there is no special HeaderBack style, then use the default DataGridView style
                HeaderBackColor = TheDataGridView.DefaultCellStyle.BackColor;
            SolidBrush HeaderBackBrush = new SolidBrush(HeaderBackColor);

            // Setting the LinePen that will be used to draw lines and rectangles (derived from the GridColor property of the DataGridView control)
            Pen TheLinePen = new Pen(TheDataGridView.GridColor, 1);

            // Setting the HeaderFont style
            Font HeaderFont = TheDataGridView.ColumnHeadersDefaultCellStyle.Font;
            if (HeaderFont == null) // If there is no special HeaderFont style, then use the default DataGridView font style
                HeaderFont = TheDataGridView.DefaultCellStyle.Font;

            // Calculating and drawing the HeaderBounds        
            RectangleF HeaderBounds = new RectangleF(CurrentX, CurrentY, mColumnPointsWidth[mColumnPoint], RowHeaderHeight);
            g.FillRectangle(HeaderBackBrush, HeaderBounds);

            // Setting the format that will be used to print each cell of the header row
            StringFormat CellFormat = new StringFormat();
            CellFormat.Trimming = StringTrimming.Word;
            // CellFormat.FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            //iqbal
            CellFormat.FormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;

            // Printing each visible cell of the header row
            RectangleF CellBounds;
            float ColumnWidth;
            for (int i = (int)mColumnPoints[mColumnPoint].GetValue(0); i < (int)mColumnPoints[mColumnPoint].GetValue(1); i++)
            {
                if (!TheDataGridView.Columns[i].Visible) continue; // If the column is not visible then ignore this iteration

                ColumnWidth = ColumnsWidth[i];

                // Check the CurrentCell alignment and apply it to the CellFormat
                if (TheDataGridView.ColumnHeadersDefaultCellStyle.Alignment.ToString().Contains("Right"))
                    CellFormat.Alignment = StringAlignment.Far;
                else if (TheDataGridView.ColumnHeadersDefaultCellStyle.Alignment.ToString().Contains("Center"))
                    CellFormat.Alignment = StringAlignment.Center;
                else
                    CellFormat.Alignment = StringAlignment.Near;

                CellBounds = new RectangleF(CurrentX, CurrentY, ColumnWidth, RowHeaderHeight);

                // Printing the cell text
                g.DrawString(TheDataGridView.Columns[i].HeaderText, HeaderFont, HeaderForeBrush, CellBounds, CellFormat);

                // Drawing the cell bounds
                if (TheDataGridView.RowHeadersBorderStyle != DataGridViewHeaderBorderStyle.None) // Draw the cell border only if the HeaderBorderStyle is not None
                    g.DrawRectangle(TheLinePen, CurrentX, CurrentY, ColumnWidth, RowHeaderHeight);

                CurrentX += ColumnWidth;
            }

            CurrentY += RowHeaderHeight;
            log.LogMethodExit(null);
        }

        // The function that print a bunch of rows that fit in one page
        // When it returns true, meaning that there are more rows still not printed, so another PagePrint action is required
        // When it returns false, meaning that all rows are printed (the CureentRow parameter reaches the last row of the DataGridView control) and no further PagePrint action is required
        private bool DrawRows(Graphics g)
        {
            log.LogMethodEntry(g);
            // Setting the LinePen that will be used to draw lines and rectangles (derived from the GridColor property of the DataGridView control)
            Pen TheLinePen = new Pen(TheDataGridView.GridColor, 1);

            // The style paramters that will be used to print each cell
            Font RowFont;
            Color RowForeColor;
            Color RowBackColor;
            SolidBrush RowForeBrush;
            SolidBrush RowBackBrush;
            SolidBrush RowAlternatingBackBrush;

            // Setting the format that will be used to print each cell
            StringFormat CellFormat = new StringFormat();
            CellFormat.Trimming = StringTrimming.Word;
            CellFormat.FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.LineLimit;

            // Printing each visible cell
            RectangleF RowBounds;
            float CurrentX;
            float ColumnWidth;
            while (CurrentRow < TheDataGridView.Rows.Count)
            {
                if (TheDataGridView.Rows[CurrentRow].Visible) // Print the cells of the CurrentRow only if that row is visible
                {
                    // Setting the row font style
                    RowFont = TheDataGridView.Rows[CurrentRow].DefaultCellStyle.Font;
                    if (RowFont == null) // If the there is no special font style of the CurrentRow, then use the default one associated with the DataGridView control
                        RowFont = TheDataGridView.DefaultCellStyle.Font;

                    // Setting the RowFore style
                    RowForeColor = TheDataGridView.Rows[CurrentRow].DefaultCellStyle.ForeColor;
                    if (RowForeColor.IsEmpty) // If the there is no special RowFore style of the CurrentRow, then use the default one associated with the DataGridView control
                        RowForeColor = TheDataGridView.DefaultCellStyle.ForeColor;
                    RowForeBrush = new SolidBrush(RowForeColor);

                    // Setting the RowBack (for even rows) and the RowAlternatingBack (for odd rows) styles
                    RowBackColor = TheDataGridView.Rows[CurrentRow].DefaultCellStyle.BackColor;
                    if (RowBackColor.IsEmpty) // If the there is no special RowBack style of the CurrentRow, then use the default one associated with the DataGridView control
                    {
                        RowBackBrush = new SolidBrush(TheDataGridView.DefaultCellStyle.BackColor);
                        RowAlternatingBackBrush = new SolidBrush(TheDataGridView.AlternatingRowsDefaultCellStyle.BackColor);
                    }
                    else // If the there is a special RowBack style of the CurrentRow, then use it for both the RowBack and the RowAlternatingBack styles
                    {
                        RowBackBrush = new SolidBrush(RowBackColor);
                        RowAlternatingBackBrush = new SolidBrush(RowBackColor);
                    }

                    // Calculating the starting x coordinate that the printing process will start from
                    CurrentX = (float)LeftMargin;
                    if (IsCenterOnPage)
                        CurrentX += (((float)PageWidth - (float)RightMargin - (float)LeftMargin) - mColumnPointsWidth[mColumnPoint]) / 2.0F;

                    // Calculating the entire CurrentRow bounds                
                    RowBounds = new RectangleF(CurrentX, CurrentY, mColumnPointsWidth[mColumnPoint], RowsHeight[CurrentRow]);

                    // Filling the back of the CurrentRow
                    if (CurrentRow % 2 == 0)
                        g.FillRectangle(RowBackBrush, RowBounds);
                    else
                        g.FillRectangle(RowAlternatingBackBrush, RowBounds);

                    // Printing each visible cell of the CurrentRow                
                    for (int CurrentCell = (int)mColumnPoints[mColumnPoint].GetValue(0); CurrentCell < (int)mColumnPoints[mColumnPoint].GetValue(1); CurrentCell++)
                    {
                        if (!TheDataGridView.Columns[CurrentCell].Visible) continue; // If the cell is belong to invisible column, then ignore this iteration

                        // Check the CurrentCell alignment and apply it to the CellFormat
                        // added by iqbal to check cell level alignment setting
                        if (TheDataGridView.Rows[CurrentRow].Cells[CurrentCell].Style.Alignment.ToString() != "NotSet")
                        {
                            if (TheDataGridView.Rows[CurrentRow].Cells[CurrentCell].Style.Alignment.ToString().Contains("Right"))
                                CellFormat.Alignment = StringAlignment.Far;
                            else if (TheDataGridView.Rows[CurrentRow].Cells[CurrentCell].Style.Alignment.ToString().Contains("Center"))
                                CellFormat.Alignment = StringAlignment.Center;
                            else
                                CellFormat.Alignment = StringAlignment.Near;
                        }
                        else
                        {
                            if (TheDataGridView.Columns[CurrentCell].DefaultCellStyle.Alignment.ToString().Contains("Right"))
                                CellFormat.Alignment = StringAlignment.Far;
                            else if (TheDataGridView.Columns[CurrentCell].DefaultCellStyle.Alignment.ToString().Contains("Center"))
                                CellFormat.Alignment = StringAlignment.Center;
                            else
                                CellFormat.Alignment = StringAlignment.Near;
                        }
                        ColumnWidth = ColumnsWidth[CurrentCell];
                        RectangleF CellBounds = new RectangleF(CurrentX, CurrentY, ColumnWidth, RowsHeight[CurrentRow]);

                        // iqbal - cell back color added
                        Color cellBackColor = TheDataGridView.Rows[CurrentRow].Cells[CurrentCell].Style.BackColor;
                        if (!cellBackColor.IsEmpty)
                        {
                            SolidBrush cellBackBrush = new SolidBrush(cellBackColor);
                            g.FillRectangle(cellBackBrush, CellBounds);
                        }

                        // Printing the cell text
                        if (TheDataGridView.Rows[CurrentRow].Cells[CurrentCell].EditedFormattedValue != null)
                        {
                            g.DrawString(TheDataGridView.Rows[CurrentRow].Cells[CurrentCell].EditedFormattedValue.ToString(), RowFont, RowForeBrush, CellBounds, CellFormat);
                        }

                        // Drawing the cell bounds
                        if (TheDataGridView.CellBorderStyle != DataGridViewCellBorderStyle.None) // Draw the cell border only if the CellBorderStyle is not None
                            g.DrawRectangle(TheLinePen, CurrentX, CurrentY, ColumnWidth, RowsHeight[CurrentRow]);

                        CurrentX += ColumnWidth;
                    }
                    CurrentY += RowsHeight[CurrentRow];

                    // Checking if the CurrentY is exceeds the page boundries
                    // If so then exit the function and returning true meaning another PagePrint action is required
                    if ((int)CurrentY > (PageHeight - TopMargin - BottomMargin))
                    {
                        CurrentRow++;
                        log.LogMethodExit(true);
                        return true;
                    }
                }
                CurrentRow++;
            }

            CurrentRow = 0;
            mColumnPoint++; // Continue to print the next group of columns

            if (mColumnPoint == mColumnPoints.Count) // Which means all columns are printed
            {
                mColumnPoint = 0;
                log.LogMethodExit(false);
                return false;
            }
            else
            {
                log.LogMethodExit(true);
                return true;
            }
           
        }

        // The method that calls all other functions
        public bool DrawDataGridView(Graphics g)
        {
            log.LogMethodEntry(g);
            try
            {
                Calculate(g);
                DrawHeader(g);
                bool bContinue = DrawRows(g);
                return bContinue;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while drawing data grid view", ex);
                MessageBox.Show("Operation failed: " + ex.Message.ToString(), Application.ProductName + " - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                log.LogMethodExit(false);
                return false;
            }
         }
    }
}
