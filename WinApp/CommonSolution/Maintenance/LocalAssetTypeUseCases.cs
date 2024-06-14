/********************************************************************************************
* Project Name - AssetType
* Description  - LocalAssetTypeUseCases
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
    class LocalAssetTypeUseCases:IAssetTypeUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalAssetTypeUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<AssetTypeDTO>> GetAssetTypes(List<KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)

        {
            return await Task<List<AssetTypeDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                AssetTypeList assetTypeList = new AssetTypeList(executionContext);
                List<AssetTypeDTO> assetTypeDTOList = assetTypeList.GetAllAssetTypes(searchParameters, sqlTransaction);

                log.LogMethodExit(assetTypeDTOList);
                return assetTypeDTOList;
            });
        }
        public async Task<string> SaveAssetTypes(List<AssetTypeDTO> assetTypeListOnDisplay)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(assetTypeListOnDisplay);
                    if (assetTypeListOnDisplay == null)
                    {
                        throw new ValidationException("assetTypeDTOList is Empty");
                    }

                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {

                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            AssetTypeList assetTypeList = new AssetTypeList(executionContext, assetTypeListOnDisplay);
                            assetTypeList.SaveAssetTypes();
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


