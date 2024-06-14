/********************************************************************************************
 * Project Name - SubscriptionMessageContentBuilder 
 * Description  -BL class to build message content for Subscription
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.110.0     14-Dec-2020    Guru S A             Created for Subscription changes                                                                               
 *2.120.0     18-Mar-2021    Guru S A             For Subscription phase 2 changes
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// SubscriptionMessageContentBuilder
    /// </summary>
    public class SubscriptionMessageContentBuilder
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SubscriptionHeaderDTO subscriptionHeaderDTO;
        private ExecutionContext executionContext;
        private Utilities utilities;
        private CustomerCreditCardsDTO customerCreditCardsDTO;
        private ParafaitFunctionEvents parafaitFunctionEvents;
        private string DATE_FORMAT = string.Empty;
        private string AMOUNT_FORMAT = string.Empty;
        private string NUMBER_FORMAT = string.Empty;
        private string DATETIME_FORMAT = string.Empty;
        private Transaction transaction;
        private bool isSubjectContent = false;

        private SubscriptionMessageContentBuilder(Utilities utilities, ExecutionContext executionContext, ParafaitFunctionEvents parafaitFunctionEvents)
        {
            log.LogMethodEntry(utilities, executionContext, parafaitFunctionEvents);
            this.utilities = utilities;
            this.executionContext = executionContext;
            this.parafaitFunctionEvents = parafaitFunctionEvents;
            log.LogMethodExit();
        }
        /// <summary>
        /// SubscriptionMessageContentBuilder
        /// </summary>
        /// <param name="utilities"></param>
        /// <param name="executionContext"></param>
        /// <param name="parafaitFunctionEvents"></param>
        /// <param name="subscriptionHeaderDTO"></param>
        /// <param name="transaction"></param>
        /// <param name="isSubjectContent"></param>
        public SubscriptionMessageContentBuilder(Utilities utilities, ExecutionContext executionContext, ParafaitFunctionEvents parafaitFunctionEvents, 
                                                SubscriptionHeaderDTO subscriptionHeaderDTO, Transaction transaction, bool isSubjectContent = false)
            : this(utilities, executionContext, parafaitFunctionEvents)
        {
            log.LogMethodEntry(executionContext, subscriptionHeaderDTO, parafaitFunctionEvents, isSubjectContent);
            this.subscriptionHeaderDTO = subscriptionHeaderDTO;
            this.transaction = transaction;
            this.customerCreditCardsDTO = null;
            this.isSubjectContent = isSubjectContent;
            SetFormats();
            log.LogMethodExit();
        }
        /// <summary>
        /// SubscriptionMessageContentBuilder
        /// </summary>
        /// <param name="utilities"></param>
        /// <param name="executionContext"></param>
        /// <param name="parafaitFunctionEvents"></param>
        /// <param name="customerCreditCardsDTO"></param>
        /// <param name="isSubjectContent"></param>
        public SubscriptionMessageContentBuilder(Utilities utilities, ExecutionContext executionContext, ParafaitFunctionEvents parafaitFunctionEvents,
                                        CustomerCreditCardsDTO customerCreditCardsDTO, bool isSubjectContent = false)
            : this(utilities, executionContext, parafaitFunctionEvents)
        {
            log.LogMethodEntry(executionContext, customerCreditCardsDTO);
            this.executionContext = executionContext;
            this.customerCreditCardsDTO = customerCreditCardsDTO;
            this.subscriptionHeaderDTO = null;
            this.transaction = null;
            this.isSubjectContent = isSubjectContent;
            SetFormats();
            log.LogMethodExit();
        }
        private void SetFormats()
        {
            log.LogMethodEntry();
            DATE_FORMAT = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATE_FORMAT");
            AMOUNT_FORMAT = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT");
            NUMBER_FORMAT = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "NUMBER_FORMAT");
            DATETIME_FORMAT = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATETIME_FORMAT");
            log.LogMethodExit();
        }
        /// <summary>
        /// GenerateMessageContent
        /// </summary>
        /// <param name="messageTemplateContent"></param>
        /// <returns></returns>
        public string GenerateMessageContent(string messageTemplateContent)
        {
            log.LogMethodEntry(messageTemplateContent);
            string formattedContent = string.Empty;
            string content = string.Empty;
            if (this.customerCreditCardsDTO != null)
            {
                content = SubscriptionCustomerCardTextFormatter(messageTemplateContent);
            }
            if (this.subscriptionHeaderDTO != null)
            {
                content = SubscriptionHeaderDTOTextFormatter(string.IsNullOrWhiteSpace(content) ? messageTemplateContent : content);
            }
            if (this.transaction != null)
            {
                TransactionEmailTemplatePrint transactionEmailTemplatePrint = new TransactionEmailTemplatePrint(executionContext, utilities, -1, transaction,null);
                content = transactionEmailTemplatePrint.BuildContent(string.IsNullOrWhiteSpace(content) ? messageTemplateContent : content);
            }
            formattedContent = content; 
            log.LogMethodExit(formattedContent);
            return formattedContent;
        }        
        private string SubscriptionCustomerCardTextFormatter(string messageTemplateContent)
        {
            log.LogMethodEntry(messageTemplateContent);
            string formattedContent = string.Empty;
            if (this.customerCreditCardsDTO != null)
            {
                TemplateKeywordFormatter templateKeywordFormatter = new TemplateKeywordFormatter();
                templateKeywordFormatter.Add("@CreditCardNumber", customerCreditCardsDTO.CreditCardNumber);
                templateKeywordFormatter.Add("@CardExpiry", customerCreditCardsDTO.CardExpiry); 
                SubscriptionHeaderListBL subscriptionHeaderListBL = new SubscriptionHeaderListBL(executionContext);
                List<KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.CUSTOMER_CREDIT_CARD_ID, this.customerCreditCardsDTO.CustomerCreditCardsId.ToString()));
                searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.IS_ACTIVE, "1"));
                searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.HAS_UNBILLED_CYCLES, "1"));
                List<SubscriptionHeaderDTO> subscriptionHeaderDTOlist = subscriptionHeaderListBL.GetSubscriptionHeaderDTOList(searchParameters, utilities, true);
                if (subscriptionHeaderDTOlist == null)
                {
                    subscriptionHeaderDTOlist = new List<SubscriptionHeaderDTO>();
                }
                templateKeywordFormatter.Add("@NumberOfActiveSubscriptions", subscriptionHeaderDTOlist.Count.ToString(NUMBER_FORMAT));
                templateKeywordFormatter.Add("@EarliestBillOnDate", GetEariliestBillOnDate(subscriptionHeaderDTOlist).ToString(DATE_FORMAT));

                TemplateText templateText = new TemplateText(messageTemplateContent);
                formattedContent = templateKeywordFormatter.Format(templateText);

                CustomerBL customerBL = new CustomerBL(executionContext, customerCreditCardsDTO.CustomerId);
                CustomerDTO customerDTO = customerBL.CustomerDTO;
                CustomerTemplateKeywordFormatter customerTemplateKeywordFormatter = null;
                if (customerDTO != null)
                {
                    customerTemplateKeywordFormatter = new CustomerTemplateKeywordFormatter(executionContext, customerDTO, null, parafaitFunctionEvents, isSubjectContent);
                    templateText = new TemplateText(formattedContent);
                    formattedContent = customerTemplateKeywordFormatter.Format(templateText);
                }
            }

            log.LogMethodExit(formattedContent);
            return formattedContent;
        }
        private DateTime GetEariliestBillOnDate(List<SubscriptionHeaderDTO> subscriptionHeaderDTOlist)
        {
            log.LogMethodEntry();
            DateTime earlierstBillOnDate = DateTime.MaxValue;
            for (int i = 0; i < subscriptionHeaderDTOlist.Count; i++)
            {
                DateTime earlierstBillOnDateTemp = subscriptionHeaderDTOlist[i].SubscriptionBillingScheduleDTOList.Where(sbs => sbs.IsActive
                                                                                                && sbs.LineType == SubscriptionLineType.BILLING_LINE
                                                                                                && (string.IsNullOrWhiteSpace(sbs.Status) || sbs.Status == SubscriptionStatus.ACTIVE)
                                                                                                && sbs.TransactionId == -1).Min(sbs => sbs.BillOnDate);
                if (earlierstBillOnDateTemp < earlierstBillOnDate)
                {
                    earlierstBillOnDate = earlierstBillOnDateTemp;
                }
            }         
            log.LogMethodExit(earlierstBillOnDate);
            return earlierstBillOnDate;
        }   
        private string SubscriptionHeaderDTOTextFormatter(string messageTemplateContent)
        {
            log.LogMethodEntry(messageTemplateContent);
            string formattedContent = string.Empty;
            if (this.subscriptionHeaderDTO != null)
            {
                TemplateKeywordFormatter templateKeywordFormatter = new TemplateKeywordFormatter();
                templateKeywordFormatter.Add("@SubscriptionName", subscriptionHeaderDTO.ProductSubscriptionName);
                templateKeywordFormatter.Add("@SubscriptionDescription", subscriptionHeaderDTO.ProductSubscriptionDescription);
                templateKeywordFormatter.Add("@SubscriptionPrice", subscriptionHeaderDTO.SubscriptionPrice.ToString(AMOUNT_FORMAT));
                templateKeywordFormatter.Add("@SubscriptionCycle", subscriptionHeaderDTO.SubscriptionCycle.ToString(NUMBER_FORMAT));
                templateKeywordFormatter.Add("@UnitOfSubscriptionCycle", UnitOfSubscriptionCycle.GetUnitOfSubscriptionCycleDescription(executionContext, subscriptionHeaderDTO.UnitOfSubscriptionCycle));
                templateKeywordFormatter.Add("@SubscriptionCycleValidity", subscriptionHeaderDTO.SubscriptionCycleValidity.ToString(NUMBER_FORMAT));
                templateKeywordFormatter.Add("@SeasonalSubscription", (subscriptionHeaderDTO.SeasonStartDate != null ? MessageContainerList.GetMessage(executionContext,"Yes")
                                                                                                              : MessageContainerList.GetMessage(executionContext, "No")));

                templateKeywordFormatter.Add("@SeasonStartDate", (subscriptionHeaderDTO.SeasonStartDate == null? "":
                                                                  ((DateTime)subscriptionHeaderDTO.SeasonStartDate).ToString(DATE_FORMAT)));
                templateKeywordFormatter.Add("@SubscriptionStartDate", (subscriptionHeaderDTO.SubscriptionStartDate == DateTime.MinValue ? "" :
                                                                  (subscriptionHeaderDTO.SubscriptionStartDate).ToString(DATE_FORMAT)));
                templateKeywordFormatter.Add("@SubscriptionEndDate", (subscriptionHeaderDTO.SubscriptionEndDate == DateTime.MinValue ? "" :
                                                                  (subscriptionHeaderDTO.SubscriptionEndDate).ToString(DATE_FORMAT)));
                //templateKeywordFormatter.Add("@SeasonEndDate", (subscriptionHeaderDTO.SeasonEndDate == null ? "" :
                //                                                  ((DateTime)subscriptionHeaderDTO.SeasonEndDate).ToString(DATE_FORMAT)));
                templateKeywordFormatter.Add("@FreeTrialPeriodCycle", (subscriptionHeaderDTO.FreeTrialPeriodCycle == null ? "0" :
                                                                                 ((int)subscriptionHeaderDTO.FreeTrialPeriodCycle).ToString(NUMBER_FORMAT)));
                templateKeywordFormatter.Add("@BillInAdvance", (subscriptionHeaderDTO.BillInAdvance ? MessageContainerList.GetMessage(executionContext, "Yes")
                                                                                                              : MessageContainerList.GetMessage(executionContext, "No")));
                templateKeywordFormatter.Add("@SelectedPaymentCollectionMode", subscriptionHeaderDTO.SelectedPaymentCollectionMode);
                templateKeywordFormatter.Add("@AutoRenew", (subscriptionHeaderDTO.AutoRenew ? MessageContainerList.GetMessage(executionContext, "Yes")
                                                                                                              : MessageContainerList.GetMessage(executionContext, "No")));
                templateKeywordFormatter.Add("@AutoRenewalMarkup", (subscriptionHeaderDTO.AutoRenewalMarkupPercent.Equals(null) ? "0" :
                                                                                 ((decimal)subscriptionHeaderDTO.AutoRenewalMarkupPercent).ToString(NUMBER_FORMAT)));
                templateKeywordFormatter.Add("@SubscriptionRenewalGrace", (subscriptionHeaderDTO.RenewalGracePeriodCycle.Equals(null) ? "0" :
                                                                                 ((decimal)subscriptionHeaderDTO.RenewalGracePeriodCycle).ToString(NUMBER_FORMAT)));
                templateKeywordFormatter.Add("@NoOfRenewalReminders", (subscriptionHeaderDTO.NoOfRenewalReminders.Equals(null) ? "0" :
                                                                                 ((decimal)subscriptionHeaderDTO.NoOfRenewalReminders).ToString(NUMBER_FORMAT)));
                templateKeywordFormatter.Add("@ReminderFrequency", (subscriptionHeaderDTO.ReminderFrequencyInDays.Equals(null) ? "0" :
                                                                                 ((decimal)subscriptionHeaderDTO.ReminderFrequencyInDays).ToString(NUMBER_FORMAT)));
                templateKeywordFormatter.Add("@FirstReminderBeforeXDays", (subscriptionHeaderDTO.SendFirstReminderBeforeXDays.Equals(null) ? "0" :
                                                                                 ((decimal)subscriptionHeaderDTO.SendFirstReminderBeforeXDays).ToString(NUMBER_FORMAT)));
                templateKeywordFormatter.Add("@LastRenewalReminderSentOn", (subscriptionHeaderDTO.LastRenewalReminderSentOn == null ? "" :
                                                                  ((DateTime)subscriptionHeaderDTO.LastRenewalReminderSentOn).ToString(DATE_FORMAT)));
                templateKeywordFormatter.Add("@RenewalReminderCount", (subscriptionHeaderDTO.RenewalReminderCount.Equals(null) ? "0" :
                                                                                 ((decimal)subscriptionHeaderDTO.RenewalReminderCount).ToString(NUMBER_FORMAT)));
                templateKeywordFormatter.Add("@PaymentRetryLimitReminderSentOn", (subscriptionHeaderDTO.LastPaymentRetryLimitReminderSentOn == null ? "" :
                                                                  ((DateTime)subscriptionHeaderDTO.LastPaymentRetryLimitReminderSentOn).ToString(DATE_FORMAT)));
                templateKeywordFormatter.Add("@PaymentRetryLimitReminderCount", (subscriptionHeaderDTO.PaymentRetryLimitReminderCount.Equals(null) ? "0" :
                                                                                 ((decimal)subscriptionHeaderDTO.PaymentRetryLimitReminderCount).ToString(NUMBER_FORMAT)));
                templateKeywordFormatter.Add("@AllowPause", (subscriptionHeaderDTO.AllowPause ? MessageContainerList.GetMessage(executionContext, "Yes")
                                                                                                              : MessageContainerList.GetMessage(executionContext, "No")));
                templateKeywordFormatter.Add("@FirstBillFromDate", subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Where(sbs => sbs.LineType == SubscriptionLineType.BILLING_LINE).Min(sbs => sbs.BillFromDate).ToString(DATE_FORMAT));
                templateKeywordFormatter.Add("@FirstBillToDate", subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Where(sbs => sbs.LineType == SubscriptionLineType.BILLING_LINE).Min(sbs => sbs.BillToDate).ToString(DATE_FORMAT));
                templateKeywordFormatter.Add("@FirstBillOnDate", subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Where(sbs => sbs.LineType == SubscriptionLineType.BILLING_LINE).Min(sbs => sbs.BillOnDate).ToString(DATE_FORMAT));
                templateKeywordFormatter.Add("@LastBillFromDate", subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Where(sbs => sbs.LineType == SubscriptionLineType.BILLING_LINE).Max(sbs => sbs.BillFromDate).ToString(DATE_FORMAT));
                templateKeywordFormatter.Add("@LastBillToDate", subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Where(sbs => sbs.LineType == SubscriptionLineType.BILLING_LINE).Max(sbs => sbs.BillToDate).ToString(DATE_FORMAT));
                templateKeywordFormatter.Add("@LastBillOnDate", subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Where(sbs => sbs.LineType == SubscriptionLineType.BILLING_LINE).Max(sbs => sbs.BillOnDate).ToString(DATE_FORMAT));

                if (subscriptionHeaderDTO.CustomerContactId > -1)
                {
                    ContactBL contactBL = new ContactBL(executionContext, subscriptionHeaderDTO.CustomerContactId);
                    ContactDTO contactDTO = contactBL.ContactDTO;
                    if (contactDTO != null)
                    {
                        templateKeywordFormatter.Add("@ContactInfo1", contactDTO.Attribute1);
                        templateKeywordFormatter.Add("@ContactInfo2", contactDTO.Attribute2);
                        templateKeywordFormatter.Add("@ContactType", contactDTO.ContactType.ToString());
                    }
                }
                if (subscriptionHeaderDTO.CustomerCreditCardsId > -1)
                {
                    CustomerCreditCardsBL customerCreditCardsBL = new CustomerCreditCardsBL(executionContext, subscriptionHeaderDTO.CustomerCreditCardsId);
                    CustomerCreditCardsDTO customerCreditCardsDTO = customerCreditCardsBL.CustomerCreditCardsDTO;
                    if (customerCreditCardsDTO != null)
                    {
                        templateKeywordFormatter.Add("@CreditCardNumber", customerCreditCardsDTO.CreditCardNumber);
                        templateKeywordFormatter.Add("@CardExpiry", customerCreditCardsDTO.CardExpiry);
                    }
                }
                TransactionBL transactionBL = new TransactionBL(executionContext, subscriptionHeaderDTO.TransactionId);
                TransactionDTO transactionDTO = transactionBL.TransactionDTO;
                if (transactionDTO != null)
                {
                    templateKeywordFormatter.Add("@TransactionDate", transactionDTO.TransactionDate.ToString(DATETIME_FORMAT));
                    templateKeywordFormatter.Add("@OriginalSystemReference", transactionDTO.OriginalSystemReference);
                    templateKeywordFormatter.Add("@TransactionOTP", transactionDTO.TransactionOTP);
                    templateKeywordFormatter.Add("@TransactionNumber", transactionDTO.TransactionNumber);
                } 

                TemplateText templateText = new TemplateText(messageTemplateContent);
                formattedContent = templateKeywordFormatter.Format(templateText);

                if (subscriptionHeaderDTO.CustomerId > -1)
                {
                    CustomerBL customerBL = new CustomerBL(executionContext, subscriptionHeaderDTO.CustomerId);
                    CustomerDTO customerDTO = customerBL.CustomerDTO;
                    CustomerTemplateKeywordFormatter customerTemplateKeywordFormatter = null;
                    if (customerDTO != null)
                    {
                        customerTemplateKeywordFormatter = new CustomerTemplateKeywordFormatter(executionContext, customerDTO, null, parafaitFunctionEvents, isSubjectContent);
                        templateText = new TemplateText(formattedContent);
                        formattedContent = customerTemplateKeywordFormatter.Format(templateText);
                    }
                }
            }

            log.LogMethodExit(formattedContent);
            return formattedContent;
        }

    }
}
