/*===========================================================================================
 * 
 *  Copyright (C)   : Advanced Card System Ltd
 * 
 *  File            : Helper.cs
 * 
 *  Description     : Contains helper methods
 * 
 *  Author          : Arturo Salvamante
 *  
 *  Date            : October 19, 2011
 * 
 *  Revision Traile : [Author] / [Date if modification] / [Details of Modifications done] 
 * =========================================================================================
 *  Modified to add Logger Methods by Deeksha on 08-Aug-2019
 * =========================================================================================*/
using System;

namespace Semnox.Parafait.Device
{
    internal class Helper
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static byte[] getBytes(string stringBytes, char delimeter)
        {
            log.LogMethodEntry(stringBytes, delimeter);
            string[] arrayString = stringBytes.Split(delimeter);
            byte[] bytesResult = new byte[arrayString.Length];
            byte tmpByte;
            int counter = 0;

            foreach (string str in arrayString)
            {
                if (byte.TryParse(str, System.Globalization.NumberStyles.HexNumber, null, out tmpByte))
                {
                    bytesResult[counter] = tmpByte;
                    counter++;
                }
                else
                {
                    log.LogMethodExit();
                    return null;
                }
            }
            log.LogMethodExit(bytesResult);
            return bytesResult;
        }

        public static byte[] getBytes(string stringBytes)
        {
            log.LogMethodEntry(stringBytes);
            string fString = "";
            int counter = 0;

            if (stringBytes.Trim() == "")
            {
                log.LogMethodExit();
                return null;
            }

            for (int i = 0; i < stringBytes.Length; i++)
            {
                if (stringBytes[i] == ' ')
                    continue;

                if (counter > 0)
                    if ((counter % 2) == 0)
                        fString += " ";

                fString += stringBytes[i].ToString();

                counter++;
            }
            byte[] returnValue = getBytes(fString, ' ');
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public static Int32 byteToInt(byte[] data, bool isLittleEndian)
        {
            log.LogMethodEntry(data, isLittleEndian);
            byte[] tmpArry = new byte[data.Length];
            Array.Copy(data, tmpArry, tmpArry.Length);

            if (tmpArry.Length != 4)
            {
                if (isLittleEndian)
                    Array.Resize(ref tmpArry, 4);
                else
                {
                    Array.Reverse(tmpArry);
                    Array.Resize(ref tmpArry, 4);
                    Array.Reverse(tmpArry);
                }
            }

            if (isLittleEndian)
            {
                int returnValue = (tmpArry[3] << 24) + (tmpArry[2] << 16) + (tmpArry[1] << 8) + tmpArry[0];
                log.LogMethodExit(returnValue);
                return returnValue;
            }
            else
            {
                int returnValue = (tmpArry[0] << 24) + (tmpArry[1] << 16) + (tmpArry[2] << 8) + tmpArry[3];
                log.LogMethodExit(returnValue);
                return returnValue;
            }
        }

        public static int byteToInt(byte[] data)
        {
            log.LogMethodEntry(data);
            int returnValue = byteToInt(data, false);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public static byte[] intToByte(int nummber)
        {
            log.LogMethodEntry(nummber);
            byte[] tmpByte = new byte[4];

            tmpByte[0] = (byte)((nummber >> 24) & 0xFF);
            tmpByte[1] = (byte)((nummber >> 16) & 0xFF);
            tmpByte[2] = (byte)((nummber >> 8) & 0xFF);
            tmpByte[3] = (byte)(nummber & 0xFF);
            log.LogMethodExit(tmpByte);
            return tmpByte;
        }

        public static byte[] intToByte(UInt32 number)
        {
            log.LogMethodEntry(number);
            byte[] tmpByte = new byte[4];

            tmpByte[0] = (byte)((number >> 24) & 0xFF);
            tmpByte[1] = (byte)((number >> 16) & 0xFF);
            tmpByte[2] = (byte)((number >> 8) & 0xFF);
            tmpByte[3] = (byte)(number & 0xFF);
            log.LogMethodExit(tmpByte);
            return tmpByte;
        }

        public static string byteAsString(byte[] bytes, int startIndex, int length, bool spaceInBetween)
        {
            log.LogMethodEntry(bytes, startIndex, length, spaceInBetween);
            byte[] newByte;

            if (bytes.Length < startIndex + length)
                Array.Resize(ref bytes, startIndex + length);

            newByte = new byte[length];
            Array.Copy(bytes, startIndex, newByte, 0, length);
            string returnvalue = byteAsString(newByte, spaceInBetween);
            log.LogMethodExit(returnvalue);
            return returnvalue;
        }

        public static string byteAsString(byte[] tmpbytes, bool spaceInBetween)
        {
            log.LogMethodEntry(tmpbytes, spaceInBetween);
            string tmpStr = string.Empty;

            if (tmpbytes == null)
                return "";
                        
            for (int i = 0; i < tmpbytes.Length; i++)
            {
                tmpStr += string.Format("{0:X2}", tmpbytes[i]);

                if (spaceInBetween)
                    tmpStr += " ";
            }
            log.LogMethodExit(tmpStr);
            return tmpStr;
        }

        public static bool byteArrayIsEqual(byte[] array1, byte[] array2, int lenght)
        {
            log.LogMethodEntry(array1, array2, lenght);
            if (array1.Length < lenght)
            {
                log.LogMethodExit(false);
                return false;
            }

            if (array2.Length < lenght)
            {
                log.LogMethodExit(false);
                return false;
            }

            for (int i = 0; i < lenght; i++)
            {
                if (array1[i] != array2[i])
                {
                    log.LogMethodExit(false);
                    return false;
                }
            }
            log.LogMethodExit(true);
            return true;
        }

        public static bool byteArrayIsEqual(byte[] array1, byte[] array2)
        {
            log.LogMethodEntry(array1, array2);
            bool returnValue = byteArrayIsEqual(array1, array2, array2.Length);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public static byte[] appendArrays(byte[] array1, byte[] array2)
        {
            log.LogMethodEntry(array1, array2);
            byte[] c = new byte[array1.Length + array2.Length];
            Buffer.BlockCopy(array1, 0, c, 0, array1.Length);
            Buffer.BlockCopy(array2, 0, c, array1.Length, array2.Length);
            log.LogMethodExit(c);
            return c;
        }

        public static byte[] appendArrays(byte[] array1, byte array2)
        {
            log.LogMethodEntry(array1, array2);
            byte[] c = new byte[1 + array1.Length];
            Buffer.BlockCopy(array1, 0, c, 0, array1.Length);
            c[array1.Length] = array2;
            log.LogMethodExit(c);
            return c;
        }

        public static String byteArrayToString(byte[] data)
        {
            log.LogMethodEntry(data);
            String str = "";

            for (int i = 0; i < data.Length; i++)
            {
                str += (char)data[i];
            }
            log.LogMethodExit(str);
            return str;
        }

    }
}
