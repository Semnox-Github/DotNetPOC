/********************************************************************************************
 * Project Name - TransactionEmailTemplatePrint
 * Description  - Class to fill email template with transaction content
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.50.00     20-Jan-2009      Guru S A       Created  
 *2.70        25-Apr-2019      Guru S A       Booking Phase 2 enhancements
 *2.70.2      04-Feb-2020      Nitin Pai      Guest App phase 2 changes
 *2.80.0      28-Apr-2020      Guru S A       Send sign waiver email changes
 *2.100.0     13-Jul-2020      Guru S A       Payment link changes
 *2.100.0     02-Nov-2020      Girish kundar  Modified: Issue fix for time zone update in the email template for attraction product
 *2.110.0     08-Dec-2020      Guru S A       Subscription changes
 *2.110.0     24-Dec-2021      Girish         Barcode image issue in email template
 *2.130.7     13-Apr-2022      Guru S A       Payment mode OTP validation changes
 *2.130.3.2   29-Sep-2022      Nitin Pai      Fix: The bar code image does not come correctly if the same email template is used for sending the email after transaction via ParafaitFunctionEvent and BuildTransactionEmailReceipt method.
 *2.150.3     21-Apr-2023      Muaaz Musthafa Added CC details to receipt such as lastFourDigitOfCC, CardType and TrxPayment date 
 *2.155.0     04-Aug-2023      Ashish S       Modified: TransactionEmailTemplatePrint constructor to fetch reservationDTO in the event reservationDTO is null during a reservation
 *2.155.0     17-Aug-2023      Muaaz Musthafa Added Gamecard details for debit card paymnet and ccp consumption discount percentage details    
 ********************************************************************************************/
