/********************************************************************************************
* Project Name - Achievements
* Description  - Specification of the IScoringEventPolicyUseCases
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   11-May-2021   Roshan Devadiga          Created
********************************************************************************************/
using Semnox.Parafait.Achievements.ScoringEngine;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Achievements
{
    /// <summary>
    /// IScoringEventPolicyUseCases
    /// </summary>
    public interface IScoringEventPolicyUseCases
    {
        Task<List<ScoringEventPolicyDTO>> GetScoringEventPolicies(List<KeyValuePair<ScoringEventPolicyDTO.SearchByScoringEventPolicyParameters, string>> searchParameters,
                                         bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null);
        Task<string> SaveScoringEventPolicies(List<ScoringEventPolicyDTO> scoringEventPolicyDTOList);

    }
}
