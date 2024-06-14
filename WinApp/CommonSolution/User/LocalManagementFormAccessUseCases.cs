/********************************************************************************************
 * Project Name - User
 * Description  - LocalManagementFormAccessUseCases class 
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
    class LocalManagementFormAccessUseCases : IManagementFormAccessUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalManagementFormAccessUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<ManagementFormAccessDTO>> GetManagementFormAccessDTOList(List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>>
                          searchParameters, SqlTransaction sqlTransaction = null
                         )
        {
            return await Task<List<ManagementFormAccessDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                ManagementFormAccessListBL managementFormAccesssListBL = new ManagementFormAccessListBL(executionContext);
                List<ManagementFormAccessDTO> managementFormAccessDTOList = managementFormAccesssListBL.GetManagementFormAccessDTOList(searchParameters, sqlTransaction);

                log.LogMethodExit(managementFormAccessDTOList);
                return managementFormAccessDTOList;
            });
        }

        //public async Task<int> GetManagementFormAccessCount(List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>>
        //                                              searchParameters, SqlTransaction sqlTransaction = null
        //                     )
        //{
        //    return await Task<int>.Factory.StartNew(() =>
        //    {
        //        log.LogMethodEntry(searchParameters);

        //        ManagementFormAccessListBL ManagementFormAccesssListBL = new ManagementFormAccessListBL(executionContext);
        //        int count = ManagementFormAccesssListBL.GetManagementFormAccessCount(searchParameters, sqlTransaction);

        //        log.LogMethodExit(count);
        //        return count;
        //    });
        //}

        public async Task<List<ManagementFormAccessDTO>> SaveManagementFormAccess(List<ManagementFormAccessDTO> managementFormAccessDTOList)
        {
            return await Task<List<ManagementFormAccessDTO>>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction transaction = new ParafaitDBTransaction())
                {
                    transaction.BeginTransaction();
                    ManagementFormAccessListBL managementFormAccessList = new ManagementFormAccessListBL(executionContext, managementFormAccessDTOList);
                    List<ManagementFormAccessDTO> result = managementFormAccessList.SaveUpdateManagementFormAccessList();
                    transaction.EndTransaction();
                    return result;
                }
            });
        }
    }
}
