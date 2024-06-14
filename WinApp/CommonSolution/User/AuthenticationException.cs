using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.User
{
    [Serializable]
    /// <summary>
    /// Represents UserAuthenticationException error that occur during user authentication. 
    /// </summary>
    public class UserAuthenticationException : ParafaitApplicationException, ISerializable
    {
        private UserAuthenticationErrorType userAuthenticationErrorType;
        /// <summary>
        /// Parameterized constructor of UserAuthenticationException.
        /// </summary>
        public UserAuthenticationException(string message, UserAuthenticationErrorType userAuthenticationErrorType) : base(message)
        {
            this.userAuthenticationErrorType = userAuthenticationErrorType;
        }
        /// <summary>
        /// Parameterized constructor of UserAuthenticationException.
        /// </summary>
        public UserAuthenticationException(string message, UserAuthenticationErrorType userAuthenticationErrorType, Exception innerException) : base(message, innerException)
        {
            this.userAuthenticationErrorType = userAuthenticationErrorType;
        }

        /// <summary>
        /// Serialization Constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        protected UserAuthenticationException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
            userAuthenticationErrorType = (UserAuthenticationErrorType) Enum.Parse(typeof(UserAuthenticationErrorType), info.GetString("userAuthenticationErrorType"));
        }
        /// <summary>
        /// ISerializable interface implementation
        /// </summary>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            info.AddValue("userAuthenticationErrorType", userAuthenticationErrorType.ToString());
            base.GetObjectData(info, context);
        }
        public UserAuthenticationErrorType UserAuthenticationErrorType
        {
            get
            {
                return userAuthenticationErrorType;
            }
        }
        }
}
