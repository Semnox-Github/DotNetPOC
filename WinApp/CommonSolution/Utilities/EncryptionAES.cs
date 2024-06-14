using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.Utilities
{
    public static class EncryptionAES
    {
        public static byte[] Encrypt(byte[] plainText, byte[] sKey)
        {
            AesCryptoServiceProvider AES = new AesCryptoServiceProvider();
            AES.BlockSize = 128;
            AES.KeySize = 256;
            AES.Padding = PaddingMode.None;
            AES.Key = sKey;
            AES.IV = Enumerable.Repeat((byte)0x00, 16).ToArray();
            ICryptoTransform AESencrypt = AES.CreateEncryptor();
            byte[] encryptedBytes = AESencrypt.TransformFinalBlock(plainText, 0, plainText.Length);

            return (encryptedBytes);
        }

        public static byte[] Decrypt(byte[] encryptedText, byte[] sKey)
        {
            try
            {
                AesCryptoServiceProvider AES = new AesCryptoServiceProvider();
                AES.BlockSize = 128;
                AES.KeySize = 256;
                AES.Padding = PaddingMode.None;
                AES.Key = sKey;
                AES.IV = Enumerable.Repeat((byte)0x00, 16).ToArray();

                ICryptoTransform AESdecrypt = AES.CreateDecryptor();
                byte[] plainTextBytes = AESdecrypt.TransformFinalBlock(encryptedText, 0, encryptedText.Length);
                return (plainTextBytes);
            }
            catch
            {
                return new byte[0];
            }
        }
    }
}
