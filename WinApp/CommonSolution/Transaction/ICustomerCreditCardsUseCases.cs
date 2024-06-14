/********************************************************************************************
 * Project Name - ICustomerCreditCardsUseCases
 * Description  - ICustomerCreditCardsUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0       25-Jan-2021      Guru S A                Created for subcription changes
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// ICustomerCreditCardsUseCases
    /// </summary>
    public interface ICustomerCreditCardsUseCases
    {
        /// <summary>
        /// GetCustomerCreditCards
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="utilities"></param> 
        /// <returns></returns>
        Task<List<CustomerCreditCardsDTO>> GetCustomerCreditCards(List<KeyValuePair<CustomerCreditCardsDTO.SearchByParameters, string>> searchParameters, Utilities utilities);
        /// <summary>
        /// SaveCustomerCreditCards
        /// </summary>
        /// <param name="customerCreditCardsDTOList"></param>
        /// <returns></returns>
        Task<string> SaveCustomerCreditCards(List<CustomerCreditCardsDTO> customerCreditCardsDTOList);
    }
}
