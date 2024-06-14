/********************************************************************************************
 * Project Name - UploadFiles
 * Description  - Upload Files class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        14-Dec-2018      Raghuveera     Modified for getting encrypted key value 
 *2.70.2        13-Aug-2019      Deeksha        Added logger methods.
 ********************************************************************************************/
using Renci.SshNet;
using Semnox.Core.Utilities;
using System;
using System.Configuration;
using System.IO;
using Tamir.SharpSsh;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// Class for handling File Upload operations
    /// </summary>
    public class UploadFiles : IUploadFiles, IDisposable
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        Utilities Utilities;
        /// <summary>
        /// property to store sftp server url
        /// </summary>
        public string SFTPUrl { get; set; }
        /// <summary>
        /// property to store sftp server port number
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// property to store sftp server username
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// property to store sftp server password
        /// </summary>
        public string Password { get; set; }

        private SftpClient sftpClient;

        private bool disposed = false;

        /// <summary>
        /// Parameterized constructor to initialize parafait utilities
        /// </summary>
        public UploadFiles(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            Utilities = _Utilities;
            SFTPUrl = Utilities.getParafaitDefaults("SFTP_HOST_ADDRESS");
            UserName = Utilities.getParafaitDefaults("SFTP_USERNAME");
            Password = Utilities.getParafaitDefaults("SFTP_PASSWORD");//Utilities.getParafaitDefaults("SFTP_PASSWORD");
            Port = Convert.ToInt32(Utilities.getParafaitDefaults("SFTP_PORT_NUMBER"));
            if(string.IsNullOrEmpty(SFTPUrl) || string.IsNullOrEmpty(UserName))
            {
                throw new ValidationException("SFTP config values not found");
            }
            sftpClient = new SftpClient(SFTPUrl, Port,
                                        UserName, Password);
            sftpClient.Connect();
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to upload files to Merkle using SFTP file transfer
        /// </summary>
        /// <param name="fileName">the file which is to be uploaded</param>
        /// <returns>Return true on successfull upload, false on failure</returns>
        public bool SFTPFileTransfer(string fileName)
        {
            log.LogMethodEntry(fileName);
            bool Success = false;
            Sftp sftp = null;
            string sftpFolderPath = ConfigurationManager.AppSettings["SFTPServerFolder"];
            if(string.IsNullOrEmpty(sftpFolderPath))
            {
                log.Debug("sftpFolderPath is not specified in the connection string");
            }
            try
            {
                if (fileName.Length > 0)
                {

                    using (var fileStream = new FileStream(fileName, FileMode.Open))
                    {
                        sftpClient.UploadFile(fileStream, sftpFolderPath + Path.GetFileName(fileName));
                        Success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing SFTPFileTransfer()" + ex.Message);
                throw ;
            }
            finally
            {
                try { sftp.Close(); }
                catch (Exception ex)
                {
                    log.Error("Error occured while executing Close()" + ex.Message);
                }

                try { sftp = null; }
                catch (Exception ex)
                {
                    log.Error("Error occured " + ex.Message);
                }

                try { GC.Collect(); }
                catch (Exception ex)
                {
                    log.Error("Error occured while executing Collect()" + ex.Message);
                }

            }
            log.LogMethodExit(Success);
            return Success;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    sftpClient.Dispose();
                }

                // Note disposing has been done.
                disposed = true;
            }
        }

        ~UploadFiles()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(disposing: false) is optimal in terms of
            // readability and maintainability.
            Dispose(disposing: false);
        }
    }
}
