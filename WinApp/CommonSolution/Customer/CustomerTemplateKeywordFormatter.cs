/********************************************************************************************
 * Project Name - CustomerTemplateKeywordFormatter 
 * Description  -Template Keyword Formatter for Customer
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.110.0     12-Dec-2020    Guru S A             Created for Subscription changes                                                                               
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Site;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// CustomerTemplateKeywordFormatter
    /// </summary>
    public class CustomerTemplateKeywordFormatter : TemplateKeywordFormatter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private CustomerDTO customerDTO;
        private AccountDTO accountDTO;
        private LookupValuesList serverDateTime;
        private string DATE_FORMAT = string.Empty;
        private string AMOUNT_FORMAT = string.Empty;
        private ParafaitFunctionEvents parafaitFunctionEvent;
        private bool isSubject = false;
        private CustomerTemplateKeywordFormatter(ExecutionContext executionContext, ParafaitFunctionEvents parafaitFunctionEvents) :base()
        {
            log.LogMethodEntry(executionContext, parafaitFunctionEvents);
            this.executionContext = executionContext;
            this.parafaitFunctionEvent = parafaitFunctionEvents;
            serverDateTime = new LookupValuesList(executionContext);
            DATE_FORMAT = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATE_FORMAT");
            AMOUNT_FORMAT = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT");
            Add("@CustomerName", string.Empty);
            Add("@Address", string.Empty);
            Add("@City", string.Empty);
            Add("@State", string.Empty);
            Add("@Pin", string.Empty);
            Add("@Phone", string.Empty);
            Add("@CustomerUniqueId", string.Empty);
            Add("@CustomerTaxCode", string.Empty);
            log.LogMethodExit();
        }
        /// <summary>
        /// CustomerTemplateKeywordFormatter
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="customerDTO"></param>
        /// <param name="accountDTO"></param>
        /// <param name="parafaitFunctionEvents"></param>
        /// <param name="isSubject"></param>
        public CustomerTemplateKeywordFormatter(ExecutionContext executionContext, CustomerDTO customerDTO, AccountDTO accountDTO, ParafaitFunctionEvents parafaitFunctionEvents, bool isSubject = false)
            : this(executionContext, parafaitFunctionEvents)
        {
            log.LogMethodEntry("customerDTO", accountDTO, parafaitFunctionEvent, isSubject);
            this.customerDTO = customerDTO;
            this.accountDTO = accountDTO;
            this.isSubject = isSubject;
            if (customerDTO != null)
            {
                AddAllTags();
            }
            else
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Customer DTO")));
            }
            log.LogMethodExit();
        }

        private void AddAllTags()
        {
            log.LogMethodEntry();
            AddCustomerFirstNameTag();
            AddCustomerNameTag();
            AddAddressTags();
            AddPhoneNumberTag();
            AddEmailIDTag();
            AddSiteNameTag();
            Add("@CustomerUniqueId", customerDTO.UniqueIdentifier);
            Add("@CustomerTaxCode", customerDTO.TaxCode);
            Add("@BaseUrl", ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ONLINE_WEBSITE_URL"));
            AddRegistrationLinkTag();
            AddResetPasswordLinkTag();
            AddCustomerVerificationCodeTag();
            AddCustomerCardNumberTag();
            AddCardCreditsTag();
            AddCardExpiryAndValidityInDaysTags();
            log.LogMethodExit();
        }
        private void AddCustomerFirstNameTag()
        {
            log.LogMethodEntry();
            string firstName = (string.IsNullOrWhiteSpace(customerDTO.FirstName) ? "" : customerDTO.FirstName);
            Add("@FirstName", firstName);
            log.LogMethodExit();
        }
        private void AddCustomerNameTag()
        {
            log.LogMethodEntry();
            string customerFullName = (string.IsNullOrWhiteSpace(customerDTO.FirstName) ? "" : customerDTO.FirstName)
                                        + " " + (string.IsNullOrWhiteSpace(customerDTO.LastName) ? "" : customerDTO.LastName);
            Add("@CustomerName", customerFullName);
            Add("@customerName", customerFullName);
            log.LogMethodExit();
        }

        private void AddAddressTags()
        {
            log.LogMethodEntry();
            string address = string.Empty;
            string city = string.Empty;
            string stateName = string.Empty;
            string postalCode = string.Empty;
            if (customerDTO.AddressDTOList != null && customerDTO.AddressDTOList.Any())
            {
                address = (string.IsNullOrWhiteSpace(customerDTO.AddressDTOList[0].Line1) ? "" : customerDTO.AddressDTOList[0].Line1)
                                        + " " + (string.IsNullOrWhiteSpace(customerDTO.AddressDTOList[0].Line2) ? "" : customerDTO.AddressDTOList[0].Line2)
                                        + " " + (string.IsNullOrWhiteSpace(customerDTO.AddressDTOList[0].Line3) ? "" : customerDTO.AddressDTOList[0].Line3);

                city = customerDTO.AddressDTOList[0].City;
                stateName = customerDTO.AddressDTOList[0].StateName;
                postalCode = customerDTO.AddressDTOList[0].PostalCode;
            }
            Add("@Address", address);
            Add("@City", city);
            Add("@State", stateName);
            Add("@Pin", postalCode);
            log.LogMethodExit();
        }

        private void AddPhoneNumberTag()
        {
            log.LogMethodEntry();
            string phoneNumber = GetForwardSlashSeperatedPhoneNumbers();
            Add("@Phone", phoneNumber);
            log.LogMethodExit();
        }

        private void AddEmailIDTag()
        {
            log.LogMethodEntry();
            string emailId = customerDTO.Email;
            Add("@EmailId", emailId);
            log.LogMethodExit();
        }

        private string GetForwardSlashSeperatedPhoneNumbers()
        {
            log.LogMethodEntry();
            string phoneNumber = String.Empty;
            if (customerDTO.ContactDTOList != null && customerDTO.ContactDTOList.Any())
            {
                StringBuilder phoneNumbers = new StringBuilder();
                for (int i = 0; i < customerDTO.ContactDTOList.Count; i++)
                {
                    if (customerDTO.ContactDTOList[i].ContactType == ContactType.PHONE)
                    {
                        phoneNumbers.Append(customerDTO.ContactDTOList[i].Attribute1).Append("/");
                    }
                }
                phoneNumber = phoneNumbers.ToString();
                if (phoneNumber.Length > 0)
                {
                    phoneNumber = phoneNumber.Substring(0, phoneNumber.Length - 1);
                }
            }
            log.LogMethodExit(phoneNumber);
            return phoneNumber;
        }

        private void AddSiteNameTag()
        {
            log.LogMethodEntry();
            string siteName = GetSiteName();
            Add("@siteName", siteName);
            Add("@SiteName", siteName);
            log.LogMethodExit();
        }
        /// <summary>
        /// GetSiteName
        /// </summary>
        /// <returns></returns>
        public string GetSiteName()
        {
            log.LogMethodEntry(executionContext);
            SiteList siteList = new SiteList(executionContext);
            List<SiteDTO> siteDTOList = siteList.GetAllSites(new List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>());
            string siteName = string.Empty;
            if (siteDTOList != null)
            {
                SiteDTO siteDTO = null;
                if (executionContext.GetSiteId() == -1)
                {
                    siteDTO = siteDTOList.FirstOrDefault();
                }
                else
                {
                    siteDTO = siteDTOList.Where(x => x.SiteId == executionContext.GetSiteId()).FirstOrDefault();
                }
                if (siteDTO != null)
                {
                    siteName = siteDTO.SiteName;
                }
            } 
            log.LogMethodExit(siteName);
            return siteName;
        }

        private void AddRegistrationLinkTag()
        {
            log.LogMethodEntry();
            if (this.parafaitFunctionEvent == ParafaitFunctionEvents.REGISTRATION_LINK_EVENT && isSubject == false)
            {
                SecurityTokenBL securityTokenBL = new SecurityTokenBL(executionContext);
                securityTokenBL.GenerateToken(customerDTO.Guid, "Customer");
                Add("@accountActivationLink", ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ACCOUNT_ACTIVATION_URL"));
                Add("@accountActivationToken", securityTokenBL.GetSecurityTokenDTO.Token);
            }
            log.LogMethodExit();
        }
        private void AddResetPasswordLinkTag()
        {
            log.LogMethodEntry();
            if (this.parafaitFunctionEvent == ParafaitFunctionEvents.RESET_PASSWORD_EVENT && isSubject == false)
            {
                string securityTokenLink = CreateResetPasswordLink();
                Add("@passwordResetTokenLink", securityTokenLink);
            }
            log.LogMethodExit();
        }


        private string CreateResetPasswordLink()
        {
            log.LogMethodEntry();
            SecurityTokenBL securityTokenBL = new SecurityTokenBL(executionContext);
            string token = customerDTO.Guid.ToString().Replace("-", "").Substring(0, 10) + System.Guid.NewGuid().ToString().Replace("-", "") + DateTime.Now.Ticks.ToString();
            securityTokenBL.GenerateToken(customerDTO.Guid, "Customer", token.ToLower());
            string securityTokenLink = "account/passwordreset/" + customerDTO.Id + "/" + securityTokenBL.GetSecurityTokenDTO.Token;
            log.LogMethodExit();
            return securityTokenLink;
        }

        private void AddCustomerVerificationCodeTag()
        {
            log.LogMethodEntry();
            if (this.parafaitFunctionEvent == ParafaitFunctionEvents.CUSTOMER_VERIFICATION_EVENT && isSubject == false)
            {
                string verificationCode = GetlatestCustomerVerificationCode();
                Add("@verificationCode", verificationCode);
            }
            log.LogMethodExit();
        }
        private string GetlatestCustomerVerificationCode()
        {
            log.LogMethodEntry();
            string verificationCode = string.Empty;
            if (customerDTO.CustomerVerificationDTO != null)
            {
                verificationCode = customerDTO.CustomerVerificationDTO.VerificationCode;
            }
            log.LogMethodExit(verificationCode);
            return verificationCode;
        }

        private void AddCustomerCardNumberTag()
        {
            log.LogMethodEntry();
            if (accountDTO != null)
            {
                Add("@cardNumber", accountDTO.TagNumber);
                Add("@CardNumber", accountDTO.TagNumber);
            }
            log.LogMethodExit(); 
        }

        private void AddCardCreditsTag()
        {
            log.LogMethodEntry();
            if (accountDTO != null)
            {                
                Add("@CardCredits", (accountDTO.Credits == null || accountDTO.Credits == 0? "0": 
                                                         ((decimal)accountDTO.Credits).ToString(AMOUNT_FORMAT))); 
            }
            log.LogMethodExit();
        }
        
        private void AddCardExpiryAndValidityInDaysTags()
        {
            log.LogMethodEntry();
            if (accountDTO != null && isSubject == false)
            {
                int cardVality = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "CARD_VALIDITY", 0);
                DateTime cardExpiryDate = accountDTO.LastUpdateDate.Date.AddMonths(cardVality);
                log.Info("cardExpiryDate: " + cardExpiryDate.ToString(DATE_FORMAT));
                Add("@CardExpiry", cardExpiryDate.ToString(DATE_FORMAT));
                int validtyInDays = (serverDateTime.GetServerDateTime().Date - cardExpiryDate).Days;
                log.Info("validtyInDays: " + validtyInDays.ToString());
                Add("@ValidityInDays", validtyInDays.ToString());
            }
            log.LogMethodExit();
        }
    }
}
