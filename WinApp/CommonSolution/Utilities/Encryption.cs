/******************************************************************************************************************
 * Project Name - Encryption
 * Description  - Encryption class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *******************************************************************************************************************
 *1.00        14-Dec-2018      Raghuveera     Modified to get enrypted key and password values 
 *2.80        25-Feb-2020      Indrajeet K    Added Encrypt and Decrypt Method of return type Byte[] & getKey()
 *2.100.0     21-Sep-2020      Mathew Ninan   Added decrypt method to return byte[]. Common method doDecrypt to return
 *                                            byte[], which is then converted as per need in existing decrypt method
 *                                            to return string in UTF8 format
 ******************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace Semnox.Core.Utilities
{
    public static class Encryption
    {
        //  Call this function to remove the key from memory after use for security
        [System.Runtime.InteropServices.DllImport("KERNEL32.DLL", EntryPoint = "RtlZeroMemory")]
        public static extern bool ZeroMemory(IntPtr Destination, int Length);
        private static readonly ExecutionContext executionContext;

        // Function to Generate a 64 bits Key.
        public static string GenerateKey()
        {
            // Create an instance of Symetric Algorithm. Key and IV is generated automatically.
            DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();

            // Use the Automatically generated key for Encryption. 
            return ASCIIEncoding.ASCII.GetString(desCrypto.Key);
        }

        static string prepareKey()
        {

            string encryptionKey = "o1!esmXn";//GetParafaitKeys("ParafaitEncryption");//Cannot be get keys from db because this line need to be executed before establishing the data base connection 
            char[] key = new char[encryptionKey.Length];
            key[0] = encryptionKey[4];
            key[1] = encryptionKey[3];
            key[2] = encryptionKey[5];
            key[3] = encryptionKey[7];
            key[4] = encryptionKey[0];
            key[5] = encryptionKey[6];
            key[6] = encryptionKey[2];
            key[7] = encryptionKey[1];

            return new string(key);
        }

        public static string Encrypt(string plainText)
        {
            return Encrypt(plainText, prepareKey());
        }

        public static string Encrypt(byte[] plainText)
        {
            return doEncrypt(plainText, prepareKey());
        }

        public static async Task Encrypt(Stream plainTextStream, Stream encryptedStream)
        {
            await doEncrypt(plainTextStream, encryptedStream, prepareKey());
            return;
        }

        public static string Encrypt(string plainText, string sKey)
        {
            MemoryStream ms = new MemoryStream();
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            ICryptoTransform desencrypt = DES.CreateEncryptor();
            CryptoStream cryptostream = new CryptoStream(ms,
               desencrypt,
               CryptoStreamMode.Write);

            byte[] bytearrayinput = new byte[plainText.Length];
            bytearrayinput = ASCIIEncoding.ASCII.GetBytes(plainText);

            return doEncrypt(bytearrayinput, sKey);
        }

        public static byte[] getKey(string insert)
        {            
            string encryptionKey = Encryption.GetParafaitKeys("MifareCard");
            byte[] key = Encoding.UTF8.GetBytes(encryptionKey);
            byte[] insertBytes = Encoding.UTF8.GetBytes(insert.PadRight(4, 'X').Substring(0, 4));
            key[16] = insertBytes[0];
            key[17] = insertBytes[1];
            key[18] = insertBytes[2];
            key[19] = insertBytes[3];
          
            return key;
        }

        public static byte[] Encrypt(byte[] plainText, byte[] sKey)
        {
            using (AesCryptoServiceProvider AES = new AesCryptoServiceProvider())
            {
                AES.BlockSize = 128;
                AES.KeySize = 256;
                AES.Padding = PaddingMode.ANSIX923;
                AES.Key = sKey;
                AES.IV = Enumerable.Repeat((byte)0x00, 16).ToArray();
                using (ICryptoTransform AESencrypt = AES.CreateEncryptor())
                {
                    byte[] encryptedBytes = AESencrypt.TransformFinalBlock(plainText, 0, plainText.Length);
                    return (encryptedBytes);
                }
            }
        }

        public static byte[] Decrypt(byte[] encryptedText, byte[] sKey)
        {            
            try
            {
                using (AesCryptoServiceProvider AES = new AesCryptoServiceProvider())
                {
                    AES.BlockSize = 128;
                    AES.KeySize = 256;
                    AES.Padding = PaddingMode.ANSIX923;
                    AES.Key = sKey;
                    AES.IV = Enumerable.Repeat((byte)0x00, 16).ToArray();

                    using (ICryptoTransform AESdecrypt = AES.CreateDecryptor())
                    {
                        byte[] plainTextBytes = AESdecrypt.TransformFinalBlock(encryptedText, 0, encryptedText.Length);                        
                        return (plainTextBytes);
                    }
                }
            }
            catch (Exception ex)
            {
                return new byte[0];
            }
        }

        static string doEncrypt(byte[] bytearrayinput, string sKey)
        {
            MemoryStream ms = new MemoryStream();
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            ICryptoTransform desencrypt = DES.CreateEncryptor();
            CryptoStream cryptostream = new CryptoStream(ms,
               desencrypt,
               CryptoStreamMode.Write);

            cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
            cryptostream.FlushFinalBlock();
            byte[] encryptedBytes = ms.ToArray();
            ms.Close();
            cryptostream.Close();
            return (Convert.ToBase64String(encryptedBytes));
        }

        static async Task doEncrypt(Stream plainTextStream, Stream encryptedStream, string sKey)
        {
            using (DESCryptoServiceProvider DES = new DESCryptoServiceProvider())
            {
                DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                using (ICryptoTransform desencrypt = DES.CreateEncryptor())
                {
                    using (CryptoStream cryptostream = new CryptoStream(encryptedStream,
                   desencrypt,
                   CryptoStreamMode.Write))
                    {
                        await plainTextStream.CopyToAsync(cryptostream);
                    }
                }
            }
        }

        public static string Decrypt(string encryptedText)
        {
            return Decrypt(encryptedText, prepareKey());
        }

        public static byte[] Decrypt(byte[] encryptedBytes)
        {
            string encryptedText = Convert.ToBase64String(encryptedBytes);
            return dodecrypt(encryptedText, prepareKey());
        }

        public static async Task<bool> IsValidEncryptedStream(Stream encryptedStream)
        {
            bool result = false;
            try
            {
                using (Stream decryptedStream = await Decrypt(encryptedStream))
                {
                    await decryptedStream.CopyToAsync(Stream.Null);
                }
                result = true;
            }
            catch (Exception)
            {
            }
            return result;
        }

        public static async Task<Stream> Decrypt(Stream encryptedStream)
        {
            return await dodecrypt(encryptedStream, prepareKey());
        }

        public static string Decrypt(string encryptedText, string sKey)
        {
            try
            {
                //DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
                ////A 64 bit key and IV is required for this provider.
                ////Set secret key For DES algorithm.
                //DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                ////Set initialization vector.
                //DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

                //MemoryStream ms = new MemoryStream(Convert.FromBase64String(encryptedText));
                //ICryptoTransform desdecrypt = DES.CreateDecryptor();
                //CryptoStream cryptostreamDecr = new CryptoStream(ms, desdecrypt, CryptoStreamMode.Read);
                //byte[] plainTextBytes = new byte[encryptedText.Length];
                //cryptostreamDecr.Read(plainTextBytes, 0, plainTextBytes.Length);
                //ms.Close();
                //cryptostreamDecr.Close();
                byte[] plainTextBytes = dodecrypt(encryptedText, sKey);
                return (Encoding.UTF8.GetString(plainTextBytes).TrimEnd('\0'));
            }
            catch
            {
                return "";
            }
        }
        public static string GetParafaitKeys(string optionName)
        {
           return GetKeys("Parafait Keys", optionName);
        }
        public static string GetKeys(string optionType, string optionName)
        {
            string keyValue = string.Empty;
            SystemOptionsBL systemOptionsBL = new SystemOptionsBL(executionContext, optionType, optionName);
            if (systemOptionsBL.GetSystemOptionsDTO != null && systemOptionsBL.GetSystemOptionsDTO.OptionId > -1)
            {
                keyValue = Decrypt(systemOptionsBL.GetSystemOptionsDTO.OptionValue);
            }
            else
            {
                throw new Exception("Unable to find the key with name: " + optionName + ".");
            }
            return keyValue;
        }

        private static byte[] dodecrypt(string encryptedText, string sKey)
        {
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            //A 64 bit key and IV is required for this provider.
            //Set secret key For DES algorithm.
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            //Set initialization vector.
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

            MemoryStream ms = new MemoryStream(Convert.FromBase64String(encryptedText));
            ICryptoTransform desdecrypt = DES.CreateDecryptor();
            CryptoStream cryptostreamDecr = new CryptoStream(ms, desdecrypt, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[encryptedText.Length];
            cryptostreamDecr.Read(plainTextBytes, 0, plainTextBytes.Length);
            ms.Close();
            cryptostreamDecr.Close();
            return plainTextBytes;
        }

        private static async Task<Stream> dodecrypt(Stream encryptedStream, string sKey)
        {
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            ICryptoTransform desdecrypt = DES.CreateDecryptor();
            CryptoStream cryptostreamDecr = new CryptoStream(encryptedStream, desdecrypt, CryptoStreamMode.Read);
            return cryptostreamDecr;
        }

        //static GCHandle gch;
        public static void PinKeyToMemory(string secretKey)
        {
            //gch = GCHandle.Alloc(secretKey, GCHandleType.Pinned);
        }

        public static void RemoveKeyFromMemory()
        {
            //ZeroMemory(gch.AddrOfPinnedObject(), 8 * 2);
            //gch.Free();
        }
    }
}
