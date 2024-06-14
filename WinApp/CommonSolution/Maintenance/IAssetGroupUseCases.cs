/********************************************************************************************
* Project Name - AssetGroup
* Description  - IAssetGroupUseCases
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   22-Apr-2021   B Mahesh Pai             Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Maintenance
{
   public interface IAssetGroupUseCases
    {
        Task<List<AssetGroupDTO>> GetAssetGroups(List<KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
         Task<string> SaveAssetGroups(List<AssetGroupDTO> assetGroupListOnDisplay);
    }
}
