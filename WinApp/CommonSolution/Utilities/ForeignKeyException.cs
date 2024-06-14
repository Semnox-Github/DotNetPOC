/********************************************************************************************
* Project Name - Utilities
* Description  - Data object of ForeignKeyException
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.60        16-May-2019   Lakshminarayan           Created 
********************************************************************************************/
using System;

namespace Semnox.Core.Utilities
{
    /// <summary>
    /// Represents foreign key error that occur during application execution. 
    /// </summary>
    public class ForeignKeyException : Exception
    {
        /// <summary>
        /// Default constructor of ForeignKeyException.
        /// </summary>
        public ForeignKeyException()
        {
        }

        /// <summary>
        /// Initializes a new instance of ForeignKeyException class with a specified error message.
        /// </summary>
        /// <param name="message">message</param>
        public ForeignKeyException(string message)
        : base(message)
        {
        }
        /// <summary>
        /// Initializes a new instance of ForeignKeyException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="inner">inner exception</param>
        public ForeignKeyException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
