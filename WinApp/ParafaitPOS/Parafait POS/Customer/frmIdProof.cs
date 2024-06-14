/********************************************************************************************
 * Project Name - Parafait_POS Class
 * Description  -  A class for frmIdProof
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
*2.80         20-Aug-2019     Girish Kundar   Modified : Added Logger methods and Removed unused namespace's 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace Parafait_POS
{
    public partial class frmIdProof : Form
    {
        public object _IDProofFileName = "";
        //Begin: Modified Added for logger function on 08-Mar-2016
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //End: Modified Added for logger function on 08-Mar-2016

        public frmIdProof(object IDProofFileName)
        {
            //Begin: Added to Configure the logger root LogLevel using App.config on 08-March-2016           
            //log = ParafaitUtils.Logger.setLogFileAppenderName(log);
            Logger.setRootLogLevel(log);
            //End: Added to Configure the logger root LogLevel using App.config on 08-March-2016

            log.LogMethodEntry(IDProofFileName);//Added for logger function on 08-Mar-2016
            InitializeComponent();

            _IDProofFileName = IDProofFileName;
            log.LogMethodExit();
        }

        private void frmIdProof_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (_IDProofFileName != null && !string.IsNullOrEmpty(_IDProofFileName.ToString()))
            {
                showFileInBrowser(_IDProofFileName.ToString());
            }
            log.LogMethodExit();
        }

        void showFileInBrowser(string fileName)
        {
            log.LogMethodEntry(fileName);//Added for logger function on 08-Mar-2016
            SqlCommand cmdImage = POSStatic.Utilities.getCommand();
            cmdImage.CommandText = "exec ReadBinaryDataFromFile @FileName";

            cmdImage.Parameters.AddWithValue("@FileName", POSStatic.Utilities.getParafaitDefaults("IMAGE_DIRECTORY") + "\\" + fileName.ToString());
            try
            {
                object o = cmdImage.ExecuteScalar();
                if (o == null || o == DBNull.Value)
                    return;

                byte[] bytes = o as byte[];
                if (bytes != null)
                {
                    string extension = fileName.ToString();
                    try
                    {
                        extension = (new System.IO.FileInfo(extension)).Extension;
                    }
                    catch { }
                    string tempFile = System.IO.Path.GetTempPath() + "ParafaitPOSCustIdProof" + Guid.NewGuid().ToString() + extension;
                    using (System.IO.FileStream file = new System.IO.FileStream(tempFile, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                    {
                        file.Write(bytes, 0, bytes.Length);
                    }

                    wbIdProof.Url = new Uri(tempFile);
                }
            }
            catch (Exception ex)
            {
                POSUtils.ParafaitMessageBox(ex.Message);
                log.Fatal("Ends-showFileInBrowser(" + fileName + ") due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
            }
            log.LogMethodExit();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string idFileName = System.IO.Path.GetFileName(ofd.FileName);
                    byte[] fileBytes = System.IO.File.ReadAllBytes(ofd.FileName);
                    SqlCommand cmd = POSStatic.Utilities.getCommand();
                    cmd.CommandText = "exec SaveBinaryDataToFile @bytes, @FileName";
                    cmd.Parameters.AddWithValue("@bytes", fileBytes);
                    cmd.Parameters.AddWithValue("@FileName", POSStatic.Utilities.getParafaitDefaults("IMAGE_DIRECTORY") + "\\" + idFileName);
                    cmd.ExecuteNonQuery();

                    _IDProofFileName = idFileName;

                    showFileInBrowser(idFileName);
                }
            }
            catch (Exception ex)
            {
                POSUtils.ParafaitMessageBox(ex.Message);
                log.Fatal("Ends-btnBrowse_Click() due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
            }
            log.LogMethodExit();
        }

        private void frmIdProof_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            wbIdProof.Url = null;
            DirectoryInfo di = new DirectoryInfo(System.IO.Path.GetTempPath());
            foreach (FileInfo fi in di.GetFiles("ParafaitPOSCustIdProof*"))
            {
                try
                {
                    fi.Delete();
                }
                catch
                {
                    log.Fatal("Ends-btnBrowse_Click() due to exception in fi.Delete ");//Added for logger function on 08-Mar-2016
                }
            }
            log.LogMethodExit();
        }
    }
}
