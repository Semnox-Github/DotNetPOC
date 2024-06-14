/********************************************************************************************
* Project Name - Utilities
* Description  - Base class of Local use cases.
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.110.0        12-Nov-2019   Lakshminarayana           Created 
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Core.Utilities
{
    /// <summary>
    /// Base class of all the use cases
    /// </summary>
    public abstract class LocalUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected ExecutionContext executionContext;
        protected string requestGuid;
        private ApplicationRequestLogDTO applicationRequestLogDTO;
        public LocalUseCases()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        public LocalUseCases(string requestGuid)
        {
            log.LogMethodEntry();
            this.requestGuid = requestGuid;
            log.LogMethodExit();
        }

        public LocalUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public LocalUseCases(ExecutionContext executionContext, string requestGuid)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.requestGuid = requestGuid;
            log.LogMethodExit();
        }

        protected bool IsDuplicateRequest()
        {
            log.LogMethodEntry();
            bool result = false;
            if(string.IsNullOrWhiteSpace(requestGuid))
            {
                log.LogMethodExit(result, "requestGuid is empty");
                return result;
            }
            ApplicationRequestLogDTO applicationRequestLogDTO = GetApplicationRequestLogDTO();
            if (applicationRequestLogDTO != null)
            {
                result = true;
            }
            log.LogMethodExit(result);
            return result;
        }

        protected string GetEntityGuid()
        {
            log.LogMethodEntry();
            string result = string.Empty;
            ApplicationRequestLogDTO applicationRequestLogDTO = GetApplicationRequestLogDTO();
            if (applicationRequestLogDTO != null && 
                applicationRequestLogDTO.ApplicationRequestLogDetailDTOList != null && 
                applicationRequestLogDTO.ApplicationRequestLogDetailDTOList.Any())
            {
                result = applicationRequestLogDTO.ApplicationRequestLogDetailDTOList.First().EntityGuid;
            }
            log.LogMethodExit(result);
            return result;
        }

        protected IEnumerable<string> GetEntityGuidList()
        {
            log.LogMethodEntry();
            IEnumerable<string> result;
            ApplicationRequestLogDTO applicationRequestLogDTO = GetApplicationRequestLogDTO();
            if (applicationRequestLogDTO != null &&
                applicationRequestLogDTO.ApplicationRequestLogDetailDTOList != null &&
                applicationRequestLogDTO.ApplicationRequestLogDetailDTOList.Any())
            {
                result = applicationRequestLogDTO.ApplicationRequestLogDetailDTOList.Select(x => x.EntityGuid);
            }
            else
            {
                result = new List<string>();
            }
            log.LogMethodExit(result);
            return result;
        }

        protected ApplicationRequestLogDTO GetApplicationRequestLogDTO()
        {
            log.LogMethodEntry();
            if(applicationRequestLogDTO != null)
            {
                log.LogMethodExit(applicationRequestLogDTO);
                return applicationRequestLogDTO;
            }
            ApplicationRequestLogListBL applicationRequestLogListBL = new ApplicationRequestLogListBL();
            applicationRequestLogDTO = applicationRequestLogListBL.GetApplicationRequestLogDTO(requestGuid);
            log.LogMethodExit(applicationRequestLogDTO);
            return applicationRequestLogDTO;
        }

        protected void CreateApplicationRequestLog(ApplicationRequestLogDTO applicationRequestLogDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(applicationRequestLogDTO);
            if (string.IsNullOrWhiteSpace(requestGuid))
            {
                log.LogMethodExit(null, "requestGuid is empty");
                return;
            }
            try
            {
                ApplicationRequestLogBL applicationRequestLogBL = new ApplicationRequestLogBL(executionContext, applicationRequestLogDTO);
                applicationRequestLogBL.Save(sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error("Error Occured while creating application request log", ex);
            }
            log.LogMethodExit();
        }

        protected void CreateApplicationRequestLog(string module, string usecase, string entityGuid, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(module, usecase, entityGuid);
            if (string.IsNullOrWhiteSpace(requestGuid))
            {
                log.LogMethodExit(null, "requestGuid is empty");
                return;
            }
            ApplicationRequestLogDTO applicationRequestLogDTO = new ApplicationRequestLogDTO(-1, requestGuid, module, usecase, ServerDateTime.Now, executionContext != null ? executionContext.UserId : string.Empty, true);
            applicationRequestLogDTO.ApplicationRequestLogDetailDTOList.Add(new ApplicationRequestLogDetailDTO(-1, -1, entityGuid, true));
            CreateApplicationRequestLog(applicationRequestLogDTO, sqlTransaction);
            log.LogMethodExit();
        }

        protected void CreateApplicationRequestLog(string module, string usecase, IEnumerable<string> entityGuidList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(module, usecase, entityGuidList);
            if (string.IsNullOrWhiteSpace(requestGuid))
            {
                log.LogMethodExit(null, "requestGuid is empty");
                return;
            }
            ApplicationRequestLogDTO applicationRequestLogDTO = new ApplicationRequestLogDTO(-1, requestGuid, module, usecase, ServerDateTime.Now, executionContext != null ? executionContext.UserId : string.Empty, true);
            foreach (var entityGuid in entityGuidList)
            {
                applicationRequestLogDTO.ApplicationRequestLogDetailDTOList.Add(new ApplicationRequestLogDetailDTO(-1, -1, entityGuid, true));
            }
            CreateApplicationRequestLog(applicationRequestLogDTO, sqlTransaction);
            log.LogMethodExit();
        }


    }
}
