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
using Semnox.Core.GenericUtilities;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Core.GenericUtilities.Excel;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// Account Export UI
    /// </summary>
    public partial class AccountExportUI : Form
    {
        private Utilities utilities;
        private AccountSearchCriteria accountSearchCriteria;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        CancellationTokenSource cancellationTokenSource;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public AccountExportUI(Utilities utilities, AccountSearchCriteria accountSearchCriteria)
        {
            log.LogMethodEntry(utilities, accountSearchCriteria);
            InitializeComponent();
            this.utilities = utilities;
            this.accountSearchCriteria = accountSearchCriteria;
            progressBar.Value = 0;
            lblMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 1592);
            ThemeUtils.SetupVisuals(this);
            log.LogMethodExit();
        }

        private async void AccountExportUI_Shown(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            btnCancel.Enabled = false;
            accountSearchCriteria.OrderBy(AccountDTO.SearchByParameters.CUSTOMER_ID);
            AccountListBL accountListBL = new AccountListBL(utilities.ExecutionContext);
            int totalNoOfAccount = accountListBL.GetAccountCount(accountSearchCriteria);
            
            if (totalNoOfAccount > 10000)
            {
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1511));
                Close();
                return;
            }
            string fileName = "Card Data " + System.DateTime.Now.ToString("dd-MMM-yyyy") + ".xls";
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            sfd.FileName = fileName;
            sfd.Filter = ".xls | *.xls";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                btnCancel.Enabled = true;
                cancellationTokenSource = new CancellationTokenSource();
                string error = await Task<string>.Factory.StartNew(() => { return ExportAccountDetails(totalNoOfAccount, sfd.FileName, cancellationTokenSource.Token); });
                if (string.IsNullOrWhiteSpace(error) == false)
                {
                    MessageBox.Show(error);
                }
            }
            Close();
            log.LogMethodExit();
        }

        private string ExportAccountDetails(int totalNoOfAccount, string file, CancellationToken token)
        {
            log.LogMethodEntry(totalNoOfAccount);
            if (token.IsCancellationRequested)
            {
                return string.Empty;
            }
            int progressIncrement = 90 / totalNoOfAccount;
            string message = string.Empty;
            int exportPageSize = 50;
            AccountListBL accountListBL = new AccountListBL(utilities.ExecutionContext);
            int totalNoOfPages = (totalNoOfAccount / exportPageSize) + ((totalNoOfAccount % exportPageSize) > 0 ? 1 : 0);
            List<AccountDTO> accountDTOList = new List<AccountDTO>();
            for (int i = 0; i < totalNoOfPages; i++)
            {
                accountSearchCriteria.Paginate(i, exportPageSize);
                var list = accountListBL.GetAccountDTOList(accountSearchCriteria, true, true);
                if (list != null)
                {
                    accountDTOList.AddRange(list);
                }
                progressBar.Invoke((Action)(() => progressBar.Value = ((90 * (i + 1)) / totalNoOfPages)));
                if (token.IsCancellationRequested)
                {
                    return string.Empty;
                }
            }
            AccountDTODefinition accountDTODefinition = new AccountDTODefinition(utilities.ExecutionContext, "");
            foreach (var accountDTO in accountDTOList)
            {
                accountDTODefinition.Configure(accountDTO);
            }
            Sheet sheet = new Sheet();
            Row headerRow = new Row();
            accountDTODefinition.BuildHeaderRow(headerRow);
            sheet.AddRow(headerRow);
            foreach (var accountDTO in accountDTOList)
            {
                Row row = new Row();
                accountDTODefinition.Serialize(row, accountDTO);
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
