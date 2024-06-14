/********************************************************************************************
 * Project Name - User
 * Description  - LocalAgentGroupsUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.0      01-Apr-2021      Prajwal S                 Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    class LocalAgentGroupsUseCases : IAgentGroupsUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalAgentGroupsUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<AgentGroupsDTO>> GetAgentGroups(List<KeyValuePair<AgentGroupsDTO.SearchByParameters, string>>
                          searchParameters, bool loadChildRecords, bool activeChildRecords, SqlTransaction sqlTransaction = null
                         )
        {
            return await Task<List<AgentGroupsDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                AgentGroupsList agentGroupsListBL = new AgentGroupsList(executionContext);
                List<AgentGroupsDTO> agentGroupsDTOList = agentGroupsListBL.GetAllAgentGroupsList(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);

                log.LogMethodExit(agentGroupsDTOList);
                return agentGroupsDTOList;
            });
        }

        public async Task<int> GetAgentGroupsCount(List<KeyValuePair<AgentGroupsDTO.SearchByParameters, string>>
                                                      searchParameters, SqlTransaction sqlTransaction = null
                             )
        {
            return await Task<int>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                AgentGroupsList agentGroupsListBL = new AgentGroupsList(executionContext);
                int count = agentGroupsListBL.GetAgentGroupsCount(searchParameters, sqlTransaction);

                log.LogMethodExit(count);
                return count;
            });
        }

        public async Task<string> SaveAgentGroups(List<AgentGroupsDTO> AgentGroupsDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(AgentGroupsDTOList);
                    if (AgentGroupsDTOList == null)
                    {
                        throw new ValidationException("AgentGroupsDTOList is Empty");
                    }

                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {

                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            AgentGroupsList agentGroupsList = new AgentGroupsList(executionContext, AgentGroupsDTOList);
                              agentGroupsList.Save(parafaitDBTrx.SQLTrx);
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
                            throw new Exception(ex.Message, ex);
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

        public async Task<string> DeleteAgentGroups(List<AgentGroupsDTO> agentGroupsDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(agentGroupsDTOList);
                    AgentGroupsList agentGroupsList = new AgentGroupsList(executionContext, agentGroupsDTOList);
                    agentGroupsList.Delete();
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
