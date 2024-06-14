
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Core.GenericUtilities.Excel;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Customer Export UI
    /// </summary>
    public partial class CustomerExportUI : Form
    {
        private Utilities utilities;
        private CustomerSearchCriteria customerSearchCriteria;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        CancellationTokenSource cancellationTokenSource;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public CustomerExportUI(Utilities utilities, CustomerSearchCriteria customerSearchCriteria)
        {
            log.LogMethodEntry(utilities, customerSearchCriteria);
            InitializeComponent();
            this.utilities = utilities;
            this.customerSearchCriteria = customerSearchCriteria;
            progressBar.Value = 0;
            lblMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 1510);
            log.LogMethodExit();
        }

        private async void CustomerExportUI_Shown(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            btnCancel.Enabled = false;
            customerSearchCriteria.OrderBy(CustomerSearchByParameters.CUSTOMER_ID);
            CustomerListBL customerListBL = new CustomerListBL(utilities.ExecutionContext);
            int totalNoOfCustomer = customerListBL.GetCustomerCount(customerSearchCriteria);
            
            if (totalNoOfCustomer > 10000)
            {
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1511));
                Close();
                return;
            }
            string fileName = "Customer Data " + System.DateTime.Now.ToString("dd-MMM-yyyy") + ".xls";
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            sfd.FileName = fileName;
            sfd.Filter = ".xls | *.xls";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                btnCancel.Enabled = true;
                cancellationTokenSource = new CancellationTokenSource();
                string error = await Task<string>.Factory.StartNew(() => { return ExportCustomerDetails(totalNoOfCustomer, sfd.FileName, cancellationTokenSource.Token); });
                if (string.IsNullOrWhiteSpace(error) == false)
                {
                    MessageBox.Show(error);
                }
            }
            Close();
            log.LogMethodExit();
        }

        private string ExportCustomerDetails(int totalNoOfCustomer, string file, CancellationToken token)
        {
            log.LogMethodEntry(totalNoOfCustomer);
            if (token.IsCancellationRequested)
            {
                return string.Empty;
            }
            int progressIncrement = 90 / totalNoOfCustomer;
            string message = string.Empty;
            int exportPageSize = 50;
            CustomerListBL customerListBL = new CustomerListBL(utilities.ExecutionContext);
            int totalNoOfPages = (totalNoOfCustomer / exportPageSize) + ((totalNoOfCustomer % exportPageSize) > 0 ? 1 : 0);
            List<CustomerDTO> customerDTOList = new List<CustomerDTO>();
            for (int i = 0; i < totalNoOfPages; i++)
            {
                customerSearchCriteria.Paginate(i, exportPageSize);
                var list = customerListBL.GetCustomerDTOList(customerSearchCriteria, true, true);
                if (list != null)
                {
                    customerDTOList.AddRange(list);
                }
                progressBar.Invoke((Action)(() => progressBar.Value = ((90 * (i + 1)) / totalNoOfPages)));
                if (token.IsCancellationRequested)
                {
                    return string.Empty;
                }
            }
            CustomerDTODefinition customerDTODefinition = new CustomerDTODefinition(utilities.ExecutionContext, "");
            foreach (var customerDTO in customerDTOList)
            {
                customerDTODefinition.Configure(customerDTO);
            }
            Sheet sheet = new Sheet();
            Row headerRow = new Row();
            customerDTODefinition.BuildHeaderRow(headerRow);
            sheet.AddRow(headerRow);
            foreach (var customerDTO in customerDTOList)
            {
                Row row = new Row();
                customerDTODefinition.Serialize(row, customerDTO);
                sheet.AddRow(row);
            }
            try
            {
                System.IO.FileStream fs = System.IO.File.Open(file, System.IO.FileMode.Create);
                fs.Close();
                System.IO.StreamWriter streamWriter = System.IO.File.AppendText(file);
                for (int i = 0; i < sheet.Rows.Count; i++)
                {
                    for (int j = 0; j < sheet.Rows[i].Cells.Count; j++)
                    {
                        streamWriter.Write(sheet.Rows[i].Cells[j].Value + '\t');
                    }
                    streamWriter.WriteLine();
                    progressBar.Invoke((Action)(() => progressBar.Value = ( 90 + ((10 * (i + 1)) / sheet.Rows.Count))));
                    if (token.IsCancellationRequested)
                    {
                        streamWriter.Close();
                        return string.Empty;
                    }
                }
                streamWriter.Close();
                System.Diagnostics.Process.Start("excel", "\"" + file + "\"");
            }
            catch (Exception ex)
            {
                log.Error("Error occured in Export To Excel Tab Delimited", ex);
                message = ex.Message;
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit(message);
            return message;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if(cancellationTokenSource != null)
                {
                    cancellationTokenSource.Cancel();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error ocuured while cancelling", ex);
            }
            log.LogMethodExit();
        }
    }
}
