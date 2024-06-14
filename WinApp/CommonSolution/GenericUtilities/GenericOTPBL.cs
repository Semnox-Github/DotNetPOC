/********************************************************************************************
 * Project Name - GenericOTP BL
 * Description  - Business Logic of the GenericOTP
 * 
 **************
 **Version Log
 **************
 *Version      Date         Modified By         Remarks          
 *********************************************************************************************
 *2.130.11   08-Aug-2022    Yashodhara C H       Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Business logic for GenericOTP class.
    /// </summary>
    public class GenericOTPBL
    {
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private GenericOTPDTO genericOTPDTO;

        /// <summary>
        /// Parameterized constructor of AccountBL class
        /// </summary>
        private GenericOTPBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates GenericOTPBL object using the GenericOTPDTO
        /// </summary>
        /// <param name="executionContext">ExecutionContext object is passed as parameter</param>
        /// <param name="GenericOTPDTO">GenericOTPDTO object is passed as parameter</param>
        public GenericOTPBL(ExecutionContext executionContext, GenericOTPDTO genericOTPDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, genericOTPDTO);
            this.genericOTPDTO = genericOTPDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates GenericOTPBL object using the id
        /// </summary>
        /// <param name="executionContext">ExecutionContext object is passed as parameter</param>
        /// <param name="id">id is passed as parameter</param>
        public GenericOTPBL(ExecutionContext executionContext, int id)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id);
            GenericOTPDataHandler genericOTPDataHandler = new GenericOTPDataHandler(null);
            this.genericOTPDTO = genericOTPDataHandler.GetGenericOTPDTO(id);
            if(this.genericOTPDTO == null)
            {
                String errorMessage = "OTP object not found";
                log.Error(errorMessage + " " + id);
                throw new ValidationException(errorMessage);
            }
            log.LogMethodExit();
        }

        //// <summary>
        /// Saves the GenericOTP
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            GenericOTPDataHandler genericOTPDataHandler = new GenericOTPDataHandler(sqlTransaction);
            if (genericOTPDTO.IsActive == true)
            {
                List<ValidationError> validationError = Validate();
                if (validationError.Any())
                {
                    string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                    log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationError.Select(x => x.Message)));
                    throw new ValidationException(message, validationError);
                }
            }
            if (genericOTPDTO.Id < 0)
            {
                log.LogVariableState("GenericOTPDTO", genericOTPDTO);
                genericOTPDTO = genericOTPDataHandler.InsertRecord(genericOTPDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                genericOTPDTO.AcceptChanges();
            }
            else
            {
                log.LogVariableState("GenericOTPDTO", genericOTPDTO);
                genericOTPDTO = genericOTPDataHandler.UpdateRecord(genericOTPDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                genericOTPDTO.AcceptChanges();
            }
            
            log.LogMethodExit();
        }

        //// <summary>
        /// Validates the GenericOTP parameters
        /// </summary>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            List<ValidationError> validationError = new List<ValidationError>();
            if (String.IsNullOrWhiteSpace(genericOTPDTO.Source))
            {
                String errorMessage = "Source is empty";
                log.Debug(errorMessage);
                validationError.Add(new ValidationError("OTP", "Source", errorMessage));
            }

            if (String.IsNullOrWhiteSpace(genericOTPDTO.EmailId) && String.IsNullOrWhiteSpace(genericOTPDTO.Phone))
            {

                string errorMessage = MessageContainerList.GetMessage(executionContext, 251);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                validationError.Add(new ValidationError("OTP", "Email or Phone", errorMessage));
            }

            if(!String.IsNullOrWhiteSpace(genericOTPDTO.Phone))
            {
                // length validation 
                if (!ValidatePhone(genericOTPDTO.Phone.Trim()))
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 785);
                    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                    validationError.Add(new ValidationError("GenericOTP", "emailId", errorMessage));
                }
            }

            if(!string.IsNullOrWhiteSpace(genericOTPDTO.EmailId))
            {
                if(!ValidateEmail(genericOTPDTO.EmailId.Trim()))
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 450);
                    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                    validationError.Add(new ValidationError("GenericOTP", "emailId", errorMessage));
                }
            }

            if (string.IsNullOrWhiteSpace(genericOTPDTO.Code))
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1087);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                validationError.Add(new ValidationError("GenericOTP", "emailId", errorMessage));
            }
            
            log.LogMethodExit(validationError);
            return validationError;
        }

        /// <summary>
        /// Method to validate email address
        /// </summary>
        /// <param name="emailAddress">string</param>
        /// <returns></returns>
        protected bool ValidateEmail(string emailAddress)
        {
            log.LogMethodEntry(emailAddress);
            bool functionReturnValue = false;
            string pattern = "^[a-zA-Z0-9][\\w\\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\\w\\.-]" + "*[a-zA-Z0-9]\\.[a-zA-Z][a-zA-Z\\.]*[a-zA-Z]$";
            Match emailAddressMatch = Regex.Match(emailAddress, pattern);
            if (emailAddressMatch.Success)
            {
                functionReturnValue = true;
            }
            else
            {
                functionReturnValue = false;
            }
            log.LogMethodExit(functionReturnValue);
            return functionReturnValue;

        }

        /// <summary>
        /// Method to validate Phone Number
        /// </summary>
        /// <param name="emailAddress">string</param>
        /// <returns></returns>
        protected bool ValidatePhone(string phoneNumber)
        {
            log.LogMethodEntry(phoneNumber);
            bool functionReturnValue = false;
            string pattern = @"^[0-9]+$";
            Match phoneNumberMatch = Regex.Match(phoneNumber, pattern);
            if (phoneNumberMatch.Success)
            {
                functionReturnValue = true;
            }
            else
            {
                functionReturnValue = false;
            }
            log.LogMethodExit(functionReturnValue);
            return functionReturnValue;

        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public GenericOTPDTO GenericOTPDTO
        {
            get
            {
                return genericOTPDTO;
            }
        }
    }

    public class GenericOTPListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor of GenericOTPBL class
        /// </summary>
        /// <param name="executionContext"></param>
        public GenericOTPListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the GenericOTP list
        /// </summary>
        public List<GenericOTPDTO> GetGenericOTPDTO(List<KeyValuePair<GenericOTPDTO.SearchByParameters, string>> searchParameter, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameter, sqlTransaction);
            GenericOTPDataHandler genericOTPDataHandler = new GenericOTPDataHandler(sqlTransaction);
            List<GenericOTPDTO> genericOTPDTOList = genericOTPDataHandler.GetGenericOTPDTOList(searchParameter, sqlTransaction);
            log.LogMethodExit(genericOTPDTOList);
            return genericOTPDTOList;
        }

    }
}
 