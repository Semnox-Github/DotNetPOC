/********************************************************************************************
 * Project Name - Lookups
 * Description  - LocalLookupsUseCases class
 *
 **************
 ** Version Log
  **************
  * Version      Date              Modified By         Remarks
 *********************************************************************************************
 2.120.0         06-May-2021       B Mahesh Pai        Modified
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.Utilities
{
    public class LocalLookupsUseCases : ILookupsUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalLookupsUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public async Task<LookupsContainerDTOCollection> GetLookupsContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            return await Task<LookupsContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);
                if (rebuildCache)
                {
                    LookupsContainerList.Rebuild(siteId);
                }
                LookupsContainerDTOCollection result = LookupsContainerList.GetLookupsContainerDTOCollection(siteId);
                if (hash == result.Hash)
                {
                    log.LogMethodExit(null, "No changes to the cache");
                    return null;
                }
                log.LogMethodExit(result);
                return result;
            });
        }
        public async Task<List<LookupsDTO>> GetLookups(List<KeyValuePair<LookupsDTO.SearchByParameters, string>> searchParameters, bool loadChild = false,
                                              bool loadActiveChildRecords = true, SqlTransaction sqlTransaction = null)

        {
            return await Task<List<LookupsDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters, loadChild, loadActiveChildRecords, sqlTransaction);

                LookupsList lookupsListBL = new LookupsList(executionContext);
                List<LookupsDTO> lookupsDTOList = lookupsListBL.GetAllLookups(searchParameters, loadChild, loadActiveChildRecords, sqlTransaction);

                log.LogMethodExit(lookupsDTOList);
                return lookupsDTOList;
            });
        }
        public async Task<string> SaveLookups(List<LookupsDTO> lookupsDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(lookupsDTOList);
                    if (lookupsDTOList == null)
                    {
                        throw new ValidationException("customerFeedbackSurveyDTOList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            LookupsList lookupsList = new LookupsList(executionContext, lookupsDTOList);
                            lookupsList.Save();
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
