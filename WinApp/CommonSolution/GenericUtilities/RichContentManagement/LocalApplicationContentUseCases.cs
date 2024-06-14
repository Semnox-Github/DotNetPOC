/********************************************************************************************
 * Project Name -ApplicationContent
 * Description  -LocalApplicationContentUseCases class
 *
 **************
 ** Version Log
  **************
  * Version      Date              Modified By         Remarks
 *********************************************************************************************
 2.120.0         12-May-2021       B Mahesh Pai       Created
 2.130.0         20-Jul-2021       Mushahid Faizan    Modified - POS UI redesign changes.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities
{
    class LocalApplicationContentUseCases:IApplicationContentUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalApplicationContentUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<ApplicationContentDTO>> GetApplicationContents(List<KeyValuePair<ApplicationContentDTO.SearchByParameters, string>> searchParameters, bool loadChildRecords = false, bool loadActiveRecords = false,
                                           SqlTransaction sqlTransaction = null)
        {
            return await Task<List<ApplicationContentDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters, loadChildRecords, loadActiveRecords, sqlTransaction);

                ApplicationContentListBL applicationContentListBL = new ApplicationContentListBL(executionContext);
                List<ApplicationContentDTO> applicationContentDTOList = applicationContentListBL.GetApplicationContentDTOList(searchParameters, loadChildRecords, loadActiveRecords, sqlTransaction);

                log.LogMethodExit(applicationContentDTOList);
                return applicationContentDTOList;
            });
        }
        public async Task<string> SaveApplicationContents(List<ApplicationContentDTO> applicationContentDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(applicationContentDTOList);
                    if (applicationContentDTOList == null)
                    {
                        throw new ValidationException("applicationContentDTOList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            ApplicationContentListBL applicationContentListBL = new ApplicationContentListBL(executionContext, applicationContentDTOList);
                            applicationContentListBL.Save();
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


        public async Task<ApplicationContentContainerDTOCollection> GetApplicationContentContainerDTOCollection(int siteId, int languageId, string hash, bool rebuildCache)
        {
            return await Task<ApplicationContentContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);
                if (rebuildCache)
                {
                    ApplicationContentContainerList.Rebuild(siteId);
                }

                ApplicationContentContainerDTOCollection result = ApplicationContentContainerList.GetApplicationContentContainerDTOCollection(siteId, languageId);

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
