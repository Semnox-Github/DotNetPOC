/********************************************************************************************
 * Project Name - Utilities
 * Description  - Exception class for error when the entity has expired 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.100       24-Sep-2020   Nitin Pai           Created: 
 ********************************************************************************************/
using System;

namespace Semnox.Core.Utilities
{
    /// <summary>
    /// Represents Entity not found error that occur during application execution.
    /// </summary>
    public class EntityExpiredException : Exception
    {
        public EntityExpiredException(string message):base(message)
        {

        }
        
        public EntityExpiredException(string message, Exception innerException):base(message, innerException)
        {

        }
    }
}
