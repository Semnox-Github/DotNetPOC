/********************************************************************************************
 * Project Name - User
 * Description  - LocalAgentsUseCases class 
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
    class LocalAgentsUseCases : IAgentsUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalAgentsUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<AgentsDTO>> GetAgents(List<KeyValuePair<AgentsDTO.SearchByParameters, string>>
                          searchParameters,  SqlTransaction sqlTransaction = null
                         )
        {
            return await Task<List<AgentsDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                AgentsList agentsListBL = new AgentsList(executionContext);
                List<AgentsDTO> AgentsDTOList = agentsListBL.GetAllAgentsList(searchParameters, sqlTransaction);

                log.LogMethodExit(AgentsDTOList);
                return AgentsDTOList;
            });
        }

        public async Task<int> GetAgentsCount(List<KeyValuePair<AgentsDTO.SearchByParameters, string>>
                                                      searchParameters, SqlTransaction sqlTransaction = null
                             )
        {
            return await Task<int>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                AgentsList AgentsListBL = new AgentsList(executionContext);
                int count = AgentsListBL.GetAgentsCount(searchParameters, sqlTransaction);

                log.LogMethodExit(count);
                return count;
            });
        }

        public async Task<string> SaveAgents(List<AgentsDTO> agentsDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(agentsDTOList);
                    if (agentsDTOList == null)
                    {
                        throw new ValidationException("AgentsDTOList is Empty");
                    }

                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {

                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            AgentsList AgentsList = new AgentsList(executionContext, agentsDTOList);
                             AgentsList.Save(parafaitDBTrx.SQLTrx);
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

        public async Task<string> DeleteAgents(List<AgentsDTO> agentsDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(agentsDTOList);
                    AgentsList agentsList = new AgentsList(executionContext, agentsDTOList);
                    agentsList.Delete();
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
