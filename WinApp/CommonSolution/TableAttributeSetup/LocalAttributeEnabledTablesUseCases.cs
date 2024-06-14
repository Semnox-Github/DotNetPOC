/********************************************************************************************
 * Project Name - TableAttributeSetup  
 * Description  - LocalAttributeEnabledTablesUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.140.0      20-Aug-2021      Fiona           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.TableAttributeSetup
{
    public class LocalAttributeEnabledTablesUseCases : LocalUseCases, IAttributeEnabledTablesUseCases

    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public LocalAttributeEnabledTablesUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<AttributeEnabledTablesDTO>> GetAttributeEnabledTables(List<KeyValuePair<AttributeEnabledTablesDTO.SearchByParameters, string>> searchParameters, bool loadChildRecords = false, bool loadActiveChild = false)
        {
            return await Task<List<AttributeEnabledTablesDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);
                AttributeEnabledTablesListBL attributeEnabledTablessListBL = new AttributeEnabledTablesListBL(executionContext);
                List<AttributeEnabledTablesDTO> attributeEnabledTablessDTOList = attributeEnabledTablessListBL.GetAttributeEnabledTables(searchParameters, loadChildRecords, loadActiveChild);
                log.LogMethodExit(attributeEnabledTablessDTOList);
                return attributeEnabledTablessDTOList;
            });
        }

        public async Task<AttributeEnabledTablesContainerDTOCollection> GetAttributeEnabledTablesContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            return await Task<AttributeEnabledTablesContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(hash, rebuildCache);
                if (rebuildCache)
                {
                    AttributeEnabledTablesContainerList.Rebuild(siteId);
                }
                AttributeEnabledTablesContainerDTOCollection result = AttributeEnabledTablesContainerList.GetAttributeEnabledTablesContainerDTOCollection(siteId);
                if (hash == result.Hash)
                {
                    log.LogMethodExit(null, "No changes to the cache");
                    return null;
                }
                log.LogMethodExit(result);
                return result;
            });
        }

        public async Task<string> SaveAttributeEnabledTables(List<AttributeEnabledTablesDTO> attributeEnabledTablesDTOList)
        {
            log.LogMethodEntry(attributeEnabledTablesDTOList);
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                if (attributeEnabledTablesDTOList == null)
                {
                    throw new ValidationException("AttributeEnabledTablesDTOList is Empty");
                }
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        AttributeEnabledTablesListBL attributeEnabledTablesList = new AttributeEnabledTablesListBL(executionContext, attributeEnabledTablesDTOList);
                        attributeEnabledTablesList.Save(parafaitDBTrx.SQLTrx);
                        parafaitDBTrx.EndTransaction();
                    }
                    catch (ValidationException valEx)
                    {
                        log.Error(valEx);
                        if(parafaitDBTrx!=null)
                        {
                            parafaitDBTrx.RollBack();
                        }
                        throw valEx;
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        if (parafaitDBTrx != null)
                        {
                            parafaitDBTrx.RollBack();
                        }
                        log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                        throw ex;
                    }
                }
                result = "Success";
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}
