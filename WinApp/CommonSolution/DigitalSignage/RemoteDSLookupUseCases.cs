/********************************************************************************************
* Project Name - DigitalSignage
* Description  - RemoteDSLookupUseCases class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.140.00    22-Apr-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.DigitalSignage
{
    class RemoteDSLookupUseCases : RemoteUseCases, IDSLookupUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string DSLOOKUP_URL = "api/DigitalSignage/DSLookUps";
        public RemoteDSLookupUseCases(ExecutionContext executionContext)
           : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<DSLookupDTO>> GetDSLookups(List<KeyValuePair<DSLookupDTO.SearchByParameters, string>> searchParameters,
                                                                                bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)

        {
            log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildRecords".ToString(), loadChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("activeChildRecords".ToString(), activeChildRecords.ToString()));
            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                List<DSLookupDTO> result = await Get<List<DSLookupDTO>>(DSLOOKUP_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<DSLookupDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<DSLookupDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case DSLookupDTO.SearchByParameters.DSLOOKUP_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("dSLookupID".ToString(), searchParameter.Value));
                        }
                        break;
                    case DSLookupDTO.SearchByParameters.DSLOOKUP_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("dSLookupName".ToString(), searchParameter.Value));
                        }
                        break;
                    case DSLookupDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;  
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveDSLookups(List<DSLookupDTO> lookupDTOList)
        {
            log.LogMethodEntry(lookupDTOList);
            try
            {
                string responseString = await Post<string>(DSLOOKUP_URL, lookupDTOList);
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
