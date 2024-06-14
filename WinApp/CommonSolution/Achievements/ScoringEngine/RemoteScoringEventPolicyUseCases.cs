/********************************************************************************************
* Project Name - Promotions
* Description  - RemoteScoringEventPolicyUseCases class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.120.00    11-May-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Achievements.ScoringEngine;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Achievements
{
    class RemoteScoringEventPolicyUseCases:RemoteUseCases,IScoringEventPolicyUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string SCORING_EVENT_POLICY_URL = "api/Game/ScoringEventPolicy";
        public RemoteScoringEventPolicyUseCases(ExecutionContext executionContext)
           : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<ScoringEventPolicyDTO>> GetScoringEventPolicies(List<KeyValuePair<ScoringEventPolicyDTO.SearchByScoringEventPolicyParameters, string>> searchParameters,
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
                List<ScoringEventPolicyDTO> result = await Get<List<ScoringEventPolicyDTO>>(SCORING_EVENT_POLICY_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<ScoringEventPolicyDTO.SearchByScoringEventPolicyParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<ScoringEventPolicyDTO.SearchByScoringEventPolicyParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case ScoringEventPolicyDTO.SearchByScoringEventPolicyParameters.SCORING_EVENT_POLICY_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("scoringEventPolicyId".ToString(), searchParameter.Value));
                        }
                        break;
                    case ScoringEventPolicyDTO.SearchByScoringEventPolicyParameters.ACHIEVEMENT_CLASS_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("achievementClassId".ToString(), searchParameter.Value));
                        }
                        break;

                    case ScoringEventPolicyDTO.SearchByScoringEventPolicyParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;                
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveScoringEventPolicies(List<ScoringEventPolicyDTO> scoringEventPolicyDTOList)
        {
            log.LogMethodEntry(scoringEventPolicyDTOList);
            try
            {
                string responseString = await Post<string>(SCORING_EVENT_POLICY_URL, scoringEventPolicyDTOList);
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
