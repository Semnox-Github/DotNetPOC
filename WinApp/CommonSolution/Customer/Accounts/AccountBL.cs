/************************************************************************************************************************************
 * Project Name - AccountBL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 *************************************************************************************************************************************
 *1.0.0       20-May-2017       Lakshminarayan     Created
 *2.4.0       09-Sep-2018       Guru S A           Modified to Redemption kiosk, Pause Allowed changes
 *2.60        20-Feb-2019       Mushahid Faizan    Added SaveUpdateAccountList() & AccountListBL() parameterized Constructor.
 *            13-Mar-2019       Raghuveera         Added new method to CardHasCreditPlus of the modified type or not
 *2.60.2      22-May-2019       Jagan Mohana       Added Transaction to SaveUpdateAccountList().
 *2.60.2      27-May-2019       Nitin Pai          Added new method to transfer CR entitlements to other cards
 *2.70.2.     06-Nov-2019       Jinto Thomas       Modified the condition to check canPauseRunningCPTime
 *2.70.2      15-Oct-2019       Nitin Pai          Gateway Cleanup
 *2.70.3      04-Feb-2020       Nitin Pai          Guest App phase 2 changes & transfer balance using account changes
 *2.80.0      19-Mar-2020       Jinto Thomas       AccountDTO chnages for newly added column SourceCreditPlusId
 *2.80.0      19-Mar-2020       Mathew NInan       Constructor change to pass validityStatus field 
 *2.80.0      13-Apr-2020       Deeksha            Split product entitlement for product type Recharge/Cardsale/Gametime
 *2.80.0      23-Jun-2020       Deeksha            Issue Fix : Miami time play can not cancel line 
 *2.90.0      30-Jun-2020       Girish Kundar      Modified : Moved the methods which acts on multiple accounts to new BL classes 
 *2.90.0      19-Aug-2020       Nitin              Issue Fix : Total credits should include purchase credits, added new method to get balance for Purchase,
 *                                                 Fixed the concurrency check in balance transfer
 *2.90        03-July-2020      Girish Kundar      Modified : Change as part of CardCodeDTOList replaced with AccountDTOList in CustomerDTO                                                 
 *2.100.0     20-Nov-2020       Mathew Ninan       Added GetAccountQRCode method to generate encrypted QR code string based on account number 
 *2.110.0     10-Dec-2020       Guru S A           For Subscription changes                   
 *2.120.0     18-Mar-2021       Guru S A           For Subscription phase 2 changes                   
 *2.120.0     09-Apr-2021       Amitha Joy         POS UI Task changes                  * 
 *2.130.0     24-April-2021     Fiona              POS UI Task changes 
 *2.130.0     07-Jul-2021       Prashanth          Modified UpdateTimeStatus(), Modified IsCreditPlusTimeRunning(), Added CheckTimePauseLimit() for Pause time task * 
*2.130.0     19-July-2021      Girish Kundar      Modified : Virtual point column added part of Arcade changes
*2.140.0     23-July-2021      Prashanth          Modified : Added RedeemLoyalty and DeductFromCreditPlusRecord methods
 *2.140.0     12-Dec-2021      Guru S A           Booking execute process performance fixes
*2.150.0     14-Dec-2021       Deeksha            Modified : As part of Transfer balance enhancements
*2.130.10    08-Sep-2022      Nitin Pai            Added function to remove card to customer link. Added as part of customer delete enhancement.
 ************************************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.CardCore;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.DBSynch;
using Semnox.Parafait.Game;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;
using Semnox.Parafait.Site;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// Business logic for Account class.
    /// </summary>
    public class AccountBL
    {
        private AccountDTO accountDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of AccountBL class
        /// </summary>
        private AccountBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            accountDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the account id as the parameter
        /// Would fetch the account object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="loadChildRecords">whether to load the child records</param>
        /// <param name="activeChildRecords">whether to load only active child records</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public AccountBL(ExecutionContext executionContext, int id,
            bool loadChildRecords = true, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, loadChildRecords, activeChildRecords, sqlTransaction);
            AccountDataHandler accountDataHandler = new AccountDataHandler(sqlTransaction);
            accountDTO = accountDataHandler.GetAccountDTO(id);
            if (accountDTO != null && accountDTO.ValidFlag)
            {
                SiteContainerDTO siteContainerDTO = SiteContainerList.GetCurrentSiteContainerDTO(executionContext);
                bool refreshed = RefreshAccountFromHQ(executionContext, accountDTO);
                if (refreshed)
                {
                    accountDTO = accountDataHandler.GetAccountDTO(id);
                }
            }
            if (accountDTO != null && loadChildRecords)
            {
                AccountBuilderBL accountBuilderBL = new AccountBuilderBL(executionContext);
                accountBuilderBL.Build(accountDTO, activeChildRecords, sqlTransaction);
            }

            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the account number as the parameter
        /// Would fetch the account object from the database based on the number passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="accountNumber">accountNumber</param>
        /// <param name="loadChildRecords">whether to load the child records</param>
        /// <param name="activeChildRecords">whether to load only active child records</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public AccountBL(ExecutionContext executionContext, string accountNumber,
            bool loadChildRecords = true, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, accountNumber, loadChildRecords, activeChildRecords, sqlTransaction);
            //TagNumberParser tagNumberParser = new TagNumberParser(executionContext);
            //TagNumber tagNumber;
            //if (tagNumberParser.TryParse(accountNumber, out tagNumber) == false)
            //{
            //    string message = tagNumberParser.Validate(accountNumber);
            //    log.LogMethodExit(null, "Throwing Exception- " + message);
            //    throw new ValidationException(message);
            //}
            AccountDataHandler accountDataHandler = new AccountDataHandler(sqlTransaction);
            accountDTO = accountDataHandler.GetAccountDTO(accountNumber);
            if (accountDTO != null && loadChildRecords)
            {
                AccountBuilderBL accountBuilderBL = new AccountBuilderBL(executionContext);
                accountBuilderBL.Build(accountDTO, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the account number as the parameter
        /// Would fetch the account object from the database based on the number passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="accountNumber">accountNumber</param>
        /// <param name="tagSiteId">site id value stored in the tag.</param>
        /// <param name="loadChildRecords">whether to load the child records</param>
        /// <param name="activeChildRecords">whether to load only active child records</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public AccountBL(ExecutionContext executionContext, string accountNumber, int tagSiteId,
            bool loadChildRecords = true, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, accountNumber, tagSiteId, loadChildRecords, activeChildRecords, sqlTransaction);
            //TagNumberParser tagNumberParser = new TagNumberParser(executionContext);
            //TagNumber tagNumber;
            //if (tagNumberParser.TryParse(accountNumber, out tagNumber) == false)
            //{
            //    string message = tagNumberParser.Validate(accountNumber);
            //    log.LogMethodExit(null, "Throwing Exception- " + message);
            //    throw new ValidationException(message);
            //}
            AccountDataHandler accountDataHandler = new AccountDataHandler(sqlTransaction);
            accountDTO = accountDataHandler.GetAccountDTO(accountNumber);
            if (accountDTO == null)
            {
                accountDTO = CreateNewAccountDTO(accountNumber);
            }
            SiteContainerDTO siteContainerDTO = SiteContainerList.GetCurrentSiteContainerDTO(executionContext);
            if (RefreshAccountFromHQ(executionContext, accountDTO, tagSiteId))
            {
                accountDTO = accountDataHandler.GetAccountDTO(accountNumber);
                if (accountDTO == null)
                {
                    accountDTO = CreateNewAccountDTO(accountNumber);
                }
            }
            if (accountDTO != null && accountDTO.AccountId > -1 && loadChildRecords)
            {
                AccountBuilderBL accountBuilderBL = new AccountBuilderBL(executionContext);
                accountBuilderBL.Build(accountDTO, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        private AccountDTO CreateNewAccountDTO(string accountNumber)
        {
            log.LogMethodEntry();
            var accountDTO = new AccountDTO(-1, accountNumber, string.Empty, null, null, false, null, null, true, null, string.Empty,
                0, 0, 0, 0, -1, 0, false, false, false, null, null, "N", null, false, 0, -1, null, null, -1, null, string.Empty, false, -1, string.Empty);
            log.LogMethodExit(accountDTO);
            return accountDTO;
        }
        public void SetStaffCard(string cardNumber, SqlTransaction sql)
        {
            log.LogMethodEntry(cardNumber);
            accountDTO = new AccountDTO(-1, cardNumber, string.Empty, DateTime.Now, null, false, null, null, true, null, string.Empty,
                                0, 0, 0, 0, -1, 0, true, false, false, null, null, "Y", null, false, 0, -1, null, null, -1, null, string.Empty, false, -1, string.Empty);
            Save(sql);
            log.LogMethodExit();
        }

        public void RemoveStaffCard(SqlTransaction sql)
        {
            log.LogMethodEntry();
            accountDTO.ValidFlag = false;
            Save(sql);
            log.LogMethodExit();
        }

        internal static bool RefreshAccountFromHQ(ExecutionContext executionContext, AccountDTO accountDTO, int? tagSiteId = null)
        {
            log.LogMethodEntry(executionContext, accountDTO, tagSiteId);
            if (SiteContainerList.IsCorporate())
            {
                log.LogMethodExit(false, "It is HQ DB no refresh required.");
                return false;
            }
            SiteContainerDTO siteContainerDTO = SiteContainerList.GetCurrentSiteContainerDTO(executionContext);
            if (accountDTO.AccountId > -1 && (accountDTO.SiteId == -1 || accountDTO.SiteId == siteContainerDTO.SiteId))
            {
                log.LogMethodExit(false, "Account belongs to the local site.");
                return false;
            }
            if (accountDTO.SiteId > 0 && accountDTO.SiteId != siteContainerDTO.SiteId) // roaming card
            {
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "ALLOW_ROAMING_CARDS") == false)
                {
                    string message = MessageContainerList.GetMessage(executionContext, 133);
                    log.LogMethodExit(null, "Throwing Exception- " + message);
                    throw new ValidationException(message);
                }

                if (RoamingSiteMasterList.IsRoamingSite(accountDTO.SiteId)) // auto roaming zone
                {
                    log.LogMethodExit(false, "auto roaming zone");
                    return false;
                }

                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "ENABLE_ON_DEMAND_ROAMING") != true ||
                    ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "AUTOMATIC_ON_DEMAND_ROAMING") != true)
                {
                    string message = MessageContainerList.GetMessage(executionContext, 196, accountDTO.SiteId);
                    log.LogMethodExit(null, "Throwing Exception- " + message);
                    throw new ValidationException(message);
                }

                int refreshFromHQThreshold = -1 * ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ROAMING_CARD_HQ_REFRESH_THRESHOLD", 60);
                DateTime serverDateTime = ServerDateTime.Now;
                if (accountDTO.RefreshFromHQTime > serverDateTime.AddMinutes(refreshFromHQThreshold))
                {
                    log.LogMethodExit(false, "Account has been refreshed from the server on -" + accountDTO.RefreshFromHQTime + " next refresh window -" + serverDateTime.AddMinutes(refreshFromHQThreshold));
                    return false;
                }
            }
            else if (accountDTO.AccountId == -1) // card not in local DB. card could be NEW or roaming from other site. 17-Mar-2016
            {
                if (tagSiteId.HasValue && tagSiteId.Value == siteContainerDTO.SiteId)
                {
                    log.LogMethodExit(false, "new tag and tag site id is same as the local site id");
                    return false;
                }
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "ENABLE_ON_DEMAND_ROAMING") != true ||
                    ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "AUTOMATIC_ON_DEMAND_ROAMING") != true)
                {
                    log.LogMethodExit(false, "new tag. not refreshing from HQ because ENABLE_ON_DEMAND_ROAMING and AUTOMATIC_ON_DEMAND_ROAMING disabled.");
                    return false;
                }
            }
            RemotingClient remotingClient = new RemotingClient();
            string Message = string.Empty;
            if (remotingClient.GetServerCard(accountDTO.TagNumber, siteContainerDTO.SiteId, ref Message) == "NOTFOUND")
            {
                log.Warn(accountDTO.TagNumber);
                log.Warn(MessageContainerList.GetMessage(executionContext, 283));
                log.LogMethodExit(false, "Account with tag number -" + accountDTO.TagNumber + " is not found in HQ server.");
                return false;
            }

            if (Message != "SUCCESS")
            {
                throw new Exception(Message);
            }
            log.LogMethodExit(true);
            return true;
        }

        /// <summary>
        /// Creates AccountBL object using the AccountDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="accountDTO">AccountDTO object</param>
        public AccountBL(ExecutionContext executionContext, AccountDTO accountDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, accountDTO);
            this.accountDTO = accountDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Checks whether Card Has Credit Plus
        /// </summary>
        /// <param name="creditPlusType">creditPlusType</param>
        /// <returns> True or false </returns>
        public bool CardHasCreditPlus(CreditPlusType creditPlusType)
        {
            log.LogMethodEntry(creditPlusType);
            if (accountDTO.AccountCreditPlusDTOList == null || (accountDTO.AccountCreditPlusDTOList != null && accountDTO.AccountCreditPlusDTOList.Count == 0))
            {
                log.LogMethodExit(false);
                return false;
            }
            else
            {
                AccountCreditPlusDTO accountCreditPlusDTO = accountDTO.AccountCreditPlusDTOList.Where((x) => x.IsActive && x.CreditPlusType.Equals(creditPlusType)).OrderByDescending((x) => x.LastUpdateDate).FirstOrDefault();
                if (accountCreditPlusDTO != null)
                {
                    log.LogMethodExit(true);
                    return true;
                }
            }
            log.LogMethodExit(false);
            return false;
        }

        /// <summary>
        /// Saves the Account
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            AccountDataHandler accountDataHandler = new AccountDataHandler(sqlTransaction);
            if (accountDTO.IsChangedRecursive)
            {
                if (accountDTO.ValidFlag)
                {
                    List<ValidationError> validationErrorList = Validate(sqlTransaction);
                    if (validationErrorList.Count > 0)
                    {
                        throw new ValidationException("Validation Failed", validationErrorList);
                    }
                }
                if (accountDTO.AccountId < 0)
                {
                    accountDTO = accountDataHandler.InsertAccount(accountDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    accountDTO.AcceptChanges();
                }
                else
                {
                    accountDTO = accountDataHandler.UpdateAccount(accountDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    accountDTO.AcceptChanges();
                }
                if (accountDTO.AccountRelationshipDTOList != null && accountDTO.AccountRelationshipDTOList.Any()) // Added to link the cards to one card
                {
                    foreach (var accountRelationshipDTO in accountDTO.AccountRelationshipDTOList)
                    {
                        if (accountRelationshipDTO.IsChanged || accountRelationshipDTO.AccountRelationshipId == -1)
                        {
                            if (accountRelationshipDTO.AccountId != accountDTO.AccountId)
                            {
                                accountRelationshipDTO.AccountId = accountDTO.AccountId;
                            }
                            AccountRelationshipBL accountRelationshipBL = new AccountRelationshipBL(executionContext, accountRelationshipDTO);
                            accountRelationshipBL.Save(sqlTransaction);
                        }
                    }
                }
                CreateRoamingData(sqlTransaction);
                if (accountDTO.AccountCreditPlusDTOList != null && accountDTO.AccountCreditPlusDTOList.Any())
                {
                    foreach (var accountCreditPlusDTO in accountDTO.AccountCreditPlusDTOList)
                    {
                        if (accountCreditPlusDTO.IsChangedRecursive || accountCreditPlusDTO.AccountCreditPlusId == -1)
                        {
                            if (accountCreditPlusDTO.AccountId != accountDTO.AccountId)
                            {
                                accountCreditPlusDTO.AccountId = accountDTO.AccountId;
                            }
                            AccountCreditPlusBL accountCreditPlusBL = new AccountCreditPlusBL(executionContext, accountCreditPlusDTO);
                            accountCreditPlusBL.Save(accountDTO.SiteId, sqlTransaction);
                        }
                    }
                }
                if (accountDTO.AccountDiscountDTOList != null && accountDTO.AccountDiscountDTOList.Any())
                {
                    foreach (var accountDiscountDTO in accountDTO.AccountDiscountDTOList)
                    {
                        if (accountDiscountDTO.IsChanged || accountDiscountDTO.AccountDiscountId == -1)
                        {
                            if (accountDiscountDTO.AccountId != accountDTO.AccountId)
                            {
                                accountDiscountDTO.AccountId = accountDTO.AccountId;
                            }
                            AccountDiscountBL accountDiscountBL = new AccountDiscountBL(executionContext, accountDiscountDTO);
                            accountDiscountBL.Save(accountDTO.SiteId, sqlTransaction);
                        }
                    }
                }
                if (accountDTO.AccountGameDTOList != null && accountDTO.AccountGameDTOList.Any())
                {
                    foreach (var accountGameDTO in accountDTO.AccountGameDTOList)
                    {
                        if (accountGameDTO.IsChangedRecursive || accountGameDTO.AccountGameId == -1)
                        {
                            if (accountGameDTO.AccountId != accountDTO.AccountId)
                            {
                                accountGameDTO.AccountId = accountDTO.AccountId;
                            }
                            AccountGameBL accountGameBL = new AccountGameBL(executionContext, accountGameDTO);
                            accountGameBL.Save(accountDTO.SiteId, sqlTransaction);
                        }
                    }
                }
                AccountSummaryBL accountSummaryBL = new AccountSummaryBL(executionContext, accountDTO);
                accountDTO.AccountSummaryDTO = accountSummaryBL.AccountSummaryDTO;
            }

            log.LogMethodExit();
        }

        /// <summary>
        /// This method should be used when the account is changed manually through
        /// management studio. This method has extra validations and creates account audit 
        /// records for future reference.
        /// </summary>
        /// <param name="savePreviousAccountStateInAccountAudit"></param>
        /// <param name="sqlTransaction"></param>
        public void SaveManualChanges(bool savePreviousAccountStateInAccountAudit, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(accountDTO, savePreviousAccountStateInAccountAudit);
            ValidateManualAccountChanges();
            AccountBL accountBL = null;
            if (savePreviousAccountStateInAccountAudit && accountDTO.AccountId >= 0)
            {
                accountBL = new AccountBL(executionContext, accountDTO.AccountId, false, false, sqlTransaction);
                CreateAccountAuditRecord(accountBL.AccountDTO, sqlTransaction);
            }

            accountBL = new AccountBL(executionContext, accountDTO);
            accountBL.Save(sqlTransaction);
            CreateAccountAuditRecord(accountDTO, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Validate Manual Account Changes
        /// </summary>
        private void ValidateManualAccountChanges()
        {
            log.LogMethodEntry(accountDTO);
            if (accountDTO.PrimaryAccount && (accountDTO.AccountId < 0 || accountDTO.CustomerId < 0))
            {
                throw new ValidationException("account needs to be linked with a customer before making it a primary account",
                    "Account", "PrimaryAccount", MessageContainerList.GetMessage(executionContext, 1595));
            }
            decimal limit = ParafaitDefaultContainerList.GetParafaitDefault<decimal>(executionContext, "MANUAL_CARD_UPDATE_ENT_LIMIT");
            if (limit > 0)
            {
                if (accountDTO.Credits > limit)
                {
                    throw new ValidationException("Max allowed value for credits / courtesy / bonus is " + limit.ToString(),
                    "Account", "Credits", MessageContainerList.GetMessage(executionContext, 1596, limit.ToString()));
                }
                if (accountDTO.Courtesy > limit)
                {
                    throw new ValidationException("Max allowed value for credits / courtesy / bonus is " + limit.ToString(),
                    "Account", "Courtesy", MessageContainerList.GetMessage(executionContext, 1596, limit.ToString()));
                }
                if (accountDTO.Bonus > limit)
                {
                    throw new ValidationException("Max allowed value for credits / courtesy / bonus is " + limit.ToString(),
                    "Account", "Bonus", MessageContainerList.GetMessage(executionContext, 1596, limit.ToString()));
                }
            }
            int techGameslimit = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "MANUAL_CARD_UPDATE_GAMES_LIMIT");
            if (techGameslimit > 0)
            {
                if (accountDTO.TechGames > techGameslimit)
                {
                    throw new ValidationException("Max allowed value for tech games is  " + techGameslimit.ToString(),
                    "Account", "Bonus", MessageContainerList.GetMessage(executionContext, 1597, techGameslimit.ToString()));
                }
            }
            /// Checking the Card number exist or not
            /// 
            if (AccountDTO.AccountId < 0)
            {
                AccountListBL accountListBL = new AccountListBL(executionContext);
                AccountSearchCriteria accountSearchCriteria = new AccountSearchCriteria(AccountDTO.SearchByParameters.TAG_NUMBER, Operator.EQUAL_TO, accountDTO.TagNumber);
                accountSearchCriteria.And(AccountDTO.SearchByParameters.VALID_FLAG, Operator.EQUAL_TO, "Y");
                List<AccountDTO> accountDTOList = accountListBL.GetAccountDTOList(accountSearchCriteria);
                if (accountDTOList != null && accountDTOList.Count > 0)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 682));
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Create Account Audit Record
        /// </summary>
        /// <param name="accountDTO">accountDTO</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        private void CreateAccountAuditRecord(AccountDTO accountDTO, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(accountDTO, sqlTransaction);
            AccountAuditDTO accountAuditDTO = new AccountAuditDTO(accountDTO);
            AccountAuditBL accountAuditBL = new AccountAuditBL(executionContext, accountAuditDTO);
            accountAuditBL.Save(sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Create Roaming Data
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        private void CreateRoamingData(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            AccountDataHandler accountDataHandler = new AccountDataHandler(sqlTransaction);
            AccountDTO updatedAccountDTO = accountDataHandler.GetAccountDTO(accountDTO.AccountId);
            DBSynchLogService dBSynchLogService = new DBSynchLogService(executionContext, "Cards", updatedAccountDTO.Guid, updatedAccountDTO.SiteId);
            dBSynchLogService.CreateRoamingData(sqlTransaction);
            log.LogMethodExit();
        }

        public void CreateAccountRoamingDataForTransaction(int transactionId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction, transactionId);
            bool onDemandRoamingEnabled = false;
            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ENABLE_ON_DEMAND_ROAMING").Equals("Y")
                        && ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AUTOMATIC_ON_DEMAND_ROAMING").Equals("Y")
                        && ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ALLOW_ROAMING_CARDS").Equals("Y")
                        )
            {
                onDemandRoamingEnabled = true;
            }
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);

            AccountDTO updatedAccountDTO = this.AccountDTO;

            DBSynchLogService dBSynchLogService = new DBSynchLogService(executionContext, "Cards", updatedAccountDTO.Guid, updatedAccountDTO.SiteId);
            dBSynchLogService.CreateRoamingData(sqlTransaction);

            if (onDemandRoamingEnabled)
            {
                //account site id does not match with transaction site id. This can happen in virtual store scenario
                if (updatedAccountDTO.SiteId != executionContext.GetSiteId())
                {
                    DBSynchLogDTO dbSynchLogDTO = new DBSynchLogDTO("I", updatedAccountDTO.Guid, "Cards", lookupValuesList.GetServerDateTime(), executionContext.GetSiteId());
                    DBSynchLogBL dbSynchLogBL = new DBSynchLogBL(executionContext, dbSynchLogDTO);
                    dbSynchLogBL.Save();
                }
            }

            if (updatedAccountDTO.AccountCreditPlusDTOList != null && updatedAccountDTO.AccountCreditPlusDTOList.Any())
            {
                updatedAccountDTO.AccountCreditPlusDTOList = updatedAccountDTO.AccountCreditPlusDTOList.Where(x => x.TransactionId == transactionId).ToList();

                if (updatedAccountDTO.AccountCreditPlusDTOList != null && updatedAccountDTO.AccountCreditPlusDTOList.Any())
                {
                    foreach (AccountCreditPlusDTO updatedAccountCreditPlus in updatedAccountDTO.AccountCreditPlusDTOList)
                    {
                        DBSynchLogService dBSynchLogServiceCP = new DBSynchLogService(executionContext, "CardCreditPlus", updatedAccountCreditPlus.Guid, updatedAccountCreditPlus.SiteId);
                        dBSynchLogServiceCP.CreateRoamingData();
                        if (onDemandRoamingEnabled)
                        {
                            DBSynchLogDTO dbSynchLogDTO = new DBSynchLogDTO("I", updatedAccountCreditPlus.Guid, "CardCreditPlus", lookupValuesList.GetServerDateTime(), updatedAccountDTO.SiteId);
                            DBSynchLogBL dbSynchLogBL = new DBSynchLogBL(executionContext, dbSynchLogDTO);
                            dbSynchLogBL.Save();
                        }

                        if (updatedAccountCreditPlus.AccountCreditPlusConsumptionDTOList != null)
                        {
                            foreach (AccountCreditPlusConsumptionDTO updatedAccountCPConsumpDTO in updatedAccountCreditPlus.AccountCreditPlusConsumptionDTOList)
                            {
                                DBSynchLogService dBSynchLogServiceCPConsp = new DBSynchLogService(executionContext, "CardCreditPlusConsumption", updatedAccountCPConsumpDTO.Guid, updatedAccountCPConsumpDTO.SiteId);
                                dBSynchLogServiceCPConsp.CreateRoamingData();
                                if (onDemandRoamingEnabled)
                                {
                                    DBSynchLogDTO dbSynchLogDTO = new DBSynchLogDTO("I", updatedAccountDTO.Guid, "CardCreditPlusConsumption", lookupValuesList.GetServerDateTime(), updatedAccountDTO.SiteId);
                                    DBSynchLogBL dbSynchLogBL = new DBSynchLogBL(executionContext, dbSynchLogDTO);
                                    dbSynchLogBL.Save();
                                }
                            }
                        }
                        if (updatedAccountCreditPlus.AccountCreditPlusPurchaseCriteriaDTOList != null)
                        {
                            foreach (AccountCreditPlusPurchaseCriteriaDTO updatedAccountCPPCDTO in updatedAccountCreditPlus.AccountCreditPlusPurchaseCriteriaDTOList)
                            {
                                DBSynchLogService dBSynchLogServiceCPPC = new DBSynchLogService(executionContext, "CardCreditPlusPurchaseCriteria", updatedAccountCPPCDTO.Guid, updatedAccountCPPCDTO.SiteId);
                                dBSynchLogServiceCPPC.CreateRoamingData();
                                if (onDemandRoamingEnabled)
                                {
                                    DBSynchLogDTO dbSynchLogDTOVs = new DBSynchLogDTO("I", updatedAccountDTO.Guid, "CardCreditPlusPurchaseCriteria", lookupValuesList.GetServerDateTime(), updatedAccountDTO.SiteId);
                                    DBSynchLogBL dbSynchLogBLVs = new DBSynchLogBL(executionContext, dbSynchLogDTOVs);
                                    dbSynchLogBLVs.Save();
                                }
                            }
                        }
                    }
                }
            }
            if (updatedAccountDTO.AccountGameDTOList != null && updatedAccountDTO.AccountGameDTOList.Any())
            {
                updatedAccountDTO.AccountGameDTOList = updatedAccountDTO.AccountGameDTOList.Where(x => x.TransactionId == transactionId).ToList();

                if (updatedAccountDTO.AccountGameDTOList != null && updatedAccountDTO.AccountGameDTOList.Any())
                {
                    foreach (AccountGameDTO updatedAccountGame in updatedAccountDTO.AccountGameDTOList)
                    {
                        DBSynchLogService dBSynchLogServiceGame = new DBSynchLogService(executionContext, "CardGames", updatedAccountGame.Guid, updatedAccountGame.SiteId);
                        dBSynchLogServiceGame.CreateRoamingData();
                        if (onDemandRoamingEnabled)
                        {
                            DBSynchLogDTO dbSynchLogDTOVs = new DBSynchLogDTO("I", updatedAccountDTO.Guid, "CardGames", lookupValuesList.GetServerDateTime(), updatedAccountDTO.SiteId);
                            DBSynchLogBL dbSynchLogBLVs = new DBSynchLogBL(executionContext, dbSynchLogDTOVs);
                            dbSynchLogBLVs.Save();
                        }

                        if (updatedAccountGame.AccountGameExtendedDTOList != null)
                        {
                            foreach (AccountGameExtendedDTO updatedAccountGameExtDTO in updatedAccountGame.AccountGameExtendedDTOList)
                            {
                                DBSynchLogService dBSynchLogServiceGameExt = new DBSynchLogService(executionContext, "CardGameExtended", updatedAccountGameExtDTO.Guid, updatedAccountGameExtDTO.SiteId);
                                dBSynchLogServiceGameExt.CreateRoamingData();

                                if (onDemandRoamingEnabled)
                                {
                                    DBSynchLogDTO dbSynchLogDTOVs = new DBSynchLogDTO("I", updatedAccountDTO.Guid, "CardGameExtended", lookupValuesList.GetServerDateTime(), updatedAccountDTO.SiteId);
                                    DBSynchLogBL dbSynchLogBLVs = new DBSynchLogBL(executionContext, dbSynchLogDTOVs);
                                    dbSynchLogBLVs.Save();
                                }
                            }
                        }
                    }
                }
            }
            if (updatedAccountDTO.AccountDiscountDTOList != null && updatedAccountDTO.AccountDiscountDTOList.Any())
            {
                updatedAccountDTO.AccountDiscountDTOList = updatedAccountDTO.AccountDiscountDTOList.Where(x => x.TransactionId == transactionId).ToList();

                if (updatedAccountDTO.AccountDiscountDTOList != null && updatedAccountDTO.AccountDiscountDTOList.Any())
                {
                    foreach (AccountDiscountDTO updatedAccountDiscount in updatedAccountDTO.AccountDiscountDTOList)
                    {
                        DBSynchLogService dBSynchLogServiceDiscount = new DBSynchLogService(executionContext, "CardDiscounts", updatedAccountDiscount.Guid, updatedAccountDiscount.SiteId);
                        dBSynchLogServiceDiscount.CreateRoamingData();

                        if (onDemandRoamingEnabled)
                        {
                            DBSynchLogDTO dbSynchLogDTOVs = new DBSynchLogDTO("I", updatedAccountDTO.Guid, "CardDiscounts", lookupValuesList.GetServerDateTime(), updatedAccountDTO.SiteId);
                            DBSynchLogBL dbSynchLogBLVs = new DBSynchLogBL(executionContext, dbSynchLogDTOVs);
                            dbSynchLogBLVs.Save();
                        }
                    }
                }
            }
            log.LogMethodExit();
        }


        public decimal GetAccountTimeBalance()
        {
            log.LogMethodEntry();
            decimal result = 0;
            if (accountDTO.Time.HasValue == false)
            {
                log.LogMethodExit(result, "Time is null");
                return result;
            }
            if (accountDTO.StartTime.HasValue == false)
            {
                result = accountDTO.Time.Value;
                log.LogMethodExit(result, "Start time is null");
                return result;
            }
            DateTime expiryDate = accountDTO.StartTime.Value.AddMinutes((double)accountDTO.Time.Value);
            result = (decimal)(((expiryDate) - ServerDateTime.Now).TotalMinutes);
            if (result < 0)
            {
                result = 0;
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public AccountDTO AccountDTO
        {
            get
            {
                return accountDTO;
            }
        }

        /// <summary>
        /// Validates the customer relationship DTO
        /// </summary>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (accountDTO.AccountId >= 0)
            {
                AccountDataHandler accountDataHandler = new AccountDataHandler(sqlTransaction);
                if (accountDataHandler.GetLastAccountUpdateDateTime(accountDTO.AccountId) > accountDTO.LastUpdateDate)
                {
                    throw new ValidationException("Card has been updated by other processes after you have refreshed. Please reload before saving changes", "Account", "", MessageContainerList.GetMessage(executionContext, 547));
                }
            }
            if (accountDTO.IsChanged)
            {
                if (string.IsNullOrWhiteSpace(accountDTO.TagNumber))
                {
                    validationErrorList.Add(new ValidationError("Account", "TagNumber", MessageContainerList.GetMessage(executionContext, 1594)));
                }
            }
            if (accountDTO.AccountCreditPlusDTOList != null)
            {
                foreach (var accountCreditPlusDTO in accountDTO.AccountCreditPlusDTOList)
                {
                    if (accountCreditPlusDTO.IsChangedRecursive)
                    {
                        AccountCreditPlusBL accountCreditPlusBL = new AccountCreditPlusBL(executionContext, accountCreditPlusDTO);
                        validationErrorList.AddRange(accountCreditPlusBL.Validate(sqlTransaction));
                    }
                }
            }
            if (accountDTO.AccountDiscountDTOList != null)
            {
                foreach (var accountDiscountDTO in accountDTO.AccountDiscountDTOList)
                {
                    if (accountDiscountDTO.IsChanged)
                    {
                        AccountDiscountBL accountDiscountBL = new AccountDiscountBL(executionContext, accountDiscountDTO);
                        validationErrorList.AddRange(accountDiscountBL.Validate(sqlTransaction));
                    }
                }
            }
            if (accountDTO.AccountGameDTOList != null)
            {
                foreach (var accountGameDTO in accountDTO.AccountGameDTOList)
                {
                    if (accountGameDTO.IsChangedRecursive)
                    {
                        AccountGameBL accountGameBL = new AccountGameBL(executionContext, accountGameDTO);
                        validationErrorList.AddRange(accountGameBL.Validate(sqlTransaction));
                    }
                }
            }
            if (accountDTO.AccountDiscountDTOList != null && accountDTO.AccountDiscountDTOList.Count > 1)
            {
                bool duplicateDiscountEntryFound = false;
                foreach (var currentAccountDiscountDTO in accountDTO.AccountDiscountDTOList)
                {
                    if (duplicateDiscountEntryFound == false && currentAccountDiscountDTO.IsActive &&
                       (currentAccountDiscountDTO.ExpiryDate.HasValue == false || currentAccountDiscountDTO.ExpiryDate.Value > ServerDateTime.Now))
                    {
                        foreach (var otherAccountDiscountDTO in accountDTO.AccountDiscountDTOList)
                        {
                            if (otherAccountDiscountDTO.IsActive &&
                                 (otherAccountDiscountDTO.ExpiryDate.HasValue == false || otherAccountDiscountDTO.ExpiryDate.Value > ServerDateTime.Now) &&
                                 otherAccountDiscountDTO.AccountDiscountId != currentAccountDiscountDTO.AccountDiscountId &&
                                 otherAccountDiscountDTO.DiscountId == currentAccountDiscountDTO.DiscountId)
                            {
                                duplicateDiscountEntryFound = true;
                                break;
                            }
                        }
                    }
                }
                if (duplicateDiscountEntryFound)
                {
                    validationErrorList.Add(new ValidationError("AccountDiscount", "DiscountId", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Discount Name"))));
                }
            }

            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Deactivates the finger print based on the userId
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public bool DeactivateFingerprint(string userId, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(userId, sqlTransaction);
            bool result = false;
            AccountDataHandler accountDataHandler = new AccountDataHandler(sqlTransaction);
            result = accountDataHandler.DeactivateFingerprint(accountDTO.AccountId, userId);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Method to check concurrency issue for an account
        /// </summary>
        /// <param name="currentUpdateTime"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns>True or false</returns>
        public bool IsAccountUpdatedByOthers(DateTime currentUpdateTime, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(currentUpdateTime, sqlTransaction, accountDTO);
            AccountDataHandler accountDataHandler = new AccountDataHandler(sqlTransaction);
            DateTime dbUpdateTime = accountDataHandler.GetLastAccountUpdateDateTime(accountDTO.AccountId);
            log.LogVariableState("dbUpdateTime", dbUpdateTime);
            if (accountDTO != null && dbUpdateTime > currentUpdateTime)
            {
                log.LogMethodExit(true);
                return true;
            }
            else
            {
                log.LogMethodExit(false);
                return false;
            }
        }
        /// <summary>
        /// Method to check concurrency issue for an account
        /// </summary>
        /// <param name="currentUpdateTime"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns>True or false</returns>
        public bool IsAccountUpdatedByOtherTransactions(DateTime currentUpdateTime, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(currentUpdateTime, sqlTransaction, accountDTO);
            AccountDataHandler accountDataHandler = new AccountDataHandler(sqlTransaction);
            if (accountDTO != null && accountDataHandler.IsUpdatedByOthers(accountDTO.AccountId, currentUpdateTime))
            {
                log.LogMethodExit(true);
                return true;
            }
            else
            {
                log.LogMethodExit(false);
                return false;
            }
        }
        /// <summary>

        /// returns whether account is allowed to roam or not
        /// </summary>
        public bool AccountAllowedToRoam()
        {
            log.LogMethodEntry();
            bool allowToRoam = true;
            //Card is roaming but ALLOW_ROAMING_CARDS is not allowed. return false
            if (this.accountDTO.SiteId != -1 && this.accountDTO.SiteId != executionContext.GetSiteId() && ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ALLOW_ROAMING_CARDS") == "N")
            {
                allowToRoam = false;
            }
            log.LogMethodExit(allowToRoam);
            return allowToRoam;
        }

        /// <summary>
        /// returns whether account belongs to technician or not
        /// </summary>
        public bool IsTechnicianAccount()
        {
            log.LogMethodEntry();
            bool technicianAccount = (this.accountDTO.TechnicianCard == "Y" ? true : false);
            log.LogMethodExit(technicianAccount);
            return technicianAccount;
        }

        /// <summary>
        /// Get total tickets for the account
        /// </summary>
        public int? GetTotalTickets()
        {
            log.LogMethodEntry();
            if (this.AccountDTO.AccountSummaryDTO == null)
            {
                log.Error("Unable to GetTotalTickets AccountDTO.AccountCreditPlusSummaryDTO == null. Ensure to load required details while creating account object ");
                log.LogVariableState("this.AccountDTO ", this.AccountDTO);
                throw new Exception(MessageContainerList.GetMessage(executionContext, 1643));
            }
            int? totalTicketsForAccount = (this.AccountDTO.TicketCount == null ? 0 : this.AccountDTO.TicketCount) + Convert.ToInt32(this.AccountDTO.AccountSummaryDTO.CreditPlusTickets.HasValue ? this.AccountDTO.AccountSummaryDTO.CreditPlusTickets : 0);
            log.LogMethodExit(totalTicketsForAccount);
            return totalTicketsForAccount;
        }

        /// <summary>
        /// Get account Id
        /// </summary>
        public int GetAccountId()
        {
            log.LogMethodEntry();
            int idValue = (this.AccountDTO != null ? this.AccountDTO.AccountId : -1);
            log.LogMethodExit(idValue);
            return idValue;
        }

        /// <summary>
        /// Get Customer Id linked with the account
        /// </summary>
        public int GetCustomerId()
        {
            log.LogMethodEntry();
            log.LogMethodExit(this.AccountDTO.CustomerId);
            return this.AccountDTO.CustomerId;
        }


        /// <summary>
        /// Check whether account is active
        /// </summary>
        public bool IsActive()
        {
            log.LogMethodEntry();
            bool isActive = this.AccountDTO.ValidFlag && (this.AccountDTO.ExpiryDate == null || this.AccountDTO.ExpiryDate > DateTime.Now);
            log.LogMethodExit(isActive);
            return isActive;
        }

        /// <summary>
        /// Generate QR code for the account using account number, UTC time and site_id.
        /// This will be encrypted using ParafaitEncryption Key.
        /// Each property is separated by | to easy separate out during decryption.
        /// </summary>
        /// <param name="siteId">Site Id for which QR code is required</param>
        /// <param name="QRType">Is QR type G - Gameplay or T - Transaction</param>
        /// <returns>Encrypted QR String</returns>
        public string GetAccountQRCode(int siteId, AccountQRType accountQRType)
        {
            log.LogMethodEntry(siteId, accountQRType);
            string splitCharacter = "|";
            if (accountDTO == null || (accountDTO != null && accountDTO.AccountId <= -1))
                return string.Empty;
            string accountQRCode = string.Empty;
            //G/TCardNo|SiteIdUTC
            accountQRCode = string.Concat(AccountQRTypeConverter.ToString(accountQRType), splitCharacter);
            accountQRCode = string.Concat(accountQRCode, accountDTO.TagNumber, splitCharacter); //Account number might vary between 8/10 or 14 bytes based on KW, MiFare or ULC
            accountQRCode = string.Concat(accountQRCode,
                                            (siteId == -1 ? accountDTO.SiteId.ToString() : siteId.ToString()),
                                            splitCharacter);
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            DateTime currentServerTime = lookupValuesList.GetServerDateTime();
            String currentServerTimeUTC = currentServerTime.ToUniversalTime().ToString("yyyyMMddHHmmss");//get UTC time in 24 hour format
            accountQRCode = string.Concat(accountQRCode, currentServerTimeUTC);
            //Encrypt QR Code using ParafaitEncryption Key
            string strParafaitKey = Encryption.GetParafaitKeys("ParafaitEncryption");
            accountQRCode = Encryption.Encrypt(accountQRCode, strParafaitKey);

            log.LogMethodExit("QR Code is " + accountQRCode);
            return accountQRCode;
        }

        public bool IsChildAccount()
        {
            log.LogMethodEntry();
            bool result = false;
            if (accountDTO.AccountRelationshipDTOList != null &&
               accountDTO.AccountRelationshipDTOList.FirstOrDefault(x => x.RelatedAccountId == accountDTO.AccountId) != null)
            {
                result = true;
            }
            log.LogMethodExit(result);
            return result;
        }

        public int GetParentAccountId()
        {
            log.LogMethodEntry();
            int result = -1;
            if (IsChildAccount())
            {
                result = accountDTO.AccountRelationshipDTOList.FirstOrDefault(x => x.RelatedAccountId == accountDTO.AccountId).AccountId;
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Tells whether running credit plus time entry can be paused or not
        /// </summary>
        ///  <param name="currentTime"></param>
        /// <param name="sqlTrx"></param>
        /// <returns></returns>
        public bool CanPauseRunningCreditPlusTime(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            bool canPauseRunningCPTime = true;
            List<AccountCreditPlusDTO> accountCreditPlusDTOList;
            //Get CP to which pause is not allowed (PAUSE_ALLOWED, "0"/PauseAllowed == false)
            if (this.AccountDTO.AccountCreditPlusDTOList == null)
            {
                AccountCreditPlusListBL accountCreditPlusListBL = new AccountCreditPlusListBL(executionContext);
                List<KeyValuePair<AccountCreditPlusDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<AccountCreditPlusDTO.SearchByParameters, string>>();
                searchParam.Add(new KeyValuePair<AccountCreditPlusDTO.SearchByParameters, string>(AccountCreditPlusDTO.SearchByParameters.ACCOUNT_ID, this.AccountDTO.AccountId.ToString()));
                searchParam.Add(new KeyValuePair<AccountCreditPlusDTO.SearchByParameters, string>(AccountCreditPlusDTO.SearchByParameters.CREDITPLUS_TYPE, CreditPlusTypeConverter.ToString(CreditPlusType.TIME)));
                searchParam.Add(new KeyValuePair<AccountCreditPlusDTO.SearchByParameters, string>(AccountCreditPlusDTO.SearchByParameters.PAUSE_ALLOWED, "0"));
                accountCreditPlusDTOList = accountCreditPlusListBL.GetAccountCreditPlusDTOList(searchParam, false, true, sqlTrx);
            }
            else
            {
                accountCreditPlusDTOList = this.AccountDTO.AccountCreditPlusDTOList.Where(cpLine => (cpLine.CreditPlusType == CreditPlusType.TIME
                                                                                                     && cpLine.PauseAllowed == false)).ToList();
            }
            log.LogVariableState("accountCreditPlusDTOList", accountCreditPlusDTOList);
            if (accountCreditPlusDTOList != null && accountCreditPlusDTOList.Count > 0)
            {
                LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                DateTime currentTime = lookupValuesList.GetServerDateTime();
                //if CP to which pause is not allowed is in use then we can not allow pause of running CP
                if (accountCreditPlusDTOList.Exists(cpLine => (cpLine.PlayStartTime != null
                                                              && new AccountCreditPlusBL(executionContext, cpLine).GetCreditPlusTime(currentTime) > 0
                                                               && (cpLine.PeriodFrom == null || (cpLine.PeriodFrom != null && cpLine.PeriodFrom <= currentTime))
                                                               && (cpLine.PeriodTo == null || (cpLine.PeriodTo != null && cpLine.PeriodTo >= currentTime))
                                                            )))
                {
                    canPauseRunningCPTime = false;
                }
            }
            log.LogMethodExit(canPauseRunningCPTime);
            return canPauseRunningCPTime;
        }

        /// <summary>
        /// Check if current Card\Account can be linked to a customer
        /// </summary>
        public bool CanAccountLinkToCustomer(ref string message)
        {
            if (accountDTO.AccountId >= 0)
            {
                AccountDataHandler accountDataHandler = new AccountDataHandler(null);
                if (accountDataHandler.GetLastAccountUpdateDateTime(accountDTO.AccountId) > accountDTO.LastUpdateDate)
                {
                    message = MessageContainerList.GetMessage(executionContext, 547);
                    return false;
                }
            }

            // check if the account is valid
            if (!IsActive())
            {
                message = MessageContainerList.GetMessage(executionContext, 547);
                return false;
            }

            if (IsTechnicianAccount())
            {
                message = MessageContainerList.GetMessage(executionContext, 4132, accountDTO != null? accountDTO.TagNumber : string.Empty);
                return false;
            }

            // Removing this as SmartFun can be used in single sites also
            //if (!AccountAllowedToRoam())
            //{
            //    message = MessageContainerList.GetMessage(executionContext, 547);
            //    return false;
            //}
            return true;
        }

        public void RemoveCustomerLink(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            String message = "";
            AccountDataHandler accountDataHandler = new AccountDataHandler(null);
            if (accountDataHandler.GetLastAccountUpdateDateTime(accountDTO.AccountId) > accountDTO.LastUpdateDate)
            {
                message = MessageContainerList.GetMessage(executionContext, 547);
                throw new ValidationException(message);
            }

            if (IsTechnicianAccount())
            {
                message = "Cannot unlink a technician account";
                throw new ValidationException(message);
            }

            accountDTO.CustomerId = -1;
            accountDataHandler = new AccountDataHandler(sqlTransaction);
            accountDataHandler.UpdateAccount(accountDTO, executionContext.GetUserId(), executionContext.SiteId);

            log.LogMethodExit();
        }

        /// <summary>
        /// Method to share the entitlements of the current card with given list of cards
        /// This will equally split the account Credit Plus and Games entitlements with the given list of cards
        /// It will also copy all the discounts with the destination cards
        /// This method will not split the card level entitlements credits, balance, 
        /// </summary>
        public bool TransferCreditPlusAndGameEntitlementsForSplitProduct(List<int> destinationAccountIdList, int transactionId, int transactionLineId, SqlTransaction sqlTransaction = null)
        {
            int numberOfShares = destinationAccountIdList.Count + 1;

            Dictionary<int, decimal> creditPlusLineShareMap = new Dictionary<int, decimal>();
            Dictionary<int, int> gameLineShareMap = new Dictionary<int, int>();
            decimal totalCreditPlusShare = 0;
            int totalGameBalanceShare = 0;

            // now traverse through the list of cards and transfer entitlements to each card
            for (int i = 0; i < destinationAccountIdList.Count; i++)
            {
                // step 1 - build the destination account
                AccountBL destinationAccountBL = new AccountBL(this.executionContext, destinationAccountIdList[i], true, true, sqlTransaction);
                if (Object.ReferenceEquals(destinationAccountBL.AccountDTO, null))
                    return false;

                if (!Object.ReferenceEquals(this.AccountDTO.AccountCreditPlusDTOList, null) && this.AccountDTO.AccountCreditPlusDTOList.Count > 0)
                {
                    // if this is for a recharge, then select only the credit plus elements that got added in this transaction
                    List<AccountCreditPlusDTO> sourceAccountCreditPlusDTOList = this.AccountDTO.AccountCreditPlusDTOList;
                    if (transactionId != -1)
                        sourceAccountCreditPlusDTOList = sourceAccountCreditPlusDTOList.Where(
                           x => (x.TransactionId == transactionId)).ToList();

                    // the share at credit line level needs to be calculated upfront as the balance in source account dto is changed by transfer
                    foreach (AccountCreditPlusDTO sourceAccountCreditPlusDTO in sourceAccountCreditPlusDTOList)
                    {
                        if (creditPlusLineShareMap.ContainsKey(sourceAccountCreditPlusDTO.AccountCreditPlusId))
                            continue;

                        AccountCreditPlusBL sourceAccountCreditPlusBL = new AccountCreditPlusBL(this.executionContext, sourceAccountCreditPlusDTO);
                        if (!sourceAccountCreditPlusBL.CanTransferBalanceToOtherAccounts())
                            continue;

                        decimal availableCPAmount = Object.ReferenceEquals(sourceAccountCreditPlusDTO.CreditPlusBalance, null) ? 0.0M : Convert.ToDecimal(sourceAccountCreditPlusDTO.CreditPlusBalance);
                        decimal share = (int)(availableCPAmount / numberOfShares);
                        if (share > 0)
                        {
                            totalCreditPlusShare += share;
                            creditPlusLineShareMap.Add(sourceAccountCreditPlusDTO.AccountCreditPlusId, share);
                        }
                    }

                    if (!TransferCreditPlusEntitlement(destinationAccountBL.AccountDTO, transactionId, transactionLineId, String.Empty, totalCreditPlusShare,
                                creditPlusLineShareMap, sqlTransaction))
                    {
                        return false;
                    }
                }

                if (!Object.ReferenceEquals(this.AccountDTO.AccountDiscountDTOList, null) && this.AccountDTO.AccountDiscountDTOList.Count > 0)
                {
                    if (destinationAccountBL.AccountDTO.AccountDiscountDTOList == null)
                        destinationAccountBL.AccountDTO.AccountDiscountDTOList = new List<AccountDiscountDTO>();

                    foreach (AccountDiscountDTO discountDTO in this.AccountDTO.AccountDiscountDTOList)
                    {

                        bool isDiscountExist = destinationAccountBL.AccountDTO.AccountDiscountDTOList.Exists(
                          x => x.IsActive && x.DiscountId == discountDTO.DiscountId);
                        if (!isDiscountExist)
                        {
                            AccountDiscountDTO clonedDiscountDTO = new AccountDiscountDTO(-1, destinationAccountBL.AccountDTO.AccountId, discountDTO.DiscountId, discountDTO.ExpiryDate,
                                    discountDTO.TransactionId, discountDTO.LineId, discountDTO.TaskId, "",
                                    DateTime.Now, discountDTO.InternetKey, true, discountDTO.SiteId,
                                    -1, false, "", discountDTO.ExpireWithMembership,
                                     discountDTO.MembershipRewardsId, discountDTO.MembershipId, "", null, discountDTO.ValidityStatus, -1);
                            clonedDiscountDTO.IsChanged = true;

                            destinationAccountBL.AccountDTO.AccountDiscountDTOList.Add(clonedDiscountDTO);
                        }
                    }
                }

                if (!Object.ReferenceEquals(this.AccountDTO.AccountGameDTOList, null) && this.AccountDTO.AccountGameDTOList.Count > 0)
                {
                    List<AccountGameDTO> sourceGameDTOList = this.AccountDTO.AccountGameDTOList;
                    if (transactionId != -1)
                        sourceGameDTOList = sourceGameDTOList.Where(
                           x => (x.TransactionId == transactionId)).ToList();
                    if (destinationAccountBL.AccountDTO.AccountGameDTOList == null)
                    {
                        destinationAccountBL.AccountDTO.AccountGameDTOList = new List<AccountGameDTO>();
                    }
                    foreach (AccountGameDTO gameDTO in sourceGameDTOList)
                    {
                        // the share at games line level needs to be calculated upfront as the balance in source account dto is changed by transfer
                        int gameshare = 0;
                        if (!gameLineShareMap.ContainsKey(gameDTO.AccountGameId))
                        {
                            gameshare = (int)(gameDTO.BalanceGames / numberOfShares);
                            gameLineShareMap.Add(gameDTO.AccountGameId, gameshare);
                            totalGameBalanceShare += gameshare;
                        }
                    }
                    if (!TransferCardGamesEntitlement(destinationAccountBL.AccountDTO, transactionId, transactionLineId, -1, totalGameBalanceShare, gameLineShareMap, sqlTransaction))
                    {
                        return false;
                    }
                }

                // save the transferred entitlements to this card
                destinationAccountBL.Save(sqlTransaction);
            }

            // save the transferred entitlements to parent card
            // should i save the parent card after every child card is saved? instead of at the end?
            this.Save(sqlTransaction);

            return true;
        }

        /// <summary>
        /// Method to transfer from the current card to destination cards
        /// Entitlement type and quantity should be specified
        /// </summary>
        public bool TransferEntitlement(AccountDTO destinationAccount, int transactionId, int transactionLineId, Dictionary<string, decimal> entitlementsToTransfer,
             Dictionary<int, decimal> transferredIdMap, SqlTransaction sqlTransaction = null)
        {
            Decimal creditsTransferred = 0;
            Decimal bonusTransferred = 0;
            Decimal ticketsTransferred = 0;

            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            DateTime currentTime = lookupValuesList.GetServerDateTime();

            foreach (KeyValuePair<string, decimal> entitlement in entitlementsToTransfer)
            {
                string entitlementType = entitlement.Key;
                decimal entitlementTransferAmount = entitlement.Value;
                decimal totalAmountTransferred = 0;
                decimal pendingToBeTransferred = entitlementTransferAmount;

                if (entitlementType.Equals(CreditPlusTypeConverter.ToString(CreditPlusType.CARD_BALANCE)))
                {
                    decimal creditsToBeTransferred = 0.0M;
                    if (accountDTO.Credits != null)
                    {
                        creditsToBeTransferred = accountDTO.Credits >= entitlementTransferAmount ? entitlementTransferAmount : Convert.ToDecimal(accountDTO.Credits.ToString());
                        if (creditsToBeTransferred > 0)
                        {
                            accountDTO.Credits -= creditsToBeTransferred;
                            destinationAccount.Credits += creditsToBeTransferred;
                        }
                        totalAmountTransferred += creditsToBeTransferred;
                        creditsTransferred += creditsToBeTransferred;
                        //pendingToBeTransferred
                    }

                    if (pendingToBeTransferred > creditsToBeTransferred)
                    {
                        if (!TransferCreditPlusEntitlement(destinationAccount, transactionId, transactionLineId, entitlementType, (pendingToBeTransferred - creditsToBeTransferred), transferredIdMap, sqlTransaction))
                            return false;
                        else
                        {
                            totalAmountTransferred += pendingToBeTransferred - creditsToBeTransferred;
                            creditsTransferred += pendingToBeTransferred - creditsToBeTransferred;
                        }
                    }
                }
                else if (entitlementType.Equals(CreditPlusTypeConverter.ToString(CreditPlusType.TICKET)))
                {
                    decimal ticketsToBeTransferred = 0.0M;
                    if (accountDTO.TicketCount != null)
                    {
                        ticketsToBeTransferred = accountDTO.TicketCount >= entitlementTransferAmount ? (entitlementTransferAmount) : Convert.ToDecimal(accountDTO.TicketCount.ToString());
                        if (ticketsToBeTransferred > 0)
                        {
                            //Uncommented reduction from source
                            accountDTO.TicketCount -= Decimal.ToInt32(ticketsToBeTransferred);
                            //create a new generic creditplus entry and add
                            AccountCreditPlusDTO genericTicketCreditPlusDTO = new AccountCreditPlusDTO(-1, ticketsToBeTransferred, CreditPlusType.TICKET, false,
                            "Balance Transfer", destinationAccount.AccountId, transactionId != -1 ? transactionId : -1, transactionId != -1 ? transactionLineId : -1, ticketsToBeTransferred,
                            null, null, null, null, null, true, true, true, true, true, true, true, null, -1, false, null, false, false, true, -1, -1, currentTime, "",
                            currentTime, destinationAccount.SiteId, -1, false, "", false, true, "", -1, AccountDTO.AccountValidityStatus.Valid, -1);

                            genericTicketCreditPlusDTO.IsChanged = true;

                            if (destinationAccount.AccountCreditPlusDTOList == null)
                                destinationAccount.AccountCreditPlusDTOList = new List<AccountCreditPlusDTO>();

                            destinationAccount.AccountCreditPlusDTOList.Add(genericTicketCreditPlusDTO);
                        }

                        //commented destination addition as it is part of credit plus
                        //destinationAccount.TicketCount += Decimal.ToInt32(ticketsToBeTransferred);
                        totalAmountTransferred += ticketsToBeTransferred;
                        ticketsTransferred += ticketsToBeTransferred;
                    }
                    if (pendingToBeTransferred > ticketsToBeTransferred)
                    {
                        if (!TransferCreditPlusEntitlement(destinationAccount, transactionId, transactionLineId, entitlementType, (pendingToBeTransferred - ticketsToBeTransferred), transferredIdMap, sqlTransaction))
                            return false;
                        else
                        {
                            totalAmountTransferred += pendingToBeTransferred - ticketsToBeTransferred;
                            ticketsTransferred += pendingToBeTransferred - ticketsToBeTransferred;
                        }

                    }
                }
                else if (entitlementType.Equals(CreditPlusTypeConverter.ToString(CreditPlusType.GAME_PLAY_BONUS)))
                {
                    decimal bonusToBeTransferred = 0.0M;
                    if (accountDTO.Bonus != null)
                    {
                        bonusToBeTransferred = accountDTO.Bonus >= entitlementTransferAmount ? entitlementTransferAmount : Convert.ToDecimal(accountDTO.Bonus.ToString());
                        if (bonusToBeTransferred > 0)
                        {
                            accountDTO.Bonus -= Decimal.ToInt32(bonusToBeTransferred);
                            destinationAccount.Bonus += bonusToBeTransferred;
                        }
                        totalAmountTransferred += bonusToBeTransferred;
                        bonusTransferred += bonusToBeTransferred;
                    }
                    if (pendingToBeTransferred > bonusToBeTransferred)
                    {
                        if (!TransferCreditPlusEntitlement(destinationAccount, transactionId, transactionLineId, entitlementType, (pendingToBeTransferred - bonusToBeTransferred), transferredIdMap, sqlTransaction))
                            return false;
                        else
                        {
                            totalAmountTransferred += pendingToBeTransferred - bonusToBeTransferred;
                            bonusTransferred += pendingToBeTransferred - bonusToBeTransferred;
                        }
                    }
                }

                else if (entitlementType.Equals(CreditPlusTypeConverter.ToString(CreditPlusType.TIME)))
                {
                    decimal timeToBeTransferred = 0.0M;
                    if (accountDTO.Time != null)
                    {
                        decimal availableTime = Convert.ToDecimal(accountDTO.Time) + GetAccountTimeBalance();
                        timeToBeTransferred = availableTime >= entitlementTransferAmount ? entitlementTransferAmount : availableTime;
                        if (timeToBeTransferred > 0)
                        {
                            accountDTO.Time -= Decimal.ToInt32(timeToBeTransferred);
                            destinationAccount.Time += timeToBeTransferred;
                        }
                        totalAmountTransferred += timeToBeTransferred;
                        timeToBeTransferred += timeToBeTransferred;
                    }
                    if (pendingToBeTransferred > timeToBeTransferred)
                    {
                        if (!TransferCreditPlusEntitlement(destinationAccount, transactionId, transactionLineId, entitlementType, (pendingToBeTransferred - timeToBeTransferred), transferredIdMap, sqlTransaction))
                            return false;
                        else
                        {
                            totalAmountTransferred += pendingToBeTransferred - timeToBeTransferred;
                            timeToBeTransferred += pendingToBeTransferred - timeToBeTransferred;
                        }
                    }
                }


                else if (entitlementType.Equals("COURTESY"))
                {
                    decimal courtesyToBeTransferred = 0.0M;
                    if (accountDTO.Courtesy != null)
                    {
                        courtesyToBeTransferred = accountDTO.Courtesy >= entitlementTransferAmount ? entitlementTransferAmount : Convert.ToDecimal(accountDTO.Courtesy.ToString());
                        if (courtesyToBeTransferred > 0)
                        {
                            accountDTO.Courtesy -= courtesyToBeTransferred;
                            destinationAccount.Courtesy += courtesyToBeTransferred;
                        }
                        totalAmountTransferred += courtesyToBeTransferred;
                    }
                }
                else if (entitlementType.Equals(CreditPlusTypeConverter.ToString(CreditPlusType.COUNTER_ITEM)))
                {
                    decimal counterItemsToBeTransferred = 0.0M;
                    if (accountDTO.AccountSummaryDTO.CreditPlusItemPurchase != null)
                    {
                        counterItemsToBeTransferred = accountDTO.AccountSummaryDTO.CreditPlusItemPurchase >= entitlementTransferAmount ? entitlementTransferAmount : Convert.ToDecimal(accountDTO.AccountSummaryDTO.CreditPlusItemPurchase.ToString());
                        totalAmountTransferred += counterItemsToBeTransferred;
                    }

                    if (!TransferCreditPlusEntitlement(destinationAccount, transactionId, transactionLineId, entitlementType, (counterItemsToBeTransferred), transferredIdMap, sqlTransaction))
                        return false;


                }

                else if (entitlementType.Equals(CreditPlusTypeConverter.ToString(CreditPlusType.GAME_PLAY_CREDIT)))
                {
                    decimal gameplayToBeTransferred = 0.0M;
                    if (accountDTO.AccountSummaryDTO.TotalGamePlayCreditsBalance != null)
                    {
                        gameplayToBeTransferred = accountDTO.AccountSummaryDTO.TotalGamePlayCreditsBalance >= entitlementTransferAmount ? entitlementTransferAmount : Convert.ToDecimal(accountDTO.AccountSummaryDTO.CreditPlusGamePlayCredits.ToString());
                        totalAmountTransferred += gameplayToBeTransferred;

                        if (!TransferCreditPlusEntitlement(destinationAccount, transactionId, transactionLineId, entitlementType, (gameplayToBeTransferred), transferredIdMap, sqlTransaction))
                            return false;
                    }
                }
                else
                {
                    throw new ValidationException("Entitlement Type " + entitlementType + " cannot be transferred.");
                }

                if (totalAmountTransferred < entitlementTransferAmount)
                {
                    throw new ValidationException("This account does not have sufficient Credit Plus Balance.");
                }
            }

            // check if the source account has been modified concurrently
            AccountBL sourceAccountCurrent = new AccountBL(this.executionContext, this.AccountDTO.AccountId, true, true, sqlTransaction);
            if (sourceAccountCurrent.AccountDTO != null)
            {
                if (creditsTransferred > 0 && (sourceAccountCurrent.AccountDTO.TotalCreditsBalance - creditsTransferred) < 0)
                {
                    String message = MessageContainerList.GetMessage(executionContext, 354, sourceAccountCurrent.AccountDTO.TagNumber);
                    throw new ValidationException(message);
                }
                if (ticketsTransferred > 0 && (sourceAccountCurrent.AccountDTO.TotalTicketsBalance - ticketsTransferred) < 0)
                {
                    String message = MessageContainerList.GetMessage(executionContext, 354, sourceAccountCurrent.AccountDTO.TagNumber);
                    throw new ValidationException(message);
                }
                if (bonusTransferred > 0 && (sourceAccountCurrent.AccountDTO.TotalBonusBalance - bonusTransferred) < 0)
                {
                    String message = MessageContainerList.GetMessage(executionContext, 354, sourceAccountCurrent.AccountDTO.TagNumber);
                    throw new ValidationException(message);
                }
            }

            //Deduct from source account first
            this.Save(sqlTransaction);

            //Increment from the destination account now
            AccountBL destinationAccountBL = new AccountBL(this.executionContext, destinationAccount);
            destinationAccountBL.Save(sqlTransaction);

            return true;
        }

        /// <summary>
        /// Method to transfer credit plus entitlements from the current card to destination cards
        /// Entitlement type and quantity should be specified
        /// </summary>
        private bool TransferCreditPlusEntitlement(AccountDTO destinationAccount, int transactionId, int transactionLineId, string entitlementType, decimal entitlementTransferAmount,
             Dictionary<int, decimal> transferredIdMap, SqlTransaction sqlTransaction = null)
        {
            decimal totalAmountTransferred = 0;
            decimal pendingToBeTransferred = entitlementTransferAmount;

            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            DateTime currentTime = lookupValuesList.GetServerDateTime();

            if (!Object.ReferenceEquals(this.AccountDTO.AccountCreditPlusDTOList, null) && this.AccountDTO.AccountCreditPlusDTOList.Any())
            {
                List<AccountCreditPlusDTO> sourceAccountCreditPlusDTOList = this.AccountDTO.AccountCreditPlusDTOList;

                // if this is for a recharge, then select only the credit plus elements that got added in this transaction
                if (transactionId != -1)
                    sourceAccountCreditPlusDTOList = sourceAccountCreditPlusDTOList.Where(
                       x => (x.TransactionId == transactionId)).ToList();

                // filter by entitlement type
                if (!String.Equals(entitlementType, String.Empty))
                    sourceAccountCreditPlusDTOList = sourceAccountCreditPlusDTOList.Where(
                            x => (x.CreditPlusType.ToString().Equals(CreditPlusTypeConverter.ToString(entitlementType)))).ToList();

                foreach (AccountCreditPlusDTO sourceAccountCreditPlusDTO in sourceAccountCreditPlusDTOList)
                {
                    AccountCreditPlusBL sourceAccountCreditPlusBL = new AccountCreditPlusBL(this.executionContext, sourceAccountCreditPlusDTO);
                    if (!sourceAccountCreditPlusBL.CanTransferBalanceToOtherAccounts())
                        continue;

                    // this will happed for a split product scenario where the entitlement has to be transferred for a particular line number
                    if (!Object.ReferenceEquals(transferredIdMap, null) && !transferredIdMap.ContainsKey(sourceAccountCreditPlusDTO.AccountCreditPlusId))
                        continue;



                    decimal availableCPAmount = Object.ReferenceEquals(sourceAccountCreditPlusDTO.CreditPlusBalance, null) ? 0.0M : Convert.ToDecimal(sourceAccountCreditPlusDTO.CreditPlusBalance);

                    // this will happed for a split product scenario where the entitlement has to be transferred for a particular line number
                    if (!Object.ReferenceEquals(transferredIdMap, null) && transferredIdMap.ContainsKey(sourceAccountCreditPlusDTO.AccountCreditPlusId))
                    {
                        transferredIdMap.TryGetValue(sourceAccountCreditPlusDTO.AccountCreditPlusId, out availableCPAmount);
                    }

                    decimal amountToBeTransferred = (availableCPAmount >= pendingToBeTransferred) ? pendingToBeTransferred : availableCPAmount;

                    // create a new CP DTO for destination card
                    AccountCreditPlusDTO cpDTOForDestinationAccount = new AccountCreditPlusDTO(-1, amountToBeTransferred, sourceAccountCreditPlusDTO.CreditPlusType, sourceAccountCreditPlusDTO.Refundable,
                        sourceAccountCreditPlusDTO.Remarks, destinationAccount.AccountId, transactionId != -1 ? transactionId : -1, transactionId != -1 ? transactionLineId : -1, amountToBeTransferred,
                        sourceAccountCreditPlusDTO.PeriodFrom, sourceAccountCreditPlusDTO.PeriodTo, sourceAccountCreditPlusDTO.TimeFrom, sourceAccountCreditPlusDTO.TimeTo, sourceAccountCreditPlusDTO.NumberOfDays, sourceAccountCreditPlusDTO.Monday,
                        sourceAccountCreditPlusDTO.Tuesday, sourceAccountCreditPlusDTO.Wednesday, sourceAccountCreditPlusDTO.Thursday, sourceAccountCreditPlusDTO.Friday, sourceAccountCreditPlusDTO.Saturday, sourceAccountCreditPlusDTO.Sunday, sourceAccountCreditPlusDTO.MinimumSaleAmount,
                        sourceAccountCreditPlusDTO.LoyaltyRuleId, sourceAccountCreditPlusDTO.ExtendOnReload, sourceAccountCreditPlusDTO.PlayStartTime, sourceAccountCreditPlusDTO.TicketAllowed, sourceAccountCreditPlusDTO.ForMembershipOnly,
                        sourceAccountCreditPlusDTO.ExpireWithMembership, -1, sourceAccountCreditPlusDTO.MembershipRewardsId, currentTime, "",
                        currentTime, destinationAccount.SiteId, -1, false, "", sourceAccountCreditPlusDTO.PauseAllowed, true, "", sourceAccountCreditPlusDTO.AccountCreditPlusId, sourceAccountCreditPlusDTO.ValidityStatus, -1);

                    //to reset the isChanged flag
                    cpDTOForDestinationAccount.IsChanged = true;
                    if (entitlementType == CreditPlusTypeConverter.ToString(CreditPlusType.TIME))
                    {
                        AccountCreditPlusBL accountCreditPlusBL = new AccountCreditPlusBL(executionContext, sourceAccountCreditPlusDTO);
                        decimal availableTimeAmount = accountCreditPlusBL.GetCreditPlusTime(currentTime);
                        accountCreditPlusBL.AccountCreditPlusDTO.CreditPlus = (availableTimeAmount >= pendingToBeTransferred) ? pendingToBeTransferred : availableTimeAmount;
                    }
                    // if the CP has consumption rules, copy the same to the destination card
                    if (!Object.ReferenceEquals(sourceAccountCreditPlusDTO.AccountCreditPlusConsumptionDTOList, null) && sourceAccountCreditPlusDTO.AccountCreditPlusConsumptionDTOList.Count > 0)
                    {
                        List<AccountCreditPlusConsumptionDTO> destinationCPConsumptionDTOList = new List<AccountCreditPlusConsumptionDTO>();
                        foreach (AccountCreditPlusConsumptionDTO sourceCPConsumptionDTO in sourceAccountCreditPlusDTO.AccountCreditPlusConsumptionDTOList)
                        {
                            AccountCreditPlusConsumptionDTO cpConsumptionDTOForDestinationAccount = new AccountCreditPlusConsumptionDTO(-1, -1, sourceCPConsumptionDTO.POSTypeId, sourceCPConsumptionDTO.ExpiryDate,
                                sourceCPConsumptionDTO.ProductId, sourceCPConsumptionDTO.GameProfileId, sourceCPConsumptionDTO.GameId, sourceCPConsumptionDTO.DiscountPercentage, sourceCPConsumptionDTO.DiscountedPrice,
                                sourceCPConsumptionDTO.ConsumptionBalance, sourceCPConsumptionDTO.QuantityLimit, sourceCPConsumptionDTO.CategoryId, sourceCPConsumptionDTO.DiscountAmount, sourceCPConsumptionDTO.OrderTypeId, "",
                                currentTime, destinationAccount.SiteId, -1, false, "", true, "", currentTime, sourceCPConsumptionDTO.ConsumptionQty);

                            cpConsumptionDTOForDestinationAccount.IsChanged = true;

                            destinationCPConsumptionDTOList.Add(cpConsumptionDTOForDestinationAccount);
                        }
                        cpDTOForDestinationAccount.AccountCreditPlusConsumptionDTOList = destinationCPConsumptionDTOList;
                    }

                    if (!Object.ReferenceEquals(sourceAccountCreditPlusDTO.AccountCreditPlusPurchaseCriteriaDTOList, null) && sourceAccountCreditPlusDTO.AccountCreditPlusPurchaseCriteriaDTOList.Count > 0)
                    {

                        List<AccountCreditPlusPurchaseCriteriaDTO> destinationCPPurchaseCriteriaDTOList = new List<AccountCreditPlusPurchaseCriteriaDTO>();
                        foreach (AccountCreditPlusPurchaseCriteriaDTO sourceCPPurchaseCriteriaDTO in sourceAccountCreditPlusDTO.AccountCreditPlusPurchaseCriteriaDTOList)
                        {
                            AccountCreditPlusPurchaseCriteriaDTO cpPurchaseCriteriaDTOForDestinationAccount = new AccountCreditPlusPurchaseCriteriaDTO(-1, -1, sourceCPPurchaseCriteriaDTO.POSTypeId,
                                sourceCPPurchaseCriteriaDTO.ProductId, "", currentTime, destinationAccount.SiteId, -1, false, "", "", currentTime);

                            cpPurchaseCriteriaDTOForDestinationAccount.IsChanged = true;

                            destinationCPPurchaseCriteriaDTOList.Add(cpPurchaseCriteriaDTOForDestinationAccount);
                        }
                        cpDTOForDestinationAccount.AccountCreditPlusPurchaseCriteriaDTOList = destinationCPPurchaseCriteriaDTOList;
                    }

                    destinationAccount.AccountCreditPlusDTOList = (Object.ReferenceEquals(destinationAccount.AccountCreditPlusDTOList, null) ? new List<AccountCreditPlusDTO>() : destinationAccount.AccountCreditPlusDTOList);
                    destinationAccount.AccountCreditPlusDTOList.Add(cpDTOForDestinationAccount);

                    // reduce the amount transferred from the original DTO
                    sourceAccountCreditPlusDTO.CreditPlusBalance -= amountToBeTransferred;
                    //sourceAccountCreditPlusDTO.CreditPlus -= amountToBeTransferred;//This is wrong!!
                    totalAmountTransferred += amountToBeTransferred;
                    pendingToBeTransferred -= amountToBeTransferred;
                    // if all the amount transferred is equal to required amount, break
                    if (totalAmountTransferred >= entitlementTransferAmount)
                        break;
                }
            }
            if (totalAmountTransferred < entitlementTransferAmount)
            {
                throw new ValidationException("This account does not have sufficient Credit Plus Balance.");
            }

            return true;
        }


        /// <summary>
        /// Method to transfer card game entitlements from the current card to destination cards
        /// Entitlement type and quantity should be specified
        /// </summary>
        private bool TransferCardGamesEntitlement(AccountDTO destinationAccount, int transactionId, int transactionLineId, int gameId, int entitlementTransferAmount,
             Dictionary<int, int> transferredIdMap, SqlTransaction sqlTransaction = null)
        {
            int totalAmountTransferred = 0;
            int pendingToBeTransferred = entitlementTransferAmount;

            if (Object.ReferenceEquals(destinationAccount, null))
                return false;

            if (!Object.ReferenceEquals(this.AccountDTO.AccountGameDTOList, null) && this.AccountDTO.AccountGameDTOList.Any())
            {
                List<AccountGameDTO> sourceAccountGameDTOList = this.AccountDTO.AccountGameDTOList;

                // if this is for a recharge, then select only the credit plus elements that got added in this transaction
                if (transactionId != -1)
                    sourceAccountGameDTOList = sourceAccountGameDTOList.Where(
                       x => (x.TransactionId == transactionId)).ToList();

                // filter by entitlement type
                if (gameId != -1)
                    sourceAccountGameDTOList = sourceAccountGameDTOList.Where(x => (x.GameId == gameId)).ToList();
                if (destinationAccount.AccountGameDTOList == null)
                {
                    destinationAccount.AccountGameDTOList = new List<AccountGameDTO>();
                }
                foreach (AccountGameDTO souceGameDTO in sourceAccountGameDTOList)
                {
                    // this will happed for a split product scenario where the entitlement has to be transferred for a particular line number
                    if (!Object.ReferenceEquals(transferredIdMap, null) && !transferredIdMap.ContainsKey(souceGameDTO.AccountGameId))
                        continue;

                    int availableGameBalance = Object.ReferenceEquals(souceGameDTO.BalanceGames, null) ? 0 : souceGameDTO.BalanceGames;

                    // this will happed for a split product scenario where the entitlement has to be transferred for a particular line number
                    if (!Object.ReferenceEquals(transferredIdMap, null) && transferredIdMap.ContainsKey(souceGameDTO.AccountGameId))
                    {
                        transferredIdMap.TryGetValue(souceGameDTO.AccountGameId, out availableGameBalance);
                    }

                    int amountToBeTransferred = (availableGameBalance >= pendingToBeTransferred) ? pendingToBeTransferred : availableGameBalance;

                    souceGameDTO.BalanceGames -= amountToBeTransferred;
                    totalAmountTransferred += amountToBeTransferred;
                    souceGameDTO.Quantity -= amountToBeTransferred;

                    AccountGameDTO clonedGametDTO = new AccountGameDTO(-1, destinationAccount.AccountId, souceGameDTO.GameId, amountToBeTransferred, souceGameDTO.ExpiryDate,
                        souceGameDTO.GameProfileId, souceGameDTO.Frequency, null, amountToBeTransferred, transactionId,
                        transactionLineId, souceGameDTO.EntitlementType, souceGameDTO.OptionalAttribute, souceGameDTO.CustomDataSetId, souceGameDTO.TicketAllowed,
                        souceGameDTO.FromDate, souceGameDTO.Monday, souceGameDTO.Tuesday, souceGameDTO.Wednesday, souceGameDTO.Thursday, souceGameDTO.Friday, souceGameDTO.Saturday, souceGameDTO.Sunday,
                        souceGameDTO.ExpireWithMembership, souceGameDTO.MembershipId, souceGameDTO.MembershipRewardsId, "", DateTime.Now, "", DateTime.Now, souceGameDTO.SiteId, -1, false, "", true, souceGameDTO.ValidityStatus, -1);

                    clonedGametDTO.IsChanged = true;

                    if (!Object.ReferenceEquals(souceGameDTO.AccountGameExtendedDTOList, null) && souceGameDTO.AccountGameExtendedDTOList.Count > 0)
                    {
                        List<AccountGameExtendedDTO> extendedGamesDTOList = new List<AccountGameExtendedDTO>();
                        foreach (AccountGameExtendedDTO extendedGameDTO in souceGameDTO.AccountGameExtendedDTOList)
                        {
                            AccountGameExtendedDTO clonedExtendedDTO = new AccountGameExtendedDTO(-1, -1, extendedGameDTO.GameId, extendedGameDTO.GameProfileId, extendedGameDTO.Exclude,
                                extendedGameDTO.PlayLimitPerGame, extendedGameDTO.SiteId, -1, false, "", true, "", DateTime.MinValue, "", DateTime.MinValue);

                            clonedExtendedDTO.IsChanged = true;
                            extendedGamesDTOList.Add(clonedExtendedDTO);
                        }
                        clonedGametDTO.AccountGameExtendedDTOList = extendedGamesDTOList;
                    }

                    destinationAccount.AccountGameDTOList.Add(clonedGametDTO);
                }
            }

            if (totalAmountTransferred < entitlementTransferAmount)
            {
                throw new ValidationException("This account does not have sufficient Games Balance.");
            }

            //AccountBL destinationAccountBL = new AccountBL(this.executionContext, destinationAccount);
            //destinationAccountBL.Save(sqlTransaction);
            //this.Save(sqlTransaction);

            return true;
        }

        /// <summary>
        /// Method to return refundable games on in the account
        /// Refundable games are added to List<AccountGameDTO> refundaleGamesList
        /// </summary>
        public List<AccountGameDTO> GetRefundableGames(DateTime currentDate)
        {
            List<AccountGameDTO> accountGameDTOList = this.AccountDTO.AccountGameDTOList;
            List<AccountGameDTO> refundableGameDTOList = new List<AccountGameDTO>();

            foreach (AccountGameDTO gameDTO in accountGameDTOList)
            {
                if (gameDTO.IsActive && gameDTO.BalanceGames > 0 && gameDTO.ExpiryDate > currentDate)
                {
                    refundableGameDTOList.Add(gameDTO);
                }
            }
            return refundableGameDTOList;
        }

        /// <summary>
        /// Method to return refundable credits on in the account
        /// Refundable games are added to List<AccountCreditPlusDTO> refundadleCreditsList
        /// </summary>
        public List<AccountCreditPlusDTO> GetRefundableCredits(DateTime currentDate, bool allowRefundOfDeposit, bool allowRefundOfCardCredits, bool allowRefundOfCreditPlus)
        {
            List<AccountCreditPlusDTO> AccountCreditPlusDTOList = this.AccountDTO.AccountCreditPlusDTOList;
            List<AccountCreditPlusDTO> RefundableCreditPlusDTOList = new List<AccountCreditPlusDTO>();

            if (allowRefundOfDeposit)
            {
                Decimal? RefundableFaceValue = this.AccountDTO.FaceValue;
            }
            if (allowRefundOfCardCredits)
            {
                Decimal? RefundableCredits = this.AccountDTO.Credits;
            }
            if (allowRefundOfCreditPlus)
            {
                foreach (AccountCreditPlusDTO creditPlusDTO in AccountCreditPlusDTOList)
                {
                    AccountCreditPlusBL sourceAccountCreditPlusBL = new AccountCreditPlusBL(this.executionContext, creditPlusDTO);
                    if (!creditPlusDTO.Refundable || (creditPlusDTO.ValidityStatus == AccountDTO.AccountValidityStatus.Hold) || !sourceAccountCreditPlusBL.CanTransferBalanceToOtherAccounts())
                        continue;

                    RefundableCreditPlusDTOList.Add(creditPlusDTO);
                }
            }
            return RefundableCreditPlusDTOList;
        }

        /// <summary>
        /// Returns total credits available in account at given datetime, excluding future
        /// </summary>
        public decimal? GetCurrentAvailableBalance()
        {
            Decimal? availableBalance = 0.0M;
            if (this.AccountDTO.AccountSummaryDTO != null)
            {
                log.LogVariableState("AccountCreditPlusDTOList", this.AccountDTO.AccountCreditPlusDTOList);
                availableBalance = (AccountDTO.Credits.HasValue ? AccountDTO.Credits.Value : 0) + AccountDTO.AccountSummaryDTO.CreditPlusCardBalance.Value +
                    AccountDTO.AccountSummaryDTO.CreditPlusItemPurchase + AccountDTO.AccountSummaryDTO.CreditPlusGamePlayCredits;
            }
            else
            {
                throw new Exception("Not able to get balance for this card");
            }
            return availableBalance;
        }

        public decimal? GetCurrentPurchaseBalance()
        {
            Decimal? availableBalance = 0.0M;
            if (this.AccountDTO.AccountSummaryDTO != null)
            {
                log.LogVariableState("AccountCreditPlusDTOList", this.AccountDTO.AccountCreditPlusDTOList);
                availableBalance = (AccountDTO.Credits.HasValue ? AccountDTO.Credits.Value : 0) + AccountDTO.AccountSummaryDTO.CreditPlusCardBalance.Value +
                    AccountDTO.AccountSummaryDTO.CreditPlusItemPurchase;
            }
            else
            {
                throw new Exception("Not able to get balance for this card");
            }
            return availableBalance;
        }

        /// <summary>
        /// Returns total credits available in account, including future
        /// </summary>
        public decimal? GetTotalAvailableBalance(DateTime currentDate)
        {
            Decimal? availableBalance = 0.0M;
            availableBalance = this.AccountDTO.TotalBonusBalance + this.AccountDTO.TotalCourtesyBalance + this.AccountDTO.TotalCreditPlusBalance + this.AccountDTO.TotalCreditsBalance;
            return availableBalance;
        }

        public void LoadDailyCardBalance(DateTime membershipProgressionDate)
        {
            log.LogMethodEntry(membershipProgressionDate);
            DateTime? startDate = null;
            if (this.accountDTO.AccountCreditPlusDTOList == null || this.accountDTO.AccountCreditPlusDTOList.Count == 0)
            {
                AccountCreditPlusListBL accountCreditPlusListBL = new AccountCreditPlusListBL(executionContext);
                List<KeyValuePair<AccountCreditPlusDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<AccountCreditPlusDTO.SearchByParameters, string>>();
                searchParams.Add(new KeyValuePair<AccountCreditPlusDTO.SearchByParameters, string>(AccountCreditPlusDTO.SearchByParameters.ACCOUNT_ID, this.accountDTO.AccountId.ToString()));
                List<AccountCreditPlusDTO> AccountCreditPlusDTOList = accountCreditPlusListBL.GetAccountCreditPlusDTOList(searchParams);
                if (AccountCreditPlusDTOList != null && AccountCreditPlusDTOList.Any())
                {
                    this.accountDTO.AccountCreditPlusDTOList = new List<AccountCreditPlusDTO>();
                    this.accountDTO.AccountCreditPlusDTOList.AddRange(AccountCreditPlusDTOList);
                }
            }
            //if (this.accountDTO.AccountCreditPlusDTOList != null && this.accountDTO.AccountCreditPlusDTOList.Any() )
            {
                DailyCardBalanceListBL dailyCardBalanceListBL = new DailyCardBalanceListBL(executionContext);
                List<KeyValuePair<DailyCardBalanceDTO.SearchByParameters, string>> searchParamDCB = new List<KeyValuePair<DailyCardBalanceDTO.SearchByParameters, string>>();
                searchParamDCB.Add(new KeyValuePair<DailyCardBalanceDTO.SearchByParameters, string>(DailyCardBalanceDTO.SearchByParameters.CARD_ID, this.accountDTO.AccountId.ToString()));
                searchParamDCB.Add(new KeyValuePair<DailyCardBalanceDTO.SearchByParameters, string>(DailyCardBalanceDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                List<DailyCardBalanceDTO> dailyCardBalanceDTOList = dailyCardBalanceListBL.GetDailyCardBalanceDTOList(searchParamDCB);
                if (dailyCardBalanceDTOList != null && dailyCardBalanceDTOList.Count > 0)
                {
                    dailyCardBalanceDTOList = dailyCardBalanceDTOList.OrderByDescending(t => t.CardBalanceDate).ToList();
                    if (dailyCardBalanceDTOList[0].CardBalanceDate == DateTime.Now.Date) //IF latestEntryDate is today then  Skip, no entries required for today
                    {
                        startDate = null;
                    }
                    else if (dailyCardBalanceDTOList[0].CardBalanceDate == DateTime.Now.Date.AddDays(-1))  //ELSE IF latestEntryDate equal to Yeasterday then
                    {
                        startDate = DateTime.Now.Date;
                    }
                    else
                    {
                        if (membershipProgressionDate >= dailyCardBalanceDTOList[0].CardBalanceDate) //IF membershipEffectiveFromDate > latestEntryDate then
                        {
                            if (this.accountDTO.IssueDate >= membershipProgressionDate) //IF Card Issue Date > membershipEffectiveFromDate Consider Card Issue Date as start day
                            {
                                startDate = Convert.ToDateTime(accountDTO.IssueDate).Date;
                            }
                            else //Else Consider MembershipEffectiveFrom date as start day
                            {
                                startDate = membershipProgressionDate.Date;
                            }
                        }
                        else //ELSE Consider latestEntryDate + 1 as start day
                        {
                            startDate = ((DateTime)dailyCardBalanceDTOList[0].CardBalanceDate).AddDays(1);
                        }
                    }
                }
                else
                {
                    //If MembershipEffectiveFromDate is within 24 hrs then only one entry is required
                    if (membershipProgressionDate >= DateTime.Now.AddHours(-24))
                    {
                        startDate = DateTime.Now.Date;
                    }
                    else
                    {    //Else membershipEffectiveFromDate > 24 hrs then  Check card issue date.
                        if (this.accountDTO.IssueDate >= DateTime.Now.AddHours(-24))  //    If it is within 24 hrs then  Only one entry is required
                        {
                            startDate = DateTime.Now.Date;
                        }
                        else
                        {
                            // If Card Issue Date is > MembershipEffectiveFrom Date then  Consider Card Issue Date as start day
                            if (this.accountDTO.IssueDate >= membershipProgressionDate)
                            {
                                startDate = Convert.ToDateTime(accountDTO.IssueDate).Date;
                            }
                            else  // Else Consider MembershipEffectiveFrom date as start day
                            {
                                startDate = membershipProgressionDate.Date;
                            }
                        }
                    }

                }
            }

            if (startDate != null)
            {
                DateTime startDateValue = ((DateTime)startDate);
                DateTime startDateValueCheck = startDateValue.AddDays(15);
                if (DateTime.Now.Date > startDateValueCheck)
                {
                    throw new Exception("More than 15 day data is missing");
                }
                else
                {
                    //For Tickets 
                    double totalTicketBalance = Convert.ToDouble(accountDTO.TicketCount);
                    double totalEarnedTicketBalance = Convert.ToDouble(accountDTO.TicketCount);
                    if (accountDTO.AccountCreditPlusDTOList != null && accountDTO.AccountCreditPlusDTOList.Any())
                    {
                        totalTicketBalance = Convert.ToDouble(accountDTO.TicketCount + accountDTO.AccountCreditPlusDTOList.Where(cp => (cp.CreditPlusType == CreditPlusType.TICKET && (cp.PeriodTo != null ? cp.PeriodTo : DateTime.Now) >= DateTime.Now)).Sum(cpl => cpl.CreditPlusBalance));

                        totalEarnedTicketBalance = Convert.ToDouble(accountDTO.TicketCount + this.accountDTO.AccountCreditPlusDTOList.Where(cp => (cp.CreditPlusType == CreditPlusType.TICKET && (cp.MembershipRewardsId == -1) && (cp.PeriodTo != null ? cp.PeriodTo : DateTime.Now) >= DateTime.Now)).Sum(cpl => cpl.CreditPlusBalance));
                    }
                    //for LP
                    double totalLoyaltyPointsBalance = Convert.ToDouble(accountDTO.LoyaltyPoints);
                    double totalRedeemableLoyaltyPointsBalance = Convert.ToDouble(accountDTO.LoyaltyPoints);
                    if (accountDTO.AccountCreditPlusDTOList != null && accountDTO.AccountCreditPlusDTOList.Any())
                    {
                        totalLoyaltyPointsBalance = Convert.ToDouble(accountDTO.LoyaltyPoints + this.accountDTO.AccountCreditPlusDTOList.Where(cp => (cp.CreditPlusType == CreditPlusType.LOYALTY_POINT && (cp.PeriodTo != null ? cp.PeriodTo : DateTime.Now) >= DateTime.Now)).Sum(cpl => cpl.CreditPlusBalance));
                        totalRedeemableLoyaltyPointsBalance = Convert.ToDouble(accountDTO.LoyaltyPoints + this.accountDTO.AccountCreditPlusDTOList.Where(cp => (cp.CreditPlusType == CreditPlusType.LOYALTY_POINT && (cp.MembershipRewardsId == -1) && (cp.ForMembershipOnly == false) && (cp.PeriodTo != null ? cp.PeriodTo : DateTime.Now) >= DateTime.Now)).Sum(cpl => cpl.CreditPlusBalance));
                    }

                    do
                    {
                        DailyCardBalanceDTO ticketDailyCardBalanceDTO = new DailyCardBalanceDTO(-1, this.accountDTO.CustomerId, this.accountDTO.AccountId, startDateValue, totalTicketBalance, totalEarnedTicketBalance, "T", executionContext.GetUserId(), DateTime.Now, executionContext.GetUserId(), DateTime.Now, "", executionContext.GetSiteId(), -1, false);
                        DailyCardBalanceDTO loayltyDailyCardBalanceDTO = new DailyCardBalanceDTO(-1, this.accountDTO.CustomerId, this.accountDTO.AccountId, startDateValue, totalLoyaltyPointsBalance, totalRedeemableLoyaltyPointsBalance, "L", executionContext.GetUserId(), DateTime.Now, executionContext.GetUserId(), DateTime.Now, "", executionContext.GetSiteId(), -1, false);
                        DailyCardBalanceBL ticketDailyCardBalanceBL = new DailyCardBalanceBL(executionContext, ticketDailyCardBalanceDTO);
                        DailyCardBalanceBL loyaltyDailyCardBalanceBL = new DailyCardBalanceBL(executionContext, loayltyDailyCardBalanceDTO);
                        ticketDailyCardBalanceBL.Save();
                        loyaltyDailyCardBalanceBL.Save();
                        startDateValue = startDateValue.AddDays(1);
                    }
                    while (startDateValue < DateTime.Now);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Removes credit plus lines in HOLD status
        /// </summary>
        public void RemoveInvalidCreditPlusLines()
        {
            // remove credit plus items currently on hold
            if (AccountDTO.AccountCreditPlusDTOList != null && AccountDTO.AccountCreditPlusDTOList.Any())
                this.AccountDTO.AccountCreditPlusDTOList = AccountDTO.AccountCreditPlusDTOList.Where(x => x.ValidityStatus != AccountDTO.AccountValidityStatus.Hold).ToList();
        }
        /// <summary>
        /// ActivateSubscriptionEntitlements
        /// </summary>
        /// <param name="subcriptionBillingScheduleId"></param>
        /// <param name="transactionId"></param>
        /// <param name="transactionLineId"></param>
        /// <param name="sqlTrx"></param>
        public void ActivateSubscriptionEntitlements(int subcriptionBillingScheduleId, int transactionId, int transactionLineId, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry();
            if (accountDTO != null)
            {
                LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                DateTime currentServerTime = lookupValuesList.GetServerDateTime();
                if (accountDTO.AccountCreditPlusDTOList != null && accountDTO.AccountCreditPlusDTOList.Any())
                {
                    List<AccountCreditPlusDTO> accountCreditPlusDTOList = accountDTO.AccountCreditPlusDTOList.Where(cp => cp.SubscriptionBillingScheduleId == subcriptionBillingScheduleId
                                                                                                                         //&& cp.TransactionId == -1
                                                                                                                         && cp.IsActive
                                                                                                                         && cp.ValidityStatus == AccountDTO.AccountValidityStatus.Hold).ToList();
                    if (accountCreditPlusDTOList != null && accountCreditPlusDTOList.Any())
                    {
                        for (int i = 0; i < accountCreditPlusDTOList.Count; i++)
                        {
                            AccountCreditPlusBL accountCreditPlusBL = new AccountCreditPlusBL(executionContext, accountCreditPlusDTOList[i]);
                            accountCreditPlusBL.ActivateSubscriptionEntitlements(transactionId, transactionLineId, sqlTrx);
                        }
                    }
                }
                if (accountDTO.AccountGameDTOList != null && accountDTO.AccountGameDTOList.Any())
                {
                    List<AccountGameDTO> accountGameDTOList = accountDTO.AccountGameDTOList.Where(cg => cg.SubscriptionBillingScheduleId == subcriptionBillingScheduleId
                                                                                                             //&& cg.TransactionId == -1
                                                                                                             && cg.IsActive
                                                                                                             && cg.ValidityStatus == AccountDTO.AccountValidityStatus.Hold).ToList();
                    if (accountGameDTOList != null && accountGameDTOList.Any())
                    {
                        for (int i = 0; i < accountGameDTOList.Count; i++)
                        {
                            AccountGameBL accountGameBL = new AccountGameBL(executionContext, accountGameDTOList[i]);
                            accountGameBL.ActivateSubscriptionEntitlements(transactionId, transactionLineId, sqlTrx);
                        }
                    }
                }
                if (accountDTO.AccountDiscountDTOList != null && accountDTO.AccountDiscountDTOList.Any())
                {
                    List<AccountDiscountDTO> accountDiscountDTOList = accountDTO.AccountDiscountDTOList.Where(cd => cd.SubscriptionBillingScheduleId == subcriptionBillingScheduleId
                                                                                                             //&& cd.TransactionId == -1 
                                                                                                             && cd.IsActive
                                                                                                             && cd.ValidityStatus == AccountDTO.AccountValidityStatus.Hold).ToList();
                    if (accountDiscountDTOList != null && accountDiscountDTOList.Any())
                    {
                        for (int i = 0; i < accountDiscountDTOList.Count; i++)
                        {
                            AccountDiscountBL accountDiscountBL = new AccountDiscountBL(executionContext, accountDiscountDTOList[i]);
                            accountDiscountBL.ActivateSubscriptionEntitlements(transactionId, transactionLineId, sqlTrx);
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Cancel Subscription Billing Cycle Entitlements
        /// </summary>
        /// <param name="subscriptionBillingCycleIdList"></param>
        /// <param name="sqlTrx"></param>
        public void CancelSubscriptionBillingCycleEntitlements(List<int> subscriptionBillingCycleIdList, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(subscriptionBillingCycleIdList, sqlTrx);
            if (subscriptionBillingCycleIdList != null && subscriptionBillingCycleIdList.Any())
            {
                if (accountDTO != null)
                {
                    LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                    DateTime currentServerTime = lookupValuesList.GetServerDateTime();
                    CancelSubscriptionBillCycleCreditPlusRecords(subscriptionBillingCycleIdList, currentServerTime);
                    CancelSubscriptionBillCycleCardGamesRecords(subscriptionBillingCycleIdList, currentServerTime);
                    CancelSubscriptionBillCycleCardDiscountRecords(subscriptionBillingCycleIdList, currentServerTime);
                }
            }
            Save(sqlTrx);
            log.LogMethodExit();
        }

        private void CancelSubscriptionBillCycleCreditPlusRecords(List<int> subscriptionBillingCycleIdList, DateTime currentServerTime)
        {
            log.LogMethodEntry(subscriptionBillingCycleIdList, currentServerTime);
            if (accountDTO.AccountCreditPlusDTOList != null && accountDTO.AccountCreditPlusDTOList.Any())
            {
                for (int i = 0; i < subscriptionBillingCycleIdList.Count; i++)
                {
                    List<AccountCreditPlusDTO> subscriptionRelatedDTOS = accountDTO.AccountCreditPlusDTOList.Where(cp => cp.SubscriptionBillingScheduleId == subscriptionBillingCycleIdList[i]
                                                                                                                       && cp.IsActive
                                                                                                                       && (cp.PeriodTo == null || cp.PeriodTo >= currentServerTime)).ToList();
                    if (subscriptionRelatedDTOS != null && subscriptionRelatedDTOS.Any())
                    {
                        for (int j = 0; j < subscriptionRelatedDTOS.Count; j++)
                        {
                            AccountCreditPlusBL accountCreditPlusBL = new AccountCreditPlusBL(executionContext, subscriptionRelatedDTOS[j]);
                            accountCreditPlusBL.CancelSubscriptionBillingCycleEntitlements();
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        private void CancelSubscriptionBillCycleCardGamesRecords(List<int> subscriptionBillingCycleIdList, DateTime currentServerTime)
        {
            log.LogMethodEntry(subscriptionBillingCycleIdList, currentServerTime);
            if (accountDTO.AccountGameDTOList != null && accountDTO.AccountGameDTOList.Any())
            {
                for (int i = 0; i < subscriptionBillingCycleIdList.Count; i++)
                {
                    List<AccountGameDTO> subscriptionRelatedDTOS = accountDTO.AccountGameDTOList.Where(cp => cp.SubscriptionBillingScheduleId == subscriptionBillingCycleIdList[i]
                                                                                                                       && cp.IsActive
                                                                                                                       && (cp.ExpiryDate == null || cp.ExpiryDate >= currentServerTime)).ToList();
                    if (subscriptionRelatedDTOS != null && subscriptionRelatedDTOS.Any())
                    {
                        for (int j = 0; j < subscriptionRelatedDTOS.Count; j++)
                        {
                            AccountGameBL accountGameBL = new AccountGameBL(executionContext, subscriptionRelatedDTOS[j]);
                            accountGameBL.CancelSubscriptionBillingCycleEntitlements();
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
        private void CancelSubscriptionBillCycleCardDiscountRecords(List<int> subscriptionBillingCycleIdList, DateTime currentServerTime)
        {
            log.LogMethodEntry(subscriptionBillingCycleIdList, currentServerTime);
            if (accountDTO.AccountDiscountDTOList != null && accountDTO.AccountDiscountDTOList.Any())
            {
                for (int i = 0; i < subscriptionBillingCycleIdList.Count; i++)
                {
                    List<AccountDiscountDTO> subscriptionRelatedDTOS = accountDTO.AccountDiscountDTOList.Where(cp => cp.SubscriptionBillingScheduleId == subscriptionBillingCycleIdList[i]
                                                                                                                       && cp.IsActive
                                                                                                                       && (cp.ExpiryDate == null || cp.ExpiryDate >= currentServerTime)).ToList();
                    if (subscriptionRelatedDTOS != null && subscriptionRelatedDTOS.Any())
                    {
                        for (int j = 0; j < subscriptionRelatedDTOS.Count; j++)
                        {
                            AccountDiscountBL accountDiscountBL = new AccountDiscountBL(executionContext, subscriptionRelatedDTOS[j]);
                            accountDiscountBL.CancelSubscriptionBillingCycleEntitlements();
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// This method should be used for Updating the Ticket Mode.
        /// </summary>
        /// <param name="sqlTransaction"></param>
        /// <param name="ticketmode"></param>/// 
        public void UpdateTicketMode(bool ticketMode)
        {
            log.LogMethodEntry(ticketMode);
            accountDTO.RealTicketMode = ticketMode;
            log.LogMethodExit();
        }
        /// <summary>
        /// This method should be used for exchange token
        /// </summary>
        /// <param name="sqlTransaction"></param>
        /// <param name="credits"></param>/// 
        public void ExchangeToken(decimal credits)
        {
            log.LogMethodEntry(credits);
            bool ticketAllowed = this.accountDTO.TicketAllowed;
            AccountCreditPlusDTO genericCreditPlusDTO = new AccountCreditPlusDTO(-1, credits, CreditPlusType.CARD_BALANCE, false,
            "Exchange Token", accountDTO.AccountId, -1, -1, credits,
            null, null, null, null, null, true, true, true, true, true, true, true, null, -1, false, null, ticketAllowed, false, false, -1, -1, ServerDateTime.Now, "",
            ServerDateTime.Now, accountDTO.SiteId, -1, false, "", false, true, "", -1, AccountDTO.AccountValidityStatus.Valid, -1);
            genericCreditPlusDTO.IsChanged = true;
            if (accountDTO.AccountCreditPlusDTOList == null)
                accountDTO.AccountCreditPlusDTOList = new List<AccountCreditPlusDTO>();
            accountDTO.AccountCreditPlusDTOList.Add(genericCreditPlusDTO);
            log.LogMethodExit();
        }
        /// <summary>
        /// This method should be used for exchange token
        /// </summary>
        /// <param name="sqlTransaction"></param>
        /// <param name="credits"></param>/// 
        public void ExchangeCredits(decimal credits)
        {
            log.LogMethodEntry(credits);
            decimal pendingtoTransfer = credits;
            decimal creditsToBeTransferred = 0;
            if (accountDTO.Credits > 0)
            {
                creditsToBeTransferred = accountDTO.Credits >= pendingtoTransfer ? pendingtoTransfer : Convert.ToDecimal(accountDTO.Credits.ToString());
                accountDTO.Credits -= creditsToBeTransferred;
                pendingtoTransfer -= creditsToBeTransferred;
            }
            else
            {
                List<AccountCreditPlusDTO> sourceAccountCreditPlusDTOList = this.AccountDTO.AccountCreditPlusDTOList.FindAll(x => x.CreditPlusType == CreditPlusType.CARD_BALANCE);
                foreach (AccountCreditPlusDTO sourceAccountCreditPlusDTO in sourceAccountCreditPlusDTOList)
                {
                    if (sourceAccountCreditPlusDTO.CreditPlusBalance != null && sourceAccountCreditPlusDTO.CreditPlusBalance > 0)
                    {
                        creditsToBeTransferred = sourceAccountCreditPlusDTO.CreditPlusBalance >= pendingtoTransfer ? pendingtoTransfer : Convert.ToDecimal(sourceAccountCreditPlusDTO.CreditPlusBalance.ToString());
                        sourceAccountCreditPlusDTO.CreditPlusBalance -= creditsToBeTransferred;
                        pendingtoTransfer -= creditsToBeTransferred;
                        if (pendingtoTransfer == 0)
                        {
                            break;
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
        public decimal ConvertPointsToTime(decimal creditsToConvert, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(creditsToConvert);
            decimal finalTimeValue = 0;
            decimal pendingtoTransfer = creditsToConvert;
            decimal creditsToBeTransferred = 0;
            decimal ticketAllowedPoints = 0;
            decimal ticketNotAllowedPoints = 0;
            decimal timePerCredit;
            decimal totalCredits = 0;


            if (IsAccountUpdatedByOthers(this.accountDTO.LastUpdateDate, sqlTransaction))
            {
                log.Error(MessageContainerList.GetMessage(executionContext, 354) + accountDTO.AccountId);
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 354));
            }
            try
            {
                timePerCredit = Convert.ToDecimal(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "TIME_IN_MINUTES_PER_CREDIT"));
            }
            catch (Exception ex)
            {
                log.Error("Unable to find valid value for timePerCredit", ex);
                timePerCredit = 0;
                log.LogVariableState("timePerCredit", timePerCredit);
            }
            if (timePerCredit <= 0)
            {
                string message = MessageContainerList.GetMessage(executionContext, 1339);
                log.Error(MessageContainerList.GetMessage(executionContext, message));
                throw new ValidationException(message);
            }

            if (this.accountDTO.Credits != null)
            {
                totalCredits += (decimal)this.accountDTO.Credits;
            }
            if (this.accountDTO.AccountSummaryDTO != null)
            {
                if (this.accountDTO.AccountSummaryDTO.CreditPlusCardBalance != null)
                {
                    totalCredits += (decimal)accountDTO.AccountSummaryDTO.CreditPlusCardBalance;
                }
                if (this.accountDTO.AccountSummaryDTO.CreditPlusGamePlayCredits != null)
                {
                    totalCredits += (decimal)accountDTO.AccountSummaryDTO.CreditPlusGamePlayCredits;
                }
            }
            if (creditsToConvert > totalCredits)
            {
                log.Error(MessageContainerList.GetMessage(executionContext, 1383));
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1383));
            }

            if (pendingtoTransfer > 0)
            {
                if (accountDTO.Credits > 0)
                {
                    creditsToBeTransferred = accountDTO.Credits >= pendingtoTransfer ? pendingtoTransfer : Convert.ToDecimal(accountDTO.Credits.ToString());
                    if (accountDTO.TicketAllowed)
                    {
                        ticketAllowedPoints += creditsToBeTransferred;
                    }
                    else
                    {
                        ticketNotAllowedPoints += creditsToBeTransferred;
                    }
                    accountDTO.Credits -= creditsToBeTransferred;
                    pendingtoTransfer -= creditsToBeTransferred;
                }

                if (pendingtoTransfer > 0)
                {
                    if (this.AccountDTO.AccountCreditPlusDTOList != null)
                    {
                        List<AccountCreditPlusDTO> sourceAccountCreditPlusDTOList = this.AccountDTO.AccountCreditPlusDTOList.FindAll(x => (x.CreditPlusType == CreditPlusType.CARD_BALANCE
                        || x.CreditPlusType == CreditPlusType.GAME_PLAY_CREDIT)
                        && x.CreditPlusBalance > 0
                        && x.ValidityStatus != AccountDTO.AccountValidityStatus.Hold
                        && (x.PeriodFrom == null || x.PeriodFrom <= ServerDateTime.Now)
                        && (x.PeriodTo == null || x.PeriodTo >= ServerDateTime.Now));
                        List<AccountCreditPlusDTO> sortedAccountCreditPlusDTOListlist = sourceAccountCreditPlusDTOList.OrderBy(x => x.PeriodTo == null ? DateTime.MaxValue : x.PeriodTo).ToList();
                        foreach (AccountCreditPlusDTO sourceAccountCreditPlusDTO in sortedAccountCreditPlusDTOListlist)
                        {
                            if (sourceAccountCreditPlusDTO.CreditPlusBalance > 0)
                            {
                                creditsToBeTransferred = sourceAccountCreditPlusDTO.CreditPlusBalance >= pendingtoTransfer ? pendingtoTransfer : Convert.ToDecimal(sourceAccountCreditPlusDTO.CreditPlusBalance);
                                if (sourceAccountCreditPlusDTO.TicketAllowed)
                                {
                                    ticketAllowedPoints += creditsToBeTransferred;
                                }
                                else
                                {
                                    ticketNotAllowedPoints += creditsToBeTransferred;
                                }
                                sourceAccountCreditPlusDTO.CreditPlusBalance -= creditsToBeTransferred;
                                pendingtoTransfer -= creditsToBeTransferred;
                                if (pendingtoTransfer == 0)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            int expiryDays;
            try
            {
                expiryDays = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CONVERTED_TIME_ENTITLEMENT_VALID_FOR_DAYS"));
            }
            catch (Exception ex)
            {
                log.Error("Unable to find valid value for expiryDays", ex);
                expiryDays = 1;
                log.LogVariableState("expiryDays", expiryDays);
            }
            DateTime periodTo = DateTime.Today.AddHours(ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "BUSINESS_DAY_START_TIME")).AddDays(expiryDays);
            //Creates Credit Plus with TicketAllowed as true
            if (ticketAllowedPoints > 0)
            {
                //create cp credits, ticketallowed)
                // conversion logic & round off , create crP
                decimal timeValue = Convert.ToInt32(ticketAllowedPoints * timePerCredit);
                timeValue = GetTimeRoundOffValue(timeValue);

                AccountCreditPlusDTO genericCreditPlusDTO = new AccountCreditPlusDTO(-1, timeValue, CreditPlusType.TIME, false,
                "Exchange Credits for Time", accountDTO.AccountId, -1, -1, timeValue,
                null, periodTo, null, null, null, true, true, true, true, true, true, true, null, -1, false, null, true, false, false, -1, -1, ServerDateTime.Now, "",
                ServerDateTime.Now, accountDTO.SiteId, -1, false, "", true, true, "", -1, AccountDTO.AccountValidityStatus.Valid, -1);
                genericCreditPlusDTO.IsChanged = true;
                if (accountDTO.AccountCreditPlusDTOList == null)
                    accountDTO.AccountCreditPlusDTOList = new List<AccountCreditPlusDTO>();
                accountDTO.AccountCreditPlusDTOList.Add(genericCreditPlusDTO);
                finalTimeValue += timeValue;
            }
            //Creates Credit Plus with TicketAllowed as false
            if (ticketNotAllowedPoints > 0)
            {
                decimal timeValue = Convert.ToInt32(ticketNotAllowedPoints * timePerCredit);
                timeValue = GetTimeRoundOffValue(timeValue);
                AccountCreditPlusDTO genericCreditPlusDTO = new AccountCreditPlusDTO(-1, timeValue, CreditPlusType.TIME, false,
                "Exchange Credits for Time", accountDTO.AccountId, -1, -1, timeValue,
                null, periodTo, null, null, null, true, true, true, true, true, true, true, null, -1, false, null, false, false, false, -1, -1, ServerDateTime.Now, "",
                ServerDateTime.Now, accountDTO.SiteId, -1, false, "", true, true, "", -1, AccountDTO.AccountValidityStatus.Valid, -1);
                genericCreditPlusDTO.IsChanged = true;
                if (accountDTO.AccountCreditPlusDTOList == null)
                    accountDTO.AccountCreditPlusDTOList = new List<AccountCreditPlusDTO>();
                accountDTO.AccountCreditPlusDTOList.Add(genericCreditPlusDTO);
                finalTimeValue += timeValue;
            }
            log.LogMethodExit(finalTimeValue);
            return finalTimeValue;
        }

        private decimal GetTimeRoundOffValue(decimal timeValue)
        {
            log.LogMethodEntry(timeValue);
            int tokenRoundOffAmountTo = -1;
            LookupsContainerDTO lookupsContainerDTO = LookupsContainerList.GetLookupsContainerDTO(executionContext.GetSiteId(), "ALOHA_ROUNDOFF_AMOUNT_TO", executionContext);
            object roundoffObj = lookupsContainerDTO.LookupValuesContainerDTOList.FirstOrDefault().LookupValue;
            if (roundoffObj != null && roundoffObj != DBNull.Value && !String.IsNullOrEmpty(roundoffObj.ToString()))
            {
                tokenRoundOffAmountTo = Convert.ToInt32(roundoffObj);
            }

            if (tokenRoundOffAmountTo != -1 && tokenRoundOffAmountTo != 0)
            {
                timeValue = Math.Round(timeValue, tokenRoundOffAmountTo);
            }
            else
            {
                timeValue = Math.Round(timeValue, 0);
            }
            log.LogMethodExit(timeValue);
            return timeValue;
        }

        public decimal ConvertTimeToPoints(decimal timeToConvert, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(timeToConvert);
            decimal pendingtoTransfer = timeToConvert;
            decimal timePerCredit;
            decimal totalTime = 0;
            decimal ticketAllowedTime = 0;
            decimal ticketNotAllowedTime = 0;
            decimal timeToBeTransferred = 0;
            decimal finalCreditValue = 0;
            if (IsAccountUpdatedByOthers(this.accountDTO.LastUpdateDate, sqlTransaction))
            {
                log.Error(MessageContainerList.GetMessage(executionContext, 354) + accountDTO.AccountId);
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 354));
            }
            try
            {
                timePerCredit = Convert.ToDecimal(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "TIME_IN_MINUTES_PER_CREDIT"));
            }
            catch (Exception ex)
            {
                log.Error("Unable to find valid value for timePerCredit", ex);
                timePerCredit = 0;
                log.LogVariableState("timePerCredit", timePerCredit);
            }
            if (timePerCredit <= 0)
            {
                string message = MessageContainerList.GetMessage(executionContext, 1339);
                log.Error(MessageContainerList.GetMessage(executionContext, message));
                throw new ValidationException(message);
            }

            if (this.accountDTO.Time != null)
            {
                totalTime += (decimal)this.accountDTO.Time;
            }
            if (this.accountDTO.AccountSummaryDTO != null)
            {
                if (this.accountDTO.AccountSummaryDTO.CreditPlusTime != null)
                {
                    totalTime += (decimal)this.accountDTO.AccountSummaryDTO.CreditPlusTime;
                }
            }
            if (timeToConvert > totalTime)
            {
                log.Error(MessageContainerList.GetMessage(executionContext, 1441));
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1441));
            }

            if (pendingtoTransfer > 0)
            {
                if (this.AccountDTO.AccountCreditPlusDTOList != null)
                {
                    List<AccountCreditPlusDTO> sourceAccountCreditPlusDTOList = this.AccountDTO.AccountCreditPlusDTOList.FindAll(x => x.CreditPlusType == CreditPlusType.TIME
                    && x.CreditPlusBalance > 0
                    && x.ValidityStatus != AccountDTO.AccountValidityStatus.Hold
                    && (x.PeriodFrom == null || x.PeriodFrom <= ServerDateTime.Now)
                    && (x.PeriodTo == null || x.PeriodTo >= ServerDateTime.Now));
                    List<AccountCreditPlusDTO> sortedAccountCreditPlusDTOListlist = sourceAccountCreditPlusDTOList.OrderBy(x => x.PeriodTo == null ? DateTime.MaxValue : x.PeriodTo).ToList();
                    foreach (AccountCreditPlusDTO sourceAccountCreditPlusDTO in sortedAccountCreditPlusDTOListlist)
                    {
                        AccountCreditPlusBL accountCreditPlusBL = new AccountCreditPlusBL(executionContext, sourceAccountCreditPlusDTO);
                        decimal creditPlusTime = accountCreditPlusBL.GetCreditPlusTime(ServerDateTime.Now);
                        if (creditPlusTime > 0)
                        {
                            if (pendingtoTransfer > creditPlusTime)
                            {
                                timeToBeTransferred = creditPlusTime;
                                pendingtoTransfer -= creditPlusTime;
                                sourceAccountCreditPlusDTO.CreditPlusBalance = 0;
                            }
                            else
                            {
                                timeToBeTransferred = pendingtoTransfer;
                                sourceAccountCreditPlusDTO.CreditPlusBalance = creditPlusTime - pendingtoTransfer;
                                pendingtoTransfer = 0;
                            }
                            if (sourceAccountCreditPlusDTO.TicketAllowed)
                            {
                                ticketAllowedTime += timeToBeTransferred;
                            }
                            else
                            {
                                ticketNotAllowedTime += timeToBeTransferred;
                            }
                            if (pendingtoTransfer <= 0)
                            {
                                break;
                            }
                        }
                    }
                }
                if (pendingtoTransfer > 0)
                {
                    if (accountDTO.Time >= (decimal?)pendingtoTransfer)
                    {
                        accountDTO.Time -= (decimal?)pendingtoTransfer;
                        if (accountDTO.TicketAllowed)
                        {
                            ticketAllowedTime += pendingtoTransfer;
                        }
                        else
                        {
                            ticketNotAllowedTime += pendingtoTransfer;
                        }
                    }
                    else
                    {
                        throw new ValidationException("Time not available..Please Check");
                    }
                }
            }

            if (ticketAllowedTime > 0)
            {
                decimal creditValue = Convert.ToDecimal(ticketAllowedTime / timePerCredit);
                creditValue = GetCreditRoundOffValue(creditValue);
                AccountCreditPlusDTO genericCreditPlusDTO = new AccountCreditPlusDTO(-1, creditValue, CreditPlusType.CARD_BALANCE, false,
                "Exchange Time for Credits", accountDTO.AccountId, -1, -1, creditValue,
                null, null, null, null, null, true, true, true, true, true, true, true, null, -1, false, null, true, false, false, -1, -1, ServerDateTime.Now, "",
                ServerDateTime.Now, accountDTO.SiteId, -1, false, "", false, true, "", -1, AccountDTO.AccountValidityStatus.Valid, -1);
                genericCreditPlusDTO.IsChanged = true;
                if (accountDTO.AccountCreditPlusDTOList == null)
                    accountDTO.AccountCreditPlusDTOList = new List<AccountCreditPlusDTO>();
                accountDTO.AccountCreditPlusDTOList.Add(genericCreditPlusDTO);
                finalCreditValue += creditValue;
            }
            if (ticketNotAllowedTime > 0)
            {
                decimal creditValue = Convert.ToDecimal(ticketNotAllowedTime / timePerCredit);
                creditValue = GetCreditRoundOffValue(creditValue);
                AccountCreditPlusDTO genericCreditPlusDTO = new AccountCreditPlusDTO(-1, creditValue, CreditPlusType.CARD_BALANCE, false,
               "Exchange Time for Credits", accountDTO.AccountId, -1, -1, creditValue,
                null, null, null, null, null, true, true, true, true, true, true, true, null, -1, false, null, false, false, false, -1, -1, ServerDateTime.Now, "",
                ServerDateTime.Now, accountDTO.SiteId, -1, false, "", false, true, "", -1, AccountDTO.AccountValidityStatus.Valid, -1);
                genericCreditPlusDTO.IsChanged = true;
                if (accountDTO.AccountCreditPlusDTOList == null)
                    accountDTO.AccountCreditPlusDTOList = new List<AccountCreditPlusDTO>();
                accountDTO.AccountCreditPlusDTOList.Add(genericCreditPlusDTO);
                finalCreditValue += creditValue;
            }
            log.LogMethodExit(finalCreditValue);
            return finalCreditValue;
        }
        private decimal GetCreditRoundOffValue(decimal creditsValue)
        {
            log.LogMethodEntry(creditsValue);
            int tokenRoundOffAmountTo = -1;
            LookupsContainerDTO lookupsContainerDTO = LookupsContainerList.GetLookupsContainerDTO(executionContext.GetSiteId(), "ALOHA_ROUNDOFF_AMOUNT_TO", executionContext);
            object roundoffObj = lookupsContainerDTO.LookupValuesContainerDTOList.FirstOrDefault().LookupValue;
            if (roundoffObj != null && roundoffObj != DBNull.Value && !String.IsNullOrEmpty(roundoffObj.ToString()))
            {
                tokenRoundOffAmountTo = Convert.ToInt32(roundoffObj);
            }

            if (tokenRoundOffAmountTo != -1)
            {
                if (tokenRoundOffAmountTo == 0 || creditsValue % tokenRoundOffAmountTo == 0)
                {
                    creditsValue = Convert.ToInt32(creditsValue);
                }
                else
                {
                    creditsValue = Convert.ToInt32(creditsValue + tokenRoundOffAmountTo - (creditsValue % tokenRoundOffAmountTo));
                }
            }
            log.LogMethodExit(creditsValue);
            return creditsValue;
        }

        public int RedeemBonusforTickets(decimal bonusToRedeem, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(bonusToRedeem);
            decimal ticketsToRedeem = GetTicketsToRedeemPerBonus();
            decimal ticketAllowedBonus = 0;
            decimal ticketNotAllowedBonus = 0;
            int ticketsEligible = 0;

            if (IsAccountUpdatedByOthers(this.accountDTO.LastUpdateDate, sqlTransaction))
            {
                log.Error(MessageContainerList.GetMessage(executionContext, 354) + this.accountDTO.AccountId);
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 354));
            }
            decimal totalbonus = Convert.ToDecimal((accountDTO.Bonus == null ? 0 : accountDTO.Bonus) + (accountDTO.AccountSummaryDTO.CreditPlusBonus == null ? 0 : accountDTO.AccountSummaryDTO.CreditPlusBonus));
            if (bonusToRedeem > totalbonus)
            {
                log.LogVariableState("redeemEntitlementDTO.FromValue", bonusToRedeem);
                log.LogVariableState("totalbonus", totalbonus);
                log.Error(MessageContainerList.GetMessage(executionContext, 354));
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 354));
            }

            decimal? remainingBonus = bonusToRedeem;
            if (this.AccountDTO.AccountCreditPlusDTOList != null)
            {
                List<AccountCreditPlusDTO> accountCreditPlusDTOList = this.AccountDTO.AccountCreditPlusDTOList.FindAll(x => x.CreditPlusType == CreditPlusType.GAME_PLAY_BONUS
                    && x.CreditPlusBalance > 0
                    && x.ValidityStatus != AccountDTO.AccountValidityStatus.Hold
                    && (x.PeriodFrom == null || x.PeriodFrom <= ServerDateTime.Now)
                    && (x.PeriodTo == null || x.PeriodTo >= ServerDateTime.Now));

                List<AccountCreditPlusDTO> sortedAccountCreditPlusDTOListlist = accountCreditPlusDTOList.OrderBy(x => x.PeriodTo == null ? DateTime.MaxValue : x.PeriodTo).ToList();

                foreach (AccountCreditPlusDTO sourceAccountCreditPlusDTO in sortedAccountCreditPlusDTOListlist)
                {
                    if (sourceAccountCreditPlusDTO.CreditPlusBalance != null && sourceAccountCreditPlusDTO.CreditPlusBalance > 0)
                    {
                        decimal? bonusToBeTransferred = sourceAccountCreditPlusDTO.CreditPlusBalance >= remainingBonus ? remainingBonus : sourceAccountCreditPlusDTO.CreditPlusBalance;
                        sourceAccountCreditPlusDTO.CreditPlusBalance -= bonusToBeTransferred;
                        remainingBonus -= bonusToBeTransferred;
                        if (sourceAccountCreditPlusDTO.TicketAllowed)
                        {
                            ticketAllowedBonus += (decimal)bonusToBeTransferred;
                        }
                        else
                        {
                            ticketNotAllowedBonus += (decimal)bonusToBeTransferred;
                        }
                        if (remainingBonus == 0)
                        {
                            break;
                        }
                    }
                }
            }

            if (remainingBonus > 0)
            {
                if (accountDTO.Bonus >= remainingBonus)
                {
                    accountDTO.Bonus -= remainingBonus;
                    if (this.accountDTO.TicketAllowed)
                    {
                        ticketAllowedBonus += (decimal)remainingBonus;
                    }
                    else
                    {
                        ticketNotAllowedBonus += (decimal)remainingBonus;
                    }
                }
                else
                {
                    throw new ValidationException("Bonus not available..Please Check");
                }
            }
            if (ticketAllowedBonus > 0)
            {
                int tickets = Convert.ToInt32((ticketAllowedBonus) * ticketsToRedeem);
                ticketsEligible += tickets;
                AccountCreditPlusDTO genericCreditPlusDTO = new AccountCreditPlusDTO(-1, tickets, CreditPlusType.TICKET, false,
                "Redeem Bonus for Ticket", accountDTO.AccountId, -1, -1, tickets, null, null, null, null, null, true, true, true, true, true, true, true, null, -1, false, null, true, false, false, -1, -1, ServerDateTime.Now, "",
                ServerDateTime.Now, accountDTO.SiteId, -1, false, "", false, true, "", -1, AccountDTO.AccountValidityStatus.Valid, -1);
                genericCreditPlusDTO.IsChanged = true;
                if (accountDTO.AccountCreditPlusDTOList == null)
                    accountDTO.AccountCreditPlusDTOList = new List<AccountCreditPlusDTO>();
                accountDTO.AccountCreditPlusDTOList.Add(genericCreditPlusDTO);
            }
            if (ticketNotAllowedBonus > 0)
            {
                int tickets = Convert.ToInt32((ticketNotAllowedBonus) * ticketsToRedeem);
                ticketsEligible += tickets;
                AccountCreditPlusDTO genericCreditPlusDTO = new AccountCreditPlusDTO(-1, tickets, CreditPlusType.TICKET, false,
                "Redeem Bonus for Ticket", accountDTO.AccountId, -1, -1, tickets, null, null, null, null, null, true, true, true, true, true, true, true, null, -1, false, null, false, false, false, -1, -1, ServerDateTime.Now, "",
                ServerDateTime.Now, accountDTO.SiteId, -1, false, "", false, true, "", -1, AccountDTO.AccountValidityStatus.Valid, -1);
                genericCreditPlusDTO.IsChanged = true;
                if (accountDTO.AccountCreditPlusDTOList == null)
                    accountDTO.AccountCreditPlusDTOList = new List<AccountCreditPlusDTO>();
                accountDTO.AccountCreditPlusDTOList.Add(genericCreditPlusDTO);
            }
            log.LogMethodExit(ticketsEligible);
            return ticketsEligible;
        }

        private decimal GetTicketsToRedeemPerBonus()
        {
            log.LogMethodEntry();
            decimal ticketsToRedeem = 0;
            try
            {
                ticketsToRedeem = ParafaitDefaultContainerList.GetParafaitDefault<decimal>(executionContext, "TICKETS_TO_REDEEM_PER_BONUS");
                if (ticketsToRedeem <= 0)
                {
                    ticketsToRedeem = 100;
                }
            }
            catch (Exception ex)
            {

                log.Error("ticketsToRedeem doesn't have a valid value! ", ex);
                ticketsToRedeem = 100;
                log.LogVariableState("ticketsToRedeem ", ticketsToRedeem);
            }
            log.LogMethodExit(ticketsToRedeem);
            return ticketsToRedeem;
        }

        public decimal RedeemTicketsforBonus(int ticketsToRedeem, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(ticketsToRedeem);
            decimal bonusEligible = 0;
            int ticketAllowedTickets = 0;
            int ticketNotAllowedTickets = 0;
            if (IsAccountUpdatedByOthers(this.accountDTO.LastUpdateDate, sqlTransaction))
            {
                log.Error(MessageContainerList.GetMessage(executionContext, 354) + this.accountDTO.AccountId);
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 354));
            }

            int totalTickets = Convert.ToInt32((this.accountDTO.TicketCount == null ? 0 : (this.accountDTO.TicketCount)) + (this.accountDTO.AccountSummaryDTO.CreditPlusTickets == null ? 0 : (this.accountDTO.AccountSummaryDTO.CreditPlusTickets)));
            if (ticketsToRedeem > totalTickets)
            {
                log.LogVariableState("redeemTickets", ticketsToRedeem);
                log.LogVariableState("totalTickets", totalTickets);
                log.Error(MessageContainerList.GetMessage(executionContext, 50));
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 50, MessageContainerList.GetMessage(executionContext, ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "REDEMPTION_TICKET_NAME_VARIANT", "Tickets"))));
            }

            decimal ticketsToRedeemperBonus = 0;
            try
            {
                ticketsToRedeemperBonus = ParafaitDefaultContainerList.GetParafaitDefault<decimal>(executionContext, "TICKETS_TO_REDEEM_PER_BONUS");
                if (ticketsToRedeemperBonus <= 0)
                {
                    ticketsToRedeemperBonus = 100;
                }
            }
            catch (Exception ex)
            {
                log.Error("ticketsToRedeem doesn't have a valid value! ", ex);
                ticketsToRedeemperBonus = 100;
                log.LogVariableState("ticketsToRedeem ", ticketsToRedeemperBonus);
            }

            decimal loadBonusLimitValue = 0;
            try
            {
                loadBonusLimitValue = (ParafaitDefaultContainerList.GetParafaitDefault<decimal>(executionContext, "LOAD_BONUS_LIMIT"));
            }
            catch (Exception ex)
            {
                log.Error("Unable to get a valid value for loadBonusLimit", ex);
                loadBonusLimitValue = 0;
                log.LogVariableState("loadBonusLimit", loadBonusLimitValue);
            }
            decimal totalBonusEligibleValue = ticketsToRedeem / ticketsToRedeemperBonus;
            if (totalBonusEligibleValue > loadBonusLimitValue)
            {
                log.LogVariableState("bonusEligibleValue", totalBonusEligibleValue);
                log.Error(MessageContainerList.GetMessage(executionContext, 43) + loadBonusLimitValue.ToString(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT")));
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 43, loadBonusLimitValue.ToString(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT"))));
            }

            //if (bonusEligible > 0)
            //{
            //    bool ticketAllowed = this.accountDTO.TicketAllowed;
            //    AccountCreditPlusDTO genericCreditPlusDTO = new AccountCreditPlusDTO(-1, bonusEligible, CreditPlusType.GAME_PLAY_BONUS, false, "Redeem Tickets for bonus", accountDTO.AccountId, -1, -1, bonusEligible, null, null, null, null, null, true, true, true, true, true, true, true, null, -1, false, null, ticketAllowed, false, false, -1, -1, ServerDateTime.Now, "",
            //    ServerDateTime.Now, accountDTO.SiteId, -1, false, "", false, true, "", -1, AccountDTO.AccountValidityStatus.Valid, -1);
            //    genericCreditPlusDTO.IsChanged = true;
            //    if (accountDTO.AccountCreditPlusDTOList == null)
            //        accountDTO.AccountCreditPlusDTOList = new List<AccountCreditPlusDTO>();
            //    accountDTO.AccountCreditPlusDTOList.Add(genericCreditPlusDTO);
            //}
            decimal? remainingTickets = ticketsToRedeem;
            if (this.AccountDTO.AccountCreditPlusDTOList != null)
            {
                List<AccountCreditPlusDTO> accountCreditPlusDTOList = this.AccountDTO.AccountCreditPlusDTOList.FindAll(x => x.CreditPlusType == CreditPlusType.TICKET
                                                                      && x.CreditPlusBalance > 0
                                                                      && x.ValidityStatus != AccountDTO.AccountValidityStatus.Hold
                                                                      && (x.PeriodFrom == null || x.PeriodFrom <= ServerDateTime.Now)
                                                                      && (x.PeriodTo == null || x.PeriodTo >= ServerDateTime.Now));
                List<AccountCreditPlusDTO> sortedAccountCreditPlusDTOListlist = accountCreditPlusDTOList.OrderBy(x => x.PeriodTo == null ? DateTime.MaxValue : x.PeriodTo).ToList();
                foreach (AccountCreditPlusDTO sourceAccountCreditPlusDTO in sortedAccountCreditPlusDTOListlist)
                {
                    if (sourceAccountCreditPlusDTO.CreditPlusBalance != null && sourceAccountCreditPlusDTO.CreditPlusBalance > 0)
                    {
                        decimal? ticketsToBeTransferred = sourceAccountCreditPlusDTO.CreditPlusBalance >= remainingTickets ? remainingTickets : sourceAccountCreditPlusDTO.CreditPlusBalance;
                        sourceAccountCreditPlusDTO.CreditPlusBalance -= ticketsToBeTransferred;
                        int tickets = 0;
                        if (ticketsToBeTransferred > 0)
                        {
                            tickets = Convert.ToInt32(ticketsToBeTransferred);
                        }
                        remainingTickets -= tickets;
                        if (sourceAccountCreditPlusDTO.TicketAllowed)
                        {
                            ticketAllowedTickets += tickets;
                        }
                        else
                        {
                            ticketNotAllowedTickets += tickets;
                        }
                        if (remainingTickets == 0)
                        {
                            break;
                        }
                    }
                }
            }

            if (remainingTickets > 0)
            {
                if (accountDTO.TicketCount >= Convert.ToInt32(remainingTickets))
                {
                    int tickets = Convert.ToInt32(remainingTickets);
                    accountDTO.TicketCount -= tickets;
                    if (this.accountDTO.TicketAllowed)
                    {
                        ticketAllowedTickets += tickets;
                    }
                    else
                    {
                        ticketNotAllowedTickets += tickets;
                    }
                }
                else
                {
                    throw new ValidationException("Tickets not available..Please Check");
                }
            }
            if (ticketAllowedTickets > 0)
            {
                decimal bonus = Decimal.Round((ticketAllowedTickets / ticketsToRedeemperBonus), 4);
                bonusEligible += bonus;
                AccountCreditPlusDTO genericCreditPlusDTO = new AccountCreditPlusDTO(-1, bonus, CreditPlusType.GAME_PLAY_BONUS, false, "Redeem Tickets for bonus", accountDTO.AccountId, -1, -1, bonus, null, null, null, null, null, true, true, true, true, true, true, true, null, -1, false, null, true, false, false, -1, -1, ServerDateTime.Now, "",
                ServerDateTime.Now, accountDTO.SiteId, -1, false, "", false, true, "", -1, AccountDTO.AccountValidityStatus.Valid, -1);
                genericCreditPlusDTO.IsChanged = true;
                if (accountDTO.AccountCreditPlusDTOList == null)
                    accountDTO.AccountCreditPlusDTOList = new List<AccountCreditPlusDTO>();
                accountDTO.AccountCreditPlusDTOList.Add(genericCreditPlusDTO);
            }
            if (ticketNotAllowedTickets > 0)
            {
                decimal bonus = Decimal.Round((ticketNotAllowedTickets / ticketsToRedeemperBonus), 4);
                bonusEligible += bonus;
                AccountCreditPlusDTO genericCreditPlusDTO = new AccountCreditPlusDTO(-1, bonus, CreditPlusType.GAME_PLAY_BONUS, false, "Redeem Tickets for bonus", accountDTO.AccountId, -1, -1, bonus, null, null, null, null, null, true, true, true, true, true, true, true, null, -1, false, null, false, false, false, -1, -1, ServerDateTime.Now, "",
                ServerDateTime.Now, accountDTO.SiteId, -1, false, "", false, true, "", -1, AccountDTO.AccountValidityStatus.Valid, -1);
                genericCreditPlusDTO.IsChanged = true;
                if (accountDTO.AccountCreditPlusDTOList == null)
                    accountDTO.AccountCreditPlusDTOList = new List<AccountCreditPlusDTO>();
                accountDTO.AccountCreditPlusDTOList.Add(genericCreditPlusDTO);
            }
            log.LogMethodExit(bonusEligible);
            return bonusEligible;
        }

        public decimal RedeemTicketsforCredits(int ticketsToRedeem, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(ticketsToRedeem);
            decimal creditsEligible = 0;
            int ticketAllowedTickets = 0;
            int ticketNotAllowedTickets = 0;
            if (IsAccountUpdatedByOthers(this.accountDTO.LastUpdateDate, sqlTransaction))
            {
                log.Error(MessageContainerList.GetMessage(executionContext, 354) + this.accountDTO.AccountId);
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 354));
            }
            int totalTickets = Convert.ToInt32((this.accountDTO.TicketCount == null ? 0 : (this.accountDTO.TicketCount)) + (this.accountDTO.AccountSummaryDTO.CreditPlusTickets == null ? 0 : (this.accountDTO.AccountSummaryDTO.CreditPlusTickets)));
            if (ticketsToRedeem > totalTickets)
            {
                log.LogVariableState("redeemTickets", ticketsToRedeem);
                log.LogVariableState("totalTickets", totalTickets);
                log.Error(MessageContainerList.GetMessage(executionContext, 50));
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 50, MessageContainerList.GetMessage(executionContext, ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "REDEMPTION_TICKET_NAME_VARIANT", "Tickets"))));
            }
            decimal ticketsToRedeemPerCredit = 0;
            try
            {
                ticketsToRedeemPerCredit = ParafaitDefaultContainerList.GetParafaitDefault<decimal>(executionContext, "TICKETS_TO_REDEEM_PER_CREDIT");
            }
            catch (Exception ex)
            {

                log.Error("ticketsToRedeem doesn't have a valid value! ", ex);
                ticketsToRedeemPerCredit = 0;
                log.LogVariableState("ticketsToRedeem ", ticketsToRedeemPerCredit);
            }

            decimal gameCardCreditLimit = 0;
            try
            {
                gameCardCreditLimit = Convert.ToDecimal(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "GAMECARD_CREDIT_LIMIT"));
            }
            catch (Exception ex)
            {
                log.Error("gameCardCreditLimit doesn't have a valid value! ", ex);
                gameCardCreditLimit = 0;
                log.LogVariableState("gameCardCreditLimit ", gameCardCreditLimit);
            }
            decimal totalCreditsEligible = ticketsToRedeem / ticketsToRedeemPerCredit;
            decimal cardCreditsLoading = Convert.ToDecimal(totalCreditsEligible);
            if (accountDTO.Credits != null)
            {
                cardCreditsLoading += Convert.ToDecimal(accountDTO.Credits);
            }
            if (accountDTO.AccountSummaryDTO.CreditPlusCardBalance != null)
            {
                cardCreditsLoading += Convert.ToDecimal(accountDTO.AccountSummaryDTO.CreditPlusCardBalance);
            }
            if (accountDTO.AccountSummaryDTO.CreditPlusGamePlayCredits != null)
            {
                cardCreditsLoading += Convert.ToDecimal(accountDTO.AccountSummaryDTO.CreditPlusGamePlayCredits);
            }
            if (gameCardCreditLimit > 0)
            {
                if (totalCreditsEligible != 0)
                {
                    if (cardCreditsLoading > gameCardCreditLimit)
                    {
                        log.Error(MessageContainerList.GetMessage(executionContext, 1168));
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1168));
                    }
                }
            }

            int? remainingTickets = ticketsToRedeem;
            if (this.accountDTO.AccountCreditPlusDTOList != null)
            {
                List<AccountCreditPlusDTO> accountCreditPlusDTOList = this.accountDTO.AccountCreditPlusDTOList.FindAll(x => x.CreditPlusType == CreditPlusType.TICKET
                && x.CreditPlusBalance > 0
                && x.ValidityStatus != AccountDTO.AccountValidityStatus.Hold
                && (x.PeriodFrom == null || x.PeriodFrom <= ServerDateTime.Now)
                && (x.PeriodTo == null || x.PeriodTo >= ServerDateTime.Now));
                List<AccountCreditPlusDTO> sortedAccountCreditPlusDTOListlist = accountCreditPlusDTOList.OrderBy(x => x.PeriodTo == null ? DateTime.MaxValue : x.PeriodTo).ToList();
                foreach (AccountCreditPlusDTO sourceAccountCreditPlusDTO in sortedAccountCreditPlusDTOListlist)
                {
                    if (sourceAccountCreditPlusDTO.CreditPlusBalance != null && sourceAccountCreditPlusDTO.CreditPlusBalance > 0)
                    {
                        int? ticketsToBeTransferred = sourceAccountCreditPlusDTO.CreditPlusBalance >= remainingTickets ? (int?)remainingTickets : Convert.ToInt32(sourceAccountCreditPlusDTO.CreditPlusBalance);
                        sourceAccountCreditPlusDTO.CreditPlusBalance -= ticketsToBeTransferred;
                        remainingTickets -= ticketsToBeTransferred;
                        if (sourceAccountCreditPlusDTO.TicketAllowed)
                        {
                            ticketAllowedTickets += (int)ticketsToBeTransferred;
                        }
                        else
                        {
                            ticketNotAllowedTickets += (int)ticketsToBeTransferred;
                        }
                        if (remainingTickets == 0)
                        {
                            break;
                        }
                    }
                }
            }

            if (remainingTickets > 0)
            {
                if (accountDTO.TicketCount >= remainingTickets)
                {
                    accountDTO.TicketCount -= (int?)remainingTickets;
                    if (this.accountDTO.TicketAllowed)
                    {
                        ticketAllowedTickets += (int)remainingTickets;
                    }
                    else
                    {
                        ticketNotAllowedTickets += (int)remainingTickets;
                    }
                }
                else
                {
                    throw new ValidationException("Tickets not available..Please Check");
                }
            }
            if (ticketAllowedTickets > 0)
            {
                decimal credits = ticketAllowedTickets / ticketsToRedeemPerCredit;
                creditsEligible += credits;
                AccountCreditPlusDTO genericCreditPlusDTO = new AccountCreditPlusDTO(-1, credits, CreditPlusType.CARD_BALANCE, false,
                "Redeem Tickets for credit", accountDTO.AccountId, -1, -1, credits, null, null, null, null, null, true, true, true, true, true, true, true, null, -1, false, null, true, false, false, -1, -1, ServerDateTime.Now, "",
                ServerDateTime.Now, accountDTO.SiteId, -1, false, "", false, true, "", -1, AccountDTO.AccountValidityStatus.Valid, -1);
                genericCreditPlusDTO.IsChanged = true;
                if (accountDTO.AccountCreditPlusDTOList == null)
                    accountDTO.AccountCreditPlusDTOList = new List<AccountCreditPlusDTO>();
                accountDTO.AccountCreditPlusDTOList.Add(genericCreditPlusDTO);
            }
            if (ticketNotAllowedTickets > 0)
            {
                decimal credits = ticketNotAllowedTickets / ticketsToRedeemPerCredit;
                creditsEligible += credits;
                AccountCreditPlusDTO genericCreditPlusDTO = new AccountCreditPlusDTO(-1, credits, CreditPlusType.CARD_BALANCE, false,
                "Redeem Tickets for credit", accountDTO.AccountId, -1, -1, credits, null, null, null, null, null, true, true, true, true, true, true, true, null, -1, false, null, false, false, false, -1, -1, ServerDateTime.Now, "",
                ServerDateTime.Now, accountDTO.SiteId, -1, false, "", false, true, "", -1, AccountDTO.AccountValidityStatus.Valid, -1);
                genericCreditPlusDTO.IsChanged = true;
                if (accountDTO.AccountCreditPlusDTOList == null)
                    accountDTO.AccountCreditPlusDTOList = new List<AccountCreditPlusDTO>();
                accountDTO.AccountCreditPlusDTOList.Add(genericCreditPlusDTO);

            }

            log.LogMethodExit(creditsEligible);
            return creditsEligible;
        }

        private bool IsTicketAllowedForLoadBonus()
        {
            log.LogMethodEntry();
            bool ticketAllowed = false;
            ProductsContainerDTO productsContainerDTO = ProductsContainerList.GetSystemProductContainerDTO(executionContext.SiteId, ProductTypeValues.GENERICSALE, "Load Bonus Task");
            if (productsContainerDTO != null)
            {
                if (!string.IsNullOrWhiteSpace(productsContainerDTO.TicketAllowed) && productsContainerDTO.TicketAllowed == "Y")
                {
                    ticketAllowed = true;
                }
            }
            log.LogMethodExit(ticketAllowed);
            return ticketAllowed;
        }

        public void LoadBonus(CreditPlusType creditPlusType, decimal bonusValue, string remarks, int trxId)
        {
            log.LogMethodEntry(creditPlusType, bonusValue, remarks, trxId);
            if (bonusValue != 0)
            {
                if (bonusValue < 0)
                {
                    decimal creditsAvailable = 0;
                    switch (creditPlusType)
                    {
                        case CreditPlusType.CARD_BALANCE:
                            if (AccountDTO.Credits != null)
                            {
                                creditsAvailable = creditsAvailable + (decimal)AccountDTO.Credits;
                            }
                            if (AccountDTO.AccountSummaryDTO != null)
                            {
                                if (AccountDTO.AccountSummaryDTO.CreditPlusCardBalance != null)
                                {
                                    creditsAvailable = creditsAvailable + (decimal)AccountDTO.AccountSummaryDTO.CreditPlusCardBalance;
                                }
                            }
                            break;
                        case CreditPlusType.LOYALTY_POINT:
                            if (AccountDTO.AccountSummaryDTO != null)
                            {
                                if (AccountDTO.AccountSummaryDTO.TotalLoyaltyPointBalance != null)
                                {
                                    creditsAvailable = creditsAvailable + (decimal)AccountDTO.AccountSummaryDTO.TotalLoyaltyPointBalance;
                                }
                            }
                            break;
                        case CreditPlusType.GAME_PLAY_CREDIT:
                            if (AccountDTO.AccountSummaryDTO != null)
                            {
                                if (AccountDTO.AccountSummaryDTO.TotalGamePlayCreditsBalance != null)
                                {
                                    creditsAvailable = creditsAvailable + (decimal)AccountDTO.AccountSummaryDTO.TotalGamePlayCreditsBalance;
                                }
                            }
                            break;
                        case CreditPlusType.GAME_PLAY_BONUS:
                            if (AccountDTO.AccountSummaryDTO != null)
                            {
                                if (AccountDTO.AccountSummaryDTO.TotalBonusBalance != null)
                                {
                                    creditsAvailable = creditsAvailable + (decimal)AccountDTO.AccountSummaryDTO.TotalBonusBalance;
                                }
                            }
                            break;
                    }
                    if ((Convert.ToDouble(bonusValue) + Convert.ToDouble(creditsAvailable)) < 0)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 49, (bonusValue * -1), creditsAvailable));
                    }
                }
                int expiryDays;
                try
                {
                    expiryDays = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "LOAD_BONUS_EXPIRY_DAYS");
                }
                catch (Exception ex)
                {
                    log.Error("Unable to find valid value for expiryDays", ex);
                    expiryDays = 1;
                    log.LogVariableState("expiryDays", expiryDays);
                }
                DateTime periodTo = DateTime.MinValue;
                if (creditPlusType == CreditPlusType.GAME_PLAY_BONUS)
                {
                    double businessStart;
                    try
                    {
                        businessStart = Convert.ToDouble(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "BUSINESS_DAY_START_TIME"));
                    }
                    catch (Exception ex)
                    {
                        log.Error("Unable to find valid value for businessStart", ex);
                        businessStart = 6;
                        log.LogVariableState("businessStart", businessStart);
                    }
                    periodTo = expiryDays == 0 ? DateTime.MinValue : DateTime.Today.AddHours(businessStart).AddDays(expiryDays);
                }
                bool autoExtendonReload = false;
                if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AUTO_EXTEND_BONUS_ON_RELOAD") == "Y")
                {
                    autoExtendonReload = true;
                }
                bool ticketAllowed = IsTicketAllowedForLoadBonus();
                if (bonusValue > 0)
                {
                    AccountCreditPlusDTO genericCreditPlusDTO = new AccountCreditPlusDTO(-1, bonusValue, creditPlusType, false,
                (string.IsNullOrEmpty(remarks.Trim()) ? "Load Bonus" : remarks), accountDTO.AccountId, trxId, 1, bonusValue, null, null, null, null, null, true, true, true, true, true, true, true, null, -1,
                autoExtendonReload, null, ticketAllowed, false, false, -1, -1, ServerDateTime.Now, "",
                ServerDateTime.Now, accountDTO.SiteId, -1, false, "", false, true, "", -1, AccountDTO.AccountValidityStatus.Valid, -1);
                    genericCreditPlusDTO.IsChanged = true;
                    if (creditPlusType == CreditPlusType.GAME_PLAY_BONUS && periodTo != DateTime.MinValue)
                    {
                        genericCreditPlusDTO.PeriodTo = periodTo;
                    }
                    if (accountDTO.AccountCreditPlusDTOList == null)
                        accountDTO.AccountCreditPlusDTOList = new List<AccountCreditPlusDTO>();
                    accountDTO.AccountCreditPlusDTOList.Add(genericCreditPlusDTO);
                }
                else if (bonusValue < 0)
                {
                    List<AccountCreditPlusDTO> validAccountCreditPlusDTOs = new List<AccountCreditPlusDTO>();
                    LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                    DateTime currentTime = lookupValuesList.GetServerDateTime();
                    if (accountDTO.AccountCreditPlusDTOList != null && accountDTO.AccountCreditPlusDTOList.Any())
                    {
                        validAccountCreditPlusDTOs.AddRange(accountDTO.AccountCreditPlusDTOList.Where(cp => cp.CreditPlusType.Equals(creditPlusType) &&
                                                                                                                          !cp.ValidityStatus.Equals(AccountDTO.AccountValidityStatus.Hold) &&
                                                                                                                          (cp.PeriodFrom == null || (cp.PeriodFrom != null && cp.PeriodFrom <= currentTime)) &&
                                                                                                                          (cp.PeriodTo == null || (cp.PeriodTo != null && cp.PeriodTo >= currentTime))).ToList());
                        validAccountCreditPlusDTOs = validAccountCreditPlusDTOs.OrderBy(x => x.PeriodTo == null ? DateTime.MaxValue : x.PeriodTo).ToList();
                    }
                    if (validAccountCreditPlusDTOs != null && validAccountCreditPlusDTOs.Count > 0)
                    {
                        List<AccountCreditPlusDTO> negativeAccountCreditPlusDTOList = new List<AccountCreditPlusDTO>();
                        decimal bonusToReduce = (decimal)(-1 * bonusValue);
                        foreach (AccountCreditPlusDTO accountCreditPlusDTO in validAccountCreditPlusDTOs)
                        {
                            if (bonusToReduce > 0 && accountCreditPlusDTO.CreditPlusBalance > 0)
                            {
                                if (bonusToReduce <= accountCreditPlusDTO.CreditPlusBalance)
                                {
                                    accountCreditPlusDTO.CreditPlusBalance = accountCreditPlusDTO.CreditPlusBalance - bonusToReduce;
                                    negativeAccountCreditPlusDTOList.Add(new AccountCreditPlusDTO(-1, -1 * bonusToReduce, accountCreditPlusDTO.CreditPlusType, accountCreditPlusDTO.Refundable, (string.IsNullOrEmpty(remarks.Trim()) ? "Load Bonus" : remarks), accountCreditPlusDTO.AccountId, trxId, 1, 0, 
                                        accountCreditPlusDTO.PeriodFrom, currentTime, accountCreditPlusDTO.TimeFrom, accountCreditPlusDTO.TimeTo, accountCreditPlusDTO.NumberOfDays, accountCreditPlusDTO.Monday, accountCreditPlusDTO.Tuesday, accountCreditPlusDTO.Wednesday, accountCreditPlusDTO.Thursday, accountCreditPlusDTO.Friday, accountCreditPlusDTO.Saturday, accountCreditPlusDTO.Sunday, accountCreditPlusDTO.MinimumSaleAmount, 
                                        accountCreditPlusDTO.LoyaltyRuleId, accountCreditPlusDTO.ExtendOnReload, accountCreditPlusDTO.PlayStartTime, accountCreditPlusDTO.TicketAllowed, accountCreditPlusDTO.ForMembershipOnly,
                                        accountCreditPlusDTO.ExpireWithMembership, accountCreditPlusDTO.MembershipId, accountCreditPlusDTO.MembershipRewardsId, accountCreditPlusDTO.PauseAllowed,
                                        accountCreditPlusDTO.IsActive, accountCreditPlusDTO.AccountCreditPlusId, accountCreditPlusDTO.ValidityStatus, accountCreditPlusDTO.SubscriptionBillingScheduleId));
                                    bonusToReduce = 0;
                                }
                                else
                                {
                                    bonusToReduce = (decimal)(bonusToReduce - accountCreditPlusDTO.CreditPlusBalance);
                                    negativeAccountCreditPlusDTOList.Add(new AccountCreditPlusDTO(-1, -1 * accountCreditPlusDTO.CreditPlusBalance, accountCreditPlusDTO.CreditPlusType, accountCreditPlusDTO.Refundable, (string.IsNullOrEmpty(remarks.Trim()) ? "Load Bonus" : remarks), accountCreditPlusDTO.AccountId, trxId, 1, 0,
                                        accountCreditPlusDTO.PeriodFrom, currentTime, accountCreditPlusDTO.TimeFrom, accountCreditPlusDTO.TimeTo, accountCreditPlusDTO.NumberOfDays, accountCreditPlusDTO.Monday, accountCreditPlusDTO.Tuesday, accountCreditPlusDTO.Wednesday, accountCreditPlusDTO.Thursday, accountCreditPlusDTO.Friday, accountCreditPlusDTO.Saturday, accountCreditPlusDTO.Sunday, accountCreditPlusDTO.MinimumSaleAmount,
                                        accountCreditPlusDTO.LoyaltyRuleId, accountCreditPlusDTO.ExtendOnReload, accountCreditPlusDTO.PlayStartTime, accountCreditPlusDTO.TicketAllowed, accountCreditPlusDTO.ForMembershipOnly,
                                        accountCreditPlusDTO.ExpireWithMembership, accountCreditPlusDTO.MembershipId, accountCreditPlusDTO.MembershipRewardsId, accountCreditPlusDTO.PauseAllowed,
                                        accountCreditPlusDTO.IsActive, accountCreditPlusDTO.AccountCreditPlusId, accountCreditPlusDTO.ValidityStatus, accountCreditPlusDTO.SubscriptionBillingScheduleId));
                                    accountCreditPlusDTO.CreditPlusBalance = 0;
                                }
                            }
                            else
                            {
                                if (bonusToReduce == 0)
                                {
                                    break;
                                }
                            }
                        }
                        accountDTO.AccountCreditPlusDTOList.AddRange(negativeAccountCreditPlusDTOList);
                    }
                }
            }
            log.LogMethodExit();
        }
        public void ExtendOnReload()
        {
            log.LogMethodEntry();
            DateTime? maxPeriodToDateTime = accountDTO.AccountCreditPlusDTOList.Where(x => x.ExtendOnReload == true && (x.CreditPlusType == CreditPlusType.GAME_PLAY_CREDIT || x.CreditPlusType == CreditPlusType.GAME_PLAY_BONUS)).Max(y => y.PeriodTo);
            List<AccountCreditPlusDTO> accountCreditPlusDTOList = accountDTO.AccountCreditPlusDTOList.Where(x => x.ExtendOnReload == true && x.CreditPlusBalance != 0 && x.PeriodTo >= DateTime.Now && (x.CreditPlusType == CreditPlusType.GAME_PLAY_CREDIT || x.CreditPlusType == CreditPlusType.GAME_PLAY_BONUS)).ToList();
            foreach (AccountCreditPlusDTO accountCreditPlusDTO in accountCreditPlusDTOList)
            {
                accountCreditPlusDTO.PeriodTo = maxPeriodToDateTime;
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Checks whether Card Has Credit Plus
        /// </summary>
        /// <param name="creditPlusType">creditPlusType</param>
        /// <returns> True or false </returns>
        public bool IsCreditPlusTimeRunning()
        {
            log.LogMethodEntry();
            if (accountDTO.AccountCreditPlusDTOList == null || (accountDTO.AccountCreditPlusDTOList != null && accountDTO.AccountCreditPlusDTOList.Count == 0))
            {
                log.LogMethodExit(false);
                return false;
            }
            else
            {
                AccountCreditPlusDTO accountCreditPlusDTO = accountDTO.AccountCreditPlusDTOList.Where((x) => x.IsActive
                && x.CreditPlusType.Equals(CreditPlusType.TIME)
                && (x.PlayStartTime != null && x.PlayStartTime != DateTime.MinValue && (x.PlayStartTime.Value.AddMinutes((double)x.CreditPlusBalance.Value)) >= DateTime.Now)
                && (x.PeriodFrom == null || (x.PeriodFrom != null && x.PeriodFrom <= DateTime.Now))
                && (x.PeriodTo == null || (x.PeriodTo != null && x.PeriodTo > DateTime.Now))).OrderByDescending((x) => x.PlayStartTime).FirstOrDefault();

                if (accountCreditPlusDTO != null)
                {
                    log.LogMethodExit(true);
                    return true;
                }
            }
            log.LogMethodExit(false);
            return false;
        }
        /// <summary>
        /// This method should be used for Updating account time status.
        /// </summary>
        /// <param name="sqlTransaction"></param>
        /// <param name="ticketmode"></param>/// 
        /// <summary>
        /// This method should be used for Updating account time status.
        /// </summary>
        /// <param name="sqlTransaction"></param>
        /// <param name="ticketmode"></param>/// 

        public void UpdateTimeStatus(AccountDTO.AccountTimeStatusEnum timeStatus)
        {
            log.LogMethodEntry(timeStatus);
            try
            {
                if (!CardHasCreditPlus(CreditPlusType.TIME))
                {
                    string message = MessageContainerList.GetMessage(executionContext, 1841);
                    log.LogMethodExit(message);
                    log.LogMethodExit("Fresh card is not loaded with credit plus so card is not active.");
                    throw new ValidationException(message);
                }
                if ((accountDTO.Time + accountDTO.AccountSummaryDTO.CreditPlusTime) > 0)
                {
                    if (accountDTO.AccountSummaryDTO.CreditPlusTime > 0)
                    {
                        string message = string.Empty;
                        bool isTimeRunning = IsCreditPlusTimeRunning();
                        if (isTimeRunning)
                        {
                            if (CheckTimePauseLimit())
                            {
                                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                                {
                                    bool canPauseRunningTime = CanPauseRunningCreditPlusTime(parafaitDBTrx.SQLTrx);
                                    if (canPauseRunningTime)
                                    {
                                        List<AccountCreditPlusDTO> creditPlusTypeTimeDTOList = new List<AccountCreditPlusDTO>();
                                        if (accountDTO.AccountCreditPlusDTOList != null && accountDTO.AccountCreditPlusDTOList.Count > 0)
                                        {
                                            creditPlusTypeTimeDTOList = accountDTO.AccountCreditPlusDTOList.Where((x) => x.IsActive
                                            && x.CreditPlusType.Equals(CreditPlusType.TIME)
                                            && (x.PlayStartTime != null
                                            && x.PlayStartTime != DateTime.MinValue
                                            && (x.PlayStartTime.Value.AddMinutes((double)x.CreditPlusBalance.Value)) >= DateTime.Now)
                                            && (x.PeriodFrom == null || (x.PeriodFrom != null && x.PeriodFrom <= DateTime.Now))
                                            && (x.PeriodTo == null || (x.PeriodTo != null && x.PeriodTo > DateTime.Now))
                                            && x.ValidityStatus == AccountDTO.AccountValidityStatus.Valid
                                            && x.CreditPlusBalance > 0).OrderBy((x) => x.PeriodTo).ToList();
                                        }
                                        if (creditPlusTypeTimeDTOList == null || creditPlusTypeTimeDTOList.Count == 0)
                                        {
                                            message = MessageContainerList.GetMessage(executionContext, 1386);
                                            throw new ValidationException(message);
                                        }
                                        else
                                        {
                                            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                                            int count = creditPlusTypeTimeDTOList.Count; // used to keep track of multiple creditplus type time
                                            foreach (AccountCreditPlusDTO accountCreditPlusDTO in creditPlusTypeTimeDTOList)
                                            {
                                                if (accountCreditPlusDTO.PlayStartTime != null && Convert.ToDateTime(accountCreditPlusDTO.PlayStartTime) != DateTime.MinValue && Convert.ToInt32(accountCreditPlusDTO.CreditPlusBalance) > 0)
                                                {
                                                    int timeBalance = int.MinValue;
                                                    count = count - 1;

                                                    AccountCreditPlusBL accountCreditPlusBL = new AccountCreditPlusBL(executionContext, accountCreditPlusDTO);
                                                    timeBalance = Convert.ToInt32(accountCreditPlusBL.GetCreditPlusTime(DateTime.Now));
                                                    if (timeBalance > 0)
                                                    {
                                                        CardCreditPlusPauseTimeLogDTO cardCreditPlusPauseTimeLogDTO = new CardCreditPlusPauseTimeLogDTO(-1,
                                                             Convert.ToInt32(accountCreditPlusDTO.AccountCreditPlusId), Convert.ToDateTime(accountCreditPlusDTO.PlayStartTime), lookupValuesList.GetServerDateTime(),
                                                             Convert.ToDouble(timeBalance), "Pause Time", executionContext.POSMachineName);
                                                        CardCreditPlusPauseTimeLogBL cardCreditPlusPauseTimeLogBL = new CardCreditPlusPauseTimeLogBL(executionContext, cardCreditPlusPauseTimeLogDTO);
                                                        cardCreditPlusPauseTimeLogBL.Save(parafaitDBTrx.SQLTrx);
                                                        accountCreditPlusDTO.PlayStartTime = null;
                                                        accountCreditPlusDTO.CreditPlusBalance = (decimal?)timeBalance;
                                                        accountCreditPlusDTO.LastUpdateDate = lookupValuesList.GetServerDateTime();
                                                        accountCreditPlusDTO.LastUpdatedBy = executionContext.UserId;
                                                    }
                                                    else
                                                    {
                                                        if (count == 0) // if count is not zero then we can check another record and check if it has timebalance > 0 and can be pause
                                                        {
                                                            message = MessageContainerList.GetMessage(executionContext, 1386);
                                                            throw new ValidationException(message);

                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        message = MessageContainerList.GetMessage(executionContext, 1706);
                                        throw new ValidationException(message);
                                    }
                                }
                            }
                        }
                        else
                        {
                            message = MessageContainerList.GetMessage(executionContext, 1386);
                            throw new ValidationException(message);

                        }
                    }
                    else
                    {
                        string message = string.Empty;
                        message = MessageContainerList.GetMessage(executionContext, 1386);
                        throw new ValidationException(message);
                    }
                }
                else
                {
                    string message = string.Empty;
                    message = MessageContainerList.GetMessage(executionContext, 1838);
                    throw new ValidationException(message);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public bool CheckTimePauseLimit()
        {
            int maxPauseTimeAllowed;
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            maxPauseTimeAllowed = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "MAX_ALLOWED_TIME_PAUSE");
            if (maxPauseTimeAllowed > 0)
            {
                CardCPPauseTimeLogList pauseTimeLogList = new CardCPPauseTimeLogList();
                List<CardCreditPlusPauseTimeLogDTO> pauseTimeLogDTOList = new List<CardCreditPlusPauseTimeLogDTO>();
                pauseTimeLogDTOList = pauseTimeLogList.GetAllCardCPPauseTimeLogByCardId(accountDTO.AccountId);
                if (pauseTimeLogDTOList != null)
                {
                    List<CardCreditPlusPauseTimeLogDTO> pauseTimeLogDTOFilteredList = new List<CardCreditPlusPauseTimeLogDTO>();
                    //Get list of DTO for number of times Pause was performed in a day for the same card
                    pauseTimeLogDTOFilteredList = pauseTimeLogDTOList.FindAll(x => x.PauseStartTime != DateTime.MinValue
                                                                                  && x.PauseStartTime >= lookupValuesList.GetServerDateTime().Date.AddHours(ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "BUSINESS_DAY_START_TIME"))
                                                                                  && x.PauseStartTime < lookupValuesList.GetServerDateTime().Date.AddDays(1).AddHours(ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "BUSINESS_DAY_START_TIME")));
                    if (pauseTimeLogDTOFilteredList.Count >= maxPauseTimeAllowed)
                    {

                        string message = MessageContainerList.GetMessage(executionContext, 1387);
                        throw new ValidationException(message);

                    }
                    else
                        return true;
                }
                else
                    return true;
            }
            else
                return true;
        }
        /// <summary>
        /// Pause Subscription Billing Cycle Entitlements
        /// </summary>
        /// <param name="subscriptionBillingCycleIdList"></param>
        /// <param name="sqlTrx"></param>
        public void PauseSubscriptionBillingCycleEntitlements(List<int> subscriptionBillingCycleIdList, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(subscriptionBillingCycleIdList, sqlTrx);
            if (subscriptionBillingCycleIdList != null && subscriptionBillingCycleIdList.Any())
            {
                if (accountDTO != null)
                {
                    LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                    DateTime currentServerTime = lookupValuesList.GetServerDateTime();
                    PauseSubscriptionBillCycleCreditPlusRecords(subscriptionBillingCycleIdList, currentServerTime);
                    PauseSubscriptionBillCycleCardGamesRecords(subscriptionBillingCycleIdList, currentServerTime);
                    PauseSubscriptionBillCycleCardDiscountRecords(subscriptionBillingCycleIdList, currentServerTime);
                }
            }
            Save(sqlTrx);
            log.LogMethodExit();
        }

        private void PauseSubscriptionBillCycleCreditPlusRecords(List<int> subscriptionBillingCycleIdList, DateTime currentServerTime)
        {
            log.LogMethodEntry(subscriptionBillingCycleIdList, currentServerTime);
            if (accountDTO.AccountCreditPlusDTOList != null && accountDTO.AccountCreditPlusDTOList.Any())
            {
                for (int i = 0; i < subscriptionBillingCycleIdList.Count; i++)
                {
                    List<AccountCreditPlusDTO> subscriptionRelatedDTOS = accountDTO.AccountCreditPlusDTOList.Where(cp => cp.SubscriptionBillingScheduleId == subscriptionBillingCycleIdList[i]
                                                                                                                       && cp.IsActive
                                                                                                                       && (cp.PeriodTo == null || cp.PeriodTo >= currentServerTime)).ToList();
                    if (subscriptionRelatedDTOS != null && subscriptionRelatedDTOS.Any())
                    {
                        for (int j = 0; j < subscriptionRelatedDTOS.Count; j++)
                        {
                            AccountCreditPlusBL accountCreditPlusBL = new AccountCreditPlusBL(executionContext, subscriptionRelatedDTOS[j]);
                            accountCreditPlusBL.PauseSubscriptionBillingCycleEntitlements();
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        private void PauseSubscriptionBillCycleCardGamesRecords(List<int> subscriptionBillingCycleIdList, DateTime currentServerTime)
        {
            log.LogMethodEntry(subscriptionBillingCycleIdList, currentServerTime);
            if (accountDTO.AccountGameDTOList != null && accountDTO.AccountGameDTOList.Any())
            {
                for (int i = 0; i < subscriptionBillingCycleIdList.Count; i++)
                {
                    List<AccountGameDTO> subscriptionRelatedDTOS = accountDTO.AccountGameDTOList.Where(cp => cp.SubscriptionBillingScheduleId == subscriptionBillingCycleIdList[i]
                                                                                                                       && cp.IsActive
                                                                                                                       && (cp.ExpiryDate == null || cp.ExpiryDate >= currentServerTime)).ToList();
                    if (subscriptionRelatedDTOS != null && subscriptionRelatedDTOS.Any())
                    {
                        for (int j = 0; j < subscriptionRelatedDTOS.Count; j++)
                        {
                            AccountGameBL accountGameBL = new AccountGameBL(executionContext, subscriptionRelatedDTOS[j]);
                            accountGameBL.PauseSubscriptionBillingCycleEntitlements();
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
        private void PauseSubscriptionBillCycleCardDiscountRecords(List<int> subscriptionBillingCycleIdList, DateTime currentServerTime)
        {
            log.LogMethodEntry(subscriptionBillingCycleIdList, currentServerTime);
            if (accountDTO.AccountDiscountDTOList != null && accountDTO.AccountDiscountDTOList.Any())
            {
                for (int i = 0; i < subscriptionBillingCycleIdList.Count; i++)
                {
                    List<AccountDiscountDTO> subscriptionRelatedDTOS = accountDTO.AccountDiscountDTOList.Where(cp => cp.SubscriptionBillingScheduleId == subscriptionBillingCycleIdList[i]
                                                                                                                       && cp.IsActive
                                                                                                                       && (cp.ExpiryDate == null || cp.ExpiryDate >= currentServerTime)).ToList();
                    if (subscriptionRelatedDTOS != null && subscriptionRelatedDTOS.Any())
                    {
                        for (int j = 0; j < subscriptionRelatedDTOS.Count; j++)
                        {
                            AccountDiscountBL accountDiscountBL = new AccountDiscountBL(executionContext, subscriptionRelatedDTOS[j]);
                            accountDiscountBL.PauseSubscriptionBillingCycleEntitlements();
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Pause Subscription Billing Cycle Entitlements
        /// </summary>
        /// <param name="subscriptionBillingCycleIdList"></param>
        /// <param name="sqlTrx"></param>
        public void PostponeSubscriptionBillingCycleEntitlements(List<SubscriptionUnPauseDetailsDTO> subscriptionUnPauseDetailsDTOList, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(subscriptionUnPauseDetailsDTOList, sqlTrx);
            if (subscriptionUnPauseDetailsDTOList != null && subscriptionUnPauseDetailsDTOList.Any())
            {
                if (accountDTO != null)
                {
                    //LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                    //DateTime currentServerTime = lookupValuesList.GetServerDateTime();
                    PostponeSubscriptionBillCycleCreditPlusRecords(subscriptionUnPauseDetailsDTOList);
                    PostponeSubscriptionBillCycleCardGamesRecords(subscriptionUnPauseDetailsDTOList);
                    PostponeSubscriptionBillCycleCardDiscountRecords(subscriptionUnPauseDetailsDTOList);
                }
            }
            Save(sqlTrx);
            log.LogMethodExit();
        }
        private void PostponeSubscriptionBillCycleCreditPlusRecords(List<SubscriptionUnPauseDetailsDTO> subscriptionUnPauseDetailsDTOList)
        {
            log.LogMethodEntry(subscriptionUnPauseDetailsDTOList);
            if (accountDTO.AccountCreditPlusDTOList != null && accountDTO.AccountCreditPlusDTOList.Any())
            {
                for (int i = 0; i < subscriptionUnPauseDetailsDTOList.Count; i++)
                {
                    List<AccountCreditPlusDTO> subscriptionRelatedDTOS = accountDTO.AccountCreditPlusDTOList.Where(cp => cp.SubscriptionBillingScheduleId == subscriptionUnPauseDetailsDTOList[i].SubscriptionBillingScheduleId
                                                                                                                       && cp.IsActive).ToList();
                    if (subscriptionRelatedDTOS != null && subscriptionRelatedDTOS.Any())
                    {
                        for (int j = 0; j < subscriptionRelatedDTOS.Count; j++)
                        {
                            AccountCreditPlusBL accountCreditPlusBL = new AccountCreditPlusBL(executionContext, subscriptionRelatedDTOS[j]);
                            accountCreditPlusBL.PostponeSubscriptionBillingCycleEntitlements(subscriptionUnPauseDetailsDTOList[i]);
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
        private void PostponeSubscriptionBillCycleCardGamesRecords(List<SubscriptionUnPauseDetailsDTO> subscriptionUnPauseDetailsDTOList)
        {
            log.LogMethodEntry(subscriptionUnPauseDetailsDTOList);
            if (accountDTO.AccountGameDTOList != null && accountDTO.AccountGameDTOList.Any())
            {
                for (int i = 0; i < subscriptionUnPauseDetailsDTOList.Count; i++)
                {
                    List<AccountGameDTO> subscriptionRelatedDTOS = accountDTO.AccountGameDTOList.Where(cp => cp.SubscriptionBillingScheduleId == subscriptionUnPauseDetailsDTOList[i].SubscriptionBillingScheduleId
                                                                                                                       && cp.IsActive).ToList();
                    if (subscriptionRelatedDTOS != null && subscriptionRelatedDTOS.Any())
                    {
                        for (int j = 0; j < subscriptionRelatedDTOS.Count; j++)
                        {
                            AccountGameBL accountGameBL = new AccountGameBL(executionContext, subscriptionRelatedDTOS[j]);
                            accountGameBL.PostponeSubscriptionBillingCycleEntitlements(subscriptionUnPauseDetailsDTOList[i]);
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
        private void PostponeSubscriptionBillCycleCardDiscountRecords(List<SubscriptionUnPauseDetailsDTO> subscriptionUnPauseDetailsDTOList)
        {
            log.LogMethodEntry(subscriptionUnPauseDetailsDTOList);
            if (accountDTO.AccountDiscountDTOList != null && accountDTO.AccountDiscountDTOList.Any())
            {
                for (int i = 0; i < subscriptionUnPauseDetailsDTOList.Count; i++)
                {
                    List<AccountDiscountDTO> subscriptionRelatedDTOS = accountDTO.AccountDiscountDTOList.Where(cp => cp.SubscriptionBillingScheduleId == subscriptionUnPauseDetailsDTOList[i].SubscriptionBillingScheduleId
                                                                                                                       && cp.IsActive).ToList();
                    if (subscriptionRelatedDTOS != null && subscriptionRelatedDTOS.Any())
                    {
                        for (int j = 0; j < subscriptionRelatedDTOS.Count; j++)
                        {
                            AccountDiscountBL accountDiscountBL = new AccountDiscountBL(executionContext, subscriptionRelatedDTOS[j]);
                            accountDiscountBL.PostponeSubscriptionBillingCycleEntitlements(subscriptionUnPauseDetailsDTOList[i]);
                        }
                    }
                }
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Resume Subscription Billing Cycle Entitlements
        /// </summary>
        /// <param name="subscriptionBillingCycleIdList"></param>
        /// <param name="sqlTrx"></param>
        public void ResumeSubscriptionBillingCycleEntitlements(List<int> subscriptionBillingCycleIdList, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(subscriptionBillingCycleIdList, sqlTrx);
            if (subscriptionBillingCycleIdList != null && subscriptionBillingCycleIdList.Any())
            {
                if (accountDTO != null)
                {
                    LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                    DateTime currentServerTime = lookupValuesList.GetServerDateTime();
                    ResumeSubscriptionBillCycleCreditPlusRecords(subscriptionBillingCycleIdList, currentServerTime, sqlTrx);
                    ResumeSubscriptionBillCycleCardGamesRecords(subscriptionBillingCycleIdList, currentServerTime, sqlTrx);
                    ResumeSubscriptionBillCycleCardDiscountRecords(subscriptionBillingCycleIdList, currentServerTime, sqlTrx);
                }
            }
            Save(sqlTrx);
            log.LogMethodExit();
        }
        private void ResumeSubscriptionBillCycleCreditPlusRecords(List<int> subscriptionBillingCycleIdList, DateTime currentServerTime, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(subscriptionBillingCycleIdList, currentServerTime, sqlTrx);
            if (accountDTO.AccountCreditPlusDTOList != null && accountDTO.AccountCreditPlusDTOList.Any())
            {
                for (int i = 0; i < subscriptionBillingCycleIdList.Count; i++)
                {
                    List<AccountCreditPlusDTO> subscriptionRelatedDTOS = accountDTO.AccountCreditPlusDTOList.Where(cp => cp.SubscriptionBillingScheduleId == subscriptionBillingCycleIdList[i]
                                                                                                                       && cp.IsActive
                                                                                                                       && (cp.PeriodTo == null || cp.PeriodTo >= currentServerTime)).ToList();
                    if (subscriptionRelatedDTOS != null && subscriptionRelatedDTOS.Any())
                    {
                        for (int j = 0; j < subscriptionRelatedDTOS.Count; j++)
                        {
                            AccountCreditPlusBL accountCreditPlusBL = new AccountCreditPlusBL(executionContext, subscriptionRelatedDTOS[j]);
                            accountCreditPlusBL.ResumeSubscriptionBillingCycleEntitlements(sqlTrx);
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
        private void ResumeSubscriptionBillCycleCardGamesRecords(List<int> subscriptionBillingCycleIdList, DateTime currentServerTime, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(subscriptionBillingCycleIdList, currentServerTime, sqlTrx);
            if (accountDTO.AccountGameDTOList != null && accountDTO.AccountGameDTOList.Any())
            {
                for (int i = 0; i < subscriptionBillingCycleIdList.Count; i++)
                {
                    List<AccountGameDTO> subscriptionRelatedDTOS = accountDTO.AccountGameDTOList.Where(cp => cp.SubscriptionBillingScheduleId == subscriptionBillingCycleIdList[i]
                                                                                                                       && cp.IsActive
                                                                                                                       && (cp.ExpiryDate == null || cp.ExpiryDate >= currentServerTime)).ToList();
                    if (subscriptionRelatedDTOS != null && subscriptionRelatedDTOS.Any())
                    {
                        for (int j = 0; j < subscriptionRelatedDTOS.Count; j++)
                        {
                            AccountGameBL accountGameBL = new AccountGameBL(executionContext, subscriptionRelatedDTOS[j]);
                            accountGameBL.ResumeSubscriptionBillingCycleEntitlements(sqlTrx);
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
        private void ResumeSubscriptionBillCycleCardDiscountRecords(List<int> subscriptionBillingCycleIdList, DateTime currentServerTime, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(subscriptionBillingCycleIdList, currentServerTime, sqlTrx);
            if (accountDTO.AccountDiscountDTOList != null && accountDTO.AccountDiscountDTOList.Any())
            {
                for (int i = 0; i < subscriptionBillingCycleIdList.Count; i++)
                {
                    List<AccountDiscountDTO> subscriptionRelatedDTOS = accountDTO.AccountDiscountDTOList.Where(cp => cp.SubscriptionBillingScheduleId == subscriptionBillingCycleIdList[i]
                                                                                                                       && cp.IsActive
                                                                                                                       && (cp.ExpiryDate == null || cp.ExpiryDate >= currentServerTime)).ToList();
                    if (subscriptionRelatedDTOS != null && subscriptionRelatedDTOS.Any())
                    {
                        for (int j = 0; j < subscriptionRelatedDTOS.Count; j++)
                        {
                            AccountDiscountBL accountDiscountBL = new AccountDiscountBL(executionContext, subscriptionRelatedDTOS[j]);
                            accountDiscountBL.ResumeSubscriptionBillingCycleEntitlements(sqlTrx);
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        public List<Tuple<int, double, double>> RedeemLoyalty(double loyaltyPoints, double redeemValue, string attribute, int trxId)
        {
            List<Tuple<int, double, double>> cpList = new List<Tuple<int, double, double>>();
            double totalRedeemValue = 0;
            int count = 0;
            double balanceLoyaltyPoints = loyaltyPoints;
            double usedCreditPlusBalance = 0;
            double cpRedeemValue = 0;
            double multiplicationFactor = redeemValue / loyaltyPoints;
            List<AccountCreditPlusDTO> loyaltyCreditPlusDTOList = null;
            if (accountDTO.AccountCreditPlusDTOList != null)
            {
                loyaltyCreditPlusDTOList = accountDTO.AccountCreditPlusDTOList.Where(x => x.CreditPlusType == CreditPlusType.LOYALTY_POINT).ToList();
            }
            if (loyaltyCreditPlusDTOList != null && loyaltyCreditPlusDTOList.Any())
            {
                foreach (AccountCreditPlusDTO accountCreditPlusDTO in loyaltyCreditPlusDTOList)
                {
                    usedCreditPlusBalance = Convert.ToDouble(accountCreditPlusDTO.CreditPlusBalance);
                    double deductableValue = 0;
                    int cpId = accountCreditPlusDTO.AccountCreditPlusId;
                    while (usedCreditPlusBalance > 0 && balanceLoyaltyPoints > 0)
                    {
                        if (balanceLoyaltyPoints < usedCreditPlusBalance)
                        {
                            deductableValue = balanceLoyaltyPoints;
                            balanceLoyaltyPoints = 0;
                            usedCreditPlusBalance = usedCreditPlusBalance - deductableValue;
                            cpRedeemValue = Math.Round(deductableValue * multiplicationFactor, 2);
                        }
                        else if (balanceLoyaltyPoints >= usedCreditPlusBalance)
                        {
                            deductableValue = usedCreditPlusBalance;
                            balanceLoyaltyPoints = balanceLoyaltyPoints - deductableValue;
                            usedCreditPlusBalance = 0;
                            cpRedeemValue = Math.Round(deductableValue * multiplicationFactor, 2);
                        }
                        cpList.Add(new Tuple<int, double, double>(cpId, deductableValue, cpRedeemValue));
                        totalRedeemValue = totalRedeemValue + cpRedeemValue;
                        count++;
                    }
                }
            }
            if (balanceLoyaltyPoints > 0)
            {
                if (accountDTO.LoyaltyPoints >= Convert.ToInt32(balanceLoyaltyPoints))
                {
                    cpRedeemValue = Math.Round(balanceLoyaltyPoints * multiplicationFactor, 2);
                    cpList.Add(new Tuple<int, double, double>(-1, balanceLoyaltyPoints, cpRedeemValue));
                    totalRedeemValue = totalRedeemValue + cpRedeemValue;
                    count++;
                }
                else
                {
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 2493));
                }
            }
            if (totalRedeemValue != redeemValue)
            {
                double balanceRedeemValue = redeemValue - totalRedeemValue;
                double newCpRedeemValue = Math.Round(cpList[count - 1].Item3 + balanceRedeemValue, 2);
                cpList[count - 1] = Tuple.Create(cpList[count - 1].Item1, cpList[count - 1].Item2, newCpRedeemValue);
            }


            for (int i = 0; i < count; i++)
            {
                if (cpList[i].Item1 != -1)
                {
                    if (!attribute.Equals("Cash", StringComparison.CurrentCultureIgnoreCase))
                    {
                        CreditPlusType creditPlusType;
                        switch (attribute.ToLower())
                        {

                            case "game play bonus":
                            case "game play credits":
                                {
                                    creditPlusType = CreditPlusType.GAME_PLAY_BONUS;
                                    break;
                                }
                            case "time":
                                {
                                    creditPlusType = CreditPlusType.TIME;
                                    break;
                                }
                            case "tickets":
                                {
                                    creditPlusType = CreditPlusType.TICKET;
                                    break;
                                }
                            default:
                                {
                                    creditPlusType = CreditPlusType.CARD_BALANCE;
                                    break;
                                }
                        }
                        LookupValuesList lookupValuesList = new LookupValuesList();
                        DateTime serverDateTime = lookupValuesList.GetServerDateTime();
                        DateTime periodFrom = serverDateTime;
                        DateTime playStartTime = DateTime.MinValue;
                        if (((DateTime)periodFrom).Hour < ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "BUSINESS_DAY_START_TIME"))
                        {
                            periodFrom = ((DateTime)periodFrom).AddDays(-1);
                        }
                        AccountDTO.AccountValidityStatus validityStatus = AccountDTO.AccountValidityStatus.Valid;

                        bool ticketAllowed = IsTicketAllowedForRedeemLoyalty();
                        AccountCreditPlusDTO genericCreditPlusDTO = new AccountCreditPlusDTO(-1, Convert.ToDecimal(cpList[i].Item3), creditPlusType, false,
                    "Redeem Loyalty Credit Plus", accountDTO.AccountId, trxId, (i + 1), Convert.ToDecimal(cpList[i].Item3), periodFrom, null, null, null, null, true, true, true, true, true, true, true, null, -1, false, playStartTime, ticketAllowed, false, false, -1, -1, ServerDateTime.Now, "",
                    ServerDateTime.Now, accountDTO.SiteId, -1, false, "", false, true, "", cpList[i].Item1, validityStatus, -1);
                        genericCreditPlusDTO.IsChanged = true;
                        if (accountDTO.AccountCreditPlusDTOList == null)
                        {
                            accountDTO.AccountCreditPlusDTOList = new List<AccountCreditPlusDTO>();
                        }
                        accountDTO.AccountCreditPlusDTOList.Add(genericCreditPlusDTO);

                    }
                    DeductFromCreditPlusRecord(cpList[i].Item1, cpList[i].Item2);
                }
                else
                {
                    if (accountDTO.LoyaltyPoints >= Convert.ToInt32(balanceLoyaltyPoints))
                    {
                        if (!attribute.Equals("Cash", StringComparison.CurrentCultureIgnoreCase))
                        {
                            CreditPlusType creditPlusType;
                            switch (attribute.ToLower())
                            {

                                case "game play bonus":
                                case "game play credits":
                                    {
                                        creditPlusType = CreditPlusType.GAME_PLAY_BONUS;
                                        break;
                                    }
                                case "time":
                                    {
                                        creditPlusType = CreditPlusType.TIME;
                                        break;
                                    }
                                case "tickets":
                                    {
                                        creditPlusType = CreditPlusType.TICKET;
                                        break;
                                    }
                                default:
                                    {
                                        creditPlusType = CreditPlusType.CARD_BALANCE;
                                        break;
                                    }
                            }
                            LookupValuesList lookupValuesList = new LookupValuesList();
                            DateTime serverDateTime = lookupValuesList.GetServerDateTime();
                            DateTime periodFrom = serverDateTime;
                            DateTime playStartTime = DateTime.MinValue;
                            if (((DateTime)periodFrom).Hour < ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "BUSINESS_DAY_START_TIME"))
                            {
                                periodFrom = ((DateTime)periodFrom).AddDays(-1);
                            }
                            AccountDTO.AccountValidityStatus validityStatus = AccountDTO.AccountValidityStatus.Valid;

                            bool ticketAllowed = IsTicketAllowedForRedeemLoyalty();
                            AccountCreditPlusDTO genericCreditPlusDTO = new AccountCreditPlusDTO(-1, Convert.ToDecimal(cpList[i].Item3), creditPlusType, false,
                            "Redeem Loyalty Credit Plus", accountDTO.AccountId, trxId, (i + 1), Convert.ToDecimal(cpList[i].Item3), periodFrom, null, null, null, null, true, true, true, true, true, true, true, null, -1, false, playStartTime, ticketAllowed, false, false, -1, -1, ServerDateTime.Now, "",
                            ServerDateTime.Now, accountDTO.SiteId, -1, false, "", false, true, "", cpList[i].Item1, validityStatus, -1);
                            genericCreditPlusDTO.IsChanged = true;
                            if (accountDTO.AccountCreditPlusDTOList == null)
                            {
                                accountDTO.AccountCreditPlusDTOList = new List<AccountCreditPlusDTO>();
                            }
                            accountDTO.AccountCreditPlusDTOList.Add(genericCreditPlusDTO);
                        }
                        accountDTO.LoyaltyPoints = accountDTO.LoyaltyPoints - Convert.ToInt32(balanceLoyaltyPoints);
                    }
                    else
                    {
                        throw new Exception(MessageContainerList.GetMessage(executionContext, 2493));
                    }
                }
            }
            return cpList;
        }
        private bool IsTicketAllowedForRedeemLoyalty()
        {
            log.LogMethodEntry();
            bool ticketAllowed = false;
            ProductsContainerDTO productsContainerDTO = ProductsContainerList.GetSystemProductContainerDTO(executionContext.SiteId, ProductTypeValues.LOYALTY);
            if (productsContainerDTO != null)
            {
                if (!string.IsNullOrWhiteSpace(productsContainerDTO.TicketAllowed) && productsContainerDTO.TicketAllowed == "Y")
                {
                    ticketAllowed = true;
                }
            }
            log.LogMethodExit(ticketAllowed);
            return ticketAllowed;
        }

        public void DeductFromCreditPlusRecord(int creditPlusId, double deductableValue)
        {
            log.LogMethodEntry(creditPlusId, deductableValue);
            AccountCreditPlusDTO accountCreditPlusDTO = accountDTO.AccountCreditPlusDTOList.FirstOrDefault(x => x.AccountCreditPlusId == creditPlusId && Convert.ToDouble(x.CreditPlusBalance) >= deductableValue);
            if (accountCreditPlusDTO != null)
            {
                accountCreditPlusDTO.CreditPlusBalance = Convert.ToDecimal(Convert.ToDouble(accountCreditPlusDTO.CreditPlusBalance) - deductableValue);
            }
            log.LogMethodExit();
        }

        public int GetCardCPConsumptionDiscPercentage(int creditPlusConsumptionId)
        {
            log.LogMethodEntry(creditPlusConsumptionId);
            int discPercentage = 0;
            try
            {
                if (accountDTO.AccountCreditPlusDTOList != null && accountDTO.AccountCreditPlusDTOList.Count > 0)
                {
                    foreach (AccountCreditPlusDTO accountCreditPlusDTO in accountDTO.AccountCreditPlusDTOList)
                    {
                        if (accountCreditPlusDTO.AccountCreditPlusConsumptionDTOList != null && accountCreditPlusDTO.AccountCreditPlusConsumptionDTOList.Count > 0)
                        {
                            AccountCreditPlusConsumptionDTO accountCreditPlusConsumptionDTO = accountCreditPlusDTO.AccountCreditPlusConsumptionDTOList.Where(cp => cp.AccountCreditPlusConsumptionId == creditPlusConsumptionId).FirstOrDefault();
                            if (accountCreditPlusConsumptionDTO != null)
                            {
                                decimal? percentage = accountCreditPlusConsumptionDTO.DiscountPercentage;
                                if (percentage != null)
                                {
                                    discPercentage = Convert.ToInt32(percentage);
                                    log.LogMethodExit(discPercentage);
                                    return discPercentage;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(discPercentage);
            return discPercentage;
        }

        public int GetApplicableCardCPConsumptionsBalance(List<ProductsContainerDTO> productsContainerDTOs)
        {
            log.LogMethodEntry(productsContainerDTOs);
            int consumptionBalance = 0;

            try
            {
                if (accountDTO.AccountCreditPlusDTOList != null || accountDTO.AccountCreditPlusDTOList.Count > 0)
                {
                    foreach (AccountCreditPlusDTO accountCreditPlusDTO in accountDTO.AccountCreditPlusDTOList)
                    {
                        if (accountCreditPlusDTO.AccountCreditPlusConsumptionDTOList != null && accountCreditPlusDTO.AccountCreditPlusConsumptionDTOList.Count > 0)
                        {
                            foreach (AccountCreditPlusConsumptionDTO accountCreditPlusConsumptionDTO in accountCreditPlusDTO.AccountCreditPlusConsumptionDTOList)
                            {
                                if (accountCreditPlusConsumptionDTO.ConsumptionBalance != null && (Convert.ToInt32(accountCreditPlusConsumptionDTO.ConsumptionBalance) > 0))
                                {
                                    foreach (ProductsContainerDTO productsContainerDTO in productsContainerDTOs)
                                    {
                                        if (accountCreditPlusConsumptionDTO.CategoryId != -1 && productsContainerDTO.CategoryId == accountCreditPlusConsumptionDTO.CategoryId)
                                        {
                                            log.LogVariableState("CategoryId: ", accountCreditPlusConsumptionDTO.CategoryId);
                                            consumptionBalance += Convert.ToInt32(accountCreditPlusConsumptionDTO.ConsumptionBalance);
                                            log.LogVariableState("Consumption balance: ", accountCreditPlusConsumptionDTO.ConsumptionBalance);
                                        }
                                        else if (accountCreditPlusConsumptionDTO.ProductId != -1 && productsContainerDTO.ProductId == accountCreditPlusConsumptionDTO.ProductId)
                                        {
                                            log.LogVariableState("ProductId Id: ", accountCreditPlusConsumptionDTO.ProductId);
                                            consumptionBalance += Convert.ToInt32(accountCreditPlusConsumptionDTO.ConsumptionBalance);
                                            log.LogVariableState("Consumption balance: ", accountCreditPlusConsumptionDTO.ConsumptionBalance);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(consumptionBalance);
            return consumptionBalance;
        }

        public bool IsEligibleToApplyCardCPConsumptionBalance(int categoryId, int productId)
        {
            log.LogMethodEntry(categoryId, productId);
            bool isEligible = false;

            try
            {
                if (accountDTO.AccountCreditPlusDTOList != null && accountDTO.AccountCreditPlusDTOList.Count > 0)
                {
                    foreach (AccountCreditPlusDTO accountCreditPlusDTO in accountDTO.AccountCreditPlusDTOList)
                    {
                        if (accountCreditPlusDTO.AccountCreditPlusConsumptionDTOList != null && accountCreditPlusDTO.AccountCreditPlusConsumptionDTOList.Count > 0)
                        {
                            foreach (AccountCreditPlusConsumptionDTO accountCreditPlusConsumptionDTO in accountCreditPlusDTO.AccountCreditPlusConsumptionDTOList)
                            {
                                if (accountCreditPlusConsumptionDTO.ConsumptionBalance != null && (Convert.ToInt32(accountCreditPlusConsumptionDTO.ConsumptionBalance) > 0))
                                {
                                    if (categoryId != -1 && categoryId == accountCreditPlusConsumptionDTO.CategoryId)
                                        isEligible = true;

                                    else if (productId == accountCreditPlusConsumptionDTO.ProductId)
                                        isEligible = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(isEligible);
            return isEligible;
        }


        /// <summary>
        /// Gets AccountCreditPlusSummary DTO
        /// </summary>
        /// <param name="accountSummaryOptions"></param>
        /// <returns></returns>
        internal List<AccountCreditPlusSummaryBL> GetAccountCreditPlusSummaryBLList(AccountSummaryOptions accountSummaryOptions)
        {
            log.LogMethodEntry(accountSummaryOptions);
            List<AccountCreditPlusSummaryBL> result = new List<AccountCreditPlusSummaryBL>();
            if (accountDTO.AccountCreditPlusDTOList != null && accountDTO.AccountCreditPlusDTOList.Count > 0)
            {
                foreach (AccountCreditPlusDTO accountCreditPlusDTO in accountDTO.AccountCreditPlusDTOList)
                {
                    if (accountSummaryOptions.CreditPlusType.HasValue && accountCreditPlusDTO.CreditPlusType != accountSummaryOptions.CreditPlusType)
                    {
                        continue;
                    }
                    if (accountCreditPlusDTO.PeriodTo.HasValue && accountCreditPlusDTO.PeriodTo.Value < ServerDateTime.Now)
                    {
                        continue;
                    }

                    if (accountCreditPlusDTO.ValidityStatus == AccountDTO.AccountValidityStatus.Hold)
                    {
                        continue;
                    }

                    if (accountSummaryOptions.ShowExpiryEntitlements == true)
                    {
                        if (accountSummaryOptions.ToDate.HasValue == false)
                        {
                            string customException = MessageContainerList.GetMessage(executionContext, 597);
                            log.Error(customException);
                            throw new ValidationException(customException);
                        }

                        DateTime fromDate;

                        if (accountSummaryOptions.FromDate.HasValue == false)
                        {
                            fromDate = new BusinessDate(executionContext.GetSiteId(), accountSummaryOptions.ToDate.Value).Start;
                        }
                        else
                        {
                            fromDate = new BusinessDate(executionContext.GetSiteId(), accountSummaryOptions.FromDate.Value).Start;
                        }
                        DateTime toDate = new BusinessDate(executionContext.GetSiteId(), accountSummaryOptions.ToDate.Value).End;
                        if (accountCreditPlusDTO.PeriodTo.HasValue == false || accountCreditPlusDTO.PeriodTo.Value < fromDate || accountCreditPlusDTO.PeriodTo.Value > toDate)
                        {
                            continue;
                        }
                    }

                    if (accountSummaryOptions.FromDate.HasValue)
                    {
                        DateTime fromDate = new BusinessDate(executionContext.GetSiteId(), accountSummaryOptions.FromDate.Value).Start;
                        if (accountCreditPlusDTO.PeriodTo.HasValue && accountCreditPlusDTO.PeriodTo.Value <= fromDate)
                        {
                            continue;
                        }
                    }

                    if (accountSummaryOptions.ToDate.HasValue)
                    {
                        DateTime toDate = new BusinessDate(executionContext.GetSiteId(), accountSummaryOptions.ToDate.Value).End;
                        if (accountCreditPlusDTO.PeriodFrom.HasValue && accountCreditPlusDTO.PeriodFrom.Value >= toDate)
                        {
                            continue;
                        }
                    }

                    if ((accountCreditPlusDTO.AccountCreditPlusConsumptionDTOList == null ||
                        accountCreditPlusDTO.AccountCreditPlusConsumptionDTOList.Any() == false) && accountCreditPlusDTO.CreditPlusBalance <= 0)
                    {
                        continue;
                    }
                    if (accountCreditPlusDTO.CreditPlusBalance <= 0 &&
                        accountCreditPlusDTO.AccountCreditPlusConsumptionDTOList != null &&
                        accountCreditPlusDTO.AccountCreditPlusConsumptionDTOList.Any(x => x.ConsumptionBalance > 0) == false)
                    {
                        continue;
                    }

                    AccountCreditPlusBL accountCreditPlusBL = new AccountCreditPlusBL(executionContext, accountCreditPlusDTO);
                    AccountCreditPlusSummaryBL accountCreditPlusSummaryBL = accountCreditPlusBL.GetAccountCreditPlusSummaryBL();
                    result.Add(accountCreditPlusSummaryBL);    
                }     
            }

            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the AccountSummaryGame DTO
        /// </summary>
        /// <param name="accountSummaryOptions"></param>
        /// <returns></returns>
        internal List<AccountGameSummaryBL> GetAccountGameSummaryBLList(AccountSummaryOptions accountSummaryOptions)
        {
            log.LogMethodEntry(accountSummaryOptions);
            List<AccountGameSummaryBL> result = new List<AccountGameSummaryBL>();
            if (accountDTO.AccountGameDTOList != null && accountDTO.AccountGameDTOList.Count > 0)
            {
                foreach (AccountGameDTO accountGameDTO in accountDTO.AccountGameDTOList)
                {
                    if (accountGameDTO.ExpiryDate.HasValue && accountGameDTO.ExpiryDate.Value < ServerDateTime.Now)
                    {
                        continue;
                    }

                    if(accountGameDTO.ValidityStatus == AccountDTO.AccountValidityStatus.Hold)
                    {
                        continue;
                    }

                    if (accountSummaryOptions.ShowExpiryEntitlements == true)
                    {
                        if (accountSummaryOptions.ToDate.HasValue == false)
                        {
                            string customException = MessageContainerList.GetMessage(executionContext, 597);
                            log.Error(customException);
                            throw new ValidationException(customException);
                        }

                        DateTime fromDate;

                        if (accountSummaryOptions.FromDate.HasValue == false)
                        {
                            fromDate = new BusinessDate(executionContext.GetSiteId(), accountSummaryOptions.ToDate.Value).Start;
                        }
                        else
                        {
                            fromDate = new BusinessDate(executionContext.GetSiteId(), accountSummaryOptions.FromDate.Value).Start;
                        }
                        DateTime toDate = new BusinessDate(executionContext.GetSiteId(), accountSummaryOptions.ToDate.Value).End;
                        if (accountGameDTO.ExpiryDate.HasValue == false || accountGameDTO.ExpiryDate.Value < fromDate || accountGameDTO.ExpiryDate.Value > toDate)
                        {
                            continue;
                        }
                    }

                    if (accountSummaryOptions.FromDate.HasValue)
                    {
                        DateTime fromDate = new BusinessDate(executionContext.GetSiteId(), accountSummaryOptions.FromDate.Value).Start;
                        if (accountGameDTO.ExpiryDate.HasValue && accountGameDTO.ExpiryDate.Value <= fromDate)
                        {
                            continue;
                        }
                    }

                    if (accountSummaryOptions.ToDate.HasValue)
                    {
                        DateTime toDate = new BusinessDate(executionContext.GetSiteId(), accountSummaryOptions.ToDate.Value).End;
                        if (accountGameDTO.FromDate.HasValue && accountGameDTO.FromDate.Value >= toDate)
                        {
                            continue;
                        }

                    }
                    if ((accountGameDTO.AccountGameExtendedDTOList == null ||
                        accountGameDTO.AccountGameExtendedDTOList.Any() == false) && accountGameDTO.BalanceGames <= 0)
                    {
                        continue;
                    }

                    AccountGameBL accountGameBL = new AccountGameBL(executionContext, accountGameDTO);
                    AccountGameSummaryBL accountGameSummaryBL = accountGameBL.GetAccountGameSummaryBL();
                    result.Add(accountGameSummaryBL);
                }
            }

            log.LogMethodExit(result);
            return result;
        }
    }

    /// <summary>
    /// Manages the list of Account
    /// </summary>
    public class AccountListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        private List<AccountDTO> accountDTOList;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public AccountListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="accountDTOList"></param>
        public AccountListBL(ExecutionContext executionContext, List<AccountDTO> accountDTOList)
        {
            log.LogMethodEntry(executionContext, accountDTOList);
            this.executionContext = executionContext;
            this.accountDTOList = accountDTOList;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the Account list
        /// </summary>
        public List<AccountDTO> GetAccountDTOList(List<KeyValuePair<AccountDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            AccountDataHandler accountDataHandler = new AccountDataHandler(sqlTransaction);
            List<AccountDTO> returnValue = accountDataHandler.GetAccountDTOList(searchParameters);
            if (RefreshAccountsFromHQ(returnValue))
            {
                returnValue = accountDataHandler.GetAccountDTOList(searchParameters);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Returns the no of accounts matching the search criteria
        /// </summary>
        /// <param name="searchCriteria">search criteria</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        /// <returns></returns>
        public int GetAccountCount(AccountSearchCriteria searchCriteria, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchCriteria, sqlTransaction);
            AccountDataHandler accountDataHandler = new AccountDataHandler(sqlTransaction);
            int accountCount = accountDataHandler.GetAccountCount(searchCriteria);
            log.LogMethodExit(accountCount);
            return accountCount;
        }

        /// <summary>
        /// Returns the Account list
        /// </summary>
        public List<AccountDTO> GetAccountDTOList(AccountSearchCriteria searchCriteria,
            bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null, bool buildActivityHistory = false, bool buildGamePlayHistory = false,
            int lastActivityHistoryId = -1, int lastGamePlayHistoryId = -1, int numberOfRecords = 10, int activityHistoryPageNumber = 0, int gamePlayHistoryPageNumber = 0,
            bool includeFutureEntitlements = false, List<KeyValuePair<AccountActivityDTO.SearchByParameters, string>> accountActivitySearchParameters = null, List<KeyValuePair<GamePlayDTO.SearchByParameters, string>> gamePlaySearchParameters = null)
        {
            log.LogMethodEntry(searchCriteria, loadChildRecords, activeChildRecords, sqlTransaction);
            AccountDataHandler accountDataHandler = new AccountDataHandler(sqlTransaction);
            List<AccountDTO> accountDTOList = accountDataHandler.GetAccountDTOList(searchCriteria);
            if (RefreshAccountsFromHQ(accountDTOList))
            {
                accountDTOList = accountDataHandler.GetAccountDTOList(searchCriteria);
            }
            if (loadChildRecords)
            {
                if (accountDTOList != null && accountDTOList.Count > 0)
                {
                    AccountBuilderBL accountBuilder = new AccountBuilderBL(executionContext);
                    accountBuilder.Build(accountDTOList, activeChildRecords, sqlTransaction, buildActivityHistory, buildGamePlayHistory,
                        lastActivityHistoryId, activityHistoryPageNumber, lastGamePlayHistoryId, gamePlayHistoryPageNumber, numberOfRecords, includeFutureEntitlements, accountActivitySearchParameters, gamePlaySearchParameters);
                }
            }
            log.LogMethodExit(accountDTOList);
            return accountDTOList;
        }

        /// <summary>
        /// Returns the no of accounts matching the search criteria
        /// </summary>
        /// <param name="searchParameters"> search criteria</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        /// <returns></returns>
        public int GetAccountCount(List<KeyValuePair<AccountDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            AccountDataHandler accountDataHandler = new AccountDataHandler(sqlTransaction);
            int accountCount = accountDataHandler.GetAccountCount(searchParameters);
            log.LogMethodExit(accountCount);
            return accountCount;
        }

        /// <summary>
        /// Returns the Account list
        /// </summary>
        public List<AccountDTO> GetAccountDTOList(List<KeyValuePair<AccountDTO.SearchByParameters, string>> searchParameters,
            bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);
            AccountDataHandler accountDataHandler = new AccountDataHandler(sqlTransaction);
            List<AccountDTO> accountDTOList = accountDataHandler.GetAccountDTOList(searchParameters);
            if (RefreshAccountsFromHQ(accountDTOList))
            {
                accountDTOList = accountDataHandler.GetAccountDTOList(searchParameters);
            }
            if (loadChildRecords)
            {
                if (accountDTOList != null && accountDTOList.Count > 0)
                {
                    AccountBuilderBL accountBuilder = new AccountBuilderBL(executionContext);
                    accountBuilder.Build(accountDTOList, activeChildRecords, sqlTransaction);
                }
            }
            log.LogMethodExit(accountDTOList);
            return accountDTOList;
        }

        /// <summary>
        /// Returns the Account list
        /// </summary>
        public List<AccountDTO> GetAccountDTOList(List<KeyValuePair<AccountDTO.SearchByParameters, string>> searchParameters,
            int pageNumber, int pageSize, bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null, bool buildActivityHistory = false, bool buildGamePlayHistory = false,
            int lastActivityHistoryId = -1, int lastGamePlayHistoryId = -1, int activityHistoryPageNumber = 0, int gamePlayHistoryPageNumber = 0, bool includeFutureEntitlements = false,
            List<KeyValuePair<AccountActivityDTO.SearchByParameters, string>> accountActivitySearchParameters = null, List<KeyValuePair<GamePlayDTO.SearchByParameters, string>> gamePlaySearchParameters = null)
        {
            log.LogMethodEntry(searchParameters, pageNumber, pageSize, loadChildRecords, activeChildRecords, sqlTransaction);
            AccountDataHandler accountDataHandler = new AccountDataHandler(sqlTransaction);
            List<AccountDTO> accountDTOList = accountDataHandler.GetAccountDTOList(searchParameters, pageNumber, pageSize);
            if (RefreshAccountsFromHQ(accountDTOList))
            {
                accountDTOList = accountDataHandler.GetAccountDTOList(searchParameters, pageNumber, pageSize);
            }
            if (loadChildRecords)
            {
                if (accountDTOList != null && accountDTOList.Count > 0)
                {
                    AccountBuilderBL accountBuilder = new AccountBuilderBL(executionContext);
                    accountBuilder.Build(accountDTOList, activeChildRecords, sqlTransaction, buildActivityHistory, buildGamePlayHistory,
                        lastActivityHistoryId, activityHistoryPageNumber, lastGamePlayHistoryId, gamePlayHistoryPageNumber, pageSize, includeFutureEntitlements, accountActivitySearchParameters, gamePlaySearchParameters);
                }
            }
            log.LogMethodExit(accountDTOList);
            return accountDTOList;
        }
        /// <summary>
        /// This method should be used to Save and Update the Accounts details for Web Management Studio.
        /// </summary>
        public List<AccountDTO> SaveUpdateManualChangesList(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            List<AccountDTO> updatedAccountDTOList = new List<AccountDTO>();
            if (accountDTOList != null)
            {
                foreach (AccountDTO accountDTO in accountDTOList)
                {
                    try
                    {
                        AccountBL accountBLObj = new AccountBL(executionContext, accountDTO);
                        accountBLObj.SaveManualChanges(true, sqlTransaction);
                        updatedAccountDTOList.Add(accountBLObj.AccountDTO);
                    }
                    catch (SqlException sqlEx)
                    {
                        log.Error(sqlEx);
                        log.LogMethodExit(null, "Throwing Sql Exception : " + sqlEx.Message);
                        if (sqlEx.Number == 547)
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));// Unable to delete this record.Please check the reference record first.
                        }
                        else
                        {
                            throw;
                        }
                    }
                    catch (ValidationException valEx)
                    {
                        log.Error(valEx);
                        log.LogMethodExit(null, "Throwing Exception : " + valEx.Message);
                        throw;
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                        throw;
                    }
                }
            }
            log.LogMethodExit(updatedAccountDTOList);
            return updatedAccountDTOList;
        }

        private bool RefreshAccountsFromHQ(List<AccountDTO> accountDTOList)
        {
            log.LogMethodEntry(accountDTOList);
            bool result = false;
            if (accountDTOList == null || accountDTOList.Any() == false)
            {
                log.LogMethodExit(result, "accountDTOList is empty");
                return result;
            }
            foreach (var accountDTO in accountDTOList)
            {
                result = result || AccountBL.RefreshAccountFromHQ(executionContext, accountDTO);
            }
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Card Number Info For Execute Process
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<AccountDTO> CardNumberInfoForExecuteProcess(List<string> accountNumberList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(accountNumberList, sqlTransaction);
            AccountDataHandler accountDataHandler = new AccountDataHandler(sqlTransaction);
            bool reactivateExpiredCards = ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "REACTIVATE_EXPIRED_CARD", false);
            int expireAfterMonths = -1;
            int bonusDays = 0;
            if (reactivateExpiredCards)
            {
                bonusDays = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "CARD_EXPIRY_GRACE_PERIOD", 0);
                expireAfterMonths = (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CARD_EXPIRY_RULE") == "ISSUEDATE"
                                        ? ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "CARD_VALIDITY", -1) : -1);
            }
            List<AccountDTO> accountDTOList = accountDataHandler.CardNumberInfoForExecuteProcess(accountNumberList, reactivateExpiredCards, expireAfterMonths, bonusDays);
            log.LogMethodExit();
            return accountDTOList;
        }

        public List<AccountDTO> GetAccountDTOList(List<int> accountIdList, bool loadActiveRecords = true,
            bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(accountIdList, loadActiveRecords, loadChildRecords, activeChildRecords, sqlTransaction);
            AccountDataHandler accountDataHandler = new AccountDataHandler(sqlTransaction);
            List<AccountDTO> accountDTOList = accountDataHandler.GetAccountDTOList(accountIdList, loadActiveRecords);
            if (RefreshAccountsFromHQ(accountDTOList))
            {
                accountDTOList = accountDataHandler.GetAccountDTOList(accountIdList, loadActiveRecords);
            }
            if (loadChildRecords)
            {
                if (accountDTOList != null && accountDTOList.Count > 0)
                {
                    AccountBuilderBL accountBuilder = new AccountBuilderBL(executionContext);
                    accountBuilder.Build(accountDTOList, activeChildRecords, sqlTransaction);
                }
            }
            log.LogMethodExit(accountDTOList);
            return accountDTOList;
        }


        public List<AccountDTO> GetAccountDTOListByCustomerIds(List<int> customerIdList, bool loadActiveRecords = true,
            bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(customerIdList, loadActiveRecords, loadChildRecords, activeChildRecords, sqlTransaction);
            AccountDataHandler accountDataHandler = new AccountDataHandler(sqlTransaction);
            List<AccountDTO> accountDTOList = accountDataHandler.GetAccountDTOListByCustomerIds(customerIdList, loadActiveRecords);
            if (RefreshAccountsFromHQ(accountDTOList))
            {
                accountDTOList = accountDataHandler.GetAccountDTOListByCustomerIds(customerIdList, loadActiveRecords);
            }
            if (loadChildRecords)
            {
                if (accountDTOList != null && accountDTOList.Count > 0)
                {
                    AccountBuilderBL accountBuilder = new AccountBuilderBL(executionContext);
                    accountBuilder.Build(accountDTOList, activeChildRecords, sqlTransaction);
                }
            }
            log.LogMethodExit(accountDTOList);
            return accountDTOList;
        }
    }

    /// <summary>
    /// Builds the complex Account entity structure
    /// </summary>
    public class AccountBuilderBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public AccountBuilderBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Builds the complex account DTO structure
        /// </summary>
        /// <param name="accountDTO">Account dto</param>
        /// <param name="activeChildRecords">whether to load only active child records</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public void Build(AccountDTO accountDTO, bool activeChildRecords = true, SqlTransaction sqlTransaction = null, bool buildActivityHistory = false, bool buildGamePlayHistory = false,
            int lastActivityHistoryId = -1, int activityHistoryPageNumber = 0, int lastGamePlayHistoryId = -1, int gamePlayHistoryPageNumber = 0, int numberOfRecords = 10, bool includeFutureEntitlements = false,
            List<KeyValuePair<AccountActivityDTO.SearchByParameters, string>> accountActivitySearchParameters = null, List<KeyValuePair<GamePlayDTO.SearchByParameters, string>> gamePlaySearchParameters = null)
        {
            log.LogMethodEntry(accountDTO, activeChildRecords);
            if (accountDTO != null && accountDTO.AccountId != -1)
            {
                AccountCreditPlusListBL accountCreditPlusListBL = new AccountCreditPlusListBL(executionContext);
                List<KeyValuePair<AccountCreditPlusDTO.SearchByParameters, string>> accountCreditPlusSearchParams = new List<KeyValuePair<AccountCreditPlusDTO.SearchByParameters, string>>();
                accountCreditPlusSearchParams.Add(new KeyValuePair<AccountCreditPlusDTO.SearchByParameters, string>(AccountCreditPlusDTO.SearchByParameters.ACCOUNT_ID, accountDTO.AccountId.ToString()));
                accountDTO.AccountCreditPlusDTOList = accountCreditPlusListBL.GetAccountCreditPlusDTOList(accountCreditPlusSearchParams, true, activeChildRecords, sqlTransaction);

                AccountGameListBL accountGameListBL = new AccountGameListBL(executionContext);
                List<KeyValuePair<AccountGameDTO.SearchByParameters, string>> accountGameSearchParams = new List<KeyValuePair<AccountGameDTO.SearchByParameters, string>>();
                accountGameSearchParams.Add(new KeyValuePair<AccountGameDTO.SearchByParameters, string>(AccountGameDTO.SearchByParameters.ACCOUNT_ID, accountDTO.AccountId.ToString()));
                accountDTO.AccountGameDTOList = accountGameListBL.GetAccountGameDTOList(accountGameSearchParams, true, activeChildRecords, sqlTransaction);

                AccountDiscountListBL accountDiscountListBL = new AccountDiscountListBL(executionContext);
                List<KeyValuePair<AccountDiscountDTO.SearchByParameters, string>> accountDiscountSearchParams = new List<KeyValuePair<AccountDiscountDTO.SearchByParameters, string>>();
                if (activeChildRecords)
                {
                    accountDiscountSearchParams.Add(new KeyValuePair<AccountDiscountDTO.SearchByParameters, string>(AccountDiscountDTO.SearchByParameters.IS_ACTIVE, "Y"));
                }
                accountDiscountSearchParams.Add(new KeyValuePair<AccountDiscountDTO.SearchByParameters, string>(AccountDiscountDTO.SearchByParameters.ACCOUNT_ID, accountDTO.AccountId.ToString()));
                accountDTO.AccountDiscountDTOList = accountDiscountListBL.GetAccountDiscountDTOList(accountDiscountSearchParams, sqlTransaction);

                AccountRelationshipListBL accountRelationshipListBL = new AccountRelationshipListBL(executionContext);
                List<KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>> accountRelationshipSearchParams = new List<KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>>();
                if (activeChildRecords)
                {
                    accountRelationshipSearchParams.Add(new KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>(AccountRelationshipDTO.SearchByParameters.IS_ACTIVE, "1"));
                    accountRelationshipSearchParams.Add(new KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>(AccountRelationshipDTO.SearchByParameters.VALID_ACCOUNTS, "Y"));
                }
                accountRelationshipSearchParams.Add(new KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>(AccountRelationshipDTO.SearchByParameters.ACCOUNT_ID, accountDTO.AccountId.ToString()));
                accountDTO.AccountRelationshipDTOList = accountRelationshipListBL.GetAccountRelationshipDTOList(accountRelationshipSearchParams, sqlTransaction);

                accountRelationshipSearchParams = new List<KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>>();
                if (activeChildRecords)
                {
                    accountRelationshipSearchParams.Add(new KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>(AccountRelationshipDTO.SearchByParameters.IS_ACTIVE, "1"));
                    accountRelationshipSearchParams.Add(new KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>(AccountRelationshipDTO.SearchByParameters.VALID_ACCOUNTS, "Y"));
                }
                accountRelationshipSearchParams.Add(new KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>(AccountRelationshipDTO.SearchByParameters.RELATED_ACCOUNT_ID, accountDTO.AccountId.ToString()));
                List<AccountRelationshipDTO> accountRelationshipDTOList = accountRelationshipListBL.GetAccountRelationshipDTOList(accountRelationshipSearchParams, sqlTransaction);
                if (accountRelationshipDTOList != null)
                {
                    if (accountDTO.AccountRelationshipDTOList == null)
                    {
                        accountDTO.AccountRelationshipDTOList = new List<AccountRelationshipDTO>();
                    }
                    accountDTO.AccountRelationshipDTOList.AddRange(accountRelationshipDTOList);
                }

                if (buildGamePlayHistory)
                {
                    GamePlaySummaryListBL gamePlaySummaryListBL = new GamePlaySummaryListBL(executionContext);
                    List<KeyValuePair<GamePlayDTO.SearchByParameters, string>> gameSearchParameters = new List<KeyValuePair<GamePlayDTO.SearchByParameters, string>>();
                    gameSearchParameters.Add(new KeyValuePair<GamePlayDTO.SearchByParameters, string>(GamePlayDTO.SearchByParameters.CARD_ID, accountDTO.AccountId.ToString()));

                    if (lastGamePlayHistoryId > -1)
                        gameSearchParameters.Add(new KeyValuePair<GamePlayDTO.SearchByParameters, string>(GamePlayDTO.SearchByParameters.LAST_GAMEPLAY_ID_OF_SET, lastGamePlayHistoryId.ToString()));

                    if (gamePlaySearchParameters != null)
                        gameSearchParameters.AddRange(gamePlaySearchParameters);

                    List<GamePlayDTO> gamePlayDTOList = gamePlaySummaryListBL.GetGamePlayDTOList(gameSearchParameters, true, numberOfRecords: numberOfRecords, pageNumber: gamePlayHistoryPageNumber);
                    if (gamePlayDTOList != null)
                    {
                        if (accountDTO.GamePlayDTOList == null)
                        {
                            accountDTO.GamePlayDTOList = new List<GamePlayDTO>();
                        }
                        accountDTO.GamePlayDTOList.AddRange(gamePlayDTOList);
                    }
                }

                if (buildActivityHistory)
                {
                    AccountActivityViewListBL accountActivityViewListBL = new AccountActivityViewListBL(executionContext);
                    List<KeyValuePair<AccountActivityDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AccountActivityDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<AccountActivityDTO.SearchByParameters, string>(AccountActivityDTO.SearchByParameters.ACCOUNT_ID, accountDTO.AccountId.ToString()));

                    if (accountActivitySearchParameters != null)
                        searchParameters.AddRange(accountActivitySearchParameters);

                    List<AccountActivityDTO> accountActivityDTOList = accountActivityViewListBL.GetAccountActivityDTOList(searchParameters, true, numberOfRecords: numberOfRecords, pageNumber: activityHistoryPageNumber, lastRowNumberId: lastActivityHistoryId);
                    if (accountActivityDTOList != null)
                    {
                        if (accountDTO.AccountActivityDTOList == null)
                        {
                            accountDTO.AccountActivityDTOList = new List<AccountActivityDTO>();
                        }
                        accountDTO.AccountActivityDTOList.AddRange(accountActivityDTOList);
                    }
                }

                AccountSummaryBL accountSummaryBL = new AccountSummaryBL(executionContext, accountDTO, includeFutureEntitlements);
                accountDTO.AccountSummaryDTO = accountSummaryBL.AccountSummaryDTO;
                accountDTO.TotalCreditsBalance = (accountDTO.Credits.HasValue ? accountDTO.Credits.Value : 0) + accountSummaryBL.AccountSummaryDTO.CreditPlusCardBalance.Value
                    + accountSummaryBL.AccountSummaryDTO.CreditPlusGamePlayCredits.Value + accountSummaryBL.AccountSummaryDTO.CreditPlusItemPurchase.Value;
                accountDTO.TotalBonusBalance = accountSummaryBL.AccountSummaryDTO.TotalBonusBalance;
                accountDTO.TotalCourtesyBalance = accountSummaryBL.AccountSummaryDTO.TotalCourtesyBalance;
                accountDTO.TotalTimeBalance = accountSummaryBL.AccountSummaryDTO.TotalTimeBalance;
                accountDTO.TotalGamesBalance = accountSummaryBL.AccountSummaryDTO.TotalGamesBalance;
                accountDTO.TotalTicketsBalance = accountSummaryBL.AccountSummaryDTO.TotalTicketsBalance;
                //Virtual arcade changes
                accountDTO.TotalVirtualPointBalance = accountSummaryBL.AccountSummaryDTO.TotalVirtualPointBalance;

            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Builds the complex accountDTO structure
        /// </summary>
        /// <param name="accountDTOList">Account dto list</param>
        /// <param name="activeChildRecords">whether to load only active child records</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public void Build(List<AccountDTO> accountDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null, bool buildActivityHistory = false, bool buildGamePlayHistory = false,
                          int lastActivityHistoryId = -1, int activityHistoryPageNumber = 0, int lastGamePlayHistoryId = -1, int gamePlayHistoryPageNumber = 0, int numberOfRecords = 100,
                          bool includeFutureEntitlements = false, List<KeyValuePair<AccountActivityDTO.SearchByParameters, string>> accountActivitySearchParameters = null, List<KeyValuePair<GamePlayDTO.SearchByParameters, string>> gamePlaySearchParameters = null)
        {
            log.LogMethodEntry(accountDTOList, activeChildRecords, sqlTransaction);
            if (accountDTOList != null && accountDTOList.Count > 0)
            {
                //StringBuilder accountIdListStringBuilder = new StringBuilder("");
                Dictionary<int, AccountDTO> accountIdAccountDictionary = new Dictionary<int, AccountDTO>();
                //string accountIdList;
                List<int> accountIdListNew = new List<int>();
                for (int i = 0; i < accountDTOList.Count; i++)
                {
                    if (accountDTOList[i].AccountId != -1)
                    {
                        accountIdAccountDictionary.Add(accountDTOList[i].AccountId, accountDTOList[i]);
                    }
                    //if (i != 0)
                    //{
                    //    accountIdListStringBuilder.Append(",");
                    //}
                    //accountIdListStringBuilder.Append(accountDTOList[i].AccountId.ToString());
                    if (accountIdListNew.Exists(acc => acc == accountDTOList[i].AccountId) == false)
                    {
                        accountIdListNew.Add(accountDTOList[i].AccountId);
                    }
                }
                //accountIdList = accountIdListStringBuilder.ToString();
                AccountCreditPlusListBL accountCreditPlusListBL = new AccountCreditPlusListBL(executionContext);
                //List<KeyValuePair<AccountCreditPlusDTO.SearchByParameters, string>> accountCreditPlusSearchParams = new List<KeyValuePair<AccountCreditPlusDTO.SearchByParameters, string>>();
                //accountCreditPlusSearchParams.Add(new KeyValuePair<AccountCreditPlusDTO.SearchByParameters, string>(AccountCreditPlusDTO.SearchByParameters.ACCOUNT_ID_LIST, accountIdList));
                List<AccountCreditPlusDTO> accountCreditPlusDTOList = accountCreditPlusListBL.GetAccountCreditPlusDTOListByAccountIds(accountIdListNew, true, activeChildRecords, sqlTransaction);
                if (accountCreditPlusDTOList != null)
                {
                    foreach (var accountCreditPlusDTO in accountCreditPlusDTOList)
                    {
                        if (accountIdAccountDictionary.ContainsKey(accountCreditPlusDTO.AccountId))
                        {
                            if (accountIdAccountDictionary[accountCreditPlusDTO.AccountId].AccountCreditPlusDTOList == null)
                            {
                                accountIdAccountDictionary[accountCreditPlusDTO.AccountId].AccountCreditPlusDTOList = new List<AccountCreditPlusDTO>();
                            }
                            accountIdAccountDictionary[accountCreditPlusDTO.AccountId].AccountCreditPlusDTOList.Add(accountCreditPlusDTO);
                        }
                    }
                }

                AccountGameListBL accountGameListBL = new AccountGameListBL(executionContext);
                //List<KeyValuePair<AccountGameDTO.SearchByParameters, string>> accountGameSearchParams = new List<KeyValuePair<AccountGameDTO.SearchByParameters, string>>();
                //accountGameSearchParams.Add(new KeyValuePair<AccountGameDTO.SearchByParameters, string>(AccountGameDTO.SearchByParameters.ACCOUNT_ID_LIST, accountIdList));
                List<AccountGameDTO> accountGameDTOList = accountGameListBL.GetAccountGameDTOListByAccountIds(accountIdListNew, true, activeChildRecords, sqlTransaction);
                if (accountGameDTOList != null)
                {
                    foreach (var accountGameDTO in accountGameDTOList)
                    {
                        if (accountIdAccountDictionary.ContainsKey(accountGameDTO.AccountId))
                        {
                            if (accountIdAccountDictionary[accountGameDTO.AccountId].AccountGameDTOList == null)
                            {
                                accountIdAccountDictionary[accountGameDTO.AccountId].AccountGameDTOList = new List<AccountGameDTO>();
                            }
                            accountIdAccountDictionary[accountGameDTO.AccountId].AccountGameDTOList.Add(accountGameDTO);
                        }
                    }
                }

                AccountDiscountListBL accountDiscountListBL = new AccountDiscountListBL(executionContext);
                //List<KeyValuePair<AccountDiscountDTO.SearchByParameters, string>> accountDiscountSearchParams = new List<KeyValuePair<AccountDiscountDTO.SearchByParameters, string>>();
                //accountDiscountSearchParams.Add(new KeyValuePair<AccountDiscountDTO.SearchByParameters, string>(AccountDiscountDTO.SearchByParameters.ACCOUNT_ID_LIST, accountIdList));
                //if (activeChildRecords)
                //{
                //    accountDiscountSearchParams.Add(new KeyValuePair<AccountDiscountDTO.SearchByParameters, string>(AccountDiscountDTO.SearchByParameters.IS_ACTIVE, "Y"));
                //}
                List<AccountDiscountDTO> accountDiscountDTOList = accountDiscountListBL.GetAccountDiscountDTOListByAccountIds(accountIdListNew, activeChildRecords, sqlTransaction);
                if (accountDiscountDTOList != null)
                {
                    foreach (var accountDiscountDTO in accountDiscountDTOList)
                    {
                        if (accountIdAccountDictionary.ContainsKey(accountDiscountDTO.AccountId))
                        {
                            if (accountIdAccountDictionary[accountDiscountDTO.AccountId].AccountDiscountDTOList == null)
                            {
                                accountIdAccountDictionary[accountDiscountDTO.AccountId].AccountDiscountDTOList = new List<AccountDiscountDTO>();
                            }
                            accountIdAccountDictionary[accountDiscountDTO.AccountId].AccountDiscountDTOList.Add(accountDiscountDTO);
                        }
                    }
                }

                AccountRelationshipListBL accountRelationshipListBL = new AccountRelationshipListBL(executionContext);
                //List<KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>> accountRelationshipSearchParams = new List<KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>>();
                //accountRelationshipSearchParams.Add(new KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>(AccountRelationshipDTO.SearchByParameters.ACCOUNT_ID_LIST, accountIdList));
                //if (activeChildRecords)
                //{
                //    accountRelationshipSearchParams.Add(new KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>(AccountRelationshipDTO.SearchByParameters.IS_ACTIVE, "1"));
                //    accountRelationshipSearchParams.Add(new KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>(AccountRelationshipDTO.SearchByParameters.VALID_ACCOUNTS, "Y"));
                //}
                List<AccountRelationshipDTO> accountRelationshipDTOList = accountRelationshipListBL.GetAccountRelationshipDTOListByAccountIds(accountIdListNew, activeChildRecords, sqlTransaction);
                if (accountRelationshipDTOList != null)
                {
                    foreach (var accountRelationshipDTO in accountRelationshipDTOList)
                    {
                        if (accountIdAccountDictionary.ContainsKey(accountRelationshipDTO.AccountId))
                        {
                            if (accountIdAccountDictionary[accountRelationshipDTO.AccountId].AccountRelationshipDTOList == null)
                            {
                                accountIdAccountDictionary[accountRelationshipDTO.AccountId].AccountRelationshipDTOList = new List<AccountRelationshipDTO>();
                            }
                            accountIdAccountDictionary[accountRelationshipDTO.AccountId].AccountRelationshipDTOList.Add(accountRelationshipDTO);
                        }
                    }
                }

                //accountRelationshipSearchParams = new List<KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>>();
                //accountRelationshipSearchParams.Add(new KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>(AccountRelationshipDTO.SearchByParameters.RELATED_ACCOUNT_ID_LIST, accountIdList));
                //if (activeChildRecords)
                //{
                //    accountRelationshipSearchParams.Add(new KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>(AccountRelationshipDTO.SearchByParameters.IS_ACTIVE, "1"));
                //    accountRelationshipSearchParams.Add(new KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>(AccountRelationshipDTO.SearchByParameters.VALID_ACCOUNTS, "Y"));
                //}
                accountRelationshipDTOList = accountRelationshipListBL.GetAccountRelationshipDTOListByRelatedAccountIds(accountIdListNew, activeChildRecords, sqlTransaction);
                if (accountRelationshipDTOList != null)
                {
                    foreach (var accountRelationshipDTO in accountRelationshipDTOList)
                    {
                        if (accountIdAccountDictionary.ContainsKey(accountRelationshipDTO.RelatedAccountId))
                        {
                            if (accountIdAccountDictionary[accountRelationshipDTO.RelatedAccountId].AccountRelationshipDTOList == null)
                            {
                                accountIdAccountDictionary[accountRelationshipDTO.RelatedAccountId].AccountRelationshipDTOList = new List<AccountRelationshipDTO>();
                            }
                            accountIdAccountDictionary[accountRelationshipDTO.RelatedAccountId].AccountRelationshipDTOList.Add(accountRelationshipDTO);
                        }
                    }
                }

                if (buildGamePlayHistory)
                {
                    GamePlaySummaryListBL gamePlaySummaryListBL = new GamePlaySummaryListBL(executionContext);
                    List<KeyValuePair<GamePlayDTO.SearchByParameters, string>> gameSearchParameters = new List<KeyValuePair<GamePlayDTO.SearchByParameters, string>>();
                    //gameSearchParameters.Add(new KeyValuePair<GamePlayDTO.SearchByParameters, string>(GamePlayDTO.SearchByParameters.CARD_ID_LIST, accountIdList));
                    if (lastGamePlayHistoryId > -1)
                        gameSearchParameters.Add(new KeyValuePair<GamePlayDTO.SearchByParameters, string>(GamePlayDTO.SearchByParameters.LAST_GAMEPLAY_ID_OF_SET, lastGamePlayHistoryId.ToString()));

                    if (gamePlaySearchParameters != null)
                        gameSearchParameters.AddRange(gamePlaySearchParameters);

                    List<GamePlayDTO> gamePlayDTOList = gamePlaySummaryListBL.GetGamePlayDTOListByAccountIds(accountIdListNew, gameSearchParameters, true, numberOfRecords: numberOfRecords, pageNumber: gamePlayHistoryPageNumber);
                    if (gamePlayDTOList != null)
                    {
                        foreach (var gamePlayDTO in gamePlayDTOList)
                        {
                            if (accountIdAccountDictionary.ContainsKey(gamePlayDTO.CardId))
                            {
                                if (accountIdAccountDictionary[gamePlayDTO.CardId].GamePlayDTOList == null)
                                {
                                    accountIdAccountDictionary[gamePlayDTO.CardId].GamePlayDTOList = new List<GamePlayDTO>();
                                }
                                accountIdAccountDictionary[gamePlayDTO.CardId].GamePlayDTOList.Add(gamePlayDTO);
                            }
                        }
                    }
                }

                if (buildActivityHistory)
                {
                    AccountActivityViewListBL accountActivityViewListBL = new AccountActivityViewListBL(executionContext);
                    List<KeyValuePair<AccountActivityDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AccountActivityDTO.SearchByParameters, string>>();
                    //searchParameters.Add(new KeyValuePair<AccountActivityDTO.SearchByParameters, string>(AccountActivityDTO.SearchByParameters.ACCOUNT_ID_LIST, accountIdList));

                    if (accountActivitySearchParameters != null)
                        searchParameters.AddRange(accountActivitySearchParameters);

                    List<AccountActivityDTO> accountActivityDTOList = accountActivityViewListBL.GetAccountActivityDTOListByAccountIds(accountIdListNew, true, numberOfRecords: numberOfRecords, pageNumber: activityHistoryPageNumber, lastRowNumberId: lastActivityHistoryId);
                    if (accountActivityDTOList != null)
                    {
                        foreach (var accountActivityDTO in accountActivityDTOList)
                        {
                            if (accountIdAccountDictionary.ContainsKey(accountActivityDTO.AccountId))
                            {
                                if (accountIdAccountDictionary[accountActivityDTO.AccountId].AccountActivityDTOList == null)
                                {
                                    accountIdAccountDictionary[accountActivityDTO.AccountId].AccountActivityDTOList = new List<AccountActivityDTO>();
                                }
                                accountIdAccountDictionary[accountActivityDTO.AccountId].AccountActivityDTOList.Add(accountActivityDTO);
                            }
                        }
                    }
                }

                foreach (var accountDTO in accountDTOList)
                {
                    AccountSummaryBL accountSummaryBL = new AccountSummaryBL(executionContext, accountDTO, includeFutureEntitlements);
                    accountDTO.AccountSummaryDTO = accountSummaryBL.AccountSummaryDTO;

                    accountDTO.TotalCreditsBalance = (accountDTO.Credits.HasValue ? accountDTO.Credits.Value : 0) + accountSummaryBL.AccountSummaryDTO.CreditPlusCardBalance.Value
                        + accountSummaryBL.AccountSummaryDTO.CreditPlusGamePlayCredits.Value + accountSummaryBL.AccountSummaryDTO.CreditPlusItemPurchase.Value;
                    accountDTO.TotalBonusBalance = accountSummaryBL.AccountSummaryDTO.TotalBonusBalance;
                    accountDTO.TotalCourtesyBalance = accountSummaryBL.AccountSummaryDTO.TotalCourtesyBalance;
                    accountDTO.TotalTimeBalance = accountSummaryBL.AccountSummaryDTO.TotalTimeBalance;
                    accountDTO.TotalGamesBalance = accountSummaryBL.AccountSummaryDTO.TotalGamesBalance;
                    accountDTO.TotalTicketsBalance = accountSummaryBL.AccountSummaryDTO.TotalTicketsBalance;
                    //Virtual arcade changes
                    accountDTO.TotalVirtualPointBalance = accountSummaryBL.AccountSummaryDTO.TotalVirtualPointBalance;
                }
            }
            log.LogMethodExit();
        }

    }
}
