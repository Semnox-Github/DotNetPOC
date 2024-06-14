/********************************************************************************************
* Project Name -AssetType
* Description  - IAssetTypeUseCases class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.120.00    22-Apr-2021       B Mahesh Pai        Created : POS UI Redesign with REST API
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Maintenance
{
   public interface IAssetTypeUseCases
    {
        Task<List<AssetTypeDTO>> GetAssetTypes(List<KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
        Task<string> SaveAssetTypes(List<AssetTypeDTO> assetTypeListOnDisplay);
    }
}
