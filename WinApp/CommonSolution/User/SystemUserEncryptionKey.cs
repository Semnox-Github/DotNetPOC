/********************************************************************************************
 * Project Name - User
 * Description  - Represents a encryption key used for encrypting user details
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.110.0        1-Jul-2019      Lakshminarayana     Created : POS Redesign 
 ********************************************************************************************/
using System.Text;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.User
{
    public class SystemUserEncryptionKey : ByteArray
    {
        private static readonly Semnox.Parafait.logging.Logger log =
            new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SystemUserEncryptionKey(string insert) : base(GenerateEncryptionKey(insert))
        {
            log.LogMethodEntry("insert");
            log.LogMethodExit();
        }

        public SystemUserEncryptionKey( byte[] keyBytes) : base(keyBytes)
        {
            log.LogMethodEntry("keyBytes");
            log.LogMethodExit();
        }

        protected static byte[] GenerateEncryptionKey(string insert, string baseKey = "46A97988SEMNOX!1CCCC9D1C581D86EE")
        {
            log.LogMethodEntry("insert");
            byte[] key = Encoding.UTF8.GetBytes(baseKey);
            byte[] insertBytes = Encoding.UTF8.GetBytes(insert.PadRight(4, 'X').Substring(0, 4));
            key[16] = insertBytes[0];
            key[17] = insertBytes[1];
            key[18] = insertBytes[2];
            key[19] = insertBytes[3];
            log.LogMethodExit("key");
            return key;
        }
        
    }
}
