/********************************************************************************************
 * Project Name -RSAEncryption Class
 * Description  -Business Class for RSA Encryption
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.50.0     14-Dec-2018    Guru S A         Application security changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.Utilities
{
    public class RSAEncryption
    {
        private static string privateKey = "nP7W6g5e2qgsjn9J7DeXuT4WOP6QKQriKz3ulxOedo4IM70BZ9DiJJokvNIx5bHkE6KYygqekX5FoqEL+NpH1UScm0dm3pTezx5lF3nvDBxwonVOW0Q494kw6gv2j44nIczUoQSeYgQKD8imy/3tJS4KOL67ErRs3ai3qDVVlGqIqdD7SpVwWumWFng0mo3RQc+2mkcxe4Bsl2OE9a0bkHcuBbrMZ2g2FWL2UNSyxtydcrTmqrO83zBGVCmjFmXAfIKnIjJPCjK0JoQPwB5QSphMrI0wsPl9v7FIetrJqzfrmG7JovQb9vDoF1CplrqdOoMauwFZRhYd0gtwm+G7lL/Q4bcZ2BXx2UnOI2PfRDp0fjqYznHRMl24KNNSSw0t8dGMG1DtowXY9MnM/92S45isLoiMACsxMtXniLS/CjyZk0i68VojiRbnVEfGjQ2gvUTKaooaXjQhJGp7eNjhtUaZ8YKFOgZV1AB/sJHboBH3mSYuzN5qPDN3SHq0nCmxz2Kq3ettWOfzNXhbScoPahh6qXPE5P8ResXnfkJxpQa716lYunuBlHfS9Doiqs7vj3TtaS9AxyJ78c9bocDSTk7W23qrvDc7dqa1i2zVJWT5E8K6PDxVBxyHlwb+d9V+B7pICyDMFu7C/cHv7+GGH/kv9Z8EMAnSFt+I+Cs58nbl+z4IvEyFzWNYF9K7szL5Q3QqeFhkSSrg4V4cNMmp30EpH1i/m7HLhkKAlFdI5YnkEf/OQOv6lC64XGScOYxy+GuHI6q/U29HUJooOr/yXzyv3ZFgkSo6wk1GEpgGUobjUR1YIZQ4KcMhfsBazh+LSmsN8W9gc7gAzzj+p0N8BE0BLl0NfSwBx5w0tXY6NpQr5S6jdFdpW3QMTLpdG6T7DTtFiB3oMv4ovYHUB2Neim3unhJ6GUQ5x9MBKrBXFAn6ecQMNaa7RqJ4eNeX6IFatd8yrRZtQT7Q5kD9N9Xc+jFV1cRihw5L9d2kqvXtjVRggOdJoD4wqv2EGsTd/gH3KBU76UFlqUrP6/tvQ7U3MXcndXFvTBPSbN4se3YYM5Yd7WasKt+KnGOO/UGOX9ARMu4KYtADZ5o2fNSTT8AJlgZNwOVTsp/oPQvjiwcCpcqbnHKuvXuhBvsbozQg9H1SsQTXhiXx0Q64GJNeboEUeoKZmCaigchBStBIqGjUyDrtII93CHua5td57Pm/Xfj67b1lDZypU5U=";
       // private static string publicKey;
        private static UnicodeEncoding _encoder = new UnicodeEncoding();

        //private static void RSA()
        //{
        //    var rsa = new RSACryptoServiceProvider();
        //    privateKey = rsa.ToXmlString(true);
        //    publicKey = rsa.ToXmlString(false);

        //    var text = "Test1";
        //    Console.WriteLine("RSA // Text to encrypt: " + text);
        //    var enc = Encrypt(text);
        //    Console.WriteLine("RSA // Encrypted Text: " + enc);
        //    var dec = Decrypt(enc);
        //    Console.WriteLine("RSA // Decrypted Text: " + dec);
        //}

        public static string Decrypt(string data)
        {
            var rsa = new RSACryptoServiceProvider(); 
            var dataArray = data.Split(new char[] { ',' });
            byte[] dataByte = new byte[dataArray.Length];
            for (int i = 0; i < dataArray.Length; i++)
            {
                dataByte[i] = Convert.ToByte(dataArray[i]);
            }

            rsa.FromXmlString(Encryption.Decrypt(privateKey));
            var decryptedByte = rsa.Decrypt(dataByte, false);
            return _encoder.GetString(decryptedByte);
        }

        public static string Encrypt(string data, string publicKey)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(Encryption.Decrypt(publicKey));
            var dataToEncrypt = _encoder.GetBytes(data);
            var encryptedByteArray = rsa.Encrypt(dataToEncrypt, false).ToArray();
            var length = encryptedByteArray.Count();
            var item = 0;
            var sb = new StringBuilder();
            foreach (var x in encryptedByteArray)
            {
                item++;
                sb.Append(x);

                if (item < length)
                    sb.Append(",");
            }

            return sb.ToString();
        }
    }
}
