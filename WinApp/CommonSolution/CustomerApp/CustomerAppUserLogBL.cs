/********************************************************************************************
 * Project Name - Customer App User Log                                                                     
 * Description  - BL for Customer App user log
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60         08-May-2019   Nitin Pai            Created for Guest app
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.CustomerApp
{
    public class CustomerAppUserLogBL
    {

        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        CustomerAppUserLogDTO userLogDTO;
        Utilities utilities = new Utilities();

        public CustomerAppUserLogBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            userLogDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public CustomerAppUserLogBL(ExecutionContext executionContext, CustomerAppUserLogDTO userLogDTO)
            : this(executionContext)
        {
            log.LogMethodEntry();
            this.userLogDTO = userLogDTO;
            log.LogMethodEntry();
        }

        public CustomerAppUserLogBL(ExecutionContext executionContext, int CustomerId, String controller, String action, String variableState, String message, String uuid)
            : this(executionContext)
        {
            log.LogMethodEntry();
            userLogDTO = new CustomerAppUserLogDTO();
            userLogDTO.CustomerId = CustomerId;
            userLogDTO.Controller = controller;
            userLogDTO.Action = action;
            userLogDTO.VariableState = variableState;
            userLogDTO.Message = message;
            userLogDTO.UUID = uuid;
            log.LogMethodEntry();
        }

        public void Save()
        {
            log.LogMethodEntry();
            int id;
            CustomerAppUserLogDataHandler userLogDataHandler = new CustomerAppUserLogDataHandler();
            if (userLogDTO.Id < 0)
            {
                id = userLogDataHandler.InsertUserLog(userLogDTO, executionContext.GetUserId().ToString(), executionContext.GetSiteId());
                userLogDTO.Id = id;
                userLogDTO.AcceptChanges();
            }
            else
            {
                userLogDataHandler.UpdateUserLog(userLogDTO, executionContext.GetUserId().ToString(), executionContext.GetSiteId());
                userLogDTO.AcceptChanges();
            }
        }
    }
}
