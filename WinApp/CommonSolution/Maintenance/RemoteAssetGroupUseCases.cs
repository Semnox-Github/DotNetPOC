/********************************************************************************************
* Project Name - AssetGroup
* Description  - RemoteAssetGroupUseCases
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
     class RemoteAssetGroupUseCases: RemoteUseCases, IAssetGroupUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string AssetGroup_URL = "api/Maintenance/AssetGroups";
        public RemoteAssetGroupUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<AssetGroupDTO>> GetAssetGroups(List<KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>> parameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();

            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                List<AssetGroupDTO> result = await Get<List<AssetGroupDTO>>(AssetGroup_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case AssetGroupDTO.SearchByAssetGroupParameters.ASSETGROUP_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("assetGroupId".ToString(), searchParameter.Value));
                        }
                        break;
                    case AssetGroupDTO.SearchByAssetGroupParameters.ASSETGROUP_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("assetGroupName".ToString(), searchParameter.Value));
                        }
                        break;
                    case AssetGroupDTO.SearchByAssetGroupParameters.ACTIVE_FLAG:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("ActiveFlag".ToString(), searchParameter.Value));
                        }
                        break;
                    

                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveAssetGroups(List<AssetGroupDTO> assetGroupListOnDisplay)
        {
            log.LogMethodEntry(assetGroupListOnDisplay);
            try
            {
                string responseString = await Post<string>(AssetGroup_URL, assetGroupListOnDisplay);
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
