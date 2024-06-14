/********************************************************************************************
 * Project Name - AccountActivityViewListBL
 * Description  - AccountActivityViewListBL
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 ********************************************************************************************* 
 *2.130.0     19-July-2021      Girish Kundar      Modified : Virtual point column added part of Arcade changes
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// Manages the list of AccountActivityView
    /// </summary>
    public class AccountActivityViewListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private AccountActivityHelper accountActivityHelper;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="accountActivityHelper">execution context</param>
        public AccountActivityViewListBL(ExecutionContext executionContext, AccountActivityHelper accountActivityHelper = null)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.accountActivityHelper = accountActivityHelper;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the AccountActivityView list
        /// </summary>
        public List<AccountActivityDTO> GetAccountActivityDTOList(List<KeyValuePair<AccountActivityDTO.SearchByParameters, string>> searchParameters, bool addSummaryRow = true, SqlTransaction sqlTransaction = null,
            int numberOfRecords = -1, int pageNumber = 0, int lastRowNumberId = -1)
        {
            log.LogMethodEntry(searchParameters);
            AccountActivityDataHandler accountActivityViewDataHandler = new AccountActivityDataHandler(sqlTransaction);
            List<AccountActivityDTO> accountActivityDTOList = accountActivityViewDataHandler.GetAccountActivityDTOList(searchParameters, numberOfRecords, pageNumber, lastRowNumberId);
            if (accountActivityDTOList != null && accountActivityDTOList.Count > 0 && addSummaryRow)
            {
                AccountActivityDTO summaryRow = GetSummaryAccountActivityDTO(accountActivityDTOList);
                accountActivityDTOList.Insert(0, summaryRow);
            }
            log.LogMethodExit(accountActivityDTOList);
            return accountActivityDTOList;
        }
        /// <summary>
        /// Returns the AccountActivityView list
        /// </summary>
        public List<AccountActivityDTO> GetAccountActivityDTOListByAccountIds(List<int> accountIdList, bool addSummaryRow = true, SqlTransaction sqlTransaction = null,
            int numberOfRecords = -1, int pageNumber = 0, int lastRowNumberId = -1)
        {
            log.LogMethodEntry(accountIdList, addSummaryRow, sqlTransaction, numberOfRecords, pageNumber, lastRowNumberId);
            AccountActivityDataHandler accountActivityViewDataHandler = new AccountActivityDataHandler(sqlTransaction);
            List<AccountActivityDTO> accountActivityDTOList = accountActivityViewDataHandler.GetAccountActivityDTOListByAccountIdList(accountIdList, numberOfRecords, pageNumber, lastRowNumberId);
            if (accountActivityDTOList != null && accountActivityDTOList.Count > 0 && addSummaryRow)
            {
                AccountActivityDTO summaryRow = GetSummaryAccountActivityDTO(accountActivityDTOList);
                accountActivityDTOList.Insert(0, summaryRow);
            }
            log.LogMethodExit(accountActivityDTOList);
            return accountActivityDTOList;
        }
        /// <summary>
        /// Returns the consolidated account activity
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="addSummaryRow"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<AccountActivityDTO> GetConsolidatedAccountActivityDTOList(List<KeyValuePair<AccountActivityDTO.SearchByParameters, string>> searchParameters, bool addSummaryRow = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<AccountActivityDTO> consolidatedAccountActivityDTOList = new List<AccountActivityDTO>();
            List<AccountActivityDTO> localAccountActivityDTOList = GetAccountActivityDTOList(searchParameters, false, sqlTransaction);
            List<AccountActivityDTO> serverAccountActivityDTOList = accountActivityHelper.GetServerAccountActivityDTOList();
            if (localAccountActivityDTOList != null && localAccountActivityDTOList.Count > 0)
            {
                consolidatedAccountActivityDTOList.AddRange(localAccountActivityDTOList);
            }
            if (serverAccountActivityDTOList != null && serverAccountActivityDTOList.Count > 0)
            {
                foreach (var serverAccountActivityDTO in serverAccountActivityDTOList)
                {
                    if (consolidatedAccountActivityDTOList.FirstOrDefault(x => x.Date == serverAccountActivityDTO.Date && x.Product == serverAccountActivityDTO.Product) == null)
                    {
                        consolidatedAccountActivityDTOList.Add(serverAccountActivityDTO);
                    }
                    else
                    {
                        AccountActivityDTO accountActivityDTO = consolidatedAccountActivityDTOList.FirstOrDefault(x => x.Date == serverAccountActivityDTO.Date && x.Product == serverAccountActivityDTO.Product);
                        if (accountActivityDTO != null)
                        {
                            accountActivityDTO.Site = serverAccountActivityDTO.Site;
                        }
                    }
                }
            }
            if (consolidatedAccountActivityDTOList != null && consolidatedAccountActivityDTOList.Count > 0)
            {
                consolidatedAccountActivityDTOList = consolidatedAccountActivityDTOList.OrderByDescending(x => x.Date).ToList();
                if(addSummaryRow)
                {
                    AccountActivityDTO summaryAccountActivityDTO = GetSummaryAccountActivityDTO(consolidatedAccountActivityDTOList);
                    consolidatedAccountActivityDTOList.Insert(0, summaryAccountActivityDTO);
                }
            }
            log.LogMethodExit(consolidatedAccountActivityDTOList);
            return consolidatedAccountActivityDTOList;
        }

        private AccountActivityDTO GetSummaryAccountActivityDTO(List<AccountActivityDTO> accountActivityDTOList)
        {
            log.LogMethodEntry(accountActivityDTOList);
            AccountActivityDTO summaryRow = new AccountActivityDTO();
            summaryRow.Product = MessageContainerList.GetMessage(executionContext, "Grand Total");
            summaryRow.Bonus = 0;
            summaryRow.Credits = 0;
            summaryRow.Courtesy = 0;
            summaryRow.LoyaltyPoints = 0;
            summaryRow.VirtualPoints = 0;
            summaryRow.Amount = 0;
            summaryRow.Time = 0;
            summaryRow.Quantity = 0;
            summaryRow.Tokens = 0;
            summaryRow.Tickets = 0;
            foreach (var accountActivityDTO in accountActivityDTOList)
            {
                if (accountActivityDTO.Bonus.HasValue)
                {
                    summaryRow.Bonus = summaryRow.Bonus.Value + accountActivityDTO.Bonus.Value;
                }
                if (accountActivityDTO.Credits.HasValue)
                {
                    summaryRow.Credits = summaryRow.Credits.Value + accountActivityDTO.Credits.Value;
                }
                if (accountActivityDTO.Courtesy.HasValue)
                {
                    summaryRow.Courtesy = summaryRow.Courtesy.Value + accountActivityDTO.Courtesy.Value;
                }
                if (accountActivityDTO.LoyaltyPoints.HasValue)
                {
                    summaryRow.LoyaltyPoints = summaryRow.LoyaltyPoints.Value + accountActivityDTO.LoyaltyPoints.Value;
                }
                if (accountActivityDTO.VirtualPoints.HasValue)
                {
                    summaryRow.VirtualPoints = summaryRow.VirtualPoints.Value + accountActivityDTO.VirtualPoints.Value;
                }
                if (accountActivityDTO.Amount.HasValue)
                {
                    summaryRow.Amount = summaryRow.Amount.Value + accountActivityDTO.Amount.Value;
                }
                if (accountActivityDTO.Time.HasValue)
                {
                    summaryRow.Time = summaryRow.Time.Value + accountActivityDTO.Time.Value;
                }
                if (accountActivityDTO.Quantity.HasValue)
                {
                    summaryRow.Quantity = summaryRow.Quantity.Value + accountActivityDTO.Quantity.Value;
                }
                if (accountActivityDTO.Tokens.HasValue)
                {
                    summaryRow.Tokens = summaryRow.Tokens.Value + accountActivityDTO.Tokens.Value;
                }
                if (accountActivityDTO.Tickets.HasValue)
                {
                    summaryRow.Tickets = summaryRow.Tickets.Value + accountActivityDTO.Tickets.Value;
                }
            }
            log.LogMethodExit(summaryRow);
            return summaryRow;
        }

        /// <summary>
        /// Returns the consolidated account activity
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="addSummaryRow"></param>        
        /// <param name="accountId"></param>
        /// <param name="cardNumber"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<AccountActivityDTO> GetServerCardActivityFromWSList(List<KeyValuePair<AccountActivityDTO.SearchByParameters, string>> searchParameters, bool addSummaryRow, string accountId, string cardNumber, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, addSummaryRow,accountId,cardNumber);
            try
            {
                List<AccountActivityDTO> consolidatedAccountActivityDTOList = new List<AccountActivityDTO>();
                List<AccountActivityDTO> localAccountActivityDTOList = GetAccountActivityDTOList(searchParameters, false, sqlTransaction);

                int siteId = executionContext.GetSiteId();
                string loginId = executionContext.GetUserId();
                string companyKey = string.Empty;

                AccountActivityDataHandler accountActivityViewDataHandler = new AccountActivityDataHandler(sqlTransaction);
                Utilities utilities = new Utilities();
                utilities.ParafaitEnv.getCompanyLoginKey();
                companyKey = utilities.ParafaitEnv.CompanyLoginKey;
                if (companyKey != null)
                {
                    companyKey = companyKey.ToString();
                }
                else
                {
                    throw new Exception(utilities.MessageUtils.getMessage(1744));
                }
                // Default value sending to the constructor for AccountActivityHelper
                accountActivityHelper = new AccountActivityHelper(-1, cardNumber, siteId);                
                List<AccountActivityDTO> serverAccountActivityDTOList = accountActivityHelper.GetServerAccountActivityDTOList(companyKey, cardNumber, siteId);

                if (localAccountActivityDTOList != null && localAccountActivityDTOList.Count > 0)
                {
                    consolidatedAccountActivityDTOList.AddRange(localAccountActivityDTOList);
                }
                if (serverAccountActivityDTOList != null && serverAccountActivityDTOList.Count > 0)
                {
                    foreach (var serverAccountActivityDTO in serverAccountActivityDTOList)
                    {
                        if (consolidatedAccountActivityDTOList.FirstOrDefault(x => x.Date == serverAccountActivityDTO.Date && x.Product == serverAccountActivityDTO.Product) == null)
                        {
                            consolidatedAccountActivityDTOList.Add(serverAccountActivityDTO);
                        }
                        else
                        {
                            AccountActivityDTO accountActivityDTO = consolidatedAccountActivityDTOList.FirstOrDefault(x => x.Date == serverAccountActivityDTO.Date && x.Product == serverAccountActivityDTO.Product);
                            if (accountActivityDTO != null)
                            {
                                accountActivityDTO.Site = serverAccountActivityDTO.Site;
                            }
                        }
                    }
                }
                if (consolidatedAccountActivityDTOList != null && consolidatedAccountActivityDTOList.Count > 0)
                {
                    consolidatedAccountActivityDTOList = consolidatedAccountActivityDTOList.OrderByDescending(x => x.Date).ToList();
                    if (addSummaryRow)
                    {
                        AccountActivityDTO summaryAccountActivityDTO = GetSummaryAccountActivityDTO(consolidatedAccountActivityDTOList);
                        consolidatedAccountActivityDTOList.Insert(0, summaryAccountActivityDTO);
                    }
                }
                log.LogMethodExit(consolidatedAccountActivityDTOList);
                return consolidatedAccountActivityDTOList;
            }
            catch
            {
                throw;
            }
        }
    }
}
