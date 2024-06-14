/********************************************************************************************
 * Project Name - Initialize
 * Description  - Initialize class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        14-Dec-2018      Raghuveera     Modified for getting encrypted key value 
 *2.70.2        13-Aug-2019      Deeksha        Added logger methods.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Configuration;
using System.IO;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// Class for Initializing public variable which are used in File Transfer Process
    /// </summary>
    public class Initialize
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities Utilities;
        #region publicVariables
        /// <summary>
        /// Property to check whether Merkle File Transfer Enabled
        /// </summary>
        private bool isMerkleFileTransferEnabled;
        /// <summary>
        /// Property to hold sftp host address
        /// </summary>
        private static string sftpHostAddress;
        /// <summary>
        /// Property to hold sftp server username
        /// </summary>
        private string sftpUserName;
        /// <summary>
        /// Property to hold sftp server password
        /// </summary>
        private string sftpPassword;
        /// <summary>
        /// Property to hold service scheduled time
        /// </summary>
        private DateTime serviceStartAt;
        /// <summary>
        /// Property to hold temo file stor path
        /// </summary>
        private string fileStorePath;
        /// <summary>
        /// Property to hold sftp port number
        /// </summary>
        private int sftpPortNumber;
        /// <summary>
        /// Property to hold encryption key 
        /// </summary>
        private string encryptionKey;
        /// <summary>
        /// Property to hold gpg exe file store path 
        /// </summary>
        private string gPGFileLocation;

        /// <summary>
        /// Property to hold backup file store path 
        /// </summary>
        private string historyFolderPath;

        /// <summary>
        /// Property to hold backup file store path 
        /// </summary>
        private string recipientEmail;

        /// <summary>
        /// Property to check whether Merkle File Transfer Enabled
        /// </summary>
        public bool IsMerkleFileTransferEnabled
        {
            get
            {
                return isMerkleFileTransferEnabled;
            }

            set
            {
                isMerkleFileTransferEnabled = value;
            }
        }
        /// <summary>
        /// Property to hold gpg exe file store path 
        /// </summary>
        public string GPGFileLocation
        {
            get
            {
                return gPGFileLocation;
            }

            set
            {
                gPGFileLocation = value;
            }
        }
        /// <summary>
        /// Property to hold encryption key 
        /// </summary>
        public string EncryptionKey
        {
            get
            {
                return encryptionKey;
            }

            set
            {
                encryptionKey = value;
            }
        }
        /// <summary>
        /// Property to hold sftp port number
        /// </summary>
        public int SftpPortNumber
        {
            get
            {
                return sftpPortNumber;
            }

            set
            {
                sftpPortNumber = value;
            }
        }
        /// <summary>
        /// Property to hold temo file stor path
        /// </summary>
        public string FileStorePath
        {
            get
            {
                return fileStorePath;
            }

            set
            {
                fileStorePath = value;
            }
        }
        /// <summary>
        /// Property to hold service scheduled time
        /// </summary>
        public DateTime ServiceStartAt
        {
            get
            {
                return serviceStartAt;
            }

            set
            {
                serviceStartAt = value;
            }
        }
        /// <summary>
        /// Property to hold sftp server password
        /// </summary>
        public string SftpPassword
        {
            get
            {
                return sftpPassword;
            }

            set
            {
                sftpPassword = value;
            }
        }
        /// <summary>
        /// Property to hold sftp server username
        /// </summary>
        public string SftpUserName
        {
            get
            {
                return sftpUserName;
            }

            set
            {
                sftpUserName = value;
            }
        }
        /// <summary>
        /// Property to hold sftp host address
        /// </summary>
        public static string SftpHostAddress
        {
            get
            {
                return sftpHostAddress;
            }

            set
            {
                sftpHostAddress = value;
            }
        }
        /// <summary>
        /// Property to hold backup file store path 
        /// </summary>
        public string HistoryFolderPath
        {
            get
            {
                return historyFolderPath;
            }

            set
            {
                historyFolderPath = value;
            }
        }

        /// <summary>
        /// to email id for sending status email
        /// </summary>
        public string RecipientEmail
        {
            get
            {
                return recipientEmail;
            }

            set
            {
                recipientEmail = value;
            }
        }

        #endregion

        /// <summary>
        /// Parameterized constructor to initialize the default variables
        /// </summary>
        /// <param name="_utilities">parafait utilities </param>
        public Initialize(Utilities _utilities)
        {
            log.LogMethodEntry(_utilities);
            Utilities = _utilities;
            IsMerkleFileTransferEnabled = (Utilities.getParafaitDefaults("ENABLE_MERKLE_INTEGRATION") == "Y");
            SftpHostAddress = Utilities.getParafaitDefaults("SFTP_HOST_ADDRESS");
            SftpUserName = Utilities.getParafaitDefaults("SFTP_USERNAME");
            SftpPassword = ParafaitDefaultContainerList.GetParafaitDefault(_utilities.ExecutionContext, "SFTP_PASSWORD");//Utilities.getParafaitDefaults("SFTP_PASSWORD");

            FileStorePath = Utilities.getParafaitDefaults("TEMP_FILE_STORE_PATH");
            HistoryFolderPath = Utilities.getParafaitDefaults("TEMP_FILE_STORE_PATH");

            if (!string.IsNullOrEmpty(Utilities.getParafaitDefaults("SFTP_PORT_NUMBER")))
            {
                SftpPortNumber = Convert.ToInt32(Utilities.getParafaitDefaults("SFTP_PORT_NUMBER"));
            }
                        
            if (!string.IsNullOrEmpty(FileStorePath))
                FileStorePath = FileStorePath +"\\"+ ConfigurationManager.AppSettings["TempFilesFolderName"].ToString();


            RecipientEmail = ConfigurationManager.AppSettings["RecipientEmail"].ToString();

            //first run create Temp directory in file store path
            if (!Directory.Exists(FileStorePath))
                Directory.CreateDirectory(FileStorePath);

            FileStorePath = FileStorePath + "\\";

            if (!string.IsNullOrEmpty(HistoryFolderPath))
                HistoryFolderPath = HistoryFolderPath + "\\" + ConfigurationManager.AppSettings["HistoryFilesFolderName"].ToString();

            //first run create backup files directory in file store path
            if (!Directory.Exists(HistoryFolderPath))
                Directory.CreateDirectory(HistoryFolderPath);

            HistoryFolderPath = HistoryFolderPath + "\\";

            //GPG Encryption related properties
            EncryptionKey = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "WAIVER_ENCRYPTION_KEY");//Utilities.getParafaitDefaults("ENCRYPTION_KEY");
            GPGFileLocation = ConfigurationManager.AppSettings["GPGExeFilePath"];
            log.LogMethodExit();
        }
    }
}
