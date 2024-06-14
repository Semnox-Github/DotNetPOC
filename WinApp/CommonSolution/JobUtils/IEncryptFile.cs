using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// Interface is contains all file encryption methods and members
    /// </summary>
    public interface IEncryptFile
    {
        /// <summary>
        /// Method to encrypt the files
        /// </summary>
        /// <param name="path">read files from Path and encrypt file</param>
        /// <returns>Returns the status true when success and false on failure</returns>
        bool EncryptFile(string path);
    }
}
