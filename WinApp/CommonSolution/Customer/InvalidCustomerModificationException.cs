using System;
using System.Runtime.Serialization;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Represents validation error that occur during application execution. 
    /// </summary>
    [Serializable]
    public class InvalidCustomerModificationException : ParafaitApplicationException, ISerializable
    {
        /// <summary>
        /// Parameterized constructor of ParafaitApplicationException.
        /// </summary>
        public InvalidCustomerModificationException(string message) : base(message)
        {
        }
        /// <summary>
        /// Parameterized constructor of ParafaitApplicationException.
        /// </summary>
        public InvalidCustomerModificationException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
