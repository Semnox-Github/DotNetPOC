/********************************************************************************************
 * Project Name - Utilities
 * Description  - Utilities class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.50.0     14-Dec-2018      Raghuveera     Modified to get enrypted key and password values 
 *2.50.0     14-Dec-2018      Guru S A       Application security changes
 *2.70.2.0     23-Sep-2019      Mithesh        POS Version Check changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
//using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Globalization;
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using System.Text.RegularExpressions;
//using Semnox.Parafait.Device;
//using Semnox.Parafait.EncryptionUtils;
using Semnox.Parafait;
//using Semnox.Core.Utilities;

//using Semnox.Core.Security;
using Semnox.Parafait.logging;
using System.Windows.Forms;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.text;
using System.Threading;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.pipeline.html;
using iTextSharp.tool.xml.pipeline.end;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml.parser;
using iTextSharp.tool.xml.net;
using iTextSharp.tool.xml.css;
using System.Web.Hosting;
//using Semnox.Parafait.Device;
//using Semnox.Core.DBUtils;


namespace Semnox.Core.Utilities
{
    public class Utilities : IDisposable
    {
        //private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(MethodBase.GetCurrentMethod().DeclaringType);
        public SqlConnection sqlConnection;
        public string encryptedConnectionString;
        public ParafaitEnv ParafaitEnv;
        public MessageUtils MessageUtils;
        public EventLog EventLog;
        public MachineLog MachineLog;

        public DeviceClass ReaderDevice;
        public DBUtils DBUtilities { get; private set; }

        public Utilities(string encryptedConnectionString)
        {
            log.LogMethodEntry("encryptedConnectionString");
            this.DBUtilities = new DBUtils(encryptedConnectionString);
            this.encryptedConnectionString = encryptedConnectionString;
            init();
            log.LogMethodExit(null);
        }

        public Utilities(string encryptedConnectionString, ParafaitEnv cachedParafaitEnv)
        {
            log.LogMethodEntry("encryptedConnectionString");
            this.DBUtilities = new DBUtils(encryptedConnectionString);
            this.encryptedConnectionString = encryptedConnectionString;
            var dbUtils = this.DBUtilities;
            if(cachedParafaitEnv != null)
            {
                ParafaitEnv = cachedParafaitEnv;
            }
            else
            {
                ParafaitEnv = new ParafaitEnv(dbUtils);
            }
            
            MessageUtils = new MessageUtils(dbUtils, this.ParafaitEnv);
            EventLog = new EventLog(this);
            MachineLog = new MachineLog(this);
            log.LogMethodExit();
        }

        public enum RandomNumberType { Numeric, AlphaNumeric, AlphaNumericWithSpecialChars }


        public void Dispose()
        {
            log.LogMethodEntry();
            try
            {
                this.DBUtilities.Dispose();
            }
            catch (Exception ex)
            {
                log.Error("Error occured while disposing the DB utilities", ex);
            }

            if (sqlConnection != null)
            {
                try
                {
                    sqlConnection.Close();
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while closing the SQL connection", ex);
                }
            }
        }

        ~Utilities()
        {
            log.LogMethodEntry();
            Dispose();
            log.LogMethodExit(null);
        }

        public Utilities()
        {
            log.LogMethodEntry();
            this.DBUtilities = new DBUtils();
            init();
            log.LogMethodExit(null);
        }

        public Utilities(SqlConnection ParafaitSQLConnection)
        {
            log.LogMethodEntry(ParafaitSQLConnection);
            this.DBUtilities = new DBUtils(ParafaitSQLConnection);
            init();
            log.LogMethodExit(null);
        }

        public ExecutionContext ExecutionContext
        {
            get
            {
                return ParafaitEnv.ExecutionContext;
            }
        }

        void init()
        {
            log.LogMethodEntry();
            var dbUtils = this.DBUtilities;
            ParafaitEnv = new ParafaitEnv(dbUtils);
            MessageUtils = new MessageUtils(dbUtils, this.ParafaitEnv);

            EventLog = new EventLog(this);

            MachineLog = new MachineLog(this);

            log.LogMethodExit(null);
        }

        public SqlConnection createConnection()
        {
            log.LogMethodEntry();
            SqlConnection returnvalue = (this.DBUtilities.createConnection());
            log.LogMethodExit();
            return (returnvalue);
        }

        public SqlConnection createConnection(string conString)
        {
            log.LogMethodEntry(conString);
            SqlConnection returnvalue = (this.DBUtilities.createConnection(conString));
            log.LogMethodExit();
            return returnvalue;
        }

        public SqlConnection getConnection()
        {
            log.LogMethodEntry();
            SqlConnection returnvalue = (this.DBUtilities.getConnection());
            log.LogMethodExit();
            return returnvalue;
        }

        public SqlCommand getCommand()
        {
            log.LogMethodEntry();
            SqlCommand returnvalue = (this.DBUtilities.getCommand());
            log.LogMethodExit();
            return (returnvalue);
        }

        public SqlCommand getCommand(SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(SQLTrx);
            SqlCommand returnvalue = (this.DBUtilities.getCommand(SQLTrx));
            log.LogMethodExit();
            return (returnvalue);
        }

        public SqlCommand getCommand(SqlConnection cnn)
        {
            log.LogMethodEntry(cnn);
            SqlCommand returnvalue = (this.DBUtilities.getCommand(cnn));
            log.LogMethodExit();
            return (returnvalue);

        }

        public bool isNumber(string TextValue)
        {
            log.LogMethodEntry(TextValue);
            try
            {
                decimal val = Math.Round(Convert.ToDecimal(TextValue), 2);
                if (String.IsNullOrEmpty(TextValue))
                {
                    log.LogMethodExit(false);
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while performing mathematical function such as round", ex);
                log.LogMethodExit(false);
                return false;
            }
            log.LogMethodExit(true);
            return true;
        }

        public bool isNumberPositive(string TextValue)
        {
            log.LogMethodEntry(TextValue);
            if (!isNumber(TextValue))
            {
                log.LogMethodExit(false);
                return false;
            }

            else
            {
                decimal val = Math.Round(Convert.ToDecimal(TextValue), 2);
                if (val < 0)
                {
                    log.LogMethodExit(false);
                    return false;
                }
            }
            log.LogMethodExit(true);
            return true;
        }

        public bool isNumberZero(string TextValue)
        {
            log.LogMethodEntry(TextValue);
            if (!isNumber(TextValue))
            {
                log.LogMethodExit(false);
                return false;
            }

            else
            {
                decimal val = Math.Round(Convert.ToDecimal(TextValue), 2);
                if (val != 0)
                {
                    log.LogMethodExit(false);
                    return false;
                }
            }
            log.LogMethodExit(true);
            return true;
        }

        public string getDateTimeFormat()
        {
            log.LogMethodEntry();
            log.LogMethodExit(ParafaitEnv.DATETIME_FORMAT);
            return (ParafaitEnv.DATETIME_FORMAT);
        }

        public string getDateFormat()
        {
            log.LogMethodEntry();
            log.LogMethodExit(ParafaitEnv.DATE_FORMAT);
            return (ParafaitEnv.DATE_FORMAT);
        }

        public string getAmountFormat()
        {
            log.LogMethodEntry();
            log.LogMethodExit(ParafaitEnv.AMOUNT_FORMAT);
            return (ParafaitEnv.AMOUNT_FORMAT);
        }

        public string getNumberFormat()
        {
            log.LogMethodEntry();
            log.LogMethodExit(ParafaitEnv.NUMBER_FORMAT);
            return (ParafaitEnv.NUMBER_FORMAT);
        }

        public DataGridViewCellStyle gridViewAmountWithCurSymbolCellStyle()
        {
            log.LogMethodEntry();
            DataGridViewCellStyle style = new DataGridViewCellStyle();

            style.Alignment = DataGridViewContentAlignment.MiddleRight;

            style.Format = ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL;
            log.LogMethodExit(style);

            return style;
        }

        public DataGridViewCellStyle gridViewAmountCellStyle()
        {
            log.LogMethodEntry();
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            style.Alignment = DataGridViewContentAlignment.MiddleRight;
            style.Format = ParafaitEnv.AMOUNT_FORMAT;
            log.LogMethodExit(style);
            return style;
        }

        public DataGridViewCellStyle gridViewNumericCellStyle()
        {
            log.LogMethodEntry();
            DataGridViewCellStyle style = new DataGridViewCellStyle();

            style.Alignment = DataGridViewContentAlignment.MiddleRight;
            style.Format = ParafaitEnv.NUMBER_FORMAT;
            log.LogMethodExit(style);
            return style;
        }

        public DataGridViewCellStyle gridViewTextCellStyle()
        {
            log.LogMethodEntry();
            DataGridViewCellStyle style = new DataGridViewCellStyle();

            style.Font = getGridFont();
            style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            log.LogMethodExit(style);
            return style;
        }

        public DataGridViewCellStyle gridViewDateTimeCellStyle()
        {
            log.LogMethodEntry();
            DataGridViewCellStyle style = new DataGridViewCellStyle();

            style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            style.Format = getDateTimeFormat();
            log.LogMethodExit(style);
            return style;
        }

        public DataGridViewCellStyle gridViewDateCellStyle()
        {
            log.LogMethodEntry();
            DataGridViewCellStyle style = new DataGridViewCellStyle();

            style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            style.Format = getDateFormat();
            log.LogMethodExit(style);
            return style;
        }

        public DataGridViewCellStyle gridViewAlternateRowStyle()
        {
            log.LogMethodEntry();
            DataGridViewCellStyle style = new DataGridViewCellStyle();

            style.BackColor = System.Drawing.Color.Azure;
            log.LogMethodExit(style);
            return style;
        }

        public DataGridViewCellStyle gridViewCustomColorRowStyle(System.Drawing.Color colorName)
        {
            log.LogMethodEntry();
            DataGridViewCellStyle style = new DataGridViewCellStyle();

            style.BackColor = colorName;
            log.LogMethodExit(style);
            return style;
        }

        public System.Drawing.Font getFont()
        {
            log.LogMethodEntry();
            System.Drawing.Font font;
            try
            {
                font = new System.Drawing.Font(ParafaitEnv.DEFAULT_FONT, ParafaitEnv.DEFAULT_FONT_SIZE, FontStyle.Regular);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while applying new font", ex);
                font = new System.Drawing.Font("Arial", 10, FontStyle.Regular);
            }
            log.LogMethodExit(font);
            return font;
        }

        public System.Drawing.Font getGridFont()
        {
            log.LogMethodEntry();
            System.Drawing.Font font;
            try
            {
                font = new System.Drawing.Font(ParafaitEnv.DEFAULT_GRID_FONT, ParafaitEnv.DEFAULT_GRID_FONT_SIZE, FontStyle.Regular);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while applying new font", ex);
                font = new System.Drawing.Font("Tahoma", 8, FontStyle.Regular);
            }
            log.LogMethodExit(font);
            return font;
        }
        /// <summary>
        /// Gets the next business day based on passed the passed datetime 
        /// </summary>
        /// <param name="datetime">Datetime to check next business day </param>
        /// <returns>Next business datetime for the passed datetime</returns>
        public DateTime GetNextBusinessDay(DateTime datetime)
        {
            DateTime nextBusinessDay;
            log.LogMethodEntry(datetime);
            int businessStartTime = Convert.ToInt32(getParafaitDefaults("BUSINESS_DAY_START_TIME"));
            nextBusinessDay = datetime.Date.AddHours(businessStartTime);
            if(nextBusinessDay.CompareTo(datetime) <= 0)
            {
                nextBusinessDay = nextBusinessDay.AddDays(1);
            }            
            log.LogMethodExit(nextBusinessDay);
            return nextBusinessDay;
        }

        public System.Drawing.Color getPOSBackgroundColor()
        {
            try
            {
                if (string.IsNullOrEmpty(ParafaitEnv.POS_SKIN_COLOR.Trim()))
                {
                    log.LogMethodExit(Color.Gray);
                    return Color.Gray;
                }

                else
                {
                    log.LogMethodExit(ColorTranslator.FromHtml(ParafaitEnv.POS_SKIN_COLOR));
                    return ColorTranslator.FromHtml(ParafaitEnv.POS_SKIN_COLOR);
                }

            }
            catch (Exception ex)
            {
                log.Error("Error occured while applying color", ex);
            }
            log.LogMethodExit(Color.Gray);
            return Color.Gray;
        }

        public DataGridViewCellStyle getColumnHeaderStyle()
        {
            log.LogMethodEntry();
            DataGridViewCellStyle style = new DataGridViewCellStyle();

            style.BackColor = System.Drawing.Color.LightBlue;
            style.ForeColor = Color.Black;
            style.SelectionBackColor = System.Drawing.Color.LightSteelBlue;
            style.Font = getGridFont();
            style.Font = new System.Drawing.Font(style.Font.FontFamily, style.Font.Size, System.Drawing.FontStyle.Bold);
            style.WrapMode = DataGridViewTriState.True;
            style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            log.LogMethodExit(style);
            return style;
        }

        public void setupDataGridProperties(ref DataGridView dgv)
        {
            log.LogMethodEntry(dgv);
            setupDataGridProperties(ref dgv, false);
            //dgv.EnableHeadersVisualStyles = false;
            //dgv.EditMode = DataGridViewEditMode.EditOnEnter;
            //dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            //dgv.BackgroundColor = getPOSBackgroundColor();
            //dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            //dgv.BorderStyle = BorderStyle.None;
            //dgv.AllowUserToOrderColumns = true;
            //dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            //dgv.AlternatingRowsDefaultCellStyle = gridViewAlternateRowStyle();
            //dgv.RowsDefaultCellStyle.SelectionBackColor = Color.LightSteelBlue;
            //dgv.RowsDefaultCellStyle.SelectionForeColor = Color.Black;
            //dgv.ColumnHeadersDefaultCellStyle = getColumnHeaderStyle();
            //dgv.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            //dgv.DefaultCellStyle = gridViewTextCellStyle();

            //System.Globalization.CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            //System.Globalization.TextInfo textInfo = cultureInfo.TextInfo;

            //foreach (DataGridViewColumn dc in dgv.Columns)
            //{
            //    if (dc.Visible && dc.HeaderText == dc.DataPropertyName)
            //    {
            //        if (dc.HeaderText.Contains("_"))
            //        {
            //            dc.HeaderText = dc.HeaderText.Replace('_', ' ');
            //            dc.HeaderText = textInfo.ToTitleCase(dc.HeaderText);
            //        }
            //        else if (dc.HeaderText.Length > 1)
            //            dc.HeaderText = char.ToUpper(dc.HeaderText[0]) + dc.HeaderText.Substring(1);
            //    }
            //}

            //if (!dgv.AllowUserToDeleteRows)
            //    dgv.RowHeadersVisible = false;
            //else
            //    dgv.RowHeadersVisible = true;

            //dgv.GridColor = Color.LightSteelBlue;

            //try
            //{
            //    dgv.BackgroundColor = dgv.Parent.BackColor;
            //}
            //catch (Exception ex)
            //{
            //    log.Error("Error occured while applying dgv.background color", ex);
            //}
            //log.LogVariableState("DGV", dgv);
            log.LogMethodExit(null);
        }

        public void setupDataGridProperties(ref DataGridView dgv, bool ignoreCellStyling = false)
        {
            log.LogMethodEntry(dgv);
            dgv.EnableHeadersVisualStyles = false;
            dgv.EditMode = DataGridViewEditMode.EditOnEnter;
            if (!ignoreCellStyling)
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgv.BackgroundColor = getPOSBackgroundColor();
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.BorderStyle = BorderStyle.None;
            dgv.AllowUserToOrderColumns = true;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgv.AlternatingRowsDefaultCellStyle = gridViewAlternateRowStyle();
            dgv.RowsDefaultCellStyle.SelectionBackColor = Color.LightSteelBlue;
            dgv.RowsDefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.ColumnHeadersDefaultCellStyle = getColumnHeaderStyle();
            dgv.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            if (!ignoreCellStyling)
                dgv.DefaultCellStyle = gridViewTextCellStyle();

            System.Globalization.CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Globalization.TextInfo textInfo = cultureInfo.TextInfo;

            foreach (DataGridViewColumn dc in dgv.Columns)
            {
                if (dc.Visible && dc.HeaderText == dc.DataPropertyName)
                {
                    if (dc.HeaderText.Contains("_"))
                    {
                        dc.HeaderText = dc.HeaderText.Replace('_', ' ');
                        dc.HeaderText = textInfo.ToTitleCase(dc.HeaderText);
                    }
                    else if (dc.HeaderText.Length > 1)
                        dc.HeaderText = char.ToUpper(dc.HeaderText[0]) + dc.HeaderText.Substring(1);
                }
            }

            if (!dgv.AllowUserToDeleteRows)
                dgv.RowHeadersVisible = false;
            else
                dgv.RowHeadersVisible = true;

            dgv.GridColor = Color.LightSteelBlue;

            try
            {
                dgv.BackgroundColor = dgv.Parent.BackColor;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while applying dgv.background color", ex);
            }
            log.LogVariableState("DGV", dgv);
            log.LogMethodExit(null);
        }

        public string getParafaitDefaults(string default_value_name)
        {
            log.LogMethodEntry(default_value_name);
            string result = ParafaitEnv.getParafaitDefaults(default_value_name);
            log.LogMethodExit(result);
            return result;
        }

        //Added method 24-Oct-2016
        public DataTable getReportGridTable(DataTable dt, int breakColumn, int[] aggrColumnList)
        {
            log.LogMethodEntry(dt, breakColumn, aggrColumnList);
            DataTable returnvalue = (getReportGridTable(dt, breakColumn, aggrColumnList, false));
            log.LogMethodExit(returnvalue);
            return (returnvalue);
        }

        //Added overload method 9-Dec-2016
        public DataTable getReportGridTable(DataTable dt, int breakColumn, int[] aggrColumnList, bool HideBreakColumn)
        {
            log.LogMethodEntry(dt, breakColumn, aggrColumnList, HideBreakColumn);
            DataTable returnvalue = (getReportGridTable(dt, breakColumn, aggrColumnList, false, true));
            log.LogMethodExit(returnvalue);
            return (returnvalue);
        }

        //Updated signature to add parameter showGrandTotal 9-Dec-2016
        //Updated signature to add parameter HideBreakColumn 12-Oct-2016
        public DataTable getReportGridTable(DataTable dt, int breakColumn, int[] aggrColumnList, bool HideBreakColumn, bool showGrandTotal)
        {
            log.LogMethodEntry(dt, breakColumn, aggrColumnList, HideBreakColumn, showGrandTotal);
            string prevBreakValue = "";
            DataTable newDT = new DataTable();
            object[] aggrColumnValues = new object[dt.Columns.Count];
            object[] GrandaggrColumnValues = new object[dt.Columns.Count];
            object[] datarow = new object[dt.Columns.Count];
            foreach (DataColumn dc in dt.Columns)
            {
                newDT.Columns.Add(dc.ColumnName, dc.DataType);
            }
            for (int rowindex = 0; rowindex < dt.Rows.Count; rowindex++)
            {
                if (dt.Rows[rowindex][breakColumn].ToString() != prevBreakValue && rowindex != 0) // sub-total row for break column
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        datarow[i] = null;
                        for (int j = 0; j < aggrColumnList.Length; j++)
                        {
                            if (aggrColumnList[j] == i)
                            {
                                datarow[i] = aggrColumnValues[i];
                            }
                        }
                    }

                    if (newDT.Columns[breakColumn].DataType.Name == "String")
                        datarow[breakColumn] = prevBreakValue + " " + MessageUtils.getMessage("Total");
                    else
                        datarow[breakColumn] = prevBreakValue;

                    newDT.Rows.Add(datarow);

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        datarow[i] = null;
                    }
                    newDT.Rows.Add(datarow);
                }

                if (dt.Rows[rowindex][breakColumn].ToString() != prevBreakValue) // header row for break column
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        datarow[i] = null;
                    }

                    for (int i = 0; i < aggrColumnValues.Length; i++)
                        aggrColumnValues[i] = 0;

                    datarow[breakColumn] = dt.Rows[rowindex][breakColumn].ToString();
                    newDT.Rows.Add(datarow);
                }

                for (int i = 0; i < dt.Columns.Count; i++) // each row
                {
                    datarow[i] = dt.Rows[rowindex][i];
                    //Start update 12-Oct-2016
                    //Added code to hide break column value in each row
                    string type = dt.Columns[i].DataType.ToString().ToLower();
                    if (i == breakColumn)
                        datarow[breakColumn] = null;
                    //End update 12-Oct-2016
                    for (int j = 0; j < aggrColumnList.Length; j++)
                    {
                        if (aggrColumnList[j] == i)
                        {
                            double val;
                            if (dt.Rows[rowindex][i] == DBNull.Value)
                                val = 0;
                            else
                                val = Convert.ToDouble(dt.Rows[rowindex][i]);
                            aggrColumnValues[i] = Convert.ToDouble(aggrColumnValues[i]) + val;
                            GrandaggrColumnValues[i] = Convert.ToDouble(GrandaggrColumnValues[i]) + val;
                        }
                    }
                }
                newDT.Rows.Add(datarow);
                prevBreakValue = dt.Rows[rowindex][breakColumn].ToString();

                if (rowindex == dt.Rows.Count - 1) // last row. print sub total
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        datarow[i] = null;
                        for (int j = 0; j < aggrColumnList.Length; j++)
                        {
                            if (aggrColumnList[j] == i)
                            {
                                datarow[i] = aggrColumnValues[i];
                            }
                        }
                    }

                    if (newDT.Columns[breakColumn].DataType.Name == "String")
                        datarow[breakColumn] = prevBreakValue + " " + MessageUtils.getMessage("Total");
                    else
                        datarow[breakColumn] = prevBreakValue;
                    newDT.Rows.Add(datarow);

                    if (showGrandTotal) //Added condition 9-Dec-2016
                    {
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            datarow[i] = null;
                        }
                        newDT.Rows.Add(datarow);

                        // Grand Total
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            datarow[i] = null;
                            for (int j = 0; j < aggrColumnList.Length; j++)
                            {
                                if (aggrColumnList[j] == i)
                                {
                                    datarow[i] = GrandaggrColumnValues[i];
                                }
                            }
                        }

                        if (newDT.Columns[breakColumn].DataType.Name == "String")
                            datarow[breakColumn] = MessageUtils.getMessage("Grand Total");
                        newDT.Rows.Add(datarow);
                    }
                }
            }
            log.LogMethodExit(newDT);
            return newDT;
        }

        public void houseKeeping()
        {
            log.LogMethodEntry();
            //System.Threading.Thread thr = new System.Threading.Thread(new System.Threading.ThreadStart(VersionSynch.SynchVersions));
            //thr.Start();
            log.LogMethodExit(null);
        }

        public void CreateExcelFromGrid(DataGridView dgvReportData, string fileName, string Heading = null)
        {
            log.LogMethodEntry(dgvReportData, fileName, Heading);
            CreateExcelFromGrid(dgvReportData, fileName, Heading, null, DateTime.MinValue, DateTime.MinValue);
            log.LogMethodExit(null);
        }

        public void CreateExcelFromGrid(DataGridView dgvReportData, string fileName,
                                        string Heading, string site_name,
                                        DateTime from_date, DateTime to_date, bool hideAlteranateRow = true,
                                        bool BGMode = false, params string[] otherParams)
        {
            log.LogMethodEntry(dgvReportData, fileName, Heading, site_name, from_date, to_date, BGMode, otherParams);
            List<DataGridView> dgv = new List<DataGridView>();
            dgv.Add(dgvReportData);
            
            CreateExcelFromGrid(dgv, fileName,
                                Heading, site_name,
                                from_date, to_date,
                                BGMode, hideAlteranateRow , otherParams);
            log.LogMethodExit(null);
        }

        public void CreateExcelFromGrid(List<DataGridView> dgvList, string fileName,
                                        string Heading, string site_name,
                                        DateTime from_date, DateTime to_date,
                                        bool BGMode = false, bool hideAlteranateRow = true , params string[] otherParams)
        {
            log.LogMethodEntry(dgvList, fileName, Heading, site_name, from_date, to_date, BGMode, otherParams);
            var excelApp = new Excel.Application();
            try
            {
                excelApp.Visible = false;
                Excel.Workbook excelbk = excelApp.Workbooks.Add(Type.Missing);

                while (excelbk.Worksheets.Count > 1)
                    (excelbk.Worksheets[1] as Excel.Worksheet).Delete();

                if (dgvList.Count > 1)
                    excelbk.Worksheets.Add(Type.Missing, Type.Missing, dgvList.Count - 1, Type.Missing);

                int sheetIndex = 1;
                foreach (DataGridView dgvReportData in dgvList)
                {
                    Excel.Worksheet xlWorkSheet1 = excelbk.Worksheets[sheetIndex++] as Excel.Worksheet;
                    xlWorkSheet1.PageSetup.Orientation = Excel.XlPageOrientation.xlLandscape;
                    string lclHeading = Heading;
                    if (string.IsNullOrEmpty(lclHeading) == true)
                        lclHeading = dgvReportData.Name;
                    if (string.IsNullOrEmpty(lclHeading) == false)
                        xlWorkSheet1.Name = (lclHeading.Length > 30 ? lclHeading.Substring(0, 30) : lclHeading).Replace("/", "-");

                    System.Drawing.Font font = dgvReportData.DefaultCellStyle.Font;
                    if (font == null)
                        font = new System.Drawing.Font("Tahoma", 8);

                    excelApp.StandardFont = font.Name;
                    excelApp.StandardFontSize = font.Size;

                    int WORKSHEETSTARTROW = 1, WORKSHEETSTARTCOL = 1;
                    if (string.IsNullOrEmpty(lclHeading) == false)
                    {
                        Excel.Range xlHeading = xlWorkSheet1.Cells[WORKSHEETSTARTROW, 3] as Excel.Range;
                        xlHeading.Value = lclHeading;
                        xlHeading.Font.Bold = true;
                        xlHeading.Font.Name = font.Name;
                        xlHeading.Font.Size = 10;
                        WORKSHEETSTARTROW++;
                    }

                    if (string.IsNullOrEmpty(site_name) == false)
                    {
                        Excel.Range xllblSite = xlWorkSheet1.Cells[WORKSHEETSTARTROW, 2] as Excel.Range;
                        xllblSite.Value = MessageUtils.getMessage("Site") + ":";
                        xllblSite.Font.Bold = true;
                        xllblSite.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;

                        Excel.Range xlSite = xlWorkSheet1.Cells[WORKSHEETSTARTROW, 3] as Excel.Range;
                        xlSite.Value = site_name;
                        xlSite.Font.Bold = true;
                        WORKSHEETSTARTROW++;

                        Excel.Range xllblFromDate = xlWorkSheet1.Cells[WORKSHEETSTARTROW, 2] as Excel.Range;
                        xllblFromDate.Value = MessageUtils.getMessage("From") + ":";
                        xllblFromDate.Font.Bold = true;
                        xllblFromDate.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;

                        Excel.Range xlFromDate = xlWorkSheet1.Cells[WORKSHEETSTARTROW, 3] as Excel.Range;
                        xlFromDate.Value = from_date.ToString("ddd, dd-MMM-yyyy h:mm tt");
                        xlFromDate.Font.Bold = true;
                        xlFromDate.Font.Name = font.Name;
                        xlFromDate.Font.Size = 8;
                        WORKSHEETSTARTROW++;

                        Excel.Range xllbltoDate = xlWorkSheet1.Cells[WORKSHEETSTARTROW, 2] as Excel.Range;
                        xllbltoDate.Value = MessageUtils.getMessage("To") + ":";
                        xllbltoDate.Font.Bold = true;
                        xllbltoDate.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;

                        Excel.Range xltoDate = xlWorkSheet1.Cells[WORKSHEETSTARTROW, 3] as Excel.Range;
                        xltoDate.Value = to_date.ToString("ddd, dd-MMM-yyyy h:mm tt");
                        xltoDate.Font.Bold = true;
                        WORKSHEETSTARTROW++;

                        //Start update 12-Oct-2016
                        //Added to see that other parameter values are shown in excel
                        if (otherParams.Length > 0)
                        {
                            for (int k = 0; k < otherParams.Length; k++)
                            {
                                if (otherParams[k] == null)
                                    continue;

                                Excel.Range xllblotherParam = xlWorkSheet1.Cells[WORKSHEETSTARTROW, 2] as Excel.Range;
                                xllblotherParam.Value = MessageUtils.getMessage(otherParams[k].Split(':')[0]) + ":";
                                xllblotherParam.Font.Bold = true;
                                xllblotherParam.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;

                                Excel.Range xlotherParam = xlWorkSheet1.Cells[WORKSHEETSTARTROW, 3] as Excel.Range;
                                xlotherParam.Value = otherParams[k].Split(':')[1];
                                xlotherParam.Font.Bold = true;
                                WORKSHEETSTARTROW++;
                            }
                        }
                        //End update 12-Oct-2016

                        Excel.Range xllblRunAt = xlWorkSheet1.Cells[WORKSHEETSTARTROW, 2] as Excel.Range;
                        xllblRunAt.Value = MessageUtils.getMessage("Run At") + ":";
                        xllblRunAt.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;

                        Excel.Range xlRunAt = xlWorkSheet1.Cells[WORKSHEETSTARTROW, 3] as Excel.Range;
                        xlRunAt.Value = DateTime.Now.ToString("ddd, dd-MMM-yyyy h:mm tt");
                        xlRunAt.Font.Name = font.Name;
                        xlRunAt.Font.Size = 8;
                        WORKSHEETSTARTROW++;

                        Excel.Range xllblRunBy = xlWorkSheet1.Cells[WORKSHEETSTARTROW, 2] as Excel.Range;
                        xllblRunBy.Value = MessageUtils.getMessage("Run By") + ":";
                        xllblRunBy.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;

                        Excel.Range xlRunBy = xlWorkSheet1.Cells[WORKSHEETSTARTROW, 3] as Excel.Range;
                        xlRunBy.Value = ParafaitEnv.Username;
                        WORKSHEETSTARTROW++;
                    }

                    //if (otherParams != null)
                    //{
                    //    int rows = otherParam_cnt;
                    //    for (int i = 6; i <= 5 + otherParam_cnt; i++)
                    //    {
                    //        Excel.Range xlRange = (Excel.Range)xlWorkSheet1.Cells[i, 1];
                    //        xlRange.Value2 = dgvReportData.Rows[i].Cells[1].Value.ToString();
                    //    }
                    //}

                    if (string.IsNullOrEmpty(lclHeading) && string.IsNullOrEmpty(site_name))
                        WORKSHEETSTARTROW = 1;
                    else
                        WORKSHEETSTARTROW++;

                    int colNum = 1;
                    int worksheetRow = WORKSHEETSTARTROW;

                    if (dgvReportData.ColumnHeadersDefaultCellStyle.Font != null)
                        font = dgvReportData.ColumnHeadersDefaultCellStyle.Font;

                    //Copy column headers
                    for (int i = 0; i < dgvReportData.Columns.Count; i++)
                    {
                        if (dgvReportData.Columns[i].Visible == false || dgvReportData.Columns[i].GetType() == typeof(DataGridViewImageColumn))
                            continue;
                        Excel.Range xlRange = xlWorkSheet1.Cells[worksheetRow, colNum] as Excel.Range;
                        xlRange.Value2 = dgvReportData.Columns[i].HeaderText;
                        xlRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(dgvReportData.ColumnHeadersDefaultCellStyle.BackColor);
                        xlRange.Borders.Color = System.Drawing.ColorTranslator.ToOle(Color.LightSteelBlue);

                        colNum += 1;
                    }

                    colNum--;
                    string colStart = Char.ConvertFromUtf32('A' + WORKSHEETSTARTCOL - 1) + (worksheetRow).ToString();
                    string colEnd = GetExcelColumnName(colNum - 1) + (worksheetRow).ToString();  //Updated 20-Jun-2017
                    Excel.Range xlHRange = xlWorkSheet1.get_Range(colStart, colEnd);
                    xlHRange.Font.Size = font.Size;
                    xlHRange.Font.Bold = font.Bold;
                    xlHRange.Font.Italic = font.Italic;
                    xlHRange.Font.Underline = font.Underline;
                    xlHRange.Font.Name = font.Name;

                    colEnd = GetExcelColumnName(colNum - 1) + (worksheetRow + dgvReportData.Rows.Count).ToString(); //Updated 20-Jun-2017
                    Excel.Range xlDataRange = xlWorkSheet1.get_Range(colStart, colEnd);
                    xlDataRange.Borders.LineStyle = DataGridViewCellBorderStyle.Single;
                    xlDataRange.Borders.Color = System.Drawing.ColorTranslator.ToOle(Color.LightSteelBlue);

                    worksheetRow = WORKSHEETSTARTROW + 1;
                    //Copy data rows
                    for (int rowCount = 0; rowCount < dgvReportData.Rows.Count; rowCount++)
                    {
                        colStart = Char.ConvertFromUtf32('A' + WORKSHEETSTARTCOL - 1) + worksheetRow.ToString();
                        colEnd = GetExcelColumnName(colNum - 1) + worksheetRow.ToString(); //Updated 20-Jun-2017
                        Excel.Range xlRowRange = xlWorkSheet1.get_Range(colStart, colEnd);

                        if (hideAlteranateRow && rowCount % 2 != 0)
                            xlRowRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(dgvReportData.AlternatingRowsDefaultCellStyle.BackColor);

                        int worksheetcol = WORKSHEETSTARTCOL;
                        for (int colCount = 0; colCount < dgvReportData.Columns.Count; colCount++)
                        {
                            if (dgvReportData.Columns[colCount].Visible == false || dgvReportData.Columns[colCount].GetType() == typeof(DataGridViewImageColumn))
                                continue;

                            Excel.Range xlRange = xlWorkSheet1.Cells[worksheetRow, worksheetcol] as Excel.Range;
                            xlRange.Value2 = dgvReportData.Rows[rowCount].Cells[colCount].FormattedValue;
                            //xlRange.Font.Color = dgvReportData.Rows[rowCount].Cells[colCount].Style.ForeColor.ToArgb();

                            if (dgvReportData.Rows[rowCount].Cells[colCount].Style.Font != null)
                                font = dgvReportData.Rows[rowCount].Cells[colCount].Style.Font;
                            else if (dgvReportData.Rows[rowCount].DefaultCellStyle.Font != null)
                                font = dgvReportData.Rows[rowCount].DefaultCellStyle.Font;
                            else
                                font = dgvReportData.DefaultCellStyle.Font;

                            if (font != null && font.Bold)
                                xlRange.Font.Bold = true;

                            worksheetcol += 1;
                        }
                        worksheetRow += 1;
                    }

                    xlDataRange.Columns.AutoFit();
                }

                excelApp.DisplayAlerts = false;
                excelbk.SaveAs(fileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Microsoft.Office.Interop.Excel.XlSaveConflictResolution.xlLocalSessionChanges, Type.Missing, Type.Missing, Type.Missing);
                excelApp.Visible = true;
            }
            catch (Exception ex)
            {
                log.Error("Error occured in create excel form grid", ex);
                if (BGMode == false)
                    MessageBox.Show(ex.Message);
                throw;
            }
            finally
            {
                if (BGMode)
                    excelApp.Quit();
            }
            log.LogMethodExit(null);
        }

        //Start update 20-Jun-2017
        //Added method to get the end column name
        public static string GetExcelColumnName(int columnIndex)
        {
            log.LogMethodEntry(columnIndex);
            //  eg  (0) should return "A"
            //      (1) should return "B"
            //      (25) should return "Z"
            //      (26) should return "AA"
            //      (27) should return "AB"
            //      ..etc..
            char firstChar;
            char secondChar;
            char thirdChar;

            if (columnIndex < 26)
            {
                string returnvalue = (((char)('A' + columnIndex)).ToString());
                log.LogMethodExit(returnvalue);
                return returnvalue;
            }

            if (columnIndex < 702)
            {
                firstChar = (char)('A' + (columnIndex / 26) - 1);
                secondChar = (char)('A' + (columnIndex % 26));
                string returnvalue1 = (string.Format("{0}{1}", firstChar, secondChar));
                log.LogMethodExit(returnvalue1);
                return (returnvalue1);
            }

            int firstInt = columnIndex / 26 / 26;
            int secondInt = (columnIndex - firstInt * 26 * 26) / 26;
            if (secondInt == 0)
            {
                secondInt = 26;
                firstInt = firstInt - 1;
            }
            int thirdInt = (columnIndex - firstInt * 26 * 26 - secondInt * 26);

            firstChar = (char)('A' + firstInt - 1);
            secondChar = (char)('A' + secondInt - 1);
            thirdChar = (char)('A' + thirdInt);
            string returnvalue2 = (string.Format("{0}{1}{2}", firstChar, secondChar, thirdChar));
            log.LogMethodExit(returnvalue2);
            return returnvalue2;
        }
        //End update 20-Jun-2017

        public void ExportToExcel(DataGridView dgv, string fileName, string Heading = null)
        {
            log.LogMethodEntry(dgv, fileName, Heading);
            ExportToExcel(dgv, fileName, Heading, null, DateTime.MinValue, DateTime.MinValue);
        }

        public void ExportToExcel(DataGridView dgv, string fileName,
                                string Heading, string site_name,
                                DateTime from_date, DateTime to_date,
                                params string[] otherParams)
        {
            log.LogMethodEntry(dgv, fileName, Heading, site_name, from_date, to_date, otherParams);
            String file_path = "";
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.FileName = fileName;
            saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog1.Filter = "Excel Files (*.xls)|*.xls|(*.xlsx)|*.xlsx";
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream stream = null;
                try
                {
                    file_path = saveFileDialog1.FileName;
                    if (System.IO.File.Exists(file_path))
                        stream = new System.IO.FileInfo(file_path).Open(System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None);
                }
                catch (Exception ex)
                {
                    log.Error("Error with file. Detailed error", ex);
                    MessageBox.Show("Error with file. Detailed error: " + ex.Message);
                    log.LogMethodExit(null);
                    return;
                }
                finally
                {
                    if (stream != null)
                        stream.Close();
                }

                Application.DoEvents();

                ApplicationContext ap = new ApplicationContext();
                System.Threading.ThreadStart thr = delegate
                {
                    Form f = new Form();
                    System.Windows.Forms.Button btnWait;
                    btnWait = new System.Windows.Forms.Button();
                    // 
                    // btnWait
                    // 
                    btnWait.BackColor = System.Drawing.Color.Transparent;
                    btnWait.BackgroundImage = Semnox.Core.Utilities.Properties.Resources.pressed1;
                    btnWait.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                    btnWait.Dock = System.Windows.Forms.DockStyle.Fill;
                    btnWait.FlatAppearance.BorderSize = 0;
                    btnWait.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
                    btnWait.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
                    btnWait.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    btnWait.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    btnWait.ForeColor = System.Drawing.Color.White;
                    btnWait.Image = Properties.Resources.PreLoader;
                    btnWait.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
                    btnWait.Location = new System.Drawing.Point(0, 0);
                    btnWait.Name = "btnWait";
                    btnWait.Size = new System.Drawing.Size(346, 89);
                    btnWait.TabIndex = 2;
                    btnWait.Text = MessageUtils.getMessage(683); //Please wait while Excel opens your file
                    btnWait.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                    btnWait.UseVisualStyleBackColor = false;
                    // 
                    // Form
                    // 
                    f.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                    f.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                    f.BackColor = System.Drawing.Color.White;
                    f.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                    f.ClientSize = new System.Drawing.Size(346, 89);
                    f.Controls.Add(btnWait);
                    f.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                    f.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                    f.TransparencyKey = System.Drawing.Color.White;

                    f.ShowInTaskbar = false;
                    f.TopLevel = f.TopMost = true;

                    try
                    {
                        ap.MainForm = f;
                        Application.Run(ap);
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error occured exporting to excel", ex);
                    }
                };
                System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(thr));
                thread.Start();

                try
                {
                    //Updated method call to include otherparams 12-Oct-2016
                    CreateExcelFromGrid(dgv, file_path, Heading, site_name, from_date, to_date,true, false, otherParams);
                }
                catch (Exception ex)
                {
                    log.Error("Error occured in create_excel_Form_grid", ex);
                }
                finally
                {
                    ap.ExitThread();
                }
            }
            log.LogMethodExit(null);
        }

        public void ExportToExcelTabDelimited(string FileName, DataTable dt)
        {
            log.LogMethodEntry(FileName, dt);
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            sfd.FileName = FileName;
            sfd.Filter = ".xls | *.xls";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    System.IO.FileStream fs = System.IO.File.Open(sfd.FileName, System.IO.FileMode.Create);
                    fs.Close();
                    System.IO.StreamWriter streamWriter = System.IO.File.AppendText(sfd.FileName);
                    foreach (DataColumn dc in dt.Columns)
                        streamWriter.Write(dc.ColumnName + '\t');

                    streamWriter.WriteLine();

                    foreach (DataRow row in dt.Rows)
                    {
                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (dc.DataType.ToString().Contains("Date") && row[dc] != DBNull.Value)
                                streamWriter.Write(Convert.ToDateTime(row[dc]).ToString("dd-MMM-yyyy h:mm:ss tt") + '\t');
                            else if(dc.ColumnName == "PlanDate" && row[dc] != DBNull.Value)
                            {
                                streamWriter.Write(Convert.ToDateTime(row[dc]).ToString("dd-MMM-yyyy") + '\t');
                            }
                            else if (dc.DataType.ToString().Contains("String"))
                                streamWriter.Write(row[dc].ToString() + '\t');
                            else
                                streamWriter.Write(row[dc].ToString() + '\t');
                        }
                        streamWriter.WriteLine();
                    }

                    streamWriter.Close();
                    System.Diagnostics.Process.Start("excel", "\"" + sfd.FileName + "\"");
                }
                catch (Exception ex)
                {
                    log.Error("Error occured in Export To Excel Tab Delimited", ex);
                    MessageBox.Show(ex.Message);
                }
            }
            log.LogMethodExit(null);
        }

        public System.Drawing.Image ConvertToImage(object DBImage)
        {
            log.LogMethodEntry(DBImage);
            if (DBImage == DBNull.Value)
            {
                log.LogMethodExit(null);
                return null;
            }

            else
            {
                byte[] b = new byte[0];
                b = DBImage as byte[];
                System.IO.MemoryStream ms = new System.IO.MemoryStream(b);
                log.LogMethodExit(System.Drawing.Image.FromStream(ms));
                return System.Drawing.Image.FromStream(ms);
            }

        }

        public byte[] ConvertToByteArray(System.Drawing.Image image)
        {
            log.LogMethodEntry(image);
            byte[] returnvalue = (this.DBUtilities.ConvertToByteArray(image));

            log.LogMethodExit(returnvalue);
            return (returnvalue);
        }

        public string ProcessScannedBarCode(string Code, int leftTrim, int rightTrim)
        {
            log.LogMethodEntry(Code, leftTrim, rightTrim);
            try
            {
                Code = System.Text.RegularExpressions.Regex.Replace(Code, @"\W+", "");
                log.LogMethodExit((Code.Substring(leftTrim, Code.Length - rightTrim - leftTrim)));
                return (Code.Substring(leftTrim, Code.Length - rightTrim - leftTrim));
            }
            catch (Exception ex)
            {
                log.Error("Error occured in Processing Scanned Bar Code", ex);
                log.LogMethodExit(Code);
                return Code;
            }
        }

        public DateTime getServerTime()
        {
            log.LogMethodEntry();
            DateTime returnvalue = ((Convert.ToDateTime(executeScalar("select getdate()"))));
            log.LogMethodExit(returnvalue);
            return (returnvalue);
        }

        public object executeScalar(string command, SqlTransaction SQLTrx, params SqlParameter[] spc)
        {
            log.LogMethodEntry();
            object returnvalue = (this.DBUtilities.executeScalar(command, SQLTrx, spc));
            log.LogMethodExit(returnvalue);
            return (returnvalue);
        }

        public object executeScalar(string command, params SqlParameter[] spc)
        {
            log.LogMethodEntry(command, spc);
            object returnvalue = (this.DBUtilities.executeScalar(command, spc));
            log.LogMethodExit(returnvalue);
            return (returnvalue);
        }

        public int executeNonQuery(string command, SqlTransaction SQLTrx, params SqlParameter[] spc)
        {
            log.LogMethodEntry(command, SQLTrx, spc);
            int returnvalue = (this.DBUtilities.executeNonQuery(command, SQLTrx, spc));
            log.LogMethodExit(returnvalue);
            return (returnvalue);
        }

        public int executeNonQuery(string command, params SqlParameter[] spc)
        {
            log.LogMethodEntry(command, spc);
            int returnvalue = (this.DBUtilities.executeNonQuery(command, spc));
            log.LogMethodExit(returnvalue);
            return (returnvalue);
        }

        public DataTable executeDataTable(string command, SqlTransaction SQLTrx, params SqlParameter[] spc)
        {
            log.LogMethodEntry(command, SQLTrx, spc);
            DataTable returnvalue = (this.DBUtilities.executeDataTable(command, SQLTrx, spc));
            log.LogMethodExit(returnvalue);
            return returnvalue;
        }

        public DataTable executeDataTable(string command, params SqlParameter[] spc)
        {
            log.LogMethodEntry(command, spc);
            DataTable returnvalue = executeDataTable(command, 30, spc);
            log.LogMethodExit(returnvalue);
            return (returnvalue);
        }

        public DataTable executeDataTable(string command, int commandTimeout, params SqlParameter[] spc)
        {
            log.LogMethodEntry(command, spc);
            DataTable returnvalue = (this.DBUtilities.executeDataTable(command, null, commandTimeout, spc));
            log.LogMethodExit(returnvalue);
            return (returnvalue);
        }

        public void logSQLCommand(string Source, SqlCommand sqlCmd)
        {
            log.LogMethodEntry(Source, sqlCmd);
            string command = sqlCmd.CommandText;
            foreach (SqlParameter sp in sqlCmd.Parameters)
            {
                if (sp.Value == null || sp.Value.Equals(DBNull.Value))
                    command = command.Replace(sp.ParameterName, "NULL");
                else
                    command = command.Replace(sp.ParameterName, sp.Value.ToString());
            }
            EventLog.logEvent(Source, 'D', sqlCmd.CommandText, command, "SQLCOMMAND", 2);
            log.LogMethodExit(null);
        }

        //public string GenerateRandomCardNumber()
        //{
        //    log.LogMethodEntry();
        //    string validCharacters = "1234567890GHIJKLMNOPQRSUVWXYZ";
        //    //string returnvalue = (GenerateRandomCardNumber(ParafaitEnv.CARD_NUMBER_LENGTH));
        //    string returnvalue = (GenerateRandomNumber(ParafaitEnv.CARD_NUMBER_LENGTH, validCharacters));
        //    log.LogMethodExit(returnvalue);
        //    return (returnvalue);
        //}

        

        public int MifareCustomerKey = -1;
        public void getMifareCustomerKey()
        {
            log.LogMethodEntry();
            object customerKey = executeScalar(@"Select top 1 CustomerKey from site");
            if (customerKey != DBNull.Value)
            {
                try
                {
                    MifareCustomerKey = Convert.ToInt32(double.Parse(customerKey.ToString().Trim()));
                }
                catch (Exception ex)
                {
                    log.Error("Error occured in Mi Fare Customer Key", ex);
                    MifareCustomerKey = -1;
                }
            }

            if (MifareCustomerKey <= 0)
            {
                MessageBox.Show(MessageUtils.getMessage(291, customerKey));
            }
            log.LogMethodExit(null);
        }

        //public string GenerateRandomCardNumber(int Width)
        //{
        //    log.LogMethodEntry(Width);
        //    string validCharacters = "1234567890GHIJKLMNOPQRSUVWXYZ";
        //    var ret = "";

        //    byte[] seed = new byte[4];
        //    new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(seed);
        //    int seedInt = BitConverter.ToInt32(seed, 0);

        //    var rnd = new Random(seedInt);
        //    while (ret.Length < Width)
        //    {
        //        ret += validCharacters[rnd.Next(validCharacters.Length - 1)].ToString();
        //    }
        //    log.LogMethodExit(ret);
        //    return ret;
        //}

        public string GenerateRandomCardNumber(int width)
        {
            log.LogMethodEntry(width);
            string validCharacters = "1234567890GHIJKLMNOPQRSUVWXYZ";
            string returnvalue = (GenerateRandomNumber(width, validCharacters));
            log.LogMethodExit(returnvalue);
            return (returnvalue);
        }

        public void setLanguage(int pLanguageId)
        {
            log.LogMethodEntry(pLanguageId);
            ParafaitEnv.getLanguageSpecifics(pLanguageId);
            if (string.IsNullOrEmpty(ParafaitEnv.CultureCode) == false)
            {
                try
                {
                    var culture = new CultureInfo(ParafaitEnv.CultureCode);
                    System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
                }
                catch (Exception ex)
                {
                    log.Error("Error occured in setting the language", ex);
                }
            }
            log.LogMethodExit(null);
        }

        public void setLanguage()
        {
            log.LogMethodEntry();
            setLanguage(-9);
            log.LogMethodExit(null);
        }

        public void setLanguage(Control c)
        {
            log.LogMethodEntry(c);
            if (ParafaitEnv.LanguageId < 0)
            {
                log.LogMethodExit(null);
                return;
            }


            try
            {
                if (c.HasChildren)
                {
                    foreach (Control child in c.Controls)
                        setLanguage(child);
                }

                string type = c.GetType().ToString().ToLower();
                type = type.Replace("system.windows.forms.", "");
                if (type == "datagridview")
                {
                    DataGridView dgv = c as DataGridView;
                    foreach (DataGridViewColumn dc in dgv.Columns)
                    {
                        dc.HeaderText = MessageUtils.getMessage(dc.HeaderText);
                    }

                    try
                    {
                        if (ParafaitEnv.LanguageCode != null && ParafaitEnv.LanguageCode.StartsWith("en", StringComparison.CurrentCultureIgnoreCase) == false)
                        {
                            if (dgv.DefaultCellStyle != null && dgv.DefaultCellStyle.Font.Bold)
                            {
                                dgv.DefaultCellStyle.Font = new System.Drawing.Font(dgv.DefaultCellStyle.Font, System.Drawing.FontStyle.Regular);
                            }

                            if (dgv.ColumnHeadersDefaultCellStyle != null && dgv.ColumnHeadersDefaultCellStyle.Font.Bold)
                            {
                                dgv.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font(dgv.ColumnHeadersDefaultCellStyle.Font, System.Drawing.FontStyle.Regular);
                            }

                            foreach (DataGridViewColumn dc in dgv.Columns)
                            {
                                if (dc.DefaultCellStyle != null && dc.DefaultCellStyle.Font != null && dc.DefaultCellStyle.Font.Bold)
                                {
                                    dc.DefaultCellStyle.Font = new System.Drawing.Font(dc.DefaultCellStyle.Font, System.Drawing.FontStyle.Regular);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error occured in setting the language", ex);
                    }
                }
                else
                {
                    if (type != "textbox" && !string.IsNullOrEmpty(c.Text))
                    {
                        string trText = MessageUtils.getMessage(c.Text);
                        if (trText.Equals(c.Text) == false)
                            c.Text = trText;
                        else if (c.Text.EndsWith(":*")) // for labels
                        {
                            string trimEnd = c.Text.TrimEnd(':', '*');
                            trText = MessageUtils.getMessage(trimEnd);
                            if (trText.Equals(trimEnd) == false)
                                c.Text = trText + ":*";
                        }
                        else if (c.Text.EndsWith(":")) // for labels
                        {
                            string trimEnd = c.Text.TrimEnd(':');
                            trText = MessageUtils.getMessage(trimEnd);
                            if (trText.Equals(trimEnd) == false)
                                c.Text = trText + ":";
                        }
                    }

                    if (ParafaitEnv.LanguageCode != null && ParafaitEnv.LanguageCode.StartsWith("en", StringComparison.CurrentCultureIgnoreCase) == false && c.Font.Bold)
                    {
                        if (type == "textbox"
                            || type == "label"
                            || type == "button"
                            || type == "checkbox"
                            || type == "radiobutton")
                        {
                            c.Font = new System.Drawing.Font(c.Font, System.Drawing.FontStyle.Regular);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured in setting the language", ex);
            }
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Enabled or disable the child control of the passed contol
        /// </summary>
        /// <param name="control">parent control</param>
        /// <param name="IsEnabled"></param>
        public void SetupAccess(Control control, bool isEnabled)
        {
            if (control.HasChildren)
            {
                foreach (Control cntrl in control.Controls)
                {
                    SetupAccess(cntrl, isEnabled);
                }
            }
            else
            {
                control.Enabled = isEnabled;
            }
        }
        public string GenerateRandomNumber(int Width, RandomNumberType randomNumberType)
        {
            log.LogMethodEntry(Width, randomNumberType);
            string validCharacters = "";
            if (randomNumberType == RandomNumberType.Numeric)
            {
                validCharacters = "1234567890";
            }
            else if (randomNumberType == RandomNumberType.AlphaNumeric)
            {
                validCharacters = "1234567890ABCDEFGHIJKLMNOPQRSUVWXYZ";
            }
            else if (randomNumberType == RandomNumberType.AlphaNumericWithSpecialChars)
            {
                validCharacters = "1234567890ABCDEFGHIJKLMNOPQRSUVWXYZ!@#$";
            }

            if (string.IsNullOrEmpty(validCharacters))
            {
                log.Debug("validCharacters is not set. Using default string");
                validCharacters = "1234567890ABCDEFGHIJKLMNOPQRSUVWXYZ";
            }
            //var ret = "";

                //byte[] seed = new byte[4];
                //new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(seed);
                //int seedInt = BitConverter.ToInt32(seed, 0);

                //var rnd = new Random(seedInt);
                //while (ret.Length < Width)
                //{
                //    ret += validCharacters[rnd.Next(validCharacters.Length - 1)].ToString();
                //}
                //log.LogMethodExit(ret);
                //return ret;
                return GenerateRandomNumber(Width, validCharacters);
        }

        

        internal string GenerateRandomNumber(int Width, string validCharacters)
        {
            log.LogMethodEntry(Width, validCharacters);
            //string validCharacters = "1234567890ABCDEFGHIJKLMNOPQRSUVWXYZ";
            if (string.IsNullOrEmpty(validCharacters))
            {
                log.Debug("validCharacters is not passed. Taking default value");
                validCharacters = "1234567890ABCDEFGHIJKLMNOPQRSUVWXYZ";
            }
            var ret = "";

            byte[] seed = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(seed);
            int seedInt = BitConverter.ToInt32(seed, 0);

            var rnd = new Random(seedInt);
            while (ret.Length < Width)
            {
                ret += validCharacters[rnd.Next(validCharacters.Length - 1)].ToString();
            }
            log.LogMethodExit(ret);
            return ret;
        }

        public int VersionCheck()
        {
            log.LogMethodEntry();
            int versionCheckResult = 0;
            try
            {
                string appName = System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName;
                string version = Assembly.GetEntryAssembly().GetName().Version.ToString();
                string[] versionList = version.Split('.');
                int[] appVersion = Array.ConvertAll(versionList, int.Parse);
                ParafaitExecutableVersionNumberBL evnBL = new ParafaitExecutableVersionNumberBL(appName, ExecutionContext.GetExecutionContext());
                if (evnBL.getParafaitExecutableVersionNumberDTO != null)
                {
                    versionCheckResult = evnBL.CompareExecutableVersions(appVersion);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured in version check", ex);
            }
            log.LogMethodExit(versionCheckResult);
            return versionCheckResult;
        }

        /// <summary>
        /// Converts the html text to pdf
        /// </summary>
        /// <param name="htmlText"></param>
        /// <returns></returns>
        public string ConvertImageToPDF(List<string> base64ImageList)
        {
            //Create a byte array that will eventually hold our final PDF
            String base64String = "";
            try
            {
                if (base64ImageList.Count > 0)
                {
                    iTextSharp.text.Image imageMain = iTextSharp.text.Image.GetInstance(Convert.FromBase64String(base64ImageList[0]));
                    using (var memoryStream = new MemoryStream())
                    {
                        float height = imageMain.Height + 40f;
                        float width = imageMain.Width + 40f;
                        float vMargin = 20f;
                        float hMargin = 20f;
                        iTextSharp.text.Rectangle size = new iTextSharp.text.Rectangle(width, height);
                        Document document = new Document(size, hMargin, hMargin, vMargin, vMargin);
                        PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                        document.Open();
                        int counter = 0;
                        foreach (string base64Image in base64ImageList)
                        {
                            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(Convert.FromBase64String(base64Image));
                            if (counter > 0)
                                document.NewPage();
                            document.Add(image);
                            counter++;

                        }
                        document.Close();
                        byte[] bytes = memoryStream.ToArray();
                        memoryStream.Close();
                        base64String = Convert.ToBase64String(bytes);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Failed to convert HTML to PDF" + ex.Message);
                base64String = "";
            }

            return base64String;
        }
    }

    public static class StaticUtils
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        public static bool CheckIfProgramAlreadyRunning(string ExeName, bool Activate = true, string MainWindowTitle = null)
        {
            log.LogMethodEntry(ExeName, Activate, MainWindowTitle);
            Process[] procs = null;
            procs = Process.GetProcessesByName(ExeName);
            foreach (Process p in procs)
            {
                if (p.Id != Process.GetCurrentProcess().Id && (MainWindowTitle == null || p.MainWindowTitle.Contains(MainWindowTitle)))
                {
                    if (Activate)
                        SetForegroundWindow(p.MainWindowHandle);
                    log.LogMethodExit(true);
                    return true;
                }
            }
            log.LogMethodExit(false);
            return false;
        }

        public static void logPOSDebug(string message)
        {
            log.LogMethodEntry(message);
            string fileName = ".\\log\\Parafait POS Debug-" + DateTime.Now.ToString("ddMMyyyy") + ".log";
            message = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss") + ":" + message;
            System.IO.File.AppendAllText(fileName, message + Environment.NewLine);
            log.LogMethodExit(null);
        }

        public static string InstalledFrameworkVersion()
        {
            log.LogMethodEntry();
            Microsoft.Win32.RegistryKey installed_versions = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP");
            string[] version_names = installed_versions.GetSubKeyNames();
            //version names start with 'v', eg, 'v3.5' which needs to be trimmed off before conversion
            string Framework = "";
            foreach (string s in version_names)
            {
                if (s.StartsWith("v", StringComparison.CurrentCultureIgnoreCase) == false)
                    continue;
                try
                {
                    string tFramework = s.Remove(0, 1);
                    if (string.CompareOrdinal(tFramework, Framework) > 0)
                        Framework = tFramework;
                }
                catch (Exception ex)
                {
                    log.Error("Error occured in installed framework version", ex);
                }
            }
            log.LogMethodExit(Framework);
            return Framework;
        }

        //Method to determine of specific configuration key needs to be added in Application Config files.
        //Key to be added - TransparentNetworkIPResolution
        public static bool InstalledFrameworkReleaseVersion()
        {
            log.LogMethodEntry();
            Microsoft.Win32.RegistryKey installedReleaseVersions = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, Microsoft.Win32.RegistryView.Registry32).OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\");
            if (installedReleaseVersions != null && installedReleaseVersions.GetValue("Release") != null)
            {
                int releaseVersion = (int)installedReleaseVersions.GetValue("Release");
                if (releaseVersion >= 393295)//Release version corresponds to version 4.6.0 or above
                {
                    log.LogMethodExit(true);
                    return true;
                }

                else
                {
                    log.LogMethodExit(false);
                    return false;
                }

            }
            else //Version would be less than 4
            {
                log.LogMethodExit(false);
                return false;
            }
        }

        public static string CaseInsensitiveReplace(string originalString, string oldValue, string newValue)
        {
            log.LogMethodEntry(originalString, oldValue, newValue);
            Regex regEx = new Regex(oldValue,
               RegexOptions.IgnoreCase | RegexOptions.Multiline);
            string returnvalue = (regEx.Replace(originalString, newValue));
            log.LogMethodExit(returnvalue);
            return (returnvalue);
        }

        public static string getParafaitConnectionString(string encryptedConnectionString)
        {
            log.LogMethodEntry("encryptedConnectionString");
            string returnvalue = DBUtils.getParafaitConnectionString(encryptedConnectionString);
            log.LogMethodExit();
            return returnvalue;
        }

        public static byte[] getKey(string insert)
        {
            log.LogMethodEntry("insert");
            string encryptionKey;
            try
            {
                encryptionKey = Encryption.GetParafaitKeys("MifareCard");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                encryptionKey ="46A97988SEMNOX!1CCCC9D1C581D86EE";
            }
            byte[] key = Encoding.UTF8.GetBytes(encryptionKey);
            byte[] insertBytes = Encoding.UTF8.GetBytes(insert.PadRight(4, 'X').Substring(0, 4));
            key[16] = insertBytes[0];
            key[17] = insertBytes[1];
            key[18] = insertBytes[2];
            key[19] = insertBytes[3];
            log.LogMethodExit("key");
            return key;
        }

        public static string EncryptConnectionString(string ConnectionString)
        {
            log.LogMethodEntry();
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(ConnectionString);

            builder.Password = Convert.ToBase64String(EncryptionAES.Encrypt(Encoding.UTF8.GetBytes(builder.Password.PadRight(16, ' ')), getKey(builder.DataSource)));
            log.LogMethodExit();
            return builder.ToString();
        }

    }
}
