/********************************************************************************************
 * Project Name - Accounts
 * Description  - RemoteAccountUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         13-Nov-2020       Mushahid Faizan         Created : POS UI Redesign with REST API
 2.140.0     23-June-2021       Prashanth               Modidied : Added GetAccountRelationships, CreateAccountRelationships, UpdateAccountRelationships
 2.130.8     10-Jun-2022       Nitin Pai               Added new method to save account nick name - account id
 ********************************************************************************************/
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using Semnox.Parafait.Game;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer.Accounts
{
    public class RemoteAccountUseCases : RemoteUseCases, IAccountUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string ACCOUNT_URL = "api/Customer/Account/Accounts";
        private const string ACCOUNT_RELATIONSHIP_URL = "api/Customer/Account/AccountRelationships";
        private const string ACCOUNT_IDENTIFIER_URL = "api/Customer/Account/{accountId}/AccountIdentifier";
        private const string ACCOUNT_VIEW_URL = "api/Customer/Account/AccountSummaryView";
        private const string ACCOUNT_GAMES_VIEW_URL = "api/Customer/Account/{accountId}/AccountGamesSummary";
        private const string ACCOUNT_DISCOUNTS_VIEW_URL = "api/Customer/Account/{accountId}/AccountDiscountsSummary";
        private const string ACCOUNT_ACTIVITY_VIEW_URL = "api/Customer/Account/{accountId}/AccountActivity";
        private const string ACCOUNT_GAME_PLAYS_URL = "api/Customer/Account/{accountId}/AccountGamePlays";
        private const string ACCOUNT_CREDIT_PLUS_SUMMARY = "api/Customer/Account/AccountCreditPlusSummary";
        private const string ACCOUNT_CREDIT_PLUS_SUMMARY_COUNT = "api/Customer/Account/AccountCreditPlusSummaryCount";
        private const string ACCOUNT_GAME_SUMMARY = "api/Customer/Account/AccountGameSummary";
        private const string ACCOUNT_GAME_SUMMARY_COUNT = "api/Customer/Account/AccountGameSummaryCount";
        public RemoteAccountUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<AccountDTOCollection> GetAccounts(string isActive = "Y", bool chkVipCustomer = false, bool chkRoamingCards = true, bool chkTechCards = false, DateTime? fromDate = null, DateTime? toDate = null,
                                            int accountId = -1, int customerId = -1, string accountNumber = null, int? tagSiteId = null, int pageNumber = 0, int pageSize = 0, bool buildChildRecords = false, bool activeRecordsOnly = false,
                                            bool buildActivityHistory = false, bool buildGamePlayHistory = false, int lastActivityHistoryId = -1, int lastGamePlayHistoryId = -1, string entitlementType = null,
                                            int activityHistoryPageNumber = 0, int gamePlayHistoryPageNumber = 0, bool buildBarCode = false, bool includeFutureEntitlements = false, bool closedTransactionsOnly = true,
                                            DateTime? activityFromDate = null, DateTime? activityToDate = null, string customerName = null)
        {
            log.LogMethodEntry(isActive, chkVipCustomer, chkRoamingCards, chkTechCards, fromDate, toDate,
                                accountId, customerId, accountNumber, tagSiteId, pageNumber, pageSize, buildChildRecords, activeRecordsOnly,
                                buildActivityHistory, buildGamePlayHistory, lastActivityHistoryId, lastGamePlayHistoryId, entitlementType,
                                activityHistoryPageNumber, gamePlayHistoryPageNumber, buildBarCode, includeFutureEntitlements, closedTransactionsOnly,
                                activityFromDate, activityToDate, customerName);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("isActive", isActive));
            searchParameterList.Add(new KeyValuePair<string, string>("chkVipCustomer", chkVipCustomer.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("chkRoamingCards", chkRoamingCards.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("chkTechCards", chkTechCards.ToString()));
            if(fromDate.HasValue)
            {
                searchParameterList.Add(new KeyValuePair<string, string>("fromDate", fromDate.Value.ToString("yyyy-mm-dd hh:mm:ss", CultureInfo.InvariantCulture)));
            }
            if(toDate.HasValue)
            {
                searchParameterList.Add(new KeyValuePair<string, string>("toDate", toDate.Value.ToString("yyyy-mm-dd hh:mm:ss", CultureInfo.InvariantCulture)));
            }
                
            searchParameterList.Add(new KeyValuePair<string, string>("accountId", accountId.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("customerId", customerId.ToString()));
            if (accountNumber != null)
            {
                searchParameterList.Add(new KeyValuePair<string, string>("accountNumber", accountNumber.ToString()));
            }
            if(tagSiteId.HasValue)
            {
                searchParameterList.Add(new KeyValuePair<string, string>("tagSiteId", tagSiteId.ToString()));
            }
            searchParameterList.Add(new KeyValuePair<string, string>("pageNumber", pageNumber.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("pageSize", pageSize.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("buildChildRecords", buildChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("activeRecordsOnly", activeRecordsOnly.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("buildActivityHistory", buildActivityHistory.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("buildGamePlayHistory", buildGamePlayHistory.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("lastActivityHistoryId", lastActivityHistoryId.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("lastGamePlayHistoryId", lastGamePlayHistoryId.ToString()));
            if(entitlementType != null)
            {
                searchParameterList.Add(new KeyValuePair<string, string>("entitlementType", entitlementType.ToString()));
            }
            
            searchParameterList.Add(new KeyValuePair<string, string>("activityHistoryPageNumber", activityHistoryPageNumber.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("gamePlayHistoryPageNumber", gamePlayHistoryPageNumber.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("buildBarCode", buildBarCode.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("includeFutureEntitlements", includeFutureEntitlements.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("closedTransactionsOnly", closedTransactionsOnly.ToString()));
            if(activityFromDate.HasValue)
            {
                searchParameterList.Add(new KeyValuePair<string, string>("activityFromDate", activityFromDate.Value.ToString("yyyy-mm-dd hh:mm:ss", CultureInfo.InvariantCulture)));
            }
            if(activityToDate.HasValue)
            {
                searchParameterList.Add(new KeyValuePair<string, string>("activityToDate", activityToDate.Value.ToString("yyyy-mm-dd hh:mm:ss", CultureInfo.InvariantCulture)));
            }
            if(customerName != null)
            {
                searchParameterList.Add(new KeyValuePair<string, string>("customerName", customerName.ToString()));
            }
            
            AccountDTOCollection result = await Get<AccountDTOCollection>(ACCOUNT_URL, searchParameterList, string.Empty);
            log.LogMethodExit(result);
            return result;
        }

        public async Task<List<AccountDTO>> SaveAccounts(List<AccountDTO> accountDTOList)
        {
            log.LogMethodEntry(accountDTOList);
            try
            {
                List<AccountDTO> updatedAccountDTOList = await Post<List<AccountDTO>>(ACCOUNT_URL, accountDTOList);
                log.LogMethodExit(updatedAccountDTOList);
                return updatedAccountDTOList;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        

        public async Task<List<AccountRelationshipDTO>> GetAccountRelationships(List<KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<KeyValuePair<string, string>> searchParams = new List<KeyValuePair<string, string>>();
            searchParams = BuildSearchParameters(searchParameters);
            List<AccountRelationshipDTO> result = null;
            try
            {
                result = await Get<List<AccountRelationshipDTO>>(ACCOUNT_RELATIONSHIP_URL, searchParams);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
            log.LogMethodExit(result);
            return result;
        }

        private List<KeyValuePair<string, string>> BuildSearchParameters(List<KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>> lookUpSearchParams)
        {
            log.LogMethodEntry(lookUpSearchParams);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<AccountRelationshipDTO.SearchByParameters, string> lookUpSearchParameter in lookUpSearchParams)
            {
                switch (lookUpSearchParameter.Key)
                {
                    case AccountRelationshipDTO.SearchByParameters.ACCOUNT_ID:
                        searchParameterList.Add(new KeyValuePair<string, string>("accountId".ToString(), lookUpSearchParameter.Value));
                        break;
                    case AccountRelationshipDTO.SearchByParameters.IS_ACTIVE:
                        searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), lookUpSearchParameter.Value));
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<List<AccountRelationshipDTO>> UpdateAccountRelationships(List<AccountRelationshipDTO> accountRelationshipDTOs)
        {
            log.LogMethodEntry(accountRelationshipDTOs);
            try
            {
                List<AccountRelationshipDTO> result = await Put<List<AccountRelationshipDTO>>(ACCOUNT_RELATIONSHIP_URL, accountRelationshipDTOs);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<List<LinkNewCardDTO>> CreateAccountRelationships(List<LinkNewCardDTO> newAccountRelationshipDTOs)
        {
            log.LogMethodEntry(newAccountRelationshipDTOs);
            try
            {
                List<LinkNewCardDTO> result = await Post<List<LinkNewCardDTO>>(ACCOUNT_RELATIONSHIP_URL, newAccountRelationshipDTOs);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<AccountDTO> SaveAccountIdentifier(int accountId, AccountDTO accountDTO)
        {
            log.LogMethodEntry(accountDTO);
            try
            {
                String remoteURL = ACCOUNT_IDENTIFIER_URL.Replace("{accountId}", accountId.ToString());
                AccountDTO updatedAccountDTO = await Post<AccountDTO>(remoteURL, accountDTO);
                log.LogMethodExit(updatedAccountDTO);
                return updatedAccountDTO;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }
        /// <summary>
        /// Returns the List of AccountSummaryViewDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of AccountSummaryViewDTO </returns>
        public async Task<List<AccountSummaryViewDTO>> GetAccountSummaryViewDTOList(string isActive = "Y", string accountIdList = null, string accountNumberList = null, int accountId = -1, string accountNumber = null, bool validFlag = false, int customerId = -1,
                                                           int? cardTypeId = -1, int uploadSiteId = -1, int masterEntityId = -1,
                                                           string cardIdentifier = null, int membershipId = -1, string membershipName = null, DateTime? entitlementFromDate = null, DateTime? entitlementToDate = null, bool includeFutureEntitlements = false, bool showExpiryEntitlements = false)
        {
            log.LogMethodEntry(isActive, accountId, accountIdList, accountNumberList, accountNumber, validFlag, customerId, cardTypeId, uploadSiteId, masterEntityId, cardIdentifier,
                               membershipId, membershipName, entitlementFromDate, entitlementToDate, includeFutureEntitlements, showExpiryEntitlements, showExpiryEntitlements);


            List<KeyValuePair<string, string>> searchParameters = new List<KeyValuePair<string, string>>();
            searchParameters.Add(new KeyValuePair<string, string>("isActive", isActive));
            searchParameters.Add(new KeyValuePair<string, string>("accountIdList", accountIdList));
            searchParameters.Add(new KeyValuePair<string, string>("accountNumberList", accountNumberList));
            searchParameters.Add(new KeyValuePair<string, string>("accountId", accountId.ToString()));
            searchParameters.Add(new KeyValuePair<string, string>("accountNumber", accountNumber));
            searchParameters.Add(new KeyValuePair<string, string>("validFlag", validFlag.ToString()));
            searchParameters.Add(new KeyValuePair<string, string>("customerId", customerId.ToString()));
            searchParameters.Add(new KeyValuePair<string, string>("cardTypeId", cardTypeId.ToString()));
            searchParameters.Add(new KeyValuePair<string, string>("uploadSiteId", uploadSiteId.ToString()));
            searchParameters.Add(new KeyValuePair<string, string>("masterEntityId", masterEntityId.ToString()));
            searchParameters.Add(new KeyValuePair<string, string>("cardIdentifier", cardIdentifier));
            searchParameters.Add(new KeyValuePair<string, string>("membershipId", membershipId.ToString()));
            searchParameters.Add(new KeyValuePair<string, string>("membershipName", membershipName));
            searchParameters.Add(new KeyValuePair<string, string>("entitlementFromDate", entitlementFromDate.ToString()));
            searchParameters.Add(new KeyValuePair<string, string>("entitlementToDate", entitlementToDate.ToString()));
            searchParameters.Add(new KeyValuePair<string, string>("includeFutureEntitlements", includeFutureEntitlements.ToString()));
            searchParameters.Add(new KeyValuePair<string, string>("showExpiryEntitlements", showExpiryEntitlements.ToString()));
            List<AccountSummaryViewDTO>  result = await Get<List<AccountSummaryViewDTO>>(ACCOUNT_VIEW_URL, searchParameters);
            log.LogMethodExit(result);
            return result;

        }

        /// <summary>
        /// Returns the List of AccountGamesSummaryViewDTO based on the search parameters.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idList"></param>
        /// <returns></returns>
        public async Task<List<AccountGamesSummaryViewDTO>> GetAccountGamesSummaryViewDTOList(int accountId)
        {
            log.LogMethodEntry(accountId);
            List<KeyValuePair<string, string>> searchParameters = new List<KeyValuePair<string, string>>();
            searchParameters.Add(new KeyValuePair<string, string>("accountId", accountId.ToString()));
            List<AccountGamesSummaryViewDTO> result = await Get<List<AccountGamesSummaryViewDTO>>(ACCOUNT_GAMES_VIEW_URL.Replace("{accountId}", accountId.ToString()), searchParameters);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the List of AccountDiscountsSummaryViewTO based on the search parameters.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idList"></param>
        /// <returns></returns>
        public async Task<List<AccountDiscountsSummaryViewDTO>> GetAccountDiscountsSummaryViewDTOList(int accountId)
        {
            log.LogMethodEntry(accountId);
            List<KeyValuePair<string, string>> searchParameters = new List<KeyValuePair<string, string>>();
            searchParameters.Add(new KeyValuePair<string, string>("accountId", accountId.ToString()));
            List<AccountDiscountsSummaryViewDTO> result = await Get<List<AccountDiscountsSummaryViewDTO>>(ACCOUNT_DISCOUNTS_VIEW_URL.Replace("{accountId}", accountId.ToString()), searchParameters);
            log.LogMethodExit(result);
            return result;
        }

        public async Task<List<AccountActivityDTO>> GetAccountActivityDTOList(int accountId, string startDate = "", string endDate = "", int numberOfDays = 180, int pageNumber = 0, bool addSummaryRow = false, int lastRowNumberId = -1)
        {
            log.LogMethodEntry(accountId, startDate, endDate, numberOfDays, pageNumber, addSummaryRow, lastRowNumberId);
            List<KeyValuePair<string, string>> searchParameters = new List<KeyValuePair<string, string>>();
            searchParameters.Add(new KeyValuePair<string, string>("accountId", accountId.ToString()));
            if (!string.IsNullOrWhiteSpace(startDate))
            {
                searchParameters.Add(new KeyValuePair<string, string>("startDate", startDate));
            }
            if (!string.IsNullOrWhiteSpace(endDate))
            {
                searchParameters.Add(new KeyValuePair<string, string>("endDate", endDate));
            }
            searchParameters.Add(new KeyValuePair<string, string>("numberOfDays", numberOfDays.ToString()));
            searchParameters.Add(new KeyValuePair<string, string>("pageNumber", pageNumber.ToString()));
            searchParameters.Add(new KeyValuePair<string, string>("addSummaryRow", addSummaryRow.ToString()));
            searchParameters.Add(new KeyValuePair<string, string>("lastRowNumberId", lastRowNumberId.ToString()));
            List<AccountActivityDTO> result = await Get<List<AccountActivityDTO>>(ACCOUNT_ACTIVITY_VIEW_URL.Replace("{accountId}", accountId.ToString()), searchParameters);
            log.LogMethodExit(result);
            return result;
        }

        public async Task<List<GamePlayDTO>> GetAccountGamePlaysDTOList(int accountId, String startDate = "", String endDate = "", int numberOfDays = 180, int pageNumber = 0, bool addSummaryRow = false, bool addDetails = false)
        {
            log.LogMethodEntry(accountId, startDate, endDate, numberOfDays, pageNumber, addSummaryRow, addDetails);
            List<KeyValuePair<string, string>> searchParameters = new List<KeyValuePair<string, string>>();
            searchParameters.Add(new KeyValuePair<string, string>("accountId", accountId.ToString()));
            if (!string.IsNullOrWhiteSpace(startDate))
            {
                searchParameters.Add(new KeyValuePair<string, string>("startDate", startDate));
            }
            if (!string.IsNullOrWhiteSpace(endDate))
            {
                searchParameters.Add(new KeyValuePair<string, string>("endDate", endDate));
            }
            searchParameters.Add(new KeyValuePair<string, string>("numberOfDays", numberOfDays.ToString()));
            searchParameters.Add(new KeyValuePair<string, string>("pageNumber", pageNumber.ToString()));
            searchParameters.Add(new KeyValuePair<string, string>("addSummaryRow", addSummaryRow.ToString()));
            searchParameters.Add(new KeyValuePair<string, string>("addDetails", addDetails.ToString()));
            List<GamePlayDTO> result = await Get<List<GamePlayDTO>>(ACCOUNT_GAME_PLAYS_URL.Replace("{accountId}", accountId.ToString()), searchParameters);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the List of AccountCreditPlusSummaryDTO based on the search parameters.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="accountNumber"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="creditPlusType"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<List<AccountCreditPlusSummaryDTO>> GetAccountCreditPlusSummaryDTOList(int accountId = -1, string accountNumber = null, DateTime? fromDate = null, DateTime? toDate = null, string creditPlusType = null, bool showExpiryEntitlements = false, int currentPage = 0,
                                                                        int pageSize = 50)
        {
            log.LogMethodEntry(accountId, accountNumber, fromDate, toDate, creditPlusType, showExpiryEntitlements, currentPage, pageSize);
            List<AccountCreditPlusSummaryDTO> result = await Get<List<AccountCreditPlusSummaryDTO>>(ACCOUNT_CREDIT_PLUS_SUMMARY, new WebApiGetRequestParameterCollection("accountId", accountId,
                                                                                                                                                                       "accountNumber", accountNumber,
                                                                                                                                                                       "fromDate", fromDate,
                                                                                                                                                                       "toDate", toDate,
                                                                                                                                                                       "creditPlusType", creditPlusType,
                                                                                                                                                                       "showExpiryEntitlements", showExpiryEntitlements,
                                                                                                                                                                       "pageNumber", currentPage,
                                                                                                                                                                       "pageSize", pageSize));
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the count of AccountCreditPlusSummaryDTO based on the search parameters.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="accountNumber"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="creditPlusType"></param>
        /// <returns></returns>
        public async Task<int> GetAccountCreditPlusSummaryDTOListCount(int accountId = -1, string accountNumber = null, DateTime? fromDate = null, DateTime? toDate = null, string creditPlusType = null, bool showExpiryEntitlements = false)
        {
            log.LogMethodEntry(accountId, accountNumber, fromDate, toDate, creditPlusType, showExpiryEntitlements);
            int result = await Get<int>(ACCOUNT_CREDIT_PLUS_SUMMARY_COUNT, new WebApiGetRequestParameterCollection("accountId", accountId,
                                                                                                                    "accountNumber", accountNumber,
                                                                                                                    "fromDate", fromDate,
                                                                                                                    "toDate", toDate,
                                                                                                                    "creditPlusType", creditPlusType,
                                                                                                                    "showExpiryEntitlements", showExpiryEntitlements));

            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the List of AccountGameSummaryDTO based on the search parameters.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="accountNumber"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<List<AccountGameSummaryDTO>> GetAccountGameSummaryDTOList(int accountId = -1, string accountNumber = null, DateTime? fromDate = null, DateTime? toDate = null, bool showExpiryEntitlements = false, int currentPage = 0,
                                                                        int pageSize = 50)
        {
            log.LogMethodEntry(accountId, accountNumber, fromDate, toDate, showExpiryEntitlements);
            List<AccountGameSummaryDTO> result = await Get<List<AccountGameSummaryDTO>>(ACCOUNT_GAME_SUMMARY, new WebApiGetRequestParameterCollection("accountId", accountId,
                                                                                                                                                                       "accountNumber", accountNumber,
                                                                                                                                                                       "fromDate", fromDate,
                                                                                                                                                                       "toDate", toDate,
                                                                                                                                                                       "showExpiryEntitlements", showExpiryEntitlements,
                                                                                                                                                                       "pageNumber", currentPage,
                                                                                                                                                                       "pageSize", pageSize));
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Returns the count of AccountGameSummaryDTO based on the search parameters.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="accountNumber"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public async Task<int> GetAccountGameSummaryDTOListCount(int accountId = -1, string accountNumber = null, DateTime? fromDate = null, DateTime? toDate = null, bool showExpiryEntitlements = false)
        {
            log.LogMethodEntry(accountId, accountNumber, fromDate, toDate, showExpiryEntitlements);
            int result = await Get<int>(ACCOUNT_GAME_SUMMARY_COUNT, new WebApiGetRequestParameterCollection("accountId", accountId,
                                                                                                                    "accountNumber", accountNumber,
                                                                                                                    "fromDate", fromDate,
                                                                                                                    "toDate", toDate,
                                                                                                                    "showExpiryEntitlements", showExpiryEntitlements));

            log.LogMethodExit(result);
            return result;
        }
    }
}
