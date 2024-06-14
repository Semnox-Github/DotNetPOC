/********************************************************************************************
* Project Name - AssetGroupAsset
* Description  - RemoteAssetGroupAssetUseCases
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   21-Apr-2021   B Mahesh Pai             Created 
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Semnox.Parafait.Maintenance
{
    class RemoteAssetGroupAssetUseCases: RemoteUseCases,IAssetGroupAssetUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string AssetGroupAsset_URL = "api/Maintenance/AssetGroupAssets";
        public RemoteAssetGroupAssetUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<AssetGroupAssetDTO>> GetAssetGroupAssets(List<KeyValuePair<AssetGroupAssetDTO.SearchByAssetGroupAssetParameters, string>> parameters)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                
                List<AssetGroupAssetDTO> result = await Get<List<AssetGroupAssetDTO>>(AssetGroupAsset_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<AssetGroupAssetDTO.SearchByAssetGroupAssetParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<AssetGroupAssetDTO.SearchByAssetGroupAssetParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case AssetGroupAssetDTO.SearchByAssetGroupAssetParameters.ASSET_GROUP_ASSET_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("assetGroupAssetId".ToString(), searchParameter.Value));
                        }
                        break;
                    case AssetGroupAssetDTO.SearchByAssetGroupAssetParameters.ASSET_GROUP_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("assetGroupId".ToString(), searchParameter.Value));
                        }
                        break;
                    case AssetGroupAssetDTO.SearchByAssetGroupAssetParameters.ASSET_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("assetId".ToString(), searchParameter.Value));
                        }
                        break;
                    case AssetGroupAssetDTO.SearchByAssetGroupAssetParameters.ACTIVE_FLAG:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("activeFlag".ToString(), searchParameter.Value));
                        }
                        break;

                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveAssetGroupAssets(List<AssetGroupAssetDTO> assetGroupAssetListOnDisplay)
        {
            log.LogMethodEntry(assetGroupAssetListOnDisplay);
            try
            {
                string responseString = await Post<string>(AssetGroupAsset_URL, assetGroupAssetListOnDisplay);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

    }

}
