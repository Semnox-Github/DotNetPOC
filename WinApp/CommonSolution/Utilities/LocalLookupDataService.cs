/********************************************************************************************
 * Project Name - Utilities 
 * Description  - LocalLookupDataService class to get the data  from local DB 
 * 
 *   
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Girish Kundar             Created : POS UI Redesign with REST API
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using Semnox.Core.Utilities;

namespace Semnox.Core.Utilities
{
    public class LocalLookupDataService : ILookupDataService
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalLookupDataService(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public List<LookupsDTO> GetLookups(List<KeyValuePair<LookupsDTO.SearchByParameters, string>> parameters , bool loadChildRecords = false)
        {
            log.LogMethodEntry(parameters);
            LookupsList lookupsList = new LookupsList(executionContext);
            int siteId = GetSiteId();
            List<LookupsDTO> lookupsDTOList = lookupsList.GetAllLookups(parameters,loadChildRecords,true);
            log.LogMethodExit(lookupsDTOList);
            return lookupsDTOList;
        }

        public string PostLookups(List<LookupsDTO> lookupsDTOList)
        {
            string result = string.Empty;
            try
            {
                log.LogMethodEntry(lookupsDTOList);
                LookupsList lookupsList = new LookupsList(executionContext, lookupsDTOList);
                lookupsList.Save();
                result = "Success";
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
            log.LogMethodExit(result);
            return result;
        }

        public string DeleteLookups(List<LookupsDTO> lookupsDTOList)
        {
            string result = string.Empty;
            try
            {
                log.LogMethodEntry(lookupsDTOList);
                LookupsList lookupsList = new LookupsList(executionContext, lookupsDTOList);
                lookupsList.Delete();
                result = "Success";
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
            log.LogMethodExit(result);
            return result;
        }

        private int GetSiteId()
        {
            log.LogMethodEntry();
            int siteId = -1;
            if (executionContext.GetIsCorporate())
            {
                siteId = executionContext.GetSiteId();
            }
            log.LogMethodExit(siteId);
            return siteId;
        }
    }
}
