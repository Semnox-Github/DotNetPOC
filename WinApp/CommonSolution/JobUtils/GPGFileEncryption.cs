/********************************************************************************************
 * Project Name - JobUtils
 * Description  - GPGFile Encryption
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        13-Aug-2019      Deeksha        Added logger methods.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Diagnostics;
using System.IO;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// Classs implements IEncryptFile Interface, it handle all GPG Encnryption related tasks
    /// </summary>
    public class GPGFileEncryption : IEncryptFile
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities Utilities;
        string pgpPath = string.Empty;
        string keyName = string.Empty;

        /// <summary>
        /// The path to the PGP file can be changed (if needed) 
        /// or executed
        /// </summary>
        public string PgpPath
        {
            get { return pgpPath; }
            set { pgpPath = value; }
        }

        /// <summary>
        /// The location of the PubRing and SecRing files
        /// </summary>
        public string KeyName
        {
            get { return keyName; }
            set { keyName = value; }
        }
        /// <summary>
        /// Public constructor stores the home directory argument
        /// </summary>
        public GPGFileEncryption(Utilities _Utilities, string publicKeyName, string pgpExeFilePath)
        {
            log.LogMethodEntry(_Utilities, publicKeyName, pgpExeFilePath);
            Utilities = _Utilities;
            KeyName = publicKeyName;
            PgpPath = pgpExeFilePath;
            log.LogMethodExit();
        }

        /// <summary>
        /// method to encrypt file using Gpg encryption methedology
        /// </summary>
        /// <param name="fileStorePath">to find file and encrypt</param>
        /// <returns>Returns true when file encryption success, false on failure</returns>
        public bool EncryptFile(string fileStorePath)
        {
            //File info
            log.LogMethodEntry(fileStorePath);
            FileInfo fi = new FileInfo(fileStorePath);
            if (!fi.Exists)
            {
                throw new Exception("Missing file.  Cannot find the file to encrypt.");
            }

            string encryptedGpgFileName = fileStorePath + ".gpg";
            // Cannot encrypt a file if it already exists

            if (File.Exists(encryptedGpgFileName))
            {
                throw new Exception("Cannot encrypt file.  File already exists");
            }

            //Confirm the existence of the PGP software4            
            FileInfo figpg = new FileInfo(PgpPath);
            if (!File.Exists(PgpPath))
            {
                throw new Exception("Cannot find PGP software.");
            }

            //Turn off all windows for the process

            ProcessStartInfo s = new ProcessStartInfo(PgpPath);
            s.CreateNoWindow = false;
            s.UseShellExecute = false;
            s.RedirectStandardInput = true;
            s.RedirectStandardOutput = true;
            s.RedirectStandardError = true;
            s.WorkingDirectory = figpg.DirectoryName;
            s.RedirectStandardError = true;
            s.RedirectStandardOutput = true;

            //Build the encryption arguments
            string recipient = " -r \"" + keyName + "\"";
            string output = " -o \"" + encryptedGpgFileName + "\"";
            string encrypt = " -e \"" + fileStorePath + "\"";

            string cmd = recipient + output + encrypt;
            s.Arguments = @"" + cmd;
            //@"-r ""842B90E789D02D49A5ABE5F340C071004F2F3023"" -o ""C:\temp\new.txt.gpg"" -e ""C:\Users\Dell-Laptop\Desktop\vendorreturnTests.txt""";
            //Utilities.EventLog.logEvent("ParafaitDataTransfer", 'I', "GPGEncryption Cmd", PgpPath + "path : "+cmd.ToString(), "MerkleFileTransfer", 1, "", "", Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
            // Execute the process and wait for it to exit.  
            // NOTE: IF THE PROCESS CRASHES, it will freeze
            bool processExited = false;
            Process p = Process.Start(s);
            p.OutputDataReceived += P_OutputDataReceived;
            processExited = p.WaitForExit(3500);
            p.StandardInput.Flush();
            p.StandardInput.Close();
            p.Close();
            //using (Process p = Process.Start(s))
            //{               
            //    // Build the encryption arguments
            //    string recipient = " -r \"" + keyName + "\"";
            //    string output = " -o \"" + encryptedGpgFileName + "\"";
            //    string encrypt = " -e \"" + fileStorePath + "\"";
            //    string cmd = "gpg" + recipient + output + encrypt;
            //    p.StandardInput.WriteLine(cmd);
            //    p.StandardInput.Flush();
            //    p.StandardInput.Close();
            //    processExited = p.WaitForExit(3500);
            //    int i = p.ExitCode;
            //    p.Close();
            //}

            log.LogMethodExit(processExited);
            return processExited;
        }

        private void P_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
    }
}
