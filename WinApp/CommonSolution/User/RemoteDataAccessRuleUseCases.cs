/********************************************************************************************
 * Project Name - User
 * Description  - RemoteDataAccessRuleUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version      Date              Modified By             Remarks          
 *********************************************************************************************
 2.120.0       31-Mar-2021       Prajwal S               Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    class RemoteDataAccessRuleUseCases : RemoteUseCases, IDataAccessRuleUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string DataAccessRule_URL = "api/HR/DataAccessRule";
        private const string DataAccessRule_COUNT_URL = "api/HR/DataAccessRuleCount";
        private const string MASK_UI_FIELDS_URL = "api/HR/MaskUIFields";

        public RemoteDataAccessRuleUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<DataAccessRuleDTO>> GetDataAccessRule(List<KeyValuePair<DataAccessRuleDTO.SearchByDataAccessRuleParameters, string>>
                          parameters, bool loadChildRecord = false, bool loadActiveChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildRecord".ToString(), loadChildRecord.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("loadActiveChildRecords".ToString(), loadActiveChildRecords.ToString()));
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                List<DataAccessRuleDTO> result = await Get<List<DataAccessRuleDTO>>(DataAccessRule_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<DataAccessRuleDTO.SearchByDataAccessRuleParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<DataAccessRuleDTO.SearchByDataAccessRuleParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case DataAccessRuleDTO.SearchByDataAccessRuleParameters.ACTIVE_FLAG:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case DataAccessRuleDTO.SearchByDataAccessRuleParameters.SITE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("siteId".ToString(), searchParameter.Value));
                        }
                        break;
                    case DataAccessRuleDTO.SearchByDataAccessRuleParameters.NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("name".ToString(), searchParameter.Value));
                        }
                        break;
                    case DataAccessRuleDTO.SearchByDataAccessRuleParameters.DATA_ACCESS_RULE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("dataAccessRuleId".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<List<DataAccessRuleDTO>> SaveDataAccessRule(List<DataAccessRuleDTO> dataAccessRuleDTOList)
        {
            log.LogMethodEntry(dataAccessRuleDTOList);
            try
            {
                List<DataAccessRuleDTO> responseString = await Post<List<DataAccessRuleDTO>>(DataAccessRule_URL, dataAccessRuleDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }


        public async Task<int> GetDataAccessRuleCount(List<KeyValuePair<DataAccessRuleDTO.SearchByDataAccessRuleParameters, string>>
                         parameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                int result = await Get<int>(DataAccessRule_COUNT_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<List<EntityExclusionDetailDTO>> GetMaskUIFields(string uiName, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(uiName, sqlTransaction);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("uiName".ToString(), uiName.ToString()));
            List<EntityExclusionDetailDTO> result = await Get<List<EntityExclusionDetailDTO>>(MASK_UI_FIELDS_URL, searchParameterList);
            log.LogMethodExit(result);
            return result;
        }
    }
}