
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Id proof selection ui
    /// </summary>
    public partial class IdProofUI : Form
    {
        private Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ProfileBL profileBL;
        private MessageBoxDelegate messageBoxDelegate;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="utilities">parafait utilities</param>
        /// <param name="profileDTO">profile dto</param>
        /// <param name="messageBoxDelegate">message box delegate</param>
        public IdProofUI(Utilities utilities, ProfileDTO profileDTO, MessageBoxDelegate messageBoxDelegate)
        {
            log.LogMethodEntry(utilities, profileDTO, messageBoxDelegate);
            InitializeComponent();
            this.utilities = utilities;
            this.profileBL = new ProfileBL(utilities.ExecutionContext, profileDTO);
            this.messageBoxDelegate = messageBoxDelegate;
            log.LogMethodExit();
        }

        private void IdProofUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (string.IsNullOrWhiteSpace(profileBL.ProfileDTO.IdProofFileURL) == false)
            {
                ShowFileInBrowser();
            }
            log.LogMethodExit();
        }

        void ShowFileInBrowser()
        {
            log.LogMethodEntry();
            try
            {
                string temporaryFileName = profileBL.CopyIdProofFileToTempDirectory();
                if(string.IsNullOrWhiteSpace(temporaryFileName) == false)
                {
                    wbIdProof.Url = new Uri(temporaryFileName);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while copying the id proof file to temporary directory", ex);
                messageBoxDelegate(ex.Message, MessageContainerList.GetMessage(utilities.ExecutionContext, "Id Proof"), MessageBoxButtons.OK);
            }
            log.LogMethodExit();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string idFileName = System.IO.Path.GetFileName(ofd.FileName);
                    byte[] fileBytes = System.IO.File.ReadAllBytes(ofd.FileName);
                    profileBL.SaveIdProofFile(idFileName, fileBytes);
                    ShowFileInBrowser();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while saving the id proof file", ex);
                messageBoxDelegate(ex.Message, MessageContainerList.GetMessage(utilities.ExecutionContext, "Id Proof"));
            }
            log.LogMethodExit();
        }


        private void IdProofUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            wbIdProof.Url = null;
            DirectoryInfo di = new DirectoryInfo(System.IO.Path.GetTempPath());
            foreach (FileInfo fi in di.GetFiles("ProfileIdProof*"))
            {
                try
                {
                    fi.Delete();
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while deleting the temporary file: " + fi.FullName, ex);
                }
            }
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            log.LogMethodExit();
        }
    }
}
