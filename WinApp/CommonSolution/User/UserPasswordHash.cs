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
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.User
{
    internal class UserPasswordHash : ByteArray
    {
        private static readonly Semnox.Parafait.logging.Logger log =
            new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public UserPasswordHash(string password, string passwordSalt, UserEncryptionKey userEncryptionKey)
            :base(CreateHash(password, passwordSalt, userEncryptionKey))
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        public UserPasswordHash(byte[] passwordHash)
            :base(passwordHash)
        {
            log.LogMethodEntry();
            if(passwordHash == null || passwordHash.Length == 0)
            {
                string message = "Invalid password hash";
                log.LogMethodExit("Throwing ParafaitApplicationException-" + message);
                throw new Semnox.Core.Utilities.ParafaitApplicationException(message);
            }
            log.LogMethodExit();
        }

        private static byte[] CreateHash(string password, string passwordSalt, UserEncryptionKey userEncryptionKey)
        {
            HMACSHA256 hmac = new HMACSHA256(userEncryptionKey);
            return hmac.ComputeHash(Encoding.UTF8.GetBytes(password + passwordSalt));
        }
    }
}
