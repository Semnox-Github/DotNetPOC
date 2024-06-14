/********************************************************************************************
 * Project Name - GenericUtilities
 * Description  - Represents a random byte array
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Represents a random byte array
    /// </summary>
    public class RandomByteArray: ByteArray
    {
        private static readonly Semnox.Parafait.logging.Logger log =
            new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="length"></param>
        public RandomByteArray(int length)
        :base((new RandomString(length * 2, "0123456789ABCDEF")).Value)
        {
            log.LogMethodEntry(length);
            log.LogMethodExit();
        }
    }
}
