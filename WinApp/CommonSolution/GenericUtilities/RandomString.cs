/********************************************************************************************
 * Project Name - GenericUtilities
 * Description  - Represents a random string
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        1-Jul-2019      Lakshminarayana     Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Represents a random string
    /// </summary>
    public class RandomString: ValueObject
    {
        private static readonly Semnox.Parafait.logging.Logger log =
            new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly string value;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="length">length of the random string</param>
        /// <param name="validCharacterSet">valid character set</param>
        public RandomString(int length, string validCharacterSet = "1234567890ABCDEFGHIJKLMNOPQRSUVWXYZ")
        {
            log.LogMethodEntry(length, validCharacterSet);
            if (length <= 0)
            {
                string errorMessage = "Invalid length parameter. length should be a positive integer";
                log.LogMethodExit(null, "Throwing exception -" + errorMessage);
                throw new ArgumentException(errorMessage);
            }
            byte[] seed = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(seed);
            int seedInt = BitConverter.ToInt32(seed, 0);
            StringBuilder sb = new StringBuilder();
            var rnd = new Random(seedInt);
            while (sb.Length < length)
            {
                char randomCharacter = validCharacterSet[rnd.Next(validCharacterSet.Length - 1)];
                sb.Append(randomCharacter);
            }

            value = sb.ToString();
            log.LogMethodExit();
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return value;
        }

        public string Value
        {
            get
            {
                return value;
            }
        }

        public static implicit operator string(RandomString randomString)
        {
            return randomString.Value;
        } 
    }
}
