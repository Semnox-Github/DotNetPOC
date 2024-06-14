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
using Semnox.Core.Utilities;
using Semnox.Parafait.Game;
using Semnox.Parafait.Languages;
using Semnox.Core.GenericUtilities.Excel;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// Account Export UI
    /// </summary>
    public partial class AccountGamePlayExportUI : Form
    {
        private Utilities utilities;
        private int accountId;
        private string tagNumber;
        private IList<GamePlayDTO> gamePlayDTOList;
        private GamePlayDTODefinition gamePlayDTODefinition;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        CancellationTokenSource cancellationTokenSource;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public AccountGamePlayExportUI(Utilities utilities, 
                                       int accountId, string tagNumber, 
                                       IList<GamePlayDTO> gamePlayDTOList,
                                       GamePlayDTODefinition gamePlayDTODefinition)
        {
            log.LogMethodEntry(utilities, accountId);
            InitializeComponent();
            this.utilities = utilities;
            this.accountId = accountId;
            this.tagNumber = tagNumber;
            this.gamePlayDTOList = gamePlayDTOList;
            this.gamePlayDTODefinition = gamePlayDTODefinition;
            progressBar.Value = 0;
            lblMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 1601);
            log.LogMethodExit();
        }

        private async void AccountExportUI_Shown(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            btnCancel.Enabled = false;
            string fileName = "Card Game Play Data " + tagNumber + " "+ System.DateTime.Now.ToString("dd-MMM-yyyy") + ".xls";
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            sfd.FileName = fileName;
            sfd.Filter = ".xls | *.xls";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                btnCancel.Enabled = true;
                cancellationTokenSource = new CancellationTokenSource();
                string error = await Task<string>.Factory.StartNew(() => { return ExportAccountActivityDetails(sfd.FileName, cancellationTokenSource.Token); });
                if (string.IsNullOrWhiteSpace(error) == false)
                {
                    MessageBox.Show(error);
                }
            }
            Close();
            log.LogMethodExit();
        }

        private string ExportAccountActivityDetails(string file, CancellationToken token)
        {
            log.LogMethodEntry(file);
            string message = string.Empty;
            if (token.IsCancellationRequested)
            {
                return string.Empty;
            }
            int totalNoOfAccountActivityRecords = gamePlayDTOList.Count;
            if (totalNoOfAccountActivityRecords > 0)
            {
                int progressIncrement = 100 / totalNoOfAccountActivityRecords;
                foreach (var gamePlayDTO in gamePlayDTOList)
                {
                    gamePlayDTODefinition.Configure(gamePlayDTO);
                }
                Sheet sheet = new Sheet();
                Row headerRow = new Row();
                gamePlayDTODefinition.BuildHeaderRow(headerRow);
                sheet.AddRow(headerRow);
                foreach (var gamePlayDTO in gamePlayDTOList)
                {
                    Row row = new Row();
                    gamePlayDTODefinition.Serialize(row, gamePlayDTO);
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
                        progressBar.Invoke((Action)(() => progressBar.Value = (((100 * (i + 1)) / sheet.Rows.Count))));
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
