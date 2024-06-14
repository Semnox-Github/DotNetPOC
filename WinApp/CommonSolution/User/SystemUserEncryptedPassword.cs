/********************************************************************************************
 * Project Name - User
 * Description  - Represents a password hash
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.110.0        1-Jul-2019      Lakshminarayana     Created : POS Redesign 
 ********************************************************************************************/
using System;
using System.Text;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.User
{
    public class SystemUserEncryptedPassword : ByteArray
    {
        private static readonly Semnox.Parafait.logging.Logger log =
            new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SystemUserEncryptedPassword(string plainTextPassword, string machineName)
            :base(Encrypt(plainTextPassword, new SystemUserEncryptionKey(machineName)))
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        public SystemUserEncryptedPassword(string encryptedPasswordBase64String)
            :base(Convert.FromBase64String(encryptedPasswordBase64String))
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        private static byte[] Encrypt(string plainTextpassword, SystemUserEncryptionKey systemUserEncryptionKey)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainTextpassword.PadRight(16, ' '));
            byte[] encryptedBytes = Semnox.Core.Utilities.EncryptionAES.Encrypt(plainTextBytes, systemUserEncryptionKey.Value);
            return (encryptedBytes);
        }

        public byte[] GetPlainTextPasswordBytes(SystemUserEncryptionKey systemUserEncryptionKey)
        {
            return Semnox.Core.Utilities.EncryptionAES.Decrypt(Value, systemUserEncryptionKey.Value);
        }

        public string GetPlainTextPassword(string machineName)
        {
            return Encoding.UTF8.GetString(GetPlainTextPasswordBytes(new SystemUserEncryptionKey(machineName))).TrimEnd();;
        }

        public string GetEncryptedBase64Password()
        {
            return Convert.ToBase64String(Value);
        }
    }
}
