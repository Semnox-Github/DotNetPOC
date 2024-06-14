/********************************************************************************************
 * Project Name - Accounts
 * Description  - IAccountUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         13-Nov-2020       Mushahid Faizan         Created : POS UI Redesign with REST API
 2.140.0     23-June-2021       Prashanth               Modidied : Added GetAccountRelationships, CreateAccountRelationships, UpdateAccountRelationships
 2.130.8     10-Jun-2022       Nitin Pai                Removed the site id from search params as relations created in site were not visible
 2.130.8     10-Jun-2022       Nitin Pai               Added new method to save account nick name - account id
 2.130.11    14-Oct-2022     Yashodhara C H            Added new method to get account, games, discounts and activity details 
 ********************************************************************************************/
using System;
using Semnox.Parafait.Game;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer.Accounts
{
    public interface IAccountUseCases
    {
        Task<AccountDTOCollection> GetAccounts(string isActive = "Y", bool chkVipCustomer = false, bool chkRoamingCards = true, bool chkTechCards = false, DateTime? fromDate = null, DateTime? toDate = null,
                                            int accountId = -1, int customerId = -1, string accountNumber = null, int? tagSiteId = null, int pageNumber = 0, int pageSize = 0, bool buildChildRecords = false, bool activeRecordsOnly = false,
                                            bool buildActivityHistory = false, bool buildGamePlayHistory = false, int lastActivityHistoryId = -1, int lastGamePlayHistoryId = -1, string entitlementType = null,
                                            int activityHistoryPageNumber = 0, int gamePlayHistoryPageNumber = 0, bool buildBarCode = false, bool includeFutureEntitlements = false, bool closedTransactionsOnly = true,
                                            DateTime? activityFromDate = null, DateTime? activityToDate = null, string customerName = null);
        Task<List<AccountDTO>> SaveAccounts(List<AccountDTO> accountDTOList);
        Task<List<AccountRelationshipDTO>> GetAccountRelationships(List<KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>> searchParameters);
        Task<List<LinkNewCardDTO>> CreateAccountRelationships(List<LinkNewCardDTO> newAccountRelationshipDTOs);
        Task<List<AccountRelationshipDTO>> UpdateAccountRelationships(List<AccountRelationshipDTO> accountRelationshipDTOs);
        Task<AccountDTO> SaveAccountIdentifier(int accountId, AccountDTO accountDTO);

        Task<List<AccountSummaryViewDTO>> GetAccountSummaryViewDTOList(string isActive = "Y", string accountIdList = null, string accountNumberList = null, int accountId = -1, string accountNumber = null, bool validFlag = false, int customerId = -1,
                                                           int? cardTypeId = -1, int uploadSiteId = -1, int masterEntityId = -1,
                                                           string cardIdentifier = null, int membershipId = -1, string membershipName = null, DateTime? entitlementFromDate = null, DateTime? entitlementToDate = null, bool includeFutureEntitlements = false, bool showExpiryEntitlements = false);
        Task <List<AccountGamesSummaryViewDTO>> GetAccountGamesSummaryViewDTOList(int accountId);

        Task <List<AccountDiscountsSummaryViewDTO>> GetAccountDiscountsSummaryViewDTOList(int accountId);

        Task<List<AccountActivityDTO>> GetAccountActivityDTOList(int accountId, String startDate = "", String endDate = "", int numberOfDays = 180, int pageNumber = 0, bool addSummaryRow = false, int lastRowNumberId = -1);

        Task<List<GamePlayDTO>> GetAccountGamePlaysDTOList(int accountId, String startDate = "", String endDate = "", int numberOfDays = 180, int pageNumber = 0, bool addSummaryRow = false, bool addDetails = false);

        Task<List<AccountCreditPlusSummaryDTO>> GetAccountCreditPlusSummaryDTOList(int accountId = -1, string accountNumber = null, DateTime? fromDate = null, DateTime? toDate = null, string creditPlusType = null, bool showExpiryEntitlements = false, int currentPage = 0,
                                                                        int pageSize = 50);

        Task<int> GetAccountCreditPlusSummaryDTOListCount(int accountId = -1, string accountNumber = null, DateTime? fromDate = null, DateTime? toDate = null, string creditPlusType = null, bool showExpiryEntitlements = false);

        Task<List<AccountGameSummaryDTO>> GetAccountGameSummaryDTOList(int accountId = -1, string accountNumber = null, DateTime? fromDate = null, DateTime? toDate = null, bool showExpiryEntitlements = false, int currentPage = 0,
                                                                        int pageSize = 50);

        Task<int> GetAccountGameSummaryDTOListCount(int accountId = -1, string accountNumber = null, DateTime? fromDate = null, DateTime? toDate = null, bool showExpiryEntitlements = false);
    }

}
