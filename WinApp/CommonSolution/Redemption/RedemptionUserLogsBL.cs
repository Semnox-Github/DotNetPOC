/********************************************************************************************
 * Project Name - Redemption
 * Description  - Buisness logic for RedemptionUserLogs
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        29-Jul-2019   Archana                 Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Redemption
{
    /// <summary>
    /// Business logic for RedemptionUserLogs class.
    /// </summary>
    public class RedemptionUserLogsBL
    {
        private RedemptionUserLogsDTO redemptionUserLogsDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of RedemptionUserLogsBL class
        /// </summary>
        public RedemptionUserLogsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates RedemptionUserLogsBL object using the redemptionUserLogsDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="redemptionUserLogsDTO">RedemptionUserLogsDTO object</param>
        public RedemptionUserLogsBL(ExecutionContext executionContext, RedemptionUserLogsDTO redemptionUserLogsDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, RedemptionUserLogsDTO);
            this.redemptionUserLogsDTO = redemptionUserLogsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        ///Constructor will fetch the RedemptionUserLogs DTO based on the RedemptionUserLog id passed 
        /// </summary>
        /// <param name="redemptionUserLogId">redemptionUserLogs id</param>
        public RedemptionUserLogsBL(ExecutionContext executionContext, int redemptionUserLogId)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext,redemptionUserLogId);
            RedemptionUserLogsDataHandler redemptionUserLogsDataHandler = new RedemptionUserLogsDataHandler(null);
            redemptionUserLogsDTO = redemptionUserLogsDataHandler.GetRedemptionUserLogsDTO(redemptionUserLogId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the RedemptionUserLogsDTO
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (redemptionUserLogsDTO.IsChanged == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            RedemptionUserLogsDataHandler redemptionUserLogsDataHandler = new RedemptionUserLogsDataHandler(sqlTransaction);
            
            if (RedemptionUserLogsDTO.RedemptionLogId < 0)
            {
                log.LogVariableState("RedemptionUserLogsDTO", RedemptionUserLogsDTO);
                redemptionUserLogsDTO = redemptionUserLogsDataHandler.Insert(redemptionUserLogsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                RedemptionUserLogsDTO.AcceptChanges();
            }
            else if (RedemptionUserLogsDTO.IsChanged)
            {
                log.LogVariableState("RedemptionUserLogsDTO", RedemptionUserLogsDTO);
                redemptionUserLogsDTO = redemptionUserLogsDataHandler.Update(redemptionUserLogsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                redemptionUserLogsDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public RedemptionUserLogsDTO RedemptionUserLogsDTO
        {
            get
            {
                return redemptionUserLogsDTO;
            }
        }

    }

    /// <summary>
    /// Manages the list of RedemptionLogsBL
    /// </summary>
    public class ReedemptionLogsListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<RedemptionUserLogsDTO> redemptionUserLogsDTOList = new List<RedemptionUserLogsDTO>();
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public ReedemptionLogsListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="RedemptionUserLogsDTOList"></param>
        public ReedemptionLogsListBL(ExecutionContext executionContext,
                                                List<RedemptionUserLogsDTO> redemptionUserLogsDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, redemptionUserLogsDTOList);
            this.redemptionUserLogsDTOList = redemptionUserLogsDTOList;
            log.LogMethodExit();
        }

        
        /// <summary>
        ///  Returns the Get the RedemptionUserLogsDTO list
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns>RedemptionUserLogsDTOList</returns>
        public List<RedemptionUserLogsDTO> GetRedemptionUserLogsDTOList(List<KeyValuePair<RedemptionUserLogsDTO.SearchByParameters, string>> searchParameters,
                                                         SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<RedemptionUserLogsDTO> redemptionUserLogsDTOList = new List<RedemptionUserLogsDTO>();
            RedemptionUserLogsDataHandler redemptionUserLogsDataHandler = new RedemptionUserLogsDataHandler(sqlTransaction);
            redemptionUserLogsDTOList = redemptionUserLogsDataHandler.GetAllRedemptionUserLogsDTOList(searchParameters);
            log.LogMethodExit(redemptionUserLogsDTOList);
            return redemptionUserLogsDTOList;
        }

        /// <summary>
        /// Saves the  List of RedemptionUserLogsDTO objects
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (redemptionUserLogsDTOList == null ||
                redemptionUserLogsDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < redemptionUserLogsDTOList.Count; i++)
            {
                RedemptionUserLogsDTO redemptionUserLogsDTO = redemptionUserLogsDTOList[i];
                try
                {
                    RedemptionUserLogsBL redemptionUserLogsBL = new RedemptionUserLogsBL(executionContext, redemptionUserLogsDTO);
                    redemptionUserLogsBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while saving RedemptionUserLogsDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("RedemptionUserLogsDTO", redemptionUserLogsDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
