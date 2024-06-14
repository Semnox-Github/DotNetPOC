

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
using Semnox.Core.GenericUtilities.Excel;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Import customer details
    /// </summary>
    public partial class ImportCustomerUI : Form
    {
        private Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Sheet uploadedSheet;
        private Sheet errorSheet;
        private int totalCustomerCount;
        private int importedCustomerCount;
        private int errorCustomerCount;
        private CustomerDTODefinition customerDTODefinition;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public ImportCustomerUI(Utilities utilities)
        {
            log.LogMethodEntry(utilities);
            InitializeComponent();
            this.utilities = utilities;
            customerDTODefinition = new CustomerDTODefinition(utilities.ExecutionContext, "");
            log.LogMethodExit();
        }

        private void ImportCustomerUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            lblMessage.Text = "";
            lnkError.Text = "";
            lblNote1.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 1531, "Clear customer id values to import the customer.");
            progressBar.Visible = false;
            log.LogMethodExit();
        }

        private async void btnUpload_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            InitializeForUpload();
            string filePath = GetFilePath();
            if(string.IsNullOrWhiteSpace(filePath))
            {
                lblMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 1524,"customer");
                return;
            }
            OnImportStart();
            uploadedSheet = await Task<Sheet>.Factory.StartNew(() => { return GetUploadSheet(filePath); });
            if(IsUploadSheetValid())
            {
                progressBar.Value = 10;
                await Task.Factory.StartNew(() => { ImportCustomers(); });
                if (errorCustomerCount > 0)
                {
                    lnkError.Text = errorCustomerCount.ToString() + " " + (errorCustomerCount == 1 ? MessageContainerList.GetMessage(utilities.ExecutionContext, "Error") : MessageContainerList.GetMessage(utilities.ExecutionContext, "Errors"));
                }
                lblMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 1523, importedCustomerCount, "customer");
            }
            OnImportEnd();
            log.LogMethodExit();
        }

        private bool IsUploadSheetValid()
        {
            log.LogMethodEntry();
            if (uploadedSheet == null)
            {
                lblMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 1520, "customer");
                return false;
            }
            if (uploadedSheet.Rows.Count < 2)
            {
                lblMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 1521, "customer");
                return false;
            }
            if (ValidateHeaderRow() == false)
            {
                lblMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 1522, "customer");
                return false;
            }
            log.LogMethodExit(true);
            return true;
        }

        private bool ValidateHeaderRow()
        {
            log.LogMethodEntry();
            bool result = false;
            int index = 0;
            try
            {
                customerDTODefinition.ValidateHeaderRow(uploadedSheet[0], ref index);
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

        private void OnImportEnd()
        {
            log.LogMethodEntry();
            progressBar.Visible = false;
            btnCancel.Enabled = true;
            btnTemplate.Enabled = true;
            btnUpload.Enabled = true;
            lnkError.Enabled = true;
            log.LogMethodExit();
        }

        private void OnImportStart()
        {
            log.LogMethodEntry();
            progressBar.Visible = true;
            lblMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 1519, "customers");
            btnCancel.Enabled = false;
            btnTemplate.Enabled = false;
            btnUpload.Enabled = false;
            lnkError.Enabled = false;
            log.LogMethodExit();
        }

        private void ImportCustomers()
        {
            log.LogMethodEntry();
            SqlConnection sqlConnection = utilities.getConnection();
            totalCustomerCount = uploadedSheet.Rows.Count;
            for (int i = 1; i < uploadedSheet.Rows.Count; i++)
            {
                int index = 0;
                try
                {
                    CustomerDTO customerDTO = (CustomerDTO)customerDTODefinition.Deserialize(uploadedSheet[0], uploadedSheet[i], ref index);
                    if(customerDTO.Id < 0)
                    {
                        SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                        try
                        {
                            CustomerBL customerBL = new CustomerBL(utilities.ExecutionContext, customerDTO);
                            customerBL.Save(sqlTransaction);
                            sqlTransaction.Commit();
                            importedCustomerCount++;
                        }
                        catch (Exception)
                        {
                            sqlTransaction.Rollback();
                            throw;
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while importing customers", ex);
                    log.LogVariableState("Row", uploadedSheet[i]);
                    if(errorSheet == null)
                    {
                        errorSheet = new Sheet();
                        errorSheet.AddRow(uploadedSheet[0]);
                        errorSheet[0].AddCell(new Cell(MessageContainerList.GetMessage(utilities.ExecutionContext, "Errors")));
                    }
                    errorSheet.AddRow(uploadedSheet[i]);
                    string errorMessage = "";
                    string seperator = "";
                    if (ex is ValidationException)
                    {
                        foreach (var validationError in (ex as ValidationException).ValidationErrorList)
                        {
                            errorMessage += seperator;
                            errorMessage += validationError.Message;
                            seperator = ", ";
                        }
                    }
                    else
                    {
                        errorMessage = ex.Message;
                    }
                    errorSheet[errorSheet.Rows.Count - 1].AddCell(new Cell(errorMessage));
                    errorCustomerCount++;
                }
                progressBar.Invoke((Action)(() => progressBar.Value = (50 + (10 * (i + 1) / totalCustomerCount))));
            }
            sqlConnection.Close();
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
                        if (range[i,j] != null)
                        {
                            if(range[i, j] is DateTime)
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
                    int progress = 50 * (i + 1) / noOfRows;
                    progressBar.Invoke((Action)(() => progressBar.Value = progress));
                    sheet.AddRow(row);
                }
                application.DisplayAlerts = false;
                application.Quit();
            }
            catch (Exception ex)
            {
                log.Error("Error occured while reading the excel file", ex);
                sheet = null;
            }
            log.LogMethodExit(sheet);
            return sheet;
        }

        private void InitializeForUpload()
        {
            log.LogMethodEntry();
            lblMessage.Text = "";
            lnkError.Text = "";
            uploadedSheet = null;
            errorSheet = null;
            progressBar.Value = 0;
            progressBar.Visible = false;
            totalCustomerCount = 0;
            importedCustomerCount = 0;
            errorCustomerCount = 0;
            log.LogMethodExit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Close();
            log.LogMethodExit();
        }

        private void btnTemplate_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                Sheet sheet = new Sheet();
                Row headerRow = new Row();
                customerDTODefinition.BuildHeaderRow(headerRow);
                sheet.AddRow(headerRow);
                string fileName = "Import Customer Data Template.xls";
                SaveExcelFile(sheet, fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error("Error occured while creating the template file", ex);
            }
            log.LogMethodExit();
        }

        private void lnkError_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if(errorSheet != null)
            {
                string fileName = "Import Customer Data erorr records.xls";
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
                log.Error("Error occured while saving the excel file", ex);
            }
        }
    }
}
