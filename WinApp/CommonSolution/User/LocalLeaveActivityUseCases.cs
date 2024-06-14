/********************************************************************************************
 * Project Name - User
 * Description  - LocalLeaveActivityUseCases class 
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    public class LocalLeaveActivityUseCases :ILeaveActivityUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalLeaveActivityUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<LeaveActivityDTO> GetLeaveActivity(int userId )
        {
            return await Task<LeaveActivityDTO>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(userId);

                LeaveActivityListBL leaveActivityListBL = new LeaveActivityListBL(executionContext);
                LeaveActivityDTO leaveActivityDTO = leaveActivityListBL.GetLeaveActivities(userId);

                log.LogMethodExit(leaveActivityDTO);
                return leaveActivityDTO;
            });
        }
    }
}
