/********************************************************************************************
 * Project Name - User
 * Description  - LocalDataAccessRuleUseCases class 
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
    class LocalDataAccessRuleUseCases : IDataAccessRuleUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalDataAccessRuleUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<DataAccessRuleDTO>> GetDataAccessRule(List<KeyValuePair<DataAccessRuleDTO.SearchByDataAccessRuleParameters, string>>
                          searchParameters, bool loadChildRecord = false, bool loadActiveChildRecords = false, SqlTransaction sqlTransaction = null
                         )
        {
            return await Task<List<DataAccessRuleDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                DataAccessRuleList dataAccessRulesListBL = new DataAccessRuleList(executionContext);
                List<DataAccessRuleDTO> dataAccessRuleDTOList = dataAccessRulesListBL.GetAllDataAccessRule(searchParameters, loadChildRecord, loadActiveChildRecords, sqlTransaction);

                log.LogMethodExit(dataAccessRuleDTOList);
                return dataAccessRuleDTOList;
            });
        }

        public async Task<int> GetDataAccessRuleCount(List<KeyValuePair<DataAccessRuleDTO.SearchByDataAccessRuleParameters, string>>
                                                      searchParameters, SqlTransaction sqlTransaction = null
                             )
        {
            return await Task<int>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                DataAccessRuleList dataAccessRulesListBL = new DataAccessRuleList(executionContext);
                int count = dataAccessRulesListBL.GetDataAccessRuleCount(searchParameters, sqlTransaction);

                log.LogMethodExit(count);
                return count;
            });
        }

        public async Task<List<DataAccessRuleDTO>> SaveDataAccessRule(List<DataAccessRuleDTO> DataAccessRuleDTOList)
        {
            return await Task<List<DataAccessRuleDTO>>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction transaction = new ParafaitDBTransaction())
                {
                    transaction.BeginTransaction();
                    DataAccessRuleList dataAccessRuleList = new DataAccessRuleList(executionContext, DataAccessRuleDTOList);
                    List<DataAccessRuleDTO> result = dataAccessRuleList.SaveUpdateDataAccessRuleList();
                    transaction.EndTransaction();
                    return result;
                }
            });
        }

        public async Task<List<EntityExclusionDetailDTO>> GetMaskUIFields(string uiName, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<EntityExclusionDetailDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(uiName, sqlTransaction);
                Users user = new Users(executionContext, executionContext.GetUserPKId());
                UserRoles userRoles = new UserRoles(executionContext, user.UserDTO.RoleId);
                List<EntityExclusionDetailDTO> entityExclusionDetailDTOList = userRoles.GetUIFieldsToHide(uiName, sqlTransaction);
                log.LogMethodExit(entityExclusionDetailDTOList);
                return entityExclusionDetailDTOList;
            });
        }
    }
}
