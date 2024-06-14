/********************************************************************************************
 * Project Name - Customer BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        06-Feb-2017      Lakshminarayana     Created 
 *2.3.0       05-Jul-2017      Guru S A         RedemptionDiscountForMembership is added
 *2.4.0       25-Nov-2018      Raghuveera       Added new method to inactivate the customer
 *2.60        04-Mar-2019      Mushahid Faizan  Added SaveUpdateCustomerList() , CustomerListBL() parameterized Constructor.
 *2.60        20-May-2019      Mehraj           Added Methods GetCustomerImageBase64(), GetIdImageBase64()
 *2.60.2      22-May-2019      Jagan Mohana     Added Transaction to SaveUpdateCustomerList()
 *2.70.2        19-Jul-2019      Girish Kundar    Modified : Save() method. Now Insert/Update method returns the DTO instead of Id. 
* 2.70.2        20-Aug-2019      Laster Menezes   Added methods GetCurrentMembershipProgressionEffectiveToDate(), IsMember(), GetMembershipName(), 
 *                                                LoadCustomerRewards(), GetMembershipCard(), GetEarnedActiveLoyaltyPoints(), GetRequiredPointsToRetainMembership(), 
 *                                                GetExpiringPointsForMembership(int noOfMonths), GetGameRewards(), 
 *                                                GetDiscountRewards(),GetHowManyMorePointsForNextMembership(),GetNextLevelMembershipName()
 *2.70.2.0      26-Sep-2019      Guru S A         Waiver phase 2 enhancement changes 
 *2.70.2.0      30-Sep-2019      Deeksha          Created GetCustomerDTOList() method with list<int> as parameter
 *2.70.2        25-Nov-2019      Girish kundar    Modified : Customer unique attribute search
 *2.70.2        06-Feb-2020      Nitin Pai        Guest App Changes
 *2.70.2        01-Jan-2020      Jeevan           Modification: ForgotPassword token method GenerateJWT Token used and saved 
 *                                                with Guid token or URL compatability
 *                                                SaveCustomer method changes to SendRegistrationMail and SMS for 
 *                                                new customer based on Default setups
 *2.70.3      14-Feb-2020       Lakshminarayana   Modified: Creating unregistered customer during check-in process
 *2.70.3      10-Mar-2020       Lakshminarayana   Updated to change the customer authentication process
 *2.70.2      20-Dec-2019       Mushahid Faizan   Modified for Customer Registration related changes. & Removed Hard-Coded Exceptions to MessageNo.                                              
 *2.80        15-Jun-2020       Nitin Pai         Moving change to replace the User Name from BL to UI Client (App, Website etc)
 *2.90        03-July-2020      Girish Kundar     Modified : Change as part of CardCodeDTOList replaced with AccountDTOList in CustomerDTO 
 *2.90.0      23-Jul-2020       Jinto Thomas      Modified: Messaging Enhancement, creatin messaging reauest using messaging clinet structure
 *2.100       19-Oct-2020       Guru S A          Enabling minor signature option for waiver
 *2.110.0     10-Dec-2020       Guru S A          For Subscription changes                   
 *2.120.0     09-Oct-2020       Guru S A          Membership engine sql session issue
 *2.120.1     31-May-2021       Nitin Pai         Show Customer age on screen/Don't allow edit of Customer with active waivers
 *2.130.7     23-Apr-2022       Nitin Pai         Add DBSyncEntries for a customer outside of SQL transaction
 *2.130.10    23-Jul-2022       Nitin Pai         Add DBSyncEntries for custom data set also for all sites
 *2.130.10    02-Sep-2022       Nitin Pai         Modified: Address field is truncated to 50 chars for backward compatability
 *2.130.10    08-Sep-2022       Nitin Pai         Added function to remove card to customer link. Added as part of customer delete enhancement.
 *2.130.10    08-Sep-2022       Nitin Pai         Enhanced customer activity user log table
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Semnox.Core.GenericUtilities;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using Semnox.Parafait.CardCore;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Customer.Membership;
using Semnox.Parafait.Customer.Waivers;
using Semnox.Parafait.DBSynch;
using Semnox.Parafait.Game;
using Semnox.Parafait.JobUtils;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Site;
using Semnox.Parafait.User;
using Semnox.Parafait.Waiver;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Business logic for Customer class.
    /// </summary>
    public class CustomerBL
    {
        private CustomerDTO customerDTO;
        private readonly ExecutionContext executionContext;
        private string passPhrase;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ParafaitDBTransaction parafaitDBTrx;
        private bool loadCustomerSignedWaivers = true;

        /// <summary>
        /// Parameterized constructor of CustomerBL class
        /// </summary>
        private CustomerBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            passPhrase = Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE");
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the customer id as the parameter
        /// Would fetch the customer object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="loadChildRecords">whether to load the child records</param>
        /// <param name="activeChildRecords">whether to load only active child records</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public CustomerBL(ExecutionContext executionContext, int id, bool loadChildRecords = true, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, loadChildRecords, activeChildRecords, sqlTransaction);
            CustomerDataHandler customerDataHandler = new CustomerDataHandler(passPhrase, sqlTransaction);
            customerDTO = customerDataHandler.GetCustomerDTO(id);
            if (customerDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "Customer", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (loadChildRecords)
            {
                Build(activeChildRecords, false, sqlTransaction);
            }

            log.LogMethodExit();
        }

        private void Build(bool activeChildRecords = true, bool loadSignedWaivers = false, SqlTransaction sqlTransaction = null, bool loadSignedWaiverFileContent = false, Utilities utilities = null, bool buildActiveCampaignActivity = false, bool buildLastVistitedDate = false, DateTime? fromDate = null, DateTime? toDate = null)
        {
            log.LogMethodEntry("customerDTO", activeChildRecords, loadSignedWaivers, loadSignedWaiverFileContent);
            if (customerDTO != null && customerDTO.Id != -1)
            {
                if (customerDTO.ProfileId != -1)
                {
                    ProfileBL profileBL = new ProfileBL(executionContext, customerDTO.ProfileId, true, activeChildRecords, sqlTransaction);
                    customerDTO.ProfileDTO = profileBL.ProfileDTO;
                }
                if (customerDTO.CustomDataSetId != -1)
                {
                    CustomDataSetBL customDataSetBL = new CustomDataSetBL(executionContext, customerDTO.CustomDataSetId, true, activeChildRecords, sqlTransaction);
                    customerDTO.CustomDataSetDTO = customDataSetBL.CustomDataSetDTO;
                }
                if (loadSignedWaivers)
                {
                    CustomerSignedWaiverListBL customerSignedWaiverListBL = new CustomerSignedWaiverListBL(executionContext);
                    List<int> custIdList = new List<int>();
                    custIdList.Add(customerDTO.Id);
                    customerDTO.CustomerSignedWaiverDTOList = customerSignedWaiverListBL.GetAllCustomerSignedWaiverList(custIdList, true, loadSignedWaiverFileContent, utilities, sqlTransaction);
                }
                if (buildLastVistitedDate || buildActiveCampaignActivity)
                {
                    String accountIdList = "";
                    AccountListBL accountListBL = new AccountListBL(executionContext);
                    List<KeyValuePair<AccountDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AccountDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.CUSTOMER_ID, customerDTO.Id.ToString()));
                    if (activeChildRecords)
                    {
                        searchParameters.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.VALID_FLAG, "Y"));
                    }
                    List<AccountDTO> accountList = accountListBL.GetAccountDTOList(searchParameters, false, true, null);
                    if (accountList != null && accountList.Any())
                    {
                        foreach (AccountDTO account in accountList)
                        {
                            if (accountIdList.Length > 0)
                                accountIdList += ",";
                            accountIdList += account.AccountId;
                        }

                        // this build the last visited date based on the card activity. Also chec from customer activity view
                        accountList = accountList.OrderByDescending(x => x.LastUpdateDate).ToList();
                        customerDTO.LastVisitedDate = accountList[0].LastUpdateDate;
                    }

                    if (buildActiveCampaignActivity || buildLastVistitedDate)
                    {
                        ActiveCampaignCustomerInfoListBL activeCampaignListBL = new ActiveCampaignCustomerInfoListBL(executionContext);
                        DateTime startDate = Convert.ToDateTime(fromDate);
                        DateTime endDate = Convert.ToDateTime(toDate);
                        List<ActiveCampaignCustomerInfoDTO> customerActivityDTOList = activeCampaignListBL.BuildCustomerActivity(customerDTO.Id, accountIdList, startDate, endDate, sqlTransaction);
                        if (customerActivityDTOList != null && customerActivityDTOList.Any())
                        {
                            customerDTO.ActiveCampaignCustomerInfoDTOList = customerActivityDTOList;

                            if (buildLastVistitedDate && customerActivityDTOList[0].LastPurchasedDate != null)
                                customerDTO.LastVisitedDate = customerDTO.LastVisitedDate < customerActivityDTOList[0].LastPurchasedDate ? customerActivityDTOList[0].LastPurchasedDate : customerDTO.LastVisitedDate;
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates CustomerBL object using the CustomerDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="customerDTO">CustomerDTO object</param>
        public CustomerBL(ExecutionContext executionContext, CustomerDTO customerDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, customerDTO);
            this.customerDTO = customerDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="customerDTO">customerDTO</param>
        /// <param name="loadCardDetails">loadCards true or false</param>
        public CustomerBL(ExecutionContext executionContext, CustomerDTO customerDTO, bool loadCardDetails)
        {
            log.LogMethodEntry(executionContext);
            this.customerDTO = customerDTO;
            this.executionContext = executionContext;
            if (string.IsNullOrWhiteSpace(passPhrase))
            {
                passPhrase = Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE");
            }
            if (loadCardDetails)
            {
                AccountListBL accountListBL = new AccountListBL(executionContext);
                AccountSearchCriteria accountSearchCriteria = new AccountSearchCriteria(AccountDTO.SearchByParameters.CUSTOMER_ID, Operator.EQUAL_TO, customerDTO.Id);
                List<AccountDTO> accountDTOList = accountListBL.GetAccountDTOList(accountSearchCriteria, true, true);
                if (customerDTO.AccountDTOList == null)
                {
                    customerDTO.AccountDTOList = new List<AccountDTO>();
                }
                if (accountDTOList != null && accountDTOList.Any())
                {
                    customerDTO.AccountDTOList = accountDTOList;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="customerDTO">customerDTO</param>
        /// <param name="cardDTO">cardDTO</param>
        /// <param name="loadCardDetails">loadCards true or false</param>
        /// <param name="sqlTransaction">sql transaction</param>
        public CustomerBL(ExecutionContext executionContext, CustomerDTO customerDTO, AccountDTO accountDTO, bool loadCardDetails, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            if (string.IsNullOrWhiteSpace(passPhrase))
            {
                passPhrase = Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE");
            }
            CustomerDataHandler customerDataHandler = new CustomerDataHandler(passPhrase, sqlTransaction);
            this.customerDTO = customerDTO;

            if (loadCardDetails)
            {
                AccountListBL accountListBL = new AccountListBL(executionContext);
                AccountSearchCriteria accountSearchCriteria = new AccountSearchCriteria(AccountDTO.SearchByParameters.CUSTOMER_ID, Operator.EQUAL_TO, customerDTO.Id);
                List<AccountDTO> accountDTOList = accountListBL.GetAccountDTOList(accountSearchCriteria, true, true, sqlTransaction);
                if (accountDTOList != null && accountDTOList.Any())
                {
                    customerDTO.AccountDTOList = accountDTOList;
                }
                else
                {
                    customerDTO.AccountDTOList = new List<AccountDTO>();
                }
            }
            if (accountDTO != null)
            {
                if (customerDTO.AccountDTOList == null)
                {
                    customerDTO.AccountDTOList = new List<AccountDTO>();
                }
                if (customerDTO.AccountDTOList.Exists(cust => cust.AccountId == accountDTO.AccountId) == false)
                {
                    customerDTO.AccountDTOList.Add(accountDTO);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Display Customer AGE
        /// </summary>
        /// <returns>Return the Current Age</returns>
        public int GetAge()
        {
            log.LogMethodEntry();

            DateTime today = DateTime.Today;
            int currentDate = (today.Year * 100 + today.Month) * 100 + today.Day;
            int customerDoB = (customerDTO.DateOfBirth.Value.Year * 100 + customerDTO.DateOfBirth.Value.Month) * 100 + customerDTO.DateOfBirth.Value.Day;
            int currentAge = (currentDate - customerDoB) / 10000;

            log.LogMethodExit(currentAge);
            return currentAge;            
        }

        /// <summary>
        /// SetGuestCustomer - Creates guest customer if not created.
        /// </summary>
        public void SetGuestCustomer()
        {
            log.LogMethodEntry();
            ParafaitDBTransaction parafaitDBTransaction = new ParafaitDBTransaction();
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParms = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                searchParms.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "WAIVER_SETUP"));
                searchParms.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "GUEST_CUSTOMER_FOR_WAIVER"));
                searchParms.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchParms);
                if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                {
                    string guestCustomerGuid = lookupValuesDTOList[0].Description;

                    if (string.IsNullOrEmpty(guestCustomerGuid))
                    {
                        parafaitDBTransaction.BeginTransaction();
                        customerDTO = BuildGuestCustomerDTO();
                        CustomerBL customerBL = new CustomerBL(executionContext, customerDTO);
                        customerBL.Save(parafaitDBTransaction.SQLTrx);
                        lookupValuesDTOList[0].Description = customerBL.CustomerDTO.Guid;
                        LookupValues lookupValues = new LookupValues(executionContext, lookupValuesDTOList[0]);
                        lookupValues.Save(parafaitDBTransaction.SQLTrx);
                        parafaitDBTransaction.EndTransaction();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (parafaitDBTransaction != null)
                {
                    parafaitDBTransaction.RollBack();
                }
                throw;
            }
            log.LogMethodExit();
        }

        private CustomerDTO BuildGuestCustomerDTO()
        {
            log.LogMethodEntry();
            CustomerDTO customerDTO = new CustomerDTO();
            try
            {
                if (customerDTO != null)
                {
                    if (customerDTO.ProfileDTO != null)
                    {
                        customerDTO.ProfileDTO.DateOfBirth = new DateTime(1753, 01, 01);
                        customerDTO.FirstName = "Waiver Guest First Name";
                        String uniqueEmail = Guid.NewGuid().ToString() + "@email.com";
                        customerDTO.Email = uniqueEmail;
                        ContactDTO contactEmailDTO = new ContactDTO();
                        contactEmailDTO.Attribute1 = contactEmailDTO.Attribute2 = uniqueEmail;
                        contactEmailDTO.ContactType = ContactType.EMAIL;
                        customerDTO.ProfileDTO.ContactDTOList = new List<ContactDTO>();
                        customerDTO.ProfileDTO.ContactDTOList.Add(contactEmailDTO);

                        if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "GENDER") == "M")
                        {
                            customerDTO.ProfileDTO.Gender = "F";
                        }
                        else
                        {
                            customerDTO.ProfileDTO.Gender = null;
                        }
                        if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ANNIVERSARY") == "M")
                        {
                            customerDTO.ProfileDTO.Anniversary = new DateTime(1753, 01, 01);
                        }
                        else
                        {
                            customerDTO.ProfileDTO.Anniversary = null;
                        }
                        if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "NOTES") == "M")
                        {
                            customerDTO.ProfileDTO.Notes = "Notes";
                        }
                        else
                        {
                            customerDTO.ProfileDTO.Notes = null;
                        }
                        if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "COMPANY") == "M")
                        {
                            customerDTO.ProfileDTO.Company = "Company";
                        }
                        else
                        {
                            customerDTO.ProfileDTO.Company = null;
                        }
                        if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DESIGNATION") == "M")
                        {
                            customerDTO.ProfileDTO.Designation = "Designation";
                        }
                        else
                        {
                            customerDTO.ProfileDTO.Designation = null;
                        }
                        if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "UNIQUE_ID") == "M")
                        {
                            customerDTO.ProfileDTO.UniqueIdentifier = Guid.NewGuid().ToString();
                        }
                        else
                        {
                            customerDTO.ProfileDTO.UniqueIdentifier = null;
                        }
                        if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "USERNAME") == "M")
                        {
                            customerDTO.ProfileDTO.UserName = "WaiverGuestCustomer";
                        }
                        else
                        {
                            customerDTO.ProfileDTO.UserName = null;
                        }
                        if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "EXTERNALSYSTEMREF") == "M")
                        {
                            customerDTO.ExternalSystemReference = "1234";
                        }
                        else
                        {
                            customerDTO.ExternalSystemReference = "";
                        }
                        if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "LAST_NAME") == "M")
                        {
                            customerDTO.LastName = "Guest Last Name";
                        }
                        else
                        {
                            customerDTO.LastName = "";
                        }

                        if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CHANNEL") == "M")
                        {
                            customerDTO.Channel = "Channel";
                        }
                        else
                        {
                            customerDTO.Channel = "";
                        }
                        if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "TAXCODE") == "M")
                        {
                            customerDTO.TaxCode = "TaxCode";
                        }
                        else
                        {
                            customerDTO.TaxCode = null;
                        }
                        if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_PHOTO") == "M")
                        {
                            customerDTO.PhotoURL = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "IMAGE_DIRECTORY");
                        }
                        else
                        {
                            customerDTO.PhotoURL = "";
                        }

                        if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "MIDDLE_NAME") == "M")
                        {
                            customerDTO.MiddleName = "Guest Middle Name";
                        }
                        else
                        {
                            customerDTO.MiddleName = "";
                        }
                        AddressDTO addressDTO = new AddressDTO();
                        bool addAddress = false;
                        if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CITY") == "M"
                            || ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "STATE") == "M"
                            || ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "COUNTRY") == "M"
                            || ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "PIN") == "M"
                            || ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ADDRESS1") == "M"
                            || ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ADDRESS2") == "M"
                            || ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ADDRESS3") == "M"
                            )
                        {
                            addAddress = true;
                            addressDTO.City = "City";
                            List<StateDTO> stateDTOs = new List<StateDTO>();
                            StateList stateList = new StateList();
                            StateParams stateParams = new StateParams();
                            stateDTOs = stateList.GetStateList(stateParams);
                            if (stateDTOs != null && stateDTOs.Any())
                            {
                                addressDTO.StateName = stateDTOs[0].Description;
                                addressDTO.StateId = stateDTOs[0].StateId;
                                addressDTO.StateCode = stateDTOs[0].State;
                                CountryBL countryBL = new CountryBL(stateDTOs[0].CountryId);
                                addressDTO.CountryId = stateDTOs[0].CountryId;
                                addressDTO.CountryName = countryBL.GetCountryDTO.CountryName;
                            }
                            addressDTO.PostalCode = "123456";
                            addressDTO.Line1 = "Address1";
                            addressDTO.Line2 = "Address2";
                            addressDTO.Line3 = "Address3";
                        }
                        if (addAddress)
                        {
                            addressDTO.AddressType = AddressType.HOME;
                            customerDTO.ProfileDTO.AddressDTOList = new List<AddressDTO>();
                            customerDTO.ProfileDTO.AddressDTOList.Add(addressDTO);
                        }
                        string uniquePhoneNumber = "1234567890";
                        if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CONTACT_PHONE") == "M")
                        {
                            int phoneNumberWidth = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "CUSTOMER_PHONE_NUMBER_WIDTH", 10);
                            using (Utilities parafaitUtility = new Utilities(parafaitDBTrx.GetConnection()))
                            {
                                //parafaitUtility.ParafaitEnv.LoginID = executionContext.GetUserId();
                                //parafaitUtility.ParafaitEnv.User_Id = executionContext.GetUserPKId();
                                //parafaitUtility.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
                                //parafaitUtility.ExecutionContext.SetIsCorporate(executionContext.GetIsCorporate());
                                //parafaitUtility.ExecutionContext.SetSiteId(executionContext.GetSiteId());
                                //parafaitUtility.ExecutionContext.SetUserId(executionContext.GetUserId());
                                uniquePhoneNumber = parafaitUtility.GenerateRandomNumber(phoneNumberWidth, Utilities.RandomNumberType.Numeric);
                            }

                            ContactDTO contactDTO = new ContactDTO();
                            bool addContact = false;
                            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CONTACT_PHONE") == "M")
                            {
                                contactDTO.Attribute1 = contactDTO.Attribute2 = uniquePhoneNumber;
                                contactDTO.ContactType = ContactType.PHONE;
                                addContact = true;
                            }
                            if (addContact)
                            {
                                customerDTO.ProfileDTO.ContactDTOList.Add(contactDTO);
                            }

                            customerDTO.PhoneNumber = uniquePhoneNumber;

                        }
                        else
                        {
                            customerDTO.PhoneNumber = "";
                        }

                        if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "FBUSERID") == "M"
                            || ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "FBACCESSTOKEN") == "M")
                        {
                            ContactDTO contactFBDTO = new ContactDTO();
                            contactFBDTO.Attribute1 = "FBUserId";
                            contactFBDTO.Attribute2 = "FBACCESSTOKEN";
                            contactFBDTO.ContactType = ContactType.FACEBOOK;
                            customerDTO.FBUserId = "FBUserId";
                            customerDTO.FBAccessToken = "FBACCESSTOKEN";
                            customerDTO.ContactDTOList.Add(contactFBDTO);
                        }
                        else
                        {
                            customerDTO.FBUserId = string.Empty;
                            customerDTO.FBAccessToken = string.Empty;
                        }
                        if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "TWACCESSTOKEN") == "M"
                           || ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "TWACCESSSECRET") == "M")
                        {
                            ContactDTO contactTWDTO = new ContactDTO();
                            contactTWDTO.Attribute1 = "TWACCESSTOKEN";
                            contactTWDTO.Attribute2 = "TWACCESSSECRET";
                            contactTWDTO.ContactType = ContactType.TWITTER;
                            customerDTO.TWAccessToken = "TWACCESSTOKEN";
                            customerDTO.TWAccessSecret = "TWACCESSSECRET";
                            customerDTO.ContactDTOList.Add(contactTWDTO);
                        }
                        else
                        {
                            customerDTO.TWAccessToken = string.Empty;
                            customerDTO.TWAccessSecret = string.Empty;
                        }
                        if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "WECHAT_ACCESS_TOKEN") == "M")
                        {
                            ContactDTO contactWeCDTO = new ContactDTO();
                            contactWeCDTO.Attribute1 = "WeChatAccessToken";
                            contactWeCDTO.ContactType = ContactType.WECHAT;
                            customerDTO.WeChatAccessToken = "TWACCESSTOKEN";
                            customerDTO.ContactDTOList.Add(contactWeCDTO);
                        }
                        else
                        {
                            customerDTO.WeChatAccessToken = string.Empty;
                        }
                        if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "RIGHTHANDED") == "M")
                        {
                            customerDTO.ProfileDTO.RightHanded = false;
                        }
                        if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "TITLE") == "M")
                        {
                            customerDTO.ProfileDTO.Title = "Mr.";
                        }
                        else
                        {
                            customerDTO.ProfileDTO.Title = null;
                        }
                        customerDTO.ProfileDTO.OptInPromotions = false;
                        if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "TERMS_AND_CONDITIONS") == "M")
                        {
                            customerDTO.ProfileDTO.PolicyTermsAccepted = true;
                        }
                        if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "OPT_IN_PROMOTIONS") == "M" ||
                            ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "OPT_IN_PROMOTIONS_MODE") == "M")
                        {
                            customerDTO.ProfileDTO.OptInPromotions = true;
                            customerDTO.ProfileDTO.OptInPromotionsMode = "None";
                        }
                    }
                    CustomDataSetDTO customDataSetDTO = null;
                    CustomAttributesListBL customAttributesListBL = new CustomAttributesListBL(executionContext);
                    List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>>();
                    searchParams.Add(new KeyValuePair<CustomAttributesDTO.SearchByParameters, string>(CustomAttributesDTO.SearchByParameters.APPLICABILITY, "CUSTOMER"));
                    searchParams.Add(new KeyValuePair<CustomAttributesDTO.SearchByParameters, string>(CustomAttributesDTO.SearchByParameters.IS_ACTIVE, "1"));
                    searchParams.Add(new KeyValuePair<CustomAttributesDTO.SearchByParameters, string>(CustomAttributesDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    List<CustomAttributesDTO> customAttributesDTOList = customAttributesListBL.GetCustomAttributesDTOList(searchParams, true);
                    if (customAttributesDTOList != null && customAttributesDTOList.Any())
                    {

                        for (int i = 0; i < customAttributesDTOList.Count; i++)
                        {
                            CustomDataDTO customDataDTO = null;
                            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, customAttributesDTOList[i].Name) == "M")
                            {
                                customDataDTO = new CustomDataDTO();
                                customDataDTO.CustomAttributeId = customAttributesDTOList[i].CustomAttributeId;
                                switch (customAttributesDTOList[i].Type)
                                {
                                    case "TEXT":
                                        {
                                            customDataDTO.CustomDataText = "Guest";
                                            break;
                                        }
                                    case "NUMBER":
                                        {
                                            customDataDTO.CustomDataNumber = 0;
                                            break;
                                        }
                                    case "DATE":
                                        {
                                            customDataDTO.CustomDataDate = new DateTime(1753, 01, 01);
                                            break;
                                        }
                                    case "LIST":
                                        {
                                            try
                                            {
                                                customDataDTO.ValueId = customAttributesDTOList[i].CustomAttributeValueListDTOList[0].ValueId;
                                            }
                                            catch (Exception ex)
                                            {
                                                customDataDTO.ValueId = 0;
                                                log.LogVariableState("customAttributesDTOList[i]", customAttributesDTOList[i]);
                                                log.Error(ex);
                                            }
                                            break;
                                        }
                                }
                            }
                            if (customDataDTO != null)
                            {
                                if (customDataSetDTO == null)
                                {
                                    customDataSetDTO = new CustomDataSetDTO();
                                    customDataSetDTO.CustomDataDTOList = new List<CustomDataDTO>();
                                }
                                customDataSetDTO.CustomDataDTOList.Add(customDataDTO);
                            }
                        }
                    }
                    if (customDataSetDTO != null)
                    {
                        customerDTO.CustomDataSetDTO = customDataSetDTO;
                    }
                    customerDTO.IsActive = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw ex;
            }
            log.LogMethodExit(customerDTO);
            return customerDTO;
        }
        /// <summary>
        /// Saves the Customer
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            CustomerDataHandler customerDataHandler = new CustomerDataHandler(passPhrase, sqlTransaction);

            List<ValidationError> validationErrorList = Validate(sqlTransaction);
            if (validationErrorList.Count > 0)
            {
                //Customer has lost the password and trying to reset/change password. meanwhile the customer mandatory fields have changed.
                //If we throw validation exception customer doesn't have any way to reset or change the password.
                if (string.IsNullOrWhiteSpace(customerDTO.Password) == false &&
                    customerDTO.Id >= 0 &&
                    customerDTO.ProfileDTO != null)
                {
                    ProfileBL profileBL = new ProfileBL(executionContext, customerDTO.ProfileDTO);
                    profileBL.UpdatePassword(customerDTO.Password, sqlTransaction);
                    customerDTO.Password = string.Empty;
                    log.LogMethodExit("Updated only customer password.");
                    return;
                }
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1946), validationErrorList);
            }
            SaveCustomer(sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Customer
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        private void SaveCustomer(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            bool isCustomerBackwardCompatibilityChanged = customerDTO.IsChanged || customerDTO.ProfileDTO.IsChangedRecursive;
            bool newCustomer = false;
            if (customerDTO.CustomDataSetDTO != null &&
               customerDTO.CustomDataSetDTO.CustomDataDTOList != null &&
               customerDTO.CustomDataSetDTO.CustomDataDTOList.Count > 0)
            {
                CustomDataSetBL customDataSetBL = new CustomDataSetBL(executionContext, customerDTO.CustomDataSetDTO);
                customDataSetBL.Save(sqlTransaction);
                if (customerDTO.CustomDataSetId != customerDTO.CustomDataSetDTO.CustomDataSetId)
                {
                    customerDTO.CustomDataSetId = customerDTO.CustomDataSetDTO.CustomDataSetId;
                }

                //Custom data should be created in master and published to all sited. This method is ensure that and entry is available in all sites.
                DBSynchLogService dBSynchLogService = new DBSynchLogService(executionContext, "CustomData", customerDTO.CustomDataSetDTO.Guid, customerDTO.CustomDataSetDTO.SiteId);
                dBSynchLogService.CreateRoamingDataForCustomer(sqlTransaction);

                foreach (var customDataDTO in customerDTO.CustomDataSetDTO.CustomDataDTOList)
                {
                    DBSynchLogService dBSynchLogServiceCustomData = new DBSynchLogService(executionContext, "CustomDataSet", customDataDTO.Guid, customDataDTO.SiteId);
                    dBSynchLogServiceCustomData.CreateRoamingDataForCustomer(sqlTransaction);
                }
            }

            if (customerDTO.ProfileDTO != null)
            {
                log.Debug("DEBUGCUSTOMERUSERNAME:" + customerDTO.Id + ":" + customerDTO.UserName + ":" + customerDTO.ProfileDTO.Id + ":" + customerDTO.ProfileDTO.UserName);
                string emailAddress = string.Empty;
                if (string.IsNullOrWhiteSpace(customerDTO.UserName) == false)
                {
                    emailAddress = customerDTO.UserName;
                }
                if (string.IsNullOrWhiteSpace(emailAddress) &&
                    customerDTO.ContactDTOList != null &&
                    customerDTO.ContactDTOList.Any(x => x.ContactType == ContactType.EMAIL))
                {
                    ContactDTO emailContactDTO = customerDTO.ContactDTOList.Where(x => x.ContactType == ContactType.EMAIL && x.IsActive).OrderByDescending(x => x.LastUpdateDate).FirstOrDefault();
                    if (emailContactDTO != null)
                    {
                        emailAddress = emailContactDTO.Attribute1;
                    }
                }

                if (string.IsNullOrWhiteSpace(customerDTO.UserName) && string.IsNullOrWhiteSpace(customerDTO.ProfileDTO.UserName)
                   && string.IsNullOrWhiteSpace(emailAddress) == false)
                {
                    customerDTO.UserName = emailAddress;
                    customerDTO.ProfileDTO.UserName = emailAddress;
                    if (IsDuplicateCustomerWithUsernameExists(sqlTransaction))
                    {
                        customerDTO.UserName = "";
                        customerDTO.ProfileDTO.UserName = "";
                    }
                    else
                    {
                        try
                        {
                            if (!IsAdult())
                            {
                                customerDTO.UserName = "";
                                customerDTO.ProfileDTO.UserName = "";
                            }
                        }
                        catch
                        {
                            //If DOB is not present, IsAdult Function throws error
                            //Catch that error here and proceed
                            //Assume customer is an Adult
                        }
                    }
                }

                ProfileBL profileBL = new ProfileBL(executionContext, customerDTO.ProfileDTO);
                profileBL.Save(sqlTransaction);
                if (customerDTO.ProfileId != customerDTO.ProfileDTO.Id)
                {
                    customerDTO.ProfileId = customerDTO.ProfileDTO.Id;
                }
            }
            CustomerDataHandler customerDataHandler = new CustomerDataHandler(passPhrase, sqlTransaction);
            if (customerDTO.Id < 0)
            {
                customerDTO = customerDataHandler.InsertCustomer(customerDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                customerDTO.AcceptChanges();
                CreateRoamingData(sqlTransaction);
                newCustomer = true;
            }
            else
            {
                if (customerDTO.IsChanged)
                {
                    customerDTO = customerDataHandler.UpdateCustomer(customerDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    customerDTO.AcceptChanges();
                    CreateRoamingData(sqlTransaction);
                }
            }
            if (customerDTO.Verified == false &&
               customerDTO.CustomerVerificationDTO != null)
            {
                if (customerDTO.CustomerVerificationDTO.CustomerId == -1)
                {
                    customerDTO.CustomerVerificationDTO.CustomerId = customerDTO.Id;
                    CustomerVerificationBL customerVerificationBL = new CustomerVerificationBL(executionContext, customerDTO.CustomerVerificationDTO);
                    customerVerificationBL.Save(sqlTransaction);

                    customerDTO.Verified = true;
                    customerDataHandler.UpdateCustomer(customerDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    customerDTO.AcceptChanges();

                    // Send registration email and SMS after the user verifies
                    if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "SEND_CUSTOMER_REGISTRATION_EMAIL"))
                    {
                        //SendRegistrationMail(sqlTransaction);
                        SendRegistrationMessage(MessagingClientDTO.MessagingChanelType.EMAIL, sqlTransaction);
                    }

                    if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "SEND_CUSTOMER_REGISTRATION_SMS"))
                    {
                        //SendRegistrationSMS(sqlTransaction);
                        SendRegistrationMessage(MessagingClientDTO.MessagingChanelType.SMS, sqlTransaction);
                    }
                }
            }
            if (customerDTO.CustomerMembershipProgressionDTOList != null && customerDTO.CustomerMembershipProgressionDTOList.Count > 0)
            {
                foreach (CustomerMembershipProgressionDTO customerMembershipProgressionDTO in customerDTO.CustomerMembershipProgressionDTOList)
                {
                    if (customerMembershipProgressionDTO.Id < 0 || customerMembershipProgressionDTO.IsChanged)
                    {
                        CustomerMembershipProgression customerMembershipProgression = new CustomerMembershipProgression(executionContext, customerMembershipProgressionDTO);
                        customerMembershipProgression.Save(customerDTO.SiteId, sqlTransaction);
                    }
                }
            }
            Dictionary<string, AccountCreditPlusDTO> mapCreditPLusDTO = new Dictionary<string, AccountCreditPlusDTO>();
            if (this.customerDTO.AccountDTOList != null && customerDTO.AccountDTOList.Any())
            {
                foreach (AccountDTO accountDTO in customerDTO.AccountDTOList)
                {
                    if (accountDTO.AccountCreditPlusDTOList != null && accountDTO.AccountCreditPlusDTOList.Any())
                    {
                        foreach (AccountCreditPlusDTO accountCreditPlusDTO in accountDTO.AccountCreditPlusDTOList)
                        {
                            mapCreditPLusDTO.Add(accountCreditPlusDTO.Guid, accountCreditPlusDTO);
                        }
                    }
                    if (accountDTO.AccountId < 0 || accountDTO.IsChangedRecursive)
                    {
                        AccountBL accountBL = new AccountBL(executionContext, accountDTO);
                        accountBL.Save(sqlTransaction);
                    } 
                }
            }
            if (customerDTO.CustomerMembershipRewardsLogDTOList != null && customerDTO.CustomerMembershipRewardsLogDTOList.Count > 0)
            {
                foreach (CustomerMembershipRewardsLogDTO customerMembershipRewardsLogDTO in customerDTO.CustomerMembershipRewardsLogDTOList)
                {
                    if (customerMembershipRewardsLogDTO.MembershipRewardsLogId < 0 || customerMembershipRewardsLogDTO.IsChanged)
                    {
                        if (customerMembershipRewardsLogDTO.MembershipRewardsLogId == -1 && !String.IsNullOrEmpty(customerMembershipRewardsLogDTO.CardCreditPlusTag))
                            customerMembershipRewardsLogDTO.CardCreditPlusId = mapCreditPLusDTO[customerMembershipRewardsLogDTO.CardCreditPlusTag].AccountCreditPlusId;
                        CustomerMembershipRewardsLogBL customerMembershipRewardsLogBL = new CustomerMembershipRewardsLogBL(executionContext, customerMembershipRewardsLogDTO);
                        customerMembershipRewardsLogBL.Save(sqlTransaction);
                    }
                }
            }

            if (this.customerDTO.CustomerSignedWaiverDTOList != null && this.customerDTO.CustomerSignedWaiverDTOList.Any())
            {
                for (int i = 0; i < this.customerDTO.CustomerSignedWaiverDTOList.Count; i++)
                {
                    if (this.customerDTO.CustomerSignedWaiverDTOList[i].IsChangedRecursive)
                    {
                        CustomerSignedWaiverBL customerSignedWaiverBL = new CustomerSignedWaiverBL(executionContext, this.customerDTO.CustomerSignedWaiverDTOList[i]);
                        customerSignedWaiverBL.Save(sqlTransaction);
                    }
                }
            }

            if (isCustomerBackwardCompatibilityChanged && Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "ENABLE_CUSTOMER_BACKWARD_COMPATIBILITY"))
            {
                AddressDTO addressDTO = customerDTO.LatestAddressDTO;
                if (addressDTO != null)
                {
                    if (!string.IsNullOrWhiteSpace(addressDTO.Line1) && addressDTO.Line1.Length > 50)
                        addressDTO.Line1 = addressDTO.Line1.Substring(0, 50);

                    if (!string.IsNullOrWhiteSpace(addressDTO.Line2) && addressDTO.Line2.Length > 50)
                        addressDTO.Line2 = addressDTO.Line2.Substring(0, 50);

                    if (!string.IsNullOrWhiteSpace(addressDTO.Line3) && addressDTO.Line3.Length > 50)
                        addressDTO.Line3 = addressDTO.Line3.Substring(0, 50);
                }

                CustomerBackwardCompatibityDataHandler customerBackwardCompatibityDataHandler = new CustomerBackwardCompatibityDataHandler(sqlTransaction);
                customerBackwardCompatibityDataHandler.UpdateCustomer(customerDTO, executionContext.GetUserId(), executionContext.GetSiteId());
            }

            if (string.IsNullOrWhiteSpace(customerDTO.Password) == false &&
               customerDTO.Id >= 0 &&
               customerDTO.ProfileDTO != null)
            {
                ProfileBL profileBL = new ProfileBL(executionContext, customerDTO.ProfileDTO);
                profileBL.UpdatePassword(customerDTO.Password, sqlTransaction);
                customerDTO.Password = string.Empty;
            }

            // nitin - check this logic
            // Send a mail to the customer based on the account activation email configuration.
            if (newCustomer && ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "ENABLE_ACCOUNT_ACTIVATION_EMAIL"))
            {
                // change this to use the existing DTO
                SendRegistrationLink(sqlTransaction);
            }

            if (newCustomer && ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "SEND_CUSTOMER_REGISTRATION_EMAIL")
                && !ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "ENABLE_ACCOUNT_ACTIVATION_EMAIL"))
            {
                //SendRegistrationMail(sqlTransaction);
                SendRegistrationMessage(MessagingClientDTO.MessagingChanelType.EMAIL, sqlTransaction);
            }

            if (newCustomer && ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "SEND_CUSTOMER_REGISTRATION_SMS")
                && !ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "ENABLE_ACCOUNT_ACTIVATION_EMAIL"))
            {
                //SendRegistrationSMS(sqlTransaction);
                SendRegistrationMessage(MessagingClientDTO.MessagingChanelType.SMS, sqlTransaction);
            }
            log.LogMethodExit();
        }


        //private void SendRegistrationMail(SqlTransaction sqlTransaction)
        //{
        //    log.LogMethodEntry();
        //    string emailAddress = string.Empty;
        //    int messagingClientId = -1;
        //    int lookUpValueId = -1;
        //    if (string.IsNullOrWhiteSpace(customerDTO.UserName) == false)
        //    {
        //        emailAddress = customerDTO.UserName;
        //    }
        //    if (string.IsNullOrWhiteSpace(emailAddress) &&
        //        customerDTO.ContactDTOList != null &&
        //        customerDTO.ContactDTOList.Any(x => x.ContactType == ContactType.EMAIL))
        //    {
        //        ContactDTO emailContactDTO = customerDTO.ContactDTOList.Where(x => x.ContactType == ContactType.EMAIL).OrderByDescending(x => x.LastUpdateDate).FirstOrDefault();
        //        if (emailContactDTO != null)
        //        {
        //            emailAddress = emailContactDTO.Attribute1;
        //        }
        //    }
        //    if (string.IsNullOrWhiteSpace(emailAddress))
        //    {
        //        log.LogMethodExit(null, "No Valid Email found to send the registartion mail");
        //        return;
        //    }
        //    EmailTemplateDTO emailTemplateDTO = null;
        //    try
        //    {
        //        emailTemplateDTO = new EmailTemplate(executionContext).GetEmailTemplate(Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_REGISTRATION_EMAIL_TEMPLATE"), executionContext.GetSiteId());
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("Error occured while retrieving the customer registation template", ex);
        //    }
        //    if (emailTemplateDTO == null)
        //    {
        //        string errorMessage = MessageContainerList.GetMessage(executionContext, 2194);
        //        log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
        //        throw new Exception(errorMessage);
        //    }
        //    string emailContent = emailTemplateDTO.EmailTemplate;
        //    emailContent = emailContent.Replace("@customerName", customerDTO.FirstName + (string.IsNullOrWhiteSpace(customerDTO.LastName) ? "" : " " + customerDTO.LastName));
        //    SiteList siteList = new SiteList(executionContext);
        //    List<SiteDTO> siteDTOList = siteList.GetAllSites(new List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>());
        //    string siteName = string.Empty;
        //    if (siteDTOList != null)
        //    {
        //        SiteDTO siteDTO = null;
        //        if (executionContext.GetSiteId() == -1)
        //        {
        //            siteDTO = siteDTOList.FirstOrDefault();
        //        }
        //        else
        //        {
        //            siteDTO = siteDTOList.Where(x => x.SiteId == executionContext.GetSiteId()).FirstOrDefault();
        //        }
        //        if (siteDTO != null)
        //        {
        //            siteName = siteDTO.SiteName;
        //        }
        //    }
        //    emailContent = emailContent.Replace("@siteName", siteName);
        //    try
        //    {
        //        MessagingClientFunctionLookUpListBL messagingClientFunctionLookUpListBL = new MessagingClientFunctionLookUpListBL(executionContext);
        //        List<MessagingClientFunctionLookUpDTO> messagingClientFunctionLookUpDTO  = messagingClientFunctionLookUpListBL.GetMessagingClientDTOListByFunctionName("CUSTOMER_FUNCTIONS_LOOKUP", "NEW_REGISTRATION", "E");
        //        if(messagingClientFunctionLookUpDTO != null && messagingClientFunctionLookUpDTO.Any())
        //            messagingClientId = messagingClientFunctionLookUpDTO[0].MessagingClientDTO.ClientId;

        //        //"Customer Welcome Email"
        //        MessagingRequestDTO messagingRequestDTO = new MessagingRequestDTO(-1, null, "New Customer Registration", "E", customerDTO.Email, "", "", null, null, null, null,
        //              emailTemplateDTO.Description, emailContent, customerDTO.Id, null, "", true, "", "", messagingClientId, false, "");
        //        MessagingRequestBL messagingRequestBL = new MessagingRequestBL(executionContext, messagingRequestDTO);
        //        messagingRequestBL.Save(sqlTransaction);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("Error occured while sending registration e-mail", ex);
        //        Semnox.Parafait.logger.EventLog.Log(executionContext, "Customer Registration", "D", string.Empty, "Unable to save message request for registration mail to customer with id " + customerDTO.Id, string.Empty, string.Empty, 3, string.Empty, string.Empty);
        //    }
        //    log.LogMethodExit();
        //}

        //private void SendRegistrationSMS(SqlTransaction sqlTransaction)
        //{
        //    log.LogMethodEntry();
        //    string phoneNumber = string.Empty;

        //    if (string.IsNullOrWhiteSpace(customerDTO.PhoneNumber) == false)
        //    {
        //        phoneNumber = customerDTO.UserName;
        //    }
        //    if (string.IsNullOrWhiteSpace(phoneNumber) &&
        //        customerDTO.ContactDTOList != null &&
        //        customerDTO.ContactDTOList.Any(x => x.ContactType == ContactType.PHONE))
        //    {
        //        ContactDTO phoneContactDTO = customerDTO.ContactDTOList.Where(x => x.ContactType == ContactType.PHONE).OrderByDescending(x => x.LastUpdateDate).FirstOrDefault();
        //        if (phoneContactDTO != null)
        //        {
        //            phoneNumber = phoneContactDTO.Attribute1;
        //        }
        //    }
        //    if (string.IsNullOrWhiteSpace(phoneNumber))
        //    {
        //        log.LogMethodExit(null, "No Valid phone number found to send the registartion SMS");
        //        return;
        //    }
        //    EmailTemplateDTO emailTemplateDTO = null;
        //    try
        //    {
        //        emailTemplateDTO = new EmailTemplate(executionContext).GetEmailTemplate(Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_REGISTRATION_SMS_TEMPLATE"), executionContext.GetSiteId());
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("Error occured while retrieving the customer registation template", ex);
        //    }
        //    if (emailTemplateDTO == null)
        //    {
        //        string errorMessage = MessageContainerList.GetMessage(executionContext, 2194);
        //        log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
        //        throw new Exception(errorMessage);
        //    }
        //    string content = emailTemplateDTO.EmailTemplate;
        //    content = content.Replace("@CustomerName", customerDTO.FirstName + (string.IsNullOrWhiteSpace(customerDTO.LastName) ? "" : " " + customerDTO.LastName));
        //    SiteList siteList = new SiteList(executionContext);
        //    List<SiteDTO> siteDTOList = siteList.GetAllSites(new List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>());
        //    string siteName = string.Empty;
        //    if (siteDTOList != null)
        //    {
        //        SiteDTO siteDTO = null;
        //        if (executionContext.GetSiteId() == -1)
        //        {
        //            siteDTO = siteDTOList.FirstOrDefault();
        //        }
        //        else
        //        {
        //            siteDTO = siteDTOList.Where(x => x.SiteId == executionContext.GetSiteId()).FirstOrDefault();
        //        }
        //        if (siteDTO != null)
        //        {
        //            siteName = siteDTO.SiteName;
        //        }
        //    }
        //    content = content.Replace("@siteName", siteName);
        //    try
        //    {
        //        string SMSGateway = ParafaitDefaultContainerList.GetParafaitDefault(this.executionContext, "SMS_GATEWAY");

        //        try
        //        {
        //            if (!string.IsNullOrEmpty(customerDTO.PhoneNumber) && (SMSGateway != MessagingClientFactory.MessagingClients.None.ToString()))
        //            {
        //                int messagingClientId = -1;
        //                MessagingClientFunctionLookUpListBL messagingClientFunctionLookUpListBL = new MessagingClientFunctionLookUpListBL(executionContext);
        //                List<MessagingClientFunctionLookUpDTO> messagingClientFunctionLookUpDTO = messagingClientFunctionLookUpListBL.GetMessagingClientDTOListByFunctionName("CUSTOMER_FUNCTIONS_LOOKUP", "NEW_REGISTRATION", "S");
        //                if (messagingClientFunctionLookUpDTO != null && messagingClientFunctionLookUpDTO.Any())
        //                    messagingClientId = messagingClientFunctionLookUpDTO[0].MessagingClientDTO.ClientId;

        //                MessagingRequestDTO messagingRequestDTO = new MessagingRequestDTO(-1, null, "", "S", "", customerDTO.PhoneNumber, "", "", null, null, null,
        //                    emailTemplateDTO.Description, content, customerDTO.Id, null, "", true, "", "", messagingClientId, false, "");
        //                MessagingRequestBL messagingRequestBL = new MessagingRequestBL(executionContext, messagingRequestDTO);
        //                try
        //                {
        //                    messagingRequestBL.Save(sqlTransaction);
        //                }
        //                catch (Exception ex)
        //                {
        //                    log.Debug("Send sms failed " + ex.Message);
        //                }

        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            log.Debug("Send Customer Verification Code Failed" + ex.Message);
        //            throw new Exception("Send Customer Verification Code Failed");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("Error occured while sending registration e-mail", ex);
        //        Semnox.Parafait.logger.EventLog.Log(executionContext, "Customer Registration", "D", string.Empty, "Unable to save message request for registration mail to customer with id " + customerDTO.Id, string.Empty, string.Empty, 3, string.Empty, string.Empty);
        //    }
        //    log.LogMethodExit();
        //}

        /// <summary>
        ///  Send Customer Registration Link
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="loginId"></param>
        /// <param name="macAddress"></param>
        public void SendRegistrationLink(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            if (CustomerHasEmailId() == false)
            {
                log.LogMethodExit(null, "No Valid Email found to send the registartion link mail");
                return;
            }
            try
            {
                // generate a verification record
                // send the registration link. the registration link is the customer guid token 
                CustomerVerificationBL customerVerificationBL = new CustomerVerificationBL(executionContext);
                customerVerificationBL.GenerateVerificationRecord(this.customerDTO.Id, "-1", Environment.MachineName, sendVerificationCode: false, sqlTransaction: sqlTransaction);

                if (customerVerificationBL.CustomerVerificationDTO.Id != -1)
                {
                    if (!customerDTO.Verified)
                    {
                        CustomerEventsBL customerEventsBL = new CustomerEventsBL(executionContext, ParafaitFunctionEvents.REGISTRATION_LINK_EVENT, customerDTO, sqlTransaction);
                        customerEventsBL.SendMessage(MessagingClientDTO.MessagingChanelType.NONE, sqlTransaction);

                        //EmailTemplateDTO emailTemplateDTO = new EmailTemplate(executionContext).GetEmailTemplate("ONLINE_CUSTOMER_ACTIVATION_EMAIL_TEMPLATE", customerDTO.SiteId);
                        //string emailContent = string.Empty;
                        //if (emailTemplateDTO != null && emailTemplateDTO.EmailTemplateId > 0)
                        //{
                        //    emailContent = emailTemplateDTO.EmailTemplate;
                        //    SecurityTokenBL securityTokenBL = new SecurityTokenBL(executionContext);
                        //    securityTokenBL.GenerateToken(customerDTO.Guid, "Customer");
                        //    emailContent = emailContent.Replace("@BaseUrl", ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ONLINE_WEBSITE_URL"));
                        //    emailContent = emailContent.Replace("@accountActivationLink", ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ACCOUNT_ACTIVATION_URL"));
                        //    emailContent = emailContent.Replace("@accountActivationToken", securityTokenBL.GetSecurityTokenDTO.Token);
                        //}
                        //else
                        //{
                        //    log.Error(MessageContainerList.GetMessage(executionContext, 1947));
                        //    throw new Exception(MessageContainerList.GetMessage(executionContext, 1947));
                        //}

                        //int messagingClientId = -1;
                        //MessagingClientFunctionLookUpListBL messagingClientFunctionLookUpListBL = new MessagingClientFunctionLookUpListBL(executionContext);
                        //List<MessagingClientFunctionLookUpDTO> messagingClientFunctionLookUpDTO = messagingClientFunctionLookUpListBL.GetMessagingClientDTOListByFunctionName("CUSTOMER_FUNCTIONS_LOOKUP", "REGISTRATION_LINK", "E");
                        //if (messagingClientFunctionLookUpDTO != null && messagingClientFunctionLookUpDTO.Any())
                        //    messagingClientId = messagingClientFunctionLookUpDTO[0].MessagingClientDTO.ClientId;
                        ////"Customer Welcome Email"
                        //MessagingRequestDTO messagingRequestDTO = new MessagingRequestDTO(-1, null, "Customer Registration Verification", "E", emailAddress, "", "", null, null, null, null,
                        //      emailTemplateDTO.Description, emailContent, customerDTO.Id, null, "", true, "", "", messagingClientId, false, "");
                        //MessagingRequestBL messagingRequestBL = new MessagingRequestBL(executionContext, messagingRequestDTO);
                        //messagingRequestBL.Save(sqlTransaction);
                    }
                    else
                    {
                        throw new Exception(MessageContainerList.GetMessage(executionContext, 1944));
                    }
                }
                else
                {
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 1945));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        public void InactivateCustomer(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);

            try
            {
                // Remove membership and expire rewards and other details
                if (customerDTO.MembershipId > -1)
                {
                    // Nitin - Should the loyalty rewards expire if the profile is deleted. Still under discussion, so commented
                    //CustomerRewards customerRewardsBL = new CustomerRewards(executionContext, this.customerDTO);
                    //customerRewardsBL.ExpireOldRewards(sqlTransaction);
                    //CustomerMembershipProgression membershipProgression = new CustomerMembershipProgression(executionContext, this.customerDTO);
                    //membershipProgression.ExpireMembershipProgressionEntry();
                    this.customerDTO.MembershipId = -1;
                }

                // Do not delink customers from waiver. This is required for legal purposes

                // Remove custom cata
                customerDTO.CustomDataSetDTO = null;
                customerDTO.CustomDataSetId = -1;
                customerDTO.IsActive = false;
                customerDTO.ProfileDTO.IsActive = false;

                if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "TITLE") == "M")
                {
                    customerDTO.ProfileDTO.Title = "Mr.";
                }
                else
                {
                    customerDTO.ProfileDTO.Title = null;
                }
                if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DESIGNATION") == "M")
                {
                    customerDTO.ProfileDTO.Designation = "Designation";
                }
                else
                {
                    customerDTO.ProfileDTO.Designation = null;
                }
                if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "EXTERNALSYSTEMREF") == "M")
                {
                    customerDTO.ExternalSystemReference = "1234";
                }
                else
                {
                    customerDTO.ExternalSystemReference = "";
                }
                if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "LAST_NAME") == "M")
                {
                    customerDTO.LastName = "Last Name";
                }
                else
                {
                    customerDTO.LastName = "";
                }

                if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CHANNEL") == "M")
                {
                    customerDTO.Channel = "Channel";
                }
                else
                {
                    customerDTO.Channel = "";
                }
                if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "TAXCODE") == "M")
                {
                    customerDTO.TaxCode = "TaxCode";
                }
                else
                {
                    customerDTO.TaxCode = null;
                }
                if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_PHOTO") == "M")
                {
                    customerDTO.PhotoURL = "";
                }
                else
                {
                    customerDTO.PhotoURL = "";
                }
                if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_NAME") == "M")
                {
                    customerDTO.FirstName = "FirstName";
                }
                else
                {
                    customerDTO.FirstName = "";
                }
                if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "MIDDLE_NAME") == "M")
                {
                    customerDTO.MiddleName = "MiddleName";
                }
                else
                {
                    customerDTO.MiddleName = "";
                }

                if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "FBUSERID") == "M")
                {
                    customerDTO.FBUserId = "FBUserId";
                }
                else
                {
                    customerDTO.FBUserId = "";
                }
                if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "FBACCESSTOKEN") == "M")
                {
                    customerDTO.FBAccessToken = "FBACCESSTOKEN";
                }
                else
                {
                    customerDTO.FBAccessToken = "";
                }
                if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "TWACCESSTOKEN") == "M")
                {
                    customerDTO.TWAccessToken = "TWACCESSTOKEN";
                }
                else
                {
                    customerDTO.TWAccessToken = "";
                }

                if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "TWACCESSSECRET") == "M")
                {
                    customerDTO.TWAccessSecret = "TWACCESSSECRET";
                }
                else
                {
                    customerDTO.TWAccessSecret = "";
                }
                if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "WECHAT_ACCESS_TOKEN") == "M")
                {
                    customerDTO.WeChatAccessToken = "WeChatAccessToken";
                }
                else
                {
                    customerDTO.WeChatAccessToken = "";
                }

                if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_NAME") == "M")
                {
                    customerDTO.ProfileDTO.FirstName = "FirstName";
                }
                else
                {
                    customerDTO.ProfileDTO.FirstName = "";
                }
                if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "MIDDLE_NAME") == "M")
                {
                    customerDTO.ProfileDTO.MiddleName = "MiddleName";
                }
                else
                {
                    customerDTO.ProfileDTO.MiddleName = "";
                }

                if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "LAST_NAME") == "M")
                {
                    customerDTO.ProfileDTO.LastName = "Last Name";
                }
                else
                {
                    customerDTO.ProfileDTO.LastName = "";
                }

                if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "BIRTH_DATE") == "M")
                {
                    customerDTO.ProfileDTO.DateOfBirth = new DateTime(1753, 01, 01);
                }
                else
                {
                    customerDTO.ProfileDTO.DateOfBirth = null;
                }
                if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "GENDER") == "M")
                {
                    customerDTO.ProfileDTO.Gender = "N";
                }
                else
                {
                    customerDTO.ProfileDTO.Gender = null;
                }
                if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ANNIVERSARY") == "M")
                {
                    customerDTO.ProfileDTO.Anniversary = new DateTime(1753, 01, 01);
                }
                else
                {
                    customerDTO.ProfileDTO.Anniversary = null;
                }
                if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "NOTES") == "M")
                {
                    customerDTO.ProfileDTO.Notes = "Notes";
                }
                else
                {
                    customerDTO.ProfileDTO.Notes = null;
                }
                if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "COMPANY") == "M")
                {
                    customerDTO.ProfileDTO.Company = "Company";
                }
                else
                {
                    customerDTO.ProfileDTO.Company = null;
                }
                if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "UNIQUE_ID") == "M")
                {
                    customerDTO.ProfileDTO.UniqueIdentifier = Guid.NewGuid().ToString();
                }
                else
                {
                    customerDTO.ProfileDTO.UniqueIdentifier = null;
                }
                if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "USERNAME") == "M")
                {
                    customerDTO.ProfileDTO.UserName = "UserName";
                }
                else
                {
                    customerDTO.ProfileDTO.UserName = null;
                }

                if (customerDTO.ProfileDTO.AddressDTOList != null)
                {
                    foreach (AddressDTO addressDTO in customerDTO.ProfileDTO.AddressDTOList)
                    {
                        if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CITY") == "M")
                        {
                            addressDTO.City = "City";
                        }
                        else
                        {
                            addressDTO.City = "";
                        }
                        List<StateDTO> stateDTOs = new List<StateDTO>();
                        StateList stateList = new StateList();
                        StateParams stateParams = new StateParams();
                        stateDTOs = stateList.GetStateList(stateParams);
                        if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "STATE") == "M")
                        {
                            addressDTO.StateName = stateDTOs[0].Description;
                            addressDTO.StateId = stateDTOs[0].StateId;
                            addressDTO.StateCode = stateDTOs[0].State;
                        }
                        else
                        {
                            addressDTO.StateName = "";
                            addressDTO.StateId = -1;
                            addressDTO.StateCode = "";
                        }
                        if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "COUNTRY") == "M")
                        {
                            CountryBL countryBL = new CountryBL(stateDTOs[0].CountryId);
                            addressDTO.CountryId = stateDTOs[0].CountryId;
                            addressDTO.CountryName = countryBL.GetCountryDTO.CountryName;
                        }
                        else
                        {
                            addressDTO.CountryId = -1;
                            addressDTO.CountryName = "";
                        }
                        if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "PIN") == "M")
                        {
                            addressDTO.PostalCode = "123456";
                        }
                        else
                        {
                            addressDTO.PostalCode = "";
                        }
                        if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ADDRESS1") == "M")
                        {
                            addressDTO.Line1 = "Address1";
                        }
                        else
                        {
                            addressDTO.Line1 = "";
                        }
                        if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ADDRESS2") == "M")
                        {
                            addressDTO.Line2 = "Address2";
                        }
                        else
                        {
                            addressDTO.Line2 = "";
                        }
                        if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ADDRESS3") == "M")
                        {
                            addressDTO.Line3 = "Address3";
                        }
                        else
                        {
                            addressDTO.Line3 = "";
                        }

                        // Do not inactivate address as this will cause backward compatability to fail
                        //addressDTO.IsActive = false;

                        if (addressDTO.ContactDTOList != null && addressDTO.ContactDTOList.Count > 0)
                        {
                            foreach (ContactDTO contactDTO in addressDTO.ContactDTOList)
                            {
                                if (contactDTO.ContactType == ContactType.EMAIL && !string.IsNullOrEmpty(contactDTO.Attribute1))
                                {
                                    contactDTO.Attribute1 = contactDTO.Guid + "@deleted.com";
                                    //contactDTO.IsActive = false;
                                }
                                else if (contactDTO.ContactType == ContactType.PHONE && !string.IsNullOrEmpty(contactDTO.Attribute1))
                                {
                                    contactDTO.Attribute1 = contactDTO.Guid;
                                    //contactDTO.IsActive = false;
                                }
                                // Other contact types like FBtoken etc are being handled separately, do not modify them                                
                            }
                        }
                    }
                }

                if (customerDTO.ProfileDTO.ContactDTOList != null)
                {
                    foreach (ContactDTO contactDTO in customerDTO.ProfileDTO.ContactDTOList)
                    {
                        if (contactDTO.ContactType == ContactType.EMAIL && !string.IsNullOrEmpty(contactDTO.Attribute1))
                        {
                            contactDTO.Attribute1 = contactDTO.Guid + "@deleted.com";
                            //contactDTO.IsActive = false;
                        }
                        else if (contactDTO.ContactType == ContactType.PHONE && !string.IsNullOrEmpty(contactDTO.Attribute1))
                        {
                            contactDTO.Attribute1 = contactDTO.Guid;
                            //contactDTO.IsActive = false;
                        }
                        // Other contact types like FBtoken etc are being handled separately, do not modify them
                    }
                }

                if (customerDTO.ProfileDTO.ProfileContentHistoryDTOList != null)
                {
                    foreach (var profileContentHistoryDTO in customerDTO.ProfileDTO.ProfileContentHistoryDTOList)
                    {
                        profileContentHistoryDTO.IsActive = false;
                    }
                }

                SaveCustomer(sqlTransaction);

                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw ex;
            }

            log.LogMethodExit();
        }
        private List<ValidationError> CheckDataAccessPermission(int membereshipId)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            Users user = new Users(executionContext, executionContext.GetUserId(), executionContext.SiteId);
            if (user.UserDTO != null)
            {
                UserRoles userRoles = new UserRoles(executionContext, user.UserDTO.RoleId);
                if (userRoles.getUserRolesDTO != null)
                {
                    DataAccessRule dataAccessRule = new DataAccessRule(executionContext, userRoles.getUserRolesDTO.DataAccessRuleId);
                    string membershipGuid = string.Empty;
                    if (membereshipId > -1 && customerDTO.MembershipId > -1)
                    {
                        MembershipBL membershipBL = new MembershipBL(executionContext, customerDTO.MembershipId);
                        membershipGuid = membershipBL.getMembershipDTO.Guid;
                    }

                    if (dataAccessRule.IsEditable("Membership", membershipGuid) == false)
                    {
                        ValidationError validationError = new ValidationError("Customer", "", MessageContainerList.GetMessage(executionContext, 1465));
                        validationErrorList.Add(validationError);
                    }
                }
            }

            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        private void CreateRoamingData(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            CustomerDataHandler customerDataHandler = new CustomerDataHandler(passPhrase, sqlTransaction);
            CustomerDTO updatedCustomerDTO = customerDataHandler.GetCustomerDTO(customerDTO.Id);
            DBSynchLogService dBSynchLogService = new DBSynchLogService(executionContext, "customers", updatedCustomerDTO.Guid, updatedCustomerDTO.SiteId);
            dBSynchLogService.CreateRoamingDataForCustomer(sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// CreateRoamingDataForCustomer
        /// </summary>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public void CreateRoamingDataForCustomer(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);

            if (customerDTO.CustomDataSetDTO != null &&
               customerDTO.CustomDataSetDTO.CustomDataDTOList != null &&
               customerDTO.CustomDataSetDTO.CustomDataDTOList.Count > 0)
            {
                //Custom data should be created in master and published to all sited. This method is ensure that and entry is available in all sites.
                DBSynchLogService dBSynchLogService = new DBSynchLogService(executionContext, "CustomData", customerDTO.CustomDataSetDTO.Guid, customerDTO.CustomDataSetDTO.SiteId);
                dBSynchLogService.CreateRoamingDataForCustomer(sqlTransaction);

                foreach (var customDataDTO in customerDTO.CustomDataSetDTO.CustomDataDTOList)
                {
                    DBSynchLogService dBSynchLogServiceCustomData = new DBSynchLogService(executionContext, "CustomDataSet", customDataDTO.Guid, customDataDTO.SiteId);
                    dBSynchLogServiceCustomData.CreateRoamingDataForCustomer(sqlTransaction);
                }
            }

            if (this.CustomerDTO.ProfileDTO != null)
            {
                ProfileBL profileBL = new ProfileBL(this.executionContext, this.CustomerDTO.ProfileDTO);
                profileBL.CreateRoamingData(sqlTransaction);

                if (this.CustomerDTO.ProfileDTO.ContactDTOList != null)
                {
                    foreach (var contactDTO in this.CustomerDTO.ProfileDTO.ContactDTOList)
                    {
                        ContactBL contactBL = new ContactBL(executionContext, contactDTO);
                        contactBL.CreateRoamingData(sqlTransaction);
                    }
                }

                if (this.CustomerDTO.ProfileDTO.AddressDTOList != null)
                {
                    foreach (var addressDTO in this.CustomerDTO.ProfileDTO.AddressDTOList)
                    {
                        AddressBL addressBL = new AddressBL(executionContext, addressDTO);
                        addressBL.CreateRoamingData(sqlTransaction);
                    }
                }
            }

            CreateRoamingData(sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// BuildCustomerSearchParametersList
        /// </summary>
        /// <param name="customerParams"></param>
        /// <returns></returns>
        public CustomerSearchCriteria BuildCustomerSearchParametersList(CustomerParams customerParams)
        {
            log.LogMethodEntry(customerParams);

            CustomerSearchCriteria customerSearchCriteria = new CustomerSearchCriteria();

            if (customerParams != null)
            {
                if (!string.IsNullOrEmpty(customerParams.FirstName))
                    customerSearchCriteria.And(CustomerSearchByParameters.PROFILE_FIRST_NAME, Operator.EQUAL_TO, customerParams.FirstName);
                if (!string.IsNullOrEmpty(customerParams.MiddleName))
                    customerSearchCriteria.And(CustomerSearchByParameters.PROFILE_MIDDLE_NAME, Operator.EQUAL_TO, customerParams.MiddleName);
                if (!string.IsNullOrEmpty(customerParams.LastName))
                    customerSearchCriteria.And(CustomerSearchByParameters.PROFILE_LAST_NAME, Operator.EQUAL_TO, customerParams.LastName);
                if (!string.IsNullOrEmpty(customerParams.Email))
                {
                    customerSearchCriteria.And(CustomerSearchByParameters.CONTACT_CONTACT_TYPE, Operator.EQUAL_TO, ContactType.EMAIL.ToString())
                    .And(CustomerSearchByParameters.CONTACT_ATTRIBUTE1, Operator.EQUAL_TO, customerParams.Email);
                }
                if (!string.IsNullOrEmpty(customerParams.Phone))
                {
                    customerSearchCriteria.And(CustomerSearchByParameters.CONTACT_CONTACT_TYPE, Operator.EQUAL_TO, ContactType.PHONE.ToString())
                        .And(CustomerSearchByParameters.CONTACT_ATTRIBUTE1, Operator.EQUAL_TO, customerParams.Phone);
                }
                if (!string.IsNullOrEmpty(customerParams.UserName))
                    customerSearchCriteria.And(CustomerSearchByParameters.CONTACT_CONTACT_TYPE, Operator.EQUAL_TO, ContactType.EMAIL.ToString())
                    .And(CustomerSearchByParameters.CONTACT_ATTRIBUTE1, Operator.EQUAL_TO, customerParams.UserName)
                    .Or(CustomerSearchByParameters.PROFILE_USER_NAME, Operator.EQUAL_TO, customerParams.UserName);

                if (customerParams.CustomerIdFrom != -1 && customerParams.CustomerIdTo != -1)
                    customerSearchCriteria.And(CustomerSearchByParameters.CUSTOMER_ID, Operator.BETWEEN, customerParams.CustomerIdFrom, customerParams.CustomerIdTo);

                if (customerParams.SiteId != -1)
                    customerSearchCriteria.And(CustomerSearchByParameters.CUSTOMER_SITE_ID, Operator.EQUAL_TO, customerParams.SiteId);

            }

            log.LogMethodExit(customerSearchCriteria);

            return customerSearchCriteria;
        }

        public void ForgotPassword()
        {
            log.LogMethodEntry();
            //string emailContent = "";
            //string emailSubject = "Account Information";
            //string securityTokenLink = CreateResetPasswordLink();

            //EmailTemplateDTO emailTemplateDTO = new EmailTemplate(executionContext).GetEmailTemplate(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ONLINE_PASSWORD_RESET_EMAIL_TEMPLATE"), customerDTO.SiteId);
            //if (emailTemplateDTO != null && emailTemplateDTO.EmailTemplateId > 0)
            //{
            //    emailContent = emailTemplateDTO.EmailTemplate;
            //    emailSubject = emailTemplateDTO.Description;
            //}
            //else
            //{
            //    emailContent = MessageContainerList.GetMessage(executionContext, " Hi ") + CustomerDTO.FirstName +
            //                   Environment.NewLine + Environment.NewLine +
            //                   MessageContainerList.GetMessage(executionContext,
            //                       "<p>Password Reset Token could not be sent -  Email Template Missing </p>") +
            //                   Environment.NewLine + Environment.NewLine +
            //                   MessageContainerList.GetMessage(executionContext, "Thank you");
            //}

            //emailContent = emailContent.Replace("@passwordResetTokenLink", securityTokenLink);

            //Email email = new Email(executionContext, CustomerDTO.Email, string.Empty, CustomerDTO.Email, emailSubject,
            //    emailContent);
            //email.Send();
            CustomerEventsBL customerEventsBL = new CustomerEventsBL(executionContext, ParafaitFunctionEvents.RESET_PASSWORD_EVENT, customerDTO);
            customerEventsBL.SendMessage(MessagingClientDTO.MessagingChanelType.NONE);
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns whether customer has a valid password
        /// </summary>
        /// <returns></returns>
        public bool HasValidPassword()
        {
            log.LogMethodEntry();
            bool result = string.IsNullOrWhiteSpace(customerDTO.ProfileDTO.Password) == false;
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Authenticates the customer
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool Authenticate(string password)
        {
            ProfileBL profileBL = new ProfileBL(executionContext, customerDTO.ProfileDTO);
            bool result = profileBL.Authenticate(password);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns whether customer has a valid email
        /// </summary>
        /// <returns></returns>
        public bool HasValidEmail()
        {
            log.LogMethodEntry();
            bool result = string.IsNullOrWhiteSpace(customerDTO.Email) == false;
            log.LogMethodExit(result);
            return result;
        }






        /// <summary>
        /// Gets the DTO
        /// </summary>
        public CustomerDTO CustomerDTO
        {
            get
            {
                return customerDTO;
            }
        }



        #region Customer Validation 
        /// <summary>
        /// Validates the customer. returns validation errors if any of the field values not valid.
        /// </summary>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            ValidationError validationError = null;
            validationErrorList.AddRange(CheckDataAccessPermission(customerDTO.MembershipId));
            validationError = ValidateStringField("Customer", "FIRST_NAME", "FirstName", customerDTO.FirstName, "First Name");
            if (validationError != null)
            {
                validationErrorList.Add(validationError);
            }
            validationError = ValidateStringField("Customer", "NOTES", "Notes", customerDTO.Notes, "Notes");
            if (validationError != null)
            {
                validationErrorList.Add(validationError);
            }
            validationError = ValidateStringField("Customer", "COMPANY", "Company", customerDTO.Company, "Company");
            if (validationError != null)
            {
                validationErrorList.Add(validationError);
            }
            validationError = ValidateStringField("Customer", "DESIGNATION", "Designation", customerDTO.Designation, "Designation");
            if (validationError != null)
            {
                validationErrorList.Add(validationError);
            }
            validationError = ValidateStringField("Customer", "UNIQUE_ID", "UniqueIdentifier", customerDTO.UniqueIdentifier, "Unique ID");
            if (validationError != null)
            {
                validationErrorList.Add(validationError);
            }
            validationError = ValidateStringField("Customer", "USERNAME", "UserName", customerDTO.UserName, "Username");
            if (validationError != null)
            {
                validationErrorList.Add(validationError);
            }
            validationError = ValidateStringField("Customer", "CUSTOMER_PHOTO", "PhotoURL", customerDTO.PhotoURL, "Customer Photo");
            if (validationError != null)
            {
                validationErrorList.Add(validationError);
            }
            validationError = ValidateStringField("Customer", "EXTERNALSYSTEMREF", "ExternalSystemReference", customerDTO.ExternalSystemReference, "External System Reference");
            if (validationError != null)
            {
                validationErrorList.Add(validationError);
            }
            validationError = ValidateStringField("Customer", "LAST_NAME", "LastName", customerDTO.LastName, "Last Name");
            if (validationError != null)
            {
                validationErrorList.Add(validationError);
            }
            validationError = ValidateStringField("Customer", "MIDDLE_NAME", "MiddleName", customerDTO.MiddleName, "Middle Name");
            if (validationError != null)
            {
                validationErrorList.Add(validationError);
            }
            validationError = ValidateStringField("Customer", "CUSTOMER_NAME", "FirstName", customerDTO.FirstName, "First Name");
            if (validationError != null)
            {
                validationErrorList.Add(validationError);
            }
            validationError = ValidateStringField("Customer", "TITLE", "Title", customerDTO.Title, "Title");
            if (validationError != null)
            {
                validationErrorList.Add(validationError);
            }
            if (string.IsNullOrWhiteSpace(customerDTO.Title) == false &&
               customerDTO.Title != "Mr." &&
               customerDTO.Title != "Mrs." &&
               customerDTO.Title != "Ms.")
            {
                validationError = new ValidationError("Customer", "Title", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Title")));
                validationErrorList.Add(validationError);
            }
            validationError = ValidateStringField("Customer", "CHANNEL", "Channel", customerDTO.Channel, "Channel");
            if (validationError != null)
            {
                validationErrorList.Add(validationError);
            }
            validationError = ValidateStringField("Customer", "TAXCODE", "TaxCode", customerDTO.TaxCode, "Tax Code");
            if (validationError != null)
            {
                validationErrorList.Add(validationError);
            }
            if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "BIRTH_DATE") == "M" &&
               customerDTO.DateOfBirth == null)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 249, MessageContainerList.GetMessage(executionContext, "Birth Date"));
                validationErrorList.Add(new ValidationError("Customer", "DateOfBirth", errorMessage));
            }
            if (customerDTO.DateOfBirth != null)
            {
                LookupValuesList serverTimeObj = new LookupValuesList(executionContext);
                if (DateTime.Compare(Convert.ToDateTime(customerDTO.DateOfBirth), serverTimeObj.GetServerDateTime()) > 0)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 4822); //Birthday can't be a date in the future
                    validationErrorList.Add(new ValidationError("Customer", "DateOfBirth", errorMessage));
                }
            }
            if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ANNIVERSARY") == "M" &&
               customerDTO.Anniversary == null)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 249, MessageContainerList.GetMessage(executionContext, "Ann. Date"));
                validationErrorList.Add(new ValidationError("Customer", "Anniversary", errorMessage));
            }
            if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "GENDER") == "M" &&
               (string.IsNullOrWhiteSpace(customerDTO.Gender) || customerDTO.Gender == "N"))
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 249, MessageContainerList.GetMessage(executionContext, "Gender"));
                validationErrorList.Add(new ValidationError("Customer", "Gender", errorMessage));
            }
            if (string.IsNullOrWhiteSpace(customerDTO.Gender) == false &&
               customerDTO.Gender != "M" &&
               customerDTO.Gender != "F" &&
               customerDTO.Gender != "N")
            {
                validationError = new ValidationError("Customer", "Gender", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Gender")));
                validationErrorList.Add(validationError);
            }
            if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ADDRESS_TYPE") == "M" ||
                Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ADDRESS1") == "M" ||
               Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ADDRESS2") == "M" ||
               Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ADDRESS3") == "M" ||
               Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CITY") == "M" ||
               Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "STATE") == "M" ||
               Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "COUNTRY") == "M" ||
               Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "PIN") == "M")
            {
                if ((customerDTO.AddressDTOList == null ||
                    customerDTO.AddressDTOList.FindAll((x) => x.IsActive).Count == 0))
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 249, MessageContainerList.GetMessage(executionContext, "Address"));
                    validationErrorList.Add(new ValidationError("Address", "Line1", errorMessage));
                }
                if (customerDTO.AddressDTOList != null)
                {
                    for (int i = 0; i < customerDTO.AddressDTOList.Count; i++)
                    {
                        var addressDTO = customerDTO.AddressDTOList[i];
                        if (addressDTO.IsActive)
                        {
                            if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ADDRESS_TYPE") == "M" &&
                           addressDTO.AddressType == AddressType.NONE)
                            {
                                string errorMessage = MessageContainerList.GetMessage(executionContext, 249, MessageContainerList.GetMessage(executionContext, "Address Type"));
                                validationError = new ValidationError("Address", "AddressType", errorMessage);
                                validationError.RecordIndex = i;
                                validationErrorList.Add(validationError);
                            }
                            validationError = ValidateStringField("Address", "ADDRESS1", "Line1", addressDTO.Line1, "Line 1");
                            if (validationError != null)
                            {
                                validationError.RecordIndex = i;
                                validationErrorList.Add(validationError);
                            }
                            validationError = ValidateStringField("Address", "ADDRESS2", "Line2", addressDTO.Line2, "Line 2");
                            if (validationError != null)
                            {
                                validationError.RecordIndex = i;
                                validationErrorList.Add(validationError);
                            }
                            validationError = ValidateStringField("Address", "ADDRESS3", "Line3", addressDTO.Line3, "Line 3");
                            if (validationError != null)
                            {
                                validationError.RecordIndex = i;
                                validationErrorList.Add(validationError);
                            }
                            validationError = ValidateStringField("Address", "CITY", "City", addressDTO.City, "City");
                            if (validationError != null)
                            {
                                validationError.RecordIndex = i;
                                validationErrorList.Add(validationError);
                            }
                            validationError = ValidateStringField("Address", "PIN", "PostalCode", addressDTO.PostalCode, "Postal Code");
                            if (validationError != null)
                            {
                                validationError.RecordIndex = i;
                                validationErrorList.Add(validationError);
                            }
                            validationError = ValidateForeignKeyField("Address", "COUNTRY", "CountryId", addressDTO.CountryId, "Country");
                            if (validationError != null)
                            {
                                validationError.RecordIndex = i;
                                validationErrorList.Add(validationError);
                            }
                            validationError = ValidateForeignKeyField("Address", "STATE", "StateId", addressDTO.StateId, "State");
                            if (validationError != null)
                            {
                                validationError.RecordIndex = i;
                                validationErrorList.Add(validationError);
                            }
                        }
                    }
                }
            }
            //if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "SEND_CUSTOMER_REGISTRATION_EMAIL"))
            //{
            //    EmailTemplateDTO emailTemplateDTO = null;
            //    try
            //    {
            //        emailTemplateDTO = new EmailTemplate(executionContext).GetEmailTemplate(Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_REGISTRATION_EMAIL_TEMPLATE"), executionContext.GetSiteId());
            //    }
            //    catch (Exception ex)
            //    {
            //        log.Error("Error occurred while retrieving the customer registration template", ex);
            //    }
            //    if (emailTemplateDTO == null)
            //    {
            //        string errorMessage = MessageContainerList.GetMessage(executionContext, 2195);
            //        validationErrorList.Add(new ValidationError("Contact", "UserName", errorMessage));
            //    }
            //}

            if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_EMAIL_OR_PHONE_MANDATORY") == "Y" &&
                GetContactDTOListOfContactType(ContactType.EMAIL).Count == 0 &&
                GetContactDTOListOfContactType(ContactType.PHONE).Count == 0)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 251);
                validationErrorList.Add(new ValidationError("Contact", "Attribute1", errorMessage));
            }

            if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CONTACT_PHONE") == "M"
                && GetContactDTOListOfContactType(ContactType.PHONE).Count == 0)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 249, MessageContainerList.GetMessage(executionContext, "Phone"));
                validationErrorList.Add(new ValidationError("Contact", "Attribute1", errorMessage));
            }

            if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "EMAIL") == "M"
                && GetContactDTOListOfContactType(ContactType.EMAIL).Count == 0)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 249, MessageContainerList.GetMessage(executionContext, "E-Mail"));
                validationErrorList.Add(new ValidationError("Contact", "Attribute1", errorMessage));
            }

            if ((Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "FBUSERID") == "M")
                && GetContactDTOListOfContactType(ContactType.FACEBOOK).Count == 0)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 249, MessageContainerList.GetMessage(executionContext, "FB UserId"));
                validationErrorList.Add(new ValidationError("Contact", "Attribute1", errorMessage));
            }

            if ((Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "FBACCESSTOKEN") == "M")
                && GetContactDTOListOfContactType(ContactType.FACEBOOK).Count == 0)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 249, MessageContainerList.GetMessage(executionContext, "FB Token"));
                validationErrorList.Add(new ValidationError("Contact", "Attribute2", errorMessage));
            }

            if ((Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "TWACCESSTOKEN") == "M")
                && GetContactDTOListOfContactType(ContactType.TWITTER).Count == 0)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 249, MessageContainerList.GetMessage(executionContext, "TW Token"));
                validationErrorList.Add(new ValidationError("Contact", "Attribute1", errorMessage));
            }

            if ((Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "TWACCESSSECRET") == "M")
                && GetContactDTOListOfContactType(ContactType.TWITTER).Count == 0)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 249, MessageContainerList.GetMessage(executionContext, "TW Secret"));
                validationErrorList.Add(new ValidationError("Contact", "Attribute2", errorMessage));
            }

            if ((Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "WECHAT_ACCESS_TOKEN") == "M")
                && GetContactDTOListOfContactType(ContactType.WECHAT).Count == 0)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 249, MessageContainerList.GetMessage(executionContext, "Wechat access token"));
                validationErrorList.Add(new ValidationError("Contact", "Attribute1", errorMessage));
            }

            if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_NAME_VALIDATION") == "Y")
            {
                if (customerDTO.FirstName.Length < 3)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 820, 3);
                    validationErrorList.Add(new ValidationError("Customer", "FirstName", errorMessage));
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(customerDTO.FirstName, @"^[a-zA-Z]+$"))
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 821, 3);
                    validationErrorList.Add(new ValidationError("Customer", "FirstName", errorMessage));
                }
                if (string.IsNullOrWhiteSpace(customerDTO.LastName) == false)
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(customerDTO.LastName, @"^[a-zA-Z]+$"))
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 821, 3);
                        validationErrorList.Add(new ValidationError("Customer", "LastName", errorMessage));
                    }
                }
                if (string.IsNullOrWhiteSpace(customerDTO.MiddleName) == false)
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(customerDTO.MiddleName, @"^[a-zA-Z]+$"))
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 821, 3);
                        validationErrorList.Add(new ValidationError("Customer", "MiddleName", errorMessage));
                    }
                }
            }
            ProfileBL profileBL = new ProfileBL(executionContext, customerDTO.ProfileDTO);
            validationErrorList.AddRange(profileBL.Validate(sqlTransaction));
            if (customerDTO.CustomDataSetDTO != null &&
               customerDTO.CustomDataSetDTO.CustomDataDTOList != null &&
               customerDTO.CustomDataSetDTO.CustomDataDTOList.Count > 0)
            {
                CustomDataSetBL customDataSetBL = new CustomDataSetBL(executionContext, customerDTO.CustomDataSetDTO);
                validationErrorList.AddRange(customDataSetBL.Validate(sqlTransaction));
            }

            if (IsDuplicateCustomerExist(sqlTransaction))
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 290);
                validationErrorList.Add(new ValidationError("Customer", "UniqueIdentifier", errorMessage));
            }

            if (IsDuplicateCustomerWithUsernameExists(sqlTransaction))
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 775);
                validationErrorList.Add(new ValidationError("Customer", "UserName", errorMessage));
            }
            if (validationErrorList.Count == 0)
            {
                // Nitin - In this function add code to go against active customers only
                validationError = CheckUniqueCustomerAttribute(sqlTransaction);
                if (validationError != null)
                {
                    validationErrorList.Add(validationError);
                }
            }

            if (validationErrorList.Count == 0 &&
                    (customerDTO.DateOfBirth == null || (customerDTO.DateOfBirth != null && IsAdult())))
            {
                log.Debug("Validating for unique customer");
                validationError = IsUniqueCustomer(sqlTransaction);
                if (validationError != null)
                {
                    log.Debug("Not a unique customer");
                    validationErrorList.Add(validationError);
                }
                log.Debug("unique customer");
            }

            // Valid Waiver Unique Attribute
            if (validationErrorList.Count == 0)
            {
                if (customerDTO.Id > -1)
                {                   
                    validationError = CheckCustomerAttributeForWaiver(sqlTransaction);
                    if (validationError != null)
                    {
                        validationErrorList.Add(validationError);
                        throw new ValidationException(validationError.Message);
                    }
                }                
            }

            if (customerDTO.Id > -1 && customerDTO.CustomerType == CustomerType.UNREGISTERED)
            {
                CustomerDataHandler customerDataHandler = new CustomerDataHandler(passPhrase, sqlTransaction);
                CustomerDTO oldCustomerDTO = customerDataHandler.GetCustomerDTO(customerDTO.Id);
                if (oldCustomerDTO.CustomerType == CustomerType.REGISTERED)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 2492);
                    validationErrorList.Add(new ValidationError("Customer", "CustomerType", errorMessage));
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        private bool IsDuplicateCustomerWithUsernameExists(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            bool result = false;
            if (string.IsNullOrWhiteSpace(customerDTO.UserName))
            {
                log.LogMethodExit(false, "UserName is empty");
                return false;
            }
            CustomerSearchCriteria searchCriteria = new CustomerSearchCriteria(CustomerSearchByParameters.PROFILE_USER_NAME, Operator.EQUAL_TO, customerDTO.UserName);
            if (customerDTO.Id > -1)
            {
                searchCriteria.And(CustomerSearchByParameters.CUSTOMER_ID, Operator.NOT_EQUAL_TO, customerDTO.Id);
            }
            CustomerDataHandler customerDataHandler = new CustomerDataHandler(passPhrase, sqlTransaction);
            List<CustomerDTO> customerDTOList = customerDataHandler.GetCustomerDTOList(searchCriteria);
            if (customerDTOList != null && customerDTOList.Count > 0)
            {
                result = true;
            }
            log.LogMethodExit(result, "UserName is empty");
            return result;
        }

        private bool IsDuplicateCustomerExist(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            CustomerSearchCriteria searchCriteria = new CustomerSearchCriteria();
            if (customerDTO.ProfileId != -1)
            {
                searchCriteria.Or(CustomerSearchByParameters.CUSTOMER_PROFILE_ID, Operator.EQUAL_TO, customerDTO.ProfileId);

            }
            if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "ALLOW_DUPLICATE_UNIQUE_ID") == false)
            {
                if (string.IsNullOrWhiteSpace(customerDTO.UniqueIdentifier) == false)
                {
                    searchCriteria.Or(CustomerSearchByParameters.PROFILE_UNIQUE_IDENTIFIER, Operator.EQUAL_TO, customerDTO.UniqueIdentifier);
                }
            }
            if (searchCriteria.ContainsCondition)
            {
                searchCriteria.OrderBy(CustomerSearchByParameters.CUSTOMER_ID)
                    .Paginate(0, 10);
                CustomerDataHandler customerDataHandler = new CustomerDataHandler(passPhrase, sqlTransaction);
                List<CustomerDTO> customerDTOList = customerDataHandler.GetCustomerDTOList(searchCriteria);
                if (customerDTOList != null && customerDTOList.Count > 0)
                {
                    foreach (var customerDTOListItem in customerDTOList)
                    {
                        if ((customerDTOListItem.Id != customerDTO.Id))
                        {
                            log.LogMethodExit(true);
                            return true;
                        }
                    }
                }
            }
            log.LogMethodExit(false);
            return false;
        }

        private List<ContactDTO> GetContactDTOListOfContactType(ContactType contactType)
        {
            log.LogMethodEntry(contactType);
            List<ContactDTO> contactDTOList = new List<ContactDTO>();
            if (customerDTO.ContactDTOList != null)
            {
                foreach (var contactDTO in customerDTO.ContactDTOList)
                {
                    if (contactDTO.ContactType == contactType && contactDTO.IsActive)
                    {
                        contactDTOList.Add(contactDTO);
                    }
                }
            }
            log.LogMethodExit(contactDTOList);
            return contactDTOList;
        }

        private ValidationError ValidateStringField(string entity, string defaultValueName, string attributeName, string attributeValue, string displayName)
        {
            log.LogMethodEntry(entity, defaultValueName, attributeName, attributeValue, displayName);
            ValidationError validationError = null;
            string specialChars = @"[-+=@]";
            if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, defaultValueName) == "M")
            {
                if (string.IsNullOrWhiteSpace(attributeValue))
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 249, MessageContainerList.GetMessage(executionContext, displayName));
                    validationError = new ValidationError(entity, attributeName, errorMessage);
                }
            }
            if (string.IsNullOrEmpty(attributeValue) == false && attributeName == "FirstName" && Regex.IsMatch(attributeValue.Substring(0, 1), specialChars))
            {
                validationError = new ValidationError(entity, attributeName, MessageContainerList.GetMessage(executionContext, 2265, MessageContainerList.GetMessage(executionContext, "First Name"), specialChars));
            }
            log.LogMethodExit(validationError);
            return validationError;
        }

        private ValidationError ValidateForeignKeyField(string entity, string defaultValueName, string attributeName, int attributeValue, string displayName)
        {
            log.LogMethodEntry(entity, defaultValueName, attributeName, attributeValue, displayName);
            ValidationError validationError = null;
            if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, defaultValueName) == "M")
            {
                if (attributeValue == -1)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 249, MessageContainerList.GetMessage(executionContext, displayName));
                    validationError = new ValidationError(entity, attributeName, errorMessage);
                }
            }
            log.LogMethodExit(validationError);
            return validationError;
        }
        #endregion

        /// <summary>
        /// Returns the customer image
        /// </summary>
        /// <returns></returns>
        public Image GetCustomerImage()
        {
            log.LogMethodEntry();
            Image customerImage = null;
            ProfileBL profileBL = new ProfileBL(executionContext, customerDTO.ProfileDTO);
            customerImage = profileBL.GetProfileImage();
            log.LogMethodExit(customerImage);
            return customerImage;
        }

        /// <summary>
        /// Return Base64 string of customer image
        /// </summary>
        /// <returns></returns>
        public string GetCustomerImageBase64()
        {
            log.LogMethodEntry();
            string customerImage = string.Empty;
            ProfileBL profileBL = new ProfileBL(executionContext, customerDTO.ProfileDTO);
            customerImage = profileBL.GetProfileImageBase64();
            log.LogMethodExit(customerImage);
            return customerImage;
        }

        /// <summary>
        /// Returns Base64 string of Idproof
        /// </summary>
        /// <returns></returns>

        public string GetIdImageBase64()
        {
            log.LogMethodEntry();
            string idImage = string.Empty;
            ProfileBL profileBL = new ProfileBL(executionContext, customerDTO.ProfileDTO);
            idImage = profileBL.GetIdImageBase64();
            log.LogMethodExit(idImage);
            return idImage;
        }

        /// <summary>
        /// Set the customer with eligible  membership
        /// </summary>
        /// <returns></returns>
        public void SetMembership(DateTime runForDate, int membershipID, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(runForDate, membershipID, sqlTransaction);
            ParafaitDBTransaction parafaitDBTrx = null;
            try
            {
                int currentMembershipId = membershipID;
                int nextMembershipId = -1;
                do
                {
                    //check whether applicable for membership and keep upgrading till eligible levels
                    if (nextMembershipId == -1 && currentMembershipId == membershipID)
                    {
                        nextMembershipId = currentMembershipId;
                    }
                    else
                    {
                        nextMembershipId = MembershipMasterList.GetNextLevelMembership(executionContext, currentMembershipId);
                    }
                    if (nextMembershipId != -1)
                    {
                        List<DateTime?> qualificationDateRange = MembershipMasterList.GetMembershipQualificationRange(executionContext, nextMembershipId, runForDate);
                        if (qualificationDateRange != null)
                        {
                            double loyaltyPoints = GetLoyaltyPoints(qualificationDateRange[0], qualificationDateRange[1]);
                            if (MembershipMasterList.IsEligibleForMembership(executionContext, nextMembershipId, loyaltyPoints))
                            {
                                currentMembershipId = nextMembershipId;
                                if (sqlTransaction == null)
                                {
                                    parafaitDBTrx = new ParafaitDBTransaction();
                                    parafaitDBTrx.BeginTransaction();
                                    sqlTransaction = parafaitDBTrx.SQLTrx;
                                }
                                //check for Primary card
                                SetPrimaryCard(runForDate, sqlTransaction);
                                SetCardsAsVip(currentMembershipId, runForDate, sqlTransaction);
                                CustomerRewards customerRewardsBL = new CustomerRewards(executionContext, this.customerDTO);
                                List<DateTime?> retentionDateRange = MembershipMasterList.GetMembershipRetentionRange(executionContext, currentMembershipId, runForDate);
                                customerRewardsBL.ApplyRewards(currentMembershipId, runForDate, sqlTransaction);
                                CustomerMembershipProgression membershipProgression = new CustomerMembershipProgression(executionContext, this.customerDTO);
                                membershipProgression.CreateMembershipProgressionEntry(currentMembershipId, retentionDateRange[0], retentionDateRange[1]);
                                this.customerDTO.MembershipId = currentMembershipId;
                                SaveCustomer(sqlTransaction);
                                if (parafaitDBTrx != null)
                                {
                                    parafaitDBTrx.EndTransaction();
                                    parafaitDBTrx.Dispose();
                                    sqlTransaction = null;
                                } 
                            }
                            else
                                nextMembershipId = -1;
                        }
                    }
                } while (nextMembershipId != -1); 
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit();
                if (parafaitDBTrx != null && parafaitDBTrx.SQLTrx != null && parafaitDBTrx.SQLTrx.Connection != null)
                {
                    parafaitDBTrx.RollBack();
                    parafaitDBTrx.Dispose();
                    sqlTransaction = null;
                }
                throw;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Set the customer with eligible  membership
        /// </summary>
        /// <returns></returns>
        public void RefreshMembership(DateTime runForDate)
        {
            log.LogMethodEntry(runForDate);
            ParafaitDBTransaction parafaitDBTrx = null;
            try
            {
                int currentMembershipId = this.customerDTO.MembershipId;
                CustomerRewards customerRewardsBL = new CustomerRewards(executionContext, this.customerDTO);
                using (parafaitDBTrx = new ParafaitDBTransaction())
                {
                    parafaitDBTrx.BeginTransaction();
                    SetPrimaryCard(runForDate, parafaitDBTrx.SQLTrx); //check for Primary card
                    SetCardsAsVip(currentMembershipId, runForDate, parafaitDBTrx.SQLTrx);
                    customerRewardsBL.ApplyRecurringRewards(this.customerDTO.MembershipId, runForDate, parafaitDBTrx.SQLTrx); //apply recurring rewards
                    SaveCustomer(parafaitDBTrx.SQLTrx);
                    parafaitDBTrx.EndTransaction(); 
                }
                //Check if customer can be upgraded to next level and keep upgrading if customer is eligble for next level
                int nextMembershipId = -1;
                do
                {
                    nextMembershipId = MembershipMasterList.GetNextLevelMembership(executionContext, currentMembershipId);
                    if (nextMembershipId != -1)
                    {
                        List<DateTime?> qualificationDateRange = MembershipMasterList.GetMembershipQualificationRange(executionContext, nextMembershipId, runForDate);
                        if (qualificationDateRange != null)
                        {
                            double loyaltyPoints = GetLoyaltyPoints(qualificationDateRange[0], qualificationDateRange[1]);
                            if (MembershipMasterList.IsEligibleForMembership(executionContext, nextMembershipId, loyaltyPoints))
                            {
                                //selected for Progression
                                currentMembershipId = nextMembershipId;
                                if (this.customerDTO.MembershipId != currentMembershipId)
                                {
                                    List<DateTime?> retentionDateRange = MembershipMasterList.GetMembershipRetentionRange(executionContext, currentMembershipId, runForDate);
                                    using (parafaitDBTrx = new ParafaitDBTransaction())
                                    {
                                        parafaitDBTrx.BeginTransaction();
                                        customerRewardsBL.ApplyRewards(currentMembershipId, runForDate, parafaitDBTrx.SQLTrx); // Apply rewards for the new membership
                                        CustomerMembershipProgression membershipProgression = new CustomerMembershipProgression(executionContext, this.customerDTO);
                                        membershipProgression.CreateMembershipProgressionEntry(currentMembershipId, retentionDateRange[0], retentionDateRange[1]);
                                        this.customerDTO.MembershipId = currentMembershipId;
                                        SaveCustomer(parafaitDBTrx.SQLTrx);
                                        parafaitDBTrx.EndTransaction(); 
                                    }
                                }
                            }
                            else
                                nextMembershipId = -1;
                        }
                    }
                } while (nextMembershipId != -1); 
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit();
                if (parafaitDBTrx != null && parafaitDBTrx.SQLTrx != null && parafaitDBTrx.SQLTrx.Connection != null)
                {
                    parafaitDBTrx.RollBack();
                    parafaitDBTrx.Dispose();
                }
                throw;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Check For Membership Retention
        /// </summary>
        /// <returns></returns>
        public void CheckForMembershipRetention(DateTime runForDate)
        {
            log.LogMethodEntry(runForDate);
            ParafaitDBTransaction parafaitDBTrx = null;
            try
            {
                using (parafaitDBTrx = new ParafaitDBTransaction())
                {
                    if (this.customerDTO != null)
                    {
                        int currentMembershipId = this.customerDTO.MembershipId;
                        int previousMembershipId = this.customerDTO.MembershipId;
                        do
                        {
                            if (previousMembershipId != -1)
                            {
                                List<DateTime?> retentionDateRange = MembershipMasterList.GetCurrentRetentionRange(executionContext, previousMembershipId, runForDate);
                                double loyaltyPoints = GetLoyaltyPoints(retentionDateRange[0], retentionDateRange[1]);
                                if (MembershipMasterList.IsEligibleForRetention(executionContext, previousMembershipId, loyaltyPoints))
                                {
                                    currentMembershipId = previousMembershipId;
                                    previousMembershipId = -1;
                                }
                                else
                                {
                                    previousMembershipId = MembershipMasterList.GetPreviousLevelMembership(executionContext, previousMembershipId);
                                    currentMembershipId = -1;
                                }
                            }
                        } while (previousMembershipId != -1);
                        parafaitDBTrx.BeginTransaction();
                        if (this.customerDTO.MembershipId == currentMembershipId) //retain at same level
                        {
                            //check for Primary card
                            SetPrimaryCard(runForDate, parafaitDBTrx.SQLTrx);
                            SetCardsAsVip(currentMembershipId, runForDate, parafaitDBTrx.SQLTrx);
                            CustomerRewards customerRewardsBL = new CustomerRewards(executionContext, this.customerDTO);
                            //Expire old rewards that needs to be expired
                            customerRewardsBL.ExpireOldRewards(runForDate, parafaitDBTrx.SQLTrx); 
                            List<DateTime?> retentionDateRange = MembershipMasterList.GetMembershipRetentionRange(executionContext, currentMembershipId, runForDate);
                            CustomerMembershipProgression membershipProgression = new CustomerMembershipProgression(executionContext, this.customerDTO);
                            membershipProgression.ExpireMembershipProgressionEntry(retentionDateRange[0]);
                            //membershipProgression.UpdateMembershipProgressionEntry(currentMembershipId, retentionDateRange[0], retentionDateRange[1]); 
                            customerRewardsBL.ApplyRewards(currentMembershipId, runForDate, parafaitDBTrx.SQLTrx); // Apply any recurring reward
                            membershipProgression.CreateMembershipProgressionEntry(currentMembershipId, retentionDateRange[0], retentionDateRange[1]);
                        }
                        else if (this.customerDTO.MembershipId != currentMembershipId && currentMembershipId != -1) //able to retain at a  lower level
                        {
                            //check for Primary card
                            SetPrimaryCard(runForDate, parafaitDBTrx.SQLTrx);
                            SetCardsAsVip(currentMembershipId, runForDate, parafaitDBTrx.SQLTrx);
                            CustomerRewards customerRewardsBL = new CustomerRewards(executionContext, this.customerDTO);
                            List<DateTime?> retentionDateRange = MembershipMasterList.GetMembershipRetentionRange(executionContext, currentMembershipId, runForDate);
                            customerRewardsBL.ApplyRewards(currentMembershipId, runForDate, parafaitDBTrx.SQLTrx); // Apply Rewards for new level
                            CustomerMembershipProgression membershipProgression = new CustomerMembershipProgression(executionContext, this.customerDTO);
                            membershipProgression.CreateMembershipProgressionEntry(currentMembershipId, retentionDateRange[0], retentionDateRange[1]);
                            this.customerDTO.MembershipId = currentMembershipId;
                        }
                        else // unable to retain any level of membership
                        {
                            CustomerRewards customerRewardsBL = new CustomerRewards(executionContext, this.customerDTO);
                            customerRewardsBL.ExpireOldRewards(runForDate, parafaitDBTrx.SQLTrx);
                            this.customerDTO.MembershipId = -1;
                        }
                        SaveCustomer(parafaitDBTrx.SQLTrx);
                        parafaitDBTrx.EndTransaction();
                        parafaitDBTrx.Dispose();
                    }
                    else
                        throw new Exception(MessageContainerList.GetMessage(executionContext, 1934));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit();
                if (parafaitDBTrx != null && parafaitDBTrx.SQLTrx != null && parafaitDBTrx.SQLTrx.Connection != null)
                {
                    parafaitDBTrx.RollBack();
                    parafaitDBTrx.Dispose();
                }
                throw;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Set the customer with purchased membership
        /// </summary>
        /// <returns></returns>
        public void SetPurchasedMembership(int membershipId, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(membershipId, sqlTrx);
            try
            {
                DateTime runForDate = CustomerRewards.GetLatestBizEndDateTime(executionContext);
                if (this.customerDTO != null)
                {
                    int currentMembershipId = this.customerDTO.MembershipId;
                    if (currentMembershipId != membershipId && currentMembershipId != -1)
                    {
                        //check whether purchased membership is below current level. if yes throw exception
                        if (MembershipMasterList.LowerThanCurrentMembershipLevel(executionContext, currentMembershipId, membershipId))
                        {
                            throw new Exception(MessageContainerList.GetMessage(executionContext, 1935));
                        }
                    }
                    CustomerRewards customerRewardsBL = new CustomerRewards(executionContext, this.customerDTO);
                    //check for Primary card
                    SetPrimaryCard(runForDate, sqlTrx);
                    SetCardsAsVip(membershipId, runForDate, sqlTrx);
                    List<DateTime?> retentionDateRange = MembershipMasterList.GetMembershipRetentionRange(executionContext, membershipId, runForDate);
                    customerRewardsBL.ExpireOldRewards(runForDate, sqlTrx);
                    customerRewardsBL.ApplyRewards(membershipId, runForDate, sqlTrx); // Apply rewards for the new membership 
                    CustomerMembershipProgression membershipProgression = new CustomerMembershipProgression(executionContext, this.customerDTO);
                    membershipProgression.CreatePurchasedMembershipEntry(membershipId, retentionDateRange[0], retentionDateRange[1]);
                    this.customerDTO.MembershipId = membershipId;
                    Save(sqlTrx);
                }
                else
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 1934));
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit();
                throw;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Overrider customer membership
        /// </summary>
        /// <returns></returns>
        public void OverRideMembership(int newMembershipId, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(newMembershipId, sqlTrx);
            List<ValidationError> validationErrorList = new List<ValidationError>();

            try
            {
                if (this.customerDTO != null)
                {
                    DateTime runForDate = CustomerRewards.GetLatestBizEndDateTime(executionContext);
                    int currentMembershipId = this.customerDTO.MembershipId;

                    if (currentMembershipId == newMembershipId)
                    {
                        return;
                    }

                    validationErrorList.AddRange(CheckDataAccessPermission(newMembershipId));
                    if (validationErrorList.Count > 0)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1936), validationErrorList);

                    }
                    else
                    {
                        validationErrorList.AddRange(CheckDataAccessPermission(currentMembershipId));
                        if (validationErrorList.Count > 0)
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1937), validationErrorList);
                        }
                    }
                    CustomerRewards customerRewardsBL = new CustomerRewards(executionContext, this.customerDTO);
                    if (newMembershipId == -1)
                    {

                        customerRewardsBL.ExpireOldRewards(runForDate, sqlTrx);
                        CustomerMembershipProgression membershipProgression = new CustomerMembershipProgression(executionContext, this.customerDTO);
                        membershipProgression.ExpireMembershipProgressionEntry(null);
                        this.customerDTO.MembershipId = -1;
                    }
                    else
                    {
                        SetPrimaryCard(runForDate, sqlTrx);
                        SetCardsAsVip(newMembershipId, runForDate, sqlTrx);
                        List<DateTime?> retentionDateRange = MembershipMasterList.GetMembershipRetentionRange(executionContext, newMembershipId, runForDate);
                        customerRewardsBL.ApplyRewards(newMembershipId, runForDate, sqlTrx); // Apply rewards for the new membership
                        CustomerMembershipProgression membershipProgression = new CustomerMembershipProgression(executionContext, this.customerDTO);
                        membershipProgression.CreateMembershipProgressionEntry(newMembershipId, retentionDateRange[0], retentionDateRange[1]);
                        this.customerDTO.MembershipId = newMembershipId;
                    }
                    SaveCustomer(sqlTrx);
                }
                else
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 1938));
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit();
                throw;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Get valid loyalty points for the ranges
        /// </summary>
        /// <returns></returns>
        public double GetLoyaltyPoints(DateTime? fromDate, DateTime? toDate)
        {
            log.LogMethodEntry(fromDate, toDate);
            double loyaltyPoints = 0;
            if (this.customerDTO != null)
            {
                DateTime? fromDateValue = (fromDate != null ? fromDate : (this.customerDTO.CreationDate != DateTime.MinValue ? this.customerDTO.CreationDate : DateTime.Now.AddYears(-25)));
                DateTime? toDateValue = (toDate != null ? toDate : DateTime.Now);
                if (this.customerDTO.AccountDTOList != null && this.customerDTO.AccountDTOList.Count > 0)
                {
                    foreach (AccountDTO accountDTO in this.customerDTO.AccountDTOList)
                    {
                        if (accountDTO.AccountCreditPlusDTOList != null && accountDTO.AccountCreditPlusDTOList.Count > 0)
                        {
                            loyaltyPoints = loyaltyPoints + Convert.ToDouble(accountDTO.AccountCreditPlusDTOList.Where(cp => (cp.CreditPlusType == CreditPlusType.LOYALTY_POINT && 
                              ((cp.PeriodFrom != null && cp.PeriodFrom != DateTime.MinValue ? cp.PeriodFrom : cp.CreationDate) >= fromDateValue
                                && (cp.PeriodFrom != null && cp.PeriodFrom != DateTime.MinValue ? cp.PeriodFrom : toDateValue) <= toDateValue))
                               ).Sum(cpl => cpl.CreditPlus));
                        }
                    }
                }
            }
            log.LogMethodExit(loyaltyPoints);
            return loyaltyPoints;
        }

        /// <summary>
        /// Get load card daily balance details
        /// </summary>
        /// <returns></returns>
        public void LoadCustomerCardDailyBalance(int currentRequestId, int concurrentProgramId)
        {
            log.LogMethodEntry(currentRequestId, concurrentProgramId);
            bool throwExceptionAtTheEnd = false;
            if (this.customerDTO.AccountDTOList != null && this.customerDTO.AccountDTOList.Count > 0)
            {
                DateTime? membershipProgressionDate = GetCurrentMembershipProgressionEffectiveFromDate();
                if (membershipProgressionDate != null)
                {
                    foreach (AccountDTO accountDTO in this.customerDTO.AccountDTOList)
                    {
                        if (accountDTO.ValidFlag)
                        {
                            AccountBL accountBL = new AccountBL(executionContext, accountDTO);
                            try
                            {
                                DateTime membershipProgressionDateValue = (DateTime)membershipProgressionDate;
                                accountBL.LoadDailyCardBalance(membershipProgressionDateValue);
                            }
                            catch (Exception ex)
                            {
                                throwExceptionAtTheEnd = true;
                                log.LogVariableState("accountDTO", accountDTO);
                                log.Error(ex);
                                ConcurrentRequestDetailsDTO concurrentRequestDetailsDTO = new ConcurrentRequestDetailsDTO(-1, currentRequestId, DateTime.Now, concurrentProgramId, "Accounts", accountDTO.AccountId, accountDTO.Guid, false, "Error", "", ex.Message, "DailyCardBalanceRun", DateTime.Now, executionContext.GetUserId(), DateTime.Now, executionContext.GetUserId(), executionContext.GetSiteId(), "", false, -1, true);
                                ConcurrentRequestDetailsBL concurrentRequestDetailsBL = new ConcurrentRequestDetailsBL(executionContext, concurrentRequestDetailsDTO);
                                concurrentRequestDetailsBL.Save();
                            }
                        }
                        else
                        {
                            log.Info("Skipping invalid card " + accountDTO.TagNumber);
                        }
                    }
                }
            }
            log.LogMethodExit(throwExceptionAtTheEnd);
            if (throwExceptionAtTheEnd)
            {
                throw new Exception(MessageContainerList.GetMessage(executionContext, "Error Loading Daily Card Balance"));
            }
        }

        private DateTime? GetCurrentMembershipProgressionEffectiveFromDate()
        {
            log.LogMethodEntry();
            DateTime? latestMembershipEffecitveFromDate = null;
            CustomerMembershipProgressionList customerMembershipProgressionList = new CustomerMembershipProgressionList(executionContext);
            List<KeyValuePair<CustomerMembershipProgressionDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<CustomerMembershipProgressionDTO.SearchByParameters, string>>();
            searchParam.Add(new KeyValuePair<CustomerMembershipProgressionDTO.SearchByParameters, string>(CustomerMembershipProgressionDTO.SearchByParameters.CUSTOMER_ID, this.customerDTO.Id.ToString()));
            searchParam.Add(new KeyValuePair<CustomerMembershipProgressionDTO.SearchByParameters, string>(CustomerMembershipProgressionDTO.SearchByParameters.MEMBERSHIP_ID, this.customerDTO.MembershipId.ToString()));
            List<CustomerMembershipProgressionDTO> customerMembershipProgressionDTOList = customerMembershipProgressionList.GetCustomerMembershipProgressionDTOList(searchParam);
            if (customerMembershipProgressionDTOList != null && customerMembershipProgressionDTOList.Count > 0)
            {
                customerMembershipProgressionDTOList = customerMembershipProgressionDTOList.OrderByDescending(t => t.EffectiveToDate).ToList();
                latestMembershipEffecitveFromDate = customerMembershipProgressionDTOList[0].EffectiveFromDate;
            }
            log.LogMethodExit(latestMembershipEffecitveFromDate);
            return latestMembershipEffecitveFromDate;
        }


        /// <summary>
        /// Get Current Membership Progression Effective ToDate
        /// </summary>
        /// <returns>latestMembershipEffecitveToDate</returns>
        public DateTime? GetCurrentMembershipEffectiveToDate()
        {
            log.LogMethodEntry();
            DateTime? latestMembershipEffecitveToDate = null;
            if (this.CustomerDTO.MembershipId < 0)
            {
                return latestMembershipEffecitveToDate;
            }
            MembershipBL membershipBL = new MembershipBL(executionContext, this.CustomerDTO.MembershipId);
            if (membershipBL.getMembershipDTO.MembershipID == -1)
            {
                return null;
            }
            if (this.customerDTO.CustomerMembershipProgressionDTOList != null && this.customerDTO.CustomerMembershipProgressionDTOList.Count == 0)
            {
                LoadCustomerRewards();
            }
            if (this.customerDTO.CustomerMembershipProgressionDTOList != null && this.customerDTO.CustomerMembershipProgressionDTOList.Count > 0)
            {
                List<CustomerMembershipProgressionDTO> customerMembershipProgressionList = new List<CustomerMembershipProgressionDTO>();
                customerMembershipProgressionList = this.customerDTO.CustomerMembershipProgressionDTOList.OrderByDescending(t => t.EffectiveToDate).ToList();
                latestMembershipEffecitveToDate = customerMembershipProgressionList[0].EffectiveToDate;
            }
            log.LogMethodExit(latestMembershipEffecitveToDate);
            return latestMembershipEffecitveToDate;
        }


        private void SetPrimaryCard(DateTime runForDate, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(runForDate, sqlTrx);
            try
            {
                if (this.customerDTO != null)
                {
                    if (this.customerDTO.AccountDTOList != null && this.customerDTO.AccountDTOList.Any())
                    {
                        LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                        DateTime cutOffDateTime = runForDate;// lookupValuesList.GetServerDateTime();

                        List<AccountDTO> primaryCardDTOList = this.customerDTO.AccountDTOList.Where(card => card.PrimaryAccount == true
                                                                                                            && card.ValidFlag == true
                                                                                                            && (card.ExpiryDate == null || card.ExpiryDate > cutOffDateTime)).ToList();
                        if (primaryCardDTOList == null || (primaryCardDTOList != null && primaryCardDTOList.Any() == false))
                        {
                            double cardLoyaltyPoints = 0;
                            int primaryCardId = -1;
                            AccountDTO primaryCustCardDTO = new AccountDTO();
                            if (this.customerDTO.AccountDTOList.Count == 1
                                && this.customerDTO.AccountDTOList[0].ValidFlag == true
                                && (this.customerDTO.AccountDTOList[0].ExpiryDate == null || this.customerDTO.AccountDTOList[0].ExpiryDate > cutOffDateTime))
                            //only 1 card
                            {
                                primaryCustCardDTO = this.customerDTO.AccountDTOList[0];
                                primaryCardId = primaryCustCardDTO.AccountId;
                            }
                            else if (this.customerDTO.AccountDTOList.Count > 1)
                            {
                                //get Parent cards for the customer. Semnox.Core.CardCore
                                ParentChildCardsListBL parentChildCardsListBL = new ParentChildCardsListBL(executionContext);
                                List<ParentChildCardsDTO> parentChildCardsDTOList = parentChildCardsListBL.GetActiveParentCardsListByCustomer(this.customerDTO.Id);
                                if (parentChildCardsDTOList != null && parentChildCardsDTOList.Count > 0)
                                {
                                    if (parentChildCardsDTOList.Count == 1) //one card, set it as primary
                                    {
                                        primaryCustCardDTO = this.customerDTO.AccountDTOList.Where(card => card.AccountId == parentChildCardsDTOList[0].ParentCardId
                                                                                                            && card.ValidFlag == true
                                                                                                            && (card.ExpiryDate == null || card.ExpiryDate > cutOffDateTime)).First();
                                        primaryCardId = primaryCustCardDTO.AccountId;
                                    }
                                    else //more than one, get oldest issued card
                                    {
                                        List<AccountDTO> validCardDTOList = this.customerDTO.AccountDTOList.Where(card => card.ValidFlag == true
                                                                                                                         && (card.ExpiryDate == null || card.ExpiryDate > cutOffDateTime)).ToList();
                                        if (validCardDTOList != null && validCardDTOList.Any())
                                        {
                                            primaryCustCardDTO = validCardDTOList.OrderBy(card => card.IssueDate).First();
                                            primaryCardId = primaryCustCardDTO.AccountId;
                                        }
                                    }
                                }
                                else
                                {  //get card with more loyalty points
                                    foreach (AccountDTO accountDTO in this.customerDTO.AccountDTOList)
                                    {
                                        if (accountDTO.AccountCreditPlusDTOList != null && accountDTO.AccountCreditPlusDTOList.Any()
                                            && accountDTO.ValidFlag == true && (accountDTO.ExpiryDate == null || accountDTO.ExpiryDate > cutOffDateTime))
                                        {
                                            double localLoyaltyPoint = Convert.ToDouble(accountDTO.AccountCreditPlusDTOList.Sum(cp => cp.CreditPlus));
                                            if (localLoyaltyPoint >= cardLoyaltyPoints && localLoyaltyPoint > 0)
                                            {
                                                cardLoyaltyPoints = localLoyaltyPoint;
                                                primaryCardId = accountDTO.AccountId;
                                                primaryCustCardDTO = accountDTO;
                                            }
                                        }
                                    }
                                    if (primaryCardId == -1)
                                    {
                                        List<AccountDTO> validCardDTOList = this.customerDTO.AccountDTOList.Where(card => card.ValidFlag == true
                                                                                                                         && (card.ExpiryDate == null || card.ExpiryDate > cutOffDateTime)).ToList();
                                        if (validCardDTOList != null && validCardDTOList.Any())
                                        {
                                            primaryCustCardDTO = validCardDTOList.OrderBy(card => card.IssueDate).First(); //get oldest issued card
                                            primaryCardId = primaryCustCardDTO.AccountId;
                                        }
                                    }
                                }
                            }

                            if (primaryCardId >= 0)
                            {
                                AccountDTO accountDTOListPrimary = primaryCustCardDTO;
                                this.customerDTO.AccountDTOList.Remove(accountDTOListPrimary);
                                if (this.customerDTO.AccountDTOList != null && this.customerDTO.AccountDTOList.Any())
                                {   //reset old primary cards if any
                                    for (int i = 0; i < this.customerDTO.AccountDTOList.Count; i++)
                                    {
                                        if (this.customerDTO.AccountDTOList[i].PrimaryAccount
                                            && this.customerDTO.AccountDTOList[i].AccountId != accountDTOListPrimary.AccountId)
                                        {
                                            this.customerDTO.AccountDTOList[i].PrimaryAccount = false;
                                            AccountBL accountBL = new AccountBL(executionContext, this.customerDTO.AccountDTOList[i]);
                                            accountBL.Save(sqlTrx);
                                            this.customerDTO.AccountDTOList[i] = accountBL.AccountDTO;
                                            this.customerDTO.AccountDTOList[i].AcceptChanges(); 
                                        }
                                    }
                                }
                                accountDTOListPrimary.PrimaryAccount = true;//set selected one as primary card
                                AccountBL primaryAccountBL = new AccountBL(executionContext, accountDTOListPrimary);
                                primaryAccountBL.Save(sqlTrx);
                                this.customerDTO.AccountDTOList.Add(primaryAccountBL.AccountDTO);
                            }
                            else
                                throw new Exception("Unable to map a card as primary card");
                        }
                    }
                    else
                        throw new Exception("No cards to map as a primary card");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit();
                throw;
            }
            log.LogMethodExit();
        }

        private void SetCardsAsVip(int currentMembershipId, DateTime runForDate, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(currentMembershipId, runForDate, sqlTrx);
            if (MembershipMasterList.IsVIPMembership(executionContext, currentMembershipId))
            {
                if (this.customerDTO.AccountDTOList != null && customerDTO.AccountDTOList.Any())
                { 
                    for (int i = 0; i < this.customerDTO.AccountDTOList.Count; i++)
                    { 
                        if (this.customerDTO.AccountDTOList[i].VipCustomer == false && this.customerDTO.AccountDTOList[i].ValidFlag
                             && (this.customerDTO.AccountDTOList[i].ExpiryDate == null || this.customerDTO.AccountDTOList[i].ExpiryDate > runForDate))
                        {
                            this.customerDTO.AccountDTOList[i].VipCustomer = true;
                            AccountBL accountBL = new AccountBL(executionContext, this.customerDTO.AccountDTOList[i]);
                            accountBL.Save(sqlTrx);
                            this.customerDTO.AccountDTOList[i] = accountBL.AccountDTO;
                            //this.customerDTO.AccountDTOList[i].AcceptChanges();
                        } 
                    }
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Get Redemption Discount For customer 
        /// </summary>
        /// <returns></returns>
        public double RedemptionDiscountForMembership()
        {
            log.LogMethodEntry();
            double retVal = 1;
            try
            {
                if (this.customerDTO != null && this.customerDTO.MembershipId > -1)
                {
                    MembershipBL membershipBL = new MembershipBL(executionContext, this.customerDTO.MembershipId);
                    retVal = (membershipBL.getMembershipDTO.RedemptionDiscount == 0 ? 1 : Math.Round((membershipBL.getMembershipDTO.RedemptionDiscount / 100), 2));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(retVal);
            return retVal;
        }



        /// <summary>
        /// IsMember method
        /// </summary>
        /// <returns></returns>
        public bool IsMember()
        {
            bool isMember = false;
            if (this.customerDTO != null)
            {
                if (this.customerDTO.MembershipId != -1)
                {
                    isMember = true;
                }
            }
            return isMember;
        }


        /// <summary>
        /// GetMembershipName method
        /// </summary>
        /// <returns>membershipName</returns>
        public string GetMembershipName()
        {
            log.LogMethodEntry();
            string membershipName = string.Empty;
            if (this.CustomerDTO.MembershipId != -1)
            {
                MembershipBL membershipBL = new MembershipBL(executionContext, this.CustomerDTO.MembershipId);
                membershipName = membershipBL.getMembershipDTO.MembershipName;
            }
            log.LogMethodExit(membershipName);
            return membershipName;

        }

        /// <summary>
        /// Loads the Customer Rewards
        /// </summary>
        public void LoadCustomerRewards()
        {
            log.LogMethodEntry();
            CustomerListBL customerListBL = new CustomerListBL(executionContext);
            List<int> custIdList = new List<int>();
            custIdList.Add(this.customerDTO.Id);
            List<CustomerDTO> customerDTOList = customerListBL.GetCustomerDTOList(custIdList, true);
            customerDTOList = customerListBL.LoadAccountDetails(customerDTOList, custIdList);
            if (this.customerDTO.CustomerMembershipRewardsLogDTOList == null
                || (this.customerDTO.CustomerMembershipRewardsLogDTOList != null && this.customerDTO.CustomerMembershipRewardsLogDTOList.Any() == false))
            {
                customerDTOList = customerListBL.LoadCustomerMembershipRewardLogs(customerDTOList, custIdList);
            }
            if (this.customerDTO.CustomerMembershipProgressionDTOList == null
                || (this.customerDTO.CustomerMembershipProgressionDTOList != null && this.customerDTO.CustomerMembershipProgressionDTOList.Any() == false))
            {
                customerDTOList = customerListBL.LoadCustomerMembershipProgressRecords(customerDTOList, custIdList);
            }
            if (customerDTOList != null && customerDTOList.Any())
            {
                this.customerDTO = customerDTOList[0];
            }
            CustomerRewards customerRewardsBL = new CustomerRewards(executionContext, this.customerDTO);
            log.LogMethodExit();
        }
        

        /// <summary>
        /// GetMembershipCard method
        /// </summary>
        /// <returns>membershipCardNumber</returns>
        public string GetMembershipCardNumber()
        {
            log.LogMethodEntry();
            string membershipCardNumber = string.Empty;
            if (this.customerDTO.AccountDTOList != null && this.customerDTO.CustomerMembershipProgressionDTOList.Count > 0)
            {
                List<AccountDTO> accountDTOList = new List<AccountDTO>();
                accountDTOList = this.customerDTO.AccountDTOList.Where(t => t.PrimaryAccount == true && t.ValidFlag == true).ToList();
                if (accountDTOList != null && accountDTOList.Any())
                {
                    membershipCardNumber = accountDTOList[0].TagNumber;
                }
            }
            log.LogMethodExit(membershipCardNumber);
            return membershipCardNumber;
        }


        /// <summary>
        /// GetMembershipCardId method
        /// </summary>
        /// <returns>membershipCardId</returns>
        public int GetMembershipCardId()
        {
            log.LogMethodEntry();
            int membershipCardId = -1;
            if (this.customerDTO.AccountDTOList != null && this.customerDTO.CustomerMembershipProgressionDTOList.Count > 0)
            {
                List<AccountDTO> accountDTOList = new List<AccountDTO>();
                accountDTOList = this.customerDTO.AccountDTOList.Where(t => t.PrimaryAccount == true && t.ValidFlag == true).ToList();
                if (accountDTOList != null && accountDTOList.Any())
                {
                    membershipCardId = accountDTOList[0].AccountId;
                }
            }
            log.LogMethodExit(membershipCardId);
            return membershipCardId;
        }


        /// <summary>
        /// Gets Active Loyalty points 
        /// </summary>
        /// <returns>earnedActiveLoyaltyPoints</returns>
        public double GetEarnedActiveLoyaltyPoints()
        {
            log.LogMethodEntry();
            double earnedActiveLoyaltyPoints = 0;

            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            DateTime serverDateTime = lookupValuesList.GetServerDateTime();

            if (this.customerDTO.AccountDTOList != null && this.customerDTO.CustomerMembershipProgressionDTOList.Count > 0)
            {
                foreach (AccountDTO accountDTO in this.customerDTO.AccountDTOList)
                {
                    if (accountDTO.ValidFlag)
                    {
                        if (accountDTO.AccountCreditPlusDTOList != null && accountDTO.AccountCreditPlusDTOList.Count > 0)
                        {
                            earnedActiveLoyaltyPoints = earnedActiveLoyaltyPoints + Convert.ToDouble(accountDTO.AccountCreditPlusDTOList.Where(cp => (cp.CreditPlusType == CreditPlusType.LOYALTY_POINT)
                                                                                                  && (cp.PeriodTo == null || cp.PeriodTo > serverDateTime)
                                                                                                   ).Sum(cpl => cpl.CreditPlus));
                        }
                    }
                }
            }
            log.LogMethodExit(earnedActiveLoyaltyPoints);
            return earnedActiveLoyaltyPoints;
        }


        /// <summary>
        /// Gets Points Required to Retain Membership
        /// </summary>
        /// <returns>requiredPointsToRetainMembership</returns>
        public double GetRequiredPointsToRetainMembership()
        {
            log.LogMethodEntry();
            double requiredPointsToRetainMembership = 0;
            if (this.customerDTO.MembershipId < 0)
            {
                return requiredPointsToRetainMembership;
            }
            MembershipBL membershipBL = new MembershipBL(executionContext, this.customerDTO.MembershipId);
            DateTime? CurrentMembershipProgressionEffectiveToDate = GetCurrentMembershipEffectiveToDate();
            List<DateTime?> retentionRange = membershipBL.GetRetentionRange(CurrentMembershipProgressionEffectiveToDate);
            double pointsEarnedInRetentionrange = GetLoyaltyPoints(retentionRange[0], retentionRange[1]);
            requiredPointsToRetainMembership = membershipBL.GetRequiredPointsToRetainMembership(pointsEarnedInRetentionrange);
            log.LogMethodExit(requiredPointsToRetainMembership);
            return requiredPointsToRetainMembership;
        }


        /// <summary>
        /// Get Expiring Points For Membership
        /// </summary>
        /// <param name="noOfMonths"></param>
        /// <returns>expiringPointsForMembership</returns>
        public double GetExpiringPointsForMembership(int noOfMonths)
        {
            log.LogMethodEntry();
            double expiringPointsForMembership = 0;
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            DateTime serverDateTime = lookupValuesList.GetServerDateTime();

            if (this.customerDTO.AccountDTOList != null && this.customerDTO.CustomerMembershipProgressionDTOList.Count > 0)
            {
                foreach (AccountDTO accountDTO in this.customerDTO.AccountDTOList)
                {
                    if (accountDTO.ValidFlag)
                    {
                        if (accountDTO.AccountCreditPlusDTOList != null && accountDTO.AccountCreditPlusDTOList.Count > 0)
                        {
                            expiringPointsForMembership = expiringPointsForMembership + Convert.ToDouble(accountDTO.AccountCreditPlusDTOList.Where(cp => (cp.CreditPlusType == CreditPlusType.LOYALTY_POINT)
                                                                                                                && ((cp.PeriodTo == null ? DateTime.MinValue : cp.PeriodTo) > serverDateTime && (cp.PeriodTo == null ? DateTime.MinValue : cp.PeriodTo) <= serverDateTime.AddMonths(noOfMonths))
                                                                                                              ).Sum(cpl => cpl.CreditPlus));
                        }
                    }
                }
            }
            log.LogMethodExit(expiringPointsForMembership);
            return expiringPointsForMembership;
        }


        /// <summary>
        ///  Get GameRewards 
        /// </summary>
        /// <returns></returns>
        public List<AccountGameDTO> GetGameRewards()
        {
            log.LogMethodEntry();
            List<AccountGameDTO> cardGamesDTOList = new List<AccountGameDTO>();
            if (this.customerDTO.AccountDTOList != null)
            {
                LoadCustomerRewards();
                foreach (AccountDTO accountDTO in this.customerDTO.AccountDTOList)
                {
                    if (accountDTO.AccountGameDTOList != null && accountDTO.AccountGameDTOList.Count > 0)
                    {
                        cardGamesDTOList.AddRange(accountDTO.AccountGameDTOList);
                    }
                }
            }

            log.LogMethodExit(cardGamesDTOList);
            return cardGamesDTOList;
        }


        /// <summary>
        /// GetDiscountRewards
        /// </summary>
        /// <returns></returns>
        public List<AccountDiscountDTO> GetDiscountRewards()
        {
            log.LogMethodEntry();
            List<AccountDiscountDTO> cardDiscountsDTOList = new List<AccountDiscountDTO>();

            if (this.customerDTO.AccountDTOList != null)
            {
                LoadCustomerRewards();
                foreach (AccountDTO accountDTO in this.customerDTO.AccountDTOList)
                {
                    if (accountDTO.AccountDiscountDTOList != null && accountDTO.AccountDiscountDTOList.Count > 0)
                    {
                        cardDiscountsDTOList.AddRange(accountDTO.AccountDiscountDTOList);
                    }
                }
            }
            log.LogMethodExit(cardDiscountsDTOList);
            return cardDiscountsDTOList;
        }


        /// <summary>
        /// Get How many more points for Next Membership
        /// </summary>
        /// <returns>howManyMorePointsForNextMembership</returns>
        public double GetHowManyMorePointsForNextMembership()
        {
            log.LogMethodEntry();
            double howManyMorePointsForNextMembership = 0;
            double nextLevelMembershipQualifyingPoints = 0;
            double pointsEarned = 0;
            int nextlevelmembershipId = MembershipMasterList.GetNextLevelMembership(executionContext, this.customerDTO.MembershipId);
            MembershipBL membershipBL = new MembershipBL(executionContext, nextlevelmembershipId);
            nextLevelMembershipQualifyingPoints = membershipBL.GetQualifyingPoints();
            List<DateTime?> qualificationRange = membershipBL.GetQualificationRange();
            if (qualificationRange != null)
            {
                pointsEarned = GetLoyaltyPoints(qualificationRange[0], qualificationRange[1]);
            }
            howManyMorePointsForNextMembership = nextLevelMembershipQualifyingPoints - pointsEarned;
            log.LogMethodExit(howManyMorePointsForNextMembership);
            return howManyMorePointsForNextMembership;
        }
        /// <summary>
        /// Get Customer WaiverList
        /// </summary>
        /// <returns></returns>
        public List<CustomerSignedWaiverDTO> GetCustomerWaiverList()
        {
            log.LogMethodEntry();
            LookupValuesList serverTimeObj = new LookupValuesList(executionContext);
            List<CustomerSignedWaiverDTO> customerSignedWaiverDTOList = null;
            CustomerSignedWaiverListBL customerSignedWaiverListBL = new CustomerSignedWaiverListBL(executionContext);
            List<KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>> searchParam = new List<KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>>();
            searchParam.Add(new KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>(CustomerSignedWaiverDTO.SearchByCSWParameters.SIGNED_FOR, this.customerDTO.Id.ToString()));
            //searchParam.Add(new KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>(CustomerSignedWaiverDTO.SearchByCSWParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParam.Add(new KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>(CustomerSignedWaiverDTO.SearchByCSWParameters.IS_ACTIVE, "1"));
            searchParam.Add(new KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>(CustomerSignedWaiverDTO.SearchByCSWParameters.IGNORE_EXPIRED, serverTimeObj.GetServerDateTime().ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            customerSignedWaiverDTOList = customerSignedWaiverListBL.GetAllCustomerSignedWaiverList(searchParam);
            if (customerSignedWaiverDTOList == null)
            {
                customerSignedWaiverDTOList = new List<CustomerSignedWaiverDTO>();
            }
            log.LogMethodExit(customerSignedWaiverDTOList);
            return customerSignedWaiverDTOList;
        }
        /// <summary>
        /// Load Customer Signed Waivers
        /// </summary>
        public void LoadCustomerSignedWaivers()
        {
            log.LogMethodEntry();
            if (this.customerDTO != null)
            {
                List<CustomerSignedWaiverDTO> customerSignedByWaiverDTOList = GetWaiversSignedByCustomer();
                List<CustomerSignedWaiverDTO> customerWaiverDTOList = GetCustomerWaiverList();
                if (customerWaiverDTOList != null && customerWaiverDTOList.Any())
                {
                    if (customerSignedByWaiverDTOList == null || customerSignedByWaiverDTOList.Count == 0)
                    {
                        customerSignedByWaiverDTOList = new List<CustomerSignedWaiverDTO>();
                    }
                    for (int i = 0; i < customerWaiverDTOList.Count; i++)
                    {
                        if (customerSignedByWaiverDTOList.Exists(csw => csw.CustomerSignedWaiverId == customerWaiverDTOList[i].CustomerSignedWaiverId) == false)
                        {
                            customerSignedByWaiverDTOList.Add(customerWaiverDTOList[i]);
                        }
                    }
                }
                this.customerDTO.CustomerSignedWaiverDTOList = new List<CustomerSignedWaiverDTO>(customerSignedByWaiverDTOList);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Get waivers signed by Customer
        /// </summary>
        /// <returns>List<CustomerSignedWaiverDTO></returns>
        public List<CustomerSignedWaiverDTO> GetWaiversSignedByCustomer()
        {
            log.LogMethodEntry();
            List<CustomerSignedWaiverDTO> customerSignedWaiverDTOList = null;
            LookupValuesList serverTimeObj = new LookupValuesList(executionContext);
            CustomerSignedWaiverListBL customerSignedWaiverListBL = new CustomerSignedWaiverListBL(executionContext);
            List<KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>> searchParam = new List<KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>>();
            searchParam.Add(new KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>(CustomerSignedWaiverDTO.SearchByCSWParameters.SIGNED_BY, this.customerDTO.Id.ToString()));
            //searchParam.Add(new KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>(CustomerSignedWaiverDTO.SearchByCSWParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParam.Add(new KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>(CustomerSignedWaiverDTO.SearchByCSWParameters.IS_ACTIVE, "1"));
            searchParam.Add(new KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>(CustomerSignedWaiverDTO.SearchByCSWParameters.IGNORE_EXPIRED, serverTimeObj.GetServerDateTime().ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            customerSignedWaiverDTOList = customerSignedWaiverListBL.GetAllCustomerSignedWaiverList(searchParam);
            if (customerSignedWaiverDTOList == null)
            {
                customerSignedWaiverDTOList = new List<CustomerSignedWaiverDTO>();
            }
            log.LogMethodExit(customerSignedWaiverDTOList);
            return customerSignedWaiverDTOList;
        }

        /// <summary>
        /// Create Customer Signed WaiverHeader
        /// </summary>
        /// <param name="waivers"></param>
        /// <param name="channel"></param>
        /// <param name="sqlTrx"></param>
        /// <returns>int</returns>
        public int CreateCustomerSignedWaiverHeader(string channel, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(channel, sqlTrx);
            LookupValuesList serverTimeObj = new LookupValuesList(executionContext);
            int headerId = -1;
            if (IsAdult() == false)
            {
                if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "WHO_CAN_SIGN_FOR_MINOR")
                    != WaiverCustomerAndSignatureDTO.WhoCanSignForMinor.MINOR.ToString())
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2302));
                    //Customer is a minor, please request the parent/guardian to sign the waivers for the minor customer
                }
            }
            CustomerSignedWaiverHeaderDTO customerSignedWaiverHeaderDTO = new CustomerSignedWaiverHeaderDTO(-1, this.customerDTO.Id, serverTimeObj.GetServerDateTime(), channel, executionContext.GetMachineId(), true, string.Empty);
            CustomerSignedWaiverHeaderBL customerSignedWaiverHeaderBL = new CustomerSignedWaiverHeaderBL(executionContext, customerSignedWaiverHeaderDTO);
            customerSignedWaiverHeaderBL.Save(sqlTrx);
            headerId = customerSignedWaiverHeaderDTO.CustomerSignedWaiverHeaderId;
            log.LogMethodExit(headerId);
            return headerId;
        }


        /// <summary>
        /// CreateCustomerSignedWaiver
        /// </summary>
        /// <param name="waiversDTO"></param>
        /// <param name="customerSignedWaiverHeaderId"></param>
        /// <param name="customerContentForWaiverDTOList"></param>
        /// <param name="custIdNameSignatureImageList"></param>
        /// <param name="managerId"></param>
        /// <param name="utilities"></param>
        /// <param name="guardianId"></param>
        public void CreateCustomerSignedWaiver(WaiversDTO waiversDTO, int customerSignedWaiverHeaderId, List<CustomerContentForWaiverDTO> customerContentForWaiverDTOList, List<WaiveSignatureImageWithCustomerDetailsDTO> custIdNameSignatureImageList, int managerId, Utilities utilities, int guardianId)
        {
            log.LogMethodEntry(waiversDTO, customerSignedWaiverHeaderId, customerContentForWaiverDTOList, custIdNameSignatureImageList, managerId);
            CustomerSignedWaiverDTO customerSignedWaiverDTO = null;
            LookupValuesList serverTimeObj = new LookupValuesList(executionContext);
            DateTime expiryDate = (waiversDTO.ValidForDays == null ? DateTime.MinValue : serverTimeObj.GetServerDateTime().AddDays((int)waiversDTO.ValidForDays));
            string signedWaiverFile = Guid.NewGuid() + ".pdf";
            log.LogVariableState("expiryDate", expiryDate);
            bool guestCustomer = false;
            int guestCustomerId = CustomerListBL.GetGuestCustomerId(executionContext);
            if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "SIGN_WAIVER_WITHOUT_CUSTOMER_REGISTRATION", false) == true
                && guestCustomerId != -1
                && customerDTO.Id == guestCustomerId
                && customerDTO.Id > -1)
            {
                guestCustomer = true;
            }
            else
            {
                if (IsAdult() == false)
                {
                    expiryDate = SetExpiryDateBasedOnAgeOfMajority(expiryDate);
                }
            }
            customerSignedWaiverDTO = new CustomerSignedWaiverDTO(-1, customerSignedWaiverHeaderId, waiversDTO.WaiverSetDetailId, signedWaiverFile, customerDTO.Id, expiryDate, true, null, null, null, waiversDTO.WaiverFileName);

            if (custIdNameSignatureImageList != null && custIdNameSignatureImageList.Any())
            {
                customerSignedWaiverDTO.WaiverSignedImageList = custIdNameSignatureImageList;
            }
            if (customerContentForWaiverDTOList != null && customerContentForWaiverDTOList.Any())
            {
                customerSignedWaiverDTO.CustomerContentForWaiverDTOList = new List<CustomerContentForWaiverDTO>(customerContentForWaiverDTOList);
            }
            if (customerSignedWaiverDTO.WaiverSignedImageList == null || customerSignedWaiverDTO.WaiverSignedImageList.Any() == false)
            {
                customerSignedWaiverDTO.SignedWaiverFileName = string.Empty; //content will be blank for manual waivers only
            }

            customerSignedWaiverDTO.GuardianId = guardianId;

            if (guestCustomer == false)
            {
                ExpireSignedWaiver(customerSignedWaiverDTO, managerId, utilities);
            }
            if (customerDTO.CustomerSignedWaiverDTOList == null)
            {
                CustomerDTO.CustomerSignedWaiverDTOList = new List<CustomerSignedWaiverDTO>();
            }
            CustomerDTO.CustomerSignedWaiverDTOList.Add(customerSignedWaiverDTO);
            log.LogMethodExit();
        }

        /// <summary>
        /// GetWaiverCode
        /// </summary>
        /// <param name="customerSignedWaiverHeaderId"></param>
        /// <returns></returns>
        public string GetWaiverCode(int customerSignedWaiverHeaderId)
        {
            log.LogMethodEntry(customerSignedWaiverHeaderId);
            bool entryFound = false;
            string waiverCode = string.Empty;
            if (this.CustomerDTO != null)
            {
                LoadCustomerSignedWaivers();
                if (this.CustomerDTO.CustomerSignedWaiverDTOList != null && this.CustomerDTO.CustomerSignedWaiverDTOList.Any())
                {
                    List<CustomerSignedWaiverDTO> customerSignedWaiverDTOList = this.customerDTO.CustomerSignedWaiverDTOList.Where(csw => csw.CustomerSignedWaiverHeaderId == customerSignedWaiverHeaderId).ToList();
                    if (customerSignedWaiverDTOList != null && customerSignedWaiverDTOList.Any())
                    {
                        entryFound = true;
                        waiverCode = customerSignedWaiverDTOList[0].WaiverCode;
                    }
                }
                if (entryFound == false)
                {
                    CustomerSignedWaiverHeaderBL customerSignedWaiverHeaderBL = new CustomerSignedWaiverHeaderBL(executionContext, customerSignedWaiverHeaderId);
                    waiverCode = customerSignedWaiverHeaderBL.GetCustomerSignedWaiverHeaderDTO.WaiverCode;
                }
            }
            log.LogMethodExit(waiverCode);
            return waiverCode;
        }

        /// <summary>
        /// IsAdult
        /// </summary>
        /// <returns>bool</returns>
        public bool IsAdult()
        {
            log.LogMethodEntry();
            bool isAdult = false;

            int ageOfMajority = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "AGE_OF_MAJORITY", -1);
            log.LogVariableState("ageOfMajority", ageOfMajority);
            if (ageOfMajority == -1)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2299, "AGE_OF_MAJORITY");
                log.LogMethodExit(message);
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2299, "AGE_OF_MAJORITY")); //Value is not defined for &1
            }
            if (customerDTO.DateOfBirth == null)
            {
                isAdult = true;  //If DOB in customerDTO is not present, then person will be considered as Adult
            }
            else if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "IGNORE_CUSTOMER_BIRTH_YEAR") == "Y")
            {
                isAdult = true;
            }
            else
            {
                double customerAge = GetCustomerAge();
                //int guestCustomerId = WaiverCustomerUtils.GetGuestCustomerId(executionContext);
                if (customerAge >= ageOfMajority)// || (this.customerDTO.Id == guestCustomerId && guestCustomerId > -1))
                {
                    isAdult = true;
                }
            }
            log.LogMethodExit(isAdult);
            return isAdult;
        }

        private double GetCustomerAge()
        {
            log.LogMethodEntry();
            LookupValuesList serverTimeObj = new LookupValuesList(executionContext);
            if (customerDTO == null || customerDTO.DateOfBirth == null)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2300)); //Customer date of birth information is missing
            }
            var today = serverTimeObj.GetServerDateTime();
            // Calculate the age.
            var customerAge = serverTimeObj.GetServerDateTime().Year - ((DateTime)customerDTO.DateOfBirth).Year;
            // Go back to the year the person was born in case of a leap year
            if (((DateTime)customerDTO.DateOfBirth).Date > today.AddYears(-customerAge))
            {
                if (DateTime.IsLeapYear(((DateTime)customerDTO.DateOfBirth).Year) && ((DateTime)customerDTO.DateOfBirth).Month == 2 && ((DateTime)customerDTO.DateOfBirth).Day == 29)
                {
                    log.LogVariableState("Birth date is Leap year day", customerDTO.DateOfBirth);
                }
                else
                {
                    customerAge--;
                }
            }
            log.LogMethodExit(customerAge);
            return customerAge;
        }

        private DateTime SetExpiryDateBasedOnAgeOfMajority(DateTime waiverExpiryDate)
        {
            log.LogMethodEntry(waiverExpiryDate);
            int ageOfMajority = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "AGE_OF_MAJORITY", -1);
            log.LogVariableState("ageOfMajority", ageOfMajority);
            if (ageOfMajority == -1)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2299, "AGE_OF_MAJORITY")); //Value is not defined for &1
            }
            if (customerDTO == null || customerDTO.DateOfBirth == null)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2300)); //Customer date of birth information is missing
            }
            DateTime expiryDate = ((DateTime)customerDTO.DateOfBirth).AddYears(ageOfMajority);
            log.LogVariableState("expiryDate", expiryDate);
            if (waiverExpiryDate > expiryDate || waiverExpiryDate == DateTime.MinValue)
            {
                waiverExpiryDate = expiryDate.AddDays(-1).AddHours(23).AddMinutes(50);
            }
            log.LogMethodExit(waiverExpiryDate);
            return waiverExpiryDate;
        }

        private void ExpireSignedWaiver(CustomerSignedWaiverDTO customerSignedWaiverDTO, int managerId, Utilities utilities)
        {
            log.LogMethodEntry(customerSignedWaiverDTO, managerId);
            LookupValuesList serverTimeObj = new LookupValuesList(executionContext);
            if (customerDTO.CustomerSignedWaiverDTOList != null && customerDTO.CustomerSignedWaiverDTOList.Any())
            {

                for (int i = 0; i < customerDTO.CustomerSignedWaiverDTOList.Count; i++)
                {
                    if (customerDTO.CustomerSignedWaiverDTOList[i].WaiverSetDetailId == customerSignedWaiverDTO.WaiverSetDetailId
                        && customerDTO.CustomerSignedWaiverDTOList[i].SignedFor == customerSignedWaiverDTO.SignedFor
                        && customerDTO.CustomerSignedWaiverDTOList[i].IsActive == true
                        && (customerDTO.CustomerSignedWaiverDTOList[i].ExpiryDate == null || customerDTO.CustomerSignedWaiverDTOList[i].ExpiryDate >= serverTimeObj.GetServerDateTime()))
                    {
                        CustomerSignedWaiverBL customerSignedWaiverBL = new CustomerSignedWaiverBL(executionContext, customerDTO.CustomerSignedWaiverDTOList[i]);
                        // if (customerSignedWaiverBL.CanDeactivate(utilities))
                        //{ UI need to do the validation whether customer signed waiver can be deactivated or not
                        customerDTO.CustomerSignedWaiverDTOList[i].DeactivatedBy = executionContext.GetUserPKId();
                        if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "WAIVER_DEACTIVATION_NEEDS_MANAGER_APPROVAL", false) && managerId == -1)
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2301));//Need manager approval to deactivate customer signed waiver
                        }
                        if (managerId > -1)
                        {
                            customerDTO.CustomerSignedWaiverDTOList[i].DeactivationApprovedBy = managerId;
                        }
                        customerDTO.CustomerSignedWaiverDTOList[i].DeactivationDate = serverTimeObj.GetServerDateTime();
                        customerDTO.CustomerSignedWaiverDTOList[i].ExpiryDate = serverTimeObj.GetServerDateTime();
                        customerDTO.CustomerSignedWaiverDTOList[i].IsActive = false;
                        //}
                        //else
                        //{
                        //    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2351, MessageContainerList.GetMessage(executionContext, "deactivate"), customerSignedWaiverBL.GetCustomerSignedWaiverDTO.CustomerSignedWaiverId));
                        //    //Cannot deactive the signed waiver id: &1, it is linked with active transaction(s) 
                        //}
                    }
                }
            }
            log.LogMethodExit();
        }

        private string GetErrorLabel(List<string> uniqueAttributesList)
        {
            log.LogMethodEntry();
            List<String> errorLable = new List<String>();
            foreach (string attribute in uniqueAttributesList)
            {
                string attributeDisplayName = CustomerDTO.GetCustomerSearchByParametersDisplayName(executionContext, attribute);
                errorLable.Add(attributeDisplayName);
            }
            string result = string.Empty;
            if (errorLable == null || errorLable.Count == 0)
            {
                log.LogMethodExit(result, "values are empty");
                return result;
            }
            result = string.Join(",", errorLable);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary> 
        /// This Method will check the unique attribute/attributes of Waiver set up in the configuration has duplicate value in the database table.
        /// </summary>
        /// <returns></returns>
        private ValidationError CheckCustomerAttributeForWaiver(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            Dictionary<CustomerSearchByParameters, string> newParameterCollection = new Dictionary<CustomerSearchByParameters, string>();
            Dictionary<CustomerSearchByParameters, string> oldParameterCollection = new Dictionary<CustomerSearchByParameters, string>();
            
            ValidationError validationError = null;
            List<string> waiverUniqueAttributesList = new List<string>();
            string waiverUniqueAttributes = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "WAIVER_CUSTOMER_LINK_ATTRIBUTES");
            if (string.IsNullOrEmpty(waiverUniqueAttributes))
            {
                log.Info("Waiver Unique attribute is not set.");
                return validationError;
            }

            // New Details of Customer
            char[] multiChar = new Char[] { ' ', ',', '.', '-', '|' };
            waiverUniqueAttributesList = waiverUniqueAttributes.Split(multiChar, StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (string attribute in waiverUniqueAttributesList)
            {
                CustomerSearchByParameters customerSearchByParameters = GetCustomerSearchCriteria(attribute);
                string searchParameterValue = GetValue(customerSearchByParameters, customerDTO);
                newParameterCollection.Add(customerSearchByParameters, searchParameterValue);
            }
            log.LogVariableState("newParameterCollection", newParameterCollection);
            // Old Details of Customer
            List<KeyValuePair<CustomerSearchByParameters, string>> customerSearchByParam = new List<KeyValuePair<CustomerSearchByParameters, string>>();
            customerSearchByParam.Add(new KeyValuePair<CustomerSearchByParameters, string>(CustomerSearchByParameters.CUSTOMER_ID, customerDTO.Id.ToString()));
            CustomerListBL customerListBL = new CustomerListBL(executionContext);
            List<CustomerDTO> customersDTOList = customerListBL.GetCustomerDTOList(customerSearchByParam, true);

            CustomerDTO currentCustomerDTO = null;
            if (customersDTOList != null && customersDTOList.Any())
            {
                currentCustomerDTO = customersDTOList.Where(x => x.Id == customerDTO.Id).FirstOrDefault();
                if (currentCustomerDTO != null)
                {
                    foreach (string attribute in waiverUniqueAttributesList)
                    {
                        CustomerSearchByParameters customerSearchByParameters = GetCustomerSearchCriteria(attribute);
                        string searchParameterValue = GetValue(customerSearchByParameters, currentCustomerDTO);
                        oldParameterCollection.Add(customerSearchByParameters, searchParameterValue);                            
                    }                            
                }
            }

            log.LogVariableState("oldParameterCollection", oldParameterCollection);
            var isCustomerDetailsChanged = newParameterCollection.Except(oldParameterCollection).ToDictionary(x => x.Key, x => x.Value);

            log.LogVariableState("isCustomerDetailsChanged", isCustomerDetailsChanged);

            List<CustomerSignedWaiverDTO> customerWaiverDTOList = GetCustomerWaiverList();
            bool activeWaiversFound = false;
            if (customerWaiverDTOList != null && customerWaiverDTOList.Any())
            {
                foreach (CustomerSignedWaiverDTO customerSignedWaiverDTO in customerWaiverDTOList)
                {
                    if (customerSignedWaiverDTO.IsActive && customerSignedWaiverDTO.ExpiryDate >= DateTime.Now)
                    {
                        activeWaiversFound = true;
                        break;
                    }
                }
            }
            log.LogVariableState("activeWaiversFound", activeWaiversFound);
            if (isCustomerDetailsChanged.Any() && activeWaiversFound)
            {
                string errorLable = GetErrorLabel(waiverUniqueAttributesList);
                string errorMessage = MessageContainerList.GetMessage(executionContext, 3031, errorLable);
                validationError = new ValidationError("Customer", "Active waiver found", errorMessage);
            }                                            
            
            log.LogMethodExit(validationError);
            return validationError;
        }


        /// <summary>
        /// This method checks whether unique attribute/attributes set up in the configuration has duplicate value in the database table
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>ValidationError</returns>
        private ValidationError CheckUniqueCustomerAttribute(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            List<KeyValuePair<CustomerSearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomerSearchByParameters, string>>();
            ValidationError validationError = null;
            List<string> uniqueAttributesList = new List<string>();
            string uniqueAttributes = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "UNIQUE_ATTRIBUTES");
            if (string.IsNullOrEmpty(uniqueAttributes))
            {
                log.Info("Unique attribute is not set. ");
                return validationError;
            }

            try
            {
                char[] multiChar = new Char[] { ' ', ',', '.', '-' };
                uniqueAttributesList = uniqueAttributes.Split(multiChar, StringSplitOptions.RemoveEmptyEntries).ToList();
                foreach (string attribute in uniqueAttributesList)
                {
                    CustomerSearchByParameters customerSearchByParameters = GetCustomerSearchCriteria(attribute);
                    string searchParameterValue = GetValue(customerSearchByParameters, customerDTO);
                    if (IsSetAsMandatoryField(customerSearchByParameters))
                    {
                        searchParameters.Add(new KeyValuePair<CustomerSearchByParameters, string>(customerSearchByParameters, searchParameterValue));
                    }
                    else
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 2402, MessageContainerList.GetMessage(executionContext, attribute)); //"Unique attribute requires &1 setup as mandatory  "
                        validationError = new ValidationError("Customer", attribute, errorMessage);
                        return validationError;
                    }
                }
            }
            catch (ValidationException ex)
            {
                validationError = new ValidationError("Customer", "Unique Attributes", ex.Message);
                return validationError;
            }

            if (searchParameters.Count > 0)
            {
                CustomerDataHandler customerDataHandler = new CustomerDataHandler(passPhrase, sqlTransaction);
                List<CustomerDTO> customerDTOList = customerDataHandler.GetCustomerDTOList(searchParameters);
                if (customerDTOList != null && customerDTOList.Count > 0)
                {
                    CustomerDTO currentCustomerDTO = customerDTOList.Where(x => x.Id != customerDTO.Id).FirstOrDefault();
                    if (currentCustomerDTO != null)
                    {
                        string errorLable = GetErrorLabel(uniqueAttributesList);
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 2403, errorLable);
                        validationError = new ValidationError("Customer", "Unique attributes", errorMessage); // "The customer registration is failed. Customers with Unique attributes already exists’");
                    }
                }
            }
            log.LogMethodExit(validationError);
            return validationError;
        }

        private ValidationError IsUniqueCustomer(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);

            ValidationError validationError = null;
            try
            {
                String uniqueAttribute1 = Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_UNIQUE_ATTRIBUTE_1", "");
                String uniqueAttribute2 = Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_UNIQUE_ATTRIBUTE_2", "");
                String uniqueAttribute3 = Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_UNIQUE_ATTRIBUTE_3", "");

                if (!String.IsNullOrWhiteSpace(uniqueAttribute1) || !String.IsNullOrWhiteSpace(uniqueAttribute2) || !String.IsNullOrWhiteSpace(uniqueAttribute3))
                {
                    bool checkUniqueness = true;
                    if (customerDTO.Id > -1)
                    {
                        CustomerBL existingCustomer = new CustomerBL(executionContext, customerDTO.Id,true,true, sqlTransaction);
                        if (existingCustomer.customerDTO.Email.Equals(customerDTO.Email, StringComparison.InvariantCultureIgnoreCase) &&
                            existingCustomer.customerDTO.PhoneNumber.Equals(customerDTO.PhoneNumber, StringComparison.InvariantCultureIgnoreCase) &&
                            existingCustomer.customerDTO.UniqueIdentifier.Equals(customerDTO.UniqueIdentifier, StringComparison.InvariantCultureIgnoreCase))
                        {
                            log.Debug("Data of unique attributes has not changed, skip check");
                            checkUniqueness = false;
                        }
                    }

                    if (checkUniqueness)
                    {
                        log.Debug("Checking for unique attributes " + uniqueAttribute1 + ":" + uniqueAttribute2 + ":" + uniqueAttribute3);
                        CustomerListBL customerListBL = new CustomerListBL(executionContext);
                        List<String> uniqueAttributes = new List<string>();
                        if (!String.IsNullOrWhiteSpace(uniqueAttribute1) && !uniqueAttribute1.Equals("NONE", StringComparison.InvariantCultureIgnoreCase))
                        {
                            uniqueAttributes.Add(uniqueAttribute1);
                        }

                        if (!String.IsNullOrWhiteSpace(uniqueAttribute2) && !uniqueAttribute2.Equals("NONE", StringComparison.InvariantCultureIgnoreCase))
                        {
                            uniqueAttributes.Add(uniqueAttribute2);
                        }

                        if (!String.IsNullOrWhiteSpace(uniqueAttribute3) && !uniqueAttribute3.Equals("NONE", StringComparison.InvariantCultureIgnoreCase))
                        {
                            uniqueAttributes.Add(uniqueAttribute3);
                        }

                        log.Debug("Checking unique attribute");
                        if (uniqueAttributes.Any())
                        {
                            CustomerSearchCriteria searchCriteria = new CustomerSearchCriteria();
                            // Check against active customer only
                            searchCriteria.And(CustomerSearchByParameters.ISACTIVE, Operator.EQUAL_TO, "1");
                            log.Debug("Adding adult check");
                            String dobMandatory = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "BIRTH_DATE", "");
                            DateTime majorityAge = ServerDateTime.Now.AddYears(-1 * ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AGE_OF_MAJORITY", 0));
                            if (!dobMandatory.Equals("M"))
                            {
                                log.Debug("DOB is not mandatory " + majorityAge.ToString("yyyy-MM-dd HH:mm:ss") + ":" + ServerDateTime.Now.Date.ToString("yyyy-MM-dd HH:mm:ss"));
                                searchCriteria.And(new CustomerSearchCriteria(
                                        CustomerSearchByParameters.IS_ADULT, Operator.LESSER_THAN_OR_EQUAL_TO, majorityAge)
                                        .Or(CustomerSearchByParameters.IS_ADULT, Operator.EQUAL_TO, ServerDateTime.Now.Date));
                            }
                            else
                            {
                                log.Debug("DOB is mandatory " + majorityAge.ToString("yyyy-MM-dd HH:mm:ss"));
                                searchCriteria.And(CustomerSearchByParameters.IS_ADULT, Operator.LESSER_THAN_OR_EQUAL_TO, majorityAge);
                            }


                            List<CustomerSearchCriteria> individualCriteriaList = new List<CustomerSearchCriteria>();
                            foreach (string uniqueAttributeType in uniqueAttributes)
                            {
                                String uniqueAttributeValue = "";
                                CustomerSearchCriteria individualCriteria = new CustomerSearchCriteria();
                                if (uniqueAttributeType.Equals("EMAIL", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    uniqueAttributeValue = customerDTO.Email;
                                    individualCriteria.And(CustomerSearchByParameters.CONTACT_IS_ACTIVE, Operator.EQUAL_TO, "1");
                                    individualCriteria.And(CustomerSearchByParameters.CONTACT_CONTACT_TYPE, Operator.EQUAL_TO, ContactType.EMAIL.ToString());
                                    individualCriteria.And(CustomerSearchByParameters.CONTACT_ATTRIBUTE1, Operator.EQUAL_TO, customerDTO.Email);

                                }
                                else if (uniqueAttributeType.Equals("PHONE", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    uniqueAttributeValue = customerDTO.PhoneNumber;
                                    individualCriteria.And(CustomerSearchByParameters.CONTACT_IS_ACTIVE, Operator.EQUAL_TO, "1");
                                    individualCriteria.And(CustomerSearchByParameters.CONTACT_CONTACT_TYPE, Operator.EQUAL_TO, ContactType.PHONE.ToString());
                                    individualCriteria.And(CustomerSearchByParameters.CONTACT_ATTRIBUTE1, Operator.EQUAL_TO, customerDTO.PhoneNumber);
                                }
                                else
                                {
                                    CustomerSearchByParameters customerSearchByParameters = GetCustomerSearchCriteria(uniqueAttributeType);
                                    uniqueAttributeValue = GetValue(customerSearchByParameters, customerDTO);
                                    individualCriteria.And(customerSearchByParameters, Operator.EQUAL_TO, uniqueAttributeValue);
                                }

                                log.Debug("uniqueAttributeType and value " + uniqueAttributeType + ":" + uniqueAttributeValue);
                                individualCriteriaList.Add(individualCriteria);
                            }

                            CustomerSearchCriteria finalCriteria = new CustomerSearchCriteria();
                            foreach (SearchCriteria tempSearchCriteria in individualCriteriaList)
                            {
                                finalCriteria.Or(tempSearchCriteria);
                            }

                            if (searchCriteria.ContainsCondition && finalCriteria.ContainsCondition)
                            {
                                searchCriteria.And(finalCriteria);
                                searchCriteria.OrderBy(CustomerSearchByParameters.CUSTOMER_ID)
                                    .Paginate(0, 10);
                                CustomerDataHandler customerDataHandler = new CustomerDataHandler(passPhrase, sqlTransaction);
                                List<CustomerDTO> customerDTOList = customerDataHandler.GetCustomerDTOList(searchCriteria);
                                if (customerDTOList != null && customerDTOList.Any())
                                {
                                    customerDTOList = customerDTOList.Where(x => x.Id != customerDTO.Id).ToList();
                                    if (customerDTOList != null && customerDTOList.Any())
                                    {
                                        string errorLable = GetErrorLabel(uniqueAttributes);
                                        string errorMessage = MessageContainerList.GetMessage(executionContext, 4660, errorLable);
                                        validationError = new ValidationError("Customer", "Unique attributes", errorMessage); // "The customer registr
                                        log.Debug(errorMessage);
                                    }
                                    else
                                    {
                                        log.Debug("Customer is unique");
                                    }
                                }
                                else
                                {
                                    log.Debug("Customer is unique");
                                }
                            }

                        }
                        log.Debug("No unique attributes are set up");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex);
                validationError = new ValidationError("Customer", "Unique attribute", ex.Message);
            }

            log.LogMethodExit(validationError);
            return validationError;
        }

        /// <summary>
        /// This method gets the string value for the CustomerSearchByParameters Key passed , from the customer DTO
        /// </summary>
        /// <param name="key">CustomerSearchByParameters Key</param>
        /// <param name="customerDTO">customerDTO</param>
        /// <returns>string</returns>
        private string GetValue(CustomerSearchByParameters key, CustomerDTO customerDTO)
        {
            log.LogMethodEntry(key, customerDTO);
            switch (key.ToString())
            {
                case "CUSTOMER_ID": { return string.IsNullOrEmpty(customerDTO.Id.ToString()) ? "-1" : customerDTO.Id.ToString(); }
                case "CUSTOMER_PROFILE_ID": { return string.IsNullOrEmpty(customerDTO.ProfileId.ToString()) ? "-1" : customerDTO.ProfileId.ToString(); }
                case "CUSTOMER_MEMBERSHIP_ID": { return string.IsNullOrEmpty(customerDTO.MembershipId.ToString()) ? "-1" : customerDTO.MembershipId.ToString(); }
                case "CUSTOMER_EXTERNAL_SYSTEM_REFERENCE": { return string.IsNullOrEmpty(customerDTO.ExternalSystemReference) ? string.Empty : customerDTO.ExternalSystemReference.ToString(); }
                case "CUSTOMER_CUSTOM_DATA_SET_ID": { return string.IsNullOrEmpty(customerDTO.CustomDataSetId.ToString()) ? "-1" : customerDTO.CustomDataSetId.ToString(); }
                case "CUSTOMER_VERIFIED": { return string.IsNullOrEmpty(customerDTO.Verified.ToString()) ? "N" : customerDTO.Verified.ToString(); }
                case "CUSTOMER_CREATED_BY": { return string.IsNullOrEmpty(customerDTO.CreatedBy) ? string.Empty : customerDTO.CreatedBy.ToString(); }
                case "CUSTOMER_CREATION_DATE": { return string.IsNullOrEmpty(customerDTO.CreationDate.ToString()) ? string.Empty : customerDTO.CreationDate.ToString(); }
                case "CUSTOMER_LAST_UPDATED_BY": { return string.IsNullOrEmpty(customerDTO.LastUpdatedBy) ? string.Empty : customerDTO.LastUpdatedBy.ToString(); }
                case "CUSTOMER_LAST_UPDATE_DATE": { return string.IsNullOrEmpty(customerDTO.LastUpdateDate.ToString()) ? DateTime.MinValue.ToString() : customerDTO.LastUpdateDate.ToString(); }
                case "CUSTOMER_SITE_ID": { return string.IsNullOrEmpty(customerDTO.SiteId.ToString()) ? string.Empty : customerDTO.SiteId.ToString(); }
                case "PROFILE_PROFILE_TYPE": { return string.IsNullOrEmpty(customerDTO.ProfileDTO.ProfileType.ToString()) ? string.Empty : customerDTO.ProfileDTO.ProfileType.ToString(); }
                case "PROFILE_TITLE": { return string.IsNullOrEmpty(customerDTO.Title.ToString()) ? string.Empty : customerDTO.Title.ToString(); }
                case "PROFILE_NOTES": { return string.IsNullOrEmpty(customerDTO.Notes.ToString()) ? string.Empty : customerDTO.Notes.ToString(); }
                case "PROFILE_ANNIVERSARY": { return string.IsNullOrEmpty(customerDTO.Anniversary.ToString()) ? DateTime.MinValue.ToString("yyyy-MM-dd HH:mm:ss") : Convert.ToDateTime(customerDTO.ProfileDTO.Anniversary).ToString("yyyy-MM-dd HH:mm:ss"); }
                case "PROFILE_PHOTO_URL": { return string.IsNullOrEmpty(customerDTO.PhotoURL.ToString()) ? string.Empty : customerDTO.ProfileDTO.PhotoURL.ToString(); }
                case "PROFILE_COMPANY": { return string.IsNullOrEmpty(customerDTO.Company.ToString()) ? string.Empty : customerDTO.ProfileDTO.Company.ToString(); }
                case "PROFILE_DESIGNATION": { return string.IsNullOrEmpty(customerDTO.Designation.ToString()) ? string.Empty : customerDTO.ProfileDTO.Designation.ToString(); }
                case "PROFILE_LAST_LOGIN_TIME": { return string.IsNullOrEmpty(customerDTO.LastLoginTime.ToString()) ? DateTime.MinValue.ToString() : customerDTO.LastLoginTime.ToString(); }
                case "PROFILE_EXTERNAL_SYSTEM_REFERENCE": { return string.IsNullOrEmpty(customerDTO.ExternalSystemReference.ToString()) ? string.Empty : customerDTO.ExternalSystemReference.ToString(); }
                case "CUSTOMER_GUID": { return string.IsNullOrEmpty(customerDTO.Guid.ToString()) ? string.Empty : customerDTO.Guid.ToString(); }
                case "MASTER_ENTITY_ID": { return string.IsNullOrEmpty(customerDTO.MasterEntityId.ToString()) ? "-1" : customerDTO.MasterEntityId.ToString(); }
                case "CUSTOMER_CHANNEL": { return string.IsNullOrEmpty(customerDTO.Channel) ? string.Empty : customerDTO.Channel.ToString(); }
                case "PROFILE_FIRST_NAME": { return string.IsNullOrEmpty(customerDTO.FirstName) ? string.Empty : customerDTO.FirstName.ToString(); }
                case "PROFILE_MIDDLE_NAME": { return string.IsNullOrEmpty(customerDTO.MiddleName) ? string.Empty : customerDTO.MiddleName.ToString(); }
                case "PROFILE_LAST_NAME": { return string.IsNullOrEmpty(customerDTO.LastName) ? string.Empty : customerDTO.LastName.ToString(); }
                case "PROFILE_DATE_OF_BIRTH": { return string.IsNullOrEmpty(customerDTO.DateOfBirth == null ? "" : customerDTO.DateOfBirth.ToString()) ? DateTime.MinValue.ToString("yyyy-MM-dd HH:mm:ss") : Convert.ToDateTime(customerDTO.DateOfBirth).ToString("yyyy-MM-dd HH:mm:ss"); }
                case "PROFILE_GENDER": { return string.IsNullOrEmpty(customerDTO.Gender) ? string.Empty : customerDTO.ToString(); }
                case "PROFILE_UNIQUE_IDENTIFIER": { return string.IsNullOrEmpty(customerDTO.UniqueIdentifier) ? string.Empty : customerDTO.UniqueIdentifier.ToString(); }
                case "PROFILE_TAX_CODE": { return string.IsNullOrEmpty(customerDTO.TaxCode) ? string.Empty : customerDTO.TaxCode.ToString(); }
                case "PROFILE_USER_NAME": { return string.IsNullOrEmpty(customerDTO.UserName) ? string.Empty : customerDTO.UserName.ToString(); }
                case "PROFILE_PASSWORD": { return string.IsNullOrEmpty(customerDTO.Password) ? string.Empty : customerDTO.Password.ToString(); }

                case "ADDRESS_LINE1":
                    {
                        string result = string.Empty;
                        if (customerDTO.AddressDTOList != null && customerDTO.AddressDTOList.Count > 0)
                        {
                            result = GetSearchParameterValue(customerDTO.AddressDTOList.Select(x => x.Line1.ToString()).ToList());
                        }
                        log.LogMethodExit(result);
                        return result;
                    }
                case "ADDRESS_LINE2":
                    {
                        string result = string.Empty;
                        if (customerDTO.AddressDTOList != null && customerDTO.AddressDTOList.Count > 0)
                        {
                            result = GetSearchParameterValue(customerDTO.AddressDTOList.Select(x => x.Line2.ToString()).ToList());
                        }
                        log.LogMethodExit(result);
                        return result;
                    }
                case "ADDRESS_LINE3":
                    {
                        string result = string.Empty;
                        if (customerDTO.AddressDTOList != null && customerDTO.AddressDTOList.Count > 0)
                        {
                            result = GetSearchParameterValue(customerDTO.AddressDTOList.Select(x => x.Line3.ToString()).ToList());
                        }
                        log.LogMethodExit(result);
                        return result;
                    }
                case "ADDRESS_CITY":
                    {
                        string result = string.Empty;
                        if (customerDTO.AddressDTOList != null && customerDTO.AddressDTOList.Count > 0)
                        {
                            result = GetSearchParameterValue(customerDTO.AddressDTOList.Select(x => x.City.ToString()).ToList());
                        }
                        log.LogMethodExit(result);
                        return result;
                    }
                case "ADDRESS_POSTAL_CODE":
                    {
                        string result = string.Empty;
                        if (customerDTO.AddressDTOList != null && customerDTO.AddressDTOList.Count > 0)
                        {
                            result = GetSearchParameterValue(customerDTO.AddressDTOList.Select(x => x.PostalCode.ToString()).ToList());
                        }
                        log.LogMethodExit(result);
                        return result;
                    }

                case "PHONE_NUMBER_LIST":
                    {
                        StringBuilder phoneNumberList = new StringBuilder(string.Empty);
                        int i = 0;
                        if (customerDTO.ContactDTOList == null || customerDTO.ContactDTOList.Count == 0)
                        {
                            return phoneNumberList.ToString();
                        }
                        var filteredContactDTOList = customerDTO.ContactDTOList.Where(x => x.ContactType == ContactType.PHONE).ToList();
                        string result = string.Empty;
                        if (filteredContactDTOList != null && filteredContactDTOList.Count > 0)
                        {
                            result = GetSearchParameterValue(filteredContactDTOList.Select(x => x.Attribute1.ToString()).ToList());
                        }
                        log.LogMethodExit(result);
                        return result;
                    }
                case "EMAIL_LIST":
                    {
                        StringBuilder customerEmailList = new StringBuilder(string.Empty);
                        int i = 0;
                        if (customerDTO.ContactDTOList == null || customerDTO.ContactDTOList.Count == 0)
                        {
                            return customerEmailList.ToString();
                        }
                        var filteredContactDTOList = customerDTO.ContactDTOList.Where(x => x.ContactType == ContactType.EMAIL).ToList(); ;
                        string result = string.Empty;
                        if (filteredContactDTOList != null && filteredContactDTOList.Count > 0)
                        {
                            result = GetSearchParameterValue(filteredContactDTOList.Select(x => x.Attribute1.ToString()).ToList());
                        }
                        log.LogMethodExit(result);
                        return result;
                    }

                case "WECHAT_ACCESS_TOKEN_LIST":
                    {
                        StringBuilder customerWechatList = new StringBuilder(string.Empty);
                        if (customerDTO.ContactDTOList == null || customerDTO.ContactDTOList.Count == 0)
                        {
                            return customerWechatList.ToString();
                        }
                        var filteredContactDTOList = customerDTO.ContactDTOList.Where(x => x.ContactType == ContactType.WECHAT).ToList(); ;
                        string result = string.Empty;
                        if (filteredContactDTOList != null && filteredContactDTOList.Count > 0)
                        {
                            result = GetSearchParameterValue(filteredContactDTOList.Select(x => x.Attribute1.ToString()).ToList());
                        }
                        log.LogMethodExit(result);
                        return result;
                    }
                case "FB_USERID_LIST":
                    {
                        StringBuilder customerFBUserId = new StringBuilder(string.Empty);
                        if (customerDTO.ContactDTOList == null || customerDTO.ContactDTOList.Count == 0)
                        {
                            return customerFBUserId.ToString();
                        }
                        var filteredContactDTOList = customerDTO.ContactDTOList.Where(x => x.ContactType == ContactType.FACEBOOK).ToList();
                        string result = string.Empty;
                        if (filteredContactDTOList != null && filteredContactDTOList.Count > 0)
                        {
                            result = GetSearchParameterValue(filteredContactDTOList.Select(x => x.Attribute1.ToString()).ToList());
                        }
                        log.LogMethodExit(result);
                        return result;
                    }
                case "TW_ACCESS_TOKEN_LIST":
                    {
                        StringBuilder customerTWIdList = new StringBuilder(string.Empty);
                        if (customerDTO.ContactDTOList == null || customerDTO.ContactDTOList.Count == 0)
                        {
                            return customerTWIdList.ToString();
                        }
                        var filteredContactDTOList = customerDTO.ContactDTOList.Where(x => x.ContactType == ContactType.TWITTER).ToList(); ;
                        string result = string.Empty;
                        if (filteredContactDTOList != null && filteredContactDTOList.Count > 0)
                        {
                            result = GetSearchParameterValue(filteredContactDTOList.Select(x => x.Attribute1.ToString()).ToList());
                        }
                        log.LogMethodExit(result);
                        return result;
                    }
                case "FB_ACCESS_TOKEN_LIST":
                    {
                        StringBuilder fbAccessTokenList = new StringBuilder(string.Empty);
                        if (customerDTO.ContactDTOList == null || customerDTO.ContactDTOList.Count == 0)
                        {
                            return fbAccessTokenList.ToString();
                        }
                        var filteredContactDTOList = customerDTO.ContactDTOList.Where(x => x.ContactType == ContactType.FACEBOOK).ToList();
                        string result = string.Empty;
                        if (filteredContactDTOList != null && filteredContactDTOList.Count > 0)
                        {
                            result = GetSearchParameterValue(filteredContactDTOList.Select(x => x.Attribute2.ToString()).ToList());
                        }
                        log.LogMethodExit(result);
                        return result;
                    }
                case "TW_ACCESS_SECRET_LIST":
                    {
                        StringBuilder twAccessTokenList = new StringBuilder(string.Empty);
                        int i = 0;
                        if (customerDTO.ContactDTOList == null || customerDTO.ContactDTOList.Count == 0)
                        {
                            return twAccessTokenList.ToString();
                        }
                        var filteredContactDTOList = customerDTO.ContactDTOList.Where(x => x.ContactType == ContactType.TWITTER).ToList();
                        string result = string.Empty;
                        if (filteredContactDTOList != null && filteredContactDTOList.Count > 0)
                        {
                            result = GetSearchParameterValue(filteredContactDTOList.Select(x => x.Attribute2.ToString()).ToList());
                        }
                        log.LogMethodExit(result);
                        return result;
                    }
                case "ADDRESS_ADDRESS_TYPE":
                    {
                        string result = string.Empty;
                        if (customerDTO.AddressDTOList != null && customerDTO.AddressDTOList.Count > 0)
                        {
                            result = GetSearchParameterValue(customerDTO.AddressDTOList.Select(x => x.AddressType.ToString()).ToList());
                        }
                        return result;
                    }

                case "ADDRESS_COUNTRY_ID":
                    {
                        string result = string.Empty;
                        if (customerDTO.AddressDTOList != null && customerDTO.AddressDTOList.Count > 0)
                        {
                            result = GetSearchParameterValue(customerDTO.AddressDTOList.Select(x => x.CountryId.ToString()).ToList());
                        }
                        return result;
                    }
                case "ADDRESS_STATE_ID":
                    {
                        string result = string.Empty;
                        if (customerDTO.AddressDTOList != null && customerDTO.AddressDTOList.Count > 0)
                        {
                            result = GetSearchParameterValue(customerDTO.AddressDTOList.Select(x => x.StateId.ToString()).ToList());
                        }
                        return result;
                    }
                case "CONTACT_CONTACT_TYPE":
                    {
                        string result = string.Empty;
                        if (customerDTO.ContactDTOList != null && customerDTO.ContactDTOList.Count > 0)
                        {
                            result = GetSearchParameterValue(customerDTO.ContactDTOList.Select(x => x.ContactType.ToString()).ToList());
                        }
                        return result;
                    }
                case "CONTACT_ATTRIBUTE1":
                    {
                        string result = string.Empty;
                        if (customerDTO.ContactDTOList != null && customerDTO.ContactDTOList.Count > 0)
                        {
                            result = GetSearchParameterValue(customerDTO.ContactDTOList.Select(x => x.Attribute1).ToList());
                        }
                        return result;
                    }

                case "CONTACT_ATTRIBUTE2":
                    {
                        string result = string.Empty;
                        if (customerDTO.ContactDTOList != null && customerDTO.ContactDTOList.Count > 0)
                        {
                            result = GetSearchParameterValue(customerDTO.ContactDTOList.Select(x => x.Attribute2).ToList());
                        }
                        return result;
                    }

                default:
                    return string.Empty;
            }

        }

        private string GetSearchParameterValue(List<string> values)
        {
            log.LogMethodEntry(values);
            string result = string.Empty;
            if (values == null || values.Count == 0)
            {
                log.LogMethodExit(result, "values are empty");
                return result;
            }
            result = string.Join(",", values);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// This methods verifies that unique attribute is set as Mandatory field or not.
        /// </summary>
        /// <param name="key">CustomerSearchByParameters key</param>
        /// <returns>bool</returns>
        private bool IsSetAsMandatoryField(CustomerSearchByParameters key)
        {
            log.LogMethodEntry(key);
            switch (key.ToString())
            {
                case "CUSTOMER_CHANNEL": { return ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CHANNEL") == "M" ? true : false; }
                case "PROFILE_FIRST_NAME": { return ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_NAME") == "M" ? true : false; }
                case "PROFILE_MIDDLE_NAME": { return ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "MIDDLE_NAME") == "M" ? true : false; }
                case "PROFILE_LAST_NAME": { return ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "LAST_NAME") == "M" ? true : false; }
                case "PROFILE_DATE_OF_BIRTH": { return ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "BIRTH_DATE") == "M" ? true : false; }
                case "PROFILE_GENDER": { return ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "GENDER") == "M" ? true : false; }
                case "PROFILE_ANNIVERSARY": { return ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ANNIVERSARY") == "M" ? true : false; }
                case "PROFILE_UNIQUE_IDENTIFIER": { return ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "UNIQUE_ID") == "M" ? true : false; }
                case "PROFILE_TAX_CODE": { return ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "TAXCODE") == "M" ? true : false; }
                case "PROFILE_USER_NAME": { return ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "USERNAME") == "M" ? true : false; }
                case "PROFILE_TITLE": { return ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "TITLE") == "M" ? true : false; }
                case "ADDRESS_LINE1": { return ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ADDRESS1") == "M" ? true : false; }
                case "ADDRESS_LINE2": { return ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ADDRESS2") == "M" ? true : false; }
                case "ADDRESS_LINE3": { return ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ADDRESS3") == "M" ? true : false; }
                case "ADDRESS_POSTAL_CODE": { return ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "PIN") == "M" ? true : false; }
                case "PHONE_NUMBER_LIST":
                    {
                        return ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CONTACT_PHONE") == "M" ||
        ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_EMAIL_OR_PHONE_MANDATORY") == "M" ? true : false;
                    }
                case "EMAIL_LIST":
                    {
                        return ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "EMAIL") == "M" ||
               ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_EMAIL_OR_PHONE_MANDATORY") == "M" ? true : false;
                    }
                case "WECHAT_ACCESS_TOKEN_LIST": { return ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "WECHAT_ACCESS_TOKEN") == "M" ? true : false; }
                case "FB_USERID_LIST": { return ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "FBUSERID") == "M" ? true : false; }
                case "TW_ACCESS_TOKEN_LIST": { return ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "TWACCESSTOKEN") == "M" ? true : false; }
                case "FB_ACCESS_TOKEN_LIST": { return ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "FBACCESSTOKEN") == "M" ? true : false; }
                case "TW_ACCESS_SECRET_LIST": { return ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "TWACCESSSECRET") == "M" ? true : false; }
                case "ADDRESS_CITY": { return ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CITY") == "M" ? true : false; }
                case "PROFILE_NOTES": { return ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "NOTES") == "M" ? true : false; }
                case "PROFILE_COMPANY": { return ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "COMPANY") == "M" ? true : false; }
                case "PROFILE_DESIGNATION": { return ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DESIGNATION") == "M" ? true : false; }
                case "ADDRESS_ADDRESS_TYPE": { return ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ADDRESS_TYPE") == "M" ? true : false; }
            }
            log.LogMethodExit(false);
            return false;

        }

        /// <summary>
        /// This method returns the CustomerSearchByParameters type for the string passed.
        /// </summary>
        /// <param name="customerSearchCriteria">customerSearchCriteria </param>
        /// <returns>CustomerSearchByParameters</returns>
        private CustomerSearchByParameters GetCustomerSearchCriteria(string customerSearchCriteria)
        {
            log.LogMethodEntry(customerSearchCriteria);
            CustomerSearchByParameters customerSearchByParameters;
            try
            {
                customerSearchByParameters = (CustomerSearchByParameters)Enum.Parse(typeof(CustomerSearchByParameters), customerSearchCriteria, true);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while parsing the customerSearchCriteria", ex);
                string errorMessage = MessageContainerList.GetMessage(executionContext, "Unique Attribute is Invalid, Please check the customer configuration." + MessageContainerList.GetMessage(executionContext, customerSearchCriteria));
                throw new ValidationException(errorMessage);
            }
            log.LogMethodExit(customerSearchByParameters);
            return customerSearchByParameters;
        }

        /// <summary>
        /// Has signed the waiver
        /// </summary>
        /// <param name="waiverId"></param>
        /// <returns>bool</returns>
        public bool HasSigned(int waiverId, DateTime trxDate)
        {
            log.LogMethodEntry(waiverId, trxDate);
            LookupValuesList serverTimeObj = new LookupValuesList(executionContext);
            bool signedWaivers = false;
            if (this.customerDTO != null)
            {
                if (loadCustomerSignedWaivers && this.customerDTO.CustomerSignedWaiverDTOList == null || this.customerDTO.CustomerSignedWaiverDTOList.Any() == false)
                {
                    this.customerDTO.CustomerSignedWaiverDTOList = GetCustomerWaiverList();
                    loadCustomerSignedWaivers = false;
                }

                if (this.customerDTO.CustomerSignedWaiverDTOList != null && this.customerDTO.CustomerSignedWaiverDTOList.Any()
                    && this.customerDTO.CustomerSignedWaiverDTOList.Exists(sw => sw.WaiverSetDetailId == waiverId))
                {
                    DateTime serverTime = serverTimeObj.GetServerDateTime();
                    if (trxDate != DateTime.MinValue)
                    {
                        serverTime = trxDate;
                    }
                    log.LogVariableState("serverTime", serverTime);
                    foreach (CustomerSignedWaiverDTO signedWaverDTO in this.customerDTO.CustomerSignedWaiverDTOList)
                    {
                        if (signedWaverDTO.WaiverSetDetailId == waiverId
                            && signedWaverDTO.IsActive == true
                            && signedWaverDTO.SignedFor == this.customerDTO.Id
                             && (signedWaverDTO.ExpiryDate == null
                                || (DateTime)signedWaverDTO.ExpiryDate == DateTime.MinValue
                                || signedWaverDTO.ExpiryDate >= serverTime))
                        {
                            signedWaivers = true;
                            break;
                        }
                    }
                }
            }
            log.LogMethodExit(signedWaivers);
            return signedWaivers;
        }
        /// <summary>
        /// Has signed the waiver
        /// </summary>
        /// <param name="waiversDTO"></param>
        /// <returns>bool</returns>
        public bool HasSigned(WaiversDTO waiversDTO, DateTime trxDate)
        {
            log.LogMethodEntry(waiversDTO, trxDate);
            bool signedWaivers = false;
            if (this.customerDTO != null)
            {
                signedWaivers = HasSigned(waiversDTO.WaiverSetDetailId, trxDate);
            }
            log.LogMethodExit(signedWaivers);
            return signedWaivers;
        }
        /// <summary>
        /// Has signed the waiver dto list
        /// </summary>
        /// <param name="waiversDTOList"></param>
        /// <returns>bool</returns>
        public bool HasSigned(List<WaiversDTO> waiversDTOList, DateTime trxDate)
        {
            log.LogMethodEntry(waiversDTOList, trxDate);
            bool signedWaivers = false;
            if (this.customerDTO != null)
            {
                if (loadCustomerSignedWaivers && this.customerDTO.CustomerSignedWaiverDTOList == null || this.customerDTO.CustomerSignedWaiverDTOList.Any() == false)
                {
                    this.customerDTO.CustomerSignedWaiverDTOList = GetCustomerWaiverList();
                    loadCustomerSignedWaivers = false;
                }
                if (this.customerDTO.CustomerSignedWaiverDTOList != null && this.customerDTO.CustomerSignedWaiverDTOList.Any())
                {
                    for (int i = 0; i < waiversDTOList.Count; i++)
                    {
                        signedWaivers = HasSigned(waiversDTOList[i].WaiverSetDetailId, trxDate);
                        if (signedWaivers == false)
                        {
                            break;
                        }
                    }
                }
            }
            log.LogMethodExit(signedWaivers);
            return signedWaivers;
        }
        /// <summary>
        /// GetCustomerSignedWaiverId
        /// </summary>
        /// <param name="waiverSetDetailsId"></param>
        /// <returns>int</returns>
        public int GetCustomerSignedWaiverId(int waiverSetDetailsId, DateTime trxDate)
        {
            log.LogMethodEntry(waiverSetDetailsId, trxDate);
            int customerSignedWaiverId = -1;
            if (this.customerDTO != null)
            {
                if (loadCustomerSignedWaivers && this.customerDTO.CustomerSignedWaiverDTOList == null || this.customerDTO.CustomerSignedWaiverDTOList.Any() == false)
                {
                    this.customerDTO.CustomerSignedWaiverDTOList = GetCustomerWaiverList();
                    loadCustomerSignedWaivers = false;
                }
                if (this.customerDTO.CustomerSignedWaiverDTOList != null && this.customerDTO.CustomerSignedWaiverDTOList.Any())
                {
                    CustomerSignedWaiverDTO matchingDTO = this.customerDTO.CustomerSignedWaiverDTOList.FirstOrDefault(csw => csw.WaiverSetDetailId == waiverSetDetailsId
                                                                                                                              && csw.SignedFor == this.customerDTO.Id
                                                                                                                              && csw.IsActive == true
                                                                                                                              && (csw.ExpiryDate == null ||
                                                                                                                                  csw.ExpiryDate >= trxDate));
                    if (matchingDTO != null)
                    {
                        customerSignedWaiverId = matchingDTO.CustomerSignedWaiverId;
                    }
                }
            }
            log.LogMethodExit(customerSignedWaiverId);
            return customerSignedWaiverId;
        }
        /// <summary>
        /// Get Signature Pending Waivers
        /// </summary>
        /// <param name="waiversDTOList"></param>
        /// <returns> List<WaiversDTO></returns>
        public List<WaiversDTO> GetSignaturePendingWaivers(List<WaiversDTO> waiversDTOList, DateTime trxDate)
        {
            log.LogMethodEntry(waiversDTOList, trxDate);
            List<WaiversDTO> pendingSignatureDTO = new List<WaiversDTO>(waiversDTOList);
            bool signedWaivers = false;
            if (this.customerDTO != null)
            {
                if (loadCustomerSignedWaivers && this.customerDTO.CustomerSignedWaiverDTOList == null || this.customerDTO.CustomerSignedWaiverDTOList.Any() == false)
                {
                    this.customerDTO.CustomerSignedWaiverDTOList = GetCustomerWaiverList();
                    loadCustomerSignedWaivers = false;
                }
                if (this.customerDTO.CustomerSignedWaiverDTOList != null && this.customerDTO.CustomerSignedWaiverDTOList.Any())
                {
                    for (int i = 0; i < waiversDTOList.Count; i++)
                    {
                        signedWaivers = false;
                        signedWaivers = HasSigned(waiversDTOList[i].WaiverSetDetailId, trxDate);
                        if (signedWaivers)
                        {
                            pendingSignatureDTO.Remove(waiversDTOList[i]);
                        }
                    }
                }
            }
            log.LogMethodExit(pendingSignatureDTO);
            return pendingSignatureDTO;
        }
        /// <summary>
        /// HasValidDateOfBirth
        /// </summary>
        /// <returns></returns>
        public bool HasValidDateOfBirth()
        {
            log.LogMethodEntry();
            DateTime dateOfBirth = DateTime.MinValue;
            bool validDate = false;
            if (this.customerDTO.DateOfBirth != null && DateTime.TryParse(this.customerDTO.DateOfBirth.ToString(), out dateOfBirth) == true)
            {
                validDate = true;
            }
            log.LogMethodExit(validDate);
            return validDate;
        }

        private void SendRegistrationMessage(MessagingClientDTO.MessagingChanelType messagingChanelType, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(messagingChanelType, sqlTrx);
            if (messagingChanelType == MessagingClientDTO.MessagingChanelType.EMAIL)
            {
                if (CustomerHasEmailId() == false)
                {
                    log.LogMethodExit("No Valid Email found to send the registartion mail");
                    return;
                }
            }
            if (messagingChanelType == MessagingClientDTO.MessagingChanelType.SMS)
            {
                if (CustomerHasPhoneNumber() == false)
                {
                    log.LogMethodExit(null, "No Valid phone number found to send the registartion SMS");
                    return;
                }
            }
            CustomerEventsBL customerEventsBL = new CustomerEventsBL(executionContext, ParafaitFunctionEvents.NEW_REGISTRATION_EVENT, customerDTO, sqlTrx);
            customerEventsBL.SendMessage(messagingChanelType, sqlTrx);
            log.LogMethodExit();
        }

        private bool CustomerHasEmailId()
        {
            log.LogMethodEntry();
            bool hasEmailId = false;
            string emailAddress = string.Empty;
            if (string.IsNullOrWhiteSpace(customerDTO.UserName) == false)
            {
                emailAddress = customerDTO.UserName;
            }
            if (string.IsNullOrWhiteSpace(emailAddress) &&
                customerDTO.ContactDTOList != null &&
                customerDTO.ContactDTOList.Any(x => x.ContactType == ContactType.EMAIL))
            {
                ContactDTO emailContactDTO = customerDTO.ContactDTOList.Where(x => x.ContactType == ContactType.EMAIL).OrderByDescending(x => x.LastUpdateDate).FirstOrDefault();
                if (emailContactDTO != null)
                {
                    emailAddress = emailContactDTO.Attribute1;
                }
            }
            if (string.IsNullOrWhiteSpace(emailAddress) == false)
            {
                hasEmailId = true;
            }
            log.LogMethodExit(hasEmailId);
            return hasEmailId;
        }
        private bool CustomerHasPhoneNumber()
        {
            log.LogMethodEntry();
            bool hasPhoneNumber = false;
            string phoneNumber = string.Empty;
            if (string.IsNullOrWhiteSpace(customerDTO.PhoneNumber) == false)
            {
                phoneNumber = customerDTO.UserName;
            }
            if (string.IsNullOrWhiteSpace(phoneNumber) &&
                customerDTO.ContactDTOList != null &&
                customerDTO.ContactDTOList.Any(x => x.ContactType == ContactType.PHONE))
            {
                ContactDTO phoneContactDTO = customerDTO.ContactDTOList.Where(x => x.ContactType == ContactType.PHONE).OrderByDescending(x => x.LastUpdateDate).FirstOrDefault();
                if (phoneContactDTO != null)
                {
                    phoneNumber = phoneContactDTO.Attribute1;
                }
            }
            if (string.IsNullOrWhiteSpace(phoneNumber) == false)
            {
                hasPhoneNumber = true;
            }
            log.LogMethodExit(hasPhoneNumber);
            return hasPhoneNumber;
        }
    }

    /// <summary>
    /// Manages the list of Customer
    /// </summary>
    public class CustomerListBL
    {
        private string passPhrase;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<CustomerDTO> customerDTOList;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public CustomerListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            passPhrase = Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE");
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
		/// <param name="accountDTOList"></param>
        public CustomerListBL(ExecutionContext executionContext, List<CustomerDTO> customerDTOList)
        {
            log.LogMethodEntry(executionContext, customerDTOList);
            this.executionContext = executionContext;
            this.customerDTOList = customerDTOList;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the no of customers matching the search criteria
        /// </summary>
        /// <param name="searchCriteria">search criteria</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        /// <returns></returns>
        public int GetCustomerCount(CustomerSearchCriteria searchCriteria, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchCriteria, sqlTransaction);
            CustomerDataHandler customerDataHandler = new CustomerDataHandler(passPhrase, sqlTransaction);
            int customerCount = customerDataHandler.GetCustomerCount(searchCriteria);
            log.LogMethodExit(customerCount);
            return customerCount;
        }

        /// <summary>
        /// Returns the Customer list
        /// </summary>
        public List<CustomerDTO> GetCustomerDTOList(CustomerSearchCriteria searchCriteria,
            bool loadChildRecords = false, bool activeChildRecords = true, bool loadSignedWaivers = false, SqlTransaction sqlTransaction = null,
             bool loadSignedWaiverFileContent = false, Utilities utilities = null)
        {
            log.LogMethodEntry(searchCriteria, loadChildRecords, activeChildRecords, loadSignedWaivers, sqlTransaction, loadSignedWaiverFileContent);
            CustomerDataHandler customerDataHandler = new CustomerDataHandler(passPhrase, sqlTransaction);
            List<CustomerDTO> customerDTOList = customerDataHandler.GetCustomerDTOList(searchCriteria);
            if (loadChildRecords)
            {
                if (customerDTOList != null && customerDTOList.Count > 0)
                {
                    Build(customerDTOList, activeChildRecords, loadSignedWaivers, sqlTransaction, loadSignedWaiverFileContent, utilities);
                }
            }
            log.LogMethodExit(customerDTOList);
            return customerDTOList;
        }

        /// <summary>
        /// Returns the no of customers matching the search criteria
        /// </summary>
        /// <param name="searchParameters"> search criteria</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        /// <returns></returns>
        public int GetCustomerCount(List<KeyValuePair<CustomerSearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            CustomerDataHandler customerDataHandler = new CustomerDataHandler(passPhrase, sqlTransaction);
            int customerCount = customerDataHandler.GetCustomerCount(searchParameters);
            log.LogMethodExit(customerCount);
            return customerCount;
        }

        /// <summary>
        /// Returns the Customer list
        /// </summary>
        public List<CustomerDTO> GetCustomerDTOList(List<KeyValuePair<CustomerSearchByParameters, string>> searchParameters,
            bool loadChildRecords = false, bool activeChildRecords = true, bool loadSignedWaivers = false, SqlTransaction sqlTransaction = null,
             bool loadSignedWaiverFileContent = false, Utilities utilities = null)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, loadSignedWaivers, sqlTransaction, loadSignedWaiverFileContent);
            CustomerDataHandler customerDataHandler = new CustomerDataHandler(passPhrase, sqlTransaction);
            List<CustomerDTO> customerDTOList = customerDataHandler.GetCustomerDTOList(searchParameters);
            if (loadChildRecords)
            {
                if (customerDTOList != null && customerDTOList.Count > 0)
                {
                    Build(customerDTOList, activeChildRecords, loadSignedWaivers, sqlTransaction, loadSignedWaiverFileContent, utilities);
                }
            }
            log.LogMethodExit(customerDTOList);
            return customerDTOList;
        }

        /// <summary>
        /// Returns the Customer list
        /// </summary>
        public List<CustomerDTO> GetCustomerDTOList(CustomerSearchCriteria searchParameters,
            int pageNumber, int pageSize, bool loadChildRecords = false, bool activeChildRecords = true, bool loadSignedWaivers = false, SqlTransaction sqlTransaction = null,
            bool loadSignedWaiverFileContent = false, Utilities utilities = null, bool buildActiveCampaignActivity = false, bool buildLastVistitedDate = false, DateTime? fromDate = null, DateTime? toDate = null)
        {
            log.LogMethodEntry(searchParameters, pageNumber, pageSize, loadChildRecords, activeChildRecords, loadSignedWaivers, sqlTransaction, loadSignedWaiverFileContent, utilities, buildActiveCampaignActivity, buildLastVistitedDate, fromDate, toDate);
            CustomerDataHandler customerDataHandler = new CustomerDataHandler(passPhrase, sqlTransaction);
            List<CustomerDTO> customerDTOList = customerDataHandler.GetCustomerDTOList(searchParameters, pageNumber, pageSize);
            if (loadChildRecords)
            {
                if (customerDTOList != null && customerDTOList.Count > 0)
                {
                    Build(customerDTOList, activeChildRecords, loadSignedWaivers, sqlTransaction, loadSignedWaiverFileContent, utilities, buildActiveCampaignActivity, buildLastVistitedDate, fromDate, toDate);
                }
            }
            log.LogMethodExit(customerDTOList);
            return customerDTOList;
        }
        

        /// <summary>
        /// Returns the Customer list having loyalty points as per parameter conditions
        /// </summary>
        public List<int> GetEligibleNewCustomerIdList(DateTime? fromDate, DateTime? toDate, int qualifyingPoints, DateTime lastRunDate, DateTime tillDateTime)
        {
            log.LogMethodEntry(fromDate, toDate, qualifyingPoints, lastRunDate, tillDateTime);
            CustomerDataHandler customerDataHandler = new CustomerDataHandler(passPhrase, null);
            List<int> customerIdList = new List<int>();
            customerIdList = customerDataHandler.GetEligibleNewCustomerIDList(fromDate, toDate, qualifyingPoints, lastRunDate, tillDateTime);
            log.LogMethodExit();
            return customerIdList;
        } 

        /// <summary>
        /// Returns the Customer list having active membership and have earned loyalty points after last run date
        /// </summary>
        public List<int> GetEligibleExistingCustomerIdList(DateTime lastRunDate, DateTime tillDateTime)
        {
            log.LogMethodEntry(lastRunDate, tillDateTime);
            CustomerDataHandler customerDataHandler = new CustomerDataHandler(passPhrase, null);
            List<int> customerIdList = new List<int>();
            customerIdList = customerDataHandler.GetEligibleExistingCustomerIDList(lastRunDate, tillDateTime);
            log.LogMethodExit();
            return customerIdList;
        }
         
        /// <summary>
        /// Returns the Customer list having expired membership
        /// </summary>
        public List<int> GetExpiredMembershipCustomerIdList(DateTime tillDateTime)
        {
            log.LogMethodEntry(tillDateTime);
            CustomerDataHandler customerDataHandler = new CustomerDataHandler(passPhrase, null);
            List<int> customerIdList = new List<int>();
            customerIdList = customerDataHandler.GetExpiredMembershipCustomerIDList(tillDateTime);
            log.LogMethodExit();
            return customerIdList;
        }

        /// <summary>
        /// Returns the Customer list eligible for daily card balance data gathering
        /// </summary>
        public List<CustomerDTO> GetCustomerForDailyCardBalance()
        {
            log.LogMethodEntry();
            CustomerDataHandler customerDataHandler = new CustomerDataHandler(passPhrase, null);
            List<CustomerDTO> customerDTOList = new List<CustomerDTO>();
            customerDTOList = customerDataHandler.GetCustomerForDailyCardBalance();
            log.LogMethodExit();
            return customerDTOList;
        }
        /// <summary>
        /// This method should be used to Save and Update the Customer details for Web Management Studio.
        /// </summary>
        public void SaveUpdateCustomerList()
        {
            log.LogMethodEntry();
            if (customerDTOList != null)
            {
                foreach (CustomerDTO customerDTO in customerDTOList)
                {
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            CustomerBL customerBL = new CustomerBL(executionContext, customerDTO);
                            customerBL.Save(parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx.Message, valEx);
                            throw;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex.Message, ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the List of CustomerListDTO
        /// </summary>
        /// <param name="customerList">CustomerList</param>
        /// <param name="sqlTransaction"></param>
        /// <returns>customerDTOList</returns>
        public List<CustomerDTO> GetCustomerDTOList(List<int> customerIdList, bool loadChildRecords = false, bool activeChildRecords = true, bool loadSignedWaivers = false,
                    SqlTransaction sqlTransaction = null, bool loadSignedWaiverFileContent = false, Utilities utilities = null)
        {
            log.LogMethodEntry(customerIdList, loadChildRecords, activeChildRecords, loadSignedWaivers, loadSignedWaiverFileContent, sqlTransaction);
            CustomerDataHandler customerDataHandler = new CustomerDataHandler(passPhrase, sqlTransaction);
            List<CustomerDTO> customerDTOList = customerDataHandler.GetCustomerDTOList(customerIdList);
            if (loadChildRecords)
            {
                if (customerDTOList != null && customerDTOList.Count > 0)
                {
                    Build(customerDTOList, activeChildRecords, loadSignedWaivers, sqlTransaction, loadSignedWaiverFileContent, utilities);
                }
            }
            log.LogMethodExit(customerDTOList);
            return customerDTOList;
        }


        /// <summary>
        /// Get Guest Customer DTO
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static int GetGuestCustomerId(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            int guestCustomerId = -1;
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParms = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchParms.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "WAIVER_SETUP"));
            searchParms.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "GUEST_CUSTOMER_FOR_WAIVER"));
            searchParms.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchParms);
            if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
            {
                string guestCustomerGuid = lookupValuesDTOList[0].Description;

                if (string.IsNullOrEmpty(guestCustomerGuid) == false)
                {
                    CustomerListBL customerListBL = new CustomerListBL(executionContext);
                    CustomerSearchCriteria customerSearchCriteria = new CustomerSearchCriteria();
                    customerSearchCriteria.And(CustomerSearchByParameters.CUSTOMER_GUID, Operator.EQUAL_TO, guestCustomerGuid);
                    List<CustomerDTO> customerDTOList = customerListBL.GetCustomerDTOList(customerSearchCriteria);
                    if (customerDTOList != null && customerDTOList.Any())
                    {
                        guestCustomerId = customerDTOList[0].Id;
                    }
                }
            }
            log.LogMethodExit(guestCustomerId);
            return guestCustomerId;
        }

        /// <summary>
        /// Returns list of all customers who were active between given date range
        /// </summary>
        /// <returns>Dictionary of customerId, customerName</returns>
        public SortedDictionary<int, string> GetActiveCustomersInDateRangeList(DateTime fromDate, DateTime toDate, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(fromDate, toDate, sqlTransaction);
            CustomerDataHandler customerDataHandler = new CustomerDataHandler(passPhrase, sqlTransaction);
            SortedDictionary<int, string> activeCustomerList = customerDataHandler.GetActiveCustomersInDateRangeList(fromDate, toDate, sqlTransaction);
            log.LogMethodExit(activeCustomerList);
            return activeCustomerList;
        }

        /// <summary>
        /// Validates if the mobile device belongs to the given customer or not
        /// </summary>
        /// <returns>CustomerDTO</returns>
        public CustomerDTO ValidateCustomerDevice(String phoneOrEmail, String deviceGUID)
        {
            CustomerDTO customerDTO = null;

            ContactListBL contactListBL = new ContactListBL(executionContext);
            List<KeyValuePair<ContactDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<ContactDTO.SearchByParameters, string>>();
            if (!String.IsNullOrEmpty(phoneOrEmail))
                searchByParameters.Add(new KeyValuePair<ContactDTO.SearchByParameters, string>(ContactDTO.SearchByParameters.ATTRIBUTE1, phoneOrEmail));
            searchByParameters.Add(new KeyValuePair<ContactDTO.SearchByParameters, string>(ContactDTO.SearchByParameters.UUID, deviceGUID));
            List<ContactDTO> contactDTOList = contactListBL.GetContactDTOList(searchByParameters);

            if (contactDTOList != null && contactDTOList.Count > 0)
            {
                List<ContactDTO> inactiveContactList = contactDTOList.Where(x => x.UUID.Equals(deviceGUID) && !x.IsActive).ToList();
                List<ContactDTO> activeContactList = contactDTOList.Where(x => x.UUID.Equals(deviceGUID) && x.IsActive).ToList();
                if (inactiveContactList != null && inactiveContactList.Any())
                {
                    CustomerActivityUserLogDTO unlinkcustomerActivityUserLogDTO = new CustomerActivityUserLogDTO(-1, null, deviceGUID,
                                   "CUSTOMER_DEVICE_VERIFICATION", "Unauthorized device for " + phoneOrEmail, ServerDateTime.Now,
                                   "POS " + executionContext.GetPosMachineGuid(), "phone:" + phoneOrEmail + " device:" + deviceGUID,
                                   Enum.GetName(typeof(CustomerActivityUserLogDTO.ActivityCategory), CustomerActivityUserLogDTO.ActivityCategory.SMARTFUN),
                                   Enum.GetName(typeof(CustomerActivityUserLogDTO.ActivitySeverity), CustomerActivityUserLogDTO.ActivitySeverity.ERROR));
                    CustomerActivityUserLogBL unlinkcustomerActivityUserLogBL = new CustomerActivityUserLogBL(executionContext, unlinkcustomerActivityUserLogDTO);
                    unlinkcustomerActivityUserLogBL.Save();
                    throw new ValidationException("This device is unauthorized for use. Please contact site to reset this device.");
                }
                else if (activeContactList != null)
                {
                    ContactDTO primaryContact = activeContactList[0];
                    //List<KeyValuePair<CustomerSearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomerSearchByParameters, string>>();
                    //searchParameters.Add(new KeyValuePair<CustomerSearchByParameters, string>(CustomerSearchByParameters.CONTACT_IS_ACTIVE, "1"));
                    //searchParameters.Add(new KeyValuePair<CustomerSearchByParameters, string>(CustomerSearchByParameters.CONTACT_CONTACT_TYPE, primaryContact.ContactType.ToString()));
                    //searchParameters.Add(new KeyValuePair<CustomerSearchByParameters, string>(CustomerSearchByParameters.CONTACT_ATTRIBUTE1, primaryContact.Attribute1));
                    //searchParameters.Add(new KeyValuePair<CustomerSearchByParameters, string>(CustomerSearchByParameters.CUSTOMER_PROFILE_ID, primaryContact.ProfileId.ToString()));
                    CustomerSearchCriteria customerSearchCriteria = new CustomerSearchCriteria();
                    customerSearchCriteria.And(CustomerSearchByParameters.CONTACT_IS_ACTIVE, Operator.EQUAL_TO, "1");
                    customerSearchCriteria.And(CustomerSearchByParameters.CONTACT_CONTACT_TYPE, Operator.EQUAL_TO, primaryContact.ContactType.ToString());
                    customerSearchCriteria.And(CustomerSearchByParameters.CONTACT_ATTRIBUTE1, Operator.EQUAL_TO, primaryContact.Attribute1);
                    customerSearchCriteria.And(CustomerSearchByParameters.CUSTOMER_PROFILE_ID, Operator.EQUAL_TO, primaryContact.ProfileId.ToString());
                    List<CustomerDTO> customerList = GetCustomerDTOList(customerSearchCriteria, 0, 1, true, true);
                    if (customerList != null && customerList.Count == 1)
                    {
                        CustomerActivityUserLogDTO unlinkcustomerActivityUserLogDTO = new CustomerActivityUserLogDTO(-1, customerList[0].Id, deviceGUID,
                                   "CUSTOMER_DEVICE_VERIFICATION", "Successful device verification for " + phoneOrEmail, ServerDateTime.Now,
                                   "POS " + executionContext.GetPosMachineGuid(), "phone:" + phoneOrEmail + " device:" + deviceGUID,
                                   Enum.GetName(typeof(CustomerActivityUserLogDTO.ActivityCategory), CustomerActivityUserLogDTO.ActivityCategory.SMARTFUN),
                                   Enum.GetName(typeof(CustomerActivityUserLogDTO.ActivitySeverity), CustomerActivityUserLogDTO.ActivitySeverity.INFO));
                        CustomerActivityUserLogBL unlinkcustomerActivityUserLogBL = new CustomerActivityUserLogBL(executionContext, unlinkcustomerActivityUserLogDTO);
                        unlinkcustomerActivityUserLogBL.Save();

                        return customerList[0];
                    }
                    else
                    {
                        CustomerActivityUserLogDTO unlinkcustomerActivityUserLogDTO = new CustomerActivityUserLogDTO(-1, null, deviceGUID,
                                   "CUSTOMER_DEVICE_VERIFICATION", "No customer record found for " + phoneOrEmail, ServerDateTime.Now,
                                   "POS " + executionContext.GetPosMachineGuid(), "phone:" + phoneOrEmail + " device:" + deviceGUID,
                                   Enum.GetName(typeof(CustomerActivityUserLogDTO.ActivityCategory), CustomerActivityUserLogDTO.ActivityCategory.SMARTFUN),
                                   Enum.GetName(typeof(CustomerActivityUserLogDTO.ActivitySeverity), CustomerActivityUserLogDTO.ActivitySeverity.ERROR));
                        CustomerActivityUserLogBL unlinkcustomerActivityUserLogBL = new CustomerActivityUserLogBL(executionContext, unlinkcustomerActivityUserLogDTO);
                        unlinkcustomerActivityUserLogBL.Save();

                        throw new ValidationException("No customer record found for this device. Please contact site to reset this device.");
                    }
                }
            }
            else
            {
                CustomerActivityUserLogDTO unlinkcustomerActivityUserLogDTO = new CustomerActivityUserLogDTO(-1, null, deviceGUID,
                                   "CUSTOMER_DEVICE_VERIFICATION", "Unknown device for " + phoneOrEmail, ServerDateTime.Now,
                                   "POS " + executionContext.GetPosMachineGuid(), "phone:" + phoneOrEmail + " device:" + deviceGUID,
                                   Enum.GetName(typeof(CustomerActivityUserLogDTO.ActivityCategory), CustomerActivityUserLogDTO.ActivityCategory.SMARTFUN),
                                   Enum.GetName(typeof(CustomerActivityUserLogDTO.ActivitySeverity), CustomerActivityUserLogDTO.ActivitySeverity.ERROR));
                CustomerActivityUserLogBL unlinkcustomerActivityUserLogBL = new CustomerActivityUserLogBL(executionContext, unlinkcustomerActivityUserLogDTO);
                unlinkcustomerActivityUserLogBL.Save();

                // there are no numbers in db. All the user to proceed to register
                throw new EntityNotFoundException("Device record not found");
            }
            return customerDTO;
        }

        public Sheet BuildTemplate()
        {
            try
            {
                log.LogMethodEntry();
                Sheet sheet = new Sheet();
                ///All column Headings are in a headerRow object
                Row headerRow = new Row();
                ///Mapper class thats map sheet object
                CustomerDTODefinition customerDTODefinition = new CustomerDTODefinition(executionContext, "");
                customerDTODefinition.BuildHeaderRow(headerRow);
                ///Building headers from customerDTODefinition
                CustomerDTO customerDTO = new CustomerDTO();
                customerDTODefinition.Configure(customerDTO);

                sheet.AddRow(headerRow);
                Row row = new Row();
                customerDTODefinition.Serialize(row, customerDTO);
                sheet.AddRow(row);
                log.LogMethodExit(sheet);
                return sheet;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }

        public Tuple<Sheet, int> BulkUpload(Sheet sheet)
        {
            log.LogMethodEntry();
            CustomerDTODefinition customerDTODefinition = new CustomerDTODefinition(executionContext, "");
            Sheet responseSheet = new Sheet();
            Sheet errorSheet = new Sheet();
            int importedCustomerCount = 0;
            int errorCustomerCount = 0;
            ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction();
            responseSheet.AddRow(sheet[0]);

            //looping the sheet object. Sheet will have  multiple roews. Top row will be column heading
            for (int i = 1; i < sheet.Rows.Count; i++)
            {
                int index = 0;
                try
                {
                    CustomerDTO rowCustomerDTO = (CustomerDTO)customerDTODefinition.Deserialize(sheet[0], sheet[i], ref index);
                    if (rowCustomerDTO != null)
                    {
                        parafaitDBTrx.BeginTransaction();
                        if (rowCustomerDTO.Id < 0)
                        {
                            try
                            {
                                CustomerBL customerBL = new CustomerBL(executionContext, rowCustomerDTO);
                                customerBL.Save(parafaitDBTrx.SQLTrx);
                                parafaitDBTrx.EndTransaction();
                                importedCustomerCount++;
                                responseSheet.AddRow(sheet[i]);
                                responseSheet[responseSheet.Rows.Count - 1].AddCell(new Cell(MessageContainerList.GetMessage(executionContext, "SUCCESS")));
                            }
                            catch (Exception ex)
                            {
                                parafaitDBTrx.RollBack();
                                throw ex;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while importing customers", ex);
                    log.LogVariableState("Row", sheet[i]);
                    if (errorSheet.Rows.Count == 0)
                    {
                        errorSheet.AddRow(sheet[0]);
                        errorSheet[0].AddCell(new Cell(MessageContainerList.GetMessage(executionContext, "Errors")));
                    }
                    errorSheet.AddRow(sheet[i]);
                    string errorMessage = "";
                    string seperator = "";
                    if (ex is ValidationException)
                    {
                        foreach (var validationError in (ex as ValidationException).ValidationErrorList)
                        {
                            errorMessage += seperator;
                            errorMessage += validationError.Message;
                            seperator = ", ";
                        }
                    }
                    else
                    {
                        errorMessage = ex.Message;
                    }
                    errorSheet[errorSheet.Rows.Count - 1].AddCell(new Cell(errorMessage));
                    errorCustomerCount++;
                }

            }
            log.LogMethodExit(errorSheet);
            return new Tuple<Sheet, int>(errorSheet, importedCustomerCount);
        }

        private void Build(List<CustomerDTO> customerDTOList, bool activeChildRecords = true, bool loadSignedWaivers = false, SqlTransaction sqlTransaction = null, bool loadSignedWaiverFileContent = false, Utilities utilities = null, bool buildActiveCampaignActivity = false, bool buildLastVistitedDate = false, DateTime? fromDate = null, DateTime? toDate = null)
        {
            log.LogMethodEntry("customerDTOList", activeChildRecords, loadSignedWaivers, sqlTransaction, loadSignedWaiverFileContent);
            if (customerDTOList != null && customerDTOList.Count > 0)
            {
                List<int> profileIdList = new List<int>();
                List<int> customDataSetIdList = new List<int>();
                Dictionary<int, CustomerDTO> profileIdCustomerDictionary = new Dictionary<int, CustomerDTO>();
                Dictionary<int, CustomerDTO> customDataSetIdCustomerDictionary = new Dictionary<int, CustomerDTO>();
                for (int i = 0; i < customerDTOList.Count; i++)
                {
                    if (profileIdCustomerDictionary.ContainsKey(customerDTOList[i].ProfileId))
                    {
                        continue;
                    }
                    if (customDataSetIdCustomerDictionary.ContainsKey(customerDTOList[i].CustomDataSetId))
                    {
                        continue;
                    }
                    if (customerDTOList[i].ProfileId != -1)
                    {
                        profileIdCustomerDictionary.Add(customerDTOList[i].ProfileId, customerDTOList[i]);
                    }

                    if (customerDTOList[i].CustomDataSetId != -1)
                    {
                        customDataSetIdCustomerDictionary.Add(customerDTOList[i].CustomDataSetId, customerDTOList[i]);
                    }
                    profileIdList.Add(customerDTOList[i].ProfileId);
                    customDataSetIdList.Add(customerDTOList[i].CustomDataSetId);
                }
                ProfileListBL profileListBL = new ProfileListBL(executionContext);
                List<ProfileDTO> profileDTOList = profileListBL.GetProfileDTOList(profileIdList, true, activeChildRecords, sqlTransaction);
                if (profileDTOList != null)
                {
                    foreach (var profileDTO in profileDTOList)
                    {
                        if (profileIdCustomerDictionary.ContainsKey(profileDTO.Id))
                        {
                            profileIdCustomerDictionary[profileDTO.Id].ProfileDTO = profileDTO;
                        }
                    }
                }

                CustomDataSetListBL customDataSetListBL = new CustomDataSetListBL(executionContext);
                List<CustomDataSetDTO> customDataSetDTOList = customDataSetListBL.GetCustomDataSetDTOList(customDataSetIdList, true, activeChildRecords, sqlTransaction);
                if (customDataSetDTOList != null)
                {
                    foreach (var customDataSetDTO in customDataSetDTOList)
                    {
                        if (customDataSetIdCustomerDictionary.ContainsKey(customDataSetDTO.CustomDataSetId))
                        {
                            customDataSetIdCustomerDictionary[customDataSetDTO.CustomDataSetId].CustomDataSetDTO = customDataSetDTO;
                        }
                    }
                }

                if (buildLastVistitedDate || buildActiveCampaignActivity)
                {
                    foreach (CustomerDTO customerDTO in customerDTOList)
                    {
                        String accountIdList = "";
                        AccountListBL accountListBL = new AccountListBL(executionContext);
                        List<KeyValuePair<AccountDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AccountDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.CUSTOMER_ID, customerDTO.Id.ToString()));
                        if (activeChildRecords)
                        {
                            searchParameters.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.VALID_FLAG, "Y"));
                        }
                        List<AccountDTO> accountList = accountListBL.GetAccountDTOList(searchParameters, false, true, null);
                        if (accountList != null && accountList.Any())
                        {
                            foreach (AccountDTO account in accountList)
                            {
                                if (accountIdList.Length > 0)
                                    accountIdList += ",";
                                accountIdList += account.AccountId;
                            }

                            // this build the last visited date based on the card activity. Also chec from customer activity view
                            accountList = accountList.OrderByDescending(x => x.LastUpdateDate).ToList();
                            customerDTO.LastVisitedDate = accountList[0].LastUpdateDate;
                        }

                        if (buildActiveCampaignActivity || buildLastVistitedDate)
                        {
                            ActiveCampaignCustomerInfoListBL activeCampaignListBL = new ActiveCampaignCustomerInfoListBL(executionContext);
                            DateTime startDate = Convert.ToDateTime(fromDate);
                            DateTime endDate = Convert.ToDateTime(toDate);
                            List<ActiveCampaignCustomerInfoDTO> customerActivityDTOList = activeCampaignListBL.BuildCustomerActivity(customerDTO.Id, accountIdList, startDate, endDate, sqlTransaction);
                            if (customerActivityDTOList != null && customerActivityDTOList.Any())
                            {
                                customerDTO.ActiveCampaignCustomerInfoDTOList = customerActivityDTOList;

                                if (buildLastVistitedDate && customerActivityDTOList[0].LastPurchasedDate != null)
                                    customerDTO.LastVisitedDate = customerDTO.LastVisitedDate < customerActivityDTOList[0].LastPurchasedDate ? customerActivityDTOList[0].LastPurchasedDate : customerDTO.LastVisitedDate;
                            }
                        }
                    }
                }

                if (loadSignedWaivers)
                {
                    CustomerSignedWaiverListBL customerSignedWaiverListBL = new CustomerSignedWaiverListBL(executionContext);
                    List<int> custIdList = new List<int>();
                    custIdList = customerDTOList.Select(cust => cust.Id).ToList();
                    if (custIdList != null && custIdList.Any())
                    {
                        List<CustomerSignedWaiverDTO> customerSignedWaiverDTOList = customerSignedWaiverListBL.GetAllCustomerSignedWaiverList(custIdList, true, loadSignedWaiverFileContent, utilities, sqlTransaction);
                        if (customerSignedWaiverDTOList != null && customerSignedWaiverDTOList.Any())
                        {
                            for (int i = 0; i < customerDTOList.Count; i++)
                            {
                                customerDTOList[i].CustomerSignedWaiverDTOList = customerSignedWaiverDTOList.Where(csw => csw.SignedBy == customerDTOList[i].Id
                                                                                                                         || csw.SignedFor == customerDTOList[i].Id).ToList();
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// LoadAccountDetails
        /// </summary>
        /// <param name="customerDTOList"></param>
        /// <param name="batchIdList"></param>
        public List<CustomerDTO> LoadAccountDetails(List<CustomerDTO> customerDTOList, List<int> batchIdList)
        {
            log.LogMethodEntry("customerDTOList", batchIdList);
            if (customerDTOList != null && customerDTOList.Any())
            {
                AccountListBL accountListBL = new AccountListBL(executionContext);
                List<AccountDTO> accountDTOList = accountListBL.GetAccountDTOListByCustomerIds(batchIdList, true, true);
                if (accountDTOList != null && accountDTOList.Any())
                {
                    for (int i = 0; i < customerDTOList.Count; i++)
                    {
                        customerDTOList[i].AccountDTOList = accountDTOList.Where(acc => acc.CustomerId == customerDTOList[i].Id).ToList();
                    }
                }
            }
            log.LogMethodExit();
            return customerDTOList;
        }
        /// <summary>
        /// LoadCustomerMembershipRewardLogs
        /// </summary>
        /// <param name="customerDTOList"></param>
        /// <param name="batchIdList"></param>
        public List<CustomerDTO> LoadCustomerMembershipRewardLogs(List<CustomerDTO> customerDTOList, List<int> batchIdList)
        {
            log.LogMethodEntry("customerDTOList", batchIdList);
            if (customerDTOList != null && customerDTOList.Any())
            {
                CustomerMembershipRewardsLogList custRewardListBL = new CustomerMembershipRewardsLogList(executionContext);
                List<CustomerMembershipRewardsLogDTO> rewardDTOList = custRewardListBL.GetCustomerMembershipRewardsLogsByCustomerIds(batchIdList);
                if (rewardDTOList != null && rewardDTOList.Any())
                {
                    for (int i = 0; i < customerDTOList.Count; i++)
                    {
                        customerDTOList[i].CustomerMembershipRewardsLogDTOList = rewardDTOList.Where(acc => acc.CustomerId == customerDTOList[i].Id).ToList();
                    }
                }
            }
            log.LogMethodExit();
            return customerDTOList;
        }
        /// <summary>
        /// LoadCustomerMembershipProgressRecords
        /// </summary>
        /// <param name="customerDTOList"></param>
        /// <param name="batchIdList"></param>
        public List<CustomerDTO> LoadCustomerMembershipProgressRecords(List<CustomerDTO> customerDTOList, List<int> batchIdList)
        {
            log.LogMethodEntry("customerDTOList", batchIdList);
            if (customerDTOList != null && customerDTOList.Any())
            {
                CustomerMembershipProgressionList custMembershipProgressionListBL = new CustomerMembershipProgressionList(executionContext);
                List<CustomerMembershipProgressionDTO> progressionDTOList = custMembershipProgressionListBL.GetCustomerMembershipProgressionByCustomerIds(batchIdList);
                if (progressionDTOList != null && progressionDTOList.Any())
                {
                    for (int i = 0; i < customerDTOList.Count; i++)
                    {
                        customerDTOList[i].CustomerMembershipProgressionDTOList = progressionDTOList.Where(acc => acc.CustomerId == customerDTOList[i].Id).ToList();
                    }
                }
            }
            log.LogMethodExit();
            return customerDTOList;
        }
    }
}
