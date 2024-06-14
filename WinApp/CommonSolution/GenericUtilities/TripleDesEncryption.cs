/********************************************************************************************
 * Project Name - GenericUtilities                                                                          
 * Description  - Triple DES Encryption algorithm used for ultralight c authentication
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 ********************************************************************************************* 
 *2.50.0      1-Jul-2019   Lakshminarayana      Created
 *2.70.2        12-Aug-2019  Deeksha              Added logger methods.
 ********************************************************************************************/
using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Encrypts the clear bytearray using Triple-des encryption
    /// </summary>
    public class TripleDesEncryption
    {
        private static readonly Semnox.Parafait.logging.Logger log =
            new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ByteArray key;

        public TripleDesEncryption(ByteArray key)
        {
            log.LogMethodEntry("key");
            if (key == null)
            {
                string errorMessage = "Invalid key. key should not be null.";
                log.LogMethodExit(null, "Throwing ArgumentException -" + errorMessage);
                throw new ArgumentException(errorMessage);
            }
            this.key = key;
            log.LogMethodExit();
        }

        /// <summary>
        /// Encrypts the clear byte array
        /// </summary>
        /// <param name="clearByteArray"></param>
        /// <param name="initializationVector"></param>
        /// <returns></returns>
        public ByteArray Encrypt(ByteArray clearByteArray, ByteArray initializationVector)
        {
            log.LogMethodEntry(clearByteArray, initializationVector);
            if (clearByteArray == null)
            {
                string errorMessage = "Invalid clear byte array. clear byte array vector should not be null.";
                log.LogMethodExit(null, "Throwing ArgumentException -" + errorMessage);
                throw new ArgumentException(errorMessage);
            }
            if (initializationVector == null)
            {
                string errorMessage = "Invalid key. Initialization vector should not be null.";
                log.LogMethodExit(null, "Throwing ArgumentException -" + errorMessage);
                throw new ArgumentException(errorMessage);
            }
            ByteArray returnValue = EncryptByteArray(clearByteArray, initializationVector);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Decrypts the encrypted byte array
        /// </summary>
        /// <param name="encryptedByteArray"></param>
        /// <param name="initializationVector"></param>
        /// <returns></returns>
        public ByteArray Decrypt(ByteArray encryptedByteArray, ByteArray initializationVector)
        {
            log.LogMethodEntry(encryptedByteArray, initializationVector);
            if (encryptedByteArray == null)
            {
                string errorMessage = "Invalid encrypted byte array. encrypted byte array vector should not be null.";
                log.LogMethodExit(null, "Throwing ArgumentException -" + errorMessage);
                throw new ArgumentException(errorMessage);
            }
            if (initializationVector == null)
            {
                string errorMessage = "Invalid key. Initialization vector should not be null.";
                log.LogMethodExit(null, "Throwing ArgumentException -" + errorMessage);
                throw new ArgumentException(errorMessage);
            }
            ByteArray returnvalue = DecryptByteArray(encryptedByteArray, initializationVector);
            log.LogMethodExit(returnvalue);
            return returnvalue;
        }

        private ByteArray EncryptByteArray(ByteArray clearByteArray, ByteArray initializationVector)
        {
            log.LogMethodEntry(clearByteArray, initializationVector);
            ByteArray encryptedByteArray;
            // Create a MemoryStream.
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Create a CryptoStream using the MemoryStream 
                // and the pass key and initialization vector (IV).
                using (TripleDESCryptoServiceProvider tripleDesCryptoServiceProvider =
                    new TripleDESCryptoServiceProvider { Padding = PaddingMode.None })
                {
                    ICryptoTransform cryptoTransform = TripleDES.IsWeakKey(key.Value)
                        ? tripleDesCryptoServiceProvider.CreateWeakEncryptor(key.Value, initializationVector.Value)
                        : tripleDesCryptoServiceProvider.CreateEncryptor(key.Value, initializationVector.Value);
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
                    {
                        // Write the byte array to the crypto stream and flush it.
                        cryptoStream.Write(clearByteArray.Value, 0, clearByteArray.Length);
                        cryptoStream.FlushFinalBlock();

                        // Get an bytearray from the 
                        // MemoryStream that holds the 
                        // encrypted data.
                        encryptedByteArray = new ByteArray(memoryStream.ToArray());
                    }
                }
            }
            log.LogMethodExit(encryptedByteArray);
            return encryptedByteArray;
        }
        private ByteArray DecryptByteArray(ByteArray encryptedByteArray, ByteArray initializationVector)
        {
            log.LogMethodEntry(encryptedByteArray, initializationVector);
            ByteArray clearByteArray;
            // Create a new MemoryStream using the passed 
            // encryptedByteArray.
            using (MemoryStream memoryStream = new MemoryStream(encryptedByteArray.Value))
            {
                using (TripleDESCryptoServiceProvider tripleDesCryptoServiceProvider =
                    new TripleDESCryptoServiceProvider { Padding = PaddingMode.None })
                {
                    ICryptoTransform cryptoTransform = TripleDES.IsWeakKey(key.Value)
                        ? tripleDesCryptoServiceProvider.CreateWeakDecryptor(key.Value, initializationVector.Value)
                        : tripleDesCryptoServiceProvider.CreateDecryptor(key.Value, initializationVector.Value);
                    // Create a CryptoStream using the MemoryStream 
                    // and the passed key and initialization vector (IV).
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Read))
                    {
                        // Create buffer to hold the decrypted data.
                        byte[] buffer = new byte[encryptedByteArray.Length];

                        // Read the decrypted data out of the crypto stream
                        // and place it into the temporary buffer.
                        cryptoStream.Read(buffer, 0, buffer.Length);

                        //Convert the buffer into a bytearray and return it.
                        clearByteArray = new ByteArray(buffer);
                    }
                }
            }

            log.LogMethodExit(clearByteArray);
            return clearByteArray;
        }
    }

    public static class TripleDesCryptoServiceProviderExtensions
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static ICryptoTransform CreateWeakEncryptor(this TripleDESCryptoServiceProvider cryptoProvider, byte[] key, byte[] iv)
        {
            log.LogMethodEntry(cryptoProvider,"key",iv);
            // reflective way of doing what CreateEncryptor() does, bypassing the check for weak keys
            MethodInfo methodInfo = cryptoProvider.GetType().GetMethod("_NewEncryptor", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] objectArray = { key, cryptoProvider.Mode, iv, cryptoProvider.FeedbackSize, 0 };
            ICryptoTransform cyrptoTransform = methodInfo.Invoke(cryptoProvider, objectArray) as ICryptoTransform;
            log.LogMethodExit(cyrptoTransform);
            return cyrptoTransform;
        }

        public static ICryptoTransform CreateWeakEncryptor(this TripleDESCryptoServiceProvider cryptoProvider)
        {
            log.LogMethodEntry(cryptoProvider);
            ICryptoTransform returnValue = CreateWeakEncryptor(cryptoProvider, cryptoProvider.Key, cryptoProvider.IV);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public static ICryptoTransform CreateWeakDecryptor(this TripleDESCryptoServiceProvider cryptoProvider, byte[] key, byte[] iv)
        {
            log.LogMethodEntry(cryptoProvider,"key",iv);
            // reflective way of doing what CreateEncryptor() does, bypassing the check for weak keys
            MethodInfo methodInfo = cryptoProvider.GetType().GetMethod("_NewEncryptor", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] objectArray = { key, cryptoProvider.Mode, iv, cryptoProvider.FeedbackSize, 1 };
            ICryptoTransform cryptoTransform = methodInfo.Invoke(cryptoProvider, objectArray) as ICryptoTransform;
            log.LogMethodExit(cryptoTransform);
            return cryptoTransform;
        }

        public static ICryptoTransform CreateWeakDecryptor(this TripleDESCryptoServiceProvider cryptoProvider)
        {
            log.LogMethodEntry(cryptoProvider);
            ICryptoTransform returnValue = CreateWeakDecryptor(cryptoProvider, cryptoProvider.Key, cryptoProvider.IV);
            log.LogMethodExit(returnValue);
            return returnValue;
        }
    }

}
