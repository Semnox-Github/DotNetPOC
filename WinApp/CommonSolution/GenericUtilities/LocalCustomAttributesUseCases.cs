/********************************************************************************************
 * Project Name -CustomAttributes
 * Description  -LocalCustomAttributesUseCases class
 *
 **************
 ** Version Log
  **************
  * Version      Date              Modified By         Remarks
 *********************************************************************************************
 2.120.0         12-May-2021       B Mahesh Pai       Created
 2.130.0         27-Jul-2021       Mushahid Faizan    Modified :- POS UI redesign changes.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities
{
    class LocalCustomAttributesUseCases:ICustomAttributesUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalCustomAttributesUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<CustomAttributesDTO>> GetCustomAttributes(List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>> searchParameters,
            bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<CustomAttributesDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);

                CustomAttributesListBL customAttributesListBL = new CustomAttributesListBL(executionContext);

                List<CustomAttributesDTO> customAttributesDTOList = customAttributesListBL.GetCustomAttributesDTOList(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);

                log.LogMethodExit(customAttributesDTOList);
                return customAttributesDTOList;
            });
        }
        public async Task<string> SaveCustomAttributes(List<CustomAttributesDTO> customAttributesDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(customAttributesDTOList);
                    if (customAttributesDTOList == null)
                    {
                        throw new ValidationException("customAttributesDTOList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            CustomAttributesListBL customAttributesListBL = new CustomAttributesListBL(executionContext, customAttributesDTOList);
                            customAttributesListBL.SaveUpdateCustomAttributesList();
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


        public async Task<CustomAttributeContainerDTOCollection> GetCustomAttributesContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            return await Task<CustomAttributeContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);
                if (rebuildCache)
                {
                    CustomAttributeContainerList.Rebuild(siteId);
                }
                CustomAttributeContainerDTOCollection result = CustomAttributeContainerList.GetCustomAttributeContainerDTOCollection(siteId);
                if (hash == result.Hash)
                {
                    log.LogMethodExit(null, "No changes to the cache");
                    return null;
                }
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}

