/********************************************************************************************
 * Project Name - Customer
 * Description  - TagSerialMappingListUI
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019      Girish kundar      Modified :Removed Unused namespace's.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using MSExcel = Microsoft.Office.Interop.Excel;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Bulk Upload card UI
    /// </summary>
    public partial class TagSerialMappingListUI : Form
    {
        private Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Sheet uploadedSheet;
        private TagSerialMappingDTODefinition tagSerialMappingDTODefinition;
        private List<TagSerialMappingDTO> tagSerialMappingDTOList;
        private CancellationTokenSource cancellationTokenSource;
        /// <summary>
        /// parameterized constructor
        /// </summary>
        /// <param name="utilities"></param>
        public TagSerialMappingListUI(Utilities utilities)
        {
            log.LogMethodEntry(utilities);
            InitializeComponent();
            this.utilities = utilities;
            utilities.setupDataGridProperties(ref dgvTagSerialMappingDTOList);
            utilities.setLanguage(this);
            tagSerialMappingDTODefinition = new TagSerialMappingDTODefinition("");
            btnCancel.Visible = false;
            lblXLRecordCount.Text = "";
            ThemeUtils.SetupVisuals(this);
            log.LogMethodExit();
        }

        private async void btnChooseFile_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ClearVariables();
            string filePath = GetFilePath();
            lblFileName.Text = filePath;
            lblXLRecordCount.Text = "";
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return;
            }
            DisableControls();
            uploadedSheet = await Task<Sheet>.Factory.StartNew(() => { return GetUploadSheet(filePath); });
            if (IsUploadSheetValid())
            {
                progressBar.Value = 10;
                tagSerialMappingDTOList = await Task<List<TagSerialMappingDTO>>.Factory.StartNew(() => { return GetTagSerialMappingDTOList(uploadedSheet); });
                tagSerialMappingDTOListBS.DataSource = tagSerialMappingDTOList;
                lblMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "&1 Records", tagSerialMappingDTOList.Count);
                lblXLRecordCount.Text = lblMessage.Text;
            }
            EnableControls();
            log.LogMethodExit();
        }

        private bool IsUploadSheetValid()
        {
            log.LogMethodEntry();
            if (uploadedSheet == null)
            {
                lblMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Invalid File");
                return false;
            }
            if (uploadedSheet.Rows.Count < 2)
            {
                lblMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 882);
                return false;
            }
            if (ValidateHeaderRow() == false)
            {
                lblMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 1522);
                return false;
            }
            log.LogMethodExit(true);
            return true;
        }

        private List<TagSerialMappingDTO> GetTagSerialMappingDTOList(Sheet uploadedSheet)
        {
            log.LogMethodEntry(uploadedSheet);
            List<TagSerialMappingDTO> tagSerialMappingDTOList = new List<TagSerialMappingDTO>();
            int totalCardCount = uploadedSheet.Rows.Count - 1;
            for (int i = 1; i < uploadedSheet.Rows.Count; i++)
            {
                int index = 0;
                TagSerialMappingDTO tagSerialMappingDTO = (TagSerialMappingDTO)tagSerialMappingDTODefinition.Deserialize(uploadedSheet[0], uploadedSheet[i], ref index);
                progressBar.Invoke((Action)(() => progressBar.Value = (50 + (i / totalCardCount * 50))));
                tagSerialMappingDTOList.Add(tagSerialMappingDTO);
            }
            log.LogMethodExit(tagSerialMappingDTOList);
            return tagSerialMappingDTOList;
        }

        private bool ValidateHeaderRow()
        {
            log.LogMethodEntry();
            bool result = false;
            int index = 0;
            try
            {
                tagSerialMappingDTODefinition.ValidateHeaderRow(uploadedSheet[0], ref index);
                result = true;
            }
            catch (Exception ex)
            {
                log.Error("Invalid header row", ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit(result);
            return result;
        }

        private void ClearVariables()
        {
            log.LogMethodEntry();
            lblMessage.Text = "";
            lblFileName.Text = "";
            uploadedSheet = null;
            progressBar.Value = 0;
            tagSerialMappingDTODefinition = new TagSerialMappingDTODefinition("");
            tagSerialMappingDTOList = new List<TagSerialMappingDTO>();
            tagSerialMappingDTOListBS.DataSource = tagSerialMappingDTOList;
            log.LogMethodExit();
        }

        private string GetFilePath()
        {
            log.LogMethodEntry();
            string filePath = string.Empty;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filePath = ofd.FileName;
            }
            log.LogMethodExit(filePath);
            return filePath;
        }

        private void DisableControls()
        {
            log.LogMethodEntry();
            btnChooseFile.Enabled = false;
            btnClear.Enabled = false;
            btnFileFormat.Enabled = false;
            btnUpload.Enabled = false;
            btnClose.Enabled = false;
            log.LogMethodExit();
        }

        private void EnableControls()
        {
            log.LogMethodEntry();
            btnChooseFile.Enabled = true;
            btnClear.Enabled = true;
            btnFileFormat.Enabled = true;
            btnUpload.Enabled = true;
            btnClose.Enabled = true;
            log.LogMethodExit();
        }

        private Sheet GetUploadSheet(string filePath)
        {
            log.LogMethodEntry();
            Sheet sheet = new Sheet();
            try
            {
                MSExcel.Application application = new MSExcel.Application();
                application.Visible = false;
                MSExcel.Workbook MyBook = application.Workbooks.Open(filePath);
                MSExcel.Worksheet MySheet = MyBook.Sheets[1];
                MySheet.Columns.ClearFormats();
                MySheet.Rows.ClearFormats();
                object[,] range = (object[,])MySheet.UsedRange.Value2;
                int noOfRows = range.GetLength(0);
                int noOfColumns = range.GetLength(1);
                log.LogVariableState("Max Row no:", noOfRows);
                log.LogVariableState("Max Column no:", noOfColumns);
                for (int i = 1; i <= range.GetLength(0); i++)
                {
                    Row row = new Row();
                    for (int j = 1; j <= range.GetLength(1); j++)
                    {
                        Cell cell = null;
                        if (range[i, j] != null)
                        {
                            if (range[i, j] is DateTime)
                            {
                                cell = new Cell(((DateTime)range[i, j]).ToString(ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "DATE_FORMAT")));
                            }
                            else
                            {
                                cell = new Cell(range[i, j].ToString());
                            }
                        }
                        else
                        {
                            cell = new Cell(string.Empty);
                        }
                        row.AddCell(cell);
                    }
                    int progress = ((i + 1) / noOfRows) * 50;
                    progressBar.Invoke((Action)(() => progressBar.Value = progress));
                    sheet.AddRow(row);
                }
                application.DisplayAlerts = false;
                application.Quit();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while reading the excel file", ex);
                sheet = null;
            }
            log.LogMethodExit(sheet);
            return sheet;
        }

        private async void btnUpload_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            DisableControls();
            btnCancel.Visible = true;
            cancellationTokenSource = new CancellationTokenSource();
            if (tagSerialMappingDTOList != null && tagSerialMappingDTOList.Count > 0)
            {
                progressBar.Value = 0;
                cancellationTokenSource = new CancellationTokenSource();
                int successCount = await Task<int>.Factory.StartNew(() => { return UploadTagSerialMappingDTOList(tagSerialMappingDTOList, cancellationTokenSource.Token); });
                lblMessage.Text = successCount.ToString() + " mappings loaded successfully";
                if(successCount< tagSerialMappingDTOList.Count)
                {
                    lblMessage.Text = lblMessage.Text + "; " + (tagSerialMappingDTOList.Count - successCount).ToString() + " failed.";
                }
            }
            else
            {
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 86));
            }
            btnCancel.Visible = false;
            EnableControls();
            log.LogMethodExit();
        }

        private int UploadTagSerialMappingDTOList(List<TagSerialMappingDTO> tagSerialMappingDTOList, CancellationToken token)
        {
            log.LogMethodEntry(tagSerialMappingDTOList, token);
            int successCount = 0;
            if (tagSerialMappingDTOList != null && tagSerialMappingDTOList.Count > 0)
            {
                for (int i = 0; i < tagSerialMappingDTOList.Count; i++)
                {
                    if(token.IsCancellationRequested)
                    {
                        break;
                    }
                    TagSerialMappingBL tagSerialMappingBL = new TagSerialMappingBL(utilities.ExecutionContext, tagSerialMappingDTOList[i]);
                    try
                    {
                        tagSerialMappingBL.Save();
                        dgvTagSerialMappingDTOList.Invoke((Action)(() => { dgvTagSerialMappingDTOList.Rows[i].Cells[Status.Index].Value = MessageContainerList.GetMessage(utilities.ExecutionContext, "Success"); }));
                        successCount++;
                    }
                    catch (ValidationException ex)
                    {
                        log.Error("Error occurred while saving tag serial mapping", ex);
                        log.LogVariableState("", tagSerialMappingDTOList[i]);
                        dgvTagSerialMappingDTOList.Invoke((Action)(() => { dgvTagSerialMappingDTOList.Rows[i].Cells[Message.Index].Value = ex.GetAllValidationErrorMessages(); }));
                        dgvTagSerialMappingDTOList.Invoke((Action)(() => { dgvTagSerialMappingDTOList.Rows[i].Cells[Status.Index].Value = MessageContainerList.GetMessage(utilities.ExecutionContext, "Failed"); }));
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error occurred while saving tag serial mapping", ex);
                        log.LogVariableState("", tagSerialMappingDTOList[i]);
                        dgvTagSerialMappingDTOList.Invoke((Action)(() => { dgvTagSerialMappingDTOList.Rows[i].Cells[Message.Index].Value = ex.Message; }));
                        dgvTagSerialMappingDTOList.Invoke((Action)(() => { dgvTagSerialMappingDTOList.Rows[i].Cells[Status.Index].Value = MessageContainerList.GetMessage(utilities.ExecutionContext, "Failed"); }));
                    }
                    progressBar.Invoke((Action)(() => { progressBar.Value = ((i + 1) / tagSerialMappingDTOList.Count) * 100; }));
                }
            }
            log.LogMethodExit(successCount);
            return successCount;
        }
        void writeLog(string text)
        {
            this.Invoke(new Action(() => { lblMessage.Text = text; }));
        }

        private void btnFileFormat_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                Sheet sheet = new Sheet();
                Row headerRow = new Row();
                tagSerialMappingDTODefinition.BuildHeaderRow(headerRow);
                sheet.AddRow(headerRow);
                string fileName = "CardBulkUpload.xls";
                SaveExcelFile(sheet, fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error("Error occurred while creating the template file", ex);
            }
            log.LogMethodExit();
        }

        private void SaveExcelFile(Sheet sheet, string filePath)
        {
            log.LogMethodEntry(sheet, filePath);
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                sfd.FileName = filePath;
                sfd.Filter = ".xls | *.xls";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    System.IO.FileStream fs = System.IO.File.Open(sfd.FileName, System.IO.FileMode.Create);
                    fs.Close();
                    System.IO.StreamWriter streamWriter = System.IO.File.AppendText(sfd.FileName);
                    for (int i = 0; i < sheet.Rows.Count; i++)
                    {
                        for (int j = 0; j < sheet.Rows[i].Cells.Count; j++)
                        {
                            streamWriter.Write(sheet.Rows[i].Cells[j].Value + '\t');
                        }
                        streamWriter.WriteLine();
                    }
                    streamWriter.Close();
                    System.Diagnostics.Process.Start("excel", "\"" + sfd.FileName + "\"");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error("Error occurred while saving the excel file", ex);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ClearVariables();
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Close();
            log.LogMethodExit();
        }

        private void frmBulkUploadCards_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ClearVariables();
            log.LogMethodExit();
        }

        private void frmBulkUploadCards_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (btnUpload.Enabled == false)
                e.Cancel = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if(cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
            }
            log.LogMethodExit();
        }
    }
}
