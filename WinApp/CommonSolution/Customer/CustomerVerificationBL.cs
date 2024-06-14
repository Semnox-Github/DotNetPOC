/********************************************************************************************
 * Project Name - CustomerVerification BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *1.00        06-Feb-2017      Lakshminarayana     Created 
 *2.70.2      19-Jul-2019      Girish Kundar       Modified : Save() method. Now Insert/Update method returns the DTO instead of Id.
 *2.80        29-Nov-2019      Mushahid Faizan     Modified for Customer Registration Changes.
 *2.90.0      23-Jul-2020      Jinto Thomas        Modified: sending verification code using MessagingClientDTO  
 *2.110.0     12-Dec-2020      Guru S A            For Subscription changes   
 *2.130.10    08-Sep-2022      Nitin Pai           Enhanced customer activity user log table
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Business logic for CustomerVerification class.
    /// </summary>
    public class CustomerVerificationBL
    {
        private CustomerVerificationDTO customerVerificationDTO;
        private readonly ExecutionContext executionContext;
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of CustomerVerificationBL class
        /// </summary>
        public CustomerVerificationBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            customerVerificationDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the customerVerification id as the parameter
        /// Would fetch the customerVerification object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="loadChildRecords">whether to load the child records</param>
        /// <param name="activeChildRecords">whether to load only active child records</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public CustomerVerificationBL(ExecutionContext executionContext, int id, bool loadChildRecords = true, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, loadChildRecords, activeChildRecords, sqlTransaction);
            CustomerVerificationDataHandler customerVerificationDataHandler = new CustomerVerificationDataHandler(sqlTransaction);
            customerVerificationDTO = customerVerificationDataHandler.GetCustomerVerificationDTO(id);
            if (loadChildRecords && customerVerificationDTO != null)
            {
                CustomerVerificationBuilderBL customerVerificationBuilderBL = new CustomerVerificationBuilderBL(executionContext);
                customerVerificationBuilderBL.Build(customerVerificationDTO, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates CustomerVerificationBL object using the CustomerVerificationDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="customerVerificationDTO">CustomerVerificationDTO object</param>
        public CustomerVerificationBL(ExecutionContext executionContext, CustomerVerificationDTO customerVerificationDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, customerVerificationDTO);
            this.customerVerificationDTO = customerVerificationDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// method will insert the verification code to storage
        /// also it will send the same verification code to customer email or phone.
        /// </summary>
        public void GenerateVerificationRecord(int customerId, string loginId, string macAddress, bool sendVerificationCode = false, SqlTransaction sqlTransaction = null, int tokenExpiry = 14600)
        {
            try
            {
                log.LogMethodEntry(customerId, loginId, macAddress);

                if (customerId == -1)
                    throw new Exception("Customer Id is InValid");

                CustomerBL customerBL = new CustomerBL(executionContext, customerId, true, sqlTransaction: sqlTransaction);
                CustomerDTO customerDTO = customerBL.CustomerDTO;

                if (customerDTO.Id == -1)
                    throw new Exception("Customer Id is InValid");

                ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction();
                using (Utilities parafaitUtility = new Utilities(parafaitDBTrx.GetConnection()))
                {
                    parafaitUtility.ParafaitEnv.LoginID = executionContext.GetUserId();
                    parafaitUtility.ParafaitEnv.User_Id = executionContext.GetUserPKId();
                    parafaitUtility.ParafaitEnv.SetPOSMachine(macAddress, macAddress);
                    parafaitUtility.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
                    parafaitUtility.ExecutionContext.SetIsCorporate(executionContext.GetIsCorporate());
                    parafaitUtility.ExecutionContext.SetSiteId(executionContext.GetSiteId());
                    parafaitUtility.ExecutionContext.SetUserId(executionContext.GetUserId());
                    parafaitUtility.ParafaitEnv.Initialize();

                    string verificationCode = parafaitUtility.GenerateRandomNumber(6, Utilities.RandomNumberType.Numeric);

                    customerVerificationDTO = new CustomerVerificationDTO(-1,
                                                                            -1,
                                                                            customerDTO.ProfileId,
                                                                            parafaitUtility.ParafaitEnv.POSMachine,
                                                                            verificationCode,
                                                                            executionContext.GetIsCorporate() ? parafaitUtility.getServerTime().AddMinutes(15) : new DateTime?(),
                                                                            true,
                                                                            loginId,
                                                                            DateTime.Now,
                                                                            loginId,
                                                                            DateTime.Now,
                                                                            executionContext.GetSiteId(),
                                                                            -1,
                                                                            false,
                                                                            "");
                    customerVerificationDTO.ProfileDTO = customerDTO.ProfileDTO;

                    this.Save(sqlTransaction);

                    if (sendVerificationCode)
                    {
                        if ((string.IsNullOrEmpty(customerDTO.Email)) && (string.IsNullOrEmpty(customerDTO.PhoneNumber)))
                        {
                            throw new Exception("Send Customer Verification Code Failed. Phone number and Email Id not found");
                        }

                        if (customerVerificationDTO.Id != -1)
                        {
                            customerDTO.CustomerVerificationDTO = customerVerificationDTO;

                            string SMSGateway = ParafaitDefaultContainerList.GetParafaitDefault(parafaitUtility.ExecutionContext, "SMS_GATEWAY");
                            log.Debug("SMSGateway:" + SMSGateway);
                            Messaging msg = new Messaging(parafaitUtility);
                            try
                            {
                                CustomerEventsBL customerEventsBL = new CustomerEventsBL(executionContext, ParafaitFunctionEvents.CUSTOMER_VERIFICATION_EVENT, customerDTO, sqlTransaction);
                                if (!string.IsNullOrEmpty(customerDTO.Email))
                                {
                                    //string body = "Dear " + customerDTO.FirstName + ",";
                                    //body += Environment.NewLine + Environment.NewLine;
                                    //body += "Your registration verification code is " + verificationCode + ".";
                                    //body += Environment.NewLine + Environment.NewLine;
                                    //body += "Thank you";
                                    //body += Environment.NewLine;
                                    //body += parafaitUtility.ParafaitEnv.SiteName;
                                    try
                                    {
                                        ////msg.SendEMailSynchronous(customerDTO.Email, "", parafaitUtility.ParafaitEnv.SiteName + " - customer registration verification", body);

                                        //int messagingClientId = -1;
                                        //MessagingClientFunctionLookUpListBL messagingClientFunctionLookUpListBL = new MessagingClientFunctionLookUpListBL(executionContext);
                                        //List<MessagingClientFunctionLookUpDTO> messagingClientFunctionLookUpDTO = messagingClientFunctionLookUpListBL.GetMessagingClientDTOListByFunctionName("CUSTOMER_FUNCTIONS_LOOKUP", "CUSTOMER_VERIFICATION", "E");
                                        //if (messagingClientFunctionLookUpDTO != null && messagingClientFunctionLookUpDTO.Any())
                                        //    messagingClientId = messagingClientFunctionLookUpDTO[0].MessagingClientDTO.ClientId;

                                        ////"Customer verification Email"
                                        //MessagingRequestDTO messagingRequestDTO = new MessagingRequestDTO(-1, null, "Customer Verification", "E", customerDTO.Email, "", "", null, null, null, null,
                                        //     parafaitUtility.ParafaitEnv.SiteName + " - customer registration verification", body, customerDTO.Id, null, "", true, "", "", messagingClientId, false, "");


                                        //MessagingRequestBL messagingRequestBL = new MessagingRequestBL(executionContext, messagingRequestDTO);
                                        //messagingRequestBL.Save(sqlTransaction);

                                        customerEventsBL.SendMessage(MessagingClientDTO.MessagingChanelType.EMAIL, sqlTransaction);

                                    }
                                    catch (Exception ex)
                                    {
                                        log.Debug("Send mail failed " + ex.Message);
                                        log.Error("Send mail failed " + customerDTO.Email + ":" + ex.Message);
                                    }
                                }

                                if (!string.IsNullOrEmpty(customerDTO.PhoneNumber) && (SMSGateway != MessagingClientFactory.MessagingClients.None.ToString()))
                                {
                                    //string body = "Dear " + customerDTO.FirstName + ", ";
                                    //body += "Your registration verification code is " + verificationCode + ". ";
                                    //body += "Thank you. ";
                                    //body += parafaitUtility.ParafaitEnv.SiteName;
                                    try
                                    {
                                        //msg.sendOTPSMSSynchronous(customerDTO.PhoneNumber, body);
                                        //int messagingClientId = -1;
                                        //MessagingClientFunctionLookUpListBL messagingClientFunctionLookUpListBL = new MessagingClientFunctionLookUpListBL(executionContext);
                                        //List<MessagingClientFunctionLookUpDTO> messagingClientFunctionLookUpDTO = messagingClientFunctionLookUpListBL.GetMessagingClientDTOListByFunctionName("CUSTOMER_FUNCTIONS_LOOKUP", "CUSTOMER_VERIFICATION", "S");
                                        //if (messagingClientFunctionLookUpDTO != null && messagingClientFunctionLookUpDTO.Any())
                                        //    messagingClientId = messagingClientFunctionLookUpDTO[0].MessagingClientDTO.ClientId;

                                        ////"Customer verification Email"
                                        //MessagingRequestDTO messagingRequestDTO = new MessagingRequestDTO(-1, null, "Customer Verification", "S", customerDTO.Email, "", "", null, null, null, null,
                                        //     "Verification Code ", body, customerDTO.Id, null, "", true, "", "", messagingClientId, false, "");
                                        //MessagingRequestBL messagingRequestBL = new MessagingRequestBL(executionContext, messagingRequestDTO);
                                        //messagingRequestBL.Save(sqlTransaction);
                                        customerEventsBL.SendMessage(MessagingClientDTO.MessagingChanelType.SMS, sqlTransaction);
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Debug("Send sms failed " + ex.Message);
                                        log.Error("Send mail failed " + customerDTO.PhoneNumber + ":" + ex.Message);
                                    }

                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error("Send Customer Verification Code Failed" + ex.Message);
                                throw new Exception("Send Customer Verification Code Failed");
                            }
                        }
                        else
                        {
                            log.Error("Send Customer Verification Code Failed as verification ID not found");
                            throw new Exception("Send Customer Verification Code Failed as verification ID not found");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
        }

        /// <summary>
        /// Saves the CustomerVerification
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (customerVerificationDTO.IsActive)
            {
                List<ValidationError> validationErrorList = Validate(sqlTransaction);
                if (validationErrorList.Count > 0)
                {
                    log.Error("Customer verification failed for " + customerVerificationDTO.Id + ":Profile" + customerVerificationDTO.ProfileId + "code" + customerVerificationDTO.VerificationCode);
                    throw new ValidationException("Validation Failed", validationErrorList);
                }
                log.Info("Customer verification succeded for " + customerVerificationDTO.Id + ":Profile" + customerVerificationDTO.ProfileId);
            }
            ProfileBL profileBL = new ProfileBL(executionContext, customerVerificationDTO.ProfileDTO);
            profileBL.Save(sqlTransaction);
            if (customerVerificationDTO.ProfileId == -1)
            {
                customerVerificationDTO.ProfileId = customerVerificationDTO.ProfileDTO.Id;
            }
            CustomerVerificationDataHandler customerVerificationDataHandler = new CustomerVerificationDataHandler(sqlTransaction);
            if (customerVerificationDTO.Id < 0)
            {
                customerVerificationDTO = customerVerificationDataHandler.InsertCustomerVerification(customerVerificationDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                customerVerificationDTO.AcceptChanges();
            }
            else
            {
                if (customerVerificationDTO.IsChanged)
                {
                    customerVerificationDTO = customerVerificationDataHandler.UpdateCustomerVerification(customerVerificationDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    customerVerificationDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public CustomerVerificationDTO CustomerVerificationDTO
        {
            get
            {
                return customerVerificationDTO;
            }
        }

        /// <summary>
        /// Validates the customer relationship DTO
        /// </summary>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (string.IsNullOrWhiteSpace(customerVerificationDTO.VerificationCode))
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Verification Code"));
                validationErrorList.Add(new ValidationError("CustomerVerification", "VerificationCode", errorMessage));
                log.LogMethodExit(validationErrorList);
                return validationErrorList;
            }

            LookupValuesList serverTimeObject = new LookupValuesList(executionContext);

            CustomerVerificationListBL customerVerificationListBL = new CustomerVerificationListBL(executionContext);
            List<KeyValuePair<CustomerVerificationDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomerVerificationDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<CustomerVerificationDTO.SearchByParameters, string>(CustomerVerificationDTO.SearchByParameters.VERIFICATION_CODE, customerVerificationDTO.VerificationCode));
            searchParameters.Add(new KeyValuePair<CustomerVerificationDTO.SearchByParameters, string>(CustomerVerificationDTO.SearchByParameters.IS_ACTIVE, "1"));
            List<CustomerVerificationDTO> customerVerificationDTOList = customerVerificationListBL.GetCustomerVerificationDTOList(searchParameters, false, false, sqlTransaction);
            if (customerVerificationDTOList != null && customerVerificationDTOList.Any())
            {
                customerVerificationDTOList = customerVerificationDTOList.OrderByDescending(x => x.CreationDate).ToList();
                serverTimeObject = new LookupValuesList(executionContext);
                CustomerVerificationDTO savedCustomerVerificationDTO = customerVerificationDTOList[0];
                if (!savedCustomerVerificationDTO.CreatedBy.Equals("AppReview", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (savedCustomerVerificationDTO.Id != customerVerificationDTO.Id && 
                        (savedCustomerVerificationDTO.ExpiryDate == null || savedCustomerVerificationDTO.ExpiryDate > serverTimeObject.GetServerDateTime()))
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 1449);
                        validationErrorList.Add(new ValidationError("CustomerVerification", "VerificationCode", errorMessage));
                        log.Error(string.Join(":", validationErrorList.ToString()));
                        log.LogMethodExit(validationErrorList);
                        return validationErrorList;
                    }
                    else if(savedCustomerVerificationDTO.ExpiryDate < serverTimeObject.GetServerDateTime())
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 1450);
                        validationErrorList.Add(new ValidationError("CustomerVerification", "Expired", errorMessage));
                        log.Error(string.Join(":", validationErrorList.ToString()));
                        log.LogMethodExit(validationErrorList);
                        return validationErrorList;
                    }
                }
            }
            else if(this.customerVerificationDTO.Id > -1)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1449);
                validationErrorList.Add(new ValidationError("CustomerVerification", "VerificationCode", errorMessage));
                log.Error(string.Join(":", validationErrorList.ToString()));
                log.LogMethodExit(validationErrorList);
                return validationErrorList;
            }
            ProfileBL profileBL = new ProfileBL(executionContext, customerVerificationDTO.ProfileDTO);
            validationErrorList.AddRange(profileBL.Validate());
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
    }

    /// <summary>
    /// Manages the list of CustomerVerification
    /// </summary>
    public class CustomerVerificationListBL
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public CustomerVerificationListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the CustomerVerification list
        /// </summary>
        public List<CustomerVerificationDTO> GetCustomerVerificationDTOList(List<KeyValuePair<CustomerVerificationDTO.SearchByParameters, string>> searchParameters,
            bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);
            CustomerVerificationDataHandler customerVerificationDataHandler = new CustomerVerificationDataHandler(sqlTransaction);
            List<CustomerVerificationDTO> customerVerificationDTOList = customerVerificationDataHandler.GetCustomerVerificationDTOList(searchParameters);
            if (loadChildRecords)
            {
                if (customerVerificationDTOList != null && customerVerificationDTOList.Count > 0)
                {
                    CustomerVerificationBuilderBL customerVerificationBuilderBL = new CustomerVerificationBuilderBL(executionContext);
                    customerVerificationBuilderBL.Build(customerVerificationDTOList, activeChildRecords, sqlTransaction);
                }
            }
            log.LogMethodExit(customerVerificationDTOList);
            return customerVerificationDTOList;
        }

        /// <summary>
        /// Get the CustomerVerification list 
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<CustomerVerificationDTO> GetCustomerVerificationList(List<KeyValuePair<CustomerVerificationDTO.SearchByParameters, string>> searchParameters,
           bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);
            ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction();

            CustomerVerificationDataHandler customerVerificationDataHandler = new CustomerVerificationDataHandler(sqlTransaction);
            List<CustomerVerificationDTO> customerVerificationDTOList = customerVerificationDataHandler.GetCustomerVerificationDTOList(searchParameters);

            // Check for the Token Expiry
            LookupValuesList serverTimeObject = new LookupValuesList(executionContext);
            if (customerVerificationDTOList.OrderByDescending(k => k.ExpiryDate).FirstOrDefault().ExpiryDate < serverTimeObject.GetServerDateTime())
            {
                customerVerificationDTOList.Find(m => m.ResendEmailToken == false).ResendEmailToken = true;
            }

            if (loadChildRecords)
            {
                if (customerVerificationDTOList != null && customerVerificationDTOList.Count > 0)
                {
                    CustomerVerificationBuilderBL customerVerificationBuilderBL = new CustomerVerificationBuilderBL(executionContext);
                    customerVerificationBuilderBL.Build(customerVerificationDTOList, activeChildRecords, sqlTransaction);
                }
            }
            log.LogMethodExit(customerVerificationDTOList);
            return customerVerificationDTOList;
        }
    }

    /// <summary>
    /// Builds the complex CustomerVerification entity structure
    /// </summary>
    public class CustomerVerificationBuilderBL
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public CustomerVerificationBuilderBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Builds the complex customerVerification DTO structure
        /// </summary>
        /// <param name="customerVerificationDTO">CustomerVerification dto</param>
        /// <param name="activeChildRecords">whether to load only active child records</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public void Build(CustomerVerificationDTO customerVerificationDTO, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(customerVerificationDTO, activeChildRecords, sqlTransaction);
            if (customerVerificationDTO != null && customerVerificationDTO.Id != -1)
            {
                if (customerVerificationDTO.ProfileId != -1)
                {
                    ProfileBL profileBL = new ProfileBL(executionContext, customerVerificationDTO.ProfileId, activeChildRecords, activeChildRecords, sqlTransaction);
                    customerVerificationDTO.ProfileDTO = profileBL.ProfileDTO;
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Builds the complex customerVerificationDTO structure
        /// </summary>
        /// <param name="customerVerificationDTOList">CustomerVerification dto list</param>
        /// <param name="activeChildRecords">whether to load only active child records</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public void Build(List<CustomerVerificationDTO> customerVerificationDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(customerVerificationDTOList, activeChildRecords, sqlTransaction);
            if (customerVerificationDTOList != null && customerVerificationDTOList.Count > 0)
            {
                StringBuilder profileIdListStringBuilder = new StringBuilder("");
                Dictionary<int, CustomerVerificationDTO> profileIdCustomerVerificationDictionary = new Dictionary<int, CustomerVerificationDTO>();
                string profileIdList;
                for (int i = 0; i < customerVerificationDTOList.Count; i++)
                {
                    if (customerVerificationDTOList[i].ProfileId != -1)
                    {
                        profileIdCustomerVerificationDictionary.Add(customerVerificationDTOList[i].ProfileId, customerVerificationDTOList[i]);
                    }

                    if (i != 0)
                    {
                        profileIdListStringBuilder.Append(",");
                    }
                    profileIdListStringBuilder.Append(customerVerificationDTOList[i].ProfileId.ToString());
                }
                profileIdList = profileIdListStringBuilder.ToString();
                ProfileListBL profileListBL = new ProfileListBL(executionContext);
                List<KeyValuePair<ProfileDTO.SearchByParameters, string>> searchProfileParams = new List<KeyValuePair<ProfileDTO.SearchByParameters, string>>();
                searchProfileParams.Add(new KeyValuePair<ProfileDTO.SearchByParameters, string>(ProfileDTO.SearchByParameters.ID_LIST, profileIdList));
                if (activeChildRecords)
                {
                    searchProfileParams.Add(new KeyValuePair<ProfileDTO.SearchByParameters, string>(ProfileDTO.SearchByParameters.IS_ACTIVE, "1"));
                }
                List<ProfileDTO> profileDTOList = profileListBL.GetProfileDTOList(searchProfileParams, true, activeChildRecords, sqlTransaction);
                if (profileDTOList != null)
                {
                    foreach (var profileDTO in profileDTOList)
                    {
                        if (profileIdCustomerVerificationDictionary.ContainsKey(profileDTO.Id))
                        {
                            profileIdCustomerVerificationDictionary[profileDTO.Id].ProfileDTO = profileDTO;
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}
