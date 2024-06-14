/********************************************************************************************
* Project Name - AssetType
* Description  - RemoteAssetTypeUseCases
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   22-Apr-2021   B Mahesh Pai             Created 
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
    class RemoteAssetTypeUseCases: RemoteUseCases,IAssetTypeUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string AssetTypes_URL = "api/Maintenance/AssetTypes";
        public RemoteAssetTypeUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<AssetTypeDTO>> GetAssetTypes(List<KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>> parameters, SqlTransaction sqlTransaction = null)
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
                List<AssetTypeDTO> result = await Get<List<AssetTypeDTO>>(AssetTypes_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case AssetTypeDTO.SearchByAssetTypeParameters.ASSETTYPE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("assettypeId".ToString(), searchParameter.Value));
                        }
                        break;
                    case AssetTypeDTO.SearchByAssetTypeParameters.ASSETTYPE_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("assetTypeName".ToString(), searchParameter.Value));
                        }
                        break;
                    case AssetTypeDTO.SearchByAssetTypeParameters.ACTIVE_FLAG:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("ActiveFlag".ToString(), searchParameter.Value));
                        }
                        break;
                    case AssetTypeDTO.SearchByAssetTypeParameters.LASTUPDATEDDATE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("lastUpdateDate".ToString(), searchParameter.Value));
                        }
                        break;

                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveAssetTypes(List<AssetTypeDTO> assetTypeListOnDisplay)
        {
            log.LogMethodEntry(assetTypeListOnDisplay);
            try
            {
                string responseString = await Post<string>(AssetTypes_URL, assetTypeListOnDisplay);
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
