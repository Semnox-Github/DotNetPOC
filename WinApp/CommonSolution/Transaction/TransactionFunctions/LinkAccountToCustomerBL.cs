/********************************************************************************************
 * Project Name - AccountService
 * Description  - Link Account To Customer
 * 
 **************
 **Version Log
 **************
 *Version    Date          Modified By            Remarks          
 *********************************************************************************************
 *2.80.0     12-Mar-2020   Girish Kundar          Created
 *2.100.0    07-Oct-2020   Mathew Ninan           Added logic to create autogen card when Customer
 *                                                is created and load bonus has auto gen enabled
 *2.130.4     22-Feb-2022   Mathew Ninan    Modified DateTime to ServerDateTime 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Site;
using Semnox.Parafait.Product;
using Semnox.Parafait.User;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    ///  Link Account To Customer BL
    /// </summary>
    public class LinkAccountToCustomerBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private Utilities utilities;
        private AccountServiceDTO accountServiceDTO;

        private LinkAccountToCustomerBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;

            Users user = new Users(executionContext, executionContext.GetUserId());
            utilities = new Utilities();
            utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
            utilities.ParafaitEnv.User_Id = user.UserDTO.UserId;
            utilities.ParafaitEnv.LoginID = user.UserDTO.LoginId;
            utilities.ParafaitEnv.RoleId = user.UserDTO.RoleId;
            utilities.ParafaitEnv.SetPOSMachine("", Environment.MachineName);
            utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
            utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
            utilities.ExecutionContext.SetIsCorporate(executionContext.GetIsCorporate());
            utilities.ExecutionContext.SetSiteId(executionContext.GetSiteId());
            utilities.ExecutionContext.SetUserId(user.UserDTO.LoginId);
            utilities.ParafaitEnv.Initialize();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="accountServiceDTO"></param>
        public LinkAccountToCustomerBL(ExecutionContext executionContext, AccountServiceDTO accountServiceDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, accountServiceDTO);
            this.accountServiceDTO = accountServiceDTO;
            log.LogMethodExit();
        }


        /// <summary>
        /// Validate the AccountDTO for links to customer
        /// </summary>
        /// <returns></returns>
        public bool CanAccountLinkToCustomer()
        {
            log.LogMethodEntry();
            AccountDTO accountDTO = accountServiceDTO.SourceAccountDTO;
            string message = string.Empty;
            if (accountDTO.AccountId >= 0)
            {
                AccountDataHandler accountDataHandler = new AccountDataHandler(null);
                if (accountDataHandler.GetLastAccountUpdateDateTime(accountDTO.AccountId) > accountDTO.LastUpdateDate)
                {
                    message = MessageContainerList.GetMessage(executionContext, 547);
                    throw new ValidationException(message);
                }
            }

            bool isActive = accountDTO.ValidFlag && (accountDTO.ExpiryDate == null || accountDTO.ExpiryDate > ServerDateTime.Now);
            if (!isActive)
            {
                message = MessageContainerList.GetMessage(executionContext, 547);
                throw new ValidationException(message);
            }
            bool technicianAccount = (accountDTO.TechnicianCard == "Y" ? true : false);
            if (technicianAccount)
            {
                message = MessageContainerList.GetMessage(executionContext, 547);
                throw new ValidationException(message);
            }

            bool allowToRoam = true;
            if (accountDTO.SiteId != -1 && accountDTO.SiteId != executionContext.GetSiteId() && ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ALLOW_ROAMING_CARDS") == "N")
            {
                allowToRoam = false;
            }
            if (!allowToRoam)
            {
                message = MessageContainerList.GetMessage(executionContext, 547);
                throw new ValidationException(message);
            }
            log.LogMethodExit(true);
            return true;
        }


        /// <summary>
        /// Link account to customer
        /// </summary>
        public void LinkAccount(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            SqlConnection sqlConnection = null;
            SqlTransaction parafaitDBTrx;
            if (sqlTransaction == null)
            {
                sqlConnection = utilities.createConnection();
                parafaitDBTrx = sqlConnection.BeginTransaction();
            }
            else
            {
                parafaitDBTrx = sqlTransaction;
            }
            try
            {
                String message = "";
                bool linkCustomerToCard = false;
                if (accountServiceDTO.SourceAccountDTO == null
                && ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "REGISTRATION_BONUS_ON_VERIFICATION").Equals("N"))
                {
                    string strProdId = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "LOAD_PRODUCT_ON_REGISTRATION");
                    int productId = -1;
                    if (int.TryParse(strProdId, out productId) == true && productId != -1)
                    {
                        Products regProductBL = new Products(productId);
                        ProductsDTO regProductDTO = regProductBL.GetProductsDTO;
                        if (regProductDTO.AutoGenerateCardNumber.Equals("Y"))
                        {
                            Semnox.Core.GenericUtilities.RandomTagNumber randomCardNumber = new Semnox.Core.GenericUtilities.RandomTagNumber(utilities.ExecutionContext);
                            Card CurrentCard = new Card(randomCardNumber.Value, utilities.ParafaitEnv.LoginID, utilities);
                            CurrentCard.primaryCard = "Y"; //Assign auto gen card as primary card
                            try
                            {
                                CurrentCard.createCard(parafaitDBTrx);
                            }
                            catch (Exception ex)
                            {
                                parafaitDBTrx.Rollback();
                                throw ex;
                            }
                            accountServiceDTO.SourceAccountDTO = new AccountBL(utilities.ExecutionContext, CurrentCard.card_id, false, false, parafaitDBTrx).AccountDTO;
                        }
                    }
                }

                if (accountServiceDTO.SourceAccountDTO == null)
                {
                    log.Error("No source account found");
                    return;
                }

                if (accountServiceDTO.SourceAccountDTO.CustomerId > -1)
                {
                    AccountBL existingAccountBL = new AccountBL(utilities.ExecutionContext, accountServiceDTO.SourceAccountDTO.AccountId, false, false, parafaitDBTrx);
                    if (existingAccountBL.AccountDTO.CustomerId == -1)
                    {
                        linkCustomerToCard = true;
                        log.Debug("link card to customer:" + linkCustomerToCard);
                        AccountBL accountBL = new AccountBL(utilities.ExecutionContext, accountServiceDTO.SourceAccountDTO);
                        accountBL.Save(parafaitDBTrx);
                    }
                    else if (existingAccountBL.AccountDTO.CustomerId != accountServiceDTO.CustomerDTO.Id)
                    {
                        log.Debug("The card is already linked to customer with Id :" + accountServiceDTO.CustomerDTO.Id);
                        throw new Exception("The card is already linked to customer with Id: " + accountServiceDTO.CustomerDTO.Id);
                    }
                }
                else  // Not having customer information
                {
                    AccountBL accountBL = new AccountBL(utilities.ExecutionContext, accountServiceDTO.SourceAccountDTO.AccountId, false, false, parafaitDBTrx);
                    accountServiceDTO.SourceAccountDTO = accountBL.AccountDTO;
                    //accountBL.Save(parafaitDBTrx); // this to change the last updated time .
                    log.Debug("check if card can be linked to customer:" + linkCustomerToCard);
                    bool canLinkAccount = CanAccountLinkToCustomer();
                    if (canLinkAccount)
                    {
                        log.Debug("linking card to customer:" + linkCustomerToCard);

                        AccountListBL accountListBL = new AccountListBL(utilities.ExecutionContext);
                        List<KeyValuePair<AccountDTO.SearchByParameters, string>> accountSearchParameters = new List<KeyValuePair<AccountDTO.SearchByParameters, string>>();
                        accountSearchParameters.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.CUSTOMER_ID, accountServiceDTO.CustomerDTO.Id.ToString()));
                        List<AccountDTO> accountDTOList = accountListBL.GetAccountDTOList(accountSearchParameters, true, true);
                        if (accountDTOList == null || accountDTOList.Count == 0)
                        {
                            log.Debug("Loading registration bonus for " + accountBL.AccountDTO.TagNumber);
                            accountBL.AccountDTO.CustomerId = accountServiceDTO.CustomerDTO.Id;
                            accountBL.Save(parafaitDBTrx);
                            Transaction transaction = new Transaction(utilities);
                            transaction.LoadRegistrationBonus(accountServiceDTO.CustomerDTO.Id, accountBL.AccountDTO.AccountId, parafaitDBTrx, Environment.MachineName);
                        }
                        else
                        {
                            Semnox.Core.Utilities.EventLog eventLog = new Semnox.Core.Utilities.EventLog(utilities);
                            eventLog.logEvent("Customer", 'D', "Registration Bonus", "Customer Id: " + accountBL.AccountDTO.CustomerId + " is given bonus already. So no bonus is given to Card Id: " + accountBL.AccountDTO.AccountId, "RegistrationBonus", 0, "", accountBL.AccountDTO.CustomerId.ToString(), null);
                            accountBL.AccountDTO.CustomerId = accountServiceDTO.CustomerDTO.Id;
                            accountBL.Save(parafaitDBTrx);
                            log.LogMethodExit("Registration bonus is not given as the customer is already registered against a card");
                        }

                        //CustomerActivityUserLogDTO customerActivityUserLogDTO = new CustomerActivityUserLogDTO(-1, accountServiceDTO.CustomerDTO.Id, Guid.NewGuid().ToString(), "LINKCARD", "Linked account " + accountServiceDTO.SourceAccountDTO.TagNumber + " to customer ", DateTime.Now);
                        //CustomerActivityUserLogBL customerActivityUserLogBL = new CustomerActivityUserLogBL(utilities.ExecutionContext, customerActivityUserLogDTO);
                        //customerActivityUserLogBL.Save(parafaitDBTrx);
                        CustomerBL customerBL = new CustomerBL(executionContext, accountServiceDTO.CustomerDTO.Id, true, true, parafaitDBTrx);
                        AccountBL accountsBL = new AccountBL(executionContext, accountServiceDTO.SourceAccountDTO.AccountId, true, true, parafaitDBTrx);
                        LinkCustomerAccountEventBL linkCustomerAccountEventBL = new LinkCustomerAccountEventBL(executionContext, customerBL.CustomerDTO, accountsBL.AccountDTO, parafaitDBTrx);
                        if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "SEND_CUSTOMER_REGISTRATION_EMAIL") &&
                            !String.IsNullOrEmpty(customerBL.CustomerDTO.Email))
                        {
                            linkCustomerAccountEventBL.SendMessage(MessagingClientDTO.MessagingChanelType.EMAIL, parafaitDBTrx);

                        }

                        if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "SEND_CUSTOMER_REGISTRATION_SMS") &&
                            !String.IsNullOrEmpty(customerBL.CustomerDTO.PhoneNumber))
                        {
                            linkCustomerAccountEventBL.SendMessage(MessagingClientDTO.MessagingChanelType.SMS, parafaitDBTrx);


                        }
                    }
                }
                if (sqlTransaction == null)
                {
                    parafaitDBTrx.Commit();
                    sqlConnection.Close();
                }
            }

            catch (Exception ex)
            {
                if (sqlTransaction == null)  //SQLTransaction handled locally
                {
                    parafaitDBTrx.Rollback();
                }
                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                }
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
