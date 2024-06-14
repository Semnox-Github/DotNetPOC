/********************************************************************************************
* Project Name - Parafait Queue Management
* Description  - Parafait Queue Manager BowlerStats
* 
**************
**Version Log
**************
*Version     Date             Modified By    Remarks          
*********************************************************************************************
*2.80        10-Sep-2019      Jinto Thomas         Added logger for methods
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace ParafaitQueueManagement
{
    public partial class BowlerStats : Form
    {
        HttpWebResponse response = null;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //StreamReader reader = null;
        public BowlerStats()
        {
            log.LogMethodEntry();
            InitializeComponent();
            log.LogMethodExit();
        }

        private void BowlerStats_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            //LoadBowlerStatistics();
            Common.setupGrid(ref dgvBowlerStatistics);
            Common.Utilities.setupDataGridProperties(ref dgvBowlerStatistics);
            loadDropDowns();
            log.LogMethodExit();
        }

        private void LoadBowlerStatistics()
        {
            log.LogMethodEntry();
           // MessageBox.Show(cmbFromDate.SelectedItem.ToString());
            string filteredata = string.Empty;
            string[] splitarray;
            string[] splitcommaarray;
            string[] ampmsplitarray;
            string fromDateValue = string.Empty;
            string toDateValue = string.Empty;
            string fromTimeValue = string.Empty;
            string toTimeValue = string.Empty;
            if(cmbFromDate.Text.ToString().Contains("AM"))
            {
                ampmsplitarray=cmbFromDate.Text.ToString().Split(':');
                fromTimeValue="0" + ampmsplitarray[0].ToString();
            }
            else
            {
                int j = 12;
                ampmsplitarray=cmbFromDate.Text.ToString().Split(':');
                j = j + Convert.ToInt32(ampmsplitarray[0]);
                fromTimeValue = j.ToString();
            }

            if (cmbToDate.Text.ToString().Contains("AM"))
            {
                ampmsplitarray = cmbToDate.Text.ToString().Split(':');
                toTimeValue = "0" + ampmsplitarray[0].ToString();
            }
            else
            {
                int k = 12;
                ampmsplitarray = cmbToDate.Text.ToString().Split(':');
                k =k + Convert.ToInt32(ampmsplitarray[0]);
                toTimeValue = k.ToString();
            }


            string externalsystemURL = Common.Utilities.getParafaitDefaults("THIRD_PARTY_SYSTEM_SYNCH_URL");
            externalsystemURL = externalsystemURL.Substring(0, externalsystemURL.IndexOf("/user"));
            fromDateValue = dtFromDate.Value.ToString("ddMMyyyy");
            toDateValue = dtTodate.Value.ToString("ddMMyyyy");
           // MessageBox.Show(externalsystemURL + "/statistic/getMasterBlasterBowlerUsage?playType=0&leaderboardName=TRADITIONAL&startDate=" + fromDateValue + "&endDate=" + toDateValue + "&startTime=" + fromTimeValue + "&endTime=" + toTimeValue);
            HttpWebRequest webreq = (HttpWebRequest)HttpWebRequest.Create(externalsystemURL+"/statistic/getMasterBlasterBowlerUsage?playType=0&leaderboardName=TRADITIONAL&startDate="+fromDateValue+"&endDate="+toDateValue+"&startTime="+fromTimeValue+"&endTime="+toTimeValue);        
         //  HttpWebRequest webreq = (HttpWebRequest)HttpWebRequest.Create("http://10.0.0.5:8080/server/spring/statistic/getMasterBlasterBowlerUsage?playType=0&leaderboardName=TRADITIONAL&startDate=13062013&endDate=15062013&startTime=15&endTime=16");        
            response = (HttpWebResponse)webreq.GetResponse();
           // string responseData=@"{"reason":null,"success":true,"startDate":null,"endDate":null,"startTimeslice":null,"endTimeslice":null,"bowlerUsage":{"MURALITHARAN":146,"WARNE":165,"AKRAM":178,"LEE":158,"MALINGA":185,"TENDULKAR_MB":113,"KUMBLE":131}}";
           if (response.StatusCode == HttpStatusCode.OK)
           {
               Stream streamresponse = response.GetResponseStream();
               StreamReader reader = new StreamReader(streamresponse);
               string responseData = reader.ReadToEnd();
               string splitstring = responseData.Substring(responseData.IndexOf("bowlerUsage"),responseData.IndexOf("}")-responseData.IndexOf("bowlerUsage"));
               splitstring = splitstring.Replace("bowlerUsage", "").Replace(":{","");               
               splitarray = splitstring.Split(',');
               List<KeyValueStruct> bowlerList = new List<KeyValueStruct>();
               for(int i=0;i<splitarray.Length;i=i+1)
               {  
                       if(splitarray[i].Contains(":"))
                       {
                           splitcommaarray = splitarray[i].Split(':');
                           bowlerList.Add(new KeyValueStruct(splitcommaarray[0].ToString().Replace("\"", ""), splitcommaarray[1].ToString()));
                       }
               }
               dgvBowlerStatistics.DataSource = bowlerList;
              
           }
            log.LogMethodExit();  
           
        }

        public class KeyValueStruct
        {
            private string _key;
            private string _val;

            public string BowlerName
            {
                get { return _key; }
                set { _key = value; }
            }
            public string Statistics
            {
                get { return _val; }
                set { _val = value; }
            }
            public KeyValueStruct()
            {
            }
            public KeyValueStruct(string key, string value)
            {
                _key = key;
                _val = value;
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            LoadBowlerStatistics();
            log.LogMethodExit();
        }

        private void  loadDropDowns()
        {
            log.LogMethodEntry();
            for (int i = 0; i < 24; i++)
            {
                string ampm;
                string hour;

                if (i < 12)
                {
                    ampm = "AM";
                }
                else
                {
                    ampm = "PM";
                }

                if (i <= 12)
                {
                    hour = i.ToString();
                }
                else
                {
                    hour = (i - 12).ToString();
                }

                cmbFromDate.Items.Add(hour + ":00 " + ampm);
                cmbToDate.Items.Add(hour + ":00 " + ampm);
            }

            cmbFromDate.SelectedItem = "6:00 AM";
            cmbToDate.SelectedItem = "6:00 AM";
            log.LogMethodExit();
        }

        private void btnStatExport_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if(dgvBowlerStatistics.Rows.Count==0)
            {
                MessageBox.Show("No data found for this date range");
                return;
            }
            Common.Utilities.ExportToExcel(dgvBowlerStatistics, "Bowler Statistics  List " + DateTime.Now.ToString("dd-MMM-yyyy"), "Bowler Statistics", Common.ParafaitEnv.SiteName,
            Convert.ToDateTime(dtFromDate.Value.ToString("dd-MMM-yyyy") + " " + cmbFromDate.Text.ToString()), Convert.ToDateTime(dtTodate.Value.ToString("dd-MMM-yyyy")+ " "+ cmbToDate.Text.ToString()));
            // ExportToExcel(dgvBowlerStatistics, "BowlerStatistics");
            log.LogMethodExit();
        }
        public void ExportToExcel(DataGridView dgv, string fileName, string Heading = null)
        {
            log.LogMethodEntry(dgv, fileName);
            ExportToExcel(dgv, fileName, Heading, null, DateTime.MinValue, DateTime.MinValue);
            log.LogMethodExit();
        }
        public void ExportToExcel(DataGridView dgv, string fileName,
                               string Heading, string site_name,
                               DateTime from_date, DateTime to_date,
                               params string[] otherParams)
        {
            log.LogMethodEntry();
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
                    MessageBox.Show("Error with file. Detailed error: " + ex.Message);
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
                   // btnWait.BackgroundImage = Properties.Resources.pressed1;
                    btnWait.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                    btnWait.Dock = System.Windows.Forms.DockStyle.Fill;
                    btnWait.FlatAppearance.BorderSize = 0;
                    btnWait.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
                    btnWait.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
                    btnWait.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    btnWait.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    btnWait.ForeColor = System.Drawing.Color.White;
                   // btnWait.Image = Properties.Resources.PreLoader;
                    btnWait.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
                    btnWait.Location = new System.Drawing.Point(0, 0);
                    btnWait.Name = "btnWait";
                    btnWait.Size = new System.Drawing.Size(346, 89);
                    btnWait.TabIndex = 2;
                    btnWait.Text = Common.Utilities.MessageUtils.getMessage(683); //Please wait while Excel opens your file
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
                    catch { }
                };
                System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(thr));
                thread.Start();

                try
                {
                    CreateExcelFromGrid(dgv, file_path, Heading, site_name, from_date, to_date);
                }
                catch { }
                finally
                {
                    ap.ExitThread();
                }
                log.LogMethodExit();
            }
        }
        public void CreateExcelFromGrid(DataGridView dgvReportData, string fileName,
                                               string Heading, string site_name,
                                               DateTime from_date, DateTime to_date,
                                               bool BGMode = false, params string[] otherParams)
        {
            log.LogMethodEntry(dgvReportData, fileName, Heading, site_name, from_date, to_date);
            List<DataGridView> dgv = new List<DataGridView>();
            dgv.Add(dgvReportData);

            CreateExcelFromGrid(dgv, fileName,
                                Heading, site_name,
                                from_date, to_date,
                                BGMode, otherParams);
            log.LogMethodExit();
        }
        public void CreateExcelFromGrid(List<DataGridView> dgvList, string fileName,
                                       string Heading, string site_name,
                                       DateTime from_date, DateTime to_date,
                                       bool BGMode = false, params string[] otherParams)
        {
            log.LogMethodEntry(dgvList, fileName, Heading, site_name, from_date, to_date);
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
                    Excel.Worksheet xlWorkSheet1 = (Excel.Worksheet)excelbk.Worksheets[sheetIndex++];
                    xlWorkSheet1.PageSetup.Orientation = Excel.XlPageOrientation.xlLandscape;
                    string lclHeading = Heading;
                    if (string.IsNullOrEmpty(lclHeading) == true)
                        lclHeading = dgvReportData.Name;
                    if (string.IsNullOrEmpty(lclHeading) == false)
                        xlWorkSheet1.Name = (lclHeading.Length > 30 ? lclHeading.Substring(0, 30) : lclHeading).Replace("/", "-");

                    Font font = dgvReportData.DefaultCellStyle.Font;
                    if (font == null)
                        font = new Font("Tahoma", 8);

                    excelApp.StandardFont = font.Name;
                    excelApp.StandardFontSize = font.Size;

                    int WORKSHEETSTARTROW = 1, WORKSHEETSTARTCOL = 1;
                    if (string.IsNullOrEmpty(lclHeading) == false)
                    {
                        Excel.Range xlHeading = (Excel.Range)xlWorkSheet1.Cells[WORKSHEETSTARTROW, 3];
                        xlHeading.Value = lclHeading;
                        xlHeading.Font.Bold = true;
                        xlHeading.Font.Name = font.Name;
                        xlHeading.Font.Size = 10;
                        WORKSHEETSTARTROW++;
                    }

                    if (string.IsNullOrEmpty(site_name) == false)
                    {
                        Excel.Range xllblSite = (Excel.Range)xlWorkSheet1.Cells[WORKSHEETSTARTROW, 2];
                        xllblSite.Value = Common.Utilities.MessageUtils.getMessage("Site") + ":";
                        xllblSite.Font.Bold = true;
                        xllblSite.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;

                        Excel.Range xlSite = (Excel.Range)xlWorkSheet1.Cells[WORKSHEETSTARTROW, 3];
                        xlSite.Value = site_name;
                        xlSite.Font.Bold = true;
                        WORKSHEETSTARTROW++;

                        Excel.Range xllblFromDate = (Excel.Range)xlWorkSheet1.Cells[WORKSHEETSTARTROW, 2];
                        xllblFromDate.Value = Common.Utilities.MessageUtils.getMessage("From") + ":";
                        xllblFromDate.Font.Bold = true;
                        xllblFromDate.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;

                        Excel.Range xlFromDate = (Excel.Range)xlWorkSheet1.Cells[WORKSHEETSTARTROW, 3];
                        xlFromDate.Value = from_date.ToString("ddd, dd-MMM-yyyy h:mm tt");
                        xlFromDate.Font.Bold = true;
                        xlFromDate.Font.Name = font.Name;
                        xlFromDate.Font.Size = 8;
                        WORKSHEETSTARTROW++;

                        Excel.Range xllbltoDate = (Excel.Range)xlWorkSheet1.Cells[WORKSHEETSTARTROW, 2];
                        xllbltoDate.Value = Common.Utilities.MessageUtils.getMessage("To") + ":";
                        xllbltoDate.Font.Bold = true;
                        xllbltoDate.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;

                        Excel.Range xltoDate = (Excel.Range)xlWorkSheet1.Cells[WORKSHEETSTARTROW, 3];
                        xltoDate.Value = to_date.ToString("ddd, dd-MMM-yyyy h:mm tt");
                        xltoDate.Font.Bold = true;
                        WORKSHEETSTARTROW++;

                        Excel.Range xllblRunAt = (Excel.Range)xlWorkSheet1.Cells[WORKSHEETSTARTROW, 2];
                        xllblRunAt.Value = Common.Utilities.MessageUtils.getMessage("Run At") + ":";
                        xllblRunAt.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;

                        Excel.Range xlRunAt = (Excel.Range)xlWorkSheet1.Cells[WORKSHEETSTARTROW, 3];
                        xlRunAt.Value = DateTime.Now.ToString("ddd, dd-MMM-yyyy h:mm tt");
                        xlRunAt.Font.Name = font.Name;
                        xlRunAt.Font.Size = 8;
                        WORKSHEETSTARTROW++;

                        Excel.Range xllblRunBy = (Excel.Range)xlWorkSheet1.Cells[WORKSHEETSTARTROW, 2];
                        xllblRunBy.Value = Common.Utilities.MessageUtils.getMessage("Run By") + ":";
                        xllblRunBy.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;

                        Excel.Range xlRunBy = (Excel.Range)xlWorkSheet1.Cells[WORKSHEETSTARTROW, 3];
                        xlRunBy.Value = Common.ParafaitEnv.Username;
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

                        Excel.Range xlRange = (Excel.Range)xlWorkSheet1.Cells[worksheetRow, colNum];
                        xlRange.Value2 = dgvReportData.Columns[i].HeaderText;
                        xlRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(dgvReportData.ColumnHeadersDefaultCellStyle.BackColor);
                        xlRange.Borders.Color = System.Drawing.ColorTranslator.ToOle(Color.LightSteelBlue);

                        colNum += 1;
                    }

                    colNum--;
                    string colStart = Char.ConvertFromUtf32('A' + WORKSHEETSTARTCOL - 1) + (worksheetRow).ToString();
                    string colEnd = Char.ConvertFromUtf32('A' + colNum - 1) + (worksheetRow).ToString();
                    MessageBox.Show(colStart + " " + colEnd);
                    Excel.Range xlHRange = xlWorkSheet1.get_Range(colStart, colEnd);
                    xlHRange.Font.Size = font.Size;
                    xlHRange.Font.Bold = font.Bold;
                    xlHRange.Font.Italic = font.Italic;
                    xlHRange.Font.Underline = font.Underline;
                    xlHRange.Font.Name = font.Name;

                    colEnd = Char.ConvertFromUtf32('A' + colNum - 1) + (worksheetRow + dgvReportData.Rows.Count).ToString();
                    Excel.Range xlDataRange = xlWorkSheet1.get_Range(colStart, colEnd);
                    xlDataRange.Borders.LineStyle = DataGridViewCellBorderStyle.Single;
                    xlDataRange.Borders.Color = System.Drawing.ColorTranslator.ToOle(Color.LightSteelBlue);

                    worksheetRow = WORKSHEETSTARTROW + 1;
                    //Copy data rows
                    for (int rowCount = 0; rowCount < dgvReportData.Rows.Count; rowCount++)
                    {
                        colStart = Char.ConvertFromUtf32('A' + WORKSHEETSTARTCOL - 1) + worksheetRow.ToString();
                        colEnd = Char.ConvertFromUtf32('A' + colNum - 1) + worksheetRow.ToString();
                        Excel.Range xlRowRange = xlWorkSheet1.get_Range(colStart, colEnd);

                        if (rowCount % 2 != 0)
                            xlRowRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(dgvReportData.AlternatingRowsDefaultCellStyle.BackColor);

                        int worksheetcol = WORKSHEETSTARTCOL;
                        for (int colCount = 0; colCount < dgvReportData.Columns.Count; colCount++)
                        {
                            if (dgvReportData.Columns[colCount].Visible == false || dgvReportData.Columns[colCount].GetType() == typeof(DataGridViewImageColumn))
                                continue;

                            Excel.Range xlRange = (Excel.Range)xlWorkSheet1.Cells[worksheetRow, worksheetcol];
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
                if (BGMode == false)
                    MessageBox.Show("Here " + ex.Message);
                log.Error(ex.Message);
            }
            finally
            {
                if (BGMode)
                    excelApp.Quit();
            }
            log.LogMethodExit();
        }
        
    }
}
