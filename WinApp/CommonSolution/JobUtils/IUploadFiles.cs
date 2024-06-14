using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// Interface is to handle all upload  files activities
    /// </summary>
    public interface IUploadFiles
    {
        /// <summary>
        /// property to store SFTP url 
        /// </summary>
        string SFTPUrl { get; set; }
        /// <summary>
        /// perperty to store port number 
        /// </summary>
        int Port { get; set; }
        /// <summary>
        /// property to store username of sftp server credentials
        /// </summary>
        string UserName { get; set; }
        /// <summary>
        /// property to store password of sftp server credentials
        /// </summary>
        string Password { get; set; }

        /// <summary>
        /// Member method to send files  to server
        /// </summary>
        bool SFTPFileTransfer(string fileName);
    }
}
