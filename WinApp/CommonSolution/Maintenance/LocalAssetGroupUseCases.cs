/********************************************************************************************
* Project Name - AssetGroup
* Description  - LocalAssetGroupUseCases
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
   public class LocalAssetGroupUseCases:IAssetGroupUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalAssetGroupUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<AssetGroupDTO>> GetAssetGroups(List<KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
                 
        {
            return await Task<List<AssetGroupDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                AssetGroupList assetGroupList = new AssetGroupList(executionContext);
                List<AssetGroupDTO> asssetGroupDTOList = assetGroupList.GetAllAssetGroups(searchParameters, sqlTransaction);

                log.LogMethodExit(asssetGroupDTOList);
                return asssetGroupDTOList;
            });
        }
        public async Task<string> SaveAssetGroups(List<AssetGroupDTO> assetGroupListOnDisplay)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(assetGroupListOnDisplay);
                    if (assetGroupListOnDisplay == null)
                    {
                        throw new ValidationException("assetGroupDTOList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            AssetGroupList assetGroupList = new AssetGroupList(assetGroupListOnDisplay, executionContext);
                            assetGroupList.SaveAssetGroups();
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
                            throw ex;
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
