/********************************************************************************************
 * Project Name - GenericUtilities                                                                          
 * Description  - Represents a mifare ultra light c card key used for authentication
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 ********************************************************************************************* 
 *2.50.0      1-Jul-2019   Lakshminarayana      Created
 ********************************************************************************************/

using System;
using System.Collections.Generic;

namespace Semnox.Core.GenericUtilities
{
    public class UlcKey : ByteArray
    {
        private static readonly Semnox.Parafait.logging.Logger log =
            new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ByteArray translatedKey;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="hexString"></param>
        public UlcKey(string hexString)
            : base(hexString)
        {
            log.LogMethodEntry();
            if (IsValidKeyBytes(Value) == false)
            {
                string errorMessage = "Invalid ultralight c key. key should be of size 16.";
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ArgumentException(errorMessage);
            }

            translatedKey = GetTranslatedKey();
            log.LogMethodExit();
        }

        public UlcKey(byte[] bytes) : base(bytes)
        {
            log.LogMethodEntry();
            if (IsValidKeyBytes(Value) == false)
            {
                string errorMessage = "Invalid ultralight c key. key should be of size 16.";
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ArgumentException(errorMessage);
            }

            translatedKey = GetTranslatedKey();
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the byte array to be used as key for 3des authentication
        /// </summary>
        /// <returns></returns>
        private ByteArray GetTranslatedKey()
        {
            log.LogMethodEntry();
            List<ByteArray> reversedKeyParts = new List<ByteArray>();
            for (int i = 0; i < 4; i++)
            {
                ByteArray keyPart = SubArray(i * 4, 4);
                ByteArray reversedKeyPart = keyPart.Reverse();
                reversedKeyParts.Add(reversedKeyPart);
            }

            List<ByteArray> translatedByteArrayList = new List<ByteArray>
            {
                reversedKeyParts[1], reversedKeyParts[0], reversedKeyParts[3], reversedKeyParts[2]
            };
            byte[] resultBytes = new byte[16];
            for (int i = 0; i < translatedByteArrayList.Count; i++)
            {
                ByteArray translatedByteArray = translatedByteArrayList[i];
                for (int j = 0; j < translatedByteArray.Length; j++)
                {
                    resultBytes[i * 4 + j] = translatedByteArray[j];
                }
            }

            ByteArray result = new ByteArray(resultBytes);
            log.LogMethodExit();
            return result;
        }

        /// <summary>
        /// Returns the key to be used as key for 3des authentication
        /// </summary>
        public ByteArray TranslatedKey
        {
            get { return translatedKey; }
        }

        public static bool IsValidKeyString(string keyHexString)
        {
            log.LogMethodEntry();
            if (IsValidHexString(keyHexString) == false)
            {
                log.LogMethodExit(false, "invalid key string");
                return false;
            }

            string keyStringWithoutSpecialCharacters = RemoveSpecialCharacters(keyHexString);
            byte[] keyByteArray = GetByteArrayFromHexString(keyStringWithoutSpecialCharacters);
            bool result = IsValidKeyBytes(keyByteArray);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Validates the Ultralight c key
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static bool IsValidKeyBytes(byte[] bytes)
        {
            log.LogMethodEntry();
            if (bytes == null)
            {
                log.LogMethodExit(false, "empty byte array");
                return false;
            }

            if (bytes.Length != 16)
            {
                log.LogMethodExit(false,
                    "byte array length is :" + bytes.Length + " ultralight key should be of length 16.");
                return false;
            }

            log.LogMethodExit(true);
            return true;
        }
    }
}