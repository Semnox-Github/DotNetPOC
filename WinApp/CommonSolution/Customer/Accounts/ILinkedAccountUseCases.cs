/********************************************************************************************
 * Project Name - Accounts
 * Description  - ILinkedAccountUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         23-Apr-2022       Nitin Pai                 Created : 
 2.130.11    14-Oct-2022     Yashodhara C H            Added new method to get linked account summary details.
 ********************************************************************************************/
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer.Accounts
{
    public interface ILinkedAccountUseCases
    {
        Task<AccountDTOCollection> GetLinkedAccounts(int customerId);

        Task<List<AccountSummaryViewDTO>> GetLinkedAccountSummaryViewDTO(int customerId);
    }
}
