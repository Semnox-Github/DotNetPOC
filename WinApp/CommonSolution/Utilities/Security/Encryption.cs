using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

namespace Semnox.Core.Utilities
{

    public static class EncryptionHASH
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static byte[] CreateHash(string plainText, byte[] sKey)
        {
            log.LogMethodEntry();
            HMACSHA256 hmac = new HMACSHA256(sKey);
            log.LogMethodExit("plain text");
            return hmac.ComputeHash(Encoding.UTF8.GetBytes(plainText));
        }

        public static bool CompareHash(byte[] Hash1, byte[] Hash2)
        {
            log.LogMethodEntry();
            if (Hash1.Length != Hash2.Length)
            {
                log.LogMethodExit(false);
                return false;
            }
               

            for (int i = 0; i < Hash1.Length; i++)
            {
                if (Hash1[i] != Hash2[i])
                {
                    log.LogMethodExit(false);
                    return false;
                }
                   
            }
            log.LogMethodExit(true);
            return true;
        }
    }
}

