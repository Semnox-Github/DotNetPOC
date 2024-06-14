/********************************************************************************************
 * Project Name - Customer
 * Description  - IMembershipUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0          05-Dec-2020      Vikas Dwivedi             Created : POS UI Redesign with REST API
 2.120.0      07-May-2021      B Mahesh Pai              SaveAllMembership code added
 2.130.3      16-Dec-2021      Abhishek                  WMS fix : Added two parameters loadChildRecords,loadActiveChildRecords
 2.140.1      20-Dec-2021       Abhishek                  WMS fix : Added two parameters loadChildRecords,loadActiveChildRecords
 ********************************************************************************************/
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer.Membership
{
    public interface IMembershipUseCases
    {
        Task<List<MembershipDTO>> GetAllMemberships(List<KeyValuePair<MembershipDTO.SearchByParameters, string>> searchParameters, bool loadChildRecords = false, bool loadActiveChildRecords = false, SqlTransaction sqlTransaction = null);
        Task<MembershipContainerDTOCollection> GetMembershipContainerDTOCollection(int siteId, string hash, bool rebuildCache);
        Task<string> SaveAllMembership(List<MembershipDTO> membershipDTOList);
    }
}
