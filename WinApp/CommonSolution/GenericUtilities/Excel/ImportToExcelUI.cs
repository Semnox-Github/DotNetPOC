/********************************************************************************************
 * Project Name - ImportToExcelUI
 * Description  - ImportToExcelUI
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        19-APR-2019   Raghuveera          Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MSExcel = Microsoft.Office.Interop.Excel;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Core.GenericUtilities.Excel
{
    /// <summary>
    /// Import excel details
    /// </summary>
    public partial class ImportToExcelUI : Form
    {
        protected Semnox.Core.Utilities.Utilities utilities;
        protected static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected Sheet uploadedSheet;
        protected Sheet errorSheet;
        protected int totalRecordCount;
        protected int importedRecordCount;
        protected int errorRecordCount;
        protected string importingItemName;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public ImportToExcelUI(Semnox.Core.Utilities.Utilities utilities)
        {
            log.LogMethodEntry(utilities);
            InitializeComponent();
            this.utilities = utilities;            
            log.LogMethodExit();
        }

        public virtual void ImportToExcelUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            lblMessage.Text = "";
            lnkError.Text = "";
            lblNote1.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 1531,"");
            progressBar.Visible = false;
            log.LogMethodExit();
        }

        public virtual async void btnUpload_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            InitializeForUpload();
            string filePath = GetFilePath();
            if (string.IsNullOrWhiteSpace(filePath))
            {
                lblMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 1524, this.Text);
                return;
            }
            OnImportStart();
            uploadedSheet = await Task<Sheet>.Factory.StartNew(() => { return GetUploadSheet(filePath); });
            if (IsUploadSheetValid())
            {
                progressBar.Value = 10;
                await Task.Factory.StartNew(() => { ImportRecords(); });
                if (errorRecordCount > 0)
                {
                    lnkError.Text = errorRecordCount.ToString() + " " + (errorRecordCount == 1 ? MessageContainerList.GetMessage(utilities.ExecutionContext, "Error") : MessageContainerList.GetMessage(utilities.ExecutionContext, "Errors"));
                }
                lblMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 1523, importedRecordCount, importingItemName);
            }
            OnImportEnd();
            log.LogMethodExit();
        }

        protected virtual bool IsUploadSheetValid()
        {
            log.LogMethodEntry();
            if (uploadedSheet == null)
            {
                lblMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 1520, this.Text);
                return false;
            }
            if (uploadedSheet.Rows.Count < 2)
            {
                lblMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 1521, this.Text);
                return false;
            }
            if (ValidateHeaderRow() == false)
            {
                lblMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 1522, this.Text);
                return false;
            }
            log.LogMethodExit(true);
            return true;
        }

        protected virtual bool ValidateHeaderRow()
        {
            log.LogMethodEntry();
            bool result = false;
            int index = 0;
            try
            {
                AttributeDefinition attributeDefinition = GetAttributeDefinition();
                attributeDefinition.ValidateHeaderRow(uploadedSheet[0], ref index);
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

        protected virtual AttributeDefinition GetAttributeDefinition()
        {
            throw new NotImplementedException();
        }

        protected virtual void OnImportEnd()
        {
            log.LogMethodEntry();
            progressBar.Visible = false;
            btnCancel.Enabled = true;
            btnTemplate.Enabled = true;
            btnUpload.Enabled = true;
            lnkError.Enabled = true;
            log.LogMethodExit();
        }

        protected virtual void OnImportStart()
        {
            log.LogMethodEntry();
            progressBar.Visible = true;
            lblMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 1519,importingItemName);
            btnCancel.Enabled = false;
            btnTemplate.Enabled = false;
            btnUpload.Enabled = false;
            lnkError.Enabled = false;
            log.LogMethodExit();
        }

        protected virtual void ImportRecords()
        {
            log.LogMethodEntry();            
            log.LogMethodExit();
        }

        protected virtual string GetFilePath()
        {
            log.LogMethodEntry();
            string filePath = string.Empty;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm;";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filePath = ofd.FileName;
            }
            log.LogMethodExit(filePath);
            return filePath;
        }

        protected virtual Sheet GetUploadSheet(string filePath)
        {
            log.LogMethodEntry();
            Sheet sheet = null;
            try
            {
                ExcelFile csvFile = new ExcelFile(filePath);
                sheet = csvFile.Read(ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "DATE_FORMAT"));
                progressBar.Invoke((Action)(() => progressBar.Value = 50));
            }
            catch (Exception ex)
            {
                log.Error("Error occured while reading the excel file", ex);
            }
            log.LogMethodExit(sheet);
            return sheet;
        }

        protected virtual void InitializeForUpload()
        {
            log.LogMethodEntry();
            lblMessage.Text = "";
            lnkError.Text = "";
            uploadedSheet = null;
            errorSheet = null;
            progressBar.Value = 0;
            progressBar.Visible = false;
            totalRecordCount = 0;
            importedRecordCount = 0;
            errorRecordCount = 0;
            log.LogMethodExit();
        }

        protected virtual void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Close();
            log.LogMethodExit();
        }

        protected void btnTemplate_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                Sheet sheet = new Sheet();
                Row headerRow = new Row();
                AttributeDefinition attributeDefinition = GetAttributeDefinition();
                attributeDefinition.BuildHeaderRow(headerRow);
                sheet.AddRow(headerRow);
                string fileName = "Import "+this.Text+" Data Template.xls";
                SaveExcelFile(sheet, fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error("Error occured while creating the template file", ex);
            }
            log.LogMethodExit();
        }

        protected void lnkError_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if(errorSheet != null)
            {
                string fileName = "Import " + this.Text + " Data error records.xls";
                SaveExcelFile(errorSheet, fileName);
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
                    ExcelFile excelFile = new ExcelFile(sfd.FileName);
                    excelFile.Write(sheet, importingItemName, sfd.FileName);
                    //System.IO.FileStream fs = System.IO.File.Open(sfd.FileName, System.IO.FileMode.Create);
                    //fs.Close();
                    //System.IO.StreamWriter streamWriter = System.IO.File.AppendText(sfd.FileName);
                    //for (int i = 0; i < sheet.Rows.Count; i++)
                    //{
                    //    for (int j = 0; j < sheet.Rows[i].Cells.Count; j++)
                    //    {
                    //        streamWriter.Write(sheet.Rows[i].Cells[j].Value + '\t');
                    //    }
                    //    streamWriter.WriteLine();
                    //}
                    //streamWriter.Close();
                    //System.Diagnostics.Process.Start("excel", "\"" + sfd.FileName + "\"");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error("Error occured while saving the excel file", ex);
            }
        }
        //protected void SaveExcelFile(Sheet sheet, string filePath)
        //{
        //    log.LogMethodEntry(sheet, filePath);
        //    try
        //    {
        //        SaveFileDialog sfd = new SaveFileDialog();
        //        sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        //        sfd.FileName = filePath;
        //        sfd.Filter = "Excel Files|*.xls;*.xlsx;";
        //        if (sfd.ShowDialog() == DialogResult.OK)
        //        {
        //            ExcelFile csvFile = new ExcelFile(sfd.FileName);
        //            csvFile.Write(sheet);
        //            System.Diagnostics.Process.Start("excel", "\"" + sfd.FileName + "\"");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //        log.Error("Error occured while saving the excel file", ex);
        //    }
        //}
    }
}
