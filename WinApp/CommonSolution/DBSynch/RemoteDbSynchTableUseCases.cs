/********************************************************************************************
 * Project Name -DBSynch
 * Description  -RemoteDbSynchTableUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    05-May-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.DBSynch
{
    class RemoteDbSynchTableUseCases:RemoteUseCases,IDbSynchTableUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string DBSYNC_TABLE_URL = "api/Configuration/DBSyncTables";
        public RemoteDbSynchTableUseCases(ExecutionContext executionContext)
          : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<DBSynchTableDTO>> GetDBSynchs(List<KeyValuePair<DBSynchTableDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();

            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                List<DBSynchTableDTO> result = await Get<List<DBSynchTableDTO>>(DBSYNC_TABLE_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<DBSynchTableDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<DBSynchTableDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case DBSynchTableDTO.SearchByParameters.DBSYNCH_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("dbSynchId".ToString(), searchParameter.Value));
                        }
                        break;
                    case DBSynchTableDTO.SearchByParameters.TABLE_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("tableName".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveDBSynchs(List<DBSynchTableDTO> dBSynchDTOList)
        {
            log.LogMethodEntry(dBSynchDTOList);
            try
            {
                string responseString = await Post<string>(DBSYNC_TABLE_URL, dBSynchDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<string> Delete(List<DBSynchTableDTO> dBSynchDTOList)
        {
            try
            {
                log.LogMethodEntry(dBSynchDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(dBSynchDTOList);
                string responseString = await Delete(DBSYNC_TABLE_URL, content);
                dynamic response = JsonConvert.DeserializeObject(responseString);
                log.LogMethodExit(response);
                return response;
            }
            catch (WebApiException wex)
            {
                log.Error(wex);
                throw;
            }
        }


    }
}
