/********************************************************************************************
 * Project Name - Accounts
 * Description  - LocalAccountUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         13-Nov-2020       Mushahid Faizan         Created : POS UI Redesign with REST API
 2.140.0     23-June-2021       Prashanth               Modidied : Added GetAccountRelationships, CreateAccountRelationships, UpdateAccountRelationships
 2.120.7     10-May-2022       Nitin Pai               Modified Fix: IsActive is not handling inputs coming as "True" or "False"
 2.130.8     10-Jun-2022       Nitin Pai               Added new method to save account nick name - account id
 2.130.11    14-Oct-2022     Yashodhara C H            Added new method to get account, games, discounts and activity details 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GenCode128;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Game;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Customer.Accounts
{
    public class LocalAccountUseCases : LocalUseCases, IAccountUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public LocalAccountUseCases(ExecutionContext executionContext) : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public async Task<AccountDTOCollection> GetAccounts(string isActive = "Y", bool chkVipCustomer = false, bool chkRoamingCards = true, bool chkTechCards = false, DateTime? fromDate = null, DateTime? toDate = null,
                                            int accountId = -1, int customerId = -1, string accountNumber = null, int? tagSiteId = null, int pageNumber = 0, int pageSize = 0, bool buildChildRecords = false, bool activeRecordsOnly = false,
                                            bool buildActivityHistory = false, bool buildGamePlayHistory = false, int lastActivityHistoryId = -1, int lastGamePlayHistoryId = -1, string entitlementType = null,
                                            int activityHistoryPageNumber = 0, int gamePlayHistoryPageNumber = 0, bool buildBarCode = false, bool includeFutureEntitlements = false, bool closedTransactionsOnly = true,
                                            DateTime? activityFromDate = null, DateTime? activityToDate = null, string customerName = null)
        {

            return await Task<AccountDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(isActive, chkVipCustomer, chkRoamingCards, chkTechCards, fromDate, toDate,
                                    accountId, customerId, accountNumber, tagSiteId, pageNumber, pageSize, buildChildRecords, activeRecordsOnly,
                                    buildActivityHistory, buildGamePlayHistory, lastActivityHistoryId, lastGamePlayHistoryId, entitlementType,
                                    activityHistoryPageNumber, gamePlayHistoryPageNumber, buildBarCode, includeFutureEntitlements, closedTransactionsOnly,
                                    activityFromDate, activityToDate, customerName);

                if(string.IsNullOrWhiteSpace(accountNumber) == false &&  tagSiteId.HasValue)
                {
                    AccountBL accountBL = new AccountBL(executionContext, accountNumber, tagSiteId.Value);
                    AccountDTOCollection result = new AccountDTOCollection(new List<AccountDTO>(){ accountBL.AccountDTO}, 1, 1, string.Empty, executionContext.Token);
                    log.LogMethodExit(result, "string.IsNullOrWhiteSpace(accountNumber) == false &&  tagSiteId.HasValue");
                    return result;
                }

                int totalNoOfPages = 0;
                List<KeyValuePair<AccountDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AccountDTO.SearchByParameters, string>>();
                DateTime startDate = DateTime.Now;
                DateTime endDate = DateTime.Now.AddDays(1);
                string accountBarCode = string.Empty;

                if (fromDate != null)
                {
                    startDate = Convert.ToDateTime(fromDate.ToString());
                    if (startDate == DateTime.MinValue)
                    {
                        string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                        log.LogMethodExit("Throwing Exception - " + customException);
                        throw new Exception(customException);
                    }
                }

                if (toDate != null)
                {
                    endDate = Convert.ToDateTime(toDate.ToString());
                    if (endDate == DateTime.MinValue)
                    {
                        string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                        log.LogMethodExit("Throwing Exception - " + customException);
                        throw new Exception(customException);
                    }
                }
                else
                {
                    endDate = ServerDateTime.Now;
                }

                if (!String.IsNullOrEmpty(isActive) && (isActive.Equals("1", StringComparison.InvariantCultureIgnoreCase)
                                                        || isActive.Equals("Y", StringComparison.InvariantCultureIgnoreCase)
                                                        || isActive.Equals("True", StringComparison.InvariantCultureIgnoreCase)))
                {
                    isActive = "Y";
                    activeRecordsOnly = true;
                    searchParameters.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.VALID_FLAG, isActive.ToString()));
                }

                if (!String.IsNullOrEmpty(isActive) && (isActive.Equals("0", StringComparison.InvariantCultureIgnoreCase)
                                                        || isActive.Equals("N", StringComparison.InvariantCultureIgnoreCase)
                                                        || isActive.Equals("False", StringComparison.InvariantCultureIgnoreCase)))
                {
                    isActive = "N";
                    activeRecordsOnly = false;
                    searchParameters.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.VALID_FLAG, isActive.ToString()));
                }

                if (chkVipCustomer)
                {
                    searchParameters.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.VIP_CUSTOMER, "Y"));
                }
                if (!chkRoamingCards)
                {
                    searchParameters.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.SITE_ID, executionContext.SiteId.ToString()));
                }
                if (chkTechCards)
                {
                    searchParameters.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.TECHNICIAN_CARD, chkTechCards ? "Y" : "N"));
                }
                if (accountId > -1)
                {
                    searchParameters.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.ACCOUNT_ID, accountId.ToString()));
                }
                if (customerId > -1)
                {
                    searchParameters.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.CUSTOMER_ID, customerId.ToString()));
                }
                if (!string.IsNullOrEmpty(accountNumber))
                {
                    searchParameters.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.TAG_NUMBER, accountNumber));
                }
                if (!string.IsNullOrEmpty(customerName))
                {
                    searchParameters.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.CUSTOMER_NAME, customerName));
                }
                if (!chkTechCards && string.IsNullOrWhiteSpace(accountNumber))
                {
                    if (fromDate != null &&  toDate != null)
                    {
                        searchParameters.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.ISSUE_DATE_FROM, startDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        searchParameters.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.ISSUE_DATE_TO, endDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    }
                }

                DateTime activityStartDate = DateTime.Now;
                DateTime activityEndDate = DateTime.Now.AddDays(1);

                if (activityFromDate != null)
                {
                    //DateTime.TryParseExact(fromDate, utilities.getParafaitDefaults("DATE_FORMAT"), CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate);
                    activityStartDate = Convert.ToDateTime(activityFromDate.ToString());
                    if (activityStartDate == DateTime.MinValue)
                    {
                       string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                        log.LogMethodExit("Throwing Exception - " + customException);
                        throw new Exception(customException);
                    }
                }

                if (activityToDate != null)
                {
                    //DateTime.TryParseExact(toDate, utilities.getParafaitDefaults("DATE_FORMAT"), CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate);
                    activityEndDate = Convert.ToDateTime(activityToDate.ToString());
                    if (activityEndDate == DateTime.MinValue)
                    {
                       string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                        log.LogMethodExit("Throwing Exception - " + customException);
                        throw new Exception(customException);
                    }
                }
                else
                {
                    activityEndDate = ServerDateTime.Now;
                }

                List<KeyValuePair<AccountActivityDTO.SearchByParameters, string>> activitySearchParameters = null;
                List<KeyValuePair<GamePlayDTO.SearchByParameters, string>> gameSearchParameters = null;
                if (activityFromDate != null)
                {
                    if(buildGamePlayHistory)
                    {
                        gameSearchParameters = new List<KeyValuePair<GamePlayDTO.SearchByParameters, string>>();
                        gameSearchParameters.Add(new KeyValuePair<GamePlayDTO.SearchByParameters, string>(GamePlayDTO.SearchByParameters.FROM_DATE, activityStartDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        gameSearchParameters.Add(new KeyValuePair<GamePlayDTO.SearchByParameters, string>(GamePlayDTO.SearchByParameters.TO_DATE, activityEndDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    }

                    if (buildActivityHistory)
                    {
                        activitySearchParameters = new List<KeyValuePair<AccountActivityDTO.SearchByParameters, string>>();
                        activitySearchParameters.Add(new KeyValuePair<AccountActivityDTO.SearchByParameters, string>(AccountActivityDTO.SearchByParameters.FROM_DATE, activityStartDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        activitySearchParameters.Add(new KeyValuePair<AccountActivityDTO.SearchByParameters, string>(AccountActivityDTO.SearchByParameters.TO_DATE, activityEndDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    }
                }
                if (closedTransactionsOnly)
                {
                    if(activitySearchParameters == null)
                        activitySearchParameters = new List<KeyValuePair<AccountActivityDTO.SearchByParameters, string>>();

                    activitySearchParameters.Add(new KeyValuePair<AccountActivityDTO.SearchByParameters, string>(AccountActivityDTO.SearchByParameters.TRANSACTION_STATUS, "CLOSED"));
                }

                AccountListBL accountListBL = new AccountListBL(executionContext);
                int totalNoOfAccounts = accountListBL.GetAccountCount(searchParameters, null);
                log.Debug("Number of account:" + totalNoOfAccounts);

                List<AccountDTO> accountDTOList = null;
                if (totalNoOfAccounts > 0)
                {
                    log.LogVariableState("totalNoOfCustomer", totalNoOfAccounts);

                    pageSize = pageSize > 500 || pageSize == 0 ? 500 : pageSize;
                    totalNoOfPages = (totalNoOfAccounts / pageSize) + ((totalNoOfAccounts % pageSize) > 0 ? 1 : 0);
                    pageNumber = pageNumber < -1 || pageNumber > totalNoOfPages ? 0 : pageNumber;

                    log.Debug("Number of pages:" + totalNoOfPages);

                    accountDTOList = accountListBL.GetAccountDTOList(searchParameters, pageNumber, pageSize, buildChildRecords, activeRecordsOnly, null,
                                buildActivityHistory, buildGamePlayHistory, lastActivityHistoryId, lastGamePlayHistoryId, activityHistoryPageNumber, gamePlayHistoryPageNumber,
                                includeFutureEntitlements, activitySearchParameters, gameSearchParameters);

                    if (!String.IsNullOrEmpty(entitlementType))
                    {
                        CreditPlusType entitlement = CreditPlusTypeConverter.FromString(entitlementType);
                        accountDTOList = accountDTOList.Where(x => x.AccountCreditPlusDTOList.Any(y => y.CreditPlusType == entitlement)).ToList();
                    }

                    if ((!String.IsNullOrEmpty(accountNumber) || accountId > -1) && buildBarCode)
                    {
                        Image image = Code128Rendering.MakeBarcodeImage(accountDTOList[0].TagNumber, 1, true);

                        if (image != null)
                        {
                            using (var stream = new MemoryStream())
                            {
                                image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                                accountBarCode = System.Convert.ToBase64String(stream.ToArray());
                            }
                        }
                    }
                }
                log.LogMethodExit(accountDTOList);
                AccountDTOCollection accountDTOCollection = new AccountDTOCollection(accountDTOList, pageNumber, totalNoOfAccounts, accountBarCode, executionContext.WebApiToken);
                return accountDTOCollection;
                
            });
        }

        public async Task<List<AccountDTO>> SaveAccounts(List<AccountDTO> accountDTOList)
        {
            return await Task<List<AccountDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(accountDTOList);
                if (accountDTOList == null)
                {
                    string errorMessage = "accountDTOList is empty";
                    log.LogMethodExit("Throwing Exception- " + errorMessage);
                    throw new ValidationException(errorMessage);
                }
                List<AccountDTO> result;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    AccountListBL accountListBL = new AccountListBL(executionContext, accountDTOList);
                    result = accountListBL.SaveUpdateManualChangesList(parafaitDBTrx.SQLTrx);
                }
                return result;
            });
        }

        public async Task<List<LinkNewCardDTO>> CreateAccountRelationships(List<LinkNewCardDTO> newAccountRelationshipDTOs)
        {
            return await Task<List<LinkNewCardDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(newAccountRelationshipDTOs);
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        List<AccountRelationshipDTO> accountRelationshipDTOs = new List<AccountRelationshipDTO>();
                        foreach (LinkNewCardDTO newAccountRelationshipDTO in newAccountRelationshipDTOs)
                        {
                            if (newAccountRelationshipDTO.AccountRelationShipDTO.RelatedAccountId > -1)
                            {
                                accountRelationshipDTOs.Add(newAccountRelationshipDTO.AccountRelationShipDTO);
                            }
                            else
                            {
                                AccountDTO accountDTO = new AccountDTO(-1, newAccountRelationshipDTO.CardNumber, string.Empty, DateTime.Now, null, false, null, null, true, null, string.Empty,
                                0, 0, 0, 0,-1, 0, true, false, false, null, null,"N", null, false, 0, -1, null, null, -1, null, string.Empty, false, -1, string.Empty);
                                AccountBL accountBL = new AccountBL(executionContext, accountDTO);
                                accountBL.Save(parafaitDBTrx.SQLTrx);
                                newAccountRelationshipDTO.AccountRelationShipDTO.RelatedAccountId = accountBL.AccountDTO.AccountId;
                                accountRelationshipDTOs.Add(newAccountRelationshipDTO.AccountRelationShipDTO);
                            }
                        }
                        AccountRelationshipListBL accountRelationshipListBL = new AccountRelationshipListBL(accountRelationshipDTOs, executionContext);
                        accountRelationshipListBL.Save(parafaitDBTrx.SQLTrx);
                        parafaitDBTrx.EndTransaction();
                    }
                    catch (ValidationException valEx)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(valEx);
                        throw;
                    }
                    catch (Exception ex)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(ex);
                        log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                        throw;
                    }
                }
                log.LogMethodExit(newAccountRelationshipDTOs);
                return newAccountRelationshipDTOs;
            }
            );
        }

        

        public async Task<List<AccountRelationshipDTO>> GetAccountRelationships(List<KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>> searchParameters)
        {
            return await Task<List<AccountRelationshipDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);
                AccountRelationshipListBL accountRelationshipListBL = new AccountRelationshipListBL(executionContext);
                using (ParafaitDBTransaction parafaitDBtrax = new ParafaitDBTransaction())
                {
                    List<AccountRelationshipDTO> result = null;
                    try
                    {
                        result = accountRelationshipListBL.GetAccountRelationshipDTOList(searchParameters, parafaitDBtrax.SQLTrx);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        throw ex;
                    }
                    log.LogMethodExit(result);
                    return result;
                }
            });
        }

        public async Task<List<AccountRelationshipDTO>> UpdateAccountRelationships(List<AccountRelationshipDTO> accountRelationshipDTOs)
        {
            return await Task<List<AccountRelationshipDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(accountRelationshipDTOs);
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        AccountRelationshipListBL accountRelationshipListBL = new AccountRelationshipListBL(accountRelationshipDTOs, executionContext);
                        accountRelationshipListBL.Save(parafaitDBTrx.SQLTrx);
                        parafaitDBTrx.EndTransaction();
                    }
                    catch (ValidationException valEx)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(valEx);
                        throw;
                    }
                    catch (Exception ex)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(ex);
                        log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                        throw;
                    }
                }
                log.LogMethodExit(accountRelationshipDTOs);
                return accountRelationshipDTOs;
            });
        }

        public async Task<AccountDTO> SaveAccountIdentifier(int accountId, AccountDTO accountDTO)
        {
            log.LogMethodEntry(accountId, accountDTO);
            return await Task<AccountDTO>.Factory.StartNew(() =>
            {
                if (accountId == -1 || accountDTO == null || string.IsNullOrWhiteSpace(accountDTO.AccountIdentifier))
                {
                    string errorMessage = "invalid inputs - accountId or accountIdentifier is empty";
                    log.LogMethodExit("Throwing Exception- " + errorMessage);
                    throw new ValidationException(errorMessage);
                }
                AccountDTO result;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    AccountBL accountBL = new AccountBL(executionContext, accountId);
                    if (accountBL.AccountDTO == null || accountBL.AccountDTO.AccountId == -1)
                    {
                        string errorMessage = "invalid accountId " + accountId;
                        log.LogMethodExit("Throwing Exception- " + errorMessage);
                        throw new ValidationException(errorMessage);
                    }

                    accountBL.AccountDTO.AccountIdentifier = accountDTO.AccountIdentifier;
                    accountBL.SaveManualChanges(true, parafaitDBTrx.SQLTrx);
                    result = accountBL.AccountDTO;
                }
                return result;
            });
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
            return await Task<List<AccountSummaryViewDTO>>.Factory.StartNew(() =>
            {
            log.LogMethodEntry(isActive, accountId, accountIdList, accountNumberList, accountNumber, validFlag, customerId, cardTypeId, uploadSiteId, masterEntityId, cardIdentifier,
                            membershipId, membershipName, entitlementFromDate, entitlementToDate, includeFutureEntitlements, showExpiryEntitlements);
                AccountSummaryViewListBL accountSummaryViewListBL = new AccountSummaryViewListBL(executionContext);
                string includeFutureRecord = string.Empty;
                string showExpiryRecord = string.Empty;
                List<KeyValuePair<AccountSummaryViewDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AccountSummaryViewDTO.SearchByParameters, string>>();
                if (!String.IsNullOrEmpty(isActive) && (isActive.Equals("1", StringComparison.InvariantCultureIgnoreCase)
                                        || isActive.Equals("Y", StringComparison.InvariantCultureIgnoreCase)
                                        || isActive.Equals("True", StringComparison.InvariantCultureIgnoreCase)))
                {
                    isActive = "Y";
                    searchParameters.Add(new KeyValuePair<AccountSummaryViewDTO.SearchByParameters, string>(AccountSummaryViewDTO.SearchByParameters.VALID_FLAG, isActive.ToString()));
                }

                if (!String.IsNullOrEmpty(isActive) && (isActive.Equals("0", StringComparison.InvariantCultureIgnoreCase)
                                                        || isActive.Equals("N", StringComparison.InvariantCultureIgnoreCase)
                                                        || isActive.Equals("False", StringComparison.InvariantCultureIgnoreCase)))
                {
                    isActive = "N";
                    searchParameters.Add(new KeyValuePair<AccountSummaryViewDTO.SearchByParameters, string>(AccountSummaryViewDTO.SearchByParameters.VALID_FLAG, isActive.ToString()));
                }
                if (string.IsNullOrWhiteSpace(accountIdList) == false)
                {
                    searchParameters.Add(new KeyValuePair<AccountSummaryViewDTO.SearchByParameters, string>(AccountSummaryViewDTO.SearchByParameters.ACCOUNT_ID_LIST, accountIdList.ToString()));
                }
                if (string.IsNullOrWhiteSpace(accountNumberList) == false)
                {
                    searchParameters.Add(new KeyValuePair<AccountSummaryViewDTO.SearchByParameters, string>(AccountSummaryViewDTO.SearchByParameters.ACCOUNT_NUMBER_LIST, accountNumberList.ToString()));
                }
                if (accountId > -1)
                {
                    searchParameters.Add(new KeyValuePair<AccountSummaryViewDTO.SearchByParameters, string>(AccountSummaryViewDTO.SearchByParameters.ACCOUNT_ID, accountId.ToString()));
                }
                if (string.IsNullOrWhiteSpace(accountNumber) == false)
                {
                    searchParameters.Add(new KeyValuePair<AccountSummaryViewDTO.SearchByParameters, string>(AccountSummaryViewDTO.SearchByParameters.ACCOUNT_NUMBER, accountNumber));
                }
                if (customerId > -1)
                {
                    searchParameters.Add(new KeyValuePair<AccountSummaryViewDTO.SearchByParameters, string>(AccountSummaryViewDTO.SearchByParameters.CUSTOMER_ID, customerId.ToString()));
                }
                if (cardTypeId > -1)
                {
                    searchParameters.Add(new KeyValuePair<AccountSummaryViewDTO.SearchByParameters, string>(AccountSummaryViewDTO.SearchByParameters.CARD_TYPE_ID, cardTypeId.ToString()));
                }
                if (uploadSiteId > -1)
                {
                    searchParameters.Add(new KeyValuePair<AccountSummaryViewDTO.SearchByParameters, string>(AccountSummaryViewDTO.SearchByParameters.UPLOAD_SITE_ID, uploadSiteId.ToString()));
                }
                if (masterEntityId > -1)
                {
                    searchParameters.Add(new KeyValuePair<AccountSummaryViewDTO.SearchByParameters, string>(AccountSummaryViewDTO.SearchByParameters.MASTER_ENTITY_ID, masterEntityId.ToString()));
                }
                if (string.IsNullOrWhiteSpace(cardIdentifier) == false)
                {
                    searchParameters.Add(new KeyValuePair<AccountSummaryViewDTO.SearchByParameters, string>(AccountSummaryViewDTO.SearchByParameters.CARD_IDENTIFIER, cardIdentifier));
                }
                if (membershipId > -1)
                {
                    searchParameters.Add(new KeyValuePair<AccountSummaryViewDTO.SearchByParameters, string>(AccountSummaryViewDTO.SearchByParameters.MEMBERSHIP_ID, membershipId.ToString()));
                }
                if (string.IsNullOrWhiteSpace(membershipName) == false)
                {
                    searchParameters.Add(new KeyValuePair<AccountSummaryViewDTO.SearchByParameters, string>(AccountSummaryViewDTO.SearchByParameters.MEMBERSHIP_NAME, membershipName));
                }

                if(entitlementToDate < entitlementFromDate)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 2642);
                    log.Error("Throwing Exception- " + errorMessage);
                    throw new ValidationException(errorMessage);
                }

                if(entitlementFromDate.HasValue)
                {
                    if (entitlementFromDate.Value == DateTime.MinValue)
                    {
                        string customException = MessageContainerList.GetMessage(executionContext, 15);
                        log.Error(customException);
                        throw new ValidationException(customException);
                    }
                    DateTime fromDate = new BusinessDate(executionContext.GetSiteId(), entitlementFromDate.Value).Start;
                    searchParameters.Add(new KeyValuePair<AccountSummaryViewDTO.SearchByParameters, string>(AccountSummaryViewDTO.SearchByParameters.ENTITLEMENT_FROMDATE, fromDate.ToString("yyyy-MM-dd HH:mm:ss.fff")));
                }

                if (entitlementToDate.HasValue)
                {
                    if (entitlementToDate.Value == DateTime.MinValue)
                    {
                        string customException = MessageContainerList.GetMessage(executionContext, 15);
                        log.Error(customException);
                        throw new ValidationException(customException);
                    }
                    DateTime toDate = new BusinessDate(executionContext.GetSiteId(), entitlementToDate.Value).End;
                    searchParameters.Add(new KeyValuePair<AccountSummaryViewDTO.SearchByParameters, string>(AccountSummaryViewDTO.SearchByParameters.ENTITLEMENT_TODATE, toDate.ToString("yyyy-MM-dd HH:mm:ss.fff")));
                }

                if (includeFutureEntitlements == true)
                {
                    includeFutureRecord = "1";
                    searchParameters.Add(new KeyValuePair<AccountSummaryViewDTO.SearchByParameters, string>(AccountSummaryViewDTO.SearchByParameters.INCLUDE_FUTURE_ENTITLEMENTS, includeFutureRecord));
                }
                else
                {
                    includeFutureRecord = "0";
                    searchParameters.Add(new KeyValuePair<AccountSummaryViewDTO.SearchByParameters, string>(AccountSummaryViewDTO.SearchByParameters.INCLUDE_FUTURE_ENTITLEMENTS, includeFutureRecord));
                }

                if (showExpiryEntitlements == true)
                {
                    if (entitlementToDate.HasValue == false)
                    {
                        string customException = MessageContainerList.GetMessage(executionContext, 597);
                        log.Error(customException);
                        throw new ValidationException(customException);
                    }
                    if (entitlementFromDate.HasValue == false)
                    {
                        DateTime fromDate = new BusinessDate(executionContext.GetSiteId(), entitlementToDate.Value).Start;
                        searchParameters.Add(new KeyValuePair<AccountSummaryViewDTO.SearchByParameters, string>(AccountSummaryViewDTO.SearchByParameters.ENTITLEMENT_FROMDATE, fromDate.ToString("yyyy-MM-dd HH:mm:ss.fff")));
                    }
                    showExpiryRecord = "1";
                    searchParameters.Add(new KeyValuePair<AccountSummaryViewDTO.SearchByParameters, string>(AccountSummaryViewDTO.SearchByParameters.SHOW_EXPIRY_ENTITLEMENTS, showExpiryRecord));
                }
                else
                {
                    showExpiryRecord = "0";
                    searchParameters.Add(new KeyValuePair<AccountSummaryViewDTO.SearchByParameters, string>(AccountSummaryViewDTO.SearchByParameters.SHOW_EXPIRY_ENTITLEMENTS, showExpiryRecord));
                }

                List<AccountSummaryViewDTO> result = accountSummaryViewListBL.GetAccountSummaryViewDTOList(searchParameters);
                log.LogMethodExit(result);
                return result;
            });
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
            return await Task<List<AccountGamesSummaryViewDTO>>.Factory.StartNew(() =>
            {
                AccountGamesSummaryViewListBL accountGamesSummaryViewListBL = new AccountGamesSummaryViewListBL(executionContext);
                List<KeyValuePair<AccountGamesSummaryViewDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AccountGamesSummaryViewDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<AccountGamesSummaryViewDTO.SearchByParameters, string>(AccountGamesSummaryViewDTO.SearchByParameters.ACCOUNT_ID, accountId.ToString()));
                List<AccountGamesSummaryViewDTO> accountGamesSummaryViewDTOs = accountGamesSummaryViewListBL.GetAccountGamesSummaryViewDTOList(searchParameters);
                log.LogMethodExit(accountGamesSummaryViewDTOs);
                return accountGamesSummaryViewDTOs;
            });
        }

        /// <summary>
        /// Returns the List ofAccountDiscountsSummaryViewDTO based on the search parameters.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idList"></param>
        /// <returns></returns>
        public async Task<List<AccountDiscountsSummaryViewDTO>> GetAccountDiscountsSummaryViewDTOList(int accountId)
        {
            log.LogMethodEntry(accountId);
            return await Task<List<AccountDiscountsSummaryViewDTO>>.Factory.StartNew(() =>
            {
                AccountDiscountsSummaryViewListBL accountDiscountsSummaryViewListBL = new AccountDiscountsSummaryViewListBL(executionContext);
                List<KeyValuePair<AccountDiscountsSummaryViewDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AccountDiscountsSummaryViewDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<AccountDiscountsSummaryViewDTO.SearchByParameters, string>(AccountDiscountsSummaryViewDTO.SearchByParameters.ACCOUNT_ID, accountId.ToString()));

                List<AccountDiscountsSummaryViewDTO> accountDiscountsSummaryViewDTOs = accountDiscountsSummaryViewListBL.GetAccountDiscountsSummaryViewDTOList(searchParameters);
                log.LogMethodExit(accountDiscountsSummaryViewDTOs);
                return accountDiscountsSummaryViewDTOs;
            });
        }

        public async Task<List<AccountActivityDTO>> GetAccountActivityDTOList(int accountId, String startDate = "", String endDate = "", int numberOfDays = 180, int pageNumber = 0, bool addSummaryRow = false, int lastRowNumberId = -1)
        {
            log.LogMethodEntry(accountId, startDate, endDate);
            return await Task< List<AccountActivityDTO>>.Factory.StartNew(() =>
            {
                AccountActivityViewListBL accountActivityViewListBL = new AccountActivityViewListBL(executionContext);
                List<KeyValuePair<AccountActivityDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AccountActivityDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<AccountActivityDTO.SearchByParameters, string>(AccountActivityDTO.SearchByParameters.ACCOUNT_ID, accountId.ToString()));

                DateTime toDate = DateTime.Now;
                DateTime fromDate = toDate.AddDays(-180);

                if (!String.IsNullOrWhiteSpace(startDate) && !String.IsNullOrWhiteSpace(endDate))
                {
                    if (!DateTime.TryParse(startDate, out fromDate))
                    {
                        string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                        log.LogMethodExit("Throwing Exception - " + customException);
                        throw new ValidationException(customException);
                    }

                    if (!DateTime.TryParse(endDate, out toDate))
                    {
                        string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                        log.LogMethodExit("Throwing Exception - " + customException);
                        throw new ValidationException(customException);
                    }

                    if (toDate < fromDate || Math.Abs((fromDate - toDate).Days) > 180)
                    {
                        string customException = "Invalid date range.";
                        log.LogMethodExit("Throwing Exception - " + customException);
                        throw new ValidationException(customException);
                    }
                }
                else if (numberOfDays > 0)
                {
                    if (numberOfDays > 180)
                    {
                        string customException = "Number of days is greater than 180.";
                        log.LogMethodExit("Throwing Exception - " + customException);
                        throw new ValidationException(customException);
                    }
                    int days = numberOfDays > 0 && numberOfDays <= 180 ? numberOfDays : 180;
                    fromDate = toDate.AddDays(-days);
                }

                log.LogVariableState("from date", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                log.LogVariableState("to date", toDate.ToString("yyyy-MM-dd HH:mm:ss"));

                searchParameters.Add(new KeyValuePair<AccountActivityDTO.SearchByParameters, string>(AccountActivityDTO.SearchByParameters.FROM_DATE, fromDate.ToString("yyyy-MM-dd HH:mm:ss")));
                searchParameters.Add(new KeyValuePair<AccountActivityDTO.SearchByParameters, string>(AccountActivityDTO.SearchByParameters.TO_DATE, toDate.ToString("yyyy-MM-dd HH:mm:ss")));
                List<AccountActivityDTO> accountActivitySummaryViewDTOs = accountActivityViewListBL.GetAccountActivityDTOList(searchParameters, addSummaryRow, null, -1, pageNumber, lastRowNumberId);
                log.LogMethodExit(accountActivitySummaryViewDTOs);
                return accountActivitySummaryViewDTOs;
            });
        }

        public async Task<List<GamePlayDTO>> GetAccountGamePlaysDTOList(int accountId, String startDate = "", String endDate = "", int numberOfDays = 180, int pageNumber = 0, bool addSummaryRow = false, bool addDetails = false)
        {
            log.LogMethodEntry(accountId, startDate, endDate);
            return await Task<List<GamePlayDTO>>.Factory.StartNew(() =>
            {
                DateTime toDate = DateTime.Now;
                DateTime fromDate = toDate.AddDays(-180);

                List<KeyValuePair<GamePlayDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<GamePlayDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<GamePlayDTO.SearchByParameters, string>(GamePlayDTO.SearchByParameters.CARD_ID, accountId.ToString()));
                if (!String.IsNullOrWhiteSpace(startDate) && !String.IsNullOrWhiteSpace(endDate))
                {
                    if (!DateTime.TryParse(startDate, out fromDate))
                    {
                        string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                        log.LogMethodExit("Throwing Exception - " + customException);
                        throw new ValidationException(customException);
                    }

                    if (!DateTime.TryParse(endDate, out toDate))
                    {
                        string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                        log.LogMethodExit("Throwing Exception - " + customException);
                        throw new ValidationException(customException);
                    }

                    if (toDate < fromDate || Math.Abs((fromDate - toDate).Days) > 180)
                    {
                        string customException = "Invalid date range.";
                        log.LogMethodExit("Throwing Exception - " + customException);
                        throw new ValidationException(customException);
                    }
                }
                else if(numberOfDays > 0)
                {
                    if (numberOfDays > 180)
                    {
                        string customException = "Number of days is greater than 180.";
                        log.LogMethodExit("Throwing Exception - " + customException);
                        throw new ValidationException(customException);
                    }
                    int days = numberOfDays > 0 && numberOfDays <= 180 ? numberOfDays : 180;
                    fromDate = toDate.AddDays(-days);
                }

                log.LogVariableState("from date", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                log.LogVariableState("to date", toDate.ToString("yyyy-MM-dd HH:mm:ss"));

                searchParameters.Add(new KeyValuePair<GamePlayDTO.SearchByParameters, string>(GamePlayDTO.SearchByParameters.FROM_DATE, fromDate.ToString("yyyy-MM-dd HH:mm:ss")));
                searchParameters.Add(new KeyValuePair<GamePlayDTO.SearchByParameters, string>(GamePlayDTO.SearchByParameters.TO_DATE, toDate.ToString("yyyy-MM-dd HH:mm:ss")));

                GamePlaySummaryListBL gamePlaySummaryListBL = new GamePlaySummaryListBL(executionContext);
                List<GamePlayDTO> gamePlayDTOList = gamePlaySummaryListBL.GetGamePlayDTOList(searchParameters, addSummaryRow, addDetails, null, -1, pageNumber);
                if (gamePlayDTOList != null && gamePlayDTOList.Count > 0)
                {
                    if (addSummaryRow)
                    {
                        GamePlayDTO summaryDTO = gamePlayDTOList[0];
                        gamePlayDTOList.RemoveAt(0);
                        gamePlayDTOList = gamePlayDTOList.OrderByDescending(x => x.PlayDate).ThenBy(x => x.Site).ToList();
                        gamePlayDTOList.Insert(0, summaryDTO);
                    }
                    else
                    {
                        gamePlayDTOList = gamePlayDTOList.OrderByDescending(x => x.PlayDate).ThenBy(x => x.Site).ToList();
                    }
                }
                return gamePlayDTOList;
            });
        }


        public async Task<List<AccountCreditPlusSummaryDTO>> GetAccountCreditPlusSummaryDTOList(int accountId = -1, string accountNumber = null, DateTime? fromDate = null, DateTime? toDate = null, string creditPlusType = null, bool showExpiryEntitlements = false, int currentPage = 0,
                                                                        int pageSize = 50)
        {
            log.LogMethodEntry(accountId, accountNumber, fromDate, toDate, creditPlusType, showExpiryEntitlements, currentPage, pageSize);
            return await Task<List<AccountCreditPlusSummaryDTO>>.Factory.StartNew(() =>
            {
                if(toDate < fromDate)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 2642);
                    log.Error("Throwing Exception- " + errorMessage);
                    throw new ValidationException(errorMessage);
                }
                AccountSummaryOptions accountSummaryOptions = accountSummaryOptions = new AccountSummaryOptions(executionContext, fromDate, toDate, creditPlusType, showExpiryEntitlements);
                AccountBL accountBL = null;
                AccountCreditPlusSummaryCollection accountCreditPlusSummaryCollection = null;
                if (accountId == -1 && string.IsNullOrEmpty(accountNumber))
                {
                    string errorMessage = "Invalid inputs - Please enter account Id or account Number";
                    log.Error("Throwing Exception- " + errorMessage);
                    throw new ValidationException(errorMessage);
                }
                if (accountId != -1)
                {
                    accountBL = new AccountBL(executionContext, accountId);
                    log.Debug("Get account details based on accountId");
                }
                else
                {
                    accountBL = new AccountBL(executionContext, accountNumber);
                    log.Debug("Get account details based on accountNumber");
                }
                if (accountBL.AccountDTO == null)
                {
                    string errorMessage = "Please enter valid account Id or account Number";
                    log.Error("Throwing Exception- " + errorMessage);
                    throw new ValidationException(errorMessage);
                    
                }
                accountCreditPlusSummaryCollection = new AccountCreditPlusSummaryCollection(executionContext, accountBL, accountSummaryOptions);
                var result = accountCreditPlusSummaryCollection
                             .AccountCreditPlusSummaryBLSequenctialList
                             .Select(x => x.GetAccountCreditPlusSummaryDTO)
                             .Skip(pageSize * currentPage)
                             .Take(pageSize)
                             .ToList();
                log.LogMethodExit(result);
                return result;
            });
        }

        public async Task<int> GetAccountCreditPlusSummaryDTOListCount(int accountId = -1, string accountNumber = null, DateTime? fromDate = null, DateTime? toDate = null, string creditPlusType = null, bool showExpiryEntitlements = false)
        {
            log.LogMethodEntry(accountId, accountNumber, fromDate, toDate, creditPlusType, showExpiryEntitlements);
            return await Task<int>.Factory.StartNew(() =>
            {
                if (toDate < fromDate)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 2642);
                    log.Error("Throwing Exception- " + errorMessage);
                    throw new ValidationException(errorMessage);
                }
                AccountSummaryOptions accountSummaryOptions = accountSummaryOptions = new AccountSummaryOptions(executionContext, fromDate, toDate, creditPlusType, showExpiryEntitlements);
                AccountBL accountBL = null;
                AccountCreditPlusSummaryCollection accountCreditPlusSummaryCollection = null;
                if (accountId == -1 && string.IsNullOrEmpty(accountNumber))
                {
                    string errorMessage = "Invalid inputs - Please enter account Id or account Number";
                    log.Error("Throwing Exception- " + errorMessage);
                    throw new ValidationException(errorMessage);
                }
                if (accountId != -1)
                {
                    accountBL = new AccountBL(executionContext, accountId);
                    log.Debug("Get account details based on accountId");
                }
                else
                {
                    accountBL = new AccountBL(executionContext, accountNumber);
                    log.Debug("Get account details based on accountNumber");
                }
                if (accountBL.AccountDTO == null)
                {
                    string errorMessage = "Please enter valid account Id or account Number";
                    log.Error("Throwing Exception- " + errorMessage);
                    throw new ValidationException(errorMessage);

                }
                accountCreditPlusSummaryCollection = new AccountCreditPlusSummaryCollection(executionContext, accountBL, accountSummaryOptions);
                int totalNoOfRecords = accountCreditPlusSummaryCollection.AccountCreditPlusSummaryBLSequenctialList.Count();
                log.LogMethodExit(totalNoOfRecords);
                return totalNoOfRecords;
            });
        }

        public async Task<List<AccountGameSummaryDTO>> GetAccountGameSummaryDTOList(int accountId = -1, string accountNumber = null, DateTime? fromDate = null, DateTime? toDate = null, bool showExpiryEntitlements = false, int currentPage = 0,
                                                                       int pageSize = 50)
        {
            log.LogMethodEntry(accountId, accountNumber, fromDate, toDate, showExpiryEntitlements, currentPage, pageSize);
            return await Task<List<AccountGameSummaryDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(accountId, accountNumber, fromDate, toDate, currentPage, pageSize);
                if (toDate < fromDate)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 2642);
                    log.Error("Throwing Exception- " + errorMessage);
                    throw new ValidationException(errorMessage);
                }
                AccountSummaryOptions accountSummaryOptions = accountSummaryOptions = new AccountSummaryOptions(executionContext, fromDate, toDate, null, showExpiryEntitlements);
                AccountBL accountBL = null;
                AccountGameSummaryCollection accountGameSummaryCollection = null;
                if (accountId == -1 && string.IsNullOrEmpty(accountNumber))
                {
                    string errorMessage = "Invalid inputs - Please enter account Id or account Number";
                    log.Error("Throwing Exception- " + errorMessage);
                    throw new ValidationException(errorMessage);
                }
                if (accountId != -1)
                {
                    accountBL = new AccountBL(executionContext, accountId);
                    log.Debug("Get account details based on accountId");
                }
                else
                {
                    accountBL = new AccountBL(executionContext, accountNumber);
                    log.Debug("Get account details based on accountNumber");
                }
                if (accountBL.AccountDTO == null)
                {
                    string errorMessage = "Please enter valid account Id or account Number";
                    log.Error("Throwing Exception- " + errorMessage);
                    throw new ValidationException(errorMessage);

                }
                accountGameSummaryCollection = new AccountGameSummaryCollection(executionContext, accountBL, accountSummaryOptions);
                var result = accountGameSummaryCollection
                             .AccountGameSummaryBLSequenctialList
                             .Select(x => x.GetAccountGameSummaryDTO)
                             .Skip(pageSize * currentPage)
                             .Take(pageSize)
                             .ToList();
                log.LogMethodExit(result);
                return result;
            });
        }

        public async Task<int> GetAccountGameSummaryDTOListCount(int accountId = -1, string accountNumber = null, DateTime? fromDate = null, DateTime? toDate = null, bool showExpiryEntitlements = false)
        {
            log.LogMethodEntry(accountId, accountNumber, fromDate, toDate, showExpiryEntitlements);
            return await Task<int>.Factory.StartNew(() =>
            {
                log.LogMethodEntry();
                if (toDate < fromDate)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 2642);
                    log.Error("Throwing Exception- " + errorMessage);
                    throw new ValidationException(errorMessage);
                }
                AccountSummaryOptions accountSummaryOptions = accountSummaryOptions = new AccountSummaryOptions(executionContext, fromDate, toDate, null, showExpiryEntitlements);
                AccountBL accountBL = null;
                AccountGameSummaryCollection accountGameSummaryCollection = null;
                if (accountId == -1 && string.IsNullOrEmpty(accountNumber))
                {
                    string errorMessage = "Invalid inputs - Please enter account Id or account Number";
                    log.Error("Throwing Exception- " + errorMessage);
                    throw new ValidationException(errorMessage);
                }
                if (accountId != -1)
                {
                    accountBL = new AccountBL(executionContext, accountId);
                    log.Debug("Get account details based on accountId");
                }
                else
                {
                    accountBL = new AccountBL(executionContext, accountNumber);
                    log.Debug("Get account details based on accountNumber");
                }
                if (accountBL.AccountDTO == null)
                {
                    string errorMessage = "Please enter valid account Id or account Number";
                    log.Error("Throwing Exception- " + errorMessage);
                    throw new ValidationException(errorMessage);

                }
                accountGameSummaryCollection = new AccountGameSummaryCollection(executionContext, accountBL, accountSummaryOptions);
                int totalNoOfRecords = accountGameSummaryCollection.AccountGameSummaryBLSequenctialList.Count();
                log.LogMethodExit(totalNoOfRecords);
                return totalNoOfRecords;
            });
        }
    }
}
