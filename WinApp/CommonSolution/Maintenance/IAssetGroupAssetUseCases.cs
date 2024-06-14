/********************************************************************************************
* Project Name - AssetGroupAsset
* Description  - IAssetGroupAssetUseCases
* * 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   21-Apr-2021  B Mahesh Pai        Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Maintenance
{
   public interface IAssetGroupAssetUseCases
    {
        Task <List<AssetGroupAssetDTO>> GetAssetGroupAssets(List<KeyValuePair<AssetGroupAssetDTO.SearchByAssetGroupAssetParameters, string>> searchParameters);
        Task<string> SaveAssetGroupAssets(List<AssetGroupAssetDTO> assetGroupAssetListOnDisplay);
              
    }
}
