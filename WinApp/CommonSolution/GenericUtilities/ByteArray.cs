/********************************************************************************************
 * Project Name - GenericUtilities
 * Description  - Represents a byte array
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        1-Jul-2019      Lakshminarayana     Created 
 *2.70.2        09-Aug-2019     Deeksha             Modified logger methods.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Represents a byte array
    /// </summary>
    public class ByteArray : ValueObject
    {
        private static readonly Semnox.Parafait.logging.Logger log =
            new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly byte[] value;

        /// <summary>
        /// Parameterized constructor 
        /// </summary>
        /// <param name="hexString">hexString string</param>
        public ByteArray(string hexString)
        {
            log.LogMethodEntry(hexString);
            if (IsValidHexString(hexString) == false)
            {
                string errorMessage = "Hex string is not valid.";
                log.LogMethodExit(null, "Throwing ArgumentException - " + errorMessage);
                throw new ArgumentException(errorMessage);
            }
            string hexStringWithoutSpecialCharacters = RemoveSpecialCharacters(hexString);
            value = GetByteArrayFromHexString(hexStringWithoutSpecialCharacters);
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="value">byte array</param>
        public ByteArray(byte[] value)
        {
            log.LogMethodEntry(value);
            if (value == null)
            {
                string errorMessage = "value is empty.";
                log.LogMethodExit(null, "Throwing ArgumentException -" + errorMessage);
                throw new ArgumentException(errorMessage);
            }

            this.value = value;
            log.LogMethodExit();
        }

        public byte this[int i]
        {
            get { return value[i]; }
        }

        public byte[] Value
        {
            get { return value; }
        }

        public int Length
        {
            get { return value.Length; }
        }

        /// <summary>
        /// Returns the hexString representation of the byte array
        /// </summary>
        /// <returns>hexString string</returns>
        public override string ToString()
        {
            log.LogMethodEntry();
            string returnValue = BitConverter.ToString(value).Replace("-", "");
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Appends the byte array. returns the appended bytearray
        /// </summary>
        /// <param name="array">bytearray to be appended</param>
        /// <returns>appended byte array</returns>
        public ByteArray Append(ByteArray array)
        {
            log.LogMethodEntry(array);
            ByteArray result = array == null ? this : Append(array.Value);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Appends the byte array. returns the appended bytearray
        /// </summary>
        /// <param name="array">bytearray to be appended</param>
        /// <returns>appended byte array</returns>
        public ByteArray Append(byte[] array)
        {
            log.LogMethodEntry(array);
            if (array == null)
            {
                log.LogMethodExit(null, "Empty array");
                return this;
            }

            byte[] concatenatedByteArray = new byte[Length + array.Length];
            value.CopyTo(concatenatedByteArray, 0);
            array.CopyTo(concatenatedByteArray, Length);
            ByteArray result = new ByteArray(concatenatedByteArray);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Appends the byte. returns the appended bytearray
        /// </summary>
        /// <param name="byteValue">byte value to be appended</param>
        /// <returns>appended byte array</returns>
        public ByteArray Append(byte byteValue)
        {
            log.LogMethodEntry(byteValue);
            byte[] concatenatedByteArray = new byte[Length + 1];
            value.CopyTo(concatenatedByteArray, 0);
            concatenatedByteArray[Length] = byteValue;
            ByteArray result = new ByteArray(concatenatedByteArray);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Prepends the byte array. returns the prepended bytearray
        /// </summary>
        /// <param name="array">bytearray to be prepended</param>
        /// <returns>prepended bytearray</returns>
        public ByteArray Prepend(ByteArray array)
        {
            log.LogMethodEntry(array);
            ByteArray result = array == null ? this : array.Append(this);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Prepends the byte array. returns the prepended bytearray
        /// </summary>
        /// <param name="array">bytearray to be prepended</param>
        /// <returns>prepended bytearray</returns>
        public ByteArray Prepend(byte[] array)
        {
            log.LogMethodEntry(array);
            if (array == null)
            {
                log.LogMethodExit(null, "array is empty");
                return this;
            }

            byte[] concatenatedByteArray = new byte[Length + array.Length];
            array.CopyTo(concatenatedByteArray, 0);
            value.CopyTo(concatenatedByteArray, array.Length);
            ByteArray result = new ByteArray(concatenatedByteArray);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Prepends the byte value. returns the prepended bytearray
        /// </summary>
        /// <param name="byteValue">byte value to be prepended</param>
        /// <returns>prepended bytearray</returns>
        public ByteArray Prepend(byte byteValue)
        {
            log.LogMethodEntry(byteValue);
            byte[] concatenatedByteArray = new byte[Length + 1];
            value.CopyTo(concatenatedByteArray, 1);
            concatenatedByteArray[0] = byteValue;
            ByteArray result = new ByteArray(concatenatedByteArray);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the sub array 
        /// </summary>
        /// <param name="index">start index of the sub array</param>
        /// <param name="length">length of the substring</param>
        /// <returns></returns>
        public ByteArray SubArray(int index, int length)
        {
            log.LogMethodEntry(index, length);
            byte[] subArray = new byte[length];
            Array.Copy(value, index, subArray, 0, length);
            ByteArray result = new ByteArray(subArray);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Rotates the byte array by specified value
        /// </summary>
        /// <param name="noOfRotation"></param>
        /// <returns></returns>
        public ByteArray RotateLeftBy(int noOfRotation)
        {
            log.LogMethodEntry(noOfRotation);
            byte[] rotatedBytes = new byte[Length];
            for (int i = 0; i < Length; i++)
            {
                rotatedBytes[i] = this[(i + noOfRotation) % Length];
            }

            ByteArray result = new ByteArray(rotatedBytes);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the reversed byte array
        /// </summary>
        /// <returns></returns>
        public ByteArray Reverse()
        {
            log.LogMethodEntry();
            byte[] reversedBytes = new byte[Length];
            for (int i = 0; i < Length; i++)
            {
                reversedBytes[i] = this[Length - 1 - i];
            }

            ByteArray result = new ByteArray(reversedBytes);
            log.LogMethodExit(result);
            return result;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
            return value.Cast<object>();
        }

        protected static bool IsValidHexString(string hexString)
        {
            log.LogMethodEntry(hexString);
            if (string.IsNullOrWhiteSpace(hexString))
            {
                log.LogMethodExit(false, "Hex string is empty");
                return false;
            }

            string hexStringWithoutSpecialCharacters = RemoveSpecialCharacters(hexString);
            if (Regex.IsMatch(hexStringWithoutSpecialCharacters, "^[a-fA-F0-9]+$") == false)
            {
                log.LogMethodExit(false, "Hex string contains invalid characters");
                return false;
            }

            if (hexStringWithoutSpecialCharacters.Length % 2 != 0)
            {
                log.LogMethodExit(false, "Hex string length should be multiple of 2");
                return false;
            }

            log.LogMethodExit(true);
            return true;
        }

        protected static string RemoveSpecialCharacters(string hexString)
        {
            log.LogMethodEntry(hexString);
            string result = hexString.Replace("-", string.Empty).Replace(" ", string.Empty);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Converts the hex string to byte array
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        protected static byte[] GetByteArrayFromHexString(string hexString)
        {
            log.LogMethodEntry(hexString);
            int length = hexString.Length;
            byte[] bytes = new byte[length / 2];
            for (int i = 0; i < length; i += 2)
                bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            log.LogMethodExit(bytes);
            return bytes;
        }

        /// <summary>
        /// Converts a instance of byte array to array of bytes implicitly
        /// </summary>
        /// <param name="byteArray"></param>
        public static implicit operator byte[](ByteArray byteArray)
        {
            return byteArray.Value;
        }

        /// <summary>
        /// compares two byte arrays
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool Equals(byte[] left,  byte[] right)
        {
            log.LogMethodEntry();
            bool result = false;
            if(left == null && right == null)
            {
                result = true;
                log.LogMethodExit(result);
                return result;
            }
            if((left == null && right != null) || (left != null && right == null))
            {
                log.LogMethodExit(result);
                return result;
            }
            ByteArray leftByteArray = new ByteArray(left);
            ByteArray rightByteArray = new ByteArray(right);
            result = leftByteArray.Equals(rightByteArray);
            log.LogMethodExit(result);
            return result;
        }
    }
}