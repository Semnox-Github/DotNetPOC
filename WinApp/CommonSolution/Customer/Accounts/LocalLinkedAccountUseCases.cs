/********************************************************************************************
 * Project Name - Accounts
 * Description  - LocalLinkedAccountUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         23-Apr-2022       Nitin Pai                 Created : 
 2.130.11    23-Sep-2022     Yashodhara C H            Added new method to get LinkedAccountsummary details
 ********************************************************************************************/
using System.Collections.Generic;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer.Accounts
{
    public class LocalLinkedAccountUseCases : LocalUseCases, ILinkedAccountUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public LocalLinkedAccountUseCases(ExecutionContext executionContext) : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        } 

        public async Task<AccountDTOCollection> GetLinkedAccounts(int customerId)
        {
            log.LogMethodEntry(customerId);
            return await Task<AccountDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(customerId);

                List<KeyValuePair<AccountDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AccountDTO.SearchByParameters, string>>();
                if (customerId > -1)
                {
                    searchParameters.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.CUSTOMER_ID, customerId.ToString()));
                }

                LinkedAccountListBL linkedAccountBL = new LinkedAccountListBL(executionContext);
                List<AccountDTO> accountDTOList = linkedAccountBL.GetLinkedAccounts(searchParameters);

                log.LogMethodExit(accountDTOList);
                AccountDTOCollection accountDTOCollection = new AccountDTOCollection(accountDTOList, 0, accountDTOList != null ? accountDTOList.Count : 0 , "", executionContext.WebApiToken);
                return accountDTOCollection;
                
            });
        }

        /// <summary>
        /// Gets the linked AccountSummaryViewDTO
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<List<AccountSummaryViewDTO>> GetLinkedAccountSummaryViewDTO(int customerId)
        {
            log.LogMethodEntry(customerId);
            return await Task<List<AccountSummaryViewDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(customerId);
                List<KeyValuePair<AccountSummaryViewDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AccountSummaryViewDTO.SearchByParameters, string>>();
                if (customerId > -1)
                {
                    searchParameters.Add(new KeyValuePair<AccountSummaryViewDTO.SearchByParameters, string>(AccountSummaryViewDTO.SearchByParameters.CUSTOMER_ID, customerId.ToString()));
                }
                LinkedAccountListBL linkedAccountBL = new LinkedAccountListBL(executionContext);
                List<AccountSummaryViewDTO> accountDTOList = linkedAccountBL.GetLinkedAccountSummaryView(searchParameters);
                log.LogMethodExit(accountDTOList);
                return accountDTOList;
            });
        }
    }
}