using GenCode128;
using iTextSharp.text.pdf;
using QRCoder;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;
using Semnox.Parafait.Site;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    public class TransactionEmailTemplatePrint
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private Transaction transaction;
        private ReservationDTO reservationDTO;
        private TransactionPaymentsDTO transactionPaymentsDTO;
        private int emailTemplateId;
        private Utilities utilities;
        private bool isSubjectContent = false;
        private const string WAIVERSETUP = "WAIVER_SETUP";
        private const string WEBSITECONFIGURATION = "WEB_SITE_CONFIGURATION";
        private const string BASEURLFORSIGNWAIVER = "BASE_URL_FOR_SIGN_WAIVER";
        private const string BASEURLFORTRANSACTIONRECEIPT = "TRANSACTION_RECEIPT_URL";
        private const string RESERVATIONWAIVERURL = "RESERVATION_WAIVER_URL";
        private const string TRANSACTIONWAIVERURL = "TRANSACTION_WAIVER_URL";
        //private const string PAYMENTLINKSETUP = "PAYMENT_LINK_SETUP"; 
        //private const string TRANSACTIONPAYMENTLINKTEMPLATE = "TRANSACTION_PAYMENT_LINK_TEMPLATE";
        /// <summary>
        /// TransactionEmailTemplatePrint
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="utls"></param>
        /// <param name="templateId"></param>
        /// <param name="transactionId"></param>
        /// <param name="sqlTrx"></param>
        /// <param name="isSubjectContent"></param>
        public TransactionEmailTemplatePrint(ExecutionContext executionContext, Utilities utls, int templateId, int transactionId, SqlTransaction sqlTrx, bool isSubjectContent = false)
        {
            log.LogMethodEntry(executionContext, utls, templateId, transactionId, sqlTrx, isSubjectContent);
            this.executionContext = executionContext;
            this.emailTemplateId = templateId;
            this.utilities = utls;
            this.isSubjectContent = isSubjectContent;
            TransactionUtils transactionUtils = new TransactionUtils(utls);
            this.transaction = transactionUtils.CreateTransactionFromDB(transactionId, utls, false, false, sqlTrx);
            if (transaction.IsReservationTransaction(sqlTrx))
            {
                reservationDTO = GetReservationDTO(sqlTrx);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// TransactionEmailTemplatePrint
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="utls"></param>
        /// <param name="templateId"></param>
        /// <param name="trx"></param>
        /// <param name="reservationDTO"></param>
        /// <param name="isSubjectContent"></param>
        public TransactionEmailTemplatePrint(ExecutionContext executionContext, Utilities utls, int templateId, Transaction trx, ReservationDTO reservationDTO, bool isSubjectContent = false)
        {
            log.LogMethodEntry(executionContext, utls, templateId, trx, reservationDTO);
            this.executionContext = executionContext;
            this.emailTemplateId = templateId;
            this.utilities = utls;
            this.transaction = trx;
            this.isSubjectContent = isSubjectContent;
            if (reservationDTO == null && transaction.IsReservationTransaction(null))
            {
                this.reservationDTO = GetReservationDTO(null);
            }
            else
            {
                this.reservationDTO = reservationDTO;
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// GenerateEmailTemplateContent
        /// </summary>
        /// <returns></returns>
        public string GenerateEmailTemplateContent(bool attachImageResources = false)
        {
            log.LogMethodEntry();
            string emailContent = "";
            if (this.emailTemplateId != -1)
            {
                EmailTemplate emailTemplate = new EmailTemplate(this.executionContext, this.emailTemplateId);
                EmailTemplateDTO emailTemplateDTO = emailTemplate.EmailTemplateDTO;


                if (emailTemplateDTO != null && emailTemplateDTO.EmailTemplateId > 0)
                {
                    emailContent = BuildContent(emailTemplateDTO.EmailTemplate, attachImageResources);
                }
                else
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2074)); //
                }
            }
            log.LogMethodExit(emailContent);
            return emailContent;
        }
        /// <summary>
        /// GenerateTicketEmailTemplateContent
        /// </summary>
        /// <returns></returns>
        public string GenerateTicketEmailTemplateContent()
        {
            log.LogMethodEntry();
            string emailContent = "";
            if (this.emailTemplateId != -1)
            {
                EmailTemplate emailTemplate = new EmailTemplate(this.executionContext, this.emailTemplateId);
                EmailTemplateDTO emailTemplateDTO = emailTemplate.EmailTemplateDTO;


                if (emailTemplateDTO != null && emailTemplateDTO.EmailTemplateId > 0)
                {
                    emailContent = GenerateTicketEmailContent(emailTemplateDTO.EmailTemplate);
                }
                else
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2074)); //
                }
            }
            log.LogMethodExit(emailContent);
            return emailContent;
        }
        /// <summary>
        /// BuildContent
        /// </summary>
        /// <param name="emailTemplate"></param>
        /// <returns></returns>
        public string BuildContent(string emailTemplate, bool includeImageResources = false)
        {
            log.LogMethodEntry(emailTemplate, includeImageResources);
            emailTemplate = GenerateEmailContent(emailTemplate, includeImageResources);
            log.LogMethodExit("emailTemplate");
            return emailTemplate;
        }

        private string GenerateEmailContent(string emailContent, bool includeImageResources = false)
        {
            log.LogMethodEntry("emailContent", includeImageResources);
            List<KeyValuePair<string, string>> keyReplaceList = new List<KeyValuePair<string, string>>();
            if (this.transaction != null)
            {
                CustomerDTO custDTO = this.transaction.customerDTO;

                TimeZoneUtil timeZoneUtil = new TimeZoneUtil();
                int offSetDuration = timeZoneUtil.GetOffSetDuration(utilities.ExecutionContext.GetSiteId(), utilities.getServerTime().Date);
                offSetDuration = offSetDuration * -1;

                if (utilities.ExecutionContext.IsCorporate)
                {
                    transaction.TransactionDate = transaction.TransactionDate.AddSeconds(offSetDuration);
                    if(reservationDTO != null)
                    {
                        reservationDTO.FromDate = reservationDTO.FromDate.AddSeconds(offSetDuration);
                        reservationDTO.ToDate = reservationDTO.ToDate.AddSeconds(offSetDuration);
                        reservationDTO.CreationDate = reservationDTO.CreationDate.AddSeconds(offSetDuration);
                        if(reservationDTO.ExpiryTime != null)
                        {
                            reservationDTO.ExpiryTime = ((DateTime)reservationDTO.ExpiryTime).AddSeconds(offSetDuration);
                        }
                    }
                }
                log.Debug("Final transactiondate after offset calculations:" + transaction.TransactionDate);

                // Get Site details
                Site.Site site = null;
                if (this.transaction.site_id != DBNull.Value && (int)this.transaction.site_id > 0)
                {
                    site = new Site.Site((int)this.transaction.site_id);
                }
                else
                {
                    SiteList siteList = new SiteList(utilities.ExecutionContext);
                    List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>> searchParams = new List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>();
                    searchParams.Add(new KeyValuePair<SiteDTO.SearchBySiteParameters, string>(SiteDTO.SearchBySiteParameters.IS_ACTIVE, "1"));
                    List<SiteDTO> siteDTOList = siteList.GetAllSites(searchParams);
                    if (siteDTOList != null && siteDTOList.Any())
                    {
                        site = new Site.Site(utilities.ExecutionContext, siteDTOList[0]);
                    }
                }

                if (site != null && site.getSitedTO != null)
                {
                    keyReplaceList.Add(new KeyValuePair<string, string>("@siteName", string.IsNullOrEmpty(site.getSitedTO.SiteName) ? "" : site.getSitedTO.SiteName));
                    keyReplaceList.Add(new KeyValuePair<string, string>("@SiteName", string.IsNullOrEmpty(site.getSitedTO.SiteName) ? "" : site.getSitedTO.SiteName));
                    keyReplaceList.Add(new KeyValuePair<string, string>("@siteAddress", string.IsNullOrEmpty(site.getSitedTO.SiteAddress) ? "" : site.getSitedTO.SiteAddress));
                }
                keyReplaceList.Add(new KeyValuePair<string, string>("@POSName", string.IsNullOrEmpty(this.transaction.POSMachine) ? "" : this.transaction.POSMachine));

                keyReplaceList.Add(new KeyValuePair<string, string>("@FirstName", (custDTO == null || custDTO.FirstName == null) ? "" : custDTO.FirstName));
                keyReplaceList.Add(new KeyValuePair<string, string>("@customerName", (custDTO == null || custDTO.FirstName == null) ? "" : custDTO.FirstName));
                keyReplaceList.Add(new KeyValuePair<string, string>("@CustomerName", (custDTO == null || custDTO.FirstName == null) ? "" : custDTO.FirstName));
                keyReplaceList.Add(new KeyValuePair<string, string>("@taxCode", (custDTO == null || custDTO.TaxCode == null) ? "" : custDTO.TaxCode));
                keyReplaceList.Add(new KeyValuePair<string, string>("@address", ((custDTO == null || custDTO.AddressDTOList == null || custDTO.AddressDTOList.Count == 0) ? "" : custDTO.AddressDTOList[0].Line1)));

                if (custDTO == null && !string.IsNullOrEmpty(this.transaction.customerIdentifier))
                {
                    log.LogVariableState("this.transaction.customerIdentifiersList", this.transaction.customerIdentifiersList);
                    if (this.transaction.customerIdentifiersList.Count > 0)
                    {
                        keyReplaceList.Add(new KeyValuePair<string, string>("@emailAddress", (this.transaction.customerIdentifiersList[0] == null ? "" : this.transaction.customerIdentifiersList[0])));
                    }
                    else
                    {
                        keyReplaceList.Add(new KeyValuePair<string, string>("@emailAddress", ""));
                    }
                    if (this.transaction.customerIdentifiersList.Count > 1)
                    {
                        keyReplaceList.Add(new KeyValuePair<string, string>("@phoneNumber", (this.transaction.customerIdentifiersList[1] == null ? "" : this.transaction.customerIdentifiersList[1])));
                    }
                    else
                    {
                        keyReplaceList.Add(new KeyValuePair<string, string>("@phoneNumber", ""));
                    }
                }
                else
                {
                    keyReplaceList.Add(new KeyValuePair<string, string>("@emailAddress", (custDTO == null || custDTO.Email == null) ? "" : custDTO.Email));
                    keyReplaceList.Add(new KeyValuePair<string, string>("@phoneNumber", (custDTO == null || custDTO.PhoneNumber == null) ? "" : custDTO.PhoneNumber));

                }
                keyReplaceList.Add(new KeyValuePair<string, string>("@TransactionId", this.transaction.Trx_id.ToString()));
                keyReplaceList.Add(new KeyValuePair<string, string>("@transactionId", this.transaction.Trx_id.ToString()));
                keyReplaceList.Add(new KeyValuePair<string, string>("@TrxNo", this.transaction.Trx_No.ToString()));
                keyReplaceList.Add(new KeyValuePair<string, string>("@trxNo", this.transaction.Trx_No.ToString()));
                keyReplaceList.Add(new KeyValuePair<string, string>("@TransactionOTP", this.transaction.transactionOTP == null ? "" : this.transaction.transactionOTP));
                keyReplaceList.Add(new KeyValuePair<string, string>("@transactionOTP", this.transaction.transactionOTP == null ? "" : this.transaction.transactionOTP));
                keyReplaceList.Add(new KeyValuePair<string, string>("@referenceNo", this.transaction.originalSystemReference == null ? "" : this.transaction.originalSystemReference));
                keyReplaceList.Add(new KeyValuePair<string, string>("@creationDate", this.transaction.TransactionDTO != null && this.transaction.TransactionDTO.CreationDate != null && this.transaction.TransactionDTO.CreationDate != DateTime.MinValue ? this.transaction.TransactionDTO.CreationDate.ToString(utilities.ParafaitEnv.DATETIME_FORMAT) : ""));
                keyReplaceList.Add(new KeyValuePair<string, string>("@fromDate", this.transaction.TransactionDate == null ? "" : this.transaction.TransactionDate.ToString(utilities.ParafaitEnv.DATE_FORMAT)));
                keyReplaceList.Add(new KeyValuePair<string, string>("@taxAmount", this.transaction.Tax_Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)));
                keyReplaceList.Add(new KeyValuePair<string, string>("@discountAmount", this.transaction.Discount_Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)));
                keyReplaceList.Add(new KeyValuePair<string, string>("@advancePaid", this.transaction.TotalPaidAmount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)));
                keyReplaceList.Add(new KeyValuePair<string, string>("@transactionAmount", this.transaction.Transaction_Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)));
                keyReplaceList.Add(new KeyValuePair<string, string>("@transactionNetAmount", this.transaction.Net_Transaction_Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)));
                keyReplaceList.Add(new KeyValuePair<string, string>("@TrxAmount", this.transaction.Transaction_Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)));
                keyReplaceList.Add(new KeyValuePair<string, string>("@balanceDue", Math.Round(this.transaction.Net_Transaction_Amount - this.transaction.TotalPaidAmount, utilities.ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero).ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)));

                keyReplaceList.Add(new KeyValuePair<string, string>("@TrxDate", this.transaction.TransactionDate == null ? "" : this.transaction.TransactionDate.ToString(utilities.ParafaitEnv.DATETIME_FORMAT)));
                if (transaction.Trx_id > 0
                    && (transaction.PrimaryCard != null
                       && (emailContent.Contains("@PrimaryCardNumber") || emailContent.Contains("@CardBalance") || emailContent.Contains("@CreditBalance")
                             || emailContent.Contains("@BonusBalance") || emailContent.Contains("@CardBalanceTickets")))
                          || (emailContent.Contains("@StartTaxLine") || emailContent.Contains("@EndTaxLine")))
                {
                    transaction.TransactionInfo.createTransactionInfo(transaction.Trx_id);
                }
                keyReplaceList.Add(new KeyValuePair<string, string>("@PrimaryCardNumber", transaction.TransactionInfo.PrimaryPaymentCardNumber));
                keyReplaceList.Add(new KeyValuePair<string, string>("@CardBalance", transaction.TransactionInfo.PrimaryCardBalance.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT)));
                keyReplaceList.Add(new KeyValuePair<string, string>("@CreditBalance", transaction.TransactionInfo.PrimaryCardCreditBalance.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT)));
                keyReplaceList.Add(new KeyValuePair<string, string>("@BonusBalance", transaction.TransactionInfo.PrimaryCardBonusBalance.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT)));
                keyReplaceList.Add(new KeyValuePair<string, string>("@CardBalanceTickets", transaction.TransactionInfo.Tickets.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT)));
                try
                {
                    int paymentModeOTPThreshholdTime = ParafaitDefaultContainerList.GetParafaitDefault<int>(utilities.ExecutionContext, "PAYMENT_MODE_OTP_THRESHOLD_TIME", 300) / 60;
                    keyReplaceList.Add(new KeyValuePair<string, string>("@PaymentModeOTPExpiry", paymentModeOTPThreshholdTime.ToString("N0")));
                }
                catch (Exception ex)
                {
                    log.Error("PAYMENT_MODE_OTP_THRESHOLD_TIME", ex);
                }

                if (!transaction.TransactionInfo.IsVirtualQueueEnabled)
                {
                    transaction.TransactionInfo.IsTransactionVirtualQueueEnabled(transaction.Trx_id, -1);
                }

                if (transaction.TransactionInfo.IsVirtualQueueEnabled)
                {
                    LookupsContainerDTO lookupsContainerDTO = LookupsContainerList.GetLookupsContainerDTO(utilities.ExecutionContext.GetSiteId(), "ADDITIONAL_PRINT_FIELDS", utilities.ExecutionContext);
                    if (lookupsContainerDTO.LookupValuesContainerDTOList != null && lookupsContainerDTO.LookupValuesContainerDTOList.Any())
                    {
                        LookupValuesContainerDTO lookupValuesContainerDTO = lookupsContainerDTO.LookupValuesContainerDTOList.FirstOrDefault(x => x.LookupValue.Equals("@VirtualQueueText"));
                        if (lookupValuesContainerDTO != null && !string.IsNullOrWhiteSpace(lookupValuesContainerDTO.Description))
                        {
                            keyReplaceList.Add(new KeyValuePair<string, string>("@VirtualQueueText", lookupValuesContainerDTO.Description));
                        }
                        lookupValuesContainerDTO = lookupsContainerDTO.LookupValuesContainerDTOList.FirstOrDefault(x => x.LookupValue.Equals("@VirtualQueueURL"));
                        if (lookupValuesContainerDTO != null && !string.IsNullOrWhiteSpace(lookupValuesContainerDTO.Description))
                        {
                            keyReplaceList.Add(new KeyValuePair<string, string>("@VirtualQueueURL", lookupValuesContainerDTO.Description + transaction.TrxGuid.ToUpper()));
                        }
                        if (emailContent.Contains("@QRCodeVirtualQueueURL")
                            && lookupValuesContainerDTO != null
                            && !string.IsNullOrWhiteSpace(lookupValuesContainerDTO.Description)
                           )
                        {
                            QRCodeGenerator qrGenerator = new QRCodeGenerator();
                            QRCodeData qrCodeData = qrGenerator.CreateQrCode(lookupValuesContainerDTO.Description + transaction.TrxGuid.ToUpper(), QRCodeGenerator.ECCLevel.Q);
                            QRCode qrCode = new QRCode(qrCodeData);
                            if (qrCode != null)
                            {
                                Image image = qrCode.GetGraphic(5);
                                if (image != null)
                                {
                                    using (var stream = new MemoryStream())
                                    {
                                        image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                                        String qrCodeB64 = System.Convert.ToBase64String(stream.ToArray());
                                        keyReplaceList.Add(new KeyValuePair<string, string>("@QRCodeVirtualQueueURL", qrCodeB64));
                                    }
                                }
                            }
                        }
                    }
                }

                if (transaction.TransactionPaymentsDTOList != null && transaction.TransactionPaymentsDTOList.Any())
                {
                    transactionPaymentsDTO = transaction.TransactionPaymentsDTOList.OrderByDescending(trxPayment => trxPayment.PaymentId).First();
                    log.Debug($"For TrxId: {transaction.Trx_id}, Transaction payment details: {transactionPaymentsDTO.ToString()}");
                }
                else
                {
                    log.Error("No transaction payment present for TrxId: " + transaction.Trx_id);
                    transactionPaymentsDTO = new TransactionPaymentsDTO();
                }

                // When Purchased using Parafait Debit card
                if (transactionPaymentsDTO.CardId > 0)
                {
                    int cardId = transactionPaymentsDTO.CardId;
                    string gameCardlabel = string.Empty;

                    //Get Gamecard details used for purchase
                    log.Debug("CardId: " + transactionPaymentsDTO.CardId + " UserId: " + executionContext.GetUserId());
                    Card currentCard = new Card(transactionPaymentsDTO.CardId, executionContext.GetUserId(), utilities);

                    if (!string.IsNullOrWhiteSpace(utilities.MessageUtils.getMessage(10017)))
                    {
                        gameCardlabel = utilities.MessageUtils.getMessage(10017);
                        log.Debug("gameCardlabel value: " + gameCardlabel);
                    }
                    else
                    {
                        log.Error("Message no- 10017 doesn't exist. Showing default gamecard label");
                        gameCardlabel = "GAME CARD";
                    }

                    keyReplaceList.Add(new KeyValuePair<string, string>("@lastFourDigitOfCC", currentCard.CardNumber != null ? currentCard.CardNumber : ""));
                    keyReplaceList.Add(new KeyValuePair<string, string>("@creditCardType", gameCardlabel));
                    keyReplaceList.Add(new KeyValuePair<string, string>("@trxPaymentDate", 
                        (transactionPaymentsDTO.PaymentDate != null && transactionPaymentsDTO.PaymentDate != DateTime.MinValue)
                        ? Convert.ToDateTime(transactionPaymentsDTO.PaymentDate).AddSeconds(offSetDuration).ToString(utilities.ParafaitEnv.DATETIME_FORMAT)
                        : ServerDateTime.Now.AddSeconds(offSetDuration).ToString(utilities.ParafaitEnv.DATETIME_FORMAT)));
                }
                else// when purchased using Credit card payment
                {
                    keyReplaceList.Add(new KeyValuePair<string, string>("@lastFourDigitOfCC", transactionPaymentsDTO.CreditCardNumber != null ? transactionPaymentsDTO.CreditCardNumber : ""));
                    keyReplaceList.Add(new KeyValuePair<string, string>("@creditCardType", transactionPaymentsDTO.CreditCardName != null ? transactionPaymentsDTO.CreditCardName : ""));
                    keyReplaceList.Add(new KeyValuePair<string, string>("@trxPaymentDate",
                        (transactionPaymentsDTO.PaymentDate != null && transactionPaymentsDTO.PaymentDate != DateTime.MinValue) 
                        ? Convert.ToDateTime(transactionPaymentsDTO.PaymentDate).AddSeconds(offSetDuration).ToString(utilities.ParafaitEnv.DATETIME_FORMAT) 
                        : ServerDateTime.Now.AddSeconds(offSetDuration).ToString(utilities.ParafaitEnv.DATETIME_FORMAT)));
                }

                //ReservationListBL reservationListBL = new ReservationListBL(this.executionContext);
                //List<KeyValuePair<ReservationDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<ReservationDTO.SearchByParameters, string>>();
                //searchParam.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.TRX_ID, this.transaction.Trx_id.ToString()));
                //searchParam.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.SITE_ID, this.executionContext.GetSiteId().ToString()));
                //List<ReservationDTO> reservationDTOList = reservationListBL.GetReservationDTOList(searchParam, sqlTrx);
                if (reservationDTO != null)//reservationDTOList != null && reservationDTOList.Count > 0)
                {
                    //ReservationBL reservationBL = new ReservationBL(executionContext, utilities, reservationDTOList[0].BookingId, sqlTrx);

                    keyReplaceList.Add(new KeyValuePair<string, string>("@fromTime", reservationDTO.FromDate.ToString(utilities.ParafaitEnv.DATETIME_FORMAT)));
                    keyReplaceList.Add(new KeyValuePair<string, string>("@toTime", (reservationDTO.ExpiryTime != null ? ((DateTime)reservationDTO.ExpiryTime).ToString(utilities.ParafaitEnv.DATETIME_FORMAT) : reservationDTO.ToDate.ToString(utilities.ParafaitEnv.DATETIME_FORMAT))));
                    keyReplaceList.Add(new KeyValuePair<string, string>("@creationDate", reservationDTO.CreationDate == null ? "" : reservationDTO.CreationDate.ToString(utilities.ParafaitEnv.DATE_FORMAT)));

                    keyReplaceList.Add(new KeyValuePair<string, string>("@reservationCode", reservationDTO.ReservationCode));
                    keyReplaceList.Add(new KeyValuePair<string, string>("@bookingName", reservationDTO.BookingName));
                    string facilityNames = reservationDTO.FacilityName;
                    keyReplaceList.Add(new KeyValuePair<string, string>("@partyRoom", facilityNames));
                    keyReplaceList.Add(new KeyValuePair<string, string>("@additionalItems", ""));

                    keyReplaceList.Add(new KeyValuePair<string, string>("@bookingRemarks", reservationDTO.Remarks));

                    int guestCount = 0;
                    int participantGuestCount = 0;
                    string bookingProductName = "";
                    Transaction.TransactionLine bookingProductTrxLine = transaction.GetBookingProductTransactionLine();
                    if (bookingProductTrxLine != null)
                    {
                        bookingProductName = bookingProductTrxLine.ProductName;
                    }

                    participantGuestCount = transaction.GetReservationTransactionGuestQuantity();
                    guestCount = participantGuestCount + reservationDTO.ExtraGuests;
                    keyReplaceList.Add(new KeyValuePair<string, string>("@bookingProduct", bookingProductName));
                    keyReplaceList.Add(new KeyValuePair<string, string>("@guestCount", guestCount.ToString(utilities.ParafaitEnv.NUMBER_FORMAT)));
                    keyReplaceList.Add(new KeyValuePair<string, string>("@participantGuestCount", participantGuestCount.ToString(utilities.ParafaitEnv.NUMBER_FORMAT)));
                    keyReplaceList.Add(new KeyValuePair<string, string>("@nonParticipantGuestCount", reservationDTO.ExtraGuests.ToString(utilities.ParafaitEnv.NUMBER_FORMAT)));

                }

                if (isSubjectContent == false)
                {
                    int TicketCount = 0;
                    TransactionCore transactionCore = new TransactionCore();
                    PurchasesListParams purchasesListParams = new PurchasesListParams();
                    purchasesListParams.TranscationId = this.transaction.Trx_id;
                    purchasesListParams.ShowTransactionLines = true;
                    purchasesListParams.LoginId = executionContext.GetUserId();
                    purchasesListParams.CustomerId = (custDTO == null ? -1 : custDTO.Id);
                    List<TransactionDetails> transactionDetails = transactionCore.GetPurchaseTransactions(purchasesListParams);
                    if (transactionDetails != null && transactionDetails.Count > 0)
                    {
                        List<TransactionDetails> transactionComboDetails = transactionCore.GetComboTransactionDetailsList(transactionDetails);
                        string productGroupCount = " ";
                        if (transactionComboDetails != null && transactionComboDetails.Count > 0)
                        {
                            if (transactionComboDetails[0].Products != null && transactionComboDetails[0].Products.Count > 0)
                            {
                                foreach (LinkedPurchasedProducts linkedPurchasedProducts in transactionComboDetails[0].Products)
                                {
                                    if (linkedPurchasedProducts.ProductType.ToLower() != "combo")
                                    {
                                        TicketCount += linkedPurchasedProducts.Quantity;
                                    }
                                }
                                productGroupCount = transactionCore.GetProductGroupCount(transactionComboDetails[0].Products);
                            }
                        }

                        keyReplaceList.Add(new KeyValuePair<string, string>("@ticketCount", TicketCount.ToString()));
                        keyReplaceList.Add(new KeyValuePair<string, string>("@productGroupCount", productGroupCount));

                        keyReplaceList.Add(new KeyValuePair<string, string>("@merchantRefNo", ""));

                        string tourOperator = "";
                        string operatorMobileNo = "";
                        if (transactionDetails[0].TransactionUserDTO != null && transactionDetails[0].TransactionUserDTO.IsAgent == "Y")
                        {
                            tourOperator = "<tr><td> Tour Operator :  " + transactionDetails[0].TransactionUserDTO.Username + "</td></tr>";
                            operatorMobileNo = "<tr><td> Mobile No :   " + transactionDetails[0].TransactionUserDTO.MobileNo + "</td></tr>";
                        }

                        keyReplaceList.Add(new KeyValuePair<string, string>("@tourOperator", tourOperator));
                        keyReplaceList.Add(new KeyValuePair<string, string>("@mobileNo", operatorMobileNo));



                        //inEmail.KeyReplaceList = listPurchaseDetails;

                        List<KeyValuePair<string, string>> imageList = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("header", "~/images/header.jpg"),
                        new KeyValuePair<string, string>("footer", "~/images/footer.gif")
                    };

                        string firstString = "@StartProductLine";
                        int Pos1 = emailContent.IndexOf(firstString) + firstString.Length;
                        int Pos2 = emailContent.IndexOf("@EndProductLine");
                        if (Pos1 > 0 && Pos2 > 0)
                        {
                            string originalProductLine = emailContent.Substring(Pos1, Pos2 - Pos1);

                            if (transactionComboDetails != null && transactionComboDetails[0] != null && transactionComboDetails[0].Products != null)
                            {
                                string productLines = GetTransactionTableString(transactionComboDetails[0], originalProductLine);
                                emailContent = emailContent.Replace("@StartProductLine" + originalProductLine + "@EndProductLine", productLines);
                            }
                        }

                        String numberFormat = transaction.Utilities.ParafaitEnv.NUMBER_FORMAT;

                        string taxFirstString = "@StartTaxLine";
                        int taxPos1 = emailContent.IndexOf(taxFirstString) + taxFirstString.Length;
                        int taxPos2 = emailContent.IndexOf("@EndTaxLine");
                        if (taxPos1 > 0 && taxPos2 > 0)
                        {
                            string originalTaxLine = emailContent.Substring(taxPos1, taxPos2 - taxPos1);

                            if (transaction.TransactionInfo != null && transaction.TransactionInfo.TrxTax != null && transaction.TransactionInfo.TrxTax.Any())
                            {
                                string taxLines = GetTransactionTaxTableString(transaction.TransactionInfo.TrxTax, originalTaxLine);
                                emailContent = emailContent.Replace("@StartTaxLine" + originalTaxLine + "@EndTaxLine", taxLines);
                            }
                            else
                            {
                                String tempLine = originalTaxLine;
                                tempLine = tempLine.Replace("@taxName", "");
                                tempLine = tempLine.Replace("@taxPercentage", "");
                                tempLine = tempLine.Replace("%", "");
                                tempLine = tempLine.Replace("@taxAmount", "");
                                emailContent = emailContent.Replace("@StartTaxLine" + originalTaxLine + "@EndTaxLine", tempLine);
                            }
                        }

                        string chargeFirstString = "@StartTaxableChargeLine";
                        int chargePos1 = emailContent.IndexOf(chargeFirstString) + chargeFirstString.Length;
                        int chargePos2 = emailContent.IndexOf("@EndTaxableChargeLine");
                        if (chargePos1 > 0 && chargePos2 > 0)
                        {
                            string originalChargeLine = emailContent.Substring(chargePos1, chargePos2 - chargePos1);

                            if (transaction.TransactionInfo != null && transaction.TransactionInfo.TrxTaxableCharges != null && transaction.TransactionInfo.TrxTaxableCharges.Any())
                            {
                                string chargeLines = GetTransactionChargesString(transaction.TransactionInfo.TrxTaxableCharges, originalChargeLine);
                                emailContent = emailContent.Replace("@StartTaxableChargeLine" + originalChargeLine + "@EndTaxableChargeLine", chargeLines);
                            }
                            else
                            {
                                String tempLine = originalChargeLine;
                                tempLine = tempLine.Replace("@ChargeName", "");
                                tempLine = tempLine.Replace("@ChargeAmount", "");
                                emailContent = emailContent.Replace("@StartTaxableChargeLine" + originalChargeLine + "@EndTaxableChargeLine", tempLine);
                            }
                        }
                        string nonTaxablechargeFirstString = "@StartNonTaxableChargeLine";
                        int nonTaxableChargePos1 = emailContent.IndexOf(nonTaxablechargeFirstString) + nonTaxablechargeFirstString.Length;
                        int nonTaxableChargePos2 = emailContent.IndexOf("@EndNonTaxableChargeLine");
                        if (nonTaxableChargePos1 > 0 && nonTaxableChargePos2 > 0)
                        {
                            string originalNonTaxableChargeLine = emailContent.Substring(nonTaxableChargePos1, nonTaxableChargePos2 - nonTaxableChargePos1);

                            if (transaction.TransactionInfo != null && transaction.TransactionInfo.TrxNonTaxableCharges != null && transaction.TransactionInfo.TrxNonTaxableCharges.Any())
                            {
                                string nonTaxableChargeLines = GetTransactionChargesString(transaction.TransactionInfo.TrxNonTaxableCharges, originalNonTaxableChargeLine);
                                emailContent = emailContent.Replace("@StartNonTaxableChargeLine" + originalNonTaxableChargeLine + "@EndNonTaxableChargeLine", nonTaxableChargeLines);
                            }
                            else
                            {
                                String tempLine = originalNonTaxableChargeLine;
                                tempLine = tempLine.Replace("@ChargeName", "");
                                tempLine = tempLine.Replace("@ChargeAmount", "");
                                emailContent = emailContent.Replace("@StartNonTaxableChargeLine" + originalNonTaxableChargeLine + "@EndNonTaxableChargeLine", tempLine);
                            }
                        }

                        if(includeImageResources == true)
                        {
                            var transactionOTPTag = string.Format("<TransactionOTP>{0}</TransactionOTP>", transaction.transactionOTP);
                            keyReplaceList.Add(new KeyValuePair<string, string>("@BarCodeTrxOTP", transactionOTPTag));
                            keyReplaceList.Add(new KeyValuePair<string, string>("@BarCodeImageTrxOTP", ""));
                        }
                       
                        if(includeImageResources == false)
                        {
                            Image image = Code128Rendering.MakeBarcodeImage(this.transaction.transactionOTP == null ? "" : this.transaction.transactionOTP, 1, true);
                            if (image != null)
                            {
                                using (var stream = new MemoryStream())
                                {
                                    image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                                    String accountBarCode = System.Convert.ToBase64String(stream.ToArray());
                                    String barCodeImage = string.Format("<img style='width:175px; height:100px' src = 'data:image/jpeg;base64,{0}'/>", accountBarCode);
                                    keyReplaceList.Add(new KeyValuePair<string, string>("@BarCodeImageTrxOTP", barCodeImage));
                                    keyReplaceList.Add(new KeyValuePair<string, string>("@BarCodeTrxOTP", ""));
                                }
                            }
                        }

                        if (includeImageResources == true)
                        {
                            var transactionOTPTag = string.Format("<TransactionOTPQR>{0}</TransactionOTPQR>", transaction.transactionOTP);
                            keyReplaceList.Add(new KeyValuePair<string, string>("@QRCodeTrxOTP", transactionOTPTag));
                            keyReplaceList.Add(new KeyValuePair<string, string>("@QRCodeImageTrxOTP", ""));
                        }

                        if (includeImageResources == false)
                        {
                            QRCodeGenerator qrGenerator = new QRCodeGenerator();
                            QRCodeData qrCodeData = qrGenerator.CreateQrCode(transaction.transactionOTP.ToString(), QRCodeGenerator.ECCLevel.Q);
                            QRCode qrCode = new QRCode(qrCodeData);

                            if (qrCode != null)
                            {
                                Image image = qrCode.GetGraphic(5);
                                if (image != null)
                                {
                                    using (var stream = new MemoryStream())
                                    {
                                        image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                                        String qrCodeB64 = System.Convert.ToBase64String(stream.ToArray());
                                        String qrCodeImage = string.Format("<img style='width:100px; height:100px' src = 'data:image/jpeg;base64,{0}'/>", qrCodeB64);
                                        keyReplaceList.Add(new KeyValuePair<string, string>("@QRCodeImageTrxOTP", qrCodeImage));
                                        keyReplaceList.Add(new KeyValuePair<string, string>("@QRCodeTrxOTP", ""));
                                    }
                                }
                            }
                        }
                    }

                    //Send Waiver email content                
                    if (emailContent.Contains("@SignWaiverLinkQRCode"))
                    {
                        string waiverLink = GenerateSignWaiverLink();
                        String qrCodeB64 = string.Empty;
                        qrCodeB64 = GenerateQRCodeB64ForURL(waiverLink);
                        if (string.IsNullOrWhiteSpace(qrCodeB64))
                        {
                            throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 2764));// "Sorry unable to generate QR code"
                        }
                        keyReplaceList.Add(new KeyValuePair<string, string>("@SignWaiverLinkQRCode", qrCodeB64));
                    }

                    //Send Waiver email content                
                    if (emailContent.Contains("@SignWaiverLink"))
                    {
                        string waiverLink = GenerateSignWaiverLink();
                        keyReplaceList.Add(new KeyValuePair<string, string>("@SignWaiverLink", waiverLink));
                    }


                    if (emailContent.Contains("@TransactionReceiptLink"))
                    {
                        string baseURLTransactionReceipt = string.Empty;
                        string securityToken = string.Empty;
                        string transactionReceiptLink = string.Empty;
                        LookupValuesList lookupValuesList = new LookupValuesList(this.utilities.ExecutionContext);
                        List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParamLP = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                        searchParamLP.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, WEBSITECONFIGURATION));
                        searchParamLP.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, BASEURLFORTRANSACTIONRECEIPT));
                        searchParamLP.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, this.utilities.ExecutionContext.GetSiteId().ToString()));
                        List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchParamLP);
                        baseURLTransactionReceipt = string.Empty;
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                        {
                            baseURLTransactionReceipt = lookupValuesDTOList[0].Description;
                            transactionReceiptLink = baseURLTransactionReceipt;
                        }
                        if (string.IsNullOrEmpty(baseURLTransactionReceipt) || string.IsNullOrEmpty(baseURLTransactionReceipt.Trim()))
                        {
                            //Base URL for Transaction payment receipt is not defined in the setup
                            throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 2653));
                        }

                        securityToken = transaction.GenerateTransactionBasedToken();
                        if (string.IsNullOrEmpty(securityToken) || string.IsNullOrEmpty(securityToken.Trim()))
                        {
                            throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 2655));
                        }
                        if (baseURLTransactionReceipt.Contains("@trxGuid"))
                        {
                            transactionReceiptLink = transactionReceiptLink.Replace("@trxGuid", (String.IsNullOrEmpty(transaction.TrxGuid) ? "" : transaction.TrxGuid));

                        }
                        else if (baseURLTransactionReceipt.Contains("@transactionToken"))
                        {
                            transactionReceiptLink = transactionReceiptLink.Replace("@transactionToken", securityToken);

                        }
                        if (baseURLTransactionReceipt.Contains("@message"))
                        {
                            transactionReceiptLink = transactionReceiptLink.Replace("@message", "");
                            
                        }
                        keyReplaceList.Add(new KeyValuePair<string, string>("@TransactionReceiptLink", transactionReceiptLink));
                    }

                    if (emailContent.Contains("@PaymentLinkQRCode"))
                    {
                        TransactionPaymentLink transactionPaymentLink = new TransactionPaymentLink(utilities.ExecutionContext, utilities, transaction);
                        string paymentPageURL = transactionPaymentLink.GeneratePaymentLink();
                        if (string.IsNullOrWhiteSpace(paymentPageURL))
                        {
                            throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 2746));// "Sorry unable to generate token for payment link URL"
                        }
                        String qrCodeB64 = string.Empty;
                        qrCodeB64 = GenerateQRCodeB64ForURL(paymentPageURL);
                        if (string.IsNullOrWhiteSpace(qrCodeB64))
                        {
                            throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 2764));// "Sorry unable to generate QR code"
                        }
                        keyReplaceList.Add(new KeyValuePair<string, string>("@PaymentLinkQRCode", qrCodeB64));
                    }
                    //Send payment link content
                    if (emailContent.Contains("@PaymentLink"))
                    {
                        TransactionPaymentLink transactionPaymentLink = new TransactionPaymentLink(utilities.ExecutionContext, utilities, transaction);
                        string paymentPageURL = transactionPaymentLink.GeneratePaymentLink();
                        if (string.IsNullOrWhiteSpace(paymentPageURL))
                        {
                            throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 2746));// "Sorry unable to generate token for payment link URL"
                        }
                        keyReplaceList.Add(new KeyValuePair<string, string>("@PaymentLink", paymentPageURL));
                    }

                    //OrderDispensingExtSystemRef content
                    if (emailContent.Contains("@OrderDispensingExtSystemRef"))
                    {
                        string orderDispensingExtSystemRef = POSPrint.GetOrderDispensingExternalSystemRef(transaction);
                        keyReplaceList.Add(new KeyValuePair<string, string>("@OrderDispensingExtSystemRef", orderDispensingExtSystemRef));
                    }
                    if (emailContent.Contains("@BarCodeOrderDispensingExtSystemRef"))
                    {
                        string orderDispensingExtSystemRef = POSPrint.GetOrderDispensingExternalSystemRef(transaction);
                        String barCodeImage = string.Empty;
                        if (string.IsNullOrWhiteSpace(orderDispensingExtSystemRef) == false)
                        {
                            Image image = Code128Rendering.MakeBarcodeImage(string.IsNullOrWhiteSpace(orderDispensingExtSystemRef)
                                                                            ? "" : orderDispensingExtSystemRef, 1, true);
                            if (image != null)
                            {
                                using (var stream = new MemoryStream())
                                {
                                    image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                                    String accountBarCode = System.Convert.ToBase64String(stream.ToArray());
                                    barCodeImage = string.Format("<img style='width:175px; height:100px' src = 'data:image/jpeg;base64,{0}'/>", accountBarCode);
                                }
                            }
                        }
                        keyReplaceList.Add(new KeyValuePair<string, string>("@BarCodeOrderDispensingExtSystemRef", barCodeImage));
                    }
                    if (emailContent.Contains("@QRCodeOrderDispensingExtSystemRef"))
                    {
                        string orderDispensingExtSystemRef = POSPrint.GetOrderDispensingExternalSystemRef(transaction);
                        String qrCodeB64 = string.Empty;
                        if (string.IsNullOrWhiteSpace(orderDispensingExtSystemRef) == false)
                        {
                            qrCodeB64 = GenerateQRCodeB64ForURL(orderDispensingExtSystemRef);
                        }
                        keyReplaceList.Add(new KeyValuePair<string, string>("@QRCodeOrderDispensingExtSystemRef", qrCodeB64));
                    }
                    //DeliveryChannelCustomerRef content
                    if (emailContent.Contains("@DeliveryChannelCustomerRef"))
                    {
                        string deliveryChannelCustomerReferenceNo = POSPrint.GetOrderDispensingDeliveryChannelCustomerReferenceNo(transaction);
                        keyReplaceList.Add(new KeyValuePair<string, string>("@DeliveryChannelCustomerRef", deliveryChannelCustomerReferenceNo));
                    }
                    if (emailContent.Contains("@BarCodeDeliveryChannelCustomerRef"))
                    {
                        string deliveryChannelCustomerReferenceNo = POSPrint.GetOrderDispensingDeliveryChannelCustomerReferenceNo(transaction);
                        String barCodeImage = string.Empty;
                        if (string.IsNullOrWhiteSpace(deliveryChannelCustomerReferenceNo) == false)
                        {
                            Image image = Code128Rendering.MakeBarcodeImage(string.IsNullOrWhiteSpace(deliveryChannelCustomerReferenceNo)
                                                                            ? "" : deliveryChannelCustomerReferenceNo, 1, true);
                            if (image != null)
                            {
                                using (var stream = new MemoryStream())
                                {
                                    image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                                    String accountBarCode = System.Convert.ToBase64String(stream.ToArray());
                                    barCodeImage = string.Format("<img style='width:175px; height:100px' src = 'data:image/jpeg;base64,{0}'/>", accountBarCode);
                                }
                            }
                        }
                        keyReplaceList.Add(new KeyValuePair<string, string>("@BarCodeDeliveryChannelCustomerRef", barCodeImage));
                    }
                    if (emailContent.Contains("@QRCodeDeliveryChannelCustomerRef"))
                    {
                        string deliveryChannelCustomerReferenceNo = POSPrint.GetOrderDispensingDeliveryChannelCustomerReferenceNo(transaction);
                        String qrCodeB64 = string.Empty;
                        if (string.IsNullOrWhiteSpace(deliveryChannelCustomerReferenceNo) == false)
                        {
                            qrCodeB64 = GenerateQRCodeB64ForURL(deliveryChannelCustomerReferenceNo);
                        }
                        keyReplaceList.Add(new KeyValuePair<string, string>("@QRCodeDeliveryChannelCustomerRef", qrCodeB64));
                    }
                }
                emailContent = UpdateTemplate(keyReplaceList, emailContent);
                emailContent = emailContent.Replace("\n", "");
            }
            return emailContent;
        }

        private string GenerateTicketEmailContent(string emailContent)
        {
            log.LogMethodEntry("emailContent");
            List<KeyValuePair<string, string>> keyReplaceList = new List<KeyValuePair<string, string>>();
            if (this.transaction != null)
            {
                CustomerDTO custDTO = this.transaction.customerDTO;

                TimeZoneUtil timeZoneUtil = new TimeZoneUtil();
                int offSetDuration = timeZoneUtil.GetOffSetDuration(utilities.ExecutionContext.GetSiteId(), utilities.getServerTime().Date);
                offSetDuration = offSetDuration * -1;
                transaction.TransactionDate = transaction.TransactionDate.AddSeconds(offSetDuration);
                log.Debug("Applying offset to ScheduleDate:" + transaction.TransactionDate);

                // Get Site details
                Site.Site site = null;
                if (this.transaction.site_id != DBNull.Value && (int)this.transaction.site_id > 0)
                {
                    site = new Site.Site((int)this.transaction.site_id);
                }
                else
                {
                    SiteList siteList = new SiteList(utilities.ExecutionContext);
                    List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>> searchParams = new List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>();
                    searchParams.Add(new KeyValuePair<SiteDTO.SearchBySiteParameters, string>(SiteDTO.SearchBySiteParameters.IS_ACTIVE, "1"));
                    List<SiteDTO> siteDTOList = siteList.GetAllSites(searchParams);
                    if (siteDTOList != null && siteDTOList.Any())
                    {
                        site = new Site.Site(utilities.ExecutionContext, siteDTOList[0]);
                    }
                }

                if (site != null && site.getSitedTO != null)
                {
                    keyReplaceList.Add(new KeyValuePair<string, string>("@siteName", string.IsNullOrEmpty(site.getSitedTO.SiteName) ? "" : site.getSitedTO.SiteName));
                    keyReplaceList.Add(new KeyValuePair<string, string>("@siteAddress", string.IsNullOrEmpty(site.getSitedTO.SiteAddress) ? "" : site.getSitedTO.SiteAddress));
                }

                keyReplaceList.Add(new KeyValuePair<string, string>("@customerName", (custDTO == null || custDTO.FirstName == null) ? "" : custDTO.FirstName));
                keyReplaceList.Add(new KeyValuePair<string, string>("@taxCode", (custDTO == null || custDTO.TaxCode == null) ? "" : custDTO.TaxCode));
                keyReplaceList.Add(new KeyValuePair<string, string>("@address", ((custDTO == null || custDTO.AddressDTOList == null || custDTO.AddressDTOList.Count == 0) ? "" : custDTO.AddressDTOList[0].Line1)));
                keyReplaceList.Add(new KeyValuePair<string, string>("@emailAddress", (custDTO == null || custDTO.Email == null) ? "" : custDTO.Email));
                keyReplaceList.Add(new KeyValuePair<string, string>("@phoneNumber", (custDTO == null || custDTO.PhoneNumber == null) ? "" : custDTO.PhoneNumber));

                keyReplaceList.Add(new KeyValuePair<string, string>("@transactionId", this.transaction.Trx_id.ToString()));
                keyReplaceList.Add(new KeyValuePair<string, string>("@transactionOTP", this.transaction.transactionOTP == null ? "" : this.transaction.transactionOTP));
                keyReplaceList.Add(new KeyValuePair<string, string>("@referenceNo", this.transaction.originalSystemReference == null ? "" : this.transaction.originalSystemReference));
                keyReplaceList.Add(new KeyValuePair<string, string>("@creationDate", this.transaction.TransactionDTO != null && this.transaction.TransactionDTO.CreationDate != null && this.transaction.TransactionDTO.CreationDate != DateTime.MinValue ? this.transaction.TransactionDTO.CreationDate.ToString(utilities.ParafaitEnv.DATETIME_FORMAT) : ""));
                keyReplaceList.Add(new KeyValuePair<string, string>("@fromDate", this.transaction.TransactionDate == null ? "" : this.transaction.TransactionDate.ToString(utilities.ParafaitEnv.DATE_FORMAT)));
                keyReplaceList.Add(new KeyValuePair<string, string>("@taxAmount", this.transaction.Tax_Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)));
                keyReplaceList.Add(new KeyValuePair<string, string>("@discountAmount", this.transaction.Discount_Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)));
                keyReplaceList.Add(new KeyValuePair<string, string>("@advancePaid", this.transaction.TotalPaidAmount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)));
                keyReplaceList.Add(new KeyValuePair<string, string>("@transactionAmount", this.transaction.Transaction_Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)));
                keyReplaceList.Add(new KeyValuePair<string, string>("@balanceDue", Math.Round(this.transaction.Net_Transaction_Amount - this.transaction.TotalPaidAmount, utilities.ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero).ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)));

                keyReplaceList.Add(new KeyValuePair<string, string>("@TrxDate", this.transaction.TransactionDate == null ? "" : this.transaction.TransactionDate.ToString(utilities.ParafaitEnv.DATETIME_FORMAT)));
                if (transaction.Trx_id > 0
                    && (transaction.PrimaryCard != null
                       && (emailContent.Contains("@PrimaryCardNumber") || emailContent.Contains("@CardBalance") || emailContent.Contains("@CreditBalance")
                             || emailContent.Contains("@BonusBalance") || emailContent.Contains("@CardBalanceTickets")))
                          || (emailContent.Contains("@StartTaxLine") || emailContent.Contains("@EndTaxLine")))
                {
                    transaction.TransactionInfo.createTransactionInfo(transaction.Trx_id);
                }
                keyReplaceList.Add(new KeyValuePair<string, string>("@PrimaryCardNumber", transaction.TransactionInfo.PrimaryPaymentCardNumber));
                keyReplaceList.Add(new KeyValuePair<string, string>("@CardBalance", transaction.TransactionInfo.PrimaryCardBalance.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT)));
                keyReplaceList.Add(new KeyValuePair<string, string>("@CreditBalance", transaction.TransactionInfo.PrimaryCardCreditBalance.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT)));
                keyReplaceList.Add(new KeyValuePair<string, string>("@BonusBalance", transaction.TransactionInfo.PrimaryCardBonusBalance.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT)));
                keyReplaceList.Add(new KeyValuePair<string, string>("@CardBalanceTickets", transaction.TransactionInfo.Tickets.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT)));
                try
                {
                    int paymentModeOTPThreshholdTime = ParafaitDefaultContainerList.GetParafaitDefault<int>(utilities.ExecutionContext, "PAYMENT_MODE_OTP_THRESHOLD_TIME", 300) / 60;
                    keyReplaceList.Add(new KeyValuePair<string, string>("@PaymentModeOTPExpiry", paymentModeOTPThreshholdTime.ToString("N0")));
                }
                catch (Exception ex)
                {
                    log.Error("PAYMENT_MODE_OTP_THRESHOLD_TIME", ex);
                }

                ReservationListBL reservationListBL = new ReservationListBL(this.executionContext);
                List<KeyValuePair<ReservationDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<ReservationDTO.SearchByParameters, string>>();
                searchParam.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.TRX_ID, this.transaction.Trx_id.ToString()));
                searchParam.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.SITE_ID, this.executionContext.GetSiteId().ToString()));
                List<ReservationDTO> reservationDTOList = reservationListBL.GetReservationDTOList(searchParam);
                if (reservationDTOList != null && reservationDTOList.Count > 0)
                {
                    ReservationBL reservationBL = new ReservationBL(executionContext, utilities, reservationDTOList[0]);

                    keyReplaceList.Add(new KeyValuePair<string, string>("@fromTime", reservationBL.GetReservationDTO.FromDate.ToString(utilities.ParafaitEnv.DATETIME_FORMAT)));
                    keyReplaceList.Add(new KeyValuePair<string, string>("@toTime", (reservationBL.GetReservationDTO.ExpiryTime != null ? ((DateTime)reservationBL.GetReservationDTO.ExpiryTime).ToString(utilities.ParafaitEnv.DATETIME_FORMAT) : reservationBL.GetReservationDTO.ToDate.ToString(utilities.ParafaitEnv.DATETIME_FORMAT))));
                    keyReplaceList.Add(new KeyValuePair<string, string>("@creationDate", reservationBL.GetReservationDTO.CreationDate == null ? "" : reservationBL.GetReservationDTO.CreationDate.ToString(utilities.ParafaitEnv.DATE_FORMAT)));

                    keyReplaceList.Add(new KeyValuePair<string, string>("@reservationCode", reservationBL.GetReservationDTO.ReservationCode));
                    keyReplaceList.Add(new KeyValuePair<string, string>("@bookingName", reservationBL.GetReservationDTO.BookingName));
                    string facilityNames = reservationBL.GetReservationDTO.FacilityName;
                    keyReplaceList.Add(new KeyValuePair<string, string>("@partyRoom", facilityNames));
                    keyReplaceList.Add(new KeyValuePair<string, string>("@additionalItems", ""));

                    keyReplaceList.Add(new KeyValuePair<string, string>("@bookingRemarks", reservationBL.GetReservationDTO.Remarks));

                    int guestCount = 0;
                    int participantGuestCount = 0;
                    string bookingProductName = "";
                    Transaction.TransactionLine bookingProductTrxLine = reservationBL.GetBookingProductTransactionLine();
                    if (bookingProductTrxLine != null)
                    {
                        bookingProductName = bookingProductTrxLine.ProductName;
                    }

                    participantGuestCount = reservationBL.GetGuestQuantity();
                    guestCount = participantGuestCount + reservationBL.GetReservationDTO.ExtraGuests;
                    keyReplaceList.Add(new KeyValuePair<string, string>("@bookingProduct", bookingProductName));
                    keyReplaceList.Add(new KeyValuePair<string, string>("@guestCount", guestCount.ToString(utilities.ParafaitEnv.NUMBER_FORMAT)));
                    keyReplaceList.Add(new KeyValuePair<string, string>("@participantGuestCount", participantGuestCount.ToString(utilities.ParafaitEnv.NUMBER_FORMAT)));
                    keyReplaceList.Add(new KeyValuePair<string, string>("@nonParticipantGuestCount", reservationBL.GetReservationDTO.ExtraGuests.ToString(utilities.ParafaitEnv.NUMBER_FORMAT)));

                }

                if (isSubjectContent == false)
                {
                    TransactionCore transactionCore = new TransactionCore();
                    PurchasesListParams purchasesListParams = new PurchasesListParams();
                    purchasesListParams.TranscationId = this.transaction.Trx_id;
                    purchasesListParams.ShowTransactionLines = true;
                    purchasesListParams.LoginId = executionContext.GetUserId();
                    purchasesListParams.CustomerId = (custDTO == null ? -1 : custDTO.Id);
                    List<TransactionDetails> transactionDetails = transactionCore.GetPurchaseTransactions(purchasesListParams);
                    if (transactionDetails != null && transactionDetails.Count > 0)
                    {
                        List<TransactionDetails> transactionComboDetails = transactionCore.GetTicketTransactionDetailsList(transactionDetails);
                        keyReplaceList.Add(new KeyValuePair<string, string>("@merchantRefNo", ""));

                        string tourOperator = "";
                        string operatorMobileNo = "";
                        if (transactionDetails[0].TransactionUserDTO != null && transactionDetails[0].TransactionUserDTO.IsAgent == "Y")
                        {
                            tourOperator = "<tr><td> Tour Operator :  " + transactionDetails[0].TransactionUserDTO.Username + "</td></tr>";
                            operatorMobileNo = "<tr><td> Mobile No :   " + transactionDetails[0].TransactionUserDTO.MobileNo + "</td></tr>";
                        }

                        keyReplaceList.Add(new KeyValuePair<string, string>("@tourOperator", tourOperator));
                        keyReplaceList.Add(new KeyValuePair<string, string>("@mobileNo", operatorMobileNo));
                        List<KeyValuePair<string, string>> imageList = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("header", "~/images/header.jpg"),
                        new KeyValuePair<string, string>("footer", "~/images/footer.gif")
                    };

                        string ticketfirstString = "@StartTicketLine";
                        int ticketPos1 = emailContent.IndexOf(ticketfirstString) + ticketfirstString.Length;
                        int ticketPos2 = emailContent.IndexOf("@EndTicketLine");
                        if (ticketPos1 > 0 && ticketPos2 > 0)
                        {
                            string originalTicketLine = emailContent.Substring(ticketPos1, ticketPos2 - ticketPos1);
                            if (transactionDetails != null && transactionDetails[0] != null && transactionDetails[0].Products != null)
                            {
                                string productLines = GetTransactionTicketTableString(transactionDetails[0], originalTicketLine, custDTO);
                                emailContent = emailContent.Replace("@StartTicketLine" + originalTicketLine + "@EndTicketLine", productLines);
                            }
                        }
                    }
                }
                emailContent = UpdateTemplate(keyReplaceList, emailContent);
                emailContent = emailContent.Replace("\n", "");
            }
            return emailContent;
        }
        /// <summary>
        /// UpdateTemplate(List<KeyValuePair<string, string>> listContRepl, string emailContent) method
        /// </summary>
        /// <param name="listContRepl">listContRepl</param>
        /// <param name="emailContent">emailContent</param>
        /// <returns>returns string emailContentUpdated</returns>
        public static string UpdateTemplate(List<KeyValuePair<string, string>> listContRepl, string emailContent)
        {
            try
            {
                if (listContRepl != null)
                {
                    if (listContRepl.Count > 0)
                    {
                        for (int i = 0; i < listContRepl.Count; i++)
                        {
                            emailContent = emailContent.Replace(listContRepl[i].Key.ToString(), listContRepl[i].Value.ToString());
                        }
                    }
                }
                return emailContent;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// added rakshith
        /// </summary>
        private string GetTransactionTableString(TransactionDetails trxd, string Emailcontent)
        {
            StringBuilder productLine = new StringBuilder();
            string[] itemList = { "@productName", "@quantity", "@date", "@visit", "@price", "@subTotal", "@cardNumberBarCode", "@cardNumberQRCode", "@cardNumber", "@CardNumber" };
            foreach (LinkedPurchasedProducts pr in trxd.Products)
            {
                if (pr.ProductType != "DISCOUNT" && pr.ProductType != ProductTypeValues.SERVICECHARGE && pr.ProductType != ProductTypeValues.GRATUITY)
                {
                    string tempProductLine = Emailcontent;
                    foreach (string item in itemList)
                    {
                        if (tempProductLine.Contains(item))
                        {
                            TimeZoneUtil timeZoneUtil = new TimeZoneUtil();
                            int offSetDuration = timeZoneUtil.GetOffSetDuration(utilities.ExecutionContext.GetSiteId(), utilities.getServerTime().Date);
                            // The DB has data in HQ timezone i.e. the offset is already applied. Reverse this to show data correctly.
                            offSetDuration = offSetDuration * -1;

                            log.Debug("Applying offset to ScheduleDate:" + offSetDuration);

                            // for card products the pr.ScheduleDate  is DateTime.MinValue
                            if (pr.ProductType == "ATTRACTION" && pr.ScheduleDate != DateTime.MinValue)
                            {
                                log.Debug("Date before offset:" + pr.ScheduleDate + ":pId" + pr.ProductId + ":pName" + pr.ProductName);
                                pr.ScheduleDate = pr.ScheduleDate.AddSeconds(offSetDuration);
                                log.Debug("Date after offset:" + pr.ScheduleDate);
                            }
                            string prodName = pr.ProductType == "ATTRACTION" ? pr.ProductName + " - " + pr.ScheduleDate.ToString(utilities.ParafaitEnv.DATETIME_FORMAT) : pr.ProductName;
                            switch (item)
                            {
                                case "@productName":
                                    string tmpDiscPer = "";
                                    tmpDiscPer = (transactionPaymentsDTO.CardId > 0) ? CheckWhetherCCPConsumptionapplied(pr, utilities.ExecutionContext) : "";
                                    tempProductLine = tempProductLine.Replace(item, (pr.LinkedLineIdentifier == "-1" ? "" : "&nbsp;&nbsp;&nbsp;&nbsp;") + prodName + "&nbsp;" + tmpDiscPer);
                                    break;
                                case "@quantity": tempProductLine = tempProductLine.Replace(item, (pr.ProductType == "COMBO" && pr.Price <= 0) ? "" : (pr.Quantity > 0 ? pr.Quantity.ToString() : "")); break;
                                case "@cardNumber": tempProductLine = tempProductLine.Replace(item, pr.CardNumber); break;
                                case "@CardNumber": tempProductLine = tempProductLine.Replace(item, pr.CardNumber); break;
                                case "@date": tempProductLine = tempProductLine.Replace(item, this.transaction.TransactionDate.ToString(utilities.ParafaitEnv.DATETIME_FORMAT)); break;
                                case "@visit": tempProductLine = tempProductLine.Replace(item, this.transaction.TransactionDate.ToString(utilities.ParafaitEnv.DATETIME_FORMAT)); break;
                                case "@price": tempProductLine = tempProductLine.Replace(item, (pr.ProductType == "COMBO" && pr.Price <= 0) ? "" : pr.Price.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)); break;
                                case "@subTotal": tempProductLine = tempProductLine.Replace(item, (pr.ProductType == "COMBO" && pr.TotalAmount <= 0) ? "" : pr.TotalAmount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)); break;
                                case "@cardNumberBarCode":
                                    Image image = Code128Rendering.MakeBarcodeImage(pr.CardNumber, 1, true);
                                    if (image != null)
                                    {
                                        using (var stream = new MemoryStream())
                                        {
                                            image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                                            String accountBarCode = System.Convert.ToBase64String(stream.ToArray());
                                            tempProductLine = tempProductLine.Replace(item, "<img style='width:175px;height:100px;'src='data:image/jpeg;base64," + accountBarCode + "'/>");
                                        }
                                    }
                                    break;
                                case "@cardNumberQRCode":
                                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(pr.CardNumber, QRCodeGenerator.ECCLevel.Q);
                                    QRCode qrCode = new QRCode(qrCodeData);
                                    if (qrCode != null)
                                    {
                                        Image imageQR = qrCode.GetGraphic(5);
                                        if (imageQR != null)
                                        {
                                            using (var stream = new MemoryStream())
                                            {
                                                imageQR.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                                                String qrCodeB64 = System.Convert.ToBase64String(stream.ToArray());
                                                tempProductLine = tempProductLine.Replace(item, "<img style='width:100px;height:100px;'src='data:image/jpeg;base64," + qrCodeB64 + "'/>");
                                            }
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                    productLine.Append(tempProductLine);
                }
            }
            return productLine.ToString();
        }

        private string CheckWhetherCCPConsumptionapplied(LinkedPurchasedProducts linkedPurchasedProduct, ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            string ccpLabel = string.Empty;
            if (transactionPaymentsDTO.CardId > 0)
            {
                Transaction.TransactionLine productTransactionLines = transaction.TransactionLineList.FirstOrDefault(tl => tl.ProductID == linkedPurchasedProduct.ProductId);

                AccountBL accountBL = new AccountBL(executionContext, transactionPaymentsDTO.CardId);
                int discPercantage = accountBL.GetCardCPConsumptionDiscPercentage(productTransactionLines.CreditPlusConsumptionId);
                string disAppliedLabel = MessageContainerList.GetMessage(utilities.ExecutionContext, 230); //"Discount Applied"
                disAppliedLabel = disAppliedLabel.Contains("Message not defined for Message No") ? "Discount Applied" : disAppliedLabel;

                if (discPercantage > 0)
                {
                    ccpLabel = $"(*{discPercantage}% {disAppliedLabel})";
                }
            }
            log.LogMethodExit(ccpLabel);
            return (ccpLabel);
        }

        private string GetTransactionTicketTableString(TransactionDetails trxd, string Emailcontent, CustomerDTO custDTO)
        {
            StringBuilder productLine = new StringBuilder();
            string[] itemList = { "@productName", "@quantity", "@date", "@visit", "@price", "@subTotal", "@cardNumberBarCode", "@cardNumberQRCode",
                "@ticketLineTrxOTPBarCode", "@ticketLineTrxOTPQRCode","@ticketLineCustomerName", "@ticketLineCustomerPhone", "@ticketLineCustomerEmail", "@cardNumber","@CardNumber"};
            foreach (LinkedPurchasedProducts pr in trxd.Products)
            {
                if (pr.ProductType != "DISCOUNT" && pr.ProductType != ProductTypeValues.SERVICECHARGE && pr.ProductType != ProductTypeValues.GRATUITY)
                {
                    string tempProductLine = Emailcontent;
                    foreach (string item in itemList)
                    {
                        if (tempProductLine.Contains(item))
                        {
                            //string prodName = pr.ProductType == "ATTRACTION" ? pr.ProductName + " - " + utilities.FormatDate(pr.ScheduleDate, longDateFormat) + (pr.AttractionBookingList != null && pr.AttractionBookingList.Count() > 0 ? " to " + pr.AttractionBookingList[0].ExpiryDate.ToString("hh:mm tt") : "") : pr.ProductName;
                            string prodName = pr.ProductType == "ATTRACTION" ? pr.ProductName + " - " + pr.ScheduleDate.ToString(utilities.ParafaitEnv.DATETIME_FORMAT) : pr.ProductName;

                            switch (item)
                            {
                                case "@productName": tempProductLine = tempProductLine.Replace(item, (pr.LinkedLineIdentifier == "-1" ? "" : "&nbsp;&nbsp;&nbsp;&nbsp;") + prodName); break;
                                case "@quantity": tempProductLine = tempProductLine.Replace(item, (pr.ProductType == "COMBO" && pr.Price <= 0) ? "" : (pr.Quantity > 0 ? pr.Quantity.ToString() : "")); break;
                                case "@cardNumber": tempProductLine = tempProductLine.Replace(item, pr.CardNumber); break;
                                case "@CardNumber": tempProductLine = tempProductLine.Replace(item, pr.CardNumber); break;
                                case "@date": tempProductLine = tempProductLine.Replace(item, this.transaction.TransactionDate.ToString(utilities.ParafaitEnv.DATETIME_FORMAT)); break;
                                case "@visit": tempProductLine = tempProductLine.Replace(item, this.transaction.TransactionDate.ToString(utilities.ParafaitEnv.DATETIME_FORMAT)); break;
                                case "@price": tempProductLine = tempProductLine.Replace(item, (pr.ProductType == "COMBO" && pr.Price <= 0) ? "" : pr.Price.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)); break;
                                case "@subTotal": tempProductLine = tempProductLine.Replace(item, (pr.ProductType == "COMBO" && pr.TotalAmount <= 0) ? "" : pr.TotalAmount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)); break;
                                case "@ticketLineCustomerName": tempProductLine = tempProductLine.Replace(item, (custDTO == null || String.IsNullOrEmpty(custDTO.FirstName)) ? "" : custDTO.FirstName); break;
                                case "@ticketLineCustomerPhone": tempProductLine = tempProductLine.Replace(item, (custDTO == null || String.IsNullOrEmpty(custDTO.PhoneNumber)) ? "" : custDTO.PhoneNumber); break;
                                case "@ticketLineCustomerEmail": tempProductLine = tempProductLine.Replace(item, (custDTO == null || String.IsNullOrEmpty(custDTO.Email)) ? "" : custDTO.Email); break;
                                case "@cardNumberBarCode":
                                    Image image = Code128Rendering.MakeBarcodeImage(pr.CardNumber, 1, true);
                                    if (image != null)
                                    {
                                        using (var stream = new MemoryStream())
                                        {
                                            image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                                            String accountBarCode = System.Convert.ToBase64String(stream.ToArray());
                                            tempProductLine = tempProductLine.Replace(item, "<img style='width:175px;height:100px;'src='data:image/jpeg;base64," + accountBarCode + "'/>");
                                        }
                                    }
                                    break;
                                case "@cardNumberQRCode":
                                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(pr.CardNumber, QRCodeGenerator.ECCLevel.Q);
                                    QRCode qrCode = new QRCode(qrCodeData);
                                    if (qrCode != null)
                                    {
                                        Image imageQR = qrCode.GetGraphic(5);
                                        if (imageQR != null)
                                        {
                                            using (var stream = new MemoryStream())
                                            {
                                                imageQR.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                                                String qrCodeB64 = System.Convert.ToBase64String(stream.ToArray());
                                                tempProductLine = tempProductLine.Replace(item, "<img style='width:100px;height:100px;'src='data:image/jpeg;base64," + qrCodeB64 + "'/>");
                                            }
                                        }
                                    }
                                    break;
                                case "@ticketLineTrxOTPBarCode":
                                    if (!String.IsNullOrEmpty(this.transaction.transactionOTP))
                                    {
                                        Image otpimage = Code128Rendering.MakeBarcodeImage(this.transaction.transactionOTP, 1, true);
                                        if (otpimage != null)
                                        {
                                            using (var stream = new MemoryStream())
                                            {
                                                otpimage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                                                String trxOtpBarCode = System.Convert.ToBase64String(stream.ToArray());
                                                tempProductLine = tempProductLine.Replace(item, "<img style='width:175px;height:100px;'src='data:image/jpeg;base64," + trxOtpBarCode + "'/>");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        tempProductLine = tempProductLine.Replace(item, "");
                                    }
                                    break;
                                case "@ticketLineTrxOTPQRCode":
                                    if (!String.IsNullOrEmpty(this.transaction.transactionOTP))
                                    {
                                        QRCodeGenerator otpqrGenerator = new QRCodeGenerator();
                                        QRCodeData otpqrCodeData = otpqrGenerator.CreateQrCode(this.transaction.transactionOTP, QRCodeGenerator.ECCLevel.Q);
                                        QRCode otpqrCode = new QRCode(otpqrCodeData);
                                        if (otpqrCode != null)
                                        {
                                            Image imageQR = otpqrCode.GetGraphic(5);
                                            if (imageQR != null)
                                            {
                                                using (var stream = new MemoryStream())
                                                {
                                                    imageQR.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                                                    String qrCodeB64 = System.Convert.ToBase64String(stream.ToArray());
                                                    tempProductLine = tempProductLine.Replace(item, "<img style='width:100px;height:100px;'src='data:image/jpeg;base64," + qrCodeB64 + "'/>");
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        tempProductLine = tempProductLine.Replace(item, "");
                                    }
                                    break;
                            }
                        }
                    }
                    productLine.Append(tempProductLine);
                }
            }
            return productLine.ToString();
        }

        private string GetTransactionTaxTableString(List<Transaction.clsTransactionInfo.TaxInfo> taxInfoList, string Emailcontent)
        {
            StringBuilder productLine = new StringBuilder();
            string[] itemList = { "@taxName", "@taxPercentage", "@taxAmount" };
            foreach (Transaction.clsTransactionInfo.TaxInfo taxInfo in taxInfoList)
            {
                string tempProductLine = Emailcontent;
                foreach (string item in itemList)
                {
                    if (tempProductLine.Contains(item))
                    {
                        switch (item)
                        {
                            case "@taxName":
                                Regex rex = new Regex("@taxName", RegexOptions.IgnoreCase);
                                tempProductLine = rex.Replace(tempProductLine, (String.IsNullOrEmpty(taxInfo.TaxName) ? "" : taxInfo.TaxName), 1);
                                break;
                            case "@taxPercentage":
                                rex = new Regex("@taxPercentage", RegexOptions.IgnoreCase);
                                tempProductLine = rex.Replace(tempProductLine, (taxInfo.Percentage <= 0) ? "" : taxInfo.Percentage.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT), 1);
                                break;
                            case "@taxAmount":
                                rex = new Regex("@taxAmount", RegexOptions.IgnoreCase);
                                tempProductLine = rex.Replace(tempProductLine, (taxInfo.TaxAmount <= 0) ? "" : taxInfo.TaxAmount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL), 1);
                                break;
                        }
                    }
                }
                productLine.Append(tempProductLine);
            }
            return productLine.ToString();
        }


        private string GenerateSignWaiverLink()
        {
            log.LogMethodEntry();
            string baseURLWaiver = string.Empty;
            string signWaiverSubLink = string.Empty;
            string securityToken = string.Empty;
            string waiverLink = string.Empty;
            LookupValuesList lookupValuesList = new LookupValuesList(this.utilities.ExecutionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParamLP = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchParamLP.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, WAIVERSETUP));
            searchParamLP.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, BASEURLFORSIGNWAIVER));
            searchParamLP.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, this.utilities.ExecutionContext.GetSiteId().ToString()));
            List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchParamLP);
            baseURLWaiver = string.Empty;
            if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
            {
                baseURLWaiver = lookupValuesDTOList[0].Description;
            }
            if (string.IsNullOrEmpty(baseURLWaiver) || string.IsNullOrEmpty(baseURLWaiver.Trim()))
            {
                throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 2653));//Base URL for sign waiver is not defined in the setup
            }

            string subURLLink = string.Empty;
            if (this.transaction.IsReservationTransaction(null))
            {
                subURLLink = RESERVATIONWAIVERURL;
            }
            else
            {
                subURLLink = TRANSACTIONWAIVERURL;
            }
            searchParamLP.Clear();
            lookupValuesDTOList = null;
            searchParamLP.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, WAIVERSETUP));
            searchParamLP.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, subURLLink));
            searchParamLP.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, this.utilities.ExecutionContext.GetSiteId().ToString()));
            lookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchParamLP);
            signWaiverSubLink = string.Empty;
            if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
            {
                signWaiverSubLink = lookupValuesDTOList[0].Description;
            }
            if (string.IsNullOrEmpty(signWaiverSubLink) || string.IsNullOrEmpty(signWaiverSubLink.Trim()))
            {
                string missingParamName = MessageContainerList.GetMessage(utilities.ExecutionContext, subURLLink) + " " + MessageContainerList.GetMessage(utilities.ExecutionContext, "parameter");
                throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 2654, missingParamName));//'&1 details is missing in sign waiver URL setup'
            }
            securityToken = transaction.GenerateTransactionBasedToken();
            if (string.IsNullOrEmpty(securityToken) || string.IsNullOrEmpty(securityToken.Trim()))
            {
                throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 2655));// "Sorry unable to generate token for sign waiver email URL"
            }
            waiverLink = baseURLWaiver + signWaiverSubLink + securityToken;
            log.LogMethodExit(waiverLink);
            return waiverLink;
        }
        private string GenerateQRCodeB64ForURL(string urlLink)
        {
            log.LogMethodEntry();
            string qrCodeB64 = GenericUtils.GenerateQRCodeB64ForString(urlLink);
            log.LogMethodExit(qrCodeB64);
            return qrCodeB64;
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

        private string GetTransactionChargesString(List<Transaction.clsTransactionInfo.ChargeInfo> chargeInfoList, string Emailcontent)
        {
            StringBuilder chargeLine = new StringBuilder();
            string[] itemList = { "@ChargeName", "@ChargeAmount" };
            foreach (Transaction.clsTransactionInfo.ChargeInfo chargeInfo in chargeInfoList)
            {
                string tempProductLine = Emailcontent;
                foreach (string item in itemList)
                {
                    if (tempProductLine.Contains(item))
                    {
                        switch (item)
                        {
                            case "@ChargeName":
                                Regex rex = new Regex("@ChargeName", RegexOptions.IgnoreCase);
                                tempProductLine = rex.Replace(tempProductLine, (String.IsNullOrEmpty(chargeInfo.ChargeName) ? "" : chargeInfo.ChargeName), 1);
                                break;
                            case "@ChargeAmount":
                                rex = new Regex("@ChargeAmount", RegexOptions.IgnoreCase);
                                tempProductLine = rex.Replace(tempProductLine, (chargeInfo.ChargeAmount <= 0) ? "" : chargeInfo.ChargeAmount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL), 1);
                                break;
                        }
                    }
                }
                chargeLine.Append(tempProductLine);
            }
            return chargeLine.ToString();
        }

    }
}
