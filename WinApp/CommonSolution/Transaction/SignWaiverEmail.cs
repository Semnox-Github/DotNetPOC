/**********************************************************************************************************************************
 * Project Name - Transaction
 * Description  - Business Logic to send sign Waiver Email
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By      Remarks          
 **********************************************************************************************************************************                                        
 *2.80.0       28-Apr-2020      Guru S A        Created for send sign waiver email changes
 **********************************************************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Transaction
{
    public class SignWaiverEmail
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private Transaction transaction;
        private Utilities utilities;
        private const string WAIVERSETUP = "WAIVER_SETUP";
        private const string BASEURLFORSIGNWAIVER = "BASE_URL_FOR_SIGN_WAIVER";
        private const string RESERVATIONWAIVERURL = "RESERVATION_WAIVER_URL";
        private const string TRANSACTIONWAIVERURL = "TRANSACTION_WAIVER_URL";
        /// <summary>
        /// Parmaeterized constructor
        /// </summary>
        /// <param name="executionContext"></param> 
        /// <param name="transaction"></param>
        public SignWaiverEmail(ExecutionContext executionContext, Transaction transaction, Utilities utilities)
        {
            log.LogMethodEntry(executionContext, transaction, utilities);
            this.executionContext = executionContext;
            this.transaction = transaction;
            this.utilities = utilities;
            log.LogMethodExit();
        }
        /// <summary>
        ///  CanSendSignWaiverEmail - validates transaction data and returns error list if conditions to send waiver email are not met 
        /// </summary>
        public List<ValidationError> CanSendSignWaiverEmail(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (this.transaction == null)
            {
                validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Waiver"), MessageContainerList.GetMessage(executionContext, "Email"), MessageContainerList.GetMessage(executionContext, 2154,
                                                                       (MessageContainerList.GetMessage(executionContext, "Transaction")))));
            }
            else
            {
                if (transaction.IsWaiverSignaturePending())
                {
                    if (transaction.Trx_id == 0)
                    {
                        validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Waiver"), MessageContainerList.GetMessage(executionContext, "Email"),
                                                                    MessageContainerList.GetMessage(executionContext, 2656)));//"Please save the Transaction first"
                    }

                    if (transaction.Status == Transaction.TrxStatus.CANCELLED
                                                   || transaction.Status == Transaction.TrxStatus.SYSTEMABANDONED
                                                   || transaction.Status == Transaction.TrxStatus.BOOKING)
                    {
                        validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Waiver"), MessageContainerList.GetMessage(executionContext, "Email"),
                                                                    MessageContainerList.GetMessage(executionContext, 2658, MessageContainerList.GetMessage(executionContext, "Transaction"), transaction.Status)));//"Sorry unable to proceed. &1 is in &2 status"
                    }


                    bool reservationTransaction = transaction.IsReservationTransaction(sqlTransaction);

                    string emailTemplate = string.Empty;
                    if (reservationTransaction)
                    {
                        emailTemplate = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "RESERVATION_WAIVER_SIGNATURE_EMAIL_TEMPLATE");
                    }
                    else
                    {
                        emailTemplate = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "TRANSACTION_WAIVER_SIGNATURE_EMAIL_TEMPLATE");
                    }
                    if (string.IsNullOrEmpty(emailTemplate))
                    {
                        validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Waiver"), MessageContainerList.GetMessage(executionContext, "Email"),
                                                             MessageContainerList.GetMessage(executionContext, 2649)));//'Email template for Send sign waiver with URL is not defined'
                    }
                    else
                    {
                        EmailTemplateDTO emailTemplateDTO = new EmailTemplate(executionContext).GetEmailTemplate(emailTemplate, executionContext.GetSiteId());
                        if (emailTemplateDTO == null || emailTemplateDTO.EmailTemplateId == -1)
                        {
                            validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Waiver"), MessageContainerList.GetMessage(executionContext, "Email"),
                                                             MessageContainerList.GetMessage(executionContext, 2649)));//'Email template for Send sign waiver with URL is not defined'
                        }
                    }
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// SendWaiverSigningLink email
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void SendWaiverSigningLink(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(utilities, sqlTransaction);
            if (transaction != null)
            {
                LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParam = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                searchParam.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, WAIVERSETUP));
                searchParam.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, BASEURLFORSIGNWAIVER));
                searchParam.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchParam, sqlTransaction);
                string baseURLWaiver = string.Empty;
                if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                {
                    baseURLWaiver = lookupValuesDTOList[0].Description;
                }
                if (string.IsNullOrEmpty(baseURLWaiver) || string.IsNullOrEmpty(baseURLWaiver.Trim()))
                {
                    log.LogMethodExit("Base url for sign waiver is not defined. Skip send sign waiver email process");
                    return;
                }
                if (transaction.IsWaiverSignaturePending())
                {
                    List<ValidationError> validationErrorList = CanSendSignWaiverEmail(sqlTransaction);
                    if (validationErrorList != null && validationErrorList.Any())
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Validation Error"), validationErrorList);
                    }
                    if (transaction.customerDTO != null || (transaction.PrimaryCard != null && transaction.PrimaryCard.customerDTO != null))
                    {
                        CustomerDTO custDTO = (transaction.PrimaryCard != null && transaction.PrimaryCard.customerDTO != null ? transaction.PrimaryCard.customerDTO : (transaction.customerDTO != null ? transaction.customerDTO : null));
                        if (custDTO != null)
                        {
                            string emailAddress = string.Empty;
                            if (string.IsNullOrWhiteSpace(custDTO.UserName) == false)
                            {
                                emailAddress = custDTO.UserName;
                            }
                            if (string.IsNullOrWhiteSpace(emailAddress) &&
                                custDTO.ContactDTOList != null &&
                                custDTO.ContactDTOList.Any(x => x.ContactType == ContactType.EMAIL))
                            {
                                ContactDTO emailContactDTO = custDTO.ContactDTOList.Where(x => x.ContactType == ContactType.EMAIL).OrderByDescending(x => x.LastUpdateDate).FirstOrDefault();
                                if (emailContactDTO != null)
                                {
                                    emailAddress = emailContactDTO.Attribute1;
                                }
                            }
                            if (string.IsNullOrWhiteSpace(emailAddress))
                            {
                                log.LogMethodExit("No valid email id found to send sign waiver mail");
                                return;
                            }
                            try
                            {
                                bool reservationTransaction = transaction.IsReservationTransaction(sqlTransaction);
                                ReservationDTO reservationDTO = null;
                                string emailTemplate = string.Empty;
                                if (reservationTransaction)
                                {
                                    emailTemplate = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "RESERVATION_WAIVER_SIGNATURE_EMAIL_TEMPLATE");
                                    reservationDTO = GetReservationDTO(sqlTransaction);
                                }
                                else
                                {
                                    emailTemplate = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "TRANSACTION_WAIVER_SIGNATURE_EMAIL_TEMPLATE");
                                }

                                EmailTemplateDTO emailTemplateDTO = new EmailTemplate(executionContext).GetEmailTemplate(emailTemplate, executionContext.GetSiteId());
                                string emailContent = string.Empty;
                                if (emailTemplateDTO != null && emailTemplateDTO.EmailTemplateId > 0)
                                {
                                    emailContent = emailTemplateDTO.EmailTemplate;

                                    TransactionEmailTemplatePrint transactionEmailTemplatePrint = new TransactionEmailTemplatePrint(executionContext, utilities, emailTemplateDTO.EmailTemplateId, transaction, reservationDTO);
                                    emailContent = transactionEmailTemplatePrint.GenerateEmailTemplateContent();

                                }
                                else
                                {
                                    throw new Exception(MessageContainerList.GetMessage(executionContext, 2649));//'Email template for Send sign waiver with URL is not defined'
                                }
                                //messaging contact mapper logic to be added here
                                //"Sign Waiver Email"
                                MessagingRequestDTO messagingRequestDTO = new MessagingRequestDTO(-1, null, "Sign Waiver Email", "E", emailAddress, "", "", null, null, null, null,
                                      emailTemplateDTO.Description, emailContent, custDTO.Id, null, "", true, "", "", -1, false, "", false, null, null, null, null);
                                MessagingRequestBL messagingRequestBL = new MessagingRequestBL(executionContext, messagingRequestDTO);
                                messagingRequestBL.Save(sqlTransaction);

                                if (reservationTransaction)
                                {
                                    object bookingGuId = null;
                                    DataTable dTable = utilities.executeDataTable(@"select b.Guid
                                                                                      from bookings b 
                                                                                     where b.TrxId = @id  ",
                                                                                   sqlTransaction,
                                                                                   new SqlParameter("@id", transaction.Trx_id));
                                    if (dTable != null && dTable.Rows.Count > 0)
                                    {
                                        bookingGuId = dTable.Rows[0]["Guid"];
                                    }
                                    if (bookingGuId == null)
                                    {
                                        throw new Exception(MessageContainerList.GetMessage(executionContext, 2650));//Unable to fetch booking details for the transaction
                                    }
                                    Core.GenericUtilities.EventLog audit = new Core.GenericUtilities.EventLog(utilities.ExecutionContext);
                                    audit.logEvent(MessageContainerList.GetMessage(executionContext, "Reservation"),
                                                                              'D', executionContext.GetUserId(),
                                                                              MessageContainerList.GetMessage(executionContext, 2651, emailAddress),//Sign waiver email is sent to &1
                                                                              MessageContainerList.GetMessage(executionContext, "Reservation"),
                                                                              0, "", bookingGuId.ToString(), sqlTransaction);

                                }
                                if (transaction.BookingAttendeeList != null && transaction.BookingAttendeeList.Any())
                                {
                                    BookingAttendeeDTO editedBADTO = transaction.BookingAttendeeList.Find(baDTO => (string.IsNullOrEmpty(baDTO.Name) == false
                                                                                                              && baDTO.Name.ToLower() == custDTO.FirstName.ToLower())
                                                                                                              && ((string.IsNullOrEmpty(baDTO.PhoneNumber) == false
                                                                                                                   && baDTO.PhoneNumber == custDTO.PhoneNumber)
                                                                                                                 || (string.IsNullOrEmpty(baDTO.Email) == false
                                                                                                                      && baDTO.Email.ToLower() == custDTO.Email.ToLower())
                                                                                                                  || custDTO.Id == baDTO.CustomerId)
                                                                                            );
                                    var index = transaction.BookingAttendeeList.IndexOf(editedBADTO);
                                    if (index != -1)
                                    {
                                        transaction.BookingAttendeeList[index].SignWaiverEmailLastSentOn = utilities.getServerTime();
                                        transaction.BookingAttendeeList[index].SignWaiverEmailSentCount = transaction.BookingAttendeeList[index].SignWaiverEmailSentCount + 1;
                                        BookingAttendee bookingAttendee = new BookingAttendee(executionContext, transaction.BookingAttendeeList[index]);
                                        bookingAttendee.Save(sqlTransaction);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                throw;
                            }
                        }
                    }
                }
                else
                {
                    log.Info("Waiver Signature is not Required or not pending");
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// GenerateWaiverSigningLink 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public string GenerateWaiverSigningLink(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(utilities, sqlTransaction);
            string urlLink = string.Empty;
            if (transaction != null)
            {
                LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParam = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                searchParam.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "WAIVER_SETUP"));
                searchParam.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "BASE_URL_FOR_SIGN_WAIVER"));
                searchParam.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchParam, sqlTransaction);
                if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                {
                    urlLink = lookupValuesDTOList[0].Description;
                }
                if (string.IsNullOrEmpty(urlLink) || string.IsNullOrEmpty(urlLink.Trim()))
                {
                    log.LogMethodExit("Base url for sign waiver is not defined. Skip send sign waiver email process");
                    return string.Empty;
                }

                List<ValidationError> validationErrorList = CanSendSignWaiverEmail(sqlTransaction);
                if (validationErrorList != null && validationErrorList.Any())
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Validation Error"), validationErrorList);
                }
                try
                { 
                    string subURLLink = string.Empty;
                    ReservationDTO reservationDTO = null;
                    if (this.transaction.IsReservationTransaction(sqlTransaction))
                    {
                        subURLLink = RESERVATIONWAIVERURL;
                        reservationDTO = GetReservationDTO(sqlTransaction);
                    }
                    else
                    {
                        subURLLink = TRANSACTIONWAIVERURL;
                    }
                    lookupValuesDTOList = null;
                    lookupValuesList = new LookupValuesList(this.utilities.ExecutionContext);
                    List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParamLP = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                    searchParamLP.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, WAIVERSETUP));
                    searchParamLP.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, subURLLink));
                    searchParamLP.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, this.utilities.ExecutionContext.GetSiteId().ToString()));
                    lookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchParamLP);
                    string signWaiverSubLink = string.Empty;
                    if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                    {
                        signWaiverSubLink = lookupValuesDTOList[0].Description;
                    }
                    if (string.IsNullOrEmpty(signWaiverSubLink) || string.IsNullOrEmpty(signWaiverSubLink.Trim()))
                    {
                        string missingParamName = MessageContainerList.GetMessage(utilities.ExecutionContext, subURLLink) + " " + MessageContainerList.GetMessage(utilities.ExecutionContext, "parameter");
                        throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 2654, missingParamName));//'&1 details is missing in sign waiver URL setup'
                    }
                    TransactionEmailTemplatePrint transactionEmailTemplatePrint = new TransactionEmailTemplatePrint(executionContext, utilities, -1, transaction, reservationDTO);

                    string securityToken = this.transaction.GenerateTransactionBasedToken();
                    if (string.IsNullOrEmpty(securityToken) || string.IsNullOrEmpty(securityToken.Trim()))
                    {
                        throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 2655));// "Sorry unable to generate token for sign waiver email URL"
                    }
                    urlLink = urlLink +signWaiverSubLink + securityToken;

                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    throw;
                }
            }
            log.LogMethodExit(urlLink);
            return urlLink;
        }

        private ReservationDTO GetReservationDTO(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            ReservationDTO reservationDTO = null;
            ReservationListBL reservationListBL = new ReservationListBL(this.executionContext);
            List<KeyValuePair<ReservationDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<ReservationDTO.SearchByParameters, string>>();
            searchParam.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.TRX_ID, this.transaction.Trx_id.ToString()));
            searchParam.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.SITE_ID, this.executionContext.GetSiteId().ToString()));
            List<ReservationDTO> reservationDTOList = reservationListBL.GetReservationDTOList(searchParam, sqlTrx);
            if (reservationDTOList != null && reservationDTOList.Count > 0)
            {
                reservationDTO = reservationDTOList[0];
                //ReservationBL reservationBL = new ReservationBL(executionContext, utilities, reservationDTOList[0].BookingId, sqlTrx);
            }
            log.LogMethodExit(reservationDTO);
            return reservationDTO;
        }
    }
}
