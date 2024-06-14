/********************************************************************************************
 * Project Name - Job Utils
 * Description  - MD5 Hash Encryption
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        13-Aug-2019      Deeksha        Added logger methods.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Security.Cryptography;
using System.Text;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// Class implements the MD5 hash encryption method
    /// </summary>
    public class MD5HashEncryption : IEncryptKey
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        Utilities Utilities;
        /// <summary>
        /// called encryption method
        /// </summary>
        /// <param name="_Utilities"></param>
        public MD5HashEncryption(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            Utilities = _Utilities;
            log.LogMethodExit();
        }
        /// <summary>
        /// To Encrypt the given key and returns the encrypted string using MD5 hash method
        /// </summary>
        /// <param name="key">key to be encrypt</param>
        /// <returns>retunrs encrypted string</returns>
        
        public string EncryptKey(string key)
        {
            log.LogMethodEntry("key");
            MD5CryptoServiceProvider MD5Code = new MD5CryptoServiceProvider();
            byte[] byteDizisi = Encoding.UTF8.GetBytes(key);
            byteDizisi = MD5Code.ComputeHash(byteDizisi);
            StringBuilder sb = new StringBuilder();
            foreach (byte ba in byteDizisi)
            {
                sb.Append(ba.ToString("x2").ToLower());
            }
            log.LogMethodExit(sb);
            return sb.ToString();
        }
    }
}
