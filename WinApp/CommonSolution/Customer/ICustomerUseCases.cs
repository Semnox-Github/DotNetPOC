/********************************************************************************************
 * Project Name - Customer
 * Description  - ICustomerUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0          05-Dec-2020      Vikas Dwivedi             Created : POS UI Redesign with REST API
 2.120.0      15-Mar-2021      Prajwal S                 Modified : Added Get of Summary DTO
 2.130.10     08-Sep-2022      Nitin Pai                 Modified as part of customer delete enhancement.
 ********************************************************************************************/
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer
{
    public interface ICustomerUseCases
    {
        Task<List<CustomerDTO>> GetCustomerDTOList(List<KeyValuePair<CustomerSearchByParameters, string>> searchParameters,
            bool loadChildRecords = false, bool activeChildRecords = true, bool loadSignedWaivers = false, bool loadSignedWaiverFileContent = false);

        Task<CustomerSummaryDTO> GetCustomerSummaryDTO(int customerId);
        Task<CustomerDTO> SaveCustomerAddress(List<AddressDTO> addressDTOList, int cutomerId);
        Task DeleteCustomer(int customerId);
        Task<string> SaveCustomerNickname(int customerId, string nickname);
    }
}
