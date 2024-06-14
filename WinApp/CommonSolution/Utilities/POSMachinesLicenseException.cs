/********************************************************************************************
* Project Name - Utilities
* Description  - Data object of ForeignKeyException
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.80       16-May-2020    Girish Kundar           Created to handle POSMachinesLicense exception for WMS in POSMachine & POSMachineType API
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.Utilities
{
    public class POSMachinesLicenseException : Exception
    {
        /// <summary>
        /// Default constructor of POSMachinesLicenseException.
        /// </summary>
        public POSMachinesLicenseException()
        {
        }

        /// <summary>
        /// Initializes a new instance of POSMachinesLicenseException class with a specified error message.
        /// </summary>
        /// <param name="message">message</param>
        public POSMachinesLicenseException(string message)
        : base(message)
        {
        }
        /// <summary>
        /// Initializes a new instance of POSMachinesLicenseException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="inner">inner exception</param>
        public POSMachinesLicenseException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
