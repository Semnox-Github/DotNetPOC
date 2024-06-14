/********************************************************************************************
 * Project Name - Accounts
 * Description  - RemoteLinkedAccountUseCases class 
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
    public class RemoteLinkedAccountUseCases : RemoteUseCases, ILinkedAccountUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string LINKED_ACCOUNT_URL = "api/Customer/Account/LinkedAccounts";
        private const string LINKED_ACCOUNT_SUMMARY_URL = "api/Customer/Account/LinkedAccountsSummary";

        public RemoteLinkedAccountUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<AccountDTOCollection> GetLinkedAccounts(int customerId)
        {
            log.LogMethodEntry(customerId);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("customerId", customerId.ToString()));
            AccountDTOCollection result = await Get<AccountDTOCollection>(LINKED_ACCOUNT_URL, searchParameterList, string.Empty);
            log.LogMethodExit(result);
            return result;
        }

        public async Task<List<AccountSummaryViewDTO>> GetLinkedAccountSummaryViewDTO(int customerId)
        {
            log.LogMethodEntry(customerId);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("customerId", customerId.ToString()));
            List<AccountSummaryViewDTO> result = await Get<List<AccountSummaryViewDTO>>(LINKED_ACCOUNT_SUMMARY_URL, searchParameterList, string.Empty);
            log.LogMethodExit(result);
            return result;
        }

    }
}
