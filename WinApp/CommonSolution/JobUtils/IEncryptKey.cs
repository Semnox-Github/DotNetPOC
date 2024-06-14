using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// To handle the all the types of string encryptions
    /// </summary>
    public interface IEncryptKey
    {
        /// <summary>
        /// to encrypt the given string for different called types
        /// </summary>
        /// <param name="key">key to be encrypt</param>
        /// <returns>return encrypted string</returns>
        string EncryptKey(string key);
    }
}
