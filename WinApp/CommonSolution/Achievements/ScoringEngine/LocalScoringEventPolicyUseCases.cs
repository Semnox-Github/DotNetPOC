/********************************************************************************************
* Project Name -Achievements
* Description  - LocalScoringEventPolicyUseCases class 
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
    public class LocalScoringEventPolicyUseCases : IScoringEventPolicyUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        public LocalScoringEventPolicyUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// GetScoringEventPoliciess
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="loadChildRecords"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public async Task<List<ScoringEventPolicyDTO>> GetScoringEventPolicies(List<KeyValuePair<ScoringEventPolicyDTO.SearchByScoringEventPolicyParameters, string>> searchParameters,
                                         bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<ScoringEventPolicyDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);

                ScoringEventPolicyListBL scoringEventPolicyListBL = new ScoringEventPolicyListBL(executionContext);
                List<ScoringEventPolicyDTO> scoringEventPolicyDTOList = scoringEventPolicyListBL.GetAllScoringEventPolicy(searchParameters, loadChildRecords, activeChildRecords, null);

                log.LogMethodExit(scoringEventPolicyDTOList);
                return scoringEventPolicyDTOList;
            });
        }
        /// <summary>
        /// SaveScoringEventPolicies
        /// </summary>
        /// <param name="scoringEventPolicyDTOList"></param>
        /// <returns></returns>

        public async Task<string> SaveScoringEventPolicies(List<ScoringEventPolicyDTO> scoringEventPolicyDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(scoringEventPolicyDTOList);
                    if (scoringEventPolicyDTOList == null)
                    {
                        throw new ValidationException("scoringEventPolicyDTOList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            ScoringEventPolicyListBL scoringEventPolicyListBL = new ScoringEventPolicyListBL(executionContext,scoringEventPolicyDTOList);
                            scoringEventPolicyListBL.Save(parafaitDBTrx.SQLTrx);
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
