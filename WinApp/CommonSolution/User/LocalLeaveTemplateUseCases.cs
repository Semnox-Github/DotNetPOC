/********************************************************************************************
 * Project Name - User
 * Description  - LocalLeaveTemplateUseCases class 
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
    class LocalLeaveTemplateUseCases : ILeaveTemplateUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalLeaveTemplateUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<LeaveTemplateDTO>> GetLeaveTemplate(List<KeyValuePair<LeaveTemplateDTO.SearchByParameters, string>>
                          searchParameters, SqlTransaction sqlTransaction = null
                         )
        {
            return await Task<List<LeaveTemplateDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                LeaveTemplateListBL leaveTemplatesListBL = new LeaveTemplateListBL(executionContext);
                List<LeaveTemplateDTO> leaveTemplateDTOList = leaveTemplatesListBL.GetAllLeaveTemplateList(searchParameters, sqlTransaction);

                log.LogMethodExit(leaveTemplateDTOList);
                return leaveTemplateDTOList;
            });
        }

        //public async Task<int> GetLeaveTemplateCount(List<KeyValuePair<LeaveTemplateDTO.SearchByParameters, string>>
        //                                              searchParameters, SqlTransaction sqlTransaction = null
        //                     )
        //{
        //    return await Task<int>.Factory.StartNew(() =>
        //    {
        //        log.LogMethodEntry(searchParameters);

        //        LeaveTemplateListBL LeaveTemplatesListBL = new LeaveTemplateListBL(executionContext);
        //        int count = LeaveTemplatesListBL.GetLeaveTemplateCount(searchParameters, sqlTransaction);

        //        log.LogMethodExit(count);
        //        return count;
        //    });
        //}

        public async Task<List<LeaveTemplateDTO>> SaveLeaveTemplate(List<LeaveTemplateDTO> leaveTemplateDTOList)
        {
            return await Task<List<LeaveTemplateDTO>>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction transaction = new ParafaitDBTransaction())
                {
                    transaction.BeginTransaction();
                    LeaveTemplateListBL leaveTemplateList = new LeaveTemplateListBL(executionContext, leaveTemplateDTOList);
                    List<LeaveTemplateDTO> result = leaveTemplateList.SaveUpdateLeaveTemplate();
                    transaction.EndTransaction();
                    return result;
                }
            });
        }

        public async Task<string> DeleteLeaveTemplate(List<LeaveTemplateDTO> leaveTemplateDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(leaveTemplateDTOList);
                    LeaveTemplateListBL leaveTemplatesList = new LeaveTemplateListBL(executionContext, leaveTemplateDTOList);
                    leaveTemplatesList.Delete();
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
