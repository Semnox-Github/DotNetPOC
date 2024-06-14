/********************************************************************************************
* Project Name - Utilities
* Description  - Represents validation error that occur during application execution. 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.100.0     12-Nov-2020   Lakshminarayana         Modified - Add methods for supporting serialization 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Linq;
using System.Security.Permissions;

namespace Semnox.Core.Utilities
{
    /// <summary>
    /// Represents validation error that occur during application execution. 
    /// </summary>
    [Serializable]
    public class ValidationException : ParafaitApplicationException, ISerializable
    {
        private IList<ValidationError> validationErrorList;
        private int index = -1;

        /// <summary>
        /// Initializes a new instance of ValidationException class with a specified error message and validation errors.
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="validationErrorList">list of validation errors</param>
        public ValidationException(string message, List<ValidationError> validationErrorList)
        : base(message)
        {
            this.validationErrorList = validationErrorList;            
        }

        /// <summary>
        /// Initializes a new instance of ValidationException class with a specified error message, validation errors and index.
        /// index will be useful while validating list
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="validationErrorList">list of validation errors</param>
        /// <param name="index">index of the invalid record</param>
        public ValidationException(string message, List<ValidationError> validationErrorList, int index)
        : base(message)
        {
            this.validationErrorList = validationErrorList;
            this.index = index;
        }
        /// <summary>
        /// Initializes a new instance of ValidationException class with a specified error message 
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="validationErrorList">list of validation errors</param>
        public ValidationException(string message)
        : base(message)
        {
            List<ValidationError> validationErrorList = new List<ValidationError>();
            ValidationError validationError = new ValidationError("", "", message);
            validationErrorList.Add(validationError);
            this.validationErrorList = validationErrorList;
        }
        /// <summary>
        /// Initializes a new instance of ValidationException class with a specified error message and field name.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="entityId"></param>
        /// <param name="displayMessage"></param>
        public ValidationException(string message, int entityId, string displayMessage) : base(message)
        {
            validationErrorList = new List<ValidationError>();
            ValidationError validationError = new ValidationError("", "", displayMessage, entityId);
            validationErrorList.Add(validationError);
        }

        /// <summary>
        /// Initializes a new instance of ValidationException class with a specified error message and field name.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="entityName"></param>
        /// <param name="fieldName"></param>
        /// <param name="displayMessage"></param>
        public ValidationException(string message, string entityName, string fieldName, string displayMessage):base(message)
        {
            validationErrorList = new List<ValidationError>();
            ValidationError validationError = new ValidationError(entityName, fieldName, displayMessage);
            validationErrorList.Add(validationError);
        }

        /// <summary>
        /// Serialization Constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        protected ValidationException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
            validationErrorList = (IList<ValidationError>)info.GetValue("validationErrorList", typeof(IList<ValidationError>));
            index = info.GetInt32("index");
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
            info.AddValue("validationErrorList", validationErrorList, typeof(IList<ValidationError>));
            info.AddValue("index", index);
            base.GetObjectData(info, context);

        }
        

        /// <summary>
        /// Get method of validationErrorList field
        /// </summary>
        public List<ValidationError> ValidationErrorList
        {
            get
            {
                return validationErrorList.ToList();
            }
        }

        /// <summary>
        /// Get method of index field
        /// </summary>
        public int Index
        {
            get
            {
                return index;
            }
        }
        /// <summary>
        /// Returns all the validation error messages to be shown to the user.
        /// </summary>
        /// <returns></returns>
        public string GetAllValidationErrorMessages()
        {
            StringBuilder errorMessageBuilder = new StringBuilder("");
            if(validationErrorList != null && validationErrorList.Count > 0)
            {
                foreach (var validationError in validationErrorList)
                {
                    errorMessageBuilder.Append(validationError.Message);
                    errorMessageBuilder.Append(Environment.NewLine);
                }
            }
            return errorMessageBuilder.ToString();
        }
        /// <summary>
        /// Returns the first validation error
        /// </summary>
        /// <returns></returns>
        public ValidationError GetFirstValidationError()
        {
            ValidationError validationError = null;
            if (validationErrorList != null && validationErrorList.Count > 0)
            {
                validationError = validationErrorList[0];
            }
            if(validationError == null)
            {
                validationError = new ValidationError();
            }
            return validationError;
        }

        public override string Message
        {
            get
            {
                string msg = GetAllValidationErrorMessages();
                if(string.IsNullOrEmpty(msg))
                {
                    msg = base.Message;
                }
                return msg;
            }
        }
    }
}
