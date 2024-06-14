/********************************************************************************************
 * Project Name - GenerateToken Programs  
 * linkName  - GenerateToken class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By     Remarks          
 *********************************************************************************************
 *1.00        09-May-2016   Rakshith        Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.ClientApp
{
    ////<summary>
    ////GenerateToken Class 
    ////</summary>
    class GenerateToken
    {
        /// <summary>
        /// GetToken generates random security code
        /// </summary>
        public string GetToken()
        {
            int size = 6;
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789=!@#$%^&*";
            var stringChars = new char[size];
            var random = new Random();
            byte[] byteArray = new byte[size];
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
                byteArray[i] = Convert.ToByte(chars[random.Next(chars.Length)]);
            }
            // return Convert.ToBase64String(byteArray);
            String token=new String(stringChars);
            return token;
        }
    }
}
