/********************************************************************************************
 * Project Name - Customer Signed Waiver Header BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        26-Sep-2019      Deeksha        Created for waiver phase 2
 *2.140.0       14-Sep-2021      Guru S A       Waiver mapping UI enhancements
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.DBSynch;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Waiver;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer.Waivers
{
    public class CustomerSignedWaiverHeaderBL
    {
        private CustomerSignedWaiverHeaderDTO customerSignedWaiverHeaderDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="customerSignedWaiverHeaderID">customerSignedWaiverHeaderID</param>
        /// <param name="loadChildRecords">loadChildRecords</param>
        /// <param name="activeChildRecords">activeChildRecords</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public CustomerSignedWaiverHeaderBL(ExecutionContext executionContext, int customerSignedWaiverHeaderID,
           bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(executionContext, customerSignedWaiverHeaderID, loadChildRecords, activeChildRecords, sqlTransaction);
            this.executionContext = executionContext;
            CustomerSignedWaiversHeaderDataHandler customerSignedWaiversHeaderDataHandler = new CustomerSignedWaiversHeaderDataHandler(sqlTransaction);
            customerSignedWaiverHeaderDTO = customerSignedWaiversHeaderDataHandler.GetCustomerSignedWaiverHeaderDTO(customerSignedWaiverHeaderID);
            if (customerSignedWaiverHeaderDTO != null && loadChildRecords)
            {
                LoadCustomerSignedWaiverDTOList(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates CustomerSignedWaiverHeaderBL object using the CustomerSignedWaiverHeaderDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="accountCreditPlusDTO">AccountCreditPlusDTO object</param>
        public CustomerSignedWaiverHeaderBL(ExecutionContext executionContext, CustomerSignedWaiverHeaderDTO customerSignedWaiverHeaderDTO)
        {
            log.LogMethodEntry(executionContext, customerSignedWaiverHeaderDTO);
            this.executionContext = executionContext;
            this.customerSignedWaiverHeaderDTO = customerSignedWaiverHeaderDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// get CustomerSignedWaiverHeaderDTO Object
        /// </summary>
        public CustomerSignedWaiverHeaderDTO GetCustomerSignedWaiverHeaderDTO
        {
            get { return customerSignedWaiverHeaderDTO; }
        }

        public List<ValidationError> Validate()
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            string errorMessage = string.Empty;
            if (customerSignedWaiverHeaderDTO == null)
            {
                errorMessage = MessageContainerList.GetMessage(executionContext, 2298, (MessageContainerList.GetMessage(executionContext, "customerSignedWaiverHeaderDTO"))); //Cannot proceed customerSignedWaiverHeaderDTO record is Empty.
                validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Waiver"), MessageContainerList.GetMessage(executionContext, "customerSignedWaiverHeaderDTO"), errorMessage));
            }
            try
            {
                EnumValueConverter enumValueConverter = new EnumValueConverter(typeof(WaiverSignatureDTO.WaiverSignatureChannel));
                enumValueConverter.FromString(customerSignedWaiverHeaderDTO.Channel.ToUpper());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Waiver"), MessageContainerList.GetMessage(executionContext, "customerSignedWaiverHeaderDTO"), MessageContainerList.GetMessage(executionContext, 4088, customerSignedWaiverHeaderDTO.Channel)));
                //&1 is not a valid channel for waiver signing
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Saves the CustomerSignedWaiverHeader
        /// Checks if the CustomerSignedWaiverHeaderId is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            CustomerSignedWaiversHeaderDataHandler customerSignedWaiversHeaderDataHandler = new CustomerSignedWaiversHeaderDataHandler(sqlTransaction);
            Validate();
            if (customerSignedWaiverHeaderDTO.CustomerSignedWaiverHeaderId < 0)
            {
                //if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "WAIVER_CODE_IS_MANDATORY_TO_FETCH_CUSTOMER") == true)
                //{
                customerSignedWaiverHeaderDTO.WaiverCode = GenerateWaiverCode(sqlTransaction);
                //}
                customerSignedWaiverHeaderDTO = customerSignedWaiversHeaderDataHandler.InsertCustomerSignedWaiverHeader(customerSignedWaiverHeaderDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                customerSignedWaiverHeaderDTO.AcceptChanges();
                CreateRoamingData(sqlTransaction);
            }
            else
            {
                if (customerSignedWaiverHeaderDTO.IsChanged)
                {
                    customerSignedWaiverHeaderDTO = customerSignedWaiversHeaderDataHandler.UpdateCustomerSignedWaiverHeader(customerSignedWaiverHeaderDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    customerSignedWaiverHeaderDTO.AcceptChanges();
                    CreateRoamingData(sqlTransaction);
                }
            }
            log.LogMethodExit();
        }

        private string GenerateWaiverCode(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            string waiverCodeValue = string.Empty;
            bool waiverCodeGenerated = false;
            CustomerSignedWaiversHeaderDataHandler customerSignedWaiversHeaderDataHandler = new CustomerSignedWaiversHeaderDataHandler(sqlTrx);
            do
            {
                RandomString randomString = new RandomString(6);
                waiverCodeValue = randomString.Value;
                if (customerSignedWaiversHeaderDataHandler.AlreadyUsedActiveWaiverCode(waiverCodeValue) == false)
                {
                    waiverCodeGenerated = true;
                }
                else
                {
                    waiverCodeGenerated = false;
                }
            }
            while (waiverCodeGenerated == false);

            log.LogMethodExit(waiverCodeValue);
            return waiverCodeValue;
        }

        public void LoadCustomerSignedWaiverDTOList(bool activeChildRecords, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords);
            CustomerSignedWaiverListBL customerSignedWaiverListBL = new CustomerSignedWaiverListBL(executionContext);
            List<KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>> searchParameters = new List<KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>>();
            searchParameters.Add(new KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>(CustomerSignedWaiverDTO.SearchByCSWParameters.CUSTOMER_SIGNED_WAIVER_HEADER_ID, customerSignedWaiverHeaderDTO.CustomerSignedWaiverHeaderId.ToString()));
            if (activeChildRecords)
            {
                searchParameters.Add(new KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>(CustomerSignedWaiverDTO.SearchByCSWParameters.IS_ACTIVE, "1".ToString()));
            }
            List<CustomerSignedWaiverDTO> customerSignedWaivers = customerSignedWaiverListBL.GetAllCustomerSignedWaiverList(searchParameters, false, null, sqlTransaction);
            if (customerSignedWaivers != null && customerSignedWaivers.Count > 0)
            {
                customerSignedWaiverHeaderDTO.CustomerSignedWaiverDTOList = customerSignedWaivers;
            }
            log.LogMethodExit();
        }

        private void CreateRoamingData(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            CustomerSignedWaiversHeaderDataHandler customerSignedWaiversHeaderDataHandler = new CustomerSignedWaiversHeaderDataHandler(sqlTransaction);
            CustomerSignedWaiverHeaderDTO updatedcustomerSignedWaiversHeaderDTO = customerSignedWaiversHeaderDataHandler.GetCustomerSignedWaiverHeaderDTO(customerSignedWaiverHeaderDTO.CustomerSignedWaiverHeaderId);
            DBSynchLogService dBSynchLogService = new DBSynchLogService(executionContext, "CustomerSignedWaiverHeader", updatedcustomerSignedWaiversHeaderDTO.Guid, updatedcustomerSignedWaiversHeaderDTO.SiteId);
            dBSynchLogService.CreateRoamingDataForCustomer(sqlTransaction);
            log.LogMethodExit();
        }
    }
    public class CustomerSignedWaiverHeaderListBL
    {
        private List<CustomerSignedWaiverHeaderDTO> customerSignedWaiverHeaderList;
        private ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public CustomerSignedWaiverHeaderListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.customerSignedWaiverHeaderList = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="customerSignedWaiverList"></param>
        /// <param name="executionContext"></param>
        public CustomerSignedWaiverHeaderListBL(ExecutionContext executionContext, List<CustomerSignedWaiverHeaderDTO> customerSignedWaiverHeaderList)
        {
            log.LogMethodEntry(executionContext, customerSignedWaiverHeaderList);
            this.executionContext = executionContext;
            this.customerSignedWaiverHeaderList = customerSignedWaiverHeaderList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Save or update records with inner collections
        /// </summary>
        public void SaveCustomerSignedWaiverHeader()
        {
            log.LogMethodEntry();
            try
            {
                if (customerSignedWaiverHeaderList != null)
                {
                    foreach (CustomerSignedWaiverHeaderDTO customerSignedWaiverHeaderDTO in customerSignedWaiverHeaderList)
                    {
                        CustomerSignedWaiverHeaderBL customerSignedWaiverHeaderBL = new CustomerSignedWaiverHeaderBL(executionContext, customerSignedWaiverHeaderDTO);
                        customerSignedWaiverHeaderBL.Save();
                    }
                }

                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Returns the Customer Signed Waiver Header List
        /// </summary>
        public List<CustomerSignedWaiverHeaderDTO> GetAllCustomerSignedWaiverList(List<KeyValuePair<CustomerSignedWaiverHeaderDTO.SearchByCSWHeaderParameters, string>> searchParameters,
                                                   bool loadChild = false, bool loadActiveChild = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChild, loadActiveChild);
            CustomerSignedWaiversHeaderDataHandler customerSignedWaiverHeaderDataHandler = new CustomerSignedWaiversHeaderDataHandler(sqlTransaction);
            List<CustomerSignedWaiverHeaderDTO> customerSignedWaiverHeaderList = customerSignedWaiverHeaderDataHandler.GetAllCustomerSignedWaiverHeaderList(searchParameters);
            if (customerSignedWaiverHeaderList != null && customerSignedWaiverHeaderList.Count > 0)
            {
                if (loadChild)
                {
                    string ids = string.Empty;
                    foreach (CustomerSignedWaiverHeaderDTO signedWaiverHeader in customerSignedWaiverHeaderList)
                    {
                        ids = ids + signedWaiverHeader.CustomerSignedWaiverHeaderId + ",";
                    }
                    if (!string.IsNullOrEmpty(ids))
                    {
                        ids = ids.Substring(0, ids.Length - 1);
                    }
                    CustomerSignedWaiverListBL customerSignedWaiverListBL = new CustomerSignedWaiverListBL(executionContext);
                    List<KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>> searchParameter = new List<KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>>();
                    searchParameter.Add(new KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>(CustomerSignedWaiverDTO.SearchByCSWParameters.CUSTOMER_SIGNED_WAIVER_HEADER_ID_LIST, ids));
                    if (loadActiveChild)
                    {
                        searchParameter.Add(new KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>(CustomerSignedWaiverDTO.SearchByCSWParameters.IS_ACTIVE, "1"));
                    }
                    List<CustomerSignedWaiverDTO> customerSignedWaivers = customerSignedWaiverListBL.GetAllCustomerSignedWaiverList(searchParameter, false, null, sqlTransaction);
                    if (customerSignedWaivers != null && customerSignedWaivers.Count > 0)
                    {
                        foreach (CustomerSignedWaiverHeaderDTO signedWaiver in customerSignedWaiverHeaderList)
                        {
                            signedWaiver.CustomerSignedWaiverDTOList = customerSignedWaivers.FindAll(x => x.CustomerSignedWaiverHeaderId == signedWaiver.CustomerSignedWaiverHeaderId);
                        }
                    }
                }

            }
            log.LogMethodExit(customerSignedWaiverHeaderList);
            return customerSignedWaiverHeaderList;
        }



        public List<CustomerDTO> GetCustomersbyWaiverCode(string waiverCode)
        {
            log.LogMethodEntry(waiverCode);
            List<CustomerDTO> customerDTOList = null;
            if (string.IsNullOrEmpty(waiverCode))
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2318));//'Please enter the Waiver code'
            }
            CustomerSignedWaiverListBL customerSignedWaiverListBL = new CustomerSignedWaiverListBL(executionContext);
            List<KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>> searchParam = new List<KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>>();
            //searchParam.Add(new KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>(CustomerSignedWaiverDTO.SearchByCSWParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParam.Add(new KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>(CustomerSignedWaiverDTO.SearchByCSWParameters.WAIVER_CODE, waiverCode));
            List<CustomerSignedWaiverDTO> customerSignedWaiverDTOList = customerSignedWaiverListBL.GetAllCustomerSignedWaiverList(searchParam);
            if (customerSignedWaiverDTOList != null && customerSignedWaiverDTOList.Any())
            {
                LookupValuesList serverTimeObj = new LookupValuesList(executionContext);
                DateTime serverTime = serverTimeObj.GetServerDateTime();
                List<int> customerIdList = customerSignedWaiverDTOList.Where(csw => csw.IsActive == true
                                                                                    && (csw.ExpiryDate == null
                                                                                        || csw.ExpiryDate >= serverTime)
                                                                            ).Select(cws => cws.SignedFor).Distinct().ToList();
                if (customerIdList != null && customerIdList.Any())
                {
                    CustomerListBL customerList = new CustomerListBL(executionContext);
                    customerDTOList = customerList.GetCustomerDTOList(customerIdList, true, true, true);
                }
            }
            log.LogMethodExit(customerDTOList);
            return customerDTOList;
        }

        /// <summary>
        /// SendWaiverEmail - Allowed tags @siteName, @CustomerName, @EmailId, @SignedBy, @SignedDate, @WaiverCode, @Channel, @SignedWaiverDetails, @QRCodeForwaiverCode
        /// </summary>
        /// <param name="customerDTO"></param>
        /// <param name="custSignedWaiverHeaderId"></param>
        /// <param name="utilities"></param>
        /// <param name="sqlTrx"></param>
        public void SendWaiverEmail(CustomerDTO customerDTO, int custSignedWaiverHeaderId, Core.Utilities.Utilities utilities, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry("customerDTO", custSignedWaiverHeaderId);
            if (customerDTO != null)
            {
                if (string.IsNullOrEmpty(customerDTO.Email) == false)
                {
                    string emailTemplateName = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "SEND_SIGNED_WAIVER_COPY_EMAIL_TEMPLATE");
                    EmailTemplateDTO emailTemplateDTO = null;
                    if (string.IsNullOrEmpty(emailTemplateName) == false)
                    {
                        EmailTemplateListBL emailTemplateListBL = new EmailTemplateListBL(executionContext);
                        List<KeyValuePair<EmailTemplateDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<EmailTemplateDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<EmailTemplateDTO.SearchByParameters, string>(EmailTemplateDTO.SearchByParameters.NAME, emailTemplateName));
                        searchParameters.Add(new KeyValuePair<EmailTemplateDTO.SearchByParameters, string>(EmailTemplateDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<EmailTemplateDTO> emailTemplateDTOList = emailTemplateListBL.GetEmailTemplateDTOList(searchParameters);
                        if (emailTemplateDTOList != null && emailTemplateDTOList.Any())
                        {
                            emailTemplateDTO = emailTemplateDTOList[0];
                        }
                        if (emailTemplateDTO != null && emailTemplateDTO.EmailTemplateId > -1)
                        {

                            if (custSignedWaiverHeaderId > -1)
                            {

                                CustomerSignedWaiverHeaderBL customerSignedWaiverHeaderBL = new CustomerSignedWaiverHeaderBL(executionContext, custSignedWaiverHeaderId, true, true, sqlTrx);
                                CustomerSignedWaiverHeaderDTO customerSignedWaiverHeaderDTO = customerSignedWaiverHeaderBL.GetCustomerSignedWaiverHeaderDTO;
                                BuildEmailBodyAndSend(customerDTO, emailTemplateDTO, customerSignedWaiverHeaderDTO, utilities, sqlTrx);

                            }
                        }
                        else
                        {
                            log.Error("SEND_SIGNED_WAIVER_COPY_EMAIL_TEMPLATE is not set with valid email template");
                        }
                    }
                    else
                    {
                        log.Error("SEND_SIGNED_WAIVER_COPY_EMAIL_TEMPLATE is not set with valid email template");
                    }
                }
                else
                {
                    log.Error("No email id provided to send waiver email");
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  ReSendWaiverEmail - Allowed tags @siteName, @CustomerName, @EmailId, @SignedBy, @SignedDate, @WaiverCode, @Channel, @SignedWaiverDetails, @QRCodeForwaiverCode
        /// </summary>
        /// <param name="customerSignedWaiverDTOList"></param>
        /// <param name="utilities"></param>
        /// <param name="sqlTrx"></param>
        public void ReSendWaiverEmail(List<CustomerSignedWaiverDTO> customerSignedWaiverDTOList, Core.Utilities.Utilities utilities, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(customerSignedWaiverDTOList);
            if (customerSignedWaiverDTOList != null && customerSignedWaiverDTOList.Any())
            {
                List<int> custSignedWaiverHeaderIdList = customerSignedWaiverDTOList.Select(csw => csw.CustomerSignedWaiverHeaderId).Distinct().ToList();
                if (custSignedWaiverHeaderIdList != null && custSignedWaiverHeaderIdList.Any())
                {
                    for (int i = 0; i < custSignedWaiverHeaderIdList.Count; i++)
                    {
                        List<CustomerSignedWaiverDTO> signedWaiverList = customerSignedWaiverDTOList.Where(csw => csw.CustomerSignedWaiverHeaderId == custSignedWaiverHeaderIdList[i]).ToList();
                        if (signedWaiverList != null && signedWaiverList.Any())
                        {
                            CustomerBL customerBL = new CustomerBL(executionContext, signedWaiverList[0].SignedBy, true, true, sqlTrx);
                            CustomerDTO customerDTO = customerBL.CustomerDTO;
                            CustomerSignedWaiverHeaderBL customerSignedWaiverHeaderBL = new CustomerSignedWaiverHeaderBL(executionContext, custSignedWaiverHeaderIdList[i], false, true, sqlTrx);
                            CustomerSignedWaiverHeaderDTO customerSignedWaiverHeaderDTO = customerSignedWaiverHeaderBL.GetCustomerSignedWaiverHeaderDTO;
                            customerSignedWaiverHeaderDTO.CustomerSignedWaiverDTOList = signedWaiverList;

                            if (string.IsNullOrEmpty(customerDTO.Email) == false)
                            {
                                string emailTemplateName = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "SEND_SIGNED_WAIVER_COPY_EMAIL_TEMPLATE");
                                EmailTemplateDTO emailTemplateDTO = null;
                                if (string.IsNullOrEmpty(emailTemplateName) == false)
                                {
                                    EmailTemplateListBL emailTemplateListBL = new EmailTemplateListBL(executionContext);
                                    List<KeyValuePair<EmailTemplateDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<EmailTemplateDTO.SearchByParameters, string>>();
                                    searchParameters.Add(new KeyValuePair<EmailTemplateDTO.SearchByParameters, string>(EmailTemplateDTO.SearchByParameters.NAME, emailTemplateName));
                                    searchParameters.Add(new KeyValuePair<EmailTemplateDTO.SearchByParameters, string>(EmailTemplateDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                                    List<EmailTemplateDTO> emailTemplateDTOList = emailTemplateListBL.GetEmailTemplateDTOList(searchParameters);
                                    if (emailTemplateDTOList != null && emailTemplateDTOList.Any())
                                    {
                                        emailTemplateDTO = emailTemplateDTOList[0];
                                    }
                                    if (emailTemplateDTO != null && emailTemplateDTO.EmailTemplateId > -1)
                                    {
                                        if (customerSignedWaiverHeaderDTO.CustomerSignedWaiverHeaderId > -1)
                                        {
                                            BuildEmailBodyAndSend(customerDTO, emailTemplateDTO, customerSignedWaiverHeaderDTO, utilities, sqlTrx);
                                        }
                                    }
                                    else
                                    {
                                        log.Error("SEND_SIGNED_WAIVER_COPY_EMAIL_TEMPLATE is not set with valid email template");
                                    }
                                }
                                else
                                {
                                    log.Error("SEND_SIGNED_WAIVER_COPY_EMAIL_TEMPLATE is not set with valid email template");
                                }
                            }
                            else
                            {
                                log.Error("No email id provided to send waiver email");
                            }

                        }
                    }
                }

            }
            log.LogMethodExit();
        }


        private void BuildEmailBodyAndSend(CustomerDTO customerDTO, EmailTemplateDTO emailTemplateDTO, CustomerSignedWaiverHeaderDTO customerSignedWaiverHeaderDTO, Core.Utilities.Utilities utilities, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry();
            string body = emailTemplateDTO.EmailTemplate;
            body = GenerateWaiverEmailContent(body, customerDTO, customerSignedWaiverHeaderDTO, utilities, sqlTrx);
            List<string> attachFiles = GenerateWaiverAttachments(customerSignedWaiverHeaderDTO, utilities.ParafaitEnv.SiteId);
            string finalAttachment = string.Empty;
            if (attachFiles != null && attachFiles.Any())
            {
                string custEmailId = customerDTO.Email;
                custEmailId = custEmailId.Replace("@", "_").Replace(".", "_");
                string consolidatedFileName = WaiverCustomerUtils.StripNonAlphaNumericExceptUnderScore(custEmailId);
                consolidatedFileName = consolidatedFileName + WaiverCustomerUtils.StripNonAlphaNumericExceptUnderScore(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.ffff")) + ".pdf";
                //string consolidateFileWithPath = Path.GetTempPath() + consolidatedFileName;
                // ZipFiles.CreateZipFile(zipAttachment, attachFiles);
                WaiverCustomerUtils.CreateConsolidatedPDF(consolidatedFileName, attachFiles);
                WaiverCustomerUtils.WriteFileToServer(consolidatedFileName, utilities);
                finalAttachment = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "IMAGE_DIRECTORY") + "\\" + consolidatedFileName;
            }

            Messaging messaging = new Messaging(utilities);
            messaging.SendEMail(customerDTO.Email, emailTemplateDTO.Description, body, customerDTO.Id.ToString(), finalAttachment, null, null, null, MessageContainerList.GetMessage(utilities.ExecutionContext, "Waiver"));
            log.LogMethodExit();
        }

        private string GenerateWaiverEmailContent(string body, CustomerDTO customerDTO, CustomerSignedWaiverHeaderDTO customerSignedWaiverHeaderDTO, Core.Utilities.Utilities utilities, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(body, customerDTO, customerSignedWaiverHeaderDTO);
            if (customerDTO != null)
            {
                body = body.Replace("@siteName", utilities.ParafaitEnv.SiteName);
                body = body.Replace("@customerName", customerDTO.FirstName);
                body = body.Replace("@emailId", customerDTO.Email);
                body = body.Replace("@emailAddress", customerDTO.Email);
                body = body.Replace("@signedBy", customerDTO.FirstName + " " + (string.IsNullOrEmpty(customerDTO.LastName) ? string.Empty : customerDTO.LastName));
                if (customerSignedWaiverHeaderDTO != null)
                {
                    body = body.Replace("@signedDate", customerSignedWaiverHeaderDTO.SignedDate.ToString(utilities.ParafaitEnv.DATETIME_FORMAT));
                    body = body.Replace("@waiverCode", customerSignedWaiverHeaderDTO.WaiverCode);
                    body = body.Replace("@BarCodeForWaiverCode", GenerateBarCode(customerSignedWaiverHeaderDTO.WaiverCode));
                    body = body.Replace("@channel", customerSignedWaiverHeaderDTO.Channel);
                    List<CustomerSignedWaiverDTO> customerSignedWaiverDTOList = customerSignedWaiverHeaderDTO.CustomerSignedWaiverDTOList;
                    if (customerSignedWaiverDTOList != null && customerSignedWaiverDTOList.Any())
                    {
                        StringBuilder signedWaiverInfo = new StringBuilder(" ");
                        signedWaiverInfo.Append("<Table><TR>" +
                                                  "<TH>" + MessageContainerList.GetMessage(executionContext, "Signed For") + "</TH>" +
                                                  "<TH>" + MessageContainerList.GetMessage(executionContext, "Waiver Name") + "</TH>" +
                                                  "<TH>" + MessageContainerList.GetMessage(executionContext, "Expiry Date") + "</TH></TR>");
                        for (int i = 0; i < customerSignedWaiverDTOList.Count; i++)
                        {
                            signedWaiverInfo.Append("<TR><TD>" + customerSignedWaiverDTOList[i].SignedForName + "</TD>" +
                                              "<TD>" + customerSignedWaiverDTOList[i].WaiverName + "</TD>" +
                                              "<TD>" + (customerSignedWaiverDTOList[i].ExpiryDate == null ? string.Empty : ((DateTime)customerSignedWaiverDTOList[i].ExpiryDate).ToString(utilities.ParafaitEnv.DATETIME_FORMAT)) + "</TD></TR>");
                        }
                        signedWaiverInfo.Append("</Table>");
                        body = body.Replace("@signedWaiverDetails", signedWaiverInfo.ToString());
                    }
                }
            }
            log.LogMethodExit(body);
            return body;
        }

        private List<string> GenerateWaiverAttachments(CustomerSignedWaiverHeaderDTO customerSignedWaiverHeaderDTO, int siteId)
        {
            log.LogMethodEntry(customerSignedWaiverHeaderDTO);
            List<string> attachementFiles = new List<string>();
            List<SignedFileInformationDTO> signedFileInformationList = new List<SignedFileInformationDTO>();
            if (customerSignedWaiverHeaderDTO != null && customerSignedWaiverHeaderDTO.CustomerSignedWaiverDTOList != null && customerSignedWaiverHeaderDTO.CustomerSignedWaiverDTOList.Any())
            {
                foreach (CustomerSignedWaiverDTO item in customerSignedWaiverHeaderDTO.CustomerSignedWaiverDTOList)
                {
                    //just send one copy
                    if (signedFileInformationList.Exists(sfi => sfi.CustomerSignedWaiverHeaderId == item.CustomerSignedWaiverHeaderId &&
                                                        sfi.WaiverSetId == item.WaiverSetId && sfi.WaiverId == item.WaiverSetDetailId &&
                                                        sfi.SignedBy == item.SignedBy &&
                                                        ((string.IsNullOrWhiteSpace(item.SignedWaiverFileName) == false && sfi.SignedWaiverFileName == item.SignedWaiverFileName) || (string.IsNullOrWhiteSpace(item.SignedWaiverFileName) == true
                                                             && string.IsNullOrWhiteSpace(sfi.SignedWaiverFileName) == true))) == false)
                    {
                        signedFileInformationList.Add(new SignedFileInformationDTO(item.CustomerSignedWaiverHeaderId, item.WaiverSetId, item.WaiverSetDetailId, item.SignedBy, item.SignedFor, item.SignedWaiverFileName));

                        CustomerSignedWaiverBL customerSignedWaiverBL = new CustomerSignedWaiverBL(executionContext, item);
                        string fileWithPath = customerSignedWaiverBL.GetDecryptedWaiverFile(siteId);
                        if (string.IsNullOrEmpty(fileWithPath) == false)
                        {
                            attachementFiles.Add(fileWithPath);
                        }
                    }
                }
            }
            log.LogMethodExit(attachementFiles);
            return attachementFiles;
        }
        private string GenerateBarCode(string waiverCode)
        {
            log.LogMethodEntry(waiverCode);
            String waiverBarcode = string.Empty;
            waiverBarcode = GenericUtils.GenerateBarCodeB64ForString(waiverCode,2);
            if (string.IsNullOrWhiteSpace(waiverBarcode))
            {
                throw new Exception(MessageContainerList.GetMessage(executionContext, 4098, MessageContainerList.GetMessage(executionContext,"Waiver Code")));
                // "Sorry, unable to generate barcode for &1
            }
            log.LogMethodExit();
            return waiverBarcode;
        }

    }
}
