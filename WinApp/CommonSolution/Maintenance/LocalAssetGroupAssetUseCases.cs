/********************************************************************************************
* Project Name - AssetGroupAsset
* Description  - LocalAssetGroupAssetUseCases
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
  public  class LocalAssetGroupAssetUseCases:IAssetGroupAssetUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalAssetGroupAssetUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<AssetGroupAssetDTO>> GetAssetGroupAssets(List<KeyValuePair<AssetGroupAssetDTO.SearchByAssetGroupAssetParameters, string>> searchParameters)
                         
        {
            return await Task<List<AssetGroupAssetDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                AssetGroupAssetMapperList assetGroupAssetList = new AssetGroupAssetMapperList(executionContext);
                List<AssetGroupAssetDTO> assetGroupassetDTOList = assetGroupAssetList.GetAllAssetGroupAsset(searchParameters);

                log.LogMethodExit(assetGroupassetDTOList);
                return assetGroupassetDTOList;
            });
        }
        public async Task<string> SaveAssetGroupAssets(List<AssetGroupAssetDTO> assetGroupAssetListOnDisplay)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(assetGroupAssetListOnDisplay);
                    if (assetGroupAssetListOnDisplay == null)
                    {
                        throw new ValidationException("assetGroupAssetListOnDisplay is Empty");
                    }

                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                            
                        try
                        {
                            parafaitDBTrx.BeginTransaction();

                            AssetGroupAssetMapperList assetGroupAssetList = new AssetGroupAssetMapperList(executionContext, assetGroupAssetListOnDisplay);
                            assetGroupAssetList.SaveAssetGroupAsset();
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw new Exception(ex.Message, ex);
                        }

                    }

                    result = "Success";
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    result = "Falied";
                }
                log.LogMethodExit(result);
                return result;
            });
        }

    }
}
