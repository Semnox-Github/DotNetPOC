/********************************************************************************************
 * Project Name - GenericOTP BL
 * Description  - Business Logic of the GenericOTP
 * 
 **************
 **Version Log
 **************
 *Version      Date         Modified By         Remarks          
 *********************************************************************************************
 *2.130.11   18-Aug-2022    Yashodhara C H       Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using Semnox.Parafait.Communication;
using System.Data.SqlClient;
using System.Text;
using System.Linq;

namespace Semnox.Core.GenericUtilities
{
    class GenericOTPVerificationBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor of GenericOTPValidationBL class
        /// </summary>
        /// <param name="executionContext"></param>
        public GenericOTPVerificationBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit(executionContext);
        }

        /// <summary>
        /// return a Random number
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="validCharacters"></param>
        /// <returns></returns>
        internal string GenerateRandomNumber(int Width, string validCharacters)
        {
            log.LogMethodEntry(Width, validCharacters);
            if (string.IsNullOrEmpty(validCharacters))
            {
                log.Debug("validCharacters is not passed. Taking default value");
                validCharacters = "1234567890ABCDEFGHIJKLMNOPQRSUVWXYZ";
            }
            var ret = "";

            byte[] seed = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(seed);
            int seedInt = BitConverter.ToInt32(seed, 0);

            var rnd = new Random(seedInt);
            while (ret.Length < Width)
            {
                ret += validCharacters[rnd.Next(validCharacters.Length - 1)].ToString();
            }
            log.LogMethodExit(ret);
            return ret;
        }

        /// <summary>
        /// Generates OTP, sets expiry time and inserts it into DB
        /// </summary>
        /// <param name="countryCode"></param>
        ///<param name="emailId"></param>
        ///<param name="phone"></param>
        ///<param name="source"></param>
        public GenericOTPDTO GenerateOTP(string phone = null, string emailId = null, string source = null, string countryCode = null, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(phone, emailId, source, countryCode);
            
            // validated the inputs here
            if(String.IsNullOrWhiteSpace(source))
            {
                String errorMessage = "Source is empty " + source;
                log.Error(errorMessage);
                throw new ValidationException(errorMessage);
            }

            ParafaitFunctionEvents parafaitFunctionEvents = 0;
            if (!Enum.TryParse(source.ToUpper(), out parafaitFunctionEvents))
            {
                String errorMessage = "Invalid Source";
                log.Error(errorMessage);
                throw new ValidationException(errorMessage);
            }
            log.Debug("Parafait event identified " + parafaitFunctionEvents);

            if (String.IsNullOrWhiteSpace(emailId) && String.IsNullOrWhiteSpace(phone))
            {
                String errorMessage = "Email or Phone is mandatory";
                log.Error(errorMessage);
                throw new ValidationException(errorMessage);
            }

            // Set default OTP length and expiry
            int otpLength = 6;
            int otpValidatityMins = 5;
            int numberOfAttempts = 1;

            try
            {
                log.Debug("Getting overridden values");
                // Check if the defaults have been overridden for specific sources
                LookupsContainerDTO lookupsContainerDTO = LookupsContainerList.GetLookupsContainerDTO(executionContext.GetSiteId(), "GENERIC_OTP");
                if (lookupsContainerDTO != null && lookupsContainerDTO.LookupValuesContainerDTOList != null)
                {
                    string descriptionValue = "";
                    foreach (LookupValuesContainerDTO lookupValuesContainerDTO in lookupsContainerDTO.LookupValuesContainerDTOList)
                    {
                        if (lookupValuesContainerDTO.LookupValue.Equals(source, StringComparison.InvariantCultureIgnoreCase))
                        {
                            descriptionValue = lookupValuesContainerDTO.Description;
                            break;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(descriptionValue) && descriptionValue.Contains("|"))
                    {
                        string[] values = descriptionValue.Split('|');
                        foreach (var lookup in values)
                        {
                            string[] innervalues = lookup.Split(':');
                            if (innervalues[0].Equals("EXP", StringComparison.InvariantCultureIgnoreCase))
                            {
                                otpValidatityMins = int.Parse(innervalues[1]);
                                log.Debug("Overridden value for expiry minutes " + otpValidatityMins);
                            }
                            else if (innervalues[0].Equals("LEN", StringComparison.InvariantCultureIgnoreCase))
                            {
                                otpLength = int.Parse(innervalues[1]);
                                log.Debug("Overridden value for otp length " + otpLength);
                            }
                            else if (innervalues[0].Equals("ATTEMPTS", StringComparison.InvariantCultureIgnoreCase))
                            {
                                numberOfAttempts = int.Parse(innervalues[1]);
                                log.Debug("Overridden value for attempts " + numberOfAttempts);
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error("Caught an error while getting defaults for OTP generation " + ex);
            }

            string otpCode = GenerateRandomNumber(otpLength, "1234567890");
            log.Debug("Generated OTP code");
            DateTime expiryTime = ServerDateTime.Now.AddMinutes(otpValidatityMins);
            log.Debug("expiry time " + expiryTime);

            log.Debug("Saving OTP");
            GenericOTPDTO genericOTPDTO = new GenericOTPDTO(-1, otpCode, phone, countryCode, emailId, source.ToUpper(), false, numberOfAttempts, expiryTime, true);
            GenericOTPBL genericOTPBL = new GenericOTPBL(executionContext, genericOTPDTO);
            genericOTPBL.Save(sqlTransaction);

            // Send the OTP email and sms
            var textFields = new List<KeyValuePair<string, string>>() {
            new KeyValuePair<string, string>("EMAILID", emailId),
            new KeyValuePair<string, string>("PHONE", phone),
            new KeyValuePair<string, string>("SOURCE", source),
            new KeyValuePair<string, string>("EXPIRYTIME", genericOTPDTO.ExpiryTime.ToString()),
            new KeyValuePair<string, string>("OTPNUMBER", genericOTPDTO.Code.ToString()),
            new KeyValuePair<string, string>("COUNTRYCODE", countryCode) };
            SendOTP(parafaitFunctionEvents, textFields, sqlTransaction);

            // OTP should not be displayed
            genericOTPBL.GenericOTPDTO.Code = "";

            log.LogMethodExit(genericOTPDTO);
            return genericOTPDTO;
        }

        private void SendOTP(ParafaitFunctionEvents parafaitFunctionEvents, List<KeyValuePair<string, string>> textFields, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            GenericEventsBL genericEventsBL = new GenericEventsBL(executionContext, parafaitFunctionEvents, textFields);

            var email = textFields.FirstOrDefault(x => x.Key == "EMAILID");
            if (email.Key != null && email.Value != null && !string.IsNullOrEmpty(email.Key) && !string.IsNullOrEmpty(email.Value))
            {
                log.Debug("Generating email ");
                genericEventsBL.SendMessage(MessagingClientDTO.MessagingChanelType.EMAIL, sqlTransaction);
                log.Debug("Generated email ");
            }

            var phone = textFields.FirstOrDefault(x => x.Key == "PHONE");
            if (phone.Key != null && phone.Value != null && !string.IsNullOrEmpty(phone.Key) && !string.IsNullOrEmpty(phone.Value))
            {
                log.Debug("Generating sms ");
                genericEventsBL.SendMessage(MessagingClientDTO.MessagingChanelType.SMS, sqlTransaction);
                log.Debug("Generated sms");
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates OTP
        /// </summary>
        /// <param name="code"></param>
        ///<param name="id"></param>
        ///<param name="siteId"></param>
        public void ValidateOTP(int id, string code, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(id, code);

            GenericOTPBL genericOTPBL = new GenericOTPBL(executionContext, id);
            if (genericOTPBL.GenericOTPDTO.ExpiryTime > ServerDateTime.Now &&
                genericOTPBL.GenericOTPDTO.Code.Equals(code, StringComparison.InvariantCultureIgnoreCase) && 
                genericOTPBL.GenericOTPDTO.RemainingAttempts > 0 &&
                genericOTPBL.GenericOTPDTO.IsVerified != null && genericOTPBL.GenericOTPDTO.IsVerified == false)
            {
                log.Debug("OTP is matching and validated " + id + ":" + code + ":" + genericOTPBL.GenericOTPDTO.RemainingAttempts);
                genericOTPBL.GenericOTPDTO.IsVerified = true;
                genericOTPBL.Save(sqlTransaction);
                log.LogMethodExit("OTP validation success");
            }
            else
            {
                log.Debug("OTP is not matching and not validated " + id + ":" + code + ":" + genericOTPBL.GenericOTPDTO.RemainingAttempts);
                genericOTPBL.GenericOTPDTO.RemainingAttempts--;
                genericOTPBL.Save(sqlTransaction);
                String errorMessage = "Invalid OTP";
                log.Debug(errorMessage);
                log.LogMethodExit("OTP validation failed");
                throw new ValidationException(errorMessage);
            }
        }

        public GenericOTPDTO ResendOTP(int id = -1, string phone = null, string emailId = null, string source = null, string countryCode = null, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(phone, emailId, source);

            // validated the inputs here
            if (String.IsNullOrWhiteSpace(source))
            {
                String errorMessage = "Source is empty " + source;
                log.Error(errorMessage);
                throw new ValidationException(errorMessage);
            }

            ParafaitFunctionEvents parafaitFunctionEvents = 0;
            if (!Enum.TryParse(source.ToUpper(), out parafaitFunctionEvents))
            {
                String errorMessage = "Invalid Source";
                log.Error(errorMessage);
                throw new ValidationException(errorMessage);
            }
            log.Debug("Parafait event identified " + parafaitFunctionEvents);

            if (id == -1 && string.IsNullOrEmpty(phone) && string.IsNullOrEmpty(emailId))
            {
                String errorMessage = "Invalid inputs";
                log.Error(errorMessage);
                throw new ValidationException(errorMessage);
            }

            bool recreateOTP = true;
            GenericOTPListBL genericOTPListBL = new GenericOTPListBL(executionContext);
            List<KeyValuePair<GenericOTPDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<GenericOTPDTO.SearchByParameters, string>>();
            if(id > -1)
            {
                searchParameters.Add(new KeyValuePair<GenericOTPDTO.SearchByParameters, string>(GenericOTPDTO.SearchByParameters.ID, id.ToString()));
            }
            if (!string.IsNullOrWhiteSpace(phone))
            {
                searchParameters.Add(new KeyValuePair<GenericOTPDTO.SearchByParameters, string>(GenericOTPDTO.SearchByParameters.PHONE, phone));
            }
            if (!string.IsNullOrWhiteSpace(emailId))
            {
                searchParameters.Add(new KeyValuePair<GenericOTPDTO.SearchByParameters, string>(GenericOTPDTO.SearchByParameters.EMAIL_ID, emailId));
            }
            searchParameters.Add(new KeyValuePair<GenericOTPDTO.SearchByParameters, string>(GenericOTPDTO.SearchByParameters.SOURCE, source.ToUpper()));

            GenericOTPDTO genericOTPDTO = null;
            List<GenericOTPDTO> genericOTPDTOList = genericOTPListBL.GetGenericOTPDTO(searchParameters);
            if (genericOTPDTOList != null && genericOTPDTOList.Any())
            {
                log.Debug("Found OTP send to this number, validate the entry");
                genericOTPDTO = genericOTPDTOList.OrderByDescending(x => x.CreatedBy).First();
                if (genericOTPDTO != null &&
                    genericOTPDTO.Source.Equals(source, StringComparison.InvariantCultureIgnoreCase) &&
                    genericOTPDTO.IsVerified != null && genericOTPDTO.IsVerified == false &&
                    genericOTPDTO.ExpiryTime != DateTime.MinValue && 
                    genericOTPDTO.ExpiryTime > ServerDateTime.Now && 
                    genericOTPDTO.RemainingAttempts > 0)
                {
                    log.Debug("Identified the previous entry. Resending the same " + genericOTPDTO.Id);
                    // Send the OTP email and sms
                    var textFields = new List<KeyValuePair<string, string>>() {
                    new KeyValuePair<string, string>("EMAILID", emailId),
                    new KeyValuePair<string, string>("PHONE", phone),
                    new KeyValuePair<string, string>("SOURCE", source),
                    new KeyValuePair<string, string>("EXPIRYTIME", genericOTPDTO.ExpiryTime.ToString()),
                    new KeyValuePair<string, string>("OTPNUMBER", genericOTPDTO.Code.ToString()),
                    new KeyValuePair<string, string>("COUNTRYCODE", countryCode)
                    };
                    SendOTP(parafaitFunctionEvents, textFields, sqlTransaction);
                    recreateOTP = false;
                }
            }

            if (recreateOTP)
            {
                log.Debug("previous OTP not identified or expired, generate and send a new OTP");
                genericOTPDTO = GenerateOTP(phone, emailId, source, countryCode, sqlTransaction);
            }
            
            // OTP should not be displayed
            genericOTPDTO.Code = "";
            log.LogMethodExit(genericOTPDTO);
            return genericOTPDTO;
        }
    }
}
