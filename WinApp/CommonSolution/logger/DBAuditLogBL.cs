/********************************************************************************************
 * Project Name - Logger
 * Description  - Business logic class of DBAuditLog
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.3      02-Feb-2020   Girish Kundar           Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.logger
{
    public class DBAuditLogBL
    {
        private DBAuditLogDTO dbAuditLogDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
       
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="dbAuditLogDTO">Parameter of the type DBAuditLogDTO</param>
        public DBAuditLogBL( ExecutionContext executionContext, DBAuditLogDTO dbAuditLogDTO)
        {
            log.LogMethodEntry(dbAuditLogDTO, executionContext);
            this.dbAuditLogDTO = dbAuditLogDTO;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the DBAuditLogDTO
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            DBAuditLogDataHandler dbAuditLogDataHandler = new DBAuditLogDataHandler(sqlTransaction);
            int rowInserted =  dbAuditLogDataHandler.Insert(dbAuditLogDTO, executionContext.GetUserId(), executionContext.GetSiteId());
            dbAuditLogDTO.AcceptChanges();
            log.LogMethodExit(rowInserted);
        }
    }

    /// <summary>
    /// Manages the list of DBAuditLog
    /// </summary>
    public class DBAuditLogListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<DBAuditLogDTO> dbAuditLogDTOList;
        private ExecutionContext executionContext;


        public DBAuditLogListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(dbAuditLogDTOList, executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="monitorLogDTOList"></param>
        /// <param name="executionContext"></param>
        public DBAuditLogListBL( ExecutionContext executionContext, List<DBAuditLogDTO> dbAuditLogDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(dbAuditLogDTOList, executionContext);
            this.dbAuditLogDTOList = dbAuditLogDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the DBAuditLogDTO list
        /// </summary>
        public List<DBAuditLogDTO> GetDBAuditLogDTOList(List<KeyValuePair<DBAuditLogDTO.SearchByParameters, string>> searchParameters ,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            DBAuditLogDataHandler dbAuditLogDataHandler = new DBAuditLogDataHandler(sqlTransaction);
            log.LogMethodExit();
            List<DBAuditLogDTO> dBAuditLogDTOList = dbAuditLogDataHandler.GetDBAuditLogDTOList(searchParameters, sqlTransaction);
            return dBAuditLogDTOList;
        }

        /// <summary>
        /// Save or update DBAuditLogDTO  for Web Management Studio
        /// </summary>
        public void Save(SqlTransaction sqlTransaction =null)
        {
            try
            {
                log.LogMethodEntry();
                if (dbAuditLogDTOList != null)
                {
                    foreach (DBAuditLogDTO dbAuditLogDTO in dbAuditLogDTOList)
                    {
                        DBAuditLogBL dbAuditLogBL = new DBAuditLogBL( executionContext, dbAuditLogDTO);
                        dbAuditLogBL.Save(sqlTransaction);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
            log.LogMethodExit();
        }
    }
}
